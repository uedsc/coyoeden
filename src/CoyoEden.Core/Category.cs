
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using SystemX.LunaAtom;
using System.Collections.Generic;
using CoyoEden.Core.Providers;
using System.Linq;
using SystemX.Web;
using SystemX.Infrastructure;
using CoyoEden.Core.Infrastructure;
using SystemX.LunaBase;
    
namespace CoyoEden.Core
{

    public partial class Category:ICategory
	{

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Category"/> class.
		/// </summary>
		public Category()
		{
			Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="description"></param>
		public Category(string title, string description):this()
		{
			Name = title;
			Description = description;
		}

		#endregion

		#region properties
		/// <summary>
		/// Gets the full title with Parent names included
		/// </summary>
		private string CompleteTitle()
		{
			if (null==Parent)
				return Name;
			else
			{
				string temp = String.Format("{0} - {1}", Parent.CompleteTitle(),Name);
				return temp;
			}
		}
		public string FullName {
			get
			{
				return CompleteTitle();
			}
		}
		/// <summary>
		/// Gets the relative link to the page displaying all posts for this category.
		/// </summary>
		public string RelativeLink
		{
			get
			{
				return String.Format("{0}category/{1}{2}", Utils.RelativeWebRoot, Utils.RemoveIllegalCharacters(Name), BlogSettings.Instance.FileExtension);
			}
		}

		/// <summary>
		/// Gets the absolute link to the page displaying all posts for this category.
		/// </summary>
		public Uri AbsoluteLink
		{
			get { return Utils.ConvertToAbsolute(RelativeLink); }
		}

		/// <summary>
		/// Gets the relative link to the feed for this category's posts.
		/// </summary>
		public string FeedRelativeLink
		{
			get
			{
				return String.Format("{0}category/feed/{1}{2}", Utils.RelativeWebRoot, Utils.RemoveIllegalCharacters(Name), BlogSettings.Instance.FileExtension);
			}
		}

		/// <summary>
		/// Gets the absolute link to the feed for this category's posts.
		/// </summary>
		public Uri FeedAbsoluteLink
		{
			get { return Utils.ConvertToAbsolute(FeedRelativeLink); }
		}

		#endregion

		#region factory methods
		/// <summary>
		/// Returns a category based on the specified id.
		/// </summary>
		public static Category GetCategory(Guid id)
		{
			return Categories.SingleOrDefault(x=>x.Id.GetValueOrDefault()==id);
		}


		private static object _SyncRoot = new object();
		private static List<Category> _Categories;
		/// <summary>
		/// Gets an unsorted list of all Categories.
		/// </summary>
		public static List<Category> Categories
		{
			get
			{
				if (_Categories == null)
				{
					lock (_SyncRoot)
					{
						if (_Categories == null)
						{
							_Categories = LoadAll();
							_Categories.Sort();
						}
					}
				}

				return _Categories;
			}
		}
		#endregion

		#region biz methods
		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return CompleteTitle();
		}
		/// <summary>
		/// Get all posts in this category.
		/// </summary>
		public List<Post> Posts
		{
			get { return Post.GetPostsByCategory(Id.Value); }
		}
		#endregion

		#region events

		public static event EventHandler<SavedEventArgs> Saved1;
        public override IBusinessObject Save()
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
			var bo=base.Save();
			if (Saved1 != null && this.Status.IsValid())
			{
				Saved1(this, action);
			}
            return bo;
		}
		#endregion

		#region IComparable<ICategory> Members

		public int CompareTo(ICategory other)
		{
			return this.FullName.CompareTo(other.FullName);
		}

		#endregion

		#region Factory methods
		public static List<Category> LoadAll() {
			return Broker.GetBusinessObjectCollection<Category>("Id is not null").ToList();
		}
		#endregion
	}
}
