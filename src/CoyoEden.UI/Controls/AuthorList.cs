#region Using

using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CoyoEden.Core;
using SystemX;

#endregion

namespace CoyoEden.UI.Controls
{
	/// <summary>
	/// Builds a authro list.
	/// </summary>
	public class AuthorList : Control
	{

		/// <summary>
		/// Initializes the <see cref="AuthorList"/> class.
		/// </summary>
		static AuthorList()
		{
			Post.Saved1 += (sender, e) => _Html = null;
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

		private static object _SyncRoot = new object();

		private static string _Html;
		/// <summary>
		/// Caches the rendered HTML in the private field and first
		/// updates it when a post has been saved (new or updated).
		/// </summary>
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
							HtmlGenericControl ul = BindAuthors();
							using (System.IO.StringWriter sw = new System.IO.StringWriter())
							{
								ul.RenderControl(new HtmlTextWriter(sw));
								_Html = sw.ToString();
							}
						}
					}
				}

				return _Html;
			}
		}

		#endregion

		/// <summary>
		/// Loops through all users and builds the HTML
		/// presentation.
		/// </summary>
		private HtmlGenericControl BindAuthors()
		{
			if (Post.Posts.Count == 0)
			{
				HtmlGenericControl p = new HtmlGenericControl("p");
				p.InnerHtml =Utils.Translate("none", "none...");
				return p;
			}

			HtmlGenericControl ul = new HtmlGenericControl("ul");
			ul.ID = "authorlist";

			foreach (MembershipUser user in Membership.GetAllUsers())
			{
				int postCount = Post.GetPostsByAuthor(user.UserName).Count;
				if (postCount == 0)
					continue;

				HtmlGenericControl li = new HtmlGenericControl("li");

				if (ShowRssIcon)
				{
					HtmlImage img = new HtmlImage();
					img.Src = String.Format("{0}pics/rssButton.gif", Utils.RelativeWebRoot);
					img.Alt = String.Format("RSS feed for {0}", user.UserName);
					img.Attributes["class"] = "rssButton";

					HtmlAnchor feedAnchor = new HtmlAnchor();
					feedAnchor.HRef = String.Format("{0}syndication.axd?author={1}", Utils.RelativeWebRoot, Utils.RemoveIllegalCharacters(user.UserName));
					feedAnchor.Attributes["rel"] = "nofollow";
					feedAnchor.Controls.Add(img);

					li.Controls.Add(feedAnchor);
				}
				
				HtmlAnchor anc = new HtmlAnchor();
				anc.HRef = String.Format("{0}author/{1}{2}", Utils.RelativeWebRoot, user.UserName, BlogSettings.Instance.FileExtension);
				anc.InnerHtml = user.UserName + " (" + postCount + ")";
				anc.Title = String.Format("Author: {0}", user.UserName);

				li.Controls.Add(anc);
				ul.Controls.Add(li);
			}

			return ul;
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