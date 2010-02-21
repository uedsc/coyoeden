using System;
using System.Collections.Generic;
using System.Text;
using CoyoEden.Core;
using SystemX.Web;
using System.Web.UI.WebControls;
using CoyoEden.Core.Web.Controls;
using CoyoEden.Core.Infrastructure;
using CoyoEden.Core.DataContracts;
using System.Web;
using System.Web.Security;
using CoyoEden.Core.Web.Extensions;
using System.Data;
using System.Web.UI;
using SystemX.Infrastructure;
using System.IO;

namespace CoyoEden.UI.Views
{
	public class CommentListView : ViewBase, ICallbackEventHandler
	{
		public HiddenField hfCaptcha;
		public Post CurrentPost { get; set; }
		public CommentInputData CommentData {
			get
			{
				var retVal = new CommentInputData();
				HttpCookie cookie = Request.Cookies["comment"];
				try
				{
					if (cookie != null)
					{
						retVal.UserName= Server.UrlDecode(cookie.Values["name"]);
						retVal.Email= cookie.Values["email"];
						retVal.Website = cookie.Values["url"];
						retVal.Country= cookie.Values["country"];
					}
					else if (Page.User.Identity.IsAuthenticated)
					{
						var user = Membership.GetUser();
						retVal.UserName = user.UserName;
						retVal.Email = user.Email;
						retVal.Website = Request.Url.Host;
					}
				}
				catch (Exception)
				{
					// Couldn't retrieve info on the visitor/user
				}
				return retVal;
			}
		}
		public string CommentViewPath {
			get
			{
				return GetViewPath("CommentView");
			}
		}
		public bool NestingSupported {
			get
			{
				return BlogSettings.Instance.IsCommentNestingEnabled ;
			}
		}
		public bool CanBeCommented {
			get
			{
				if (!BlogSettings.Instance.IsCommentEnabled) return false;
				if (!CurrentPost.IsCommentEnabled.Value || (BlogSettings.Instance.DaysCommentsAreEnabled > 0 &&
				   CurrentPost.DateCreated.Value.AddDays(BlogSettings.Instance.DaysCommentsAreEnabled) < DateTime.Now.Date))
				{
					   return false;
				}
				return true;
			}
		}
		#region base overrides
		protected override void OnLoad(EventArgs e)
		{
			AssertPost();
			if (!Page.IsPostBack && !Page.IsCallback)
			{
				if (Page.User.Identity.IsAuthenticated)
				{
					if (Request.QueryString["deletecomment"] != null)
						DeleteComment();

					if (Request.QueryString["deletecommentandchildren"] != null)
						DeleteCommentAndChildren();

					if (!string.IsNullOrEmpty(Request.QueryString["approvecomment"]))
						ApproveComment();

					if (!string.IsNullOrEmpty(Request.QueryString["approveallcomments"]))
						ApproveAllComments();
				}
			}


			Page.ClientScript.GetCallbackEventReference(this, "arg", null, string.Empty);
			base.OnLoad(e);
		}
		#endregion

