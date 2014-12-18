using System;
using System.Collections.Generic;
using Android.Database;
using Android.Content;
using Android.Views;
using FloPotatoes;
using Android.Widget;
using Android.Graphics;
using Xamarin;

namespace FloPotatoes.Android
{
	public class MovieAdapter : BaseAdapter
	{
		private const int TYPE_ITEM = 0;
		private const int TYPE_SEPARATOR = 1;
		private const int TYPE_MAX_COUNT = 2;
		private List<Object> mergerList;
		private List<DataSetObserver> observers = new List<DataSetObserver>();
		private Context ctx;
		private LayoutInflater inflater;

		public MovieAdapter (Context newCtx)
		{
			this.ctx = newCtx;
			this.inflater = (LayoutInflater) this.ctx.GetSystemService(Context.LayoutInflaterService);
			mergerList = new List<Object>();
		}

		public void fillData(Dictionary<string, List<Movie>> list) {
			mergerList = new List<Object>();
			foreach (KeyValuePair<string, List<Movie>> pair in list)
			{
				string key = pair.Key;
				List<Movie> movieList = pair.Value;
				mergerList.Add(key);
				mergerList.AddRange(movieList);
			}
		}

		public override void RegisterDataSetObserver(DataSetObserver observer) {
			observers.Add(observer);
		}

		public override void UnregisterDataSetObserver(DataSetObserver observer) {
			observers.Remove(observer);
		}

		public override void NotifyDataSetChanged() {
			base.NotifyDataSetChanged ();
			foreach (DataSetObserver observer in observers)
			{
				observer.OnChanged();
			}
		}

		public int getCount() {
			int count = 0;
			try {
				count = mergerList.Count;
			} catch (NullReferenceException e) {
				Console.WriteLine(e.ToString());
				Insights.Report(e);
				count = 0;
			}
			return count;
		}

		public override Java.Lang.Object GetItem(int position) {
			return null;
		}

		public Object GetMovie(int position) {
			return this.mergerList [position];
		}

		public override long GetItemId(int position) {
			return position;
		}

		public override bool HasStableIds {
			get {
				return false;
			}
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder = null;
			int type = GetItemViewType(position);
			if (convertView == null) {
				switch (type) {
				case TYPE_ITEM:
					convertView = inflater.Inflate (Resource.Layout.Adapter_Home_Movie, parent, false);
					ImageView pictureView = convertView.FindViewById<ImageView> (Resource.Id.pictureView);
					ImageView certifiedView = convertView.FindViewById<ImageView> (Resource.Id.certifiedView);
					TextView titleView = convertView.FindViewById<TextView> (Resource.Id.titleView);
					TextView scoreView = convertView.FindViewById<TextView> (Resource.Id.scoreView);
					TextView actorView = convertView.FindViewById<TextView> (Resource.Id.actorView);
					TextView mpaaView = convertView.FindViewById<TextView> (Resource.Id.mpaaView);
					TextView runtime = convertView.FindViewById<TextView> (Resource.Id.runtime);
					TextView dateView = convertView.FindViewById<TextView> (Resource.Id.dateView);
					holder = new ViewHolder(null, pictureView, certifiedView, titleView, scoreView, actorView, mpaaView, runtime, dateView);
					break;
				case TYPE_SEPARATOR:
					convertView = inflater.Inflate(Resource.Layout.Separator, parent, false);
					TextView separator = convertView.FindViewById<TextView> (Resource.Id.separatorView);
					holder = new ViewHolder(separator, null, null, null, null, null, null, null, null);
					break;
				}
				convertView.SetTag(Resource.Id.runtime, holder);
			} else {
				holder = (ViewHolder) convertView.GetTag(Resource.Id.runtime);
			}

			Object item = GetMovie(position);
			if (item is string) {
				holder.Update((string) item, ctx);
			} else {
				holder.Update((Movie) item, ctx);
			}
			convertView.ContentDescription = "rowCell"+ position.ToString();

			return convertView;
		}

		public override int GetItemViewType(int position) {
			return IsSeparator(position) ? TYPE_SEPARATOR : TYPE_ITEM;
		}

		public bool IsSeparator(int position) {
			return GetMovie(position) is string;
		}

		public override bool AreAllItemsEnabled ()
		{
			return false;
		}

		public override bool IsEnabled (int position)
		{
			return !IsSeparator(position);
		}

		public override int Count {
			get {
				return getCount ();
			}
		}

		public override int ViewTypeCount {
			get {
				return TYPE_MAX_COUNT;
			}
		}

		private class ViewHolder : Java.Lang.Object {
			private ImageView pictureView;
			private ImageView certifiedView;
			private TextView titleView;
			private TextView scoreView;
			private TextView actorView;
			private TextView mpaaView;
			private TextView runtime;
			private TextView separator;
			private TextView dateView;

			public ViewHolder(TextView separator, ImageView pictureView, ImageView certifiedView, TextView titleView, TextView scoreView, TextView actorView, TextView mpaaView, TextView runtime, TextView dateView) {
				this.pictureView = pictureView;
				this.separator = separator;
				this.certifiedView = certifiedView;
				this.titleView = titleView;
				this.scoreView = scoreView;
				this.actorView = actorView;
				this.mpaaView = mpaaView;
				this.runtime = runtime;
				this.dateView = dateView;
			}

			public async void Update(Movie m, Context ctx) {
				string path = await PictureManager.Download (m.Posters.Thumbnail.AbsoluteUri);
				Bitmap myBitmap = BitmapFactory.DecodeFile(path);
				pictureView.SetImageBitmap (myBitmap);
				titleView.Text = m.Title;
				scoreView.Text = (m.Ratings.Critics_Score).ToString() +" %";
				mpaaView.Text = m.Mpaa_Rating;
				int certifiedPic = ctx.Resources.GetIdentifier ("drawable/" + m.GetRatingIcon (), null, ctx.PackageName);
				certifiedView.SetImageResource (certifiedPic);
				certifiedView.ContentDescription = "criticsRatingView-" + m.Ratings.Critics_Rating;
				runtime.Text = m.GetRuntimeReadable();
				int actorMax = m.Abridged_Cast.Count < 2 ? m.Abridged_Cast.Count : 2;
				string value = string.Join (", ", m.Abridged_Cast.ConvertAll(actor => actor.Name).ToArray(), 0, actorMax);
				actorView.Text = value;
				dateView.Text = m.Release_Dates.GetTheaterDateReadable ();
			}

			public void Update(string s, Context ctx) {
				separator.Text = s;
			}
		}
	}
}

