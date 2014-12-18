using System;

namespace FloPotatoes
{
	public class Posters
	{
		public Uri Thumbnail { get; set; }
		public Uri Profile { get; set; }
		public Uri Detailed { get; set; }
		private Uri _original;
		public Uri Original
		{
			get { return this._original; }
			set {
				string fullUrl = value.AbsoluteUri;
				if (fullUrl.EndsWith ("_tmb.jpg"))
					fullUrl = fullUrl.Replace ("_tmb.jpg", "_ori.jpg");
				this._original = new Uri(fullUrl);
			}
		}

		public Posters ()
		{
		}
	}
}

