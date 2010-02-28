#region using

using System;
using System.Web;
using CoyoEden.Core;
using System.Net.Mail;
using System.Threading;
using CoyoEden.Core.Infrastructure;

#endregion

/// <summary>
/// Sends an e-mail to the blog owner whenever a comment is added.
/// </summary>
[Extension("Sends an e-mail to the blog owner whenever a comment is added", "1.3", "CoyoEden.NET")]
public class SendCommentMail
{

	/// <summary>
	/// Hooks up an event handler to the Post.CommentAdded event.
	/// </summary>
	static SendCommentMail()
	{
		Post.CommentAdded += new EventHandler<EventArgs>(Post_CommentAdded);
	}

	private static void Post_CommentAdded(object sender, EventArgs e)
	{
		Post post = (Post)((PostComment)sender).Parent;
		if (post != null && BlogSettings.Instance.SendMailOnComment && !Thread.CurrentPrincipal.Identity.IsAuthenticated)
		{
			var comment = post.Comments[post.Comments.Count - 1];
			// Trackback and pingback comments don't have a '@' symbol in the e-mail field.
			string replyTo = comment.Email.Contains("@") ? comment.Email : BlogSettings.Instance.Email;
			string subject = " comment on ";

			if (comment.Email == "trackback")
				subject = " trackback on ";
			else if (comment.Email == "pingback")
				subject = " pingback on ";

            System.Globalization.CultureInfo defaultCulture = BlogSettings.GetDefaultCulture();

			ServingEventArgs args = new ServingEventArgs(comment.Content, ServingLocation.Email);
			PostComment.OnServing(comment, args);
			string body = args.Body;

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(BlogSettings.Instance.Email);
			mail.To.Add(BlogSettings.Instance.Email);
            mail.ReplyTo = new MailAddress(replyTo, HttpUtility.HtmlDecode(comment.Author));
			mail.Subject = BlogSettings.Instance.EmailSubjectPrefix + subject + post.Title;
			mail.Body = "<div style=\"font: 11px verdana, arial\">";
			mail.Body += String.Format("{0}<br /><br />", body.Replace(Environment.NewLine, "<br />"));
            mail.Body += string.Format("<strong>{0}</strong>: <a href=\"{1}\">{2}</a><br /><br />", SystemX.Web.Utils.Translate("post", null, defaultCulture), post.PermaLink + "#id_" + comment.Id, post.Title);

            string deleteLink = String.Format("{0}?deletecomment={1}", post.AbsoluteLink, comment.Id);
            mail.Body += string.Format("<a href=\"{0}\">{1}</a>", deleteLink, SystemX.Web.Utils.Translate("delete", null, defaultCulture));

			if (BlogSettings.Instance.EnableCommentsModeration)
			{
                string approveLink = String.Format("{0}?approvecomment={1}", post.AbsoluteLink, comment.Id);
                mail.Body += string.Format(" | <a href=\"{0}\">{1}</a>", approveLink, SystemX.Web.Utils.Translate("approve", null, defaultCulture));
			}

			mail.Body += "<br />_______________________________________________________________________________<br />";
			mail.Body += "<h3>Author information</h3>";
			mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
			mail.Body += String.Format("<strong>Name:</strong> {0}<br />", comment.Author);
			mail.Body += String.Format("<strong>E-mail:</strong> {0}<br />", comment.Email);
			mail.Body += string.Format("<strong>Website:</strong> <a href=\"{0}\">{0}</a><br />", comment.Website);

			if (comment.Country != null)
				mail.Body += String.Format("<strong>Country code:</strong> {0}<br />", comment.Country.ToUpperInvariant());

			if (HttpContext.Current != null)
			{
				mail.Body += String.Format("<strong>IP address:</strong> {0}<br />", HttpContext.Current.Request.UserHostAddress);
				mail.Body += String.Format("<strong>User-agent:</strong> {0}", HttpContext.Current.Request.UserAgent);
			}

			mail.Body += "</div>";
			mail.Body += "</div>";

			SystemX.Utils.SendMailMessageAsync(mail);
		}
	}

}
