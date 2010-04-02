// Author:					Joe Audette
// Created:				    2004-09-19
// Last Modified:			2010-03-20
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Configuration;
using System.Data;
using log4net;
using Cynthia.Data;

namespace Cynthia.Business
{
    /// <summary>
    /// Encapsulates a thread and post
    /// </summary>
    public class ForumThread : IIndexableContent
    {

        #region Constructors

        public ForumThread()
        {

        }

        public ForumThread(int threadId)
        {
            GetThread(threadId);
        }

        public ForumThread(int threadId, int postId)
        {
            GetThread(threadId);
            GetPost(postId);
        }

        #endregion


        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(ForumThread));


        private int threadID = -1;
        private int forumID = -1;
        private int moduleID = -1;
        private int origForumID = 0;
        private DateTime threadDate = DateTime.UtcNow;
        private string startedBy = string.Empty;
        private int startedByUserID = -1;
        private string subject = string.Empty;
        private int totalViews = 0;
        private int totalReplies = 0;
        private bool isLocked = false;
        //sorted in db by SortOrder, ItemID
        private int sortOrder = 100;
        private int forumSequence = 1;
        private DateTime mostRecentPostDate = DateTime.UtcNow;
        private string mostRecentPostUser = string.Empty;
        private int mostRecentPostUserID = 0;
        private int postsPerPage = 0;
        private int totalPages = 1;

        //post properties
        private int postID = -1;
        private int threadSequence = 1;
        private string postSubject = string.Empty;
        private string postDate = string.Empty;
        private bool isApproved = true;
        private int postUserID = -1;
        private string postUserName = string.Empty;
        private int postSortOrder = 100;
        private string postMessage = string.Empty;
        private bool subscribeUserToThread = false;
        private int siteId = -1;
        private string searchIndexPath = string.Empty;


        #endregion


        #region Public Properties

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

        public int ThreadId
        {
            get { return threadID; }
        }

        public int ForumId
        {
            get { return forumID; }
            set { forumID = value; }
        }

        public int ModuleId
        {
            get { return moduleID; }

        }

        public DateTime ThreadDate
        {
            get { return threadDate; }
        }

        public string StartedBy
        {
            get { return startedBy; }
            set { startedBy = value; }
        }

