using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using FloPotatoes;

namespace FloPotatoes.IOS
{
	partial class MovieTableCell : UITableViewCell
	{
		public MovieTableCell (string cellId) : base (UITableViewCellStyle.Default, cellId)
		{
		}

		public void Update(Movie movie)
		{
			titleLabel.Text = movie.Title;
		}
	}
}
