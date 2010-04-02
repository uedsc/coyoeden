// Author:					    Joe Audette
// Created:				        2007-08-30
// Last Modified:			    2010-01-30
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
using System.Collections.Generic;
using System.Data;
using System.Threading;
using log4net;
using Cynthia.Web.Framework;

namespace Cynthia.Business.WebHelpers
{
    /// <summary>
    ///
    /// </summary>
    public class HtmlContentIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(HtmlContentIndexBuilderProvider));

        public HtmlContentIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
            if (disableSearchIndex) { return; }

            if (pageSettings == null)
            {
                log.Error("pageSettings passed in to HtmlContentIndexBuilderProvider.RebuildIndex was null");
                return;
            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info("HtmlContentIndexBuilderProvider indexing page - " 
                + pageSettings.PageName);

            try
            {
                Guid htmlFeatureGuid 
                    = new Guid("113FB01C-6408-4607-B0F7-1379E2512396");
                ModuleDefinition htmlFeature 
                    = new ModuleDefinition(htmlFeatureGuid);

                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                HtmlRepository repository = new HtmlRepository();

                DataTable dataTable = repository.GetHtmlContentByPage(
                    pageSettings.SiteId,
                    pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    IndexItem indexItem = new IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;

                    // generally we should not include the page meta because it can result in duplicate results
                    // one for each instance of html content on the page because they all use the smae page meta.
                    // since page meta should reflect the content of the page it is sufficient to just index the content
                    if ((ConfigurationManager.AppSettings["IndexPageMeta"] != null) && (ConfigurationManager.AppSettings["IndexPageMeta"] == "true"))
                    {
                        indexItem.PageMetaDescription = pageSettings.PageMetaDescription;
                        indexItem.PageMetaKeywords = pageSettings.PageMetaKeyWords;
                    }

                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    if (pageSettings.UseUrl)
                    {
                        indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
                        indexItem.UseQueryStringParams = false;
                    }
                    indexItem.FeatureId = htmlFeatureGuid.ToString();
                    indexItem.FeatureName = htmlFeature.FeatureName;
                    indexItem.FeatureResourceFile = htmlFeature.ResourceFile;

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Title"].ToString();
                    // added the remove markup 2010-01-30 because some javascript strings like ]]> were apearing in search results if the content conatined jacvascript
                    indexItem.Content = SecurityHelper.RemoveMarkup(row["Body"].ToString());

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

                    log.Debug("Indexed " + indexItem.Title);

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
            bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
            if (disableSearchIndex) { return; }

            if (sender == null) return;
            if (!(sender is HtmlContent)) return;

            HtmlContent content = (HtmlContent)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            content.SiteId = siteSettings.SiteId;
            content.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(content.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        content.ModuleId,
                        content.ItemId);
                }
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), content))
                {
                    if (log.IsDebugEnabled) log.Debug("HtmlContentIndexBuilderProvider.IndexItem queued");
                }
                else
                {
                    if (log.IsErrorEnabled) log.Error("Failed to queue a thread for HtmlContentIndexBuilderProvider.IndexItem");
                }
                //IndexItem(content);
            }

        }

        private static void IndexItem(object o)
        {
            bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
            if (disableSearchIndex) { return; }

            if (o == null) return;
            if (!(o is HtmlContent)) return;

            HtmlContent content = o as HtmlContent;
            IndexItem(content);

        }

        private static void IndexItem(HtmlContent content)
        {
            bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
            if (disableSearchIndex) { return; }
            //SiteSettings siteSettings 
            //    = CacheHelper.GetCurrentSiteSettings();
            
            //if (
            //    (content == null)
            //    || (siteSettings == null)
            //    )
            //{
            //    return;
            //}

            Guid htmlFeatureGuid 
                = new Guid("113FB01C-6408-4607-B0F7-1379E2512396");
            ModuleDefinition htmlFeature 
                = new ModuleDefinition(htmlFeatureGuid);
            Module module = new Module(content.ModuleId);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(content.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    content.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                IndexItem indexItem = new IndexItem();
                if (content.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = content.SearchIndexPath;
                }
                indexItem.SiteId = content.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                if (pageSettings.UseUrl)
                {
                    indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
                    indexItem.UseQueryStringParams = false;
                }

                // generally we should not include the page meta because it can result in duplicate results
                // one for each instance of html content on the page because they all use the smae page meta.
                // since page meta should reflect the content of the page it is sufficient to just index the content
                if ((ConfigurationManager.AppSettings["IndexPageMeta"] != null) && (ConfigurationManager.AppSettings["IndexPageMeta"] == "true"))
                {
                    indexItem.PageMetaDescription = pageSettings.PageMetaDescription;
                    indexItem.PageMetaKeywords = pageSettings.PageMetaKeyWords;
                }

                indexItem.FeatureId = htmlFeatureGuid.ToString();
                indexItem.FeatureName = htmlFeature.FeatureName;
                indexItem.FeatureResourceFile = htmlFeature.ResourceFile;

                indexItem.ItemId = content.ItemId;
                indexItem.ModuleId = content.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = content.Title;
                indexItem.Content = SecurityHelper.RemoveMarkup(content.Body);
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                IndexHelper.RebuildIndex(indexItem);
            }

            log.Debug("Indexed " + content.Title);
         
        }

    }
}
