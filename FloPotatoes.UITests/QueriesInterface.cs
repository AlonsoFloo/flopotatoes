using System;
using Xamarin.UITest.Queries;

namespace FloPotatoes.UITests
{
	public interface QueriesInterface
	{
		Func<AppQuery, AppQuery> OpeningHeader { get; }

		Func<AppQuery, AppQuery> BoxOfficeHeader { get; }

		Func<AppQuery, AppQuery> FirstBoxOfficeRatingPic { get; }

		Func<AppQuery, AppQuery> MovieCell { get; }

		Func<AppQuery, AppQuery> SynopsysText { get; }
	}
}

