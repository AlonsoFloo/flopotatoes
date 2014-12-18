
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Net;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FloPotatoes;
using Android.Graphics;
using Android.Text;
using Xamarin;

namespace FloPotatoes.Android
{
	[Activity (Label = "DetailsActivity")]			
	public class DetailsActivity : Activity
	{
		private Movie movie;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Activity_Movie);

			int movieId = this.Intent.GetIntExtra ("movieId", 0);
			RefreshMovieData(movieId);
			RefreshCritics (movieId);
		}

		public async void RefreshMovieData(int movieId)
		{
			var handle = Insights.TrackTime("TimeLoadMovie", new Dictionary<string, string> {
				{ "MovieID", movieId.ToString() }
			});
			handle.Start ();
			movie = await PotatoesManager.Instance.GetFullMovieData (movieId);
			handle.Stop ();

			TextView title = FindViewById<TextView>(Resource.Id.titleView);
			title.Text = movie.Title;

			ImageView ratingPicView = FindViewById<ImageView>(Resource.Id.ratingPicView);
			int certifiedPic = this.Resources.GetIdentifier ("drawable/" + movie.GetRatingIcon (), null, this.PackageName);
			ratingPicView.SetImageResource (certifiedPic);

			TextView ratingPercentView = FindViewById<TextView>(Resource.Id.ratingPercentView);
			ratingPercentView.Text = movie.Ratings.Critics_Score.ToString() +" %";

			TextView userRatingPercentView = FindViewById<TextView>(Resource.Id.userRatingPercentView);
			userRatingPercentView.Text = movie.Ratings.Audience_Score.ToString() +" %";

			TextView actorView = FindViewById<TextView>(Resource.Id.actorView);
			int actorMax = movie.Abridged_Cast.Count < 2 ? movie.Abridged_Cast.Count : 2;
			actorView.Text = string.Join (", ", movie.Abridged_Cast.ConvertAll(actor => actor.Name).ToArray(), 0, actorMax);


			TextView theaterRealseView = FindViewById<TextView>(Resource.Id.theaterRealseView);
			theaterRealseView.Text = movie.Release_Dates.GetTheaterDateReadable();

			TextView mpaaView = FindViewById<TextView>(Resource.Id.mpaaView);
			mpaaView.Text = movie.Mpaa_Rating;

			TextView runtime = FindViewById<TextView>(Resource.Id.runtime);
			runtime.Text = movie.GetRuntimeReadable();

			TextView synopsisView = FindViewById<TextView>(Resource.Id.synopsisView);
			synopsisView.Ellipsize = TextUtils.TruncateAt.End;
			synopsisView.SetMaxLines(4);
			synopsisView.Text = movie.Synopsis;

			TextView directorView = FindViewById<TextView>(Resource.Id.directorView);
			directorView.Text = string.Join (", ", movie.Abridged_Directors.ConvertAll(director => director.Name).ToArray());

			TextView genreView = FindViewById<TextView>(Resource.Id.genreView);
			genreView.Text = string.Join (", ", movie.Genres);

			TextView runningTimeView = FindViewById<TextView>(Resource.Id.runningTimeView);
			runningTimeView.Text = movie.GetRuntimeReadable();

			TextView theaterReleaseView = FindViewById<TextView>(Resource.Id.theaterReleaseView);
			theaterReleaseView.Text = movie.Release_Dates.GetTheaterDateReadable();

			LinearLayout castList = FindViewById<LinearLayout>(Resource.Id.castListView);
			castList.RemoveAllViews();
			LayoutInflater inflater = (LayoutInflater) this.GetSystemService(Context.LayoutInflaterService);
			foreach (People p in movie.Abridged_Cast) {
				View view = inflater.Inflate(Resource.Layout.Adapter_Movie_Cast, null);
				TextView text = view.FindViewById<TextView>(Resource.Id.textView);
				text.Text = p.Name;
				castList.AddView(view);
			}

			LinearLayout titleBar = FindViewById<LinearLayout>(Resource.Id.titleBar);
			titleBar.Click += delegate {
				Finish();
			};

			LinearLayout movieDesc = FindViewById<LinearLayout>(Resource.Id.movieDesc);
			movieDesc.Click += delegate {
				this.openUrl(movie.Links.Alternate.AbsoluteUri);
			};

			// Download the picture after displaying all the data
			ImageView pictureView = FindViewById<ImageView>(Resource.Id.pictureView);
			handle = Insights.TrackTime("TimeLoadMoviePosters", movie.GetData());
			handle.Start ();
			string path = await PictureManager.Download (movie.Posters.Original.AbsoluteUri);
			Bitmap ThumbnailBitmap = BitmapFactory.DecodeFile(path);
			pictureView.SetImageBitmap (ThumbnailBitmap);
			handle.Stop ();
		}

		public async void RefreshCritics(int movieId)
		{
			var handle = Insights.TrackTime("TimeLoadReview", new Dictionary<string, string> {
				{ "MovieID", movieId.ToString() }
			});
			handle.Start ();
			List<Review> reviewList = await PotatoesManager.Instance.GetReviews (movieId);
			handle.Stop ();

			LinearLayout criticsReviewList = FindViewById<LinearLayout>(Resource.Id.criticsReviewListView);
			criticsReviewList.RemoveAllViews();
			LayoutInflater inflater = (LayoutInflater) this.GetSystemService(Context.LayoutInflaterService);
			foreach (Review r in reviewList) {
				View view = inflater.Inflate(Resource.Layout.Adapter_Movie_Reviews, null);
				TextView nameView = view.FindViewById<TextView>(Resource.Id.nameView);
				nameView.Text = r.Critic;
				TextView sourceView = view.FindViewById<TextView>(Resource.Id.sourceView);
				sourceView.Text = r.Publication;
				TextView quoteView = view.FindViewById<TextView>(Resource.Id.quoteView);
				quoteView.Text = r.Quote;
				ImageView ratingPicView = view.FindViewById<ImageView>(Resource.Id.certifiedView);
				int certifiedPic = this.Resources.GetIdentifier ("drawable/" + r.GetFreshnessIcon (), null, this.PackageName);
				ratingPicView.SetImageResource (certifiedPic);

				view.Click += delegate {
					this.openUrl(r.Links.Review.AbsoluteUri);
				};

				criticsReviewList.AddView(view);
			}
		}

		private void openUrl(String url) {
			global::Android.Net.Uri uri = global::Android.Net.Uri.Parse (url);
			Intent browserIntent = new Intent(Intent.ActionView, uri);
			StartActivity(browserIntent);
		}
	}
}

