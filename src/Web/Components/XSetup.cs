// Author:					Joe Audette
// Created:				    2006-02-03
// Last Modified:		    2009-12-07
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.IO;
using System.Collections.ObjectModel;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web
{
   
    public sealed class CSetup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CSetup));

        public const string DefaultPageEncoding = "<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />";

        #region File System Tests

        private static void TestForWritableDataDirectory()
        {
            try
            {
                TouchTestFile();
            }
            catch(UnauthorizedAccessException ex)
            {
                throw new Exception("You need to make the Data folder and all its children writable by the ASP.NET worker process. ASPNET on XP, IISWPG on Win2003", ex);

            }

        }

        public static bool DataFolderIsWritable()
        {
            bool result = false;

            try
            {
                TouchTestFile();
                result = true;
            }
            catch (UnauthorizedAccessException){ }
            //catch (ArgumentNullException) { }
            //catch (NotSupportedException) { }
            //catch (ArgumentException) { }
            //catch (FileNotFoundException) { }

            return result;

        }

        public static void TouchTestFile(String pathToFile)
        {
            if (pathToFile != null)
            {

                if (File.Exists(pathToFile))
                {
                    File.SetLastWriteTimeUtc(pathToFile, DateTime.UtcNow);
                }
                else
                {
                    StreamWriter streamWriter = File.CreateText(pathToFile);
                    streamWriter.Close();
                }
            }

        }

        public static void TouchTestFile()
        {
            if (HttpContext.Current != null)
            {

                String pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/test.config");
                TouchTestFile(pathToTestFile);


                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Sites/1/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/Sites/1/"));
                }

                pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/test.config");
                TouchTestFile(pathToTestFile);

                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Sites/1/systemfiles/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/Sites/1/systemfiles/"));
                }

                pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/systemfiles/test.config");
                TouchTestFile(pathToTestFile);


                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Sites/1/GalleryImages/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/Sites/1/GalleryImages/"));
                }

                pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/GalleryImages/test.config");
                TouchTestFile(pathToTestFile);

                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Sites/1/FolderGalleries/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/Sites/1/FolderGalleries/"));
                }

                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/"));
                }

                pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/test.config");
                TouchTestFile(pathToTestFile);

                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/History/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/History/"));
                }

                pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/History/test.config");
                TouchTestFile(pathToTestFile);


                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Sites/1/index/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/Sites/1/index/"));
                }

                pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/index/test.config");
                TouchTestFile(pathToTestFile);
                if (File.Exists(pathToTestFile)) File.Delete(pathToTestFile);


            }
        }

        //public static bool VerifyGalleryFolders(int siteId, int moduleId)
        //{
        //    bool result = false;
        //    string galleryFolderPath = HttpContext.Current.Server.MapPath(GetApplicationRoot() + "/Data/Sites/")
        //        + siteId.ToString() + Path.DirectorySeparatorChar + "GalleryImages"
        //        + Path.DirectorySeparatorChar + moduleId.ToString() + Path.DirectorySeparatorChar;

        //    try
        //    {
        //        if (!Directory.Exists(galleryFolderPath))
        //        {
        //            Directory.CreateDirectory(galleryFolderPath);
        //            Directory.CreateDirectory(galleryFolderPath + "Thumbnails" + Path.DirectorySeparatorChar);
        //            Directory.CreateDirectory(galleryFolderPath + "WebImages" + Path.DirectorySeparatorChar);
        //            Directory.CreateDirectory(galleryFolderPath + "FullSizeImages" + Path.DirectorySeparatorChar);

        //        }

        //        result = true;
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        if (log.IsErrorEnabled)
        //        {
        //            log.Error("Error creating directories in SiteUtils.VerifyGalleryFolders", ex);
        //        }
        //    }

        //    return result;

        //}

        /// <summary>
        /// Added param NotUseFolders
        /// </summary>
        /// <param name="siteID"></param>
        /// <param name="moduleID"></param>
        /// <param name="notUseFolders"></param>
        /// <returns></returns>
        public static bool VerifyGalleryFolders(int siteId, int moduleId)
        {
            
            bool result = false;
            
            try
            {
                string galleryFolderPath;

                galleryFolderPath = HttpContext.Current.Server.MapPath("~/Data/Sites/" + siteId.ToInvariantString() + "/media/");
                if (!Directory.Exists(galleryFolderPath)) { Directory.CreateDirectory(galleryFolderPath); }

                if (WebConfigSettings.ImageGalleryUseMediaFolder)
                {
                    galleryFolderPath = HttpContext.Current.Server.MapPath("~/Data/Sites/"
                        + siteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/");
                }
                else
                {
                    galleryFolderPath = HttpContext.Current.Server.MapPath("~/Data/Sites/"
                        + siteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/");
                }

                if (!Directory.Exists(galleryFolderPath)) { Directory.CreateDirectory(galleryFolderPath); }

                string fullSizeImageFolderPath = galleryFolderPath + "FullSizeImages" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(fullSizeImageFolderPath)) { Directory.CreateDirectory(fullSizeImageFolderPath); }

                string webImagePath = galleryFolderPath + "WebImages" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(webImagePath)) { Directory.CreateDirectory(webImagePath); }

                string thumbnailPath = galleryFolderPath + "Thumbnails" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(thumbnailPath)) { Directory.CreateDirectory(thumbnailPath); }

                result = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error("Error creating directories in SiteUtils.VerifyGalleryFolders", ex);  
            }

            return result;

           
        }

        public static void EnsureFolderGalleryFolder(SiteSettings siteSettings)
        {
            if (HttpContext.Current == null) return;

            string path = "~/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) 
                + "/FolderGalleries/";

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            }


        }

        #endregion

        #region System Probing

        public static bool UpgradeIsNeeded()
        {
            bool result = false;

            Version dbCodeVersion = DatabaseHelper.DBCodeVersion();
            Version dbSchemaVersion = DatabaseHelper.DBSchemaVersion();
            if (dbCodeVersion > dbSchemaVersion) result = true;

            return result;
        }

        public static bool RunningInFullTrust()
        {
            bool result = false;

            AspNetHostingPermissionLevel currentTrustLevel = SecurityHelper.GetCurrentTrustLevel();

            if (currentTrustLevel == AspNetHostingPermissionLevel.Unrestricted) { result = true; }

            return result;

        }

        //public static bool RunningInFullTrust()
        //{
        //    bool result = true;
        //    // if reflection is not allowed, assume medium trust
        //    try
        //    {
        //        TestReflection();

        //    }
        //    catch (SecurityException ex)
        //    {
        //        log.Error("must be running in medium trust:", ex);
        //        result = false;
        //    }
        //    catch (System.Reflection.ReflectionTypeLoadException ex)
        //    {
        //        log.Error("must be running in medium trust:", ex);
        //        result = false;
        //    }


        //    return result;

        //}

        private static void TestReflection()
        {
#if ! MONO
            Collection<Type> webPartTypes = WebPartHelper.GetWebPartsFromAssemblies(WebConfigSettings.AssembliesNotSearchedForWebParts);
#endif
        }

        #endregion

        #region Schema Upgrade methods

        public static void DoSchemaUpgrade(String overrideConnectionInfo)
        {
            log.Debug("CSetup entered DoSchemaUpgrade");
            bool canAlterSchema = DatabaseHelper.CanAlterSchema(overrideConnectionInfo);

            if (
                (HttpContext.Current != null)
                && (canAlterSchema)
                )
            {
                Version currentSchemaVersion = DatabaseHelper.DBSchemaVersion();

                Guid appID = DatabaseHelper.GetApplicationId();
                String pathToScriptFolder
                    = HttpContext.Current.Server.MapPath("~/Setup/SchemaUpgradeScripts/"
                        + DatabaseHelper.DBPlatform().ToLowerInvariant()
                        + "/" + DatabaseHelper.GetApplicationName().ToLowerInvariant()
                        + "/");

                if (Directory.Exists(pathToScriptFolder))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(pathToScriptFolder);
                    FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");

                    foreach (FileInfo fileInfo in scriptFiles)
                    {
                        Version scriptVersion
                            = DatabaseHelper.ParseVersionFromFileName(fileInfo.Name);

                        if (
                            (scriptVersion != null)
                            && (scriptVersion > currentSchemaVersion)
                            && (!DatabaseHelper.SchemaScriptHasBeenRun(appID, fileInfo.Name))
                            )
                        {
                            DatabaseHelper.RunScript(appID, fileInfo, overrideConnectionInfo);
                            DoPostScriptTasks(scriptVersion, overrideConnectionInfo);
                        }

                    }

                }
                else
                {
                    log.Error("CSetup.DoSchemaUpgrade Error could not find path: " + pathToScriptFolder);
                }

            }
            else
            {
                log.Error("CSetup.DoSchemaUpgrade Error: no httpcontext or no permission to alter schema. ");
            }

        }

        public static void DoPostScriptTasks(
            Version scriptVersion,
            String overrideConnectionInfo)
        {
            if (scriptVersion == new Version(2, 2, 3, 0))
            {
                DatabaseHelper.DoVersion2230PostUpgradeTasks(overrideConnectionInfo);
            }

            if (scriptVersion == new Version(2, 2, 3, 4))
            {
                DatabaseHelper.DoVersion2234PostUpgradeTasks(overrideConnectionInfo);
            }

            if (scriptVersion == new Version(2, 2, 4, 7))
            {
                DatabaseHelper.DoVersion2247PostUpgradeTasks(overrideConnectionInfo);
            }

            if (scriptVersion == new Version(2, 2, 5, 3))
            {
                DatabaseHelper.DoVersion2253PostUpgradeTasks(overrideConnectionInfo);
            }

            if (scriptVersion == new Version(2, 3, 2, 0))
            {
                DatabaseHelper.DoVersion2320PostUpgradeTasks(overrideConnectionInfo);
            }


        }



        #endregion

        #region Create Initial Data

        public static SiteSettings CreateNewSite()
        {
            String templateFolderPath = GetMessageTemplateFolder();
            String templateFolder = templateFolderPath;

            SiteSettings newSite = new SiteSettings();

            newSite.SiteName = GetMessageTemplate(templateFolder, "InitialSiteNameContent.config");
           
            
            if (ConfigurationManager.AppSettings["DefaultInitialSkin"] != null)
            {
                newSite.Skin = ConfigurationManager.AppSettings["DefaultInitialSkin"];
            }
            else
            {
                newSite.Skin = GetMessageTemplate(templateFolder, "InitialSkinContent.config");
            }

            newSite.Logo = GetMessageTemplate(templateFolder, "InitialSiteLogoContent.config");

            newSite.AllowHideMenuOnPages = false;
            newSite.AllowNewRegistration = true;
            newSite.AllowPageSkins = false;
            newSite.AllowUserFullNameChange = false;
            newSite.AllowUserSkins = false;
            newSite.AutoCreateLdapUserOnFirstLogin = true;
            newSite.DefaultFriendlyUrlPattern = SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX;
            newSite.EditorSkin = SiteSettings.ContentEditorSkin.normal;
            //newSite.EncryptPasswords = false;
            newSite.Icon = String.Empty;
            newSite.IsServerAdminSite = true;
            newSite.ReallyDeleteUsers = true;
            newSite.SiteLdapSettings.Port = 389;
            newSite.SiteLdapSettings.RootDN = String.Empty;
            newSite.SiteLdapSettings.Server = String.Empty;
            newSite.UseEmailForLogin = true;
            newSite.UseLdapAuth = false;
            newSite.UseSecureRegistration = false;
            newSite.UseSslOnAllPages = WebConfigSettings.SslIsRequiredByWebServer;
            //newSite.CreateInitialDataOnCreate = false;

            newSite.AllowPasswordReset = true;
            newSite.AllowPasswordRetrieval = true;
            //0 = clear, 1= hashed, 2= encrypted
            newSite.PasswordFormat = WebConfigSettings.InitialSitePasswordFormat;

            newSite.RequiresQuestionAndAnswer = true;
            newSite.MaxInvalidPasswordAttempts = 10;
            newSite.PasswordAttemptWindowMinutes = 5;
            newSite.RequiresUniqueEmail = true;
            newSite.MinRequiredNonAlphanumericCharacters = 0;
            newSite.MinRequiredPasswordLength = 7;
            newSite.PasswordStrengthRegularExpression = String.Empty;
            newSite.DefaultEmailFromAddress = GetMessageTemplate(templateFolder, "InitialEmailFromContent.config");

            newSite.EnableMyPageFeature = false;


            newSite.Save();

            return newSite;


        }

        public static void CreateRequiredRolesAndAdminUser(SiteSettings site)
        {
            
            Role adminRole = new Role();
            adminRole.RoleName = "Admins";
            adminRole.SiteId = site.SiteId;
            adminRole.SiteGuid = site.SiteGuid;
            adminRole.Save();
            adminRole.RoleName = "Administrators";
            adminRole.Save();
            
            Role roleAdminRole = new Role();
            roleAdminRole.RoleName = "Role Admins";
            roleAdminRole.SiteId = site.SiteId;
            roleAdminRole.SiteGuid = site.SiteGuid;
            roleAdminRole.Save();
            roleAdminRole.RoleName = "Role Administrators";
            roleAdminRole.Save();
            

            Role contentAdminRole = new Role();
            contentAdminRole.RoleName = "Content Administrators";
            contentAdminRole.SiteId = site.SiteId;
            contentAdminRole.SiteGuid = site.SiteGuid;
            contentAdminRole.Save();

            Role authenticatedUserRole = new Role();
            authenticatedUserRole.RoleName = "Authenticated Users";
            authenticatedUserRole.SiteId = site.SiteId;
            authenticatedUserRole.SiteGuid = site.SiteGuid;
            authenticatedUserRole.Save();

            Role contentPublisherRole = new Role();
            contentPublisherRole.RoleName = "Content Publishers";
            contentPublisherRole.SiteId = site.SiteId;
            contentPublisherRole.SiteGuid = site.SiteGuid;
            contentPublisherRole.Save();

            Role contentAuthorRole = new Role();
            contentAuthorRole.RoleName = "Content Authors";
            contentAuthorRole.SiteId = site.SiteId;
            contentAuthorRole.SiteGuid = site.SiteGuid;
            contentAuthorRole.Save();


            Role newsletterAdminRole = new Role();
            newsletterAdminRole.RoleName = "Newsletter Administrators";
            newsletterAdminRole.SiteId = site.SiteId;
            newsletterAdminRole.SiteGuid = site.SiteGuid;
            newsletterAdminRole.Save();

            // if using related sites mode there is a problem if we already have user admin@admin.com
            // and we create another one in the child site with the same email and login so we need to make it different
            // we could just skip creating this user since in related sites mode all users come from the first site
            // but then if the config were changed to not related sites mode there would be no admin user
            // so in related sites mode we create one only as a backup in case settings are changed later
            int countOfSites = SiteSettings.SiteCount();
            string siteDifferentiator = string.Empty;
            if (
                (countOfSites >= 1)
                && (WebConfigSettings.UseRelatedSiteMode)
                )
            {
                if (site.SiteId > 1)
                {
                    siteDifferentiator = site.SiteId.ToInvariantString();
                }
            }

            CMembershipProvider membership = Membership.Provider as CMembershipProvider;
            bool overridRelatedSiteMode = true;
            SiteUser adminUser = new SiteUser(site, overridRelatedSiteMode);
            adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
            adminUser.Name = "Admin";
            adminUser.LoginName = "admin" + siteDifferentiator;
            adminUser.Password = "admin";

            if (membership != null)
                adminUser.Password = membership.EncodePassword("admin", site);

            adminUser.PasswordQuestion = "What is your user name?";
            adminUser.PasswordAnswer = "admin";
            adminUser.Save();

            Role.AddUser(adminRole.RoleId, adminUser.UserId, adminRole.RoleGuid, adminUser.UserGuid);
            
        }

        private static SiteUser EnsureAdminUser(SiteSettings site)
        {
            // if using related sites mode there is a problem if we already have user admin@admin.com
            // and we create another one in the child site with the same email and login so we need to make it different
            // we could just skip creating this user since in related sites mode all users come from the first site
            // but then if the config were changed to not related sites mode there would be no admin user
            // so in related sites mode we create one only as a backup in case settings are changed later
            int countOfSites = SiteSettings.SiteCount();
            string siteDifferentiator = string.Empty;
            if (
                (countOfSites >= 1)
                && (WebConfigSettings.UseRelatedSiteMode)
                )
            {
                siteDifferentiator = site.SiteId.ToString(CultureInfo.InvariantCulture);
            }

            CMembershipProvider membership = Membership.Provider as CMembershipProvider;
            bool overridRelatedSiteMode = true;
            SiteUser adminUser = new SiteUser(site, overridRelatedSiteMode);
            adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
            adminUser.Name = "Admin";
            adminUser.LoginName = "admin" + siteDifferentiator;
            bool userExists = false;
            if (site.UseEmailForLogin)
            {
                userExists = SiteUser.EmailExistsInDB(site.SiteId, adminUser.Email);
            }
            else
            {
                userExists = SiteUser.LoginExistsInDB(site.SiteId, adminUser.LoginName);
            }

            if (!userExists)
            {
                adminUser.Password = "admin";

                if (membership != null)
                    adminUser.Password = membership.EncodePassword("admin", site);

                adminUser.PasswordQuestion = "What is your user name?";
                adminUser.PasswordAnswer = "admin";
                adminUser.Save();

                //Role.AddUser(adminRole.RoleId, adminUser.UserId, adminRole.RoleGuid, adminUser.UserGuid);
            }
            else
            {
                if (site.UseEmailForLogin)
                {
                    adminUser = new SiteUser(site, adminUser.Email);
                }
                else
                {
                    adminUser = new SiteUser(site, adminUser.LoginName);
                }
            }

            return adminUser;

        }

        public static void EnsureRolesAndAdminUser(SiteSettings site)
        {
            SiteUser adminUser = EnsureAdminUser(site);

            if (!Role.Exists(site.SiteId, "Admins"))
            {
                Role adminRole = new Role();
                adminRole.RoleName = "Admins";
                adminRole.SiteId = site.SiteId;
                adminRole.SiteGuid = site.SiteGuid;
                adminRole.Save();
                adminRole.RoleName = "Administrators";
                adminRole.Save();

                Role.AddUser(adminRole.RoleId, adminUser.UserId, adminRole.RoleGuid, adminUser.UserGuid);

                // if using related sites mode there is a problem if we already have user admin@admin.com
                // and we create another one in the child site with the same email and login so we need to make it different
                // we could just skip creating this user since in related sites mode all users come from the first site
                // but then if the config were changed to not related sites mode there would be no admin user
                // so in related sites mode we create one only as a backup in case settings are changed later
                //int countOfSites = SiteSettings.SiteCount();
                //string siteDifferentiator = string.Empty;
                //if (
                //    (countOfSites >= 1)
                //    && (WebConfigSettings.UseRelatedSiteMode)
                //    )
                //{
                //    siteDifferentiator = site.SiteId.ToString(CultureInfo.InvariantCulture);
                //}

                //CMembershipProvider membership = Membership.Provider as CMembershipProvider;
                //bool overridRelatedSiteMode = true;
                //SiteUser adminUser = new SiteUser(site, overridRelatedSiteMode);
                //adminUser.Email = "admin" + siteDifferentiator + "@admin.com";
                //adminUser.Name = "Admin";
                //adminUser.LoginName = "admin" + siteDifferentiator;
                //bool userExists = false;
                //if (site.UseEmailForLogin)
                //{
                //    userExists = SiteUser.EmailExistsInDB(site.SiteId, adminUser.Email);
                //}
                //else
                //{
                //    userExists = SiteUser.LoginExistsInDB(site.SiteId, adminUser.LoginName);
                //}

                //if (!userExists)
                //{
                //    adminUser.Password = "admin";

                //    if (membership != null)
                //        adminUser.Password = membership.EncodePassword("admin", site);

                //    adminUser.PasswordQuestion = "What is your user name?";
                //    adminUser.PasswordAnswer = "admin";
                //    adminUser.Save();

                //    Role.AddUser(adminRole.RoleId, adminUser.UserId, adminRole.RoleGuid, adminUser.UserGuid);
                //}

            }

            if (!Role.Exists(site.SiteId, "Role Admins"))
            {
                Role roleAdminRole = new Role();
                roleAdminRole.RoleName = "Role Admins";
                roleAdminRole.SiteId = site.SiteId;
                roleAdminRole.SiteGuid = site.SiteGuid;
                roleAdminRole.Save();
                roleAdminRole.RoleName = "Role Administrators";
                roleAdminRole.Save();
            }

            if (!Role.Exists(site.SiteId, "Content Administrators"))
            {
                Role contentAdminRole = new Role();
                contentAdminRole.RoleName = "Content Administrators";
                contentAdminRole.SiteId = site.SiteId;
                contentAdminRole.SiteGuid = site.SiteGuid;
                contentAdminRole.Save();
            }

            if (!Role.Exists(site.SiteId, "Authenticated Users"))
            {
                Role authenticatedUserRole = new Role();
                authenticatedUserRole.RoleName = "Authenticated Users";
                authenticatedUserRole.SiteId = site.SiteId;
                authenticatedUserRole.SiteGuid = site.SiteGuid;
                authenticatedUserRole.Save();
            }

            if (!Role.Exists(site.SiteId, "Content Publishers"))
            {
                Role contentPublisherRole = new Role();
                contentPublisherRole.RoleName = "Content Publishers";
                contentPublisherRole.SiteId = site.SiteId;
                contentPublisherRole.SiteGuid = site.SiteGuid;
                contentPublisherRole.Save();
            }

            if (!Role.Exists(site.SiteId, "Content Authors"))
            {
                Role contentAuthorRole = new Role();
                contentAuthorRole.RoleName = "Content Authors";
                contentAuthorRole.SiteId = site.SiteId;
                contentAuthorRole.SiteGuid = site.SiteGuid;
                contentAuthorRole.Save();
            }

            if (!Role.Exists(site.SiteId, "Newsletter Administrators"))
            {
                Role newsletterAdminRole = new Role();
                newsletterAdminRole.RoleName = "Newsletter Administrators";
                newsletterAdminRole.SiteId = site.SiteId;
                newsletterAdminRole.SiteGuid = site.SiteGuid;
                newsletterAdminRole.Save();
            }

           

        }
        

        #endregion

        #region CreateNewSiteData(SiteSettings siteSettings)

        public static void CreateNewSiteData(SiteSettings siteSettings)
        {
            
            CreateRequiredRolesAndAdminUser(siteSettings);
            CreateDefaultSiteFolders(siteSettings.SiteId);
            CreateOrRestoreSiteSkins(siteSettings.SiteId);
            if (PageSettings.GetCountOfPages(siteSettings.SiteId) == 0)
            {
                SetupDefaultContentPages(siteSettings);
            }

        }

        public static void SetupDefaultContentPages(SiteSettings siteSettings)
        {
            ContentPageConfiguration appPageConfig
                = ContentPageConfiguration.GetConfig();

            foreach (ContentPage contentPage in appPageConfig.ContentPages)
            {
                PageSettings pageSettings = new PageSettings();
                if (PageSettings.Exists(contentPage.PageGuid))
                {
                    pageSettings.PageGuid = Guid.NewGuid();
                }
                else
                {
                    pageSettings.PageGuid = contentPage.PageGuid;
                    
                }

                if (contentPage.ParentGuid != Guid.Empty)
                {
                    
                    PageSettings parentPage = new PageSettings(contentPage.ParentGuid);
                    if (parentPage.SiteGuid == siteSettings.SiteGuid)
                    {
                        pageSettings.ParentGuid = parentPage.PageGuid;
                        pageSettings.ParentId = parentPage.PageId;
                    }

                }

                pageSettings.SiteId = siteSettings.SiteId;
                pageSettings.SiteGuid = siteSettings.SiteGuid;
                pageSettings.AuthorizedRoles = contentPage.VisibleToRoles;
                pageSettings.EditRoles = contentPage.EditRoles;
                pageSettings.DraftEditOnlyRoles = contentPage.DraftEditRoles;
                pageSettings.CreateChildPageRoles = contentPage.CreateChildPageRoles;
                pageSettings.MenuImage = contentPage.MenuImage;
                pageSettings.PageMetaKeyWords = contentPage.PageMetaKeyWords;
                pageSettings.PageMetaDescription = contentPage.PageMetaDescription;
                if (contentPage.ResourceFile.Length > 0)
                {
                    pageSettings.PageName
                        = ResourceHelper.GetResourceString(contentPage.ResourceFile, contentPage.Name);

                    if (contentPage.Title.Length > 0)
                    {
                        pageSettings.PageTitle
                        = ResourceHelper.GetResourceString(contentPage.ResourceFile, contentPage.Title);
                    }
                }
                else
                {
                    pageSettings.PageName = contentPage.Name;
                    pageSettings.PageTitle = contentPage.Title;
                }
                pageSettings.PageOrder = contentPage.PageOrder;

                pageSettings.Url = contentPage.Url;
                pageSettings.RequireSsl = contentPage.RequireSsl;
                pageSettings.Save();

                if (!FriendlyUrl.Exists(siteSettings.SiteId, pageSettings.Url))
                {
                    if (!WebPageInfo.IsPhysicalWebPage(pageSettings.Url))
                    {
                        FriendlyUrl friendlyUrl = new FriendlyUrl();
                        friendlyUrl.SiteId = siteSettings.SiteId;
                        friendlyUrl.SiteGuid = siteSettings.SiteGuid;
                        friendlyUrl.PageGuid = pageSettings.PageGuid;
                        friendlyUrl.Url = pageSettings.Url.Replace("~/", string.Empty);
                        friendlyUrl.RealUrl = "~/Default.aspx?pageid="
                            + pageSettings.PageId.ToString();
                        friendlyUrl.Save();
                    }
                }



                foreach (ContentPageItem pageItem in contentPage.PageItems)
                {
                    ModuleDefinition moduleDef
                        = new ModuleDefinition(pageItem.FeatureGuid);

                    // this only adds if its not already there
                    try
                    {
                        SiteSettings.AddFeature(siteSettings.SiteGuid, pageItem.FeatureGuid);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    if (moduleDef.ModuleDefId > -1)
                    {
                        Module module = new Module();
                        module.SiteId = siteSettings.SiteId;
                        module.SiteGuid = siteSettings.SiteGuid;
                        module.PageId = pageSettings.PageId;
                        module.ModuleDefId = moduleDef.ModuleDefId;
                        module.FeatureGuid = moduleDef.FeatureGuid;
                        module.PaneName = pageItem.Location;
                        if (contentPage.ResourceFile.Length > 0)
                        {
                            module.ModuleTitle
                                = ResourceHelper.GetResourceString(contentPage.ResourceFile, pageItem.ContentTitle);
                        }
                        else
                        {
                            module.ModuleTitle = pageItem.ContentTitle;
                        }
                        module.ModuleOrder = pageItem.SortOrder;
                        module.CacheTime = pageItem.CacheTimeInSeconds;
                        module.Icon = moduleDef.Icon;
                        module.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
                        module.Save();

                        // we can add any feature module to a page, but
                        // limited to adding content to html module for now
                        // maybe need to think about a way to let features
                        // define their own content format and just 
                        // pass a node of
                        // xml to the feature installer

                        Guid htmlContentGuid = new Guid("113FB01C-6408-4607-B0F7-1379E2512396");
                        if (
                            (pageItem.FeatureGuid == htmlContentGuid)
                            && (pageItem.ContentTemplate.EndsWith(".config"))
                            )
                        {
                            HtmlContent htmlContent = new HtmlContent();
                            htmlContent.ModuleId = module.ModuleId;
                            htmlContent.Body = ResourceHelper.GetMessageTemplate(ResourceHelper.GetDefaultCulture(), pageItem.ContentTemplate);
                            htmlContent.ModuleGuid = module.ModuleGuid;
                            HtmlRepository repository = new HtmlRepository();
                            repository.Save(htmlContent);
                            string cacheDependencyKey = "Module-" + module.ModuleId.ToString();
                            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                        }
                    }
                }
                
            }

            // this is only to make sure its initialized
            // before indexing is queued on a thread
            // because there is no HttpContext on
            // external threads and httpcontext is needed to initilaize
            // once initialized its cached
            //commented out 2009-05-11, the search index will be built on the first search
            //IndexBuilderProviderCollection
            //    indexProviders = IndexBuilderManager.Providers;

            //IndexHelper.VerifySearchIndex(siteSettings);

            CacheHelper.ResetSiteMapCache();
        }

       

        
        private static void EnsureSiteFeature(Guid siteGuid, Guid featureGuid)
        {
            if ((siteGuid == Guid.Empty) || (featureGuid == Guid.Empty)) return;

            SiteSettings.AddFeature(siteGuid, featureGuid);


        }



        

        #endregion



        #region Site Folder Creation

        public static bool CreateDefaultSiteFolders(int siteId)
        {
            return CreateDefaultSiteFolders(siteId, true);
        }

        public static bool CreateDefaultSiteFolders(
            int siteId, 
            bool includeStandardFiles)
        {
            if (HttpContext.Current == null) { return false; }

            string siteFolderPath = HttpContext.Current.Server.MapPath(GetApplicationRoot() + "/Data/Sites/") + siteId.ToString(CultureInfo.InvariantCulture) + Path.DirectorySeparatorChar;
            string sourceFilesPath = HttpContext.Current.Server.MapPath(GetApplicationRoot() + "/Data/");
            DirectoryInfo dir;
            FileInfo[] theFiles;
            //DirectoryInfo[] theDirectories;

            if (!Directory.Exists(siteFolderPath))
            {
                Directory.CreateDirectory(siteFolderPath);
            }

            //if (!Directory.Exists(siteFolderPath + "avatars"))
            //{
            //    Directory.CreateDirectory(siteFolderPath + "avatars");
            //}

            if (!Directory.Exists(siteFolderPath + "systemfiles"))
            {
                Directory.CreateDirectory(siteFolderPath + "systemfiles");
            }

            if (!Directory.Exists(siteFolderPath + "banners"))
            {
                Directory.CreateDirectory(siteFolderPath + "banners");
            }

            if (!Directory.Exists(siteFolderPath + "flash"))
            {
                Directory.CreateDirectory(siteFolderPath + "flash");
            }

            if (!Directory.Exists(siteFolderPath + "GalleryImages"))
            {
                Directory.CreateDirectory(siteFolderPath + "GalleryImages");
            }

            if (!Directory.Exists(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "EditorUploadImages"))
            {
                Directory.CreateDirectory(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "EditorUploadImages");
            }

            if (!Directory.Exists(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "FullSizeImages"))
            {
                Directory.CreateDirectory(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "FullSizeImages");
            }

            if (!Directory.Exists(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "Thumbnails"))
            {
                Directory.CreateDirectory(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "Thumbnails");
            }

            if (!Directory.Exists(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "WebImages"))
            {
                Directory.CreateDirectory(siteFolderPath + "GalleryImages" + Path.DirectorySeparatorChar + "WebImages");
            }

            if (!Directory.Exists(siteFolderPath + "logos"))
            {
                Directory.CreateDirectory(siteFolderPath + "logos");
            }

            if (!Directory.Exists(siteFolderPath + "htmlfragments"))
            {
                Directory.CreateDirectory(siteFolderPath + "htmlfragments");
            }

            if (!Directory.Exists(siteFolderPath + "index"))
            {
                Directory.CreateDirectory(siteFolderPath + "index");
            }

            if (!Directory.Exists(siteFolderPath + "SharedFiles"))
            {
                Directory.CreateDirectory(siteFolderPath + "SharedFiles");
            }

            if (!Directory.Exists(siteFolderPath + "SharedFiles" + Path.DirectorySeparatorChar + "History"))
            {
                Directory.CreateDirectory(siteFolderPath + "SharedFiles" + Path.DirectorySeparatorChar + "History");
            }

            if (!Directory.Exists(siteFolderPath + "skins"))
            {
                Directory.CreateDirectory(siteFolderPath + "skins");
            }

            if (!Directory.Exists(siteFolderPath + "xml"))
            {
                Directory.CreateDirectory(siteFolderPath + "xml");
            }

            if (!Directory.Exists(siteFolderPath + "xsl"))
            {
                Directory.CreateDirectory(siteFolderPath + "xsl");
            }

            

            if (includeStandardFiles)
            {
                //if (Directory.Exists(sourceFilesPath
                //    + Path.DirectorySeparatorChar + "avatars"))
                //{
                //    dir = new DirectoryInfo(sourceFilesPath 
                //        + Path.DirectorySeparatorChar + "avatars");

                //    theFiles = dir.GetFiles();
                //    string destinationFolder = siteFolderPath 
                //        + Path.DirectorySeparatorChar + "avatars" 
                //        + Path.DirectorySeparatorChar;

                //    foreach (FileInfo f in theFiles)
                //    {
                //        if (!File.Exists(destinationFolder + f.Name))
                //        {
                //            File.Copy(f.FullName, destinationFolder + f.Name, true);
                //        }
                //    }
                //}

               
                if (Directory.Exists(sourceFilesPath 
                    + Path.DirectorySeparatorChar + "logos"))
                {
                    dir = new DirectoryInfo(sourceFilesPath 
                        + Path.DirectorySeparatorChar + "logos");

                    theFiles = dir.GetFiles();
                    string destinationFolder = siteFolderPath 
                        + Path.DirectorySeparatorChar + "logos" 
                        + Path.DirectorySeparatorChar;

                    foreach (FileInfo f in theFiles)
                    {
                        if (!File.Exists(destinationFolder + f.Name))
                        {
                            File.Copy(f.FullName, destinationFolder + f.Name);
                        }
                    }
                }

                if (Directory.Exists(sourceFilesPath
                    + Path.DirectorySeparatorChar + "xml"))
                {
                    dir = new DirectoryInfo(sourceFilesPath
                        + Path.DirectorySeparatorChar + "xml");

                    theFiles = dir.GetFiles();
                    string destinationFolder = siteFolderPath
                        + Path.DirectorySeparatorChar + "xml"
                        + Path.DirectorySeparatorChar;

                    foreach (FileInfo f in theFiles)
                    {
                        if (!File.Exists(destinationFolder + f.Name))
                        {
                            File.Copy(f.FullName, destinationFolder + f.Name);
                        }
                    }
                }

                if (Directory.Exists(sourceFilesPath
                    + Path.DirectorySeparatorChar + "xsl"))
                {
                    dir = new DirectoryInfo(sourceFilesPath
                        + Path.DirectorySeparatorChar + "xsl");

                    theFiles = dir.GetFiles();
                    string destinationFolder = siteFolderPath
                        + Path.DirectorySeparatorChar + "xsl"
                        + Path.DirectorySeparatorChar;

                    foreach (FileInfo f in theFiles)
                    {
                        if (!File.Exists(destinationFolder + f.Name))
                        {
                            File.Copy(f.FullName, destinationFolder + f.Name);
                        }
                    }
                }

                if (Directory.Exists(sourceFilesPath
                    + Path.DirectorySeparatorChar + "htmlfragments"))
                {
                    dir = new DirectoryInfo(sourceFilesPath
                        + Path.DirectorySeparatorChar + "htmlfragments");

                    theFiles = dir.GetFiles();
                    string destinationFolder = siteFolderPath
                        + Path.DirectorySeparatorChar + "htmlfragments"
                        + Path.DirectorySeparatorChar;

                    foreach (FileInfo f in theFiles)
                    {
                        if (!File.Exists(destinationFolder + f.Name))
                        {
                            File.Copy(f.FullName, destinationFolder + f.Name);
                        }
                    }
                }

                

            }

            EnsureAdditionalSiteFolders(siteId);

            return true;
        }

        public static void EnsureAdditionalSiteFolders()
        {
            DataTable dataTable = SiteSettings.GetSiteIdList();
            foreach (DataRow row in dataTable.Rows)
            {
                int siteId = Convert.ToInt32(row["SiteID"]);
                EnsureAdditionalSiteFolders(siteId);

            }

        }

        public static void EnsureAdditionalSiteFolders(int siteId)
        {
            string siteFolderPath = HttpContext.Current.Server.MapPath(GetApplicationRoot() + "/Data/Sites/"
                    + siteId.ToString(CultureInfo.InvariantCulture) + "/");

            
            EnsureTemplateImageFolder(siteFolderPath);
            EnsureUserFilesFolder(siteFolderPath);
          

        }

        public static void EnsureTemplateImageFolder(string siteFolderPath)
        {
           
            try
            {
                if (Directory.Exists(siteFolderPath + "htmltemplateimages"))
                {
                    return;
                }
                else
                {
                    Directory.CreateDirectory(siteFolderPath + "htmltemplateimages");
                }

                string sourceFilesPath = HttpContext.Current.Server.MapPath(GetApplicationRoot() + "/Data/");

                DirectoryInfo dir;
                FileInfo[] theFiles;

                if (Directory.Exists(sourceFilesPath
                        + Path.DirectorySeparatorChar + "htmltemplateimages"))
                {
                    dir = new DirectoryInfo(sourceFilesPath
                        + Path.DirectorySeparatorChar + "htmltemplateimages");

                    theFiles = dir.GetFiles();
                    string destinationFolder = siteFolderPath
                        + Path.DirectorySeparatorChar + "htmltemplateimages"
                        + Path.DirectorySeparatorChar;

                    foreach (FileInfo f in theFiles)
                    {
                        if (!File.Exists(destinationFolder + f.Name))
                        {
                            File.Copy(f.FullName, destinationFolder + f.Name);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                log.Error("failed to ensure content template image folder or files", ex);
            }

        }

        public static void EnsureUserFilesFolder(string siteFolderPath)
        {
            
            if (Directory.Exists(siteFolderPath + "userfiles"))
            {
                return;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(siteFolderPath + "userfiles");
                }
                catch (IOException ex)
                {
                    log.Error("failed to ensure user files folder", ex);
                }
            }

        }

        public static void EnsureSkins(int siteId)
        {
            if (HttpContext.Current == null) { return; }

            string skinFolderPath = HttpContext.Current.Server.MapPath(GetApplicationRoot() + "/Data/Sites/") 
                + siteId.ToString(CultureInfo.InvariantCulture) 
                + Path.DirectorySeparatorChar
                + "skins";

            if (!Directory.Exists(skinFolderPath))
            {
                CreateOrRestoreSiteSkins(siteId);
            }

        }

        public static bool CreateOrRestoreSiteSkins(int siteId)
        {
            if (HttpContext.Current == null) { return false; }

            string siteFolderPath = HttpContext.Current.Server.MapPath(String.Format("{0}/Data/Sites/{1}/", GetApplicationRoot(),siteId.ToString(CultureInfo.InvariantCulture)));
            string sourceFilesPath = HttpContext.Current.Server.MapPath(String.Format("{0}/Data/", GetApplicationRoot()));
            var siteSkinFolderPath = String.Format("{0}skins", siteFolderPath);
            var srcSkinFolderPath = string.Format("{0}skins",sourceFilesPath);
            FileInfo[] theFiles;
            if (!Directory.Exists(srcSkinFolderPath)) return false;
            //create site skins
            if (!Directory.Exists(siteSkinFolderPath))
            {
                Directory.CreateDirectory(siteSkinFolderPath);
            }
            //copy skin files from master
            var dirDestination = new DirectoryInfo(siteSkinFolderPath);

            var srcDir = new DirectoryInfo(srcSkinFolderPath);

            var dirList = srcDir.GetDirectories();
            DirectoryInfo[] theSubDirectories;
            //root folders
            foreach (var d in dirList)
            {
                // don't want .svn OR _svn files
                if (d.Name.StartsWith(".") || d.Name.StartsWith("_")) continue;
                try
                {
                    dirDestination.CreateSubdirectory(d.Name);
                    theFiles = d.GetFiles();
                    foreach (FileInfo f in theFiles)
                    {
                        try
                        {
                            File.Copy(
                                f.FullName,
                                dirDestination.FullName + Path.DirectorySeparatorChar
                                + d.Name + Path.DirectorySeparatorChar + f.Name, true);
                        }
                        catch (UnauthorizedAccessException) { }
                        catch (System.IO.IOException) { }
                        //catch (System.IO.DirectoryNotFoundException) { }

                    }

                    //sub folders such as img/
                    theSubDirectories = d.GetDirectories();
                    foreach (DirectoryInfo sub in theSubDirectories)
                    {
                        if (sub.Name.StartsWith(".") || sub.Name.StartsWith("_")) { continue; } //.svn files
                        dirDestination.CreateSubdirectory(d.Name + Path.DirectorySeparatorChar + sub.Name);
                        theFiles = sub.GetFiles();
                        foreach (FileInfo f in theFiles)
                        {
                            try
                            {
                                File.Copy(
                                    f.FullName,
                                    dirDestination.FullName + Path.DirectorySeparatorChar
                                    + d.Name + Path.DirectorySeparatorChar + sub.Name + Path.DirectorySeparatorChar + f.Name, true);
                            }
                            catch (UnauthorizedAccessException) { }
                            catch (System.IO.IOException) { }
                            //catch (System.IO.DirectoryNotFoundException) { }

                        }
                    }

                }
                catch (System.Security.SecurityException ex)
                {
                    log.Error("error trying to copy skins into site skins folder ", ex);
                }
            }

            return true;
        }//CreateOrRestoreSiteSkins

        #endregion

        #region Newsletter Setup

        public static void CreateDefaultLetterTemplates(Guid siteGuid)
        {
            if (HttpContext.Current == null) return;

            string pathToTemplates = HttpContext.Current.Server.MapPath("~/Data/emailtemplates");

            if (!Directory.Exists(pathToTemplates)) return;

            DirectoryInfo dir = new DirectoryInfo(pathToTemplates);
            FileInfo[] templates = dir.GetFiles("*.config");
            foreach (FileInfo template in templates)
            {
                LetterHtmlTemplate emailTemplate = new LetterHtmlTemplate();
                emailTemplate.SiteGuid = siteGuid;
                emailTemplate.Title = template.Name.Replace(".config", string.Empty);
                StreamReader contentStream = template.OpenText();
                emailTemplate.Html = contentStream.ReadToEnd();
                contentStream.Close();
                emailTemplate.Save();


            }


        }

        #endregion

        #region Helper Methods

        public static string GetMessageTemplate(String templateFolder, String templateFile)
        {
            if (templateFile != null)
            {

                string culture = ConfigurationManager.AppSettings["Culture"];
                if (culture == null)
                {
                    culture = "en-US-";
                }

                string messageFile;

                if (File.Exists(templateFolder + culture + "-" + templateFile))
                {
                    messageFile = templateFolder + culture + "-" + templateFile;
                }
                else
                {
                    messageFile = templateFolder + "en-US-" + templateFile;

                }

                FileInfo file = new FileInfo(messageFile);
                StreamReader sr = file.OpenText();
                string message = sr.ReadToEnd();
                sr.Close();
                return message;
            }
            else
            {
                return String.Empty;
            }

        }

        public static String GetMessageTemplateFolder()
        {
            string result = String.Empty;

            if (HttpContext.Current != null)
            {
                result = HttpContext.Current.Server.MapPath(GetApplicationRoot()
                    + "/Data/MessageTemplates") + Path.DirectorySeparatorChar;
            }
            return result;

        }


        public static string GetApplicationRoot()
        {

            if (HttpContext.Current.Request.ApplicationPath.Length == 1)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
        }

        // Returns hostname[:port] to use when constructing the site root URL.
        private static string GetHost(string protocol)
        {
            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

            // Most proxies add an X-Forwarded-Host header which contains the original Host header
            // including any non-default port.

            string forwardedHosts = HttpContext.Current.Request.Headers["X-Forwarded-Host"];
            if (forwardedHosts != null)
            {
                // If the request passed thru multiple proxies, they will be separated by commas.
                // We only care about the first one.
                string forwardedHost = forwardedHosts.Split(',')[0];
                string[] serverAndPort = forwardedHost.Split(':');
                serverName = serverAndPort[0];
                serverPort = null;
                if (serverAndPort.Length > 1)
                {
                    serverPort = serverAndPort[1];

                }

            }

            // Only include a port if it is not the default for the protocol and MapAlternatePort = true
            // in the config file.
            if ((protocol == "http" && serverPort == "80")
                || (protocol == "https" && serverPort == "443"))
            {
                serverPort = null;
            }

            // added to fix issue reported by user running normal on port 80 but ssl on port 472
            if (protocol == "https" && serverPort == "80")
            {

                //string blnMapSSLPort = ConfigurationManager.AppSettings["MapAlternateSSLPort"];
                if (WebConfigSettings.MapAlternateSSLPort)
                {
                    string alternatSSLPort = ConfigurationManager.AppSettings["AlternateSSLPort"];
                    if (alternatSSLPort != null)
                    {
                        serverPort = alternatSSLPort;
                    }

                }


            }

            string host = serverName;

            if (serverPort != null)
            {
                //string mapPortSetting = ConfigurationManager.AppSettings["MapAlternatePort"];
                if (WebConfigSettings.MapAlternatePort)
                {
                    host += ":" + serverPort;
                }
            }
            return host;
        }

        public static string GetSiteRoot()
        {

            string protocol = "http";
            if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
            {
                protocol += "s";
            }

            string host = GetHost(protocol);
            return protocol + "://" + host + GetApplicationRoot();
        }

        public static string GetHostName()
        {
            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();

            return serverName;
        }

        public static string GetSecureSiteRoot()
        {
            string protocol = "https";
            string host = GetHost(protocol);
            return protocol + "://" + host + GetApplicationRoot();
        }

        public static string GetVirtualRoot()
        {
            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            return "/" + serverName + GetApplicationRoot();
        }



        #endregion









    }

}
