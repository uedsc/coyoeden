// Author:					Joe Audette
// Created:				    2004-08-14
// Last Modified:			2010-01-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using log4net;
using Cynthia.Data;

namespace Cynthia.Business
{
    /// <summary>
    /// Represents a blog post
    /// </summary>
    public class Blog : IIndexableContent
    {
        #region Constructors

        public Blog()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Blog"/> class.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public Blog(int itemId)
        {
            if (itemId > 0)
            {
                GetBlog(itemId);
            }

        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(Blog));

        private int itemID = -1;
        private Guid blogGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private int moduleID = -1;
        private string userName = string.Empty;
        private string title = string.Empty;

        private string location = string.Empty;

        //aliased as Abstract
        private string excerpt = string.Empty;

        private string description = string.Empty;
        private DateTime startDate = DateTime.UtcNow;
        private bool isPublished = true;
        private bool isInNewsletter = true;
        private bool includeInFeed = true;
        private string category = string.Empty;
        private int allowCommentsForDays = 60;
        private Guid userGuid = Guid.Empty;
        private Guid lastModUserGuid = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;
        private string itemUrl = string.Empty;
        private string previousPostUrl = string.Empty;
        private string previousPostTitle = string.Empty;
        private string nextPostUrl = string.Empty;
        private string nextPostTitle = string.Empty;
        private int commentCount = 0;

        private string metaKeywords = string.Empty;
        private string metaDescription = string.Empty;
        private string compiledMeta = string.Empty;

        private int siteId = -1;
        private string searchIndexPath = string.Empty;


        
        

        #endregion

        #region Public Properties

