
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using SystemX.LunaAtom;
using System.ComponentModel;
using SystemX.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Threading;
using SystemX.Web;
using CoyoEden.Core.Providers;
using System.Net.Mail;
using System.Web;
using CoyoEden.Core.Infrastructure;
using SystemX.LunaBase;

namespace CoyoEden.Core
{    
    public partial class Post:IComparable<Post>,IPublishable
    {

		#region Constructor
		/// <summary>
		/// The default contstructor assign default values.
		/// </summary>
		public Post()
		{
			Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
			DateCreated = DateTime.Now;
			IsPublished = true;
			IsCommentEnabled = true;
		}

		#endregion

		#region properties

		public AuthorProfile AuthorProfile
		{
			get { return AuthorProfile.GetProfile(Author); }
		}
		public List<IComment> Comments {
			get
			{
				return PostComments.Cast<IComment>().ToList();
			}
		}
		/// <summary>
		/// A collection of Approved comments for the post sorted by date.
		/// </summary>
		public List<IComment> ApprovedComments
		{
			get { return Comments.FindAll(c => c.IsApproved.Value); }
		}
		/// <summary>
		/// A collection of comments waiting for approval for the post, sorted by date.
		/// </summary>
		public List<IComment> NotApprovedComments
		{
			get { return Comments.FindAll(c => !c.IsApproved.Value); }
		}
		private List<IComment> _NestedComments;

		/// <summary>
		/// A collection of the comments that are nested as replies
		/// </summary>
		public List<IComment> NestedComments
		{
			get
			{
				if (_NestedComments == null)
					CreateNestedComments();
				return _NestedComments;
			}
		}

		/// <summary>
		/// Clears all nesting of comments
		/// </summary>
		private void ResetNestedComments()
		{
			// void the List<>
			_NestedComments = null;
			// go through all comments and remove sub comments
			Comments.ForEach(x=>x.Comments.Clear());
			
		}

		/// <summary>
		/// Nests comments based on Id and ParentId
		/// </summary>
		private void CreateNestedComments()
		{
			// instantiate object
			_NestedComments = new List<IComment>();

			// temporary ID/Comment table
			Hashtable commentTable = new Hashtable();

			foreach (var comment in Comments)
			{
				// add to hashtable for lookup
				commentTable.Add(comment.Id, comment);

				// check if this is a child comment
				if (comment.ParentID == Guid.Empty)
				{
					// root comment, so add it to the list
					_NestedComments.Add(comment);
				}
				else
				{
					// child comment, so find parent
					var parentComment = commentTable[comment.ParentID] as IComment;
					if (parentComment != null)
					{
						// double check that this sub comment has not already been added
						if (parentComment.Comments.IndexOf(comment) == -1)
							parentComment.Comments.Add(comment);
					}
					else
					{
						// just add to the base to prevent an error
						_NestedComments.Add(comment);
					}
				}
			}
			// kill this data
			commentTable = null;
		}
		/// <summary>
		/// An unsorted List of categories.
		/// </summary>
		public StateList<ICategory> Categories
		{
			get {
				var retVal = new StateList<ICategory>();
				PostCategories.ForEach(x => {
					retVal.Add(x.Category);
				});
				return retVal;
			}
		}
		/// <summary>
		/// An unsorted collection of tags.
		/// </summary>
		public StateList<string> Tags
		{
			get
			{
				var retVal = new StateList<string>();
				PostTags.ForEach(x => {
					retVal.Add(x.Tag);
				});
				return retVal;
			}
		}
		/// <summary>
		/// Gets a collection of email addresses that is signed up for 
		/// comment notification on the specific post.
		/// </summary>
		public StateList<string> NotificationEmails
		{
			get {
				var retVal = new StateList<string>();
				PostNotifies.ForEach(x => {
					retVal.Add(x.NotifyAddress);
				});
				return retVal;
			}
		}
		/// <summary>
		/// Gets the previous post relative to this one based on time.
		/// <remarks>
		/// If this post is the oldest, then it returns null.
		/// </remarks>
		/// </summary>
		public Post Previous
		{
			get;
			set;
		}
		/// <summary>
		/// Gets the next post relative to this one based on time.
		/// <remarks>
		/// If this post is the newest, then it returns null.
		/// </remarks>
		/// </summary>
		public Post Next
		{
			get;
			set;
		}


