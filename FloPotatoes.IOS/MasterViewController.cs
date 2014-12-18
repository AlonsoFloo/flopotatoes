using System;
using System.Drawing;
using System.Collections.Generic;

using Foundation;
using UIKit;
using FloPotatoes;
using System.Threading.Tasks;
using Xamarin;
using ObjCRuntime;

namespace FloPotatoes.IOS
{
	public partial class MasterViewController : UITableViewController
	{
		DataSource dataSource;

		public MasterViewController (IntPtr handle) : base (handle)
		{
			#if DEBUG
			Xamarin.Calabash.Start();
			#endif
			Title = "Movie List";
			PictureManager.SaveLocation = System.IO.Directory.GetParent (Environment.GetFolderPath (Environment.SpecialFolder.Personal)).ToString () + "/tmp";
		}

		async void GetData()
		{
			var handle = Insights.TrackTime("TimeLoadMovieList");
			handle.Start();
			dataSource.movies.Add(await PotatoesManager.Instance.GetMoviesOpening ());
			dataSource.movies.Add(await PotatoesManager.Instance.GetMoviesBoxOffice ());
			dataSource.movies.Add(await PotatoesManager.Instance.GetMoviesTheater ());
			handle.Stop();

			TableView.ReloadData ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TableView.Source = dataSource = new DataSource (this);

			TableView.SeparatorColor = UIColor.Gray;
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
			TableView.RowHeight = MovieListCell.MovieCellRowHeight;
			RefreshControl = new UIRefreshControl();
			RefreshControl.ValueChanged += HandleValueChanged;

			GetData ();
		}

		async void HandleValueChanged (object sender, EventArgs e)
		{
			var handle = Insights.TrackTime("TimeLoadMovieList");
			handle.Start();
			dataSource.movies.Add(await PotatoesManager.Instance.GetMoviesOpening ());
			dataSource.movies.Add(await PotatoesManager.Instance.GetMoviesBoxOffice ());
			dataSource.movies.Add(await PotatoesManager.Instance.GetMoviesTheater ());
			handle.Stop();

			TableView.ReloadData ();
			RefreshControl.EndRefreshing ();
		}

		class DataSource : UITableViewSource
		{
			public List<List<Movie>> movies { get; set; }
			readonly MasterViewController controller;

			public DataSource (MasterViewController controller)
			{
				this.controller = controller;
				movies = new List<List<Movie>>();
			}

			// Customize the number of sections in the table view.
			public override nint NumberOfSections (UITableView tableView)
			{
				return movies != null && movies.Count > 0 ? movies.Count : 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				if (movies != null && movies.Count > 0) {
					List<Movie> mv = movies [(int) section];
					return mv.Count;
				}
				return 1;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (movies == null || movies.Count < 1)
				{
					UITableViewCell cellLoading = new UITableViewCell ();
					UIActivityIndicatorView indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
					indicator.StartAnimating ();
					indicator.Frame = cellLoading.Frame;
					cellLoading.AddSubview (indicator);
					return cellLoading;
				}

				var cell = tableView.DequeueReusableCell (MovieListCell.CellId) as MovieListCell ?? new MovieListCell ();
				cell.Movie = movies [indexPath.Section] [indexPath.Row];
				cell.AccessibilityIdentifier = "section" + indexPath.Section.ToString () + "-row" + indexPath.Row.ToString ();
				return cell;
			}

			public override UIView GetViewForHeader (UITableView tableView, nint section)
			{
				if (movies == null || movies.Count < 1) {
					return null;
				}

				UIView myView = new UIView {
					BackgroundColor = UIColor.Yellow,
				};
				UILabel headerLabel = new UILabel {
					TextColor = UIColor.Black,
					TextAlignment = UITextAlignment.Left,
					Font = UIFont.FromName ("HelveticaNeue-Light", 10),
				};
				headerLabel.Frame = new RectangleF (5, 5, 200, 12);
				headerLabel.Text = TitleForHeader((int) section);
				myView.AddSubviews(headerLabel);
				return myView;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				controller.PerformSegue("showDetail", indexPath);
				tableView.DeselectRow (indexPath, true);
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return false;
			}

			public string TitleForHeader (int section)
			{
				string title = "";
				switch (section)
				{
				case 0:
					title = "OPENING THIS WEEK";
					break;
				case 1:
					title = "TOP BOX OFFICE";
					break;
				case 2:
					title = "ALSO IN THEATERS";
					break;
				default:
					throw new Exception("Wrong section number"); // should not occur
				}
				return title;
			}
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail") {
				var indexPath = TableView.IndexPathForSelectedRow;
				Movie item = dataSource.movies [indexPath.Section] [indexPath.Row];
				Insights.Track("MovieViewed", item.GetData());

				((DetailViewController)segue.DestinationViewController).SetMovie(item);
			}
		}
	}
}

