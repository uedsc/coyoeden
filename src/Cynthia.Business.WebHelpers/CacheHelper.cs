using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using log4net;
using Cynthia.Business.Statistics;
using Cynthia.Web.Framework;
using Rss;

namespace Cynthia.Business.WebHelpers
{
    public static class CacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CacheHelper));

        public static void RemoveAllCacheItems()
        {
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                String key = cacheEnum.Key.ToString();
                HttpRuntime.Cache.Remove(key);
            }
        }


        public static void TouchCacheFile(String pathToCacheFile)
        {
            if (pathToCacheFile == null) return;

            try
            {
                if (File.Exists(pathToCacheFile))
                {
                    File.SetLastWriteTimeUtc(pathToCacheFile, DateTime.UtcNow);
                }
                else
                {
                    File.CreateText(pathToCacheFile).Close();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error(ex);
            }
        }



        public static void SetClientCaching(HttpContext context, DateTime lastModified)
        {
            if (context == null) return;

            context.Response.Cache.SetETag(lastModified.Ticks.ToString());
            context.Response.Cache.SetLastModified(lastModified);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
            context.Response.Cache.SetSlidingExpiration(true);
        }


        public static void SetFileCaching(HttpContext context, string fileName)
        {
            if (fileName == null) return;
            if (context == null) return;
            if (fileName.Length == 0) return;

            context.Response.AddFileDependency(fileName);
            context.Response.Cache.SetETagFromFileDependencies();
            context.Response.Cache.SetLastModifiedFromFileDependencies();
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
            context.Response.Cache.SetSlidingExpiration(true);
        } 


        #region SiteSettings

        public static SiteSettings GetCurrentSiteSettings()
        {
            //return GetSiteSettingsFromCache();
            return GetSiteSettingsFromContext();
        }

        private static SiteSettings GetSiteSettingsFromContext()
        {
            if (HttpContext.Current == null) return null;

            SiteSettings siteSettings = HttpContext.Current.Items["SiteSettings"] as SiteSettings;
            if (siteSettings == null)
            {
                siteSettings = GetSiteSettingsFromCache();
                if (siteSettings != null)
                    HttpContext.Current.Items["SiteSettings"] = siteSettings;
            }
            return siteSettings;
        }


        private static SiteSettings GetSiteSettingsFromCache()
        {
    
            bool useFolderForSiteDetection = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);
            string cachekey;

            if (useFolderForSiteDetection)
            {
                string siteFolderName = VirtualFolderEvaluator.VirtualFolderName();
                if (siteFolderName.Length == 0) siteFolderName = "root";
                cachekey = "SiteSettings_" + siteFolderName;
            }
            else
            {
                String hostName = WebUtils.GetHostName();
                cachekey = "SiteSettings_" + hostName;
            }

            if (HttpRuntime.Cache[cachekey] == null)
            {
                int cacheTimeout;
                bool loadFromWebConfigSucceeded =
                    int.TryParse(ConfigurationManager.AppSettings["SiteSettingsCacheDurationInSeconds"], out cacheTimeout);
                if (!loadFromWebConfigSucceeded) cacheTimeout = 120;

                RefreshSiteSettingsCache(cachekey, cacheTimeout);
            }

            // Return SiteSettings object from cache, or null if it is not there for some reason
            return HttpRuntime.Cache[cachekey] as SiteSettings;
        }

        private static void RefreshSiteSettingsCache(String cacheKey, int cacheTimeout)
        {
            if (HttpContext.Current == null) return;

            SiteSettings siteSettings = LoadSiteSettings();
            if (siteSettings == null) return;

            String pathToCacheDependencyFile = HttpContext.Current.Server.MapPath(
                "~/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/sitesettingscachedependecy.config");

            if (pathToCacheDependencyFile != null)
            {
                EnsureCacheFile(pathToCacheDependencyFile);
            }

            CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyFile);
            
            DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
            TimeSpan slidingExpiration = TimeSpan.Zero;
            CacheItemPriority priority = CacheItemPriority.Default;
            CacheItemRemovedCallback callback = null;

            HttpRuntime.Cache.Insert(
                cacheKey,
                siteSettings,
                cacheDependency,
                absoluteExpiration,
                slidingExpiration,
                priority,
                callback);
        }


        private static SiteSettings LoadSiteSettings()
        {
            if (log.IsDebugEnabled) log.Debug("CacheHelper.cs LoadSiteSettings");

            SiteSettings siteSettings = null;

            try
            {
                bool useFolderForSiteDetection = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);
                
                string siteFolderName;
                if (useFolderForSiteDetection)
                {
                    siteFolderName = VirtualFolderEvaluator.VirtualFolderName();
                }
                else
                {
                    siteFolderName = string.Empty;
                }
                
                if (useFolderForSiteDetection)
                {
                    Guid siteGuid = SiteFolder.GetSiteGuid(siteFolderName);
                    siteSettings = new SiteSettings(siteGuid);
                }
                else
                {
                    siteSettings = new SiteSettings(WebUtils.GetHostName());
                }

                if (siteSettings.SiteId > -1)
                {
                    siteSettings.SiteRoot = WebUtils.GetSiteRoot();
                    if (useFolderForSiteDetection)
                    {
                        siteSettings.SiteFolderName = siteFolderName;
                    }
                    SetCurrentSkinBaseUrl(siteSettings);
                }
                else
                {
                    siteSettings = null;
                    log.Error("CacheHelper failed to load siteSettings");
                }
            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error("Error trying to obtain siteSettings", ex);
            }
            catch (InvalidOperationException ex)
            {
                log.Error("Error trying to obtain siteSettings", ex);
            }
            catch (IndexOutOfRangeException ex)
            {
                log.Error("Error trying to obtain siteSettings", ex);
            }

            return siteSettings;
        }

        /// <summary>
        /// This method should not normally be used, typically you should use the overload with no inputs.
        /// This one is only for supporting mutli sites with the same users across sites. In that case all users
        /// are attached to a specific site, so the membership provider calls this method
        /// to get a sitesettings object with the global security settings.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static SiteSettings GetSiteSettings(int siteId)
        {
           
            return GetSiteSettingsFromContext(siteId);
        }

        private static SiteSettings GetSiteSettingsFromContext(int siteId)
        {
            if (HttpContext.Current == null) return null;

            string contextKey = "SiteSettings" + siteId.ToString(CultureInfo.InvariantCulture);

            SiteSettings siteSettings = HttpContext.Current.Items[contextKey] as SiteSettings;
            if (siteSettings == null)
            {
                siteSettings = GetSiteSettingsFromCache(siteId);
                if (siteSettings != null)
                    HttpContext.Current.Items[contextKey] = siteSettings;
            }
            return siteSettings;
        }

        private static SiteSettings GetSiteSettingsFromCache(int siteId)
        {
            if (siteId == -1) { return null; }

            string cachekey = "SiteSettings_" + siteId.ToString(CultureInfo.InvariantCulture);
           
            if (HttpRuntime.Cache[cachekey] == null)
            {
                int cacheTimeout;
                bool loadFromWebConfigSucceeded =
                    int.TryParse(ConfigurationManager.AppSettings["SiteSettingsCacheDurationInSeconds"], out cacheTimeout);
                if (!loadFromWebConfigSucceeded) cacheTimeout = 120;

                RefreshSiteSettingsCache(siteId, cachekey, cacheTimeout);
            }

            // Return SiteSettings object from cache, or null if it is not there for some reason
            return HttpRuntime.Cache[cachekey] as SiteSettings;
        }

        private static void RefreshSiteSettingsCache(int siteId, String cacheKey, int cacheTimeout)
        {
            if (HttpContext.Current == null) return;

            SiteSettings siteSettings = new SiteSettings(siteId);
            

            String pathToCacheDependencyFile = HttpContext.Current.Server.MapPath(
                "~/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/relsitesettingscachedependecy.config");

            if (pathToCacheDependencyFile != null)
            {
                EnsureCacheFile(pathToCacheDependencyFile);
            }

            CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyFile);

            DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
            TimeSpan slidingExpiration = TimeSpan.Zero;
            CacheItemPriority priority = CacheItemPriority.Default;
            CacheItemRemovedCallback callback = null;

            HttpRuntime.Cache.Insert(
                cacheKey,
                siteSettings,
                cacheDependency,
                absoluteExpiration,
                slidingExpiration,
                priority,
                callback);
        }


        

        public static List<String> GetBannedIPList()
        {
            string cachekey = "bannedipaddresses" ;
            
            if (HttpRuntime.Cache[cachekey] == null)
            {
                List<String> bannedIPs = BannedIPAddress.GetAllBannedIPs();

                int cacheTimeout = 120;

                String pathToCacheDependencyFile 
                    = HttpContext.Current.Server.MapPath(
                "~/Data/bannedipcachedependency.config");

                if (pathToCacheDependencyFile != null)
                {
                    EnsureCacheFile(pathToCacheDependencyFile);
                }

                CacheDependency cacheDependency 
                    = new CacheDependency(pathToCacheDependencyFile);

                DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
                TimeSpan slidingExpiration = TimeSpan.Zero;
                CacheItemPriority priority = CacheItemPriority.Default;
                CacheItemRemovedCallback callback = null;

                HttpRuntime.Cache.Insert(
                    cachekey,
                    bannedIPs,
                    cacheDependency,
                    absoluteExpiration,
                    slidingExpiration,
                    priority,
                    callback);

            }

            return HttpRuntime.Cache[cachekey] as List<String>;
        }


        public static void SetCurrentSkinBaseUrl(SiteSettings siteSettings)
        {
            if (siteSettings == null) return;

            string currentSkin = siteSettings.Skin + "/";

            if (siteSettings.AllowUserSkins)
            {
                string skinCookieName = "CUserSkin" + siteSettings.SiteId.ToString();

                if (CookieHelper.CookieExists(skinCookieName))
                {
                    string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
                    if (cookieValue.Length > 0)
                    {
                        currentSkin = cookieValue + "/";
                    }
                }
            }

            string skinFolder = WebUtils.GetSiteRoot() + "/Data/Sites/"
                                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/skins/";

            siteSettings.SkinBaseUrl = skinFolder + currentSkin;
        }

        public static void ClearRelatedSiteCache(int relatedSiteId)
        {
            DataTable siteIds = SiteSettings.GetSiteIdList();

            foreach (DataRow row in siteIds.Rows)
            {
                int siteId = Convert.ToInt32(row["SiteID"]);

                if(siteId != relatedSiteId){ TouchSiteSettingsCacheDependencyFile(siteId); }

            }

        }


        public static void TouchSiteSettingsCacheDependencyFile()
        {
            if (HttpContext.Current == null) return;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return;

            TouchSiteSettingsCacheDependencyFile(siteSettings.SiteId);
        }

        public static void TouchSiteSettingsCacheDependencyFile(int siteId)
        {
            if (HttpContext.Current == null) return;

            string path = HttpContext.Current.Server.MapPath(
                "~/Data/Sites/"
                + siteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/sitesettingscachedependecy.config");

            TouchCacheFile(path);
        }

        #endregion

        #region MembershipStatistics

        public static MembershipStatistics GetCurrentMembershipStatistics()
        {
            return GetMembershipStatisticsFromCache();
        }


        private static MembershipStatistics GetMembershipStatisticsFromCache()
        {
            String hostName = WebUtils.GetHostName();
            string cachekey = "MembershipStatistics_" + hostName;

            if (HttpRuntime.Cache[cachekey] == null)
            {
                int cacheTimeout;
                bool loadFromWebConfigSucceeded =
                    int.TryParse(ConfigurationManager.AppSettings["SiteSettingsCacheDurationInSeconds"], out cacheTimeout);
                if (!loadFromWebConfigSucceeded) cacheTimeout = 120;

                RefreshMembershipStatisticsCache(cachekey, cacheTimeout);
            }

            // Return MembershipStatistics object from cache, or null if it is not there for some reason
            return HttpRuntime.Cache[cachekey] as MembershipStatistics;
        }

        private static void RefreshMembershipStatisticsCache(String cacheKey, int cacheTimeout)
        {
            if (HttpContext.Current == null) return;

            MembershipStatistics membershipStatistics = LoadMembershipStatistics();
            if (membershipStatistics == null) return;

            String pathToCacheDependencyFile 
                = GetPathToMembershipStatisticsCacheDependencyFile();

            if (pathToCacheDependencyFile != null)
            {
                EnsureCacheFile(pathToCacheDependencyFile);
            }

            CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyFile);

            DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
            TimeSpan slidingExpiration = TimeSpan.Zero;
            CacheItemPriority priority = CacheItemPriority.Default;
            CacheItemRemovedCallback callback = null;

            HttpRuntime.Cache.Insert(
                cacheKey,
                membershipStatistics,
                cacheDependency,
                absoluteExpiration,
                slidingExpiration,
                priority,
                callback);
        }


        private static MembershipStatistics LoadMembershipStatistics()
        {
            if (log.IsDebugEnabled) log.Debug("CacheHelper.cs LoadMembershipStatistics");

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            return new MembershipStatistics(
                siteSettings,
                DateTime.Today.ToUniversalTime().AddHours(DateTimeHelper.GetPreferredGmtOffset()));
        }


        public static void TouchMembershipStatisticsCacheDependencyFile()
        {
            TouchCacheFile(GetPathToMembershipStatisticsCacheDependencyFile());
        }

        private static string GetPathToMembershipStatisticsCacheDependencyFile()
        {
            if (HttpContext.Current == null) return null;
            
            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;
            
            return HttpContext.Current.Server.MapPath(
                "~/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/membershipstatisticscachedependecy.config");
        }

        #endregion

        #region SiteMap/Menu/PageSettings

        public static void ResetSiteMapCache()
        {
            TouchMenuCacheDependencyFile();
            TouchCacheFile(GetPathToSiteMapCacheDependencyFile());
        }

        public static void ResetSiteMapCache(int siteId)
        {
            TouchMenuCacheDependencyFile(siteId);
            TouchCacheFile(GetPathToSiteMapCacheDependencyFile(siteId));
        }

        public static string GetPathToSiteMapCacheDependencyFile()
        {
            if (HttpContext.Current == null) return null;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            return HttpContext.Current.Server.MapPath(
                "~/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) 
                + "/systemfiles/sitemapcachedependecy.config");
        }

        public static string GetPathToSiteMapCacheDependencyFile(int siteId)
        {
            if (HttpContext.Current == null) return null;

            return HttpContext.Current.Server.MapPath(
                "~/Data/Sites/" + siteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/sitemapcachedependecy.config");
        }
                

        public static PageSettings GetCurrentPage()
        {
            if (HttpContext.Current == null) return null;

            PageSettings currentPage = HttpContext.Current.Items["CurrentPage"] as PageSettings;
            if (currentPage == null)
            {
                currentPage = LoadCurrentPage();
                if (currentPage != null)
                    HttpContext.Current.Items["CurrentPage"] = currentPage;
            }
            return currentPage;
        }


        private static PageSettings LoadCurrentPage()
        {
            if (log.IsDebugEnabled) log.Debug("CacheHelper.cs LoadCurrentPage");

            int pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            bool useFolderForSiteDetection
                = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);
            string virtualFolder;

            if (useFolderForSiteDetection)
            {
                virtualFolder = VirtualFolderEvaluator.VirtualFolderName();
            }
            else
            {
                virtualFolder = string.Empty;
            }
                

            PageSettings currentPage = new PageSettings(siteSettings.SiteId, pageID);
            if (currentPage.SiteId != siteSettings.SiteId)
            {   // probably url manipulation trying to use a pageid that
                // doesn't belong to the site so just return the home page
                currentPage = new PageSettings(siteSettings.SiteId, -1);
            }
            
            //if (currentPage.SiteID != siteSettings.SiteID)
            //{
            //    // throwing exceptions is bad for performance, just redirect
            //    if (HttpContext.Current != null) WebUtils.SetupRedirect(null, WebUtils.GetSiteRoot());
            //    return null;
            //}

            if (
                (useFolderForSiteDetection)
                && (virtualFolder.Length > 0)
                && (currentPage.Url.StartsWith("~/"))
                )
            {
                currentPage.Url
                    = currentPage.Url.Replace("~/", "~/" + virtualFolder + "/");

                currentPage.UrlHasBeenAdjustedForFolderSites = true;
            }

            if (
                (useFolderForSiteDetection)
                && (virtualFolder.Length > 0)
                && (!currentPage.UseUrl)
                )
            {
                currentPage.Url
                    = "~/" + virtualFolder + "/Default.aspx?pageid="
                    + currentPage.PageId.ToString();
                currentPage.UseUrl = true;
                currentPage.UrlHasBeenAdjustedForFolderSites = true;
            }


            using (IDataReader reader = Module.GetPageModules(currentPage.PageId))
            {
                while (reader.Read())
                {
                    Module m = new Module();
                    m.ModuleId = Convert.ToInt32(reader["ModuleID"]);
                    m.ModuleGuid = new Guid(reader["Guid"].ToString());
                    m.ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]);
                    m.PageId = Convert.ToInt32(reader["PageID"]);
                    m.PaneName = reader["PaneName"].ToString();
                    m.ModuleTitle = reader["ModuleTitle"].ToString();
                    m.ViewRoles = reader["ViewRoles"].ToString();
                    m.AuthorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
                    m.DraftEditRoles = reader["DraftEditRoles"].ToString();
                    m.CacheTime = Convert.ToInt32(reader["CacheTime"]);
                    m.ModuleOrder = Convert.ToInt32(reader["ModuleOrder"]);

                    m.HideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
                    m.HideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);

                    if (reader["EditUserID"] != DBNull.Value)
                    {
                        m.EditUserId = Convert.ToInt32(reader["EditUserID"]);
                    }

                    string showTitle = reader["ShowTitle"].ToString();
                    m.ShowTitle = (showTitle == "True" || showTitle == "1");
                    m.ControlSource = reader["ControlSrc"].ToString();


                    currentPage.Modules.Add(m);
                }
            }

            return currentPage;
        }


        //called by sitemapprovider
        //called by IndexHelper
        public static Collection<PageSettings> GetMenuPages()
        {
            return GetMenuPagesFromContext();  
        }

        private static Collection<PageSettings> GetMenuPagesFromContext()
        {
            if (HttpContext.Current == null) return null;

            Collection<PageSettings> menuPages = HttpContext.Current.Items["MenuPages"] as Collection<PageSettings>;
            if (menuPages == null)
            {
                menuPages = GetMenuPagesFromCache();
                if (menuPages != null)
                    HttpContext.Current.Items["MenuPages"] = menuPages;
            }
            return menuPages;
        }

        private static Collection<PageSettings> GetMenuPagesFromCache()
        {
            // 2008-09-06 realized we are caching 2 copies of pages because we cache it here but also the SiteMap itself is cached
            // since sitemap is cached there is no need to cache this collection, it was just using memory to cache it but not really helping performance
            // if we find out the caching was beneficial we can turn it back on with this config setting
            bool cachePagesForMenu = ConfigHelper.GetBoolProperty("CachePagesForMenu", false);

            if (!cachePagesForMenu)
            {
                return LoadMenuPages();
            }

            string cachekey;
            bool useFolderForSiteDetection = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);

            if (useFolderForSiteDetection)
            {
                string siteFolderName = VirtualFolderEvaluator.VirtualFolderName();
                if (siteFolderName.Length == 0) siteFolderName = "root";
                cachekey = "MenuPages_" + siteFolderName;
            }
            else
            {
                String hostName = WebUtils.GetHostName();
                cachekey = "MenuPages_" + hostName;
            }

            if (HttpRuntime.Cache[cachekey] == null)
            {
                TouchMenuCacheDependencyFile();

                int cacheTimeout = 360;
                if (ConfigurationManager.AppSettings["MenuCacheDurationInSeconds"] != null)
                {
                    cacheTimeout = int.Parse(ConfigurationManager.AppSettings["MenuCacheDurationInSeconds"]);
                }

                RefreshMenuCache(cachekey, cacheTimeout);
            }

            return HttpRuntime.Cache[cachekey] as Collection<PageSettings>;
        }


        private static void RefreshMenuCache(String cacheKey, int cacheTimeout)
        {
            if (HttpContext.Current == null) return;

            Collection<PageSettings> menuPages = LoadMenuPages();
            if (menuPages == null) return;

            String pathToCacheDependencyFile = GetPathToMenuCacheDependencyFile();

            CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyFile);

            DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
            TimeSpan slidingExpiration = TimeSpan.Zero;
            CacheItemPriority priority = CacheItemPriority.Default;
            CacheItemRemovedCallback callback = null;

            HttpRuntime.Cache.Insert(
                cacheKey,
                menuPages,
                cacheDependency,
                absoluteExpiration,
                slidingExpiration,
                priority,
                callback);
        }


        private static Collection<PageSettings> LoadMenuPages()
        {
            Collection<PageSettings> menuPages 
                = new Collection<PageSettings>();
            
            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return menuPages;

            bool useFolderForSiteDetection 
                = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);
            string virtualFolder;
            if (useFolderForSiteDetection)
            {
                virtualFolder = VirtualFolderEvaluator.VirtualFolderName();
            }
            else
            {
                virtualFolder = string.Empty;
            }


            using (IDataReader reader = PageSettings.GetPageList(siteSettings.SiteId))
            {

                int i = 0;
                while (reader.Read())
                {
                    PageSettings pageDetails = new PageSettings();
                    pageDetails.SiteId = siteSettings.SiteId;
                    pageDetails.PageId = Convert.ToInt32(reader["PageID"]);
                    pageDetails.ParentId = Convert.ToInt32(reader["ParentID"]);
                    pageDetails.PageName = reader["PageName"].ToString();
                    pageDetails.MenuImage = reader["MenuImage"].ToString();
                    pageDetails.PageOrder = Convert.ToInt32(reader["PageOrder"]);
                    pageDetails.AuthorizedRoles = reader["AuthorizedRoles"].ToString();
                    pageDetails.EditRoles = reader["EditRoles"].ToString();
                    pageDetails.DraftEditOnlyRoles = reader["DraftEditRoles"].ToString();
                    pageDetails.CreateChildPageRoles = reader["CreateChildPageRoles"].ToString();

            
                    pageDetails.UseUrl = Convert.ToBoolean(reader["UseUrl"]);
                    pageDetails.Url = reader["Url"].ToString();
                    pageDetails.IncludeInMenu = Convert.ToBoolean(reader["IncludeInMenu"]);
                    pageDetails.IncludeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]);
                    pageDetails.IncludeInSearchMap = Convert.ToBoolean(reader["IncludeInSearchMap"]);
                    pageDetails.IsClickable = Convert.ToBoolean(reader["IsClickable"]);
                    pageDetails.ShowHomeCrumb = Convert.ToBoolean(reader["ShowHomeCrumb"]);
                    pageDetails.RequireSsl = Convert.ToBoolean(reader["RequireSSL"]);

                    if (
                        (useFolderForSiteDetection)
                        && (virtualFolder.Length > 0)
                        && (pageDetails.Url.StartsWith("~/"))
                        )
                    {
                        pageDetails.Url
                            = pageDetails.Url.Replace("~/", "~/" + virtualFolder + "/");
                    }

                    if (
                        (useFolderForSiteDetection)
                        && (virtualFolder.Length > 0)
                        && (!pageDetails.UseUrl)
                        )
                    {
                        pageDetails.Url
                            = "~/" + virtualFolder + "/Default.aspx?pageid="
                            + pageDetails.PageId.ToString();
                        pageDetails.UseUrl = true;
                    }

                    
                    pageDetails.OpenInNewWindow = Convert.ToBoolean(reader["OpenInNewWindow"]);
                    pageDetails.ShowChildPageMenu = Convert.ToBoolean(reader["ShowChildPageMenu"]);
                    pageDetails.ShowChildPageBreadcrumbs = Convert.ToBoolean(reader["ShowChildBreadCrumbs"]);
                    pageDetails.PageIndex = i;

                    string cf = reader["ChangeFrequency"].ToString();
                    switch (cf)
                    {
                        case "Always":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Always;
                            break;

                        case "Hourly":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Hourly;
                            break;

                        case "Daily":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Daily;
                            break;

                        case "Monthly":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Monthly;
                            break;

                        case "Yearly":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Yearly;
                            break;

                        case "Never":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Never;
                            break;

                        case "Weekly":
                        default:
                            pageDetails.ChangeFrequency = PageChangeFrequency.Weekly;
                            break;


                    }

                    string smp = reader["SiteMapPriority"].ToString().Trim();
                    if (smp.Length > 0) pageDetails.SiteMapPriority = smp;

                    if (reader["LastModifiedUTC"] != DBNull.Value)
                    {
                        pageDetails.LastModifiedUtc = Convert.ToDateTime(reader["LastModifiedUTC"]);
                    }

                    pageDetails.PageGuid = new Guid(reader["PageGuid"].ToString());
                    pageDetails.ParentGuid = new Guid(reader["ParentGuid"].ToString());

               
                    pageDetails.HideAfterLogin = Convert.ToBoolean(reader["HideAfterLogin"]);

                    pageDetails.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    pageDetails.CompiledMeta = reader["CompiledMeta"].ToString();
                    if (reader["CompiledMetaUtc"] != DBNull.Value)
                    {
                        pageDetails.CompiledMetaUtc = Convert.ToDateTime(reader["CompiledMetaUtc"]);
                    }

                    pageDetails.IsPending = Convert.ToBoolean(reader["IsPending"]);

                    menuPages.Add(pageDetails);
                    i++;
                }
            }

            return menuPages;
        }


        public static void TouchMenuCacheDependencyFile()
        {
            TouchCacheFile(GetPathToMenuCacheDependencyFile());
        }

        public static void TouchMenuCacheDependencyFile(int siteId)
        {
            TouchCacheFile(GetPathToMenuCacheDependencyFile(siteId));
        }


        private static string GetPathToMenuCacheDependencyFile()
        {
            if (HttpContext.Current == null) return null;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            return HttpContext.Current.Server.MapPath(
                "~/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/menucachedependecy.config");
        }

        private static string GetPathToMenuCacheDependencyFile(int siteId)
        {
            if (HttpContext.Current == null) return null;

            return HttpContext.Current.Server.MapPath(
                "~/Data/Sites/"
                + siteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/menucachedependecy.config");
        }

        #endregion

        #region Module Cache

        public static int GetDefaultModuleCacheTime()
        {
            int cacheTime;
            if (!int.TryParse(ConfigurationManager.AppSettings["DefaultModuleCacheDurationInSeconds"], out cacheTime))
                cacheTime = 360;
            return cacheTime;
        }

        public static String GetPathToCacheDependencyFile(String cacheDependencyKey)
        {
            if (HttpContext.Current == null) return null;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            return HttpContext.Current.Server.MapPath(
                "~/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/" + cacheDependencyKey + "cachedependecy.config");
        }


        


        public static void TouchCacheDependencyFile(String cacheDependencyKey)
        {
            if (HttpContext.Current == null) return;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return;

            string pathToCacheDependencyFile = HttpContext.Current.Server.MapPath(
                    "~/Data/Sites/"
                    + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                    + "/systemfiles/" + cacheDependencyKey + "cachedependecy.config");

            TouchCacheFile(pathToCacheDependencyFile);
        }


        public static void EnsureCacheFile(String pathToCacheFile)
        {
            if (pathToCacheFile == null) return;

            if (!File.Exists(pathToCacheFile))
            {
                TouchCacheFile(pathToCacheFile);
            }
        }


        #endregion

        #region Theme Cache

        public static void ResetThemeCache()
        {
            TouchCacheFile(GetPathToThemeCacheDependencyFile());
        }


        public static string GetPathToThemeCacheDependencyFile()
        {
            if (HttpContext.Current == null) return null;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            return HttpContext.Current.Server.MapPath(
                "~/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/systemfiles/themecachedependecy.config");
        }

        #endregion

       

    }
}
