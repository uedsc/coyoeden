// Author:             Joe Audette
// Created:            2006-01-19
// Last Modified:      2008-01-18

using System;
using System.Web;
using System.Web.Hosting;
using System.IO;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using log4net;

namespace Cynthia.Web
{
    /// <summary>
    /// theme is difficult to override for page or user specific C skins
    /// because theme is cached by the runtime so whatever the virtual path 
    /// provider returns initially is cached for a long time for other requests.
    /// I have come up with a solution for page specific skins so the theme served
    /// is correct but not for user specific skins
    /// 
    /// </summary>
    public class CThemeVirtualFile : VirtualFile
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CThemeVirtualFile));

        private String pathToFile;
        public CThemeVirtualFile(String virtualPath)
            : base(virtualPath)
        {
            pathToFile = virtualPath;
        }

        public override Stream Open()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            bool reset = false;
            if (siteSettings != null)
            {
                if (
                    (currentPage != null)
                    &&(siteSettings.AllowPageSkins)
                    &&(currentPage.Skin.Length > 0)
                    &&(pathToFile.Contains("App_Themes/pageskin"))
                    )
                {
                    pathToFile = pathToFile.Replace("App_Themes/pageskin", "Data/Sites/"
                       + siteSettings.SiteId.ToString()
                       + "/skins/" + currentPage.Skin);
                }
                else
                {
                    reset = true;
                    pathToFile = pathToFile.Replace("App_Themes/default", "Data/Sites/"
                       + siteSettings.SiteId.ToString()
                       + "/skins/" + siteSettings.Skin);
                }

            }
            if (reset)
            {
                try
                {
                    CacheHelper.ResetThemeCache();
                }
                catch (UnauthorizedAccessException ex)
                {
                    log.Error("Error trying to reset theme cache", ex);
                }
            }

            String filePath = HttpContext.Current.Server.MapPath(pathToFile);

            try
            {
                return File.OpenRead(filePath);
            }
            catch (DirectoryNotFoundException ex)
            {
                log.Error("Error trying to set theme", ex);
            }
            catch (FileNotFoundException ex)
            {
                log.Error("Error trying to set theme", ex);
            }

            if(HttpContext.Current != null)
            return File.OpenRead(HttpContext.Current.Server.MapPath(this.VirtualPath));

            return null;
           
        }

    }

}