		/// <summary>
		/// The absolute permanent link to the post.
		/// </summary>
		public Uri PermaLink
		{
			get { return new Uri(String.Format("{0}post.aspx?id={1}", Utils.AbsoluteWebRoot, Id)); }
		}

		/// <summary>
		/// A relative-to-the-site-root path to the post.
		/// Only for in-site use.
		/// </summary>
		public string RelativeLink
		{
			get
			{
				string slug = Utils.RemoveIllegalCharacters(Slug) + BlogSettings.Instance.FileExtension;

				if (BlogSettings.Instance.TimeStampPostLinks)
					return String.Format("{0}post/{1}{2}", Utils.RelativeWebRoot, DateCreated, slug);

				return String.Format("{0}post/{1}", Utils.RelativeWebRoot, slug);
			}
		}

		/// <summary>
		/// The absolute link to the post.
		/// </summary>
		public Uri AbsoluteLink
		{
			get { return Utils.ConvertToAbsolute(RelativeLink); }
		}

		/// <summary>
		/// The trackback link to the post.
		/// </summary>
		public Uri TrackbackLink
		{
			get { return new Uri(String.Format("{0}trackback.axd?id={1}", Utils.AbsoluteWebRoot, Id)); }
		}

		private static object _SyncRoot = new object();
		private static List<Post> _Posts;
		/// <summary>
		/// A sorted collection of all posts in the blog.
		/// Sorted by date.
		/// </summary>
		public static List<Post> Posts
		{
			get
			{
				if (_Posts == null)
				{
					lock (_SyncRoot)
					{
						if (_Posts == null)
						{
							_Posts = LoadAll();
							_Posts.TrimExcess();
							AddRelations();
						}
					}
				}

				return _Posts;
			}
		}

		/// <summary>
		/// Sets the Previous and Next properties to all posts.
		/// </summary>
		private static void AddRelations()
		{
			for (int i = 0; i < _Posts.Count; i++)
			{
				_Posts[i].Next = null;
				_Posts[i].Previous = null;
				if (i > 0)
					_Posts[i].Next = _Posts[i - 1];

				if (i < _Posts.Count - 1)
					_Posts[i].Previous = _Posts[i + 1];
			}
		}
		#endregion

		#region IPublishable Members
		/// <summary>
		/// Gets whether or not the post is visible or not.
		/// </summary>
		public bool IsVisible
		{
			get
			{
				var IsAuthenticated = Thread.CurrentPrincipal.Identity.IsAuthenticated;
				if (IsAuthenticated || (IsPublished.Value && DateCreated <= DateTime.Now.AddHours(BlogSettings.Instance.Timezone)))
					return true;

				return false;
			}
		}


		/// <summary>
		/// Gets whether a post is available to visitors not logged into the blog.
		/// </summary>
		public bool IsVisibleToPublic
		{
			get
			{
				return IsPublished.Value && DateCreated <= DateTime.Now.AddHours(BlogSettings.Instance.Timezone);
			}
		}
		#endregion

