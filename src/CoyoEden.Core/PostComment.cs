
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using SystemX.LunaAtom;
using System.Xml.Serialization;
using SystemX.Web;
using System.Collections.Generic;
using System.ComponentModel;
using CoyoEden.Core.Infrastructure;
using SystemX.Infrastructure;

namespace CoyoEden.Core
{
    
    public partial class PostComment:IComment
    {
		#region IPublishable Members

		public string Title
		{
			get {
				return String.Format("{0} on {1}", Author, Parent.Title);
			}
		}

		public string RelativeLink
		{
			get { return Parent.RelativeLink.ToString() + "#id_" + Id; }
		}

		public Uri AbsoluteLink
		{
			get { return new Uri(Parent.AbsoluteLink + "#id_" + Id); }
		}

		public string Description
		{
			get { return string.Empty; }
		}


		public bool IsVisible
		{
			get { return this.IsApproved.GetValueOrDefault(); }
		}

		public bool IsVisibleToPublic
		{
			get { return this.IsApproved.GetValueOrDefault(); }
		}
		public IPublishable Parent
		{
			get;
			set;
		}
		///<summary>
		/// Process that approved or rejected comment
		///</summary>
		[XmlElement]
		public string ModeratedBy
		{
			get;
			set;
		}
		/// <summary>
		/// Abbriviated content
		/// </summary>
		public string Teaser
		{
			get
			{
				string ret = SystemX.Utils.StripHtml(Content).Trim();
				if (ret.Length > 120)
					return String.Format("{0} ...", ret.Substring(0, 116));
				return ret;
			}
		}
		public List<IComment> Comments { get; set; }

		Uri IComment.Website
		{
			get
			{
				return new Uri(Website);
			}
			set
            {
            	Website = value.ToString();
            }
		}
		public DateTime? DateModified
		{
			get { return DateCreated; }
		}
		public StateList<ICategory> Categories
		{
			get { return null; }
		}

		public bool? IsPublished
		{
			get {
				return IsApproved;
			}
		}
		#endregion

		#region IComparable<IComment> Members

		public int CompareTo(IComment other)
		{
			return DateCreated.Value.CompareTo(other.DateCreated.Value);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the post is being served to the output stream.
		/// </summary>
		public static event EventHandler<ServingEventArgs> Serving;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		public static void OnServing(IComment comment, ServingEventArgs arg)
		{
			if (Serving != null)
			{
				Serving(comment, arg);
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

		/// <summary>
		/// Occurs just before a comment is approved by the comment moderator.
		/// </summary>
		public static event EventHandler<CancelEventArgs> Approving;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		internal static void OnApproving(IComment comment, CancelEventArgs e)
		{
			if (Approving != null)
			{
				Approving(comment, e);
			}
		}

		/// <summary>
		/// Occurs after a comment is approved by the comment moderator.
		/// </summary>
		public static event EventHandler<EventArgs> Approved;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		internal static void OnApproved(IComment comment)
		{
			if (Approved != null)
			{
				Approved(comment, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Occurs when the page is being attacked by robot spam.
		/// </summary>
		public static event EventHandler<EventArgs> SpamAttack;
		/// <summary>
		/// Raises the SpamAttack event in a safe way
		/// </summary>
		public static void OnSpamAttack()
		{
			if (SpamAttack != null)
			{
				SpamAttack(null, new EventArgs());
			}
		}

		#endregion

	}
}