		#region methods
		protected virtual void AssertPost() {
			if (CurrentPost == null)
				Response.Redirect(Utils.RelativeWebRoot);
		}
		protected virtual void LoadNestedComments(List<IComment> nestedComments,PlaceHolder phComments)
		{
			if (!NestingSupported) return;
			if (nestedComments.Count == 0) return;
			nestedComments.ForEach(x =>
			{
				var control = (CommentViewBase)LoadControl(CommentViewPath);
				if (x.IsApproved.Value || !BlogSettings.Instance.EnableCommentsModeration || (!x.IsApproved.Value && Page.User.Identity.IsAuthenticated))
				{
					control.Comment = x as PostComment;
					control.Post = CurrentPost;

					if (x.Comments.Count > 0)
					{
						// find the next placeholder and add the subcomments to it
						PlaceHolder phSubComments = control.FindControl("phSubComments") as PlaceHolder;
						if (phSubComments != null)
						{
							LoadNestedComments(x.Comments, phSubComments);
						}
					}

					phComments.Controls.Add(control);
				}	
			});
		}
		protected virtual void LoadUnnestedComments(PlaceHolder phComments) {
			if (NestingSupported) return;
			//Add approved Comments
			foreach (var comment in CurrentPost.Comments)
			{
				if (comment.IsApproved.Value || !BlogSettings.Instance.EnableCommentsModeration)
				{
					CommentViewBase control = (CommentViewBase)LoadControl(CommentViewPath);
					control.Comment = comment as PostComment;
					control.Post = CurrentPost;
					phComments.Controls.Add(control);
				}
			}

			//Add unapproved comments
			if (!Page.User.Identity.IsAuthenticated) return;
			foreach (var comment in CurrentPost.NotApprovedComments)
			{
				CommentViewBase control = (CommentViewBase)LoadControl(CommentViewPath);
				control.Comment = comment as PostComment;
				control.Post = CurrentPost;
				phComments.Controls.Add(control);
			}
		}
		protected virtual void BindCountries(DropDownList ddlCountry)
		{
			ddlCountry.Items.Add(new ListItem("[Not specified]", ""));
			CountryData.Countries.ForEach(x => {
				ddlCountry.Items.Add(new ListItem(x.EnglishName, x.TwoLetterISORegionName));
			});
			if (ddlCountry.SelectedIndex == 0)
			{
				ddlCountry.SelectedValue =Utils.BrowserRegionInfo().TwoLetterISORegionName.ToLowerInvariant();
			};
		}
		protected virtual void UpdateCookie(CommentInputData data) {
			HttpCookie cookie = new HttpCookie("comment");
			cookie.Expires = DateTime.Now.AddMonths(24);
			cookie.Values.Add("name", Server.UrlEncode(data.UserName));
			cookie.Values.Add("email", data.Email);
			cookie.Values.Add("url", data.Website);
			cookie.Values.Add("country", data.Country);
			Response.Cookies.Add(cookie);
		}
		/// <summary>
		/// Validates that the name of the person writing posting a comment
		/// cannot be the same as the author of the post.
		/// </summary>
		protected virtual void CheckAuthorName(object sender, ServerValidateEventArgs e)
		{
			var txtName = sender as TextBox;
			e.IsValid = true;
			if (!Page.User.Identity.IsAuthenticated)
			{
				if (txtName.Text.ToLowerInvariant() == CurrentPost.Author.ToLowerInvariant())
					e.IsValid = false;
			}
		}