        public int StartedByUserId
        {
            get { return startedByUserID; }
            set { startedByUserID = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public int TotalViews
        {
            get { return totalViews; }
        }

        public int TotalReplies
        {
            get { return totalReplies; }
        }

        public int TotalPages
        {
            get { return totalPages; }
        }


        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public int ForumSequence
        {
            get { return forumSequence; }
        }


        public DateTime MostRecentPostDate
        {
            get { return mostRecentPostDate; }
        }

        public string MostRecentPostUser
        {
            get { return mostRecentPostUser; }
        }

        public int MostRecentPostUserId
        {
            get { return mostRecentPostUserID; }
        }

        //post properties

        public int PostId
        {
            get { return postID; }
        }

        public int ThreadSequence
        {
            get { return threadSequence; }
        }

        public string PostSubject
        {
            get { return postSubject; }
            set { postSubject = value; }
        }

        public string PostDate
        {
            get { return postDate; }
        }

        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        public int PostUserId
        {
            get { return postUserID; }
            set { postUserID = value; }
        }

        public string PostUserName
        {
            get { return postUserName; }
            set { postUserName = value; }
        }

        public int PostSortOrder
        {
            get { return postSortOrder; }
            set { postSortOrder = value; }
        }

        public string PostMessage
        {
            get { return postMessage; }
            set { postMessage = value; }
        }

        public bool SubscribeUserToThread
        {
            get { return subscribeUserToThread; }
            set { subscribeUserToThread = value; }
        }


        #endregion


        #region Private Methods

        private void GetThread(int threadId)
        {
            using (IDataReader reader = DBForums.ForumThreadGetThread(threadId))
            {
                if (reader.Read())
                {

                    this.threadID = int.Parse(reader["ThreadID"].ToString());
                    if (reader["ForumID"] != DBNull.Value)
                    {
                        this.forumID = this.origForumID = int.Parse(reader["ForumID"].ToString());
                    }
                    if (reader["ThreadDate"] != DBNull.Value)
                    {
                        this.threadDate = Convert.ToDateTime(reader["ThreadDate"].ToString());
                    }
                    this.startedBy = reader["StartedBy"].ToString();
                    if (reader["StartedByUserID"] != DBNull.Value)
                    {
                        this.startedByUserID = int.Parse(reader["StartedByUserID"].ToString());
                    }

                    this.subject = reader["ThreadSubject"].ToString();
                    if (reader["TotalViews"] != DBNull.Value)
                    {
                        this.totalViews = Convert.ToInt32(reader["TotalViews"]);
                    }

                    if (reader["TotalReplies"] != DBNull.Value)
                    {
                        this.totalReplies = Convert.ToInt32(reader["TotalReplies"]);
                    }

                    if (reader["SortOrder"] != DBNull.Value)
                    {
                        this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
                    }
                    if (reader["ForumSequence"] != DBNull.Value)
                    {
                        this.forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                    }

                    if (reader["PostsPerPage"] != DBNull.Value)
                    {
                        this.postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                    }

                    if (this.totalReplies + 1 > this.postsPerPage)
                    {
                        this.totalPages = this.totalReplies / this.postsPerPage;
                        int remainder = 0;
                        int pageCount = Math.DivRem(this.totalReplies + 1, this.postsPerPage, out remainder);
                        if ((remainder > 0) || (pageCount > this.totalPages))
                        {
                            this.totalPages += 1;
                        }
                    }
                    else
                    {
                        this.totalPages = 1;
                    }

                    // this is to support dbs that don't have bit data type
                    string locked = reader["IsLocked"].ToString();
                    this.isLocked = (locked == "True" || locked == "1");

                    if (reader["MostRecentPostDate"] != DBNull.Value)
                    {
                        this.mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                    }

                    this.mostRecentPostUser = reader["MostRecentPostUser"].ToString();

                    if (reader["MostRecentPostUserID"] != DBNull.Value)
                    {
                        this.mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
                    }

                }

            }


        }

        private void GetPost(int postId)
        {
            using (IDataReader reader = DBForums.ForumThreadGetPost(postId))
            {
                if (reader.Read())
                {
                    this.postID = Convert.ToInt32(reader["PostID"]);
                    this.postUserID = Convert.ToInt32(reader["UserID"]);
                    this.postSubject = reader["Subject"].ToString();
                    this.postMessage = reader["Post"].ToString();

                    // this is to support dbs that don't have bit data type
                    string approved = reader["Approved"].ToString();
                    this.isApproved = (approved == "True" || approved == "1");
                }

            }

        }


        private bool CreateThread()
        {
            int newID = -1;

            newID = DBForums.ForumThreadCreate(
                this.forumID,
                this.postSubject,
                this.sortOrder,
                this.isLocked,
                this.postUserID,
                DateTime.UtcNow);


            this.threadID = newID;
            Forum.IncrementThreadCount(this.forumID);

            return (newID > -1);

        }

        private bool CreatePost()
        {
            int newID = -1;
            bool approved = false;
            if (
                (ConfigurationManager.AppSettings["PostsApprovedByDefault"] != null)
                && (string.Equals(ConfigurationManager.AppSettings["PostsApprovedByDefault"], "true", StringComparison.InvariantCultureIgnoreCase))
                )
            {
                approved = true;
            }

            this.mostRecentPostDate = DateTime.UtcNow;
            newID = DBForums.ForumPostCreate(
                this.threadID,
                this.postSubject,
                this.postMessage,
                approved,
                this.PostUserId,
                this.mostRecentPostDate);

            this.postID = newID;
            Forum.IncrementPostCount(this.forumID, this.postUserID, this.mostRecentPostDate);
            SiteUser.IncrementTotalPosts(this.postUserID);
            //IndexHelper.IndexItem(this);

            bool result = (newID > -1);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }

        private bool UpdatePost()
        {
            bool result = false;

            result = DBForums.ForumPostUpdate(
                this.postID,
                this.postSubject,
                this.postMessage,
                this.sortOrder,
                this.isApproved);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }



        private bool IncrementReplyStats()
        {
            return DBForums.ForumThreadIncrementReplyStats(
                this.threadID,
                this.postUserID,
                this.mostRecentPostDate);

        }

        private void ResetThreadSequences()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBForums.ForumThreadGetPosts(this.threadID))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);
                }

            }

            int sequence = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                DBForums.ForumPostUpdateThreadSequence(
                    Convert.ToInt32(row["PostID"]),
                    sequence);
                sequence += 1;
            }

        }


        #endregion


        #region Public Methods

        public int Post()
        {
            bool newThread = (this.threadID < 0);
            if (newThread)
            {
                this.CreateThread();
            }

            if (this.postID > -1)
            {
                this.UpdatePost();
            }
            else
            {
                this.CreatePost();
                if (!newThread)
                {
                    this.IncrementReplyStats();
                }
            }

            if (this.subscribeUserToThread)
            {
                DBForums.ForumThreadAddSubscriber(this.threadID, this.postUserID);

            }


            return this.postID;

        }

        public bool DeletePost(int postId)
        {
            bool deleted = DBForums.ForumPostDelete(postId);
            if (deleted)
            {
                Forum.DecrementPostCount(this.forumID);
                if (this.totalReplies > 0)
                {
                    DBForums.ForumThreadDecrementReplyStats(this.threadID);
                }
                Forum forum = new Forum(this.forumID);

                this.moduleID = forum.ModuleId;
                this.postID = postId;

                ContentChangedEventArgs e = new ContentChangedEventArgs();
                e.IsDeleted = true;
                OnContentChanged(e);

                int threadPostCount = ForumThread.GetPostCount(this.threadID);
                if (threadPostCount == 0)
                {
                    ForumThread.Delete(this.threadID);
                    Forum.DecrementThreadCount(this.forumID);

                }

                ResetThreadSequences();
            }


            return deleted;
        }



        public bool UpdateThreadViewStats()
        {
            return DBForums.ForumThreadUpdateViewStats(this.threadID);


        }

        public IDataReader GetPosts(int pageNumber)
        {
            return DBForums.ForumThreadGetPosts(this.threadID, pageNumber);
        }

        public IDataReader GetPosts()
        {
            return DBForums.ForumThreadGetPosts(this.threadID);
        }

        public DataTable GetPostIdList()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBForums.ForumThreadGetPosts(this.threadID))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;

        }

        public IDataReader GetPostsReverseSorted()
        {
            return DBForums.ForumThreadGetPostsReverseSorted(this.threadID);
        }


        public DataSet GetThreadSubscribers()
        {
            return DBForums.ForumThreadGetSubscribers(this.forumID, this.threadID, this.postUserID);
        }

        public bool UpdateThread()
        {
            bool result = false;

            result = DBForums.ForumThreadUpdate(
                this.threadID,
                this.forumID,
                this.subject,
                this.sortOrder,
                this.isLocked);

            if (this.forumID != this.origForumID)
            {

                Forum.DecrementThreadCount(this.origForumID);
                Forum.IncrementThreadCount(this.forumID);

                ForumThreadMovedArgs e = new ForumThreadMovedArgs();
                e.ForumId = forumID;
                e.OriginalForumId = origForumID;
                OnThreadMoved(e);


                Forum.RecalculatePostStats(this.origForumID);
                Forum.RecalculatePostStats(this.forumID);
            }

            return result;
        }



        #endregion


        #region Static Methods


        public static bool Unsubscribe(int threadId, int userId)
        {
            return DBForums.ForumThreadUNSubscribe(threadId, userId);
        }

        public static bool UnsubscribeAll(int userId)
        {
            return DBForums.ForumThreadUnsubscribeAll(userId);
        }



        public static bool Delete(int threadId)
        {
            bool status = false;

            ForumThread forumThread = new ForumThread(threadId);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBForums.ForumThreadGetPosts(threadId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                forumThread.DeletePost(Convert.ToInt32(row["PostID"]));
            }

            status = DBForums.ForumThreadDelete(threadId);

            return status;
        }

        public static int GetPostCount(int threadId)
        {
            return DBForums.ForumThreadGetPostCount(threadId);
        }


        public static DataTable GetPostsByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ThreadID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Post", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            using (IDataReader reader = DBForums.ForumThreadGetPostsByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ThreadID"] = reader["ThreadID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Subject"] = reader["Subject"];
                    row["Post"] = reader["Post"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }


        public static IDataReader GetPageByUser(
            int userId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            return DBForums.GetThreadPageByUser(
                userId,
                pageNumber,
                pageSize,
                out totalPages);

        }



        public static bool IsSubscribed(int threadId, int userId)
        {
            return DBForums.ForumThreadSubscriptionExists(threadId, userId);
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

        public delegate void ThreadMovedEventHandler(object sender, ForumThreadMovedArgs e);

        public event ThreadMovedEventHandler ThreadMoved;

        protected void OnThreadMoved(ForumThreadMovedArgs e)
        {
            if (ThreadMoved != null)
            {
                ThreadMoved(this, e);
            }
        }

    }
}
