#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using System.Xml;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CoyoEden.Core;
using SystemX.Web;
using CoyoEden.Core.Infrastructure;
using CoyoEden.UI.Controls;

#endregion

public partial class widgets_RecentComments_widget : WidgetBase
{

	private const int DEFAULT_NUMBER_OF_COMMENTS = 10;

	static widgets_RecentComments_widget()
	{
		Post.CommentAdded += delegate { HttpRuntime.Cache.Remove("widget_recentcomments"); };
		Post.CommentRemoved += delegate { HttpRuntime.Cache.Remove("widget_recentcomments"); };
		BlogSettings.Changed += delegate { HttpRuntime.Cache.Remove("widget_recentcomments"); };
	}

	public override void LoadWidget()
	{
		int numberOfComments = DEFAULT_NUMBER_OF_COMMENTS;
		if (CurrentSettings.ContainsKey("numberofcomments"))
			numberOfComments = int.Parse(CurrentSettings["numberofcomments"]);

		if (HttpRuntime.Cache["widget_recentcomments"] == null)
		{
			var comments = new List<IComment>();

			foreach (Post post in Post.Posts)
			{
				if (post.IsVisible)
				{
					comments.AddRange(post.Comments.FindAll(c => c.IsApproved.Value && c.Email.Contains("@")));
				}
			}

			comments.Sort();
			comments.Reverse();

			int max = Math.Min(comments.Count, numberOfComments);
			var list = comments.GetRange(0, max);
			HttpRuntime.Cache.Insert("widget_recentcomments", list);
		}

		string content = RenderComments((List<IComment>)HttpRuntime.Cache["widget_recentcomments"], CurrentSettings);

		LiteralControl html = new LiteralControl(content); //new LiteralControl((string)HttpRuntime.Cache["widget_recentcomments"]);
		phPosts.Controls.Add(html);
	}

	private string RenderComments(List<IComment> comments, StringDictionary settings)
	{
		if (comments.Count == 0)
		{
			//HttpRuntime.Cache.Insert("widget_recentcomments", "<p>" + Resources.labels.none + "</p>");
			return String.Format("<p>{0}</p>", Resources.labels.none);
		}

		HtmlGenericControl ul = new HtmlGenericControl("ul");
		ul.Attributes.Add("class", "recentComments");
		ul.ID = "recentComments";

		foreach (var comment in comments)
		{
			if (comment.IsApproved.Value)
			{
				HtmlGenericControl li = new HtmlGenericControl("li");

				// The post title
				HtmlAnchor title = new HtmlAnchor();
				title.HRef = comment.Parent.RelativeLink.ToString();
				title.InnerText = comment.Parent.Title;
				title.Attributes.Add("class", "postTitle");
				li.Controls.Add(title);

				// The comment count on the post
				LiteralControl count = new LiteralControl(" (" + ((Post)comment.Parent).ApprovedComments.Count + ")<br />");
				li.Controls.Add(count);

				// The author
				if (comment.Website != null)
				{
					HtmlAnchor author = new HtmlAnchor();
                    author.Attributes.Add("rel", "nofollow");
					author.HRef = comment.Website.ToString();
					author.InnerHtml = comment.Author;
					li.Controls.Add(author);

					LiteralControl wrote = new LiteralControl(String.Format(" {0}: ", Resources.labels.wrote));
					li.Controls.Add(wrote);
				}
				else
				{
					LiteralControl author = new LiteralControl(comment.Author + " " + Resources.labels.wrote + ": ");
					li.Controls.Add(author);
				}

				// The comment body
				string commentBody = Regex.Replace(comment.Content, @"\[(.*?)\]", "");
				int bodyLength = Math.Min(commentBody.Length, 50);

				commentBody = commentBody.Substring(0, bodyLength);
				if (commentBody.Length > 0)
				{
					if (commentBody[commentBody.Length - 1] == '&')
					{
						commentBody = commentBody.Substring(0, commentBody.Length - 1);
					}
				}
				commentBody += comment.Content.Length <= 50 ? " " : "?";
				LiteralControl body = new LiteralControl(commentBody);
				li.Controls.Add(body);

				// The comment link
				HtmlAnchor link = new HtmlAnchor();
				link.HRef = comment.Parent.RelativeLink + "#id_" + comment.Id;
				link.InnerHtml = String.Format("[{0}]", Resources.labels.more);
				link.Attributes.Add("class", "moreLink");
				li.Controls.Add(link);

				ul.Controls.Add(li);
			}
		}

		using (StringWriter sw = new StringWriter())
		{
			ul.RenderControl(new HtmlTextWriter(sw));
			string ahref = "<a href=\"{0}syndication.axd?comments=true\">Comment RSS <img src=\"{0}pics/rssButton.gif\" alt=\"\" /></a>";
			string rss = string.Format(ahref, Utils.RelativeWebRoot);
			sw.Write(rss);
			return sw.ToString();
		}
		//HttpRuntime.Cache.Insert("widget_recentcomments", sw.ToString());
	}

	public override string Name
	{
		get { return "RecentComments"; }
	}

	public override bool IsEditable
	{
		get { return true; }
	}

}