		/// <summary>
		/// Displays a delete link to visitors that is authenticated
		/// using the default membership provider.
		/// </summary>
		/// <param name="id">The id of the comment.</param>
		protected string AdminLink(string id)
		{
			if (Page.User.Identity.IsAuthenticated)
			{
				StringBuilder sb = new StringBuilder();
				foreach (var comment in CurrentPost.Comments)
				{
					if (comment.Id.ToString() == id)
						sb.AppendFormat(" | <a href=\"mailto:{0}\">{0}</a>", comment.Email);
				}

				string confirmDelete = "Are you sure you want to delete the comment?";
				sb.AppendFormat(" | <a href=\"?deletecomment={0}\" onclick=\"return confirm('{1}?')\">{2}</a>",
								id.ToString(), confirmDelete, "Delete");
				return sb.ToString();
			}

			return string.Empty;
		}
		/// <summary>
		/// Displays BBCodes dynamically loaded from settings.
		/// </summary>
		protected virtual string BBCodes()
		{
			try
			{
				string retVal = string.Empty;
				string title = string.Empty;
				string code = string.Empty;

				ExtensionSettings settings = ExtensionManager.GetSettings("BBCode");
				DataTable table = settings.GetDataTable();

				foreach (DataRow row in table.Rows)
				{
					code = (string)row["Code"];
					title = String.Format("[{0}][/{0}]", code);
					retVal += String.Format("<a title=\"{0}\" href=\"javascript:void(CoyoEden.addBbCode('{1}'))\">{1}</a>", title, code);
				}
				return retVal;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}


		#endregion

		#region ICallbackEventHandler Members
		private string _Callback;
		public string GetCallbackResult()
		{
			return _Callback;
		}

		public void RaiseCallbackEvent(string eventArgument)
		{
			if (!BlogSettings.Instance.IsCommentEnabled)
				return;

			string[] args = eventArgument.Split(new string[] { "-|-" }, StringSplitOptions.None);
			var commentData = new CommentInputData();
			commentData.UserName = args[0];
			commentData.Email = args[1];
			commentData.Website = args[2];
			commentData.Country = args[3];
			commentData.Content = args[4];
			
			bool notify = bool.Parse(args[5]);
			bool isPreview = bool.Parse(args[6]);
			string sentCaptcha = args[7];
			//If there is no "reply to" comment, args[8] is empty
			Guid replyToCommentID = String.IsNullOrEmpty(args[8]) ? Guid.Empty : new Guid(args[8]);

			string storedCaptcha = hfCaptcha.Value;

			if (sentCaptcha != storedCaptcha)
				return;

			var comment = new PostComment();
			comment.Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
			comment.ParentID = replyToCommentID;
			comment.Author = Server.HtmlEncode(commentData.UserName);
			comment.Email = commentData.Email;
			comment.Content = Server.HtmlEncode(commentData.Content);
			comment.Ip = Request.UserHostAddress;
			comment.Country = commentData.Country;
			comment.DateCreated = DateTime.Now;
			comment.Parent = CurrentPost;
			comment.IsApproved = !BlogSettings.Instance.EnableCommentsModeration;

			if (Page.User.Identity.IsAuthenticated)
				comment.IsApproved = true;

			if (commentData.Website.Trim().Length > 0)
			{
				if (!commentData.Website.ToLowerInvariant().Contains("://"))
					commentData.Website = String.Format("http://{0}", commentData.Website);

				Uri url;
				if (Uri.TryCreate(commentData.Website, UriKind.Absolute, out url))
					comment.Website = url.ToString();
			}

			if (!isPreview)
			{
				if (notify && !CurrentPost.NotificationEmails.Contains(commentData.Email))
					CurrentPost.NotificationEmails.Add(commentData.Email);
				else if (!notify && CurrentPost.NotificationEmails.Contains(commentData.Email))
					CurrentPost.NotificationEmails.Remove(commentData.Email);

				CurrentPost.AddComment(comment);
				UpdateCookie(commentData);
			}

			
			CommentViewBase control = (CommentViewBase)LoadControl(CommentViewPath);
			control.Comment = comment;
			control.Post = CurrentPost;

			using (StringWriter sw = new StringWriter())
			{
				control.RenderControl(new HtmlTextWriter(sw));
				_Callback = sw.ToString();
			}
		}

		#endregion


		private void ApproveComment()
		{
			foreach (var comment in CurrentPost.NotApprovedComments)
			{
				if (comment.Id == new Guid(Request.QueryString["approvecomment"]))
				{
					CurrentPost.ApproveComment(comment as PostComment);

					int index = Request.RawUrl.IndexOf("?");
					string url = Request.RawUrl.Substring(0, index);
					Response.Redirect(url, true);
				}
			}
		}

		private void ApproveAllComments()
		{

			CurrentPost.ApproveAllComments();

			int index = Request.RawUrl.IndexOf("?");
			string url = Request.RawUrl.Substring(0, index);
			Response.Redirect(url, true);
		}

		private void DeleteComment()
		{
			foreach (var comment in CurrentPost.Comments)
			{
				if (comment.Id == new Guid(Request.QueryString["deletecomment"]))
				{
					CurrentPost.RemoveComment(comment as PostComment);

					int index = Request.RawUrl.IndexOf("?");
					string url = String.Format("{0}#comment", Request.RawUrl.Substring(0, index));
					Response.Redirect(url, true);
				}
			}
		}

		private void DeleteCommentAndChildren()
		{
			foreach (var comment in CurrentPost.Comments)
			{
				if (comment.Id == new Guid(Request.QueryString["deletecommentandchildren"]))
				{
					// collect comments to delete first so the Nesting isn't lost
					var commentsToDelete = new List<IComment>();

					CollectCommentToDelete(comment, commentsToDelete);

					foreach (var commentToDelete in commentsToDelete)
						CurrentPost.RemoveComment(commentToDelete as PostComment);

					int index = Request.RawUrl.IndexOf("?");
					string url = String.Format("{0}#comment", Request.RawUrl.Substring(0, index));
					Response.Redirect(url, true);
				}
			}
		}


		private void CollectCommentToDelete(IComment comment, List<IComment> commentsToDelete)
		{
			commentsToDelete.Add(comment);
			// recursive collection
			foreach (var subComment in comment.Comments)
				CollectCommentToDelete(subComment, commentsToDelete);
		}
	}
}
