using System;
using Xamarin.UITest.Queries;

namespace FloPotatoes.UITests
{
	public class IOSQueries : QueriesInterface
	{
		public Func<AppQuery, AppQuery> OpeningHeader { get { return c => c.Class("UIView").Text("OPENING THIS WEEK"); } }

		public Func<AppQuery, AppQuery> BoxOfficeHeader { get { return c => c.Class("UIView").Text("TOP BOX OFFICE"); } }

		public Func<AppQuery, AppQuery> FirstBoxOfficeRatingPic { get { return c => c.Id("section1-row0").Child().Marked("criticsRatingView-Fresh"); } }

		public Func<AppQuery, AppQuery> MovieCell { get { return c => c.Id("section0-row0"); } }

		public Func<AppQuery, AppQuery> SynopsysText { get { return c => c.Marked("synopsisLabel"); } }
	}
}

