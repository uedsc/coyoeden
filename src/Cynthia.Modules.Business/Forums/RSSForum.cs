

using System;
using System.Collections;
using System.Data;
using Cynthia.Data;


namespace Cynthia.Business
{
    /// <summary>
    /// Represents an rss feed for the forum
    /// </summary>
    /// 
    public class RssGroup
    {
        #region Constructors

        public RssGroup()
        { }

        #endregion

        #region Private Properties
        private int siteID = -1;
        private int pageID = -1;
        private int moduleID = -1;
        private int itemID = -1;
        private int threadID = -1;
        private int maximumDays = -1;
        private DateTime createdDate;

        #endregion

        #region Public Properties

        public int ItemId
        {
            get { return itemID; }
            set { itemID = value; }
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
        public int PageId
        {
            get { return pageID; }
            set { pageID = value; }
        }
        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }
        public int ThreadId
        {
            get { return threadID; }
            set { threadID = value; }
        }
        public int MaximumDays
        {
            get { return maximumDays; }
            set { maximumDays = value; }
        }


        #endregion

        #region Public Methods
        public IDataReader GetPostsForRss()
        {
            return DBGroups.GroupThreadGetPostsForRss(SiteId, PageId, ModuleId, ItemId, ThreadId, MaximumDays);
        }
        #endregion

    }
}
