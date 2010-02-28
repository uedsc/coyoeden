#region Using

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Globalization;
using CoyoEden.Core;
using SystemX.Web;
using CoyoEden.Core.Infrastructure;

#endregion

namespace CoyoEden.UI.Views{
	/// <summary>
	/// The PostView.ascx that is located in the themes folder
	/// has to inherit from this class. 
	/// <remarks>
	/// It provides the basic functionaly needed to display a post.
	/// </remarks>
	/// </summary>
	public class PostView : UserControl
	{
		/// <summary>
		/// Lets process our .Body content and build up our controls collection
		/// inside the 'BodyContent' placeholder.
		/// 
		/// User controls are insterted into the blog in the following format..
		/// [UserControl:~/path/usercontrol.ascx]
		/// 
		/// TODO : Expose user control parameters.
		/// 
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
			// Used to track where we are in the 'Body' as we parse it.
			int currentPosition = 0;
			string content = Body;
			PlaceHolder bodyContent = (PlaceHolder)FindControl("BodyContent");

			if (bodyContent != null)
			{
				MatchCollection myMatches = _BodyRegex.Matches(content);

				foreach (Match myMatch in myMatches)
				{
					// Add literal for content before custom tag should it exist.
					if (myMatch.Index > currentPosition)
					{
						bodyContent.Controls.Add(new LiteralControl(content.Substring(currentPosition, myMatch.Index - currentPosition)));
					}

					// Now lets add our user control.
					try
					{
						string all = myMatch.Groups[1].Value.Trim();
						Control usercontrol = null;

						if (!all.EndsWith(".ascx", StringComparison.OrdinalIgnoreCase))
						{
							int index = all.IndexOf(".ascx", StringComparison.OrdinalIgnoreCase) + 5;
							usercontrol = LoadControl(all.Substring(0, index));

							string parameters = Server.HtmlDecode(all.Substring(index));
							Type type = usercontrol.GetType();
							string[] paramCollection = parameters.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

							foreach (string param in paramCollection)
							{
								string name = param.Split('=')[0].Trim();
								string value = param.Split('=')[1].Trim();
								System.Reflection.PropertyInfo property = type.GetProperty(name);
								property.SetValue(usercontrol, Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture), null);
							}
						}
						else
						{
							usercontrol = LoadControl(all);
						}

						bodyContent.Controls.Add(usercontrol);

						// Now we will update our position.
						//currentPosition = myMatch.Index + myMatch.Groups[0].Length;
					}
					catch (Exception)
					{
						// Whoopss, can't load that control so lets output something that tells the developer that theres a problem.
                        bodyContent.Controls.Add(new LiteralControl(String.Format("ERROR - UNABLE TO LOAD CONTROL : {0}", myMatch.Groups[1].Value)));
					}

					currentPosition = myMatch.Index + myMatch.Groups[0].Length;
				}

