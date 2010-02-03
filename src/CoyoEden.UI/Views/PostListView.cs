using System;
using System.Collections.Generic;
using System.Web.UI;
using CoyoEden.Core.Infrastructure;
using CoyoEden.Core;
using CoyoEden.Core.Web.Controls;
using System.Text.RegularExpressions;
using SystemX.Web;

namespace CoyoEden.UI.Views
{
	public class PostListView:ViewBase
	{
		private ViewManager<PostViewBase> vmPost;

		#region view data
		public bool NothingToShow {
			get
			{
				return Posts==null||VisiblePosts.Count==0||ShowCountActual<0;
			}
		}
		private List<IPublishable> _Posts = Post.Posts.ConvertAll<IPublishable>(x => x as IPublishable);
		/// <summary>
		/// get All the Posts
		/// </summary>
		public List<IPublishable> Posts {
			get
			{
				return _Posts;
			}
			set
            {
            	_Posts = value;
            }
		}
		/// <summary>
		/// All visible posts
		/// </summary>
		public List<IPublishable> VisiblePosts {
			get
			{
				return Posts.FindAll(x=>x.IsVisible);
			}
		}
		public List<IPublishable> PostsToShow {
			get
			{
				return VisiblePosts.GetRange(StartIndex, ShowCountActual);
			}
		}
		/// <summary>
		/// show count
		/// </summary>
		public int ShowCount {
			get
			{
				return Math.Min(BlogSettings.Instance.PostsPerPage, VisiblePosts.Count);
			}
		}
		public int ShowCountActual{
			get
            {
            	if (StartIndex+ShowCount>VisiblePosts.Count){
					return VisiblePosts.Count-StartIndex;
				}
				return ShowCount;
            }
		}
		public int StartIndex{
			get
            {
            	return CurrentPage()*ShowCount;
            }
		}
		public int StopIndex{
			get
            {
            	return (StartIndex+ShowCountActual) ;
            }
		}
		public bool HasPreviousPosts{
			get
            {
            	return StopIndex<VisiblePosts.Count;
            }
		}
		public string PostViewPath {
			get
			{
				return GetViewPath("PostView");
			}
		}

		private static readonly Regex REMOVE_DEFAULT_ASPX = new Regex("default\\.aspx", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private string PageUrlFormat {
			get
			{
				string path = Request.RawUrl;

				// Leave "default.aspx" when posts for a specific year/month or specific date are displayed.
				if (!(Request.QueryString["year"] != null || Request.QueryString["date"] != null))
					path = REMOVE_DEFAULT_ASPX.Replace(path, string.Empty);

				if (path.Contains("?"))
				{
					if (path.Contains("page="))
					{
						int index = path.IndexOf("page=");
						path = path.Substring(0, index);
					}
					else
					{
						path += "&";
					}
				}
				else
				{
					path += "?";
				}
				path = path + "page={0}";
				return path;
			}
		}
		public string PreviousPageUrl {
			get
			{
				return string.Format(PageUrlFormat,CurrentPage()+2);
			}
		}
		public string NextPageUrl {
			get
			{
				return string.Format(PageUrlFormat,CurrentPage());
			}
		}
		#endregion

		#region methods
		/// <summary>
		/// Retrieves the current page index based on the QueryString.
		/// </summary>
		protected int CurrentPage()
		{
			int index = 0;
			string page = Request.QueryString["page"];
			if (page != null && int.TryParse(page, out index) && index > 0)
				index--;

			return index;
		}
		protected string RenderSinglePost(Post post, int index) {
			vmPost.Control.ShowExcerpt = BlogSettings.Instance.ShowDescriptionInPostList;
			vmPost.Control.Post = post;
			vmPost.Control.Index = index;
			vmPost.Control.ID = post.Id.ToString().Replace("-", string.Empty);
			vmPost.Control.Location = ServingLocation.PostList;
			var postHtml = vmPost.Render();
			return postHtml;
		}
		#endregion

		#region base overrides
		protected override void OnPreRender(EventArgs e)
		{
			var page = CurrentPage();
			if (page > 0)
			{
				(Page as SiteBasePage).AddGenericLink("next", "Next page", NextPageUrl);
				Page.Title += String.Format(" - Page {0}", (page + 1));
			};
			if (HasPreviousPosts)
			{
				(Page as SiteBasePage).AddGenericLink("prev", "Previous page", PreviousPageUrl);
			}
			base.OnPreRender(e);
		}
		protected override void OnLoad(EventArgs e)
		{
			vmPost = new ViewManager<PostViewBase>(PostViewPath);
			base.OnLoad(e);
		}
		#endregion
	}
}
