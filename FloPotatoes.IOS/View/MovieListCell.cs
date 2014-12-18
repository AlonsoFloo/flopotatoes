using System;
using UIKit;
using FloPotatoes;
using System.Drawing;

namespace FloPotatoes.IOS
{
	public class MovieListCell : UITableViewCell
	{
		public const string CellId = "MovieListCell";
		public const int MovieCellRowHeight = 90;
		Movie movie;
		UILabel titleLabel;
		UIImageView imageView;
		UIImageView criticsRatingView;
		UILabel criticsScoreLabel;
		UILabel actorLabel;
		UILabel subLabel;
		UILabel dateLabel;

		public Movie Movie {
			get { return movie; }
			set {
				movie = value;

				Update ();
			}
		}

		public async void Update()
		{
			titleLabel.Text = movie.Title;
			string path = await PictureManager.Download (movie.Posters.Thumbnail.AbsoluteUri);
			UIImage thumbnails = new UIImage(path);
			imageView.Image = thumbnails;
			string criticsRessources = movie.GetRatingIcon () + ".png";
			UIImage criticsImage = new UIImage(criticsRessources);
			criticsRatingView.Image = criticsImage;
			criticsRatingView.AccessibilityLabel = "criticsRatingView-"+ movie.Ratings.Critics_Rating;
			criticsScoreLabel.Text = movie.Ratings.Critics_Score.ToString() + "%";
			int actorMax = movie.Abridged_Cast.Count < 2 ? movie.Abridged_Cast.Count : 2;
			actorLabel.Text = string.Join (", ", movie.Abridged_Cast.ConvertAll(actor => actor.Name).ToArray(), 0, actorMax);
			subLabel.Text = movie.Mpaa_Rating + ", " + movie.GetRuntimeReadable ();;
			dateLabel.Text = movie.Release_Dates.GetTheaterDateReadable ();
		}
		

		public MovieListCell ()
		{
			ContentView.BackgroundColor = UIColor.White;
			Accessory = UITableViewCellAccessory.DisclosureIndicator;

			titleLabel = new UILabel {
				TextColor = UIColor.Blue,
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("HelveticaNeue-Light", 10),
				Layer = {
					ShadowRadius = 3,
					ShadowColor = UIColor.Black.CGColor,
					ShadowOffset = new System.Drawing.SizeF(0,1f),
					ShadowOpacity = .5f,
				}
			};

			imageView = new UIImageView ();
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			criticsRatingView = new UIImageView ();
			criticsRatingView.ContentMode = UIViewContentMode.ScaleAspectFit;

			criticsScoreLabel = new UILabel {
				TextColor = UIColor.DarkTextColor,
				Font = UIFont.FromName ("HelveticaNeue-Light", 8),
			};

			actorLabel = new UILabel {
				TextColor = UIColor.DarkTextColor,
				Font = UIFont.FromName ("HelveticaNeue-Light", 8),
			};

			subLabel = new UILabel {
				TextColor = UIColor.DarkTextColor,
				Font = UIFont.FromName ("HelveticaNeue-Light", 8),
			};

			dateLabel = new UILabel {
				TextColor = UIColor.DarkTextColor,
				Font = UIFont.FromName ("HelveticaNeue-Light", 8),
			};

			ContentView.AddSubviews (titleLabel, imageView, criticsRatingView, criticsScoreLabel, actorLabel, subLabel, dateLabel);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var bounds = ContentView.Bounds;

			imageView.Frame = new RectangleF (
				(float) bounds.X + 5,
				(float) bounds.Y + 5,
				(float) 50,
				80
			);

			titleLabel.Frame = new RectangleF (
				(float) bounds.X + 60,
				(float) bounds.Y + 5,
				(float) bounds.Width - 60,
				12
			);

			criticsRatingView.Frame = new RectangleF (
				(float) bounds.X + 60,
				(float) bounds.Y + 20,
				(float) 12,
				12
			);

			criticsScoreLabel.Frame = new RectangleF (
				(float) bounds.X + 80,
				(float) bounds.Y + 20,
				(float) bounds.Width - 90,
				10
			);

			actorLabel.Frame = new RectangleF (
				(float) bounds.X + 60,
				(float) bounds.Y + 35,
				(float) bounds.Width - 60,
				10
			);

			subLabel.Frame = new RectangleF (
				(float) bounds.X + 60,
				(float) bounds.Y + 50,
				(float) bounds.Width - 60,
				10
			);

			dateLabel.Frame = new RectangleF (
				(float) bounds.X + 60,
				(float) bounds.Y + 65,
				(float) bounds.Width - 60,
				10
			);
		}
	}
}