				// Finally we add any trailing static text.
				bodyContent.Controls.Add(new LiteralControl(content.Substring(currentPosition, content.Length - currentPosition)));
			}
			else
			{
				// We have no placeholder so we assume this is an old style <% =Body %> theme and do nothing.
			}
		}

		private static readonly Regex _BodyRegex = new Regex(@"\[UserControl:(.*?)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>
		/// Shows the post if it isn\t published.
		/// </summary>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (!Post.IsVisible && !Page.User.Identity.IsAuthenticated)
			{
				this.Visible = false;
			}
		}

		/// <summary>
		/// The Post object that is displayed through the PostView.ascx control.
		/// </summary>
		/// <value>The Post object that has to be displayed.</value>
		public virtual Post Post
		{
			get { return (Post)(ViewState["Post"] ?? default(Post)); }
			set { ViewState["Post"] = value; }
		}

		private ServingLocation _Location = ServingLocation.None;
		/// <summary>
		/// The location where the serving takes place.
		/// </summary>
		public ServingLocation Location
		{
			get { return _Location; }
			set { _Location = value; }
		}

		private bool _ShowExcerpt;
		/// <summary>
		/// Gets or sets whether or not to show the entire post or just the excerpt/description.
		/// </summary>
		public bool ShowExcerpt
		{
			get { return _ShowExcerpt; }
			set { _ShowExcerpt = value; }
		}
        public bool HidePostIcon { get; set; }
        public bool ShowUserIcon { get; set; }
		private int _Index;
		/// <summary>
		/// The index of the post in a list of posts displayed
		/// </summary>
		public int Index
		{
			get { return _Index; }
			set { _Index = value; }
		}

		/// <summary>
		/// Gets the body of the post. Important: use this instead of Post.Content.
		/// </summary>
		public string Body
		{
			get
			{
				string body = Post.Content;
				if (ShowExcerpt)
				{
					string link = String.Format(" <a href=\"{0}\" title=\"\" class=\"more-link\">[{1}]</a>", Post.RelativeLink, Utils.Translate("more"));

					if (!string.IsNullOrEmpty(Post.Description))
					{
						body = Post.Description.Replace(Environment.NewLine, "<br />") + link;
					}
					else
					{
						body = SystemX.Utils.StripHtml(Post.Content);
						if (body.Length > BlogSettings.Instance.DescriptionCharacters)
							body = String.Format("{0}...{1}", body.Substring(0, BlogSettings.Instance.DescriptionCharacters), link);
					}
				}

				ServingEventArgs arg = new ServingEventArgs(body, this.Location);
				Post.OnServing(Post, arg);

				if (arg.Cancel)
				{
					if (arg.Location == ServingLocation.SinglePost)
					{
						Response.Redirect("~/error404.aspx", true);
					}
					else
					{
						this.Visible = false;
					}
				}

				return arg.Body ?? string.Empty;
			}
		}

		/// <summary>
		/// Gets the comment feed link.
		/// </summary>
		/// <value>The comment feed.</value>
		public string CommentFeed
		{
			get { return Post.RelativeLink.Replace("/post/", "/post/feed/"); }
		}
        /// <summary>
        /// icon for the post
        /// </summary>
        public string Icon {
            get
            {
                if (string.IsNullOrEmpty(Post.Icon)) return string.Format("{0}img/nopic_post.jpg",ThemeRoot);
                return Post.Icon;
            }
        }
        /// <summary>
        /// Current theme
        /// </summary>
        public virtual string CurrentTheme
        {
            get
            {
                string theme = Request.QueryString["theme"];
                theme = string.IsNullOrEmpty(theme) ? BlogSettings.Instance.Theme : theme;
                return theme;
            }
        }
        /// <summary>
        /// Url root of current theme
        /// </summary>
        public virtual string ThemeRoot
        {
            get
            {
                return string.Format("{0}themes/{1}/", Utils.AbsoluteWebRoot, CurrentTheme);
            }
        }
		#region Protected methods

		/// <summary>
		/// Displays the Post's categories seperated by the specified string.
		/// </summary>
		protected virtual string CategoryLinks(string separator)
		{
			string[] keywords = new string[Post.Categories.Count];
			string link = "<a href=\"{0}\" title=\"View all posts in {1}\" rel=\"category tag\" class=\"link_category\">{1}</a>";
			for (int i = 0; i < Post.Categories.Count; i++)
			{
                Category c = Category.GetCategory(Post.Categories[i].Id.Value);
                if (c != null)
				{
					keywords[i] = string.Format(CultureInfo.InvariantCulture, link, c.RelativeLink, c.Name);
				}
			}


			return string.Join(separator, keywords);
		}

		/// <summary>
		/// Displays the Post's tags seperated by the specified string.
		/// </summary>
		protected virtual string TagLinks(string separator)
		{
			if (Post.Tags.Count == 0)
				return null;

			string[] tags = new string[Post.Tags.Count];
			string link = "<a href=\"{0}/{1}\" rel=\"tag\">{2}</a>";
            string path = String.Format("{0}?tag=", Utils.RelativeWebRoot);
			for (int i = 0; i < Post.Tags.Count; i++)
			{
				string tag = Post.Tags[i];
				tags[i] = string.Format(CultureInfo.InvariantCulture, link, path, HttpUtility.UrlEncode(tag), HttpUtility.HtmlEncode(tag));
			}

			return string.Join(separator, tags);
		}

		/// <summary>
		/// Displays an Edit and Delete link to any 
		/// authenticated user.
		/// </summary>
		protected virtual string AdminLinks
		{
			get
			{
				if (Page.User.IsInRole(BlogSettings.Instance.AdministratorRole) || Page.User.Identity.Name.Equals(Post.Author, StringComparison.OrdinalIgnoreCase))
				{
					string confirmDelete = string.Format(CultureInfo.InvariantCulture, Utils.Translate("areYouSure"), Utils.Translate("delete").ToLowerInvariant(), Utils.Translate("thePost"));
					StringBuilder sb = new StringBuilder();

					if (Post.NotApprovedComments.Count > 0)
					{
						sb.AppendFormat(CultureInfo.InvariantCulture, "<a href=\"{0}\">{1} ({2})</a> | ", Post.RelativeLink, Utils.Translate("unapprovedcomments"), Post.NotApprovedComments.Count);
                        sb.AppendFormat(CultureInfo.InvariantCulture, "<a href=\"{0}\">{1}</a> | ", String.Format("{0}?approveallcomments=true", Post.RelativeLink), Utils.Translate("approveallcomments"));

					}

                    sb.AppendFormat(CultureInfo.InvariantCulture, "<a href=\"{0}\">{1}</a> | ", String.Format("{0}admin/Pages/PostEdit.aspx?id={1}", Utils.AbsoluteWebRoot, Post.Id), Utils.Translate("edit"));
					sb.AppendFormat(CultureInfo.InvariantCulture, "<a href=\"javascript:void(0);\" onclick=\"if (confirm('{2}')) location.href='{0}?deletepost={1}'\">{3}</a> | ", Post.RelativeLink, Post.Id.ToString(), confirmDelete, Utils.Translate("delete"));
					return sb.ToString();
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Enable visitors to rate the post.
		/// </summary>
		protected virtual string Rating
		{
			get
			{
				if (!BlogSettings.Instance.EnableRating)
					return string.Empty;

				//string script = "<div id=\"rating_{0}\"></div><script type=\"text/javascript\">CoyoEden.showRating('{0}',{1},{2});</script>";
				string script = "<div class=\"ratingcontainer\" style=\"visibility:hidden\">{0}|{1}|{2}</div>";
				return string.Format(script, Post.Id, Post.Raters, Post.Rating.Value.ToString("#.0", CultureInfo.InvariantCulture));
			}
		}

		#endregion
	}
}