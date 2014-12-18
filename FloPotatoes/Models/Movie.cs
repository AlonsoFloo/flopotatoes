using System;
using System.Collections.Generic;

namespace FloPotatoes
{
	public class Movie
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int Year { get; set; }
		public int Runtime { get; set; }
		public string Mpaa_Rating { get; set; }
		public ReleaseDates Release_Dates { get; set; }
		public Ratings Ratings { get; set; }
		public string Synopsis { get; set; }
		public Posters Posters { get; set; }
		public List<People> Abridged_Cast { get; set; }
		public List<People> Abridged_Directors { get; set; }
		public string[] Genres { get; set; }
		public Links Links { get; set; }

		public Movie ()
		{
		}

		public string GetRatingIcon() {
			if (this.Ratings.Critics_Rating.ToLower() == (PotatoesManager.FRESH)) {
				return "icon_fresh";
			} else if (this.Ratings.Critics_Rating.ToLower() == (PotatoesManager.CERTIFIED_FRESH)) {
				return "icon_critics_fresh";
			}
			return "icon_rotten";
		}

		public String GetRuntimeReadable() {
			TimeSpan time = TimeSpan.FromMinutes (Runtime);
			return ((int) time.TotalHours) +" hr. "+ time.Minutes +" min";
		}

		public Dictionary<string, string> GetData()
		{
			return new Dictionary <string,string> {
				{ "MovieID", this.Id.ToString() },
				{ "MovieTitle", this.Title },
				{ "Posters_Original", this.Posters.Original.AbsoluteUri }
			};
		}
	}
}