        public Guid BlogGuid
        {
            get { return blogGuid; }

        }

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        public int ItemId
        {
            get { return itemID; }
        }

        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public Guid LastModUserGuid
        {
            get { return lastModUserGuid; }
            set { lastModUserGuid = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public string Excerpt
        {
            get { return excerpt; }
            set { excerpt = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string MetaKeywords
        {
            get { return metaKeywords; }
            set { metaKeywords = value; }
        }

        public string MetaDescription
        {
            get { return metaDescription; }
            set { metaDescription = value; }
        }

        public string CompiledMeta
        {
            get { return compiledMeta; }
            set { compiledMeta = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public bool IsPublished
        {
            get { return isPublished; }
            set { isPublished = value; }
        }

        public bool IsInNewsletter
        {
            get { return isInNewsletter; }
            set { isInNewsletter = value; }
        }

        public bool IncludeInFeed
        {
            get { return includeInFeed; }
            set { includeInFeed = value; }
        }

        public int AllowCommentsForDays
        {
            get { return allowCommentsForDays; }
            set { allowCommentsForDays = value; }
        }

        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }

        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        public string ItemUrl
        {
            get { return itemUrl; }
            set { itemUrl = value; }
        }

        public string PreviousPostUrl
        {
            get { return previousPostUrl; }

        }

        public string NextPostUrl
        {
            get { return nextPostUrl; }

        }

        public string PreviousPostTitle
        {
            get { return previousPostTitle; }

        }

        public string NextPostTitle
        {
            get { return nextPostTitle; }

        }

        public int CommentCount
        {
            get { return commentCount; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public string SearchIndexPath
        {
            get { return searchIndexPath; }
            set { searchIndexPath = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the blog.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        private void GetBlog(int itemId)
        {
            using (IDataReader reader = DBBlog.GetSingleBlog(itemId, DateTime.UtcNow))
            {
                if (reader.Read())
                {
                    this.itemID = int.Parse(reader["ItemID"].ToString(), CultureInfo.InvariantCulture);
                    this.moduleID = int.Parse(reader["ModuleID"].ToString(), CultureInfo.InvariantCulture);
                    this.userName = reader["Name"].ToString();
                    this.title = reader["Heading"].ToString();
                    this.excerpt = reader["Abstract"].ToString();
                    this.description = reader["Description"].ToString();

                    this.metaKeywords = reader["MetaKeywords"].ToString();
                    this.metaDescription = reader["MetaDescription"].ToString();

                    this.startDate = Convert.ToDateTime(reader["StartDate"].ToString());

                    // this is to support dbs that don't have bit data type
                    //string inNews = reader["IsInNewsletter"].ToString();
                    //this.isInNewsletter = (inNews == "True" || inNews == "1");

                    this.isInNewsletter = Convert.ToBoolean(reader["IsInNewsletter"]);

                    //string inFeed = reader["IncludeInFeed"].ToString();
                    //this.includeInFeed = (inFeed == "True" || inFeed == "1");

                    this.includeInFeed = Convert.ToBoolean(reader["IncludeInFeed"]);

                    if (reader["AllowCommentsForDays"] != DBNull.Value)
                    {
                        this.allowCommentsForDays = Convert.ToInt32(reader["AllowCommentsForDays"]);
                    }

                    this.blogGuid = new Guid(reader["BlogGuid"].ToString());
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    this.location = reader["Location"].ToString();
                    this.compiledMeta = reader["CompiledMeta"].ToString();

                    if (reader["CreatedDate"] != DBNull.Value)
                    {
                        this.createdUtc = Convert.ToDateTime(reader["CreatedDate"]);
                    }

                    if (reader["LastModUtc"] != DBNull.Value)
                    {
                        this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    }

                    string var = reader["UserGuid"].ToString();
                    if (var.Length == 36) this.userGuid = new Guid(var);

                    var = reader["LastModUserGuid"].ToString();
                    if (var.Length == 36) this.lastModUserGuid = new Guid(var);

                    itemUrl = reader["ItemUrl"].ToString();

                    previousPostUrl = reader["PreviousPost"].ToString();
                    previousPostTitle = reader["PreviousPostTitle"].ToString();
                    nextPostUrl = reader["NextPost"].ToString();
                    nextPostTitle = reader["NextPostTitle"].ToString();

                    commentCount = Convert.ToInt32(reader["CommentCount"]);

                    isPublished = Convert.ToBoolean(reader["IsPublished"]);
                }

            }

        }

        /// <summary>
        /// Creates a new blog
        /// </summary>
        /// <returns>true if successful</returns>
        private bool Create()
        {
            int newID = 0;
            blogGuid = Guid.NewGuid();
            createdUtc = DateTime.UtcNow;

            newID = DBBlog.AddBlog(
                this.blogGuid,
                this.moduleGuid,
                this.moduleID,
                this.userName,
                this.title,
                this.excerpt,
                this.description,
                this.startDate,
                this.isInNewsletter,
                this.includeInFeed,
                this.allowCommentsForDays,
                this.location,
                this.userGuid,
                this.createdUtc,
                this.itemUrl,
                this.metaKeywords,
                this.metaDescription,
                this.compiledMeta,
                this.isPublished);

            this.itemID = newID;

            bool result = (newID > 0);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
        private bool Update()
        {
            this.lastModUtc = DateTime.UtcNow;

            bool result = DBBlog.UpdateBlog(
                this.moduleID,
                this.itemID,
                this.userName,
                this.title,
                this.excerpt,
                this.description,
                this.startDate,
                this.isInNewsletter,
                this.includeInFeed,
                this.allowCommentsForDays,
                this.location,
                this.lastModUserGuid,
                this.lastModUtc,
                this.itemUrl,
                this.metaKeywords,
                this.metaDescription,
                this.compiledMeta,
                this.isPublished);

            //IndexHelper.IndexItem(this);
            ContentChangedEventArgs e = new ContentChangedEventArgs();
            OnContentChanged(e);

            return result;
        }


        #endregion

        #region Public Methods

        public void CreateHistory(Guid siteGuid)
        {
            if (this.blogGuid == Guid.Empty) { return; }

            Blog currentVersion = new Blog(this.itemID);
            if (currentVersion.Description == this.Description) { return; }

            ContentHistory history = new ContentHistory();
            history.ContentGuid = currentVersion.BlogGuid;
            history.Title = currentVersion.Title;
            history.ContentText = currentVersion.Description;
            history.SiteGuid = siteGuid;
            history.UserGuid = currentVersion.LastModUserGuid;
            history.CreatedUtc = currentVersion.LastModUtc;
            history.Save();

        }


        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if (this.itemID > 0)
            {
                return Update();
            }
            else
            {
                return Create();
            }


        }

        public bool Delete()
        {
            DBBlog.DeleteItemCategories(itemID);
            DBBlog.DeleteAllCommentsForBlog(itemID);
            DBBlog.UpdateCommentStats(this.moduleID);
            bool result = DBBlog.DeleteBlog(this.itemID);
            DBBlog.UpdateEntryStats(this.moduleID);

            ContentChangedEventArgs e = new ContentChangedEventArgs();
            e.IsDeleted = true;
            OnContentChanged(e);

            return result;
        }




        #endregion


        #region Static Methods
        /// <summary>
        /// Gets the blogs.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="endDate">The end date.</param>
		/// <param name="categoryIds">categories specified</param>
        /// <returns></returns>
		public static IDataReader GetBlogs(int moduleId, DateTime beginDate, params int[] categoryIds)
        {
			if (categoryIds != null && categoryIds.Length > 0 && categoryIds[0] == -1)
				return DBBlog.GetBlogs(moduleId, beginDate, DateTime.UtcNow);
            return DBBlog.GetBlogs(moduleId, beginDate, DateTime.UtcNow,categoryIds);
        }

        public static IDataReader GetPage(
            int moduleId,
            DateTime beginDate,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBBlog.GetPage(moduleId, beginDate, DateTime.UtcNow, pageNumber, pageSize, out totalPages);
        }


        public static IDataReader GetBlogsForSiteMap(int siteId)
        {
            return DBBlog.GetBlogsForSiteMap(siteId, DateTime.UtcNow);
        }

        /// <summary>
        /// Gets unpublished blog posts
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static IDataReader GetDrafts(int moduleId)
        {
            return DBBlog.GetDrafts(moduleId);
        }

        public static int CountOfDrafts(int moduleId)
        {
            int result = 0;

            using (IDataReader reader = GetDrafts(moduleId))
            {
                while (reader.Read())
                {
                    result += 1;
                }
            }


            return result;

        }

        public static bool DeleteByModule(int moduleId)
        {
            return DBBlog.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBBlog.DeleteBySite(siteId);
        }


        public static IDataReader GetBlogStats(int moduleId)
        {
            return DBBlog.GetBlogStats(moduleId);
        }

        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId)
        {
            return DBBlog.GetBlogEntriesByMonth(month, year, moduleId, DateTime.UtcNow);
        }

        public static IDataReader GetBlogMonthArchive(int moduleId)
        {
            return DBBlog.GetBlogMonthArchive(moduleId, DateTime.UtcNow);
        }

        public static IDataReader GetSingleBlog(int itemId)
        {
            return DBBlog.GetSingleBlog(itemId, DateTime.UtcNow);
        }



        //public static bool DeleteBlog(int itemID) 
        //{
        //    //TODO: make instance method to support ContentChanged event

        //    Blog blog = new Blog(itemID);
        //    bool result = dbBlog.Blog_DeleteBlog(itemID);
        //    if (result)
        //    {
        //        result = dbBlog.Blog_DeleteItemCategories(itemID);
        //        //IndexHelper.RemoveIndexItem(blog.ModuleID, itemID);
        //    }

        //    return result;
        //}

        public static IDataReader GetBlogComments(int moduleId, int itemId)
        {
            return DBBlog.GetBlogComments(moduleId, itemId);
        }

        public static DataTable GetBlogCommentsTable(int moduleId, int itemId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Comment", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));

            using (IDataReader reader = DBBlog.GetBlogComments(moduleId, itemId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["Comment"] = reader["Comment"];
                    row["Name"] = reader["Name"];

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        public static bool AddBlogComment(
            int moduleId,
            int itemId,
            String name,
            String title,
            String url,
            String comment,
            DateTime dateCreated)
        {
            if (name == null)
            {
                name = "unknown";
            }
            if (name.Length < 1)
            {
                name = "unknown";
            }

            if ((title != null) && (url != null) && (comment != null))
            {
                if (title.Length > 100)
                {
                    title = title.Substring(0, 100);
                }

                if (name.Length > 100)
                {
                    name = name.Substring(0, 100);
                }

                if (url.Length > 200)
                {
                    url = url.Substring(0, 200);
                }

                return DBBlog.AddBlogComment(
                    moduleId,
                    itemId,
                    name,
                    title,
                    url,
                    comment,
                    dateCreated);
            }

            return false;
        }

        public static bool DeleteBlogComment(int commentId)
        {
            return DBBlog.DeleteBlogComment(commentId);
        }


        public static int AddBlogCategory(int moduleId, string category)
        {
            return DBBlog.AddBlogCategory(moduleId, category);
        }

        public static bool UpdateBlogCategory(
            int categoryId,
            string category)
        {
            return DBBlog.UpdateBlogCategory(categoryId, category);

        }


        public static bool DeleteCategory(int categoryId)
        {
            return DBBlog.DeleteCategory(categoryId);
        }


        public static IDataReader GetCategories(int moduleId)
        {
            return DBBlog.GetCategories(moduleId);
        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            return DBBlog.GetCategoriesList(moduleId);
        }


        public static int AddItemCategory(
            int itemId,
            int categoryId)
        {
            return DBBlog.AddBlogItemCategory(itemId, categoryId);
        }

        public static bool DeleteItemCategories(int itemId)
        {
            return DBBlog.DeleteItemCategories(itemId);
        }

        public static IDataReader GetItemCategories(int itemId)
        {
            return DBBlog.GetBlogItemCategories(itemId);
        }

        public static IDataReader GetEntriesByCategory(int moduleId, int categoryId)
        {
            return DBBlog.GetEntriesByCategory(moduleId, categoryId, DateTime.UtcNow);
        }

        public static IDataReader GetCategory(int categoryId)
        {
            return DBBlog.GetCategory(categoryId);
        }



        public static DataTable GetBlogsByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("CommentCount", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("ItemUrl", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("StartDate", typeof(DateTime));
            dataTable.Columns.Add("MetaDescription", typeof(string));
            dataTable.Columns.Add("MetaKeywords", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            using (IDataReader reader = DBBlog.GetBlogsByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["CommentCount"] = reader["CommentCount"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Title"] = reader["Title"];
                    row["ItemUrl"] = reader["ItemUrl"];
                    row["Description"] = reader["Description"];
                    row["StartDate"] = Convert.ToDateTime(reader["StartDate"]);
                    row["MetaDescription"] = reader["MetaDescription"];
                    row["MetaKeywords"] = reader["MetaKeywords"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }



        #endregion

        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        protected void OnContentChanged(ContentChangedEventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, e);
            }
        }




        #endregion




    }
}
