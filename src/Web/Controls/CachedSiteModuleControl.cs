/// Created:			2004-12-26
/// Last Modified:		2007-08-16

using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web
{
	
	public class CachedSiteModuleControl : Control 
	{
		private Module  moduleConfiguration;
		private String  cachedOutput = null;
		private int     siteID = 0;
        private static readonly ILog log = LogManager.GetLogger(typeof(CachedSiteModuleControl));


        public Module ModuleConfiguration
        {
            get { return moduleConfiguration; }
            set { moduleConfiguration = value; }
        }

        public int ModuleId
        {
            get { return moduleConfiguration.ModuleId; }
        }

		public int PageId
		{
            get { return moduleConfiguration.PageId; }
		}

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

		public String CacheKey 
		{
			get 
			{
				return "Module-"
                    + CultureInfo.CurrentCulture.Name
                    + moduleConfiguration.ModuleId.ToString(CultureInfo.InvariantCulture) 
                    + HttpContext.Current.Request.QueryString
                    + WebUser.IsInRoles("Admins;Content Administrators;" + moduleConfiguration.AuthorizedEditRoles);
			}
		}

        public String CacheDependencyKey
        {
            get
            {
                return "Module-" + moduleConfiguration.ModuleId.ToString(CultureInfo.InvariantCulture);
            }
        }

		

		protected override void CreateChildControls()
		{

			if (
                (!Page.IsPostBack)
                &&(moduleConfiguration.CacheTime > 0)
                )
			{
				cachedOutput = (string) HttpRuntime.Cache[CacheKey];
			}

			if ((Page.IsPostBack)||(cachedOutput == null))
			{

				base.CreateChildControls();

				SiteModuleControl module = (SiteModuleControl) Page.LoadControl(moduleConfiguration.ControlSource);
                
				module.ModuleConfiguration = this.ModuleConfiguration;
				module.SiteId = this.SiteId;

                try
                {
                    this.Controls.Add(module);
                }
                catch (HttpException ex)
                {
                    // when searching using the search input from a page
                    // with viewstate enabled, this exception can be thrown
                    // because when the  SearchResults page accesses the Page.PreviousPage
                    // the viewstate is not the same
                    // the user never sees this and the search works since we catch
                    // the exception
                    if (log.IsErrorEnabled)
                    {
                        if (HttpContext.Current != null)
                        {
                            log.Error("Exception caught and handled in CachedSiteModule for requested url:" 
                                + HttpContext.Current.Request.RawUrl, ex);
                        }
                        else
                        {
                            log.Error("Exception caught and handled in CachedSiteModule", ex);
                        }
                    }

                }
			}
		}


		protected override void Render(HtmlTextWriter output)
		{
			if (
                (Page.IsPostBack)
                ||(moduleConfiguration.CacheTime == 0)
                )
			{
				base.Render(output);
				return;
			}

			if (cachedOutput == null)
			{

                TextWriter tempWriter = new StringWriter(CultureInfo.InvariantCulture);
				base.Render(new HtmlTextWriter(tempWriter));
				cachedOutput = tempWriter.ToString();

                String pathToCacheDependencyFile 
                    = CacheHelper.GetPathToCacheDependencyFile(CacheDependencyKey);

                if (pathToCacheDependencyFile != null)
                {
                    CacheHelper.EnsureCacheFile(pathToCacheDependencyFile);
                }

                //AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
                //aggregateCacheDependency.Add(new CacheDependency(pathToCacheDependencyFile));
                //// more dependencies can be added if needed

                //DateTime absoluteExpiration = DateTime.Now.AddSeconds(moduleConfiguration.CacheTime);
                //TimeSpan slidingExpiration = TimeSpan.Zero;
                //CacheItemPriority priority = CacheItemPriority.Default;
                //CacheItemRemovedCallback callback = null;

                //HttpRuntime.Cache.Insert(
                //    CacheKey,
                //    cachedOutput,
                //    aggregateCacheDependency,
                //    absoluteExpiration,
                //    slidingExpiration,
                //    priority,
                //    callback);

                 // Mono doesn't like aggregate dependencies
                // and we don't need it here.

                CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyFile);

                DateTime absoluteExpiration = DateTime.Now.AddSeconds(moduleConfiguration.CacheTime);
                TimeSpan slidingExpiration = TimeSpan.Zero;
                CacheItemPriority priority = CacheItemPriority.Default;
                CacheItemRemovedCallback callback = null;

                HttpRuntime.Cache.Insert(
                    CacheKey,
                    cachedOutput,
                    cacheDependency,
                    absoluteExpiration,
                    slidingExpiration,
                    priority,
                    callback);

			}

			output.Write(cachedOutput);
		}
	}

	


}
