
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using SystemX.Infrastructure;
using SystemX;
using System.Threading;
using System.Collections.Generic;
using CoyoEden.Core.Providers;
using CoyoEden.Core.Infrastructure;

namespace CoyoEden.Core
{
	public partial class Page : IPublishable
    {
		public Page() {
			Id=GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
			DateCreated = DateTime.Now;
		}

		public bool HasParentPage {
			get
			{
				return ParentID.HasValue;
			}
		}
		public bool HasChildPages {
			get
			{
				return SubPages.Count>0;
			}
		}

		/// <summary>
		/// Gets whether or not this page is visible to visitors not logged into the blog.
		/// </summary>
		/// <value></value>
		public bool IsVisibleToPublic
		{
			get { return IsPublished.GetValueOrDefault(); }
		}
		/// <summary>
		/// Gets whether or not this page should be shown
		/// </summary>
		/// <value></value>
		public bool IsVisible
		{
			get { return Thread.CurrentPrincipal.Identity.IsAuthenticated || IsVisibleToPublic; }
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
				return String.Format("{0}page/{1}", Utils.RelativeWebRoot, slug);
			}
		}

		/// <summary>
		/// The absolute link to the post.
		/// </summary>
		public Uri AbsoluteLink
		{
			get { return Utils.ConvertToAbsolute(RelativeLink); }
		}

		private static object _SyncRoot = new object();
		private static List<Page> _Pages;
		/// <summary>
		/// Gets an unsorted list of all pages.
		/// </summary>
		public static List<Page> Pages
		{
			get
			{
				if (_Pages == null)
				{
					lock (_SyncRoot)
					{
						if (_Pages == null)
						{
							_Pages = LoadAll();
							_Pages.Sort(( p1,  p2)=> { return String.Compare(p1.Title, p2.Title); });
						}
					}
				}

				return _Pages;
			}
		}

		#region IPublishable Members
		public string Author
		{
			get
			{
				return BlogSettings.Instance.AuthorName;
			}
		}
		public StateList<ICategory> Categories
		{
			get { return null; }
		}
		#endregion

		#region factory methods
		/// <summary>
		/// Returns a page based on the specified id.
		/// </summary>
		public static Page GetPage(Guid id)
		{
			return Broker.GetBusinessObject<Page>(string.Format("Id='{0}'",id));
		}


		/// <summary>
		/// Returns the front page if any is available.
		/// </summary>
		public static Page GetFrontPage()
		{
			return Broker.GetBusinessObject<Page>("IsFrontPage=1");
		}
		public static List<Page> LoadAll() {
			return Broker.GetBusinessObjectCollection<Page>("Id is not null");
		}
		#endregion

		#region Events

		/// <summary>
		/// Occurs when the page is being served to the output stream.
		/// </summary>
		public static event EventHandler<ServingEventArgs> Serving;
		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		public static void OnServing(Page page, ServingEventArgs arg)
		{
			if (Serving != null)
			{
				Serving(page, arg);
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
		public override void Save()
		{
			var action = new SavedEventArgs(SaveAction.None);
			if (this.Status.IsDeleted)
			{
				action.Action = SaveAction.Delete;
			}
			else if (this.Status.IsNew)
			{
				action.Action = SaveAction.Insert;
			}
			else if (this.Status.IsDirty)
			{
				action.Action = SaveAction.Update;
			};
			base.Save();
			if (Saved1 != null && this.Status.IsValid())
			{
				Saved1(this, action);
			}
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

	}
}
