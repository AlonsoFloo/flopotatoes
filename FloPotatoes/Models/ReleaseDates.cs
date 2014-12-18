using System;

namespace FloPotatoes
{
	public class ReleaseDates
	{
		public string Theater { get; set; }

		public ReleaseDates ()
		{
		}

		public string GetTheaterDateReadable()
		{
			DateTime dt = Convert.ToDateTime (Theater);
			string pattern = "MMM dd, yyyy";
			return dt.ToString (pattern);
		}
	}
}

