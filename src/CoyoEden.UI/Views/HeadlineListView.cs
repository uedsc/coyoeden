using System;
using System.Collections.Generic;
using CoyoEden.Core;
using SystemX.Web;
using CoyoEden.Core.Infrastructure;

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
		private string _ItemViewPath;
		/// <summary>
		/// View path of the item view.
		/// </summary>
		public string ItemViewPath
		{
			get
			{
				return _ItemViewPath;
			}
			set
            {
            	_ItemViewPath = value;
            }
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_ItemViewPath =_ItemViewPath?? ViewPath("HeadlineView");
			vmPost = new ViewManager<PostView>(ItemViewPath);
			ShowCount = ShowCount < 1 ? 1 : ShowCount;
			var tempTotal = 0;
			HeadlinesToShow = Post.GetPosts(true, ShowCount, out tempTotal);
			Total = tempTotal;
		}
		private ViewManager<PostView> vmPost;
		protected string RenderSinglePost(Post post, int index)
		{
			vmPost.Control.ShowExcerpt = true;
			vmPost.Control.Post = post;
			vmPost.Control.Index = index;
			vmPost.Control.ID = String.Format("excerpt{0}", post.Id.ToString().Replace("-", string.Empty));
			vmPost.Control.Location = ServingLocation.PostList;
			var postHtml = vmPost.Render();
			return postHtml;
		}
	}
}
