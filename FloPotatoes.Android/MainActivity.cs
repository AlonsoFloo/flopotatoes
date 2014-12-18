using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Threading.Tasks;
using FloPotatoes;
using Android.Support.V4.Widget;
using Xamarin
;
using Android.Telephony;

namespace FloPotatoes.Android
{
	[Activity (Label = "FloPotatoes", MainLauncher = true, Icon = "@drawable/ic_launcher")]
	public class MainActivity : Activity
	{
		private SwipeRefreshLayout swipeLayout;
		private List<Movie> openingList;
		private List<Movie> boxOfficeList;
		private List<Movie> theaterList;
		private ListView list;
		private MovieAdapter adp;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Insights.Initialize("171ab89133056e09d9bc53dc25cfe80a7236ae48", ApplicationContext);
			TelephonyManager mTelephonyMgr = (TelephonyManager) GetSystemService(Context.TelephonyService);
			String imei = mTelephonyMgr.DeviceId;
			Insights.Identify(imei, new Dictionary<string, string>{
				{"SDK", Build.VERSION.SdkInt.ToString()}
			});

			PictureManager.SaveLocation = CacheDir.AbsolutePath;
			SetContentView (Resource.Layout.Splashscreen);

			Launch ();
			GetData ();
		}

		async private void Launch()
		{
			await Task.Delay(3000);
			SetContentView (Resource.Layout.Main);
			swipeLayout = FindViewById<SwipeRefreshLayout> (Resource.Id.swipe_container);
			swipeLayout.SetColorScheme(Resource.Color.Orange,
				Resource.Color.Blue,
				Resource.Color.Black,
				Resource.Color.Tomatoes);
			swipeLayout.Refresh += HandleRefresh;
			InitListView();
		}

		async void HandleRefresh (object sender, EventArgs e)
		{
			var handle = Insights.TrackTime("TimeLoadMovieList");
			handle.Start();
			openingList = await PotatoesManager.Instance.GetMoviesOpening ();
			boxOfficeList = await PotatoesManager.Instance.GetMoviesBoxOffice ();
			theaterList = await PotatoesManager.Instance.GetMoviesTheater ();
			handle.Stop();
			swipeLayout.Refreshing = false;
		}

		async void GetData()
		{
			var handle = Insights.TrackTime("TimeLoadMovieList");
			handle.Start();
			openingList = await PotatoesManager.Instance.GetMoviesOpening ();
			boxOfficeList = await PotatoesManager.Instance.GetMoviesBoxOffice ();
			theaterList = await PotatoesManager.Instance.GetMoviesTheater ();
			handle.Stop();
			if (list != null) {
				FillAdp ();
			}
		}

		private void InitListView() {
			list = FindViewById<ListView> (Resource.Id.listView);
			this.adp = new MovieAdapter(this);
			list.Adapter = adp;
			list.ItemClick += (sender, e) => { 
				MovieSelected(e.Position);
			};
			FillAdp ();
		}

		private void MovieSelected(int position) {
			if (!this.adp.IsSeparator(position)) {
				Movie m = (Movie) this.adp.GetMovie (position);
				Insights.Track("MovieViewed", m.GetData());
				Intent intent = new Intent(this, typeof(DetailsActivity));
				intent.PutExtra("movieId", m.Id);
				StartActivity(intent);
			}
		}


		private void FillAdp() {
			// fill
			Dictionary<string, List<Movie>> list = new Dictionary<string, List<Movie>>();
			if (this.openingList != null && this.openingList.Count > 0) {
				list.Add(GetString(Resource.String.List_Opening), this.openingList);
			}
			if (this.boxOfficeList != null && this.boxOfficeList.Count > 0) {
				list.Add(GetString(Resource.String.List_Box_Office), this.boxOfficeList);
			}
			if (this.theaterList != null && this.theaterList.Count > 0) {
				list.Add(GetString(Resource.String.List_Theater), this.theaterList);
			}
			this.adp.fillData(list);
			this.adp.NotifyDataSetChanged();
		}

		public void DownloadSucceed(int type)
		{
			swipeLayout.Refreshing = false;
		}

		public void DownloadFailed(int type)
		{
			swipeLayout.Refreshing = false;
		}
	}
}


