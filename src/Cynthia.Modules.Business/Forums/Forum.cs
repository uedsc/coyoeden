

using System;
using System.Data;
using log4net;
using Cynthia.Data;

namespace Cynthia.Business
{
    /// <summary>
    /// Represents a forum
    /// </summary>
    public class Group
    {

        #region Constructors

        public Group()
        { }

        public Group(int forumId)
        {
            if (forumId > -1)
            {
                GetGroup(forumId);
            }

        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(Group));

        private int itemID = -1;
        private int moduleID = -1;
        private DateTime createdDate = DateTime.UtcNow;
        private string createdBy = string.Empty;
        private int createdByUserID;
        private string title = string.Empty;
        private string description = string.Empty;
        private bool isModerated;
        private bool isActive = true;
        //sorted in db by SortOrder, ItemID
        private int sortOrder = 100;
        private int postsPerPage = 10;
        private int threadsPerPage = 20;
        private int threadCount;
        private int postCount;
        private int totalPages = 1;
        private DateTime mostRecentPostDate = DateTime.UtcNow;
        private string mostRecentPostUser = string.Empty;
        private bool allowAnonymousPosts = false;


        #endregion

        #region Public Properties

        public int ItemId
        {
            get { return itemID; }
        }

        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public int CreatedByUserId
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool AllowAnonymousPosts
        {
            get { return allowAnonymousPosts; }
            set { allowAnonymousPosts = value; }
        }


        public bool IsModerated
        {
            get { return isModerated; }
            set { isModerated = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public int PostsPerPage
        {
            get { return postsPerPage; }
            set { postsPerPage = value; }
        }

        public int TopicsPerPage
        {
            get { return threadsPerPage; }
            set { threadsPerPage = value; }
        }

        public int TopicCount
        {
            get { return threadCount; }
            set { threadCount = value; }
        }

        public int PostCount
        {
            get { return postCount; }
            set { postCount = value; }
        }

        public int TotalPages
        {
            get { return totalPages; }
        }

        public DateTime MostRecentPostDate
        {
            get { return mostRecentPostDate; }
        }

        public string MostRecentPostUser
        {
            get { return mostRecentPostUser; }
        }




        #endregion

        #region Private Methods

        private void GetGroup(int forumId)
        {
            using (IDataReader reader = DBGroups.GetGroup(forumId))
            {
                if (reader.Read())
                {

                    this.itemID = int.Parse(reader["ItemID"].ToString());
                    this.moduleID = int.Parse(reader["ModuleID"].ToString());
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.createdBy = reader["CreatedByUser"].ToString();
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    // this is to support dbs that don't have bit data type
                    string anon = reader["AllowAnonymousPosts"].ToString();
                    this.allowAnonymousPosts = (anon == "True" || anon == "1");
                    string moderated = reader["IsModerated"].ToString();
                    this.isModerated = (moderated == "True" || moderated == "1");
                    string active = reader["IsActive"].ToString();
                    this.isActive = (active == "True" || active == "1");
                    this.sortOrder = int.Parse(reader["SortOrder"].ToString());
                    this.postsPerPage = int.Parse(reader["PostsPerPage"].ToString());
                    this.threadsPerPage = int.Parse(reader["TopicsPerPage"].ToString());
                    this.threadCount = int.Parse(reader["TopicCount"].ToString());
                    this.postCount = int.Parse(reader["PostCount"].ToString());
                    if (reader["MostRecentPostDate"] != DBNull.Value)
                    {
                        this.mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                    }
                    this.mostRecentPostUser = reader["MostRecentPostUser"].ToString();

                    if (this.threadCount > this.threadsPerPage)
                    {
                        this.totalPages = this.threadCount / this.threadsPerPage;
                        int remainder = 0;
                        Math.DivRem(this.threadCount, this.threadsPerPage, out remainder);
                        if (remainder > 0)
                        {
                            this.totalPages += 1;
                        }
                    }
                    else
                    {
                        this.totalPages = 1;
                    }

                }
                else
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("IDataReader didn't read in Group.GetGroup");
                    }


                }
            }
            


        }

        private bool Create()
        {
            int newID = -1;

            newID = DBGroups.Create(
                this.moduleID,
                this.createdByUserID,
                this.title,
                this.description,
                this.isModerated,
                this.isActive,
                this.sortOrder,
                this.postsPerPage,
                this.threadsPerPage,
                this.allowAnonymousPosts);

            this.itemID = newID;

            return (newID > -1);

        }


        private bool Update()
        {

            return DBGroups.Update(
                this.itemID,
                this.createdByUserID,
                this.title,
                this.description,
                this.isModerated,
                this.isActive,
                this.sortOrder,
                this.postsPerPage,
                this.threadsPerPage,
                this.allowAnonymousPosts);
        }

        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.itemID > -1)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }


        public IDataReader GetThreads(int pageNumber)
        {
            return DBGroups.GetThreads(this.itemID, pageNumber);
        }

        public bool Subscribe(int userId)
        {
            return DBGroups.AddSubscriber(this.itemID, userId);
        }

        public bool Unsubscribe(int userId)
        {
            return DBGroups.Unsubscribe(this.itemID, userId);
        }

        #endregion

        #region Static Methods

        public static bool UnsubscribeAll(int userId)
        {
            return DBGroups.UnsubscribeAll(userId);
        }

        public static IDataReader GetGroups(int moduleId, int userId)
        {
            return DBGroups.GetGroups(moduleId, userId);
        }


        public static bool IncrementPostCount(int forumId, int mostRecentPostUserId, DateTime mostRecentPostDate)
        {
            return DBGroups.IncrementPostCount(forumId, mostRecentPostUserId, mostRecentPostDate);
        }

        public static bool IncrementPostCount(int forumId)
        {
            return DBGroups.IncrementPostCount(forumId);
        }

        public static bool DecrementPostCount(int forumId)
        {
            return DBGroups.DecrementPostCount(forumId);
        }

        public static bool RecalculatePostStats(int forumId)
        {
            //implemented for PostgreSQL. --Dean 9/11/05
            return DBGroups.RecalculatePostStats(forumId);

        }

        public static bool DecrementTopicCount(int forumId)
        {
            return DBGroups.DecrementTopicCount(forumId);
        }

        public static bool IncrementTopicCount(int forumId)
        {
            return DBGroups.IncrementTopicCount(forumId);
        }

        public static bool Delete(int itemId)
        {
            return DBGroups.Delete(itemId);
        }

        public static bool DeleteByModule(int moduleId)
        {
            return DBGroups.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBGroups.DeleteBySite(siteId);
        }

        public static bool IsSubscribed(int forumId, int userId)
        {
            return DBGroups.GroupSubscriptionExists(forumId, userId);
        }

        /// <summary>
        /// passing in -1 for userId will update the stats of all users.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UpdateUserStats(int userId)
        {
            return DBGroups.UpdateUserStats(userId);
        }

        public static IDataReader GetSubscriberPage(
            int forumId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBGroups.GetSubscriberPage(
                forumId,
                pageNumber,
                pageSize,
                out totalPages);
        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            return DBGroups.DeleteSubscription(subscriptionId);
        }


        #endregion

    }
}
