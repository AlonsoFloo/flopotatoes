using System;

namespace FloPotatoes
{
	public class Review
	{
		public string Critic { get; set; }
		public string Date { get; set; }
		public string Original_Score { get; set; }
		public string Freshness { get; set; }
		public string Publication { get; set; }
		public string Quote { get; set; }
		public Links Links { get; set; }

		public Review ()
		{
		}

		public string GetFreshnessIcon() {
			if (this.Freshness.ToLower() == (PotatoesManager.FRESH)) {
				return "icon_fresh";
			} else if (this.Freshness.ToLower() == (PotatoesManager.CERTIFIED_FRESH)) {
				return "icon_critics_fresh";
			}
			return "icon_rotten";
		}
	}
}

