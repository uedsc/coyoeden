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
using System.Text;
using System.Threading;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;

namespace Cynthia.Features
{
    
    public class BlogIndexBuilderProvider : IndexBuilderProvider
    {
        public BlogIndexBuilderProvider()
        { }

        private static readonly ILog log
            = LogManager.GetLogger(typeof(BlogIndexBuilderProvider));

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (pageSettings == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("pageSettings object passed to BlogIndexBuilderProvider.RebuildIndex was null");
                }
                return;
            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info("BlogIndexBuilderProvider indexing page - "
                + pageSettings.PageName);

            //try
            //{
            Guid blogFeatureGuid = new Guid("D1D86E30-7864-40fe-BCBF-46E77AF80342");
            ModuleDefinition blogFeature = new ModuleDefinition(blogFeatureGuid);

            List<PageModule> pageModules
                    = PageModule.GetPageModulesByPage(pageSettings.PageId);

            DataTable dataTable
                = Blog.GetBlogsByPage(
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
                indexItem.FeatureId = blogFeatureGuid.ToString();
                indexItem.FeatureName = blogFeature.FeatureName;
                indexItem.FeatureResourceFile = blogFeature.ResourceFile;

                indexItem.ItemId = Convert.ToInt32(row["ItemID"], CultureInfo.InvariantCulture);
                indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
                indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                indexItem.Title = row["Title"].ToString();
                indexItem.ViewPage = row["ItemUrl"].ToString().Replace("~/", string.Empty);

                indexItem.PageMetaDescription = row["MetaDescription"].ToString();
                indexItem.PageMetaKeywords = row["MetaKeywords"].ToString();

                DateTime blogStart = Convert.ToDateTime(row["StartDate"]);

                if (indexItem.ViewPage.Length > 0)
                {
                    indexItem.UseQueryStringParams = false;
                }
                else
                {
                    indexItem.ViewPage = "Blog/ViewPost.aspx";
                }
                indexItem.Content = row["Description"].ToString();
                int commentCount = Convert.ToInt32(row["CommentCount"]);

                if (commentCount > 0)
                {	// index comments
                    StringBuilder stringBuilder = new StringBuilder();
                    DataTable comments = Blog.GetBlogCommentsTable(indexItem.ModuleId, indexItem.ItemId);

                    foreach (DataRow commentRow in comments.Rows)
                    {
                        stringBuilder.Append("  " + commentRow["Comment"].ToString());
                        stringBuilder.Append("  " + commentRow["Name"].ToString());

                        if (log.IsDebugEnabled) log.Debug("BlogIndexBuilderProvider.RebuildIndex add comment ");

                    }

                    indexItem.OtherContent = stringBuilder.ToString();

                }

                // lookup publish dates
                foreach (PageModule pageModule in pageModules)
                {
                    if (indexItem.ModuleId == pageModule.ModuleId)
                    {
                        indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                        indexItem.PublishEndDate = pageModule.PublishEndDate;
                    }
                }

                if (blogStart > indexItem.PublishBeginDate) { indexItem.PublishBeginDate = blogStart; }
                


                IndexHelper.RebuildIndex(indexItem, indexPath);

                if (log.IsDebugEnabled) log.Debug("Indexed " + indexItem.Title);

            }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //}


        }


        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (sender == null) return;
            if (!(sender is Blog)) return;

            Blog blog = (Blog)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            blog.SiteId = siteSettings.SiteId;
            blog.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);


            if (e.IsDeleted)
            {
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(blog.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        blog.ModuleId,
                        blog.ItemId);
                }
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), blog))
                {
                    if (log.IsDebugEnabled) log.Debug("BlogIndexBuilderProvider.IndexItem queued");
                }
                else
                {
                    if (log.IsErrorEnabled) log.Error("Failed to queue a thread for BlogIndexBuilderProvider.IndexItem");
                }
                //IndexItem(blog);
            }


        }


        private static void IndexItem(object o)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (o == null) return;
            if (!(o is Blog)) return;

            Blog content = o as Blog;
            IndexItem(content);

        }


        private static void IndexItem(Blog blog)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (blog == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("blog object passed to BlogIndexBuilderProvider.IndexItem was null");
                }
                return;
            }

            Module module = new Module(blog.ModuleId);
            Guid blogFeatureGuid = new Guid("D1D86E30-7864-40fe-BCBF-46E77AF80342");
            ModuleDefinition blogFeature = new ModuleDefinition(blogFeatureGuid);

            // get comments so  they can be indexed too
            StringBuilder stringBuilder = new StringBuilder();
            using (IDataReader comments = Blog.GetBlogComments(blog.ModuleId, blog.ItemId))
            {
                while (comments.Read())
                {
                    stringBuilder.Append("  " + comments["Comment"].ToString());
                    stringBuilder.Append("  " + comments["Name"].ToString());

                    if (log.IsDebugEnabled) log.Debug("BlogIndexBuilderProvider.IndexItem add comment ");

                }
            }

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(blog.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    blog.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                IndexItem indexItem = new IndexItem();
                if (blog.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = blog.SearchIndexPath;
                }
                indexItem.SiteId = blog.SiteId;
                indexItem.PageId = pageSettings.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                if (blog.ItemUrl.Length > 0)
                {
                    indexItem.ViewPage = blog.ItemUrl.Replace("~/", string.Empty);
                    indexItem.UseQueryStringParams = false;
                }
                else
                {
                    indexItem.ViewPage = "Blog/ViewPost.aspx";
                }

                indexItem.PageMetaDescription = blog.MetaDescription;
                indexItem.PageMetaKeywords = blog.MetaKeywords;
                indexItem.ItemId = blog.ItemId;
                indexItem.ModuleId = blog.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = blog.Title;
                indexItem.Content = blog.Description + " " + blog.MetaDescription + " " + blog.MetaKeywords;
                indexItem.FeatureId = blogFeatureGuid.ToString();
                indexItem.FeatureName = blogFeature.FeatureName;
                indexItem.FeatureResourceFile = blogFeature.ResourceFile;

                indexItem.OtherContent = stringBuilder.ToString();
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                if (blog.StartDate > pageModule.PublishBeginDate) { indexItem.PublishBeginDate = blog.StartDate; }

                IndexHelper.RebuildIndex(indexItem);
            }

            if (log.IsDebugEnabled) log.Debug("Indexed " + blog.Title);

        }

    }
}
