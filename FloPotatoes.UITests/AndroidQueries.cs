using System;
using Xamarin.UITest.Queries;

namespace FloPotatoes.UITests
{
	public class AndroidQueries : QueriesInterface
	{
		public Func<AppQuery, AppQuery> OpeningHeader { get { return c => c.Text("Opening This Week"); } }

		public Func<AppQuery, AppQuery> BoxOfficeHeader { get { return c => c.Text("Top Box Office"); } }

		public Func<AppQuery, AppQuery> FirstBoxOfficeRatingPic { get { return c => c.Marked ("rowCell12").All().Id("certifiedView").Marked ("criticsRatingView-Fresh"); } }

		public Func<AppQuery, AppQuery> MovieCell { get { return c => c.Marked ("rowCell1"); } }

		public Func<AppQuery, AppQuery> SynopsysText { get { return c => c.Id("synopsisView"); } }
	}
}