		#region IComparable<Post> Members

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the 
		/// objects being compared. The return value has the following meanings: 
		/// Value Meaning Less than zero This object is less than the other parameter.Zero 
		/// This object is equal to other. Greater than zero This object is greater than other.
		/// </returns>
		public int CompareTo(Post other)
		{
			return other.DateCreated.GetValueOrDefault().CompareTo(this.DateCreated.GetValueOrDefault());
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs before a new comment is added.
		/// </summary>
		public static event EventHandler<CancelEventArgs> AddingComment;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		protected virtual void OnAddingComment(IComment comment, CancelEventArgs e)
		{
			if (AddingComment != null)
			{
				AddingComment(comment, e);
			}
		}

		/// <summary>
		/// Occurs when a comment is added.
		/// </summary>
		public static event EventHandler<EventArgs> CommentAdded;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		protected virtual void OnCommentAdded(IComment comment)
		{
			if (CommentAdded != null)
			{
				CommentAdded(comment, new EventArgs());
			}
		}

		/// <summary>
		/// Occurs before a new comment is updated.
		/// </summary>
		public static event EventHandler<CancelEventArgs> UpdatingComment;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		protected virtual void OnUpdatingComment(IComment comment, CancelEventArgs e)
		{
			if (UpdatingComment != null)
			{
				UpdatingComment(comment, e);
			}
		}

		/// <summary>
		/// Occurs when a comment is updated.
		/// </summary>
		public static event EventHandler<EventArgs> CommentUpdated;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		protected virtual void OnCommentUpdated(IComment comment)
		{
			if (CommentUpdated != null)
			{
				CommentUpdated(comment, new EventArgs());
			}
		}

		/// <summary>
		/// Occurs before comment is removed.
		/// </summary>
		public static event EventHandler<CancelEventArgs> RemovingComment;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		protected virtual void OnRemovingComment(IComment comment, CancelEventArgs e)
		{
			if (RemovingComment != null)
			{
				RemovingComment(comment, e);
			}
		}

		/// <summary>
		/// Occurs when a comment has been removed.
		/// </summary>
		public static event EventHandler<EventArgs> CommentRemoved;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		protected virtual void OnCommentRemoved(IComment comment)
		{
			if (CommentRemoved != null)
			{
				CommentRemoved(comment, new EventArgs());
			}
		}

		/// <summary>
		/// Occurs when a visitor rates the post.
		/// </summary>
		public static event EventHandler<EventArgs> Rated;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		protected virtual void OnRated(Post post)
		{
			if (Rated != null)
			{
				Rated(post, new EventArgs());
			}
		}

		/// <summary>
		/// Occurs when the post is being served to the output stream.
		/// </summary>
		public static event EventHandler<ServingEventArgs> Serving;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		public static void OnServing(Post post, ServingEventArgs arg)
		{
			if (Serving != null)
			{
				Serving(post, arg);
			}
		}

		/// <summary>
		/// Raises the Serving event
		/// </summary>
		public void OnServing(ServingEventArgs eventArgs)
		{
			if (Serving != null)
			{
				Serving(this, eventArgs);
			}
		}

		public static event EventHandler<SavedEventArgs> Saved1;
        public static event EventHandler<SavedEventArgs> Saving;
        public override IBusinessObject Save()
		{
			var action = new SavedEventArgs(SaveAction.None);
			if (this.Status.IsDeleted) {
				action.Action = SaveAction.Delete;
			}
			else if (this.Status.IsNew)
			{
				action.Action = SaveAction.Insert;
			}
			else if(this.Status.IsDirty) {
				action.Action = SaveAction.Update;
			};

			if (Saving != null) {
				Saving(this, action);
			}

			var bo=base.Save();
			
			if (Saved1 != null && this.Status.IsValid()) {
				Saved1(this, action);
			}
            return bo;
		}
		#endregion

		#region base overrides
		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return Title;
		}
		#endregion

