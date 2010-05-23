/// Author:					    Joe Audette
/// Created:				    2007-08-30
/// Last Modified:			    2009-07-22
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Threading;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;

namespace Cynthia.Modules
{
    
    public class GroupThreadIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(GroupThreadIndexBuilderProvider));

        public GroupThreadIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if ((pageSettings == null) || (indexPath == null))
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("pageSettings object or index path passed to GroupThreadIndexBuilderProvider.RebuildIndex was null");
                }
                return;

            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info("GroupThreadIndexBuilderProvider indexing page - "
                + pageSettings.PageName);

            try
            {
                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                Guid forumFeatureGuid = new Guid("E75BAF8C-7079-4d10-A122-1AA3624E26F2");
                ModuleDefinition forumFeature = new ModuleDefinition(forumFeatureGuid);

                DataTable dataTable = GroupThread.GetPostsByPage(
                    pageSettings.SiteId,
                    pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    IndexItem indexItem = new IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    indexItem.FeatureId = forumFeatureGuid.ToString();
                    indexItem.FeatureName = forumFeature.FeatureName;
                    indexItem.FeatureResourceFile = forumFeature.ResourceFile;

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Subject"].ToString();
                    indexItem.Content = row["Post"].ToString();
                    indexItem.ViewPage = "Groups/Thread.aspx";
                    indexItem.QueryStringAddendum = "&thread="
                        + row["TopicID"].ToString()
                        + "&postid=" + row["PostID"].ToString();

                    // lookup publish dates
                    foreach (PageModule pageModule in pageModules)
                    {
                        if (indexItem.ModuleId == pageModule.ModuleId)
                        {
                            indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                            indexItem.PublishEndDate = pageModule.PublishEndDate;
                        }
                    }

                    IndexHelper.RebuildIndex(indexItem, indexPath);

                    if (log.IsDebugEnabled) log.Debug("Indexed " + indexItem.Title);

                }

            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error(ex);
            }

        }

        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (sender == null) return;
            if (!(sender is GroupThread)) return;


            GroupThread forumThread = (GroupThread)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            forumThread.SiteId = siteSettings.SiteId;
            forumThread.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(RemoveGroupIndexItem), forumThread))
                {
                    if (log.IsDebugEnabled) log.Debug("GroupThreadIndexBuilderProvider.RemoveGroupIndexItem queued");
                }
                else
                {
                    if (log.IsErrorEnabled) log.Error("Failed to queue a thread for GroupThreadIndexBuilderProvider.RemoveGroupIndexItem");
                }

                //RemoveGroupIndexItem(forumThread);
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), forumThread))
                {
                    if (log.IsDebugEnabled) log.Debug("GroupThreadIndexBuilderProvider.IndexItem queued");
                }
                else
                {
                    if (log.IsErrorEnabled) log.Error("Failed to queue a thread for GroupThreadIndexBuilderProvider.IndexItem");
                }

                //IndexItem(forumThread);
            }

        }

        private static void IndexItem(object oGroupThread)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (oGroupThread == null) return;
            if (!(oGroupThread is GroupThread)) return;

            GroupThread forumThread = oGroupThread as GroupThread;
            IndexItem(forumThread);

        }

        private static void IndexItem(GroupThread forumThread)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            //if (siteSettings == null)
            //{
            //    if (log.IsErrorEnabled)
            //    {
            //        log.Error("siteSettings object retrieved in GroupThreadIndexBuilderProvider.IndexItem was null");
            //    }
            //    return;
            //}

            if (forumThread == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("forumThread object passed in GroupThreadIndexBuilderProvider.IndexItem was null");
                }
                return;

            }

            Group forum = new Group(forumThread.GroupId);
            Module module = new Module(forum.ModuleId);
            Guid forumFeatureGuid = new Guid("E75BAF8C-7079-4d10-A122-1AA3624E26F2");
            ModuleDefinition forumFeature = new ModuleDefinition(forumFeatureGuid);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(forum.ModuleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    forumThread.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                IndexItem indexItem = new IndexItem();
                if (forumThread.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = forumThread.SearchIndexPath;
                }
                indexItem.SiteId = forumThread.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.PageName = pageSettings.PageName;
                // permissions are kept in sync in search index
                // so that results are filtered by role correctly
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                indexItem.ItemId = forumThread.GroupId;
                indexItem.ModuleId = forum.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.ViewPage = "Groups/Thread.aspx";
                indexItem.QueryStringAddendum = "&thread="
                    + forumThread.ThreadId.ToString()
                    + "&postid=" + forumThread.PostId.ToString();
                indexItem.FeatureId = forumFeatureGuid.ToString();
                indexItem.FeatureName = forumFeature.FeatureName;
                indexItem.FeatureResourceFile = forumFeature.ResourceFile;
                indexItem.Title = forumThread.Subject;
                indexItem.Content = forumThread.PostMessage;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                IndexHelper.RebuildIndex(indexItem);

                if (log.IsDebugEnabled) log.Debug("Indexed "
                    + forumThread.Subject);

            }

        }

        public void ThreadMovedHandler(object sender, GroupThreadMovedArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            GroupThread forumThread = (GroupThread)sender;
            DataTable postIDList = forumThread.GetPostIdList();

            Group origGroup = new Group(e.OriginalGroupId);
            foreach (DataRow row in postIDList.Rows)
            {
                int postID = Convert.ToInt32(row["PostID"]);
                GroupThread post = new GroupThread(forumThread.ThreadId, postID);

                RemoveGroupIndexItem(
                    origGroup.ModuleId,
                    e.OriginalGroupId,
                    forumThread.ThreadId,
                    postID);

                IndexItem(post);

            }


        }

        public static void RemoveGroupIndexItem(object oGroupThread)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (!(oGroupThread is GroupThread)) return;

            GroupThread forumThread = oGroupThread as GroupThread;

            //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            //if (siteSettings == null)
            //{
            //    if (log.IsErrorEnabled)
            //    {
            //        log.Error("siteSettings object retrieved in GroupThreadIndexBuilderProvider.RemoveGroupIndexItem was null");
            //    }
            //    return;
            //}

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(forumThread.ModuleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                IndexItem indexItem = new IndexItem();
                // note we are just assigning the properties 
                // needed to derive the key so it can be found and
                // deleted from the index
                indexItem.SiteId = forumThread.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.ModuleId = forumThread.ModuleId;
                indexItem.ItemId = forumThread.GroupId;
                indexItem.QueryStringAddendum = "&thread="
                    + forumThread.ThreadId.ToString(CultureInfo.InvariantCulture)
                    + "&postid=" + forumThread.PostId.ToString(CultureInfo.InvariantCulture);

                IndexHelper.RemoveIndex(indexItem);
            }

            if (log.IsDebugEnabled) log.Debug("Removed Index ");

        }

        public static void RemoveGroupIndexItem(
            int moduleId,
            int itemId,
            int threadId,
            int postId)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("siteSettings object retrieved in GroupThreadIndexBuilderProvider.RemoveGroupIndexItem was null");
                }
                return;
            }

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(moduleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                IndexItem indexItem = new IndexItem();
                // note we are just assigning the properties 
                // needed to derive the key so it can be found and
                // deleted from the index
                indexItem.SiteId = siteSettings.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.ModuleId = moduleId;
                indexItem.ItemId = itemId;
                indexItem.QueryStringAddendum = "&thread="
                    + threadId.ToString()
                    + "&postid=" + postId.ToString();

                IndexHelper.RemoveIndex(indexItem);
            }

            if (log.IsDebugEnabled) log.Debug("Removed Index ");

        }


    }
}
