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
using System.Data;
using System.Globalization;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;

namespace Cynthia.Modules
{
    public class LinksIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(LinksIndexBuilderProvider));

        public LinksIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (pageSettings == null)
            {
                if (log.IsErrorEnabled) log.Error("LinksIndexBuilderProvider.RebuildIndex error: pageSettings was null ");
                return;
            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info("LinksIndexBuilderProvider indexing page - "
                + pageSettings.PageName);

            try
            {
                Guid linksFeatureGuid = new Guid("5E52DEA6-7DE4-4bb7-B1CF-1324D4371956");
                ModuleDefinition linksFeature = new ModuleDefinition(linksFeatureGuid);

                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                DataTable dataTable = Link.GetLinksByPage(
                    pageSettings.SiteId, pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    IndexItem indexItem = new IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();

                    if (pageSettings.UseUrl)
                    {
                        indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
                        indexItem.UseQueryStringParams = false;
                    }

                    indexItem.FeatureId = linksFeatureGuid.ToString();
                    indexItem.FeatureName = linksFeature.FeatureName;
                    indexItem.FeatureResourceFile = linksFeature.ResourceFile;

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"], CultureInfo.InvariantCulture);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Title"].ToString();
                    indexItem.Content = row["Description"].ToString();
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

            Link link = (Link)sender;
            if (e.IsDeleted)
            {
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(link.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        link.ModuleId,
                        link.ItemId);
                }
            }
            else
            {
                IndexItem(link);
            }

        }

        private static void IndexItem(Link link)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("Link object passed to Links.IndexItem was null");
                }

                return;
            }

            if (link == null) return;

            Guid linksFeatureGuid = new Guid("5E52DEA6-7DE4-4bb7-B1CF-1324D4371956");
            ModuleDefinition linksFeature = new ModuleDefinition(linksFeatureGuid);
            Module module = new Module(link.ModuleId);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(link.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    siteSettings.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                IndexItem indexItem = new IndexItem();
                indexItem.SiteId = siteSettings.SiteId;
                indexItem.PageId = pageSettings.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                indexItem.FeatureId = linksFeatureGuid.ToString();
                indexItem.FeatureName = linksFeature.FeatureName;
                indexItem.FeatureResourceFile = linksFeature.ResourceFile;

                indexItem.ItemId = link.ItemId;
                indexItem.ModuleId = link.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = link.Title;
                indexItem.Content = link.Description;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                IndexHelper.RebuildIndex(indexItem);
            }

            if (log.IsDebugEnabled) log.Debug("Indexed " + link.Title);


        }

    }
}
