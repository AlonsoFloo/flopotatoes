using System;
using System.Drawing;
using System.Collections.Generic;

using Foundation;
using UIKit;
using FloPotatoes;
using Xamarin;

namespace FloPotatoes.IOS
{
	public partial class DetailViewController : UIViewController
	{
		Movie movie;
		CastDataSource castDataSource;
		ReviewsDataSource reviewsDataSource;

		public DetailViewController (IntPtr handle) : base (handle)
		{
		}

		public void SetMovie (Movie newMovie)
		{
			if (movie != newMovie) {
				movie = newMovie;

				// Update the view
				ConfigureView ();
				ConfigureReviews ();
			}
		}

		async void ConfigureView ()
		{
			if (IsViewLoaded && movie != null)
			{
				var handle = Insights.TrackTime("TimeLoadMovie", new Dictionary<string, string> {
					{ "MovieID", movie.Id.ToString() }
				});
				handle.Start ();
				movie = await PotatoesManager.Instance.GetFullMovieData (movie.Id);
				handle.Stop ();

				this.Title = movie.Title;

				criticsScoreLabel.Text = movie.Ratings.Critics_Score.ToString() + "%";
				audienceScoreLabel.Text = movie.Ratings.Audience_Score.ToString() + "%";
				string criticsRessources = movie.GetRatingIcon () + ".png";
				UIImage criticsImage = new UIImage(criticsRessources);
				criticsRatingView.Image = criticsImage;

				firstCastLabel.Text = string.Join (", ", movie.Abridged_Cast.ConvertAll(actor => actor.Name).ToArray(), 0, 2);
				dateLabel.Text = "In theaters "+ movie.Release_Dates.GetTheaterDateReadable ();
				subLabel.Text = movie.Mpaa_Rating + ", " + movie.GetRuntimeReadable ();

				synopsisLabel.Text = movie.Synopsis;
				synopsisLabel.AccessibilityIdentifier = "synopsisLabel";
				directorsLabel.Text = string.Join (", ", movie.Abridged_Directors.ConvertAll(director => director.Name).ToArray());
				mpaaLabel.Text = movie.Mpaa_Rating;
				timeLabel.Text = movie.GetRuntimeReadable ();
				genresLabels.Text = string.Join (", ", movie.Genres);
				theaterLabel.Text = movie.Release_Dates.GetTheaterDateReadable ();

				moreButton.TouchUpInside += delegate {
					OpenUrl(movie.Links.Alternate.AbsoluteUri);
				};

				tableViewCast.Source = castDataSource = new CastDataSource ();
				tableViewCast.SeparatorColor = UIColor.Gray;
				tableViewCast.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
				castDataSource.actor = movie.Abridged_Cast;
				tableViewCast.ReloadData ();

				// Download the picture after displaying all the data
				handle = Insights.TrackTime("TimeLoadMovie", new Dictionary<string, string> {
					{ "MovieID", movie.Id.ToString() }
				});
				handle.Start ();
				string path = await PictureManager.Download (movie.Posters.Original.AbsoluteUri);
				UIImage thumbnail = new UIImage(path);
				moviePicView.Image = thumbnail;
				handle.Stop ();
			}
		}

		async void ConfigureReviews ()
		{
			if (IsViewLoaded && movie != null)
			{
				var handle = Insights.TrackTime("TimeLoadMovie", new Dictionary<string, string> {
					{ "MovieID", movie.Id.ToString() }
				});
				handle.Start ();
				List<Review> reviewList = await PotatoesManager.Instance.GetReviews (movie.Id);
				handle.Stop ();
				tableViewReview.Source = reviewsDataSource = new ReviewsDataSource (this);
				tableViewReview.SeparatorColor = UIColor.Gray;
				tableViewReview.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
				reviewsDataSource.reviews = reviewList;
				tableViewReview.ReloadData ();
			}
		}

		public void OpenUrl (string url)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
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
			scrollView.ContentSize = new SizeF((float) View.Frame.Width, 1153f);

			// Update the view
			ConfigureView ();
			ConfigureReviews ();
		}

		class CastDataSource : UITableViewSource
		{
			public List<People> actor { get; set; }

			public CastDataSource ()
			{
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				if (actor != null && actor.Count > 0) {
					return actor.Count;
				}
				return 0;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell ("ActorTableCell");
				if (cell == null)
					cell = new UITableViewCell (UITableViewCellStyle.Default, "ActorTableCell");
				cell.TextLabel.Text = actor[indexPath.Row].Name;
				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				return false;
			}
		}

		class ReviewsDataSource : UITableViewSource
		{
			public List<Review> reviews { get; set; }
			readonly DetailViewController controller;

			public ReviewsDataSource (DetailViewController controller)
			{
				this.controller = controller;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				if (reviews != null && reviews.Count > 0) {
					return reviews.Count;
				}
				return 0;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell ("ReviewsTableCell");
				if (cell == null)
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, "ReviewsTableCell");
				Review r = reviews [indexPath.Row];
				cell.TextLabel.Text = r.Critic + " ( "+ r.Publication  +" )";
				string criticsRessources = r.GetFreshnessIcon () + ".png";
				cell.ImageView.Image = new UIImage(criticsRessources);
				cell.DetailTextLabel.Text = r.Quote;
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				controller.OpenUrl(reviews[indexPath.Row].Links.Review.AbsoluteUri);
				tableView.DeselectRow (indexPath, true);
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				return false;
			}
		}
	}
}

