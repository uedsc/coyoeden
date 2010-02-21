#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using CoyoEden.Core;
using SystemX.Web;

#endregion

namespace CoyoEden.UI.Controls
{
	/// <summary>
	/// Builds a category list.
	/// </summary>
	public class CategoryList : Control
	{

		static CategoryList()
		{
			Post.Saved1 += (sender, e) => _Html = null;
			Category.Saved1 += (sender, e) => _Html = null;
		}

		#region Properties

		private static bool _ShowRssIcon = true;
		/// <summary>
		/// Gets or sets whether or not to show feed icons next to the category links.
		/// </summary>
		public bool ShowRssIcon
		{
			get { return _ShowRssIcon; }
			set
			{
				if (_ShowRssIcon != value)
				{
					_ShowRssIcon = value;
					_Html = null;
				}
			}
		}

		private bool _ShowPostCount;

		public bool ShowPostCount
		{
			get { return _ShowPostCount; }
			set
			{
				if (_ShowPostCount!= value)
				{
					_ShowPostCount = value;
					_Html = null;
				}
			}
		}


		private static object _SyncRoot = new object();
		private static string _Html;
		private string Html
		{
			get
			{
				if (_Html == null)
				{
					lock (_SyncRoot)
					{
						if (_Html == null)
						{
							HtmlGenericControl ul = BindCategories();
							System.IO.StringWriter sw = new System.IO.StringWriter();
							ul.RenderControl(new HtmlTextWriter(sw));
							_Html = sw.ToString();
						}
					}
				}

				return _Html;
			}
		}

		#endregion

		private HtmlGenericControl BindCategories()
		{
			SortedDictionary<string, Guid> dic = SortCategories();

			if (dic.Keys.Count == 0)
			{
				HtmlGenericControl none = new HtmlGenericControl("p");
				none.InnerText = "None";
				return none;
			}

			HtmlGenericControl ul = new HtmlGenericControl("ul");
			ul.ID = "categorylist";

			foreach (Guid id in dic.Values)
			{

                // Find full category
			    Category cat = Category.GetCategory(id);
			    string key = cat.FullName;

				HtmlGenericControl li = new HtmlGenericControl("li");

				if (ShowRssIcon)
				{
					HtmlImage img = new HtmlImage();
					img.Src = String.Format("{0}pics/rssButton.gif", Utils.RelativeWebRoot);
					img.Alt = String.Format("{0} feed for {1}", BlogSettings.Instance.SyndicationFormat.ToUpperInvariant(), key);
					img.Attributes["class"] = "rssButton";

					HtmlAnchor feedAnchor = new HtmlAnchor();
                    feedAnchor.HRef = cat.FeedRelativeLink;
					feedAnchor.Attributes["rel"] = "nofollow";
					feedAnchor.Controls.Add(img);

					li.Controls.Add(feedAnchor);
				}

				int posts = Post.GetPostsByCategory(dic[key]).FindAll(delegate(Post p)
				{
					return p.IsVisible;
				}).Count;

				string postCount = " (" + posts + ")";
				if (!ShowPostCount)
					postCount = null;

				HtmlAnchor anc = new HtmlAnchor();
                anc.HRef = cat.RelativeLink;
				anc.InnerHtml = HttpUtility.HtmlEncode(key) + postCount;
				anc.Title = String.Format("Category: {0}", key);

				li.Controls.Add(anc);
				ul.Controls.Add(li);
			}

			return ul;
		}

		private SortedDictionary<string, Guid> SortCategories()
		{
			SortedDictionary<string, Guid> dic = new SortedDictionary<string, Guid>();
			foreach (Category cat in Category.Categories)
			{
				if (HasPosts(cat))
					dic.Add(cat.FullName, cat.Id.Value);
			}

			return dic;
		}

		private bool HasPosts(Category cat)
		{
			foreach (Post post in Post.Posts)
			{
				if (post.IsVisible)
				{
					foreach (Category category in post.Categories)
					{
						if (category == cat)
							return true;
					}
				}

			}
			return false;
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.Write(Html);
			writer.Write(Environment.NewLine);
		}
	}
}