		#region factory methods
		/// <summary>
		/// Returs a post based on the specified id.
		/// </summary>
		public static Post GetPost(Guid id)
		{
			return GetPost(id, true);
		}
		public static Post GetPost(string title) { 
			title = Utils.RemoveIllegalCharacters(title);
			var item = Broker.GetBusinessObject<Post>(string.Format("Title='{0}'", title));
			return item;
		}
		public static Post GetPost(Guid id, bool loadNeighbor) {
			var item = Broker.GetBusinessObject<Post>(string.Format("Id='{0}'", id));
			if (null == item) return null;
			if (loadNeighbor)
			{
				//previous post
				var tempItems = GetPosts(item.DateCreated.Value, true, 1);
				if (tempItems != null && tempItems.Count > 0) item.Previous = tempItems[0];
				//next post
				tempItems = GetPosts(item.DateCreated.Value, false, 1);
				if (tempItems != null && tempItems.Count > 0) item.Next = tempItems[0];
			}
			return item;
		}
		public static List<Post> GetPosts(DateTime dateFlag,bool isBefore,int count) { 
			var criteriaStr=string.Format("DateCreated {0} '{1}' ",
				isBefore?"<":">",
				dateFlag
				);
			var tempInt=0;
			var items = Broker.GetBusinessObjectCollection<Post>(criteriaStr, "", 0, count, out tempInt);
			return items.ToList();
		}
		/// <summary>
		/// TODO:Get headline posts
		/// </summary>
		/// <param name="isHeadline"></param>
		/// <param name="count"></param>
		/// <param name="total"></param>
		/// <returns></returns>
		public static List<Post> GetPosts(bool isHeadline,int count,out int total) {
			total = 0;
			var retVal = default(List<Post>);
			var criteriaStr = "Id is not null";
			if (isHeadline) {
				criteriaStr += " and IsHeadline=1";
			}
			retVal = Broker.GetBusinessObjectCollection<Post>(criteriaStr, " DateModified Desc", 0, count, out total).ToList();
			return retVal;
		}
		/// <summary>
		/// Returns all posts in the specified category
		/// </summary>
		public static List<Post> GetPostsByCategory(Guid categoryId)
		{
			var items = Broker.GetBusinessObjectCollection<PostCategory>(string.Format("CategoryID='{0}'", categoryId));
			if (items == null || items.Count == 0) return null;
			var retVal = from item in items
						 select item.Post;
			return retVal.ToList();
		}
		/// <summary>
		/// Checks to see if the specified title has already been used
		/// by another post.
		/// <remarks>
		/// Titles must be unique because the title is part of the URL.
		/// </remarks>
		/// </summary>
		public static bool IsTitleUnique(string title)
		{
			var item = GetPost(title);
			return item == null;
		}

		/// <summary>
		/// Returns all posts written by the specified author.
		/// </summary>
		public static List<Post> GetPostsByAuthor(string author)
		{
			author = Utils.RemoveIllegalCharacters(author);
			var items = Broker.GetBusinessObjectCollection<Post>(string.Format("Author='{0}'",author));
			return items.ToList();
		}

		/// <summary>
		/// Returns all posts tagged with the specified tag.
		/// </summary>
		public static List<Post> GetPostsByTag(string tag)
		{
			tag = Utils.RemoveIllegalCharacters(tag);
			var items = Broker.GetBusinessObjectCollection<PostTag>(string.Format("Tag='{0}'",tag));
			if (items == null || items.Count == 0) return null;

			var retVal = from item in items
						 select item.Post;
			return retVal.ToList();

		}

		/// <summary>
		/// Returns all posts published between the two dates.
		/// </summary>
		public static List<Post> GetPostsByDate(DateTime dateFrom, DateTime dateTo)
		{
			var items = Broker.GetBusinessObjectCollection<Post>(string.Format("DateCreated>='{0}' AND DateCreated<='{1}'", dateFrom, dateTo));
			return items.ToList();
		}
		public static List<Post> LoadAll() {
			return Broker.GetBusinessObjectCollection<Post>("Id is not null").ToList();
		}
		#endregion

		#region biz methods

		/// <summary>
		/// Adds a rating to the post.
		/// </summary>
		public void Rate(int rating)
		{
			if (Raters > 0)
			{
				float total = Raters.Value * Rating.Value;
				total += rating;
				Raters++;
				Rating = (float)(total / Raters);
			}
			else
			{
				Raters = 1;
				Rating = rating;
			}

			Save();
			OnRated(this);
		}

		/// <summary>
		/// Imports Post (without all standard saving routines
		/// </summary>
		public void Import()
		{
			Save();
		}

		/// <summary>
		/// Force reload of all posts
		/// </summary>
		public static void Reload()
		{
			_Posts = LoadAll();
			_Posts.Sort();
			AddRelations();
		}

		/// <summary>
		/// Adds a comment to the collection and saves the post.
		/// </summary>
		/// <param name="comment">The comment to add to the post.</param>
		public void AddComment(PostComment comment)
		{
			CancelEventArgs e = new CancelEventArgs();
			OnAddingComment(comment, e);
			if (!e.Cancel)
			{
				Comments.Add(comment);
				comment.Save();
				OnCommentAdded(comment);

				if (comment.IsApproved.Value)
					SendNotifications(comment);
			}
		}

