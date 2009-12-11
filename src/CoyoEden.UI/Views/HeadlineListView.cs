using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core;

namespace CoyoEden.UI.Views
{
	public class HeadlineListView:ViewBase
	{
		public int ShowCount { get; set; }
		public int Total{get;private set;}
		public List<Post> HeadlinesToShow
		{
			get;
			private set;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			ShowCount = ShowCount < 1 ? 1 : ShowCount;
			var tempTotal = 0;
			HeadlinesToShow = Post.GetPosts(true, ShowCount, out tempTotal);
			Total = tempTotal;
		}
	}
}
