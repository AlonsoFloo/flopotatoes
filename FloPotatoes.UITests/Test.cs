using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using Xamarin.UITest.iOS;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace FloPotatoes.UITests
{
	[TestFixture ()]
	public class Test
	{

		/// <summary>
		/// This variable determine if the tests will run on iOS or Android.
		/// </summary>
		public static readonly bool LocalTestsUsingiOS = true;
		/// <summary>
		/// In some cases UITest will not be able to resolve the path to the Android SDK. 
		/// Set this to your local Android SDK path.
		/// </summary>
		public static readonly string PathToAndroidSdk = "/Users/WuzUrDaddy/Library/Developer/Xamarin/android-sdk-macosx";

		public string PathToIPA { get; private set; }

		public string PathToAPK { get; private set; }

		QueriesInterface _queries;

		IApp _app;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			if (TestEnvironment.IsTestCloud)
			{
				PathToAPK = String.Empty;
				PathToIPA = String.Empty;
			}
			else
			{
				string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
				FileInfo fi = new FileInfo(currentFile);
				string dir = fi.Directory.Parent.Parent.Parent.FullName;

				PathToIPA = Path.Combine(dir, "FloPotatoes.IOS", "bin", "iPhoneSimulator", "Debug", "FloPotatoesIOS.app");
				PathToAPK = Path.Combine(dir, "FloPotatoes.Android", "bin", "Release", "FloPotatoes.Android.apk");
			}
		}

		[SetUp]
		public void SetUp()
		{
			if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudiOS))
			{
				_app = ConfigureApp.iOS.ApiKey("00a72a49a146ea2e2cd677258b139c99").StartApp();
				_queries = new IOSQueries();
			}
			else if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudAndroid))
			{
				_queries = new AndroidQueries();
				_app = ConfigureApp.Android.ApiKey("00a72a49a146ea2e2cd677258b139c99").StartApp();
			}
			else if (TestEnvironment.Platform.Equals(TestPlatform.Local))
			{
				if (LocalTestsUsingiOS)
				{
					_queries = new IOSQueries();

					_app = ConfigureApp.iOS
						.EnableLocalScreenshots()
						.ApiKey("00a72a49a146ea2e2cd677258b139c99")
						.AppBundle(PathToIPA)
						.StartApp();
				}
				else
				{
					if (string.IsNullOrWhiteSpace(PathToAndroidSdk))
					{
						return;
					}
					string androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
					if (string.IsNullOrWhiteSpace(androidHome))
					{
						Environment.SetEnvironmentVariable("ANDROID_HOME", PathToAndroidSdk);
					}
					_queries = new AndroidQueries();

					_app = ConfigureApp.Android
						.ApkFile(PathToAPK)
						.ApiKey("00a72a49a146ea2e2cd677258b139c99")
						.EnableLocalScreenshots()
						.StartApp();
				}
			}
			else
			{
				throw new NotImplementedException(String.Format("I don't know this platform {0}", TestEnvironment.Platform));
			}
		}

		[Test ()]
		public void Home_FirstBoxOfficeMovieIsFresh ()
		{
			_app.WaitForElement(_queries.OpeningHeader, "Opening header did not appear", TimeSpan.FromSeconds(10));
			_app.ScrollDown ();
			_app.ScrollDown ();

			AppResult[] results = _app.Query(_queries.BoxOfficeHeader);
			Assert.IsTrue(results.Length > 0, "The Box Office hearder is not displayed on the screen");

			results = _app.Query (_queries.FirstBoxOfficeRatingPic);
			_app.Screenshot("The first Box Office movie.");
			Assert.IsTrue(results.Length > 0, "The first Box Office movie is not fresh");
		}

		[Test ()]
		public void Home_GoToDetails ()
		{
			_app.WaitForElement(_queries.OpeningHeader, "Opening header did not appear", TimeSpan.FromSeconds(10));
			_app.Tap(_queries.MovieCell);

			_app.Screenshot("The movie details is displayed on the screen.");
			_app.WaitForElement(_queries.SynopsysText, "The synopsys text is not displayed on the screen", TimeSpan.FromSeconds(10));
			AppResult[] results = _app.Query (_queries.SynopsysText);
			Assert.IsTrue(results.Length > 0, "The synopsys text is not displayed on the screen");
		}

		[Test ()]
		public void Details_Load ()
		{
			_app.WaitForElement(_queries.OpeningHeader, "Opening header did not appear", TimeSpan.FromSeconds(10));
			_app.Tap(_queries.MovieCell);

			_app.WaitForElement(_queries.SynopsysText, "The synopsys text is not displayed on the screen", TimeSpan.FromSeconds(10));
			_app.Screenshot("The movie details is displayed on the screen.");
			AppResult[] results = _app.Query (_queries.SynopsysText);
			Assert.IsTrue(results.Length > 0, "The synopsys text is not displayed on the screen");
			var cell = results [0];
			Assert.IsTrue(cell.Text.Length > 0, "The synopsys text is not fill");
		}
	}
}