		/// <summary>
		/// Updates a comment in the collection and saves the post.
		/// </summary>
		/// <param name="comment">The comment to update in the post.</param>
		public void UpdateComment(PostComment comment)
		{
			CancelEventArgs e = new CancelEventArgs();
			OnUpdatingComment(comment, e);
			if (!e.Cancel)
			{
				int inx = Comments.IndexOf(comment);

				Comments[inx] = comment as IComment;

				DateModified = DateTime.Now;

				comment.Save();
				Save();

				OnCommentUpdated(comment);
			}
		}

		/// <summary>
		/// Imports a comment to comment collection and saves.  Does not
		/// notify user or run extension events.
		/// </summary>
		/// <param name="comment">The comment to add to the post.</param>
		public void ImportComment(PostComment comment)
		{
			Comments.Add(comment);
			comment.Save();

		}

		/// <summary>
		/// Sends a notification to all visitors  that has registered
		/// to retrieve notifications for the specific post.
		/// </summary>
		private void SendNotifications(PostComment comment)
		{
			if (NotificationEmails.Count == 0 || comment.IsApproved == false)
				return;

			foreach (string email in NotificationEmails)
			{
				if (email != comment.Email)
				{
					// Intentionally using AbsoluteLink instead of PermaLink so the "unsubscribe-email" QS parameter
					// isn't dropped when post.aspx.cs does a 301 redirect to the RelativeLink, before the unsubscription
					// process takes place.
					string unsubscribeLink = AbsoluteLink.ToString();
					unsubscribeLink += String.Format("{0}unsubscribe-email={1}", (unsubscribeLink.Contains("?") ? "&" : "?"), HttpUtility.UrlEncode(email));

					MailMessage mail = new MailMessage();
					mail.From = new MailAddress(BlogSettings.Instance.Email, BlogSettings.Instance.Name);
					mail.Subject = String.Format("New comment on {0}", Title);
					mail.Body = String.Format("Comment by {0}<br /><br />", comment.Author);
					mail.Body += String.Format("{0}<br /><br />", comment.Content.Replace(Environment.NewLine, "<br />"));
					mail.Body += string.Format("<a href=\"{0}\">{1}</a>", PermaLink + "#id_" + comment.Id, Title);
					mail.Body += "<br /><br /><hr />";
					mail.Body += string.Format("<a href=\"{0}\">{1}</a>", unsubscribeLink, Utils.Translate("commentNotificationUnsubscribe"));

					mail.To.Add(email);
                    SystemX.Utils.SendMailMessageAsync(mail);
				}
			}
		}

		/// <summary>
		/// Removes a comment from the collection and saves the post.
		/// </summary>
		/// <param name="comment">The comment to remove from the post.</param>
		public void RemoveComment(PostComment comment)
		{
			CancelEventArgs e = new CancelEventArgs();
			OnRemovingComment(comment, e);
			if (!e.Cancel)
			{
				Comments.Remove(comment);
				comment.MarkForDelete();
				comment.Save();
				OnCommentRemoved(comment);
				comment = null;
			}
		}

		/// <summary>
		/// Approves a Comment for publication.
		/// </summary>
		/// <param name="comment">The Comment to approve</param>
		public void ApproveComment(PostComment comment)
		{
			CancelEventArgs e = new CancelEventArgs();
			PostComment.OnApproving(comment, e);
			
			if (!e.Cancel)
			{
				int inx = Comments.IndexOf(comment);
				Comments[inx].IsApproved = true;
				this.DateModified = comment.DateCreated;
				comment.Save();
				Save();
				PostComment.OnApproved(comment);
				SendNotifications(comment);
			}
		}

		/// <summary>
		/// Approves all the comments in a post.  Included to save time on the approval process.
		/// </summary>
		public void ApproveAllComments()
		{
			foreach (PostComment comment in Comments)
			{
				ApproveComment(comment);
			}
		}

		#endregion
	}
}
