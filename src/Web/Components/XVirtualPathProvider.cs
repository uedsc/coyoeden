// Author:             Joe Audette
// Created:            2005-01-17
// Last Modified:      2008-01-18

using System;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class CVirtualPathProvider : VirtualPathProvider
    {
       
        /// <summary>
        ///  AppInitialize() is not used in Cynthia. It would be called on startup
        ///  if this file was in the App_Code folder but who wants a folder with a generic 
        ///  name like App_code and besides we can just as easily register our 
        ///  VirtualPathProvider in the Global.asax.cs Application_OnStart as we do
        ///
        /// </summary>
        public static void AppInitialize()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new CVirtualPathProvider());
        }

        public override string CombineVirtualPaths(string basePath, string relativePath)
        {
           
            return base.CombineVirtualPaths(basePath, relativePath);
          

        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            
            return base.GetDirectory(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (virtualPath == null) return null;

            // if in Medium trust
            if (ConfigurationManager.AppSettings["Lucene.Net.lockdir"] != null)
            {
                return base.GetFile(virtualPath);
            }

            // don't handle requests for css, its not needed
            // under IIS requests for .css are not handled by
            // the asp.net runtime anyway but under the VS web server
            // all files are so this is to prevent side effects of that
            // our StyleSheet user control correctly handles css
            // so we don't need to re-map files here
            String loweredPath = virtualPath.ToLower();
            if (
                (
                (virtualPath.Contains("App_Themes/default"))
                || (virtualPath.Contains("App_Themes/pageskin"))
                )
                && (!loweredPath.EndsWith(".css"))
                && (!loweredPath.EndsWith(".gif"))
                && (!loweredPath.EndsWith(".jpg"))
                && (!loweredPath.EndsWith(".png"))
                && (!loweredPath.EndsWith(".js"))
                && (!loweredPath.EndsWith(".ico"))
                && (!loweredPath.EndsWith(".axd"))
                )
            {
                try
                {
                    return new CThemeVirtualFile(virtualPath);
                }
                catch (System.UnauthorizedAccessException)
                { }
                
            }
            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies, 
            DateTime utcStart)
        {
            if (virtualPath.Contains("App_Themes/default"))
            {
                String pathToDependencyFile = CacheHelper.GetPathToThemeCacheDependencyFile();
                String pathToThemeFile = SiteUtils.GetFullPathToThemeFile();
                if(pathToDependencyFile != null)
                {
                    AggregateCacheDependency dependency = new AggregateCacheDependency();
                    dependency.Add(new CacheDependency(pathToDependencyFile));
                    try
                    {
                        dependency.Add(new CacheDependency(pathToThemeFile));
                    }
                    catch (HttpException)
                    { // this can happen if the site is configured for a skin that doesn't exist

                    }
                    return dependency;

                }
            }


            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

    }
}
