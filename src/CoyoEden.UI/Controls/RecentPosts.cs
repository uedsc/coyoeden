#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections.Generic;
using CoyoEden.Core;
using CoyoEden.Core.Infrastructure;
using SystemX;

#endregion

namespace CoyoEden.UI.Controls
{
  /// <summary>
  /// Shows a chronological list of recent posts.
  /// </summary>
  public class RecentPosts : Control
  {

    static RecentPosts()
    {
      BuildPostList();
      Post.Saved1 += new EventHandler<SavedEventArgs>(Post_Saved);
      Post.CommentAdded += delegate { BuildPostList(); };
      Post.CommentRemoved += delegate { BuildPostList(); };
      Post.Rated += delegate { BuildPostList(); };
      BlogSettings.Changed += delegate { BuildPostList(); };
    }

    static void Post_Saved(object sender, SavedEventArgs e)
    {
      if (e.Action != SaveAction.Update)
        BuildPostList();
    }

    private static object _SyncRoot = new object();
    private static List<Post> _Posts = new List<Post>();

    private static void BuildPostList()
    {
      lock (_SyncRoot)
      {
        int number = BlogSettings.Instance.NumberOfRecentPosts;
        if (number > Post.Posts.Count)
          number = Post.Posts.Count;

        int counter = 1;
        _Posts.Clear();
        foreach (Post post in Post.Posts)
        {
          if (counter <= number && post.IsVisibleToPublic)
          {
            _Posts.Add(post);
            counter++;
          }
        }
      }
    }

    private static string RenderPosts()
    {

			if (_Posts.Count == 0)
			{
				return String.Format("<p>{0}</p>", Utils.Translate("none", "none..."));
			}

      StringBuilder sb = new StringBuilder();
      sb.Append("<ul class=\"recentPosts\" id=\"recentPosts\">");

      foreach (Post post in _Posts)
      {
        if (!post.IsVisibleToPublic)
          continue;

        string rating = Math.Round(post.Rating.Value, 1).ToString(System.Globalization.CultureInfo.InvariantCulture);

        string link = "<li><a href=\"{0}\">{1}</a>{2}{3}</li>";
		string comments = string.Format("<span>{0}: {1}</span>", Utils.Translate("comments", "comments"), post.ApprovedComments.Count);
		string rate = string.Format("<span>{0}: {1} / {2}</span>", Utils.Translate("rating", "rating"), rating, post.Raters);

        if (!BlogSettings.Instance.DisplayCommentsOnRecentPosts || !BlogSettings.Instance.IsCommentEnabled)
          comments = null;

        if (!BlogSettings.Instance.DisplayRatingsOnRecentPosts || !BlogSettings.Instance.EnableRating)
          rate = null;

        if (post.Raters == 0)
			rating = Utils.Translate("notRatedYet", "notRatedYet");

        sb.AppendFormat(link, post.RelativeLink, HttpUtility.HtmlEncode(post.Title), comments, rate);
      }

      sb.Append("</ul>");
      return sb.ToString();
    }

    public override void RenderControl(HtmlTextWriter writer)
    {
      if (Page.IsCallback)
        return;
      
      string html = RenderPosts();
      writer.Write(html);
    }
  }
}