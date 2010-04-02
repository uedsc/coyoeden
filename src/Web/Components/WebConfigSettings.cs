using System;
using System.Configuration;
using Cynthia.Web.Framework;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web
{
    public static class WebConfigSettings
    {

        public static string AssembliesNotSearchedForWebParts
        {
            get
            {
                if (ConfigurationManager.AppSettings["AssembliesNotSearchedForWebParts"] != null)
                {
                    return ConfigurationManager.AppSettings["AssembliesNotSearchedForWebParts"];
                }
                return "AjaxControlToolkit.dll Argotic.Common.dll Argotic.Core.dll Argotic.Extensions.dll Blacklight.Silverlight.Controls.dll Brettle.Web.NeatHtml.dll Brettle.Web.NeatHtmlTools.dll Brettle.Web.NeatUpload.dll Brettle.Web.NeatUpload.GreyBoxProgressBar.dll CookComputing.XmlRpcV2.dll CSSFriendly.dll DayPilot.dll DotNetOpenAuth.dll DotNetOpenMail.dll GCheckout.dll Jayrock.dll Jayrock.Json.dll log4net.dll Lucene.Net.dll MetaDataExtractor.dll Microsoft.Web.Preview.dll Cynthia.Business.dll Cynthia.Business.WebHelpers.dll Cynthia.Data.dll Cynthia.Net.dll Cynthia.Web.Controls.dll Cynthia.Web.dll Cynthia.Web.Editor.dll Cynthia.Web.Framework.dll MySql.Data.dll Newtonsoft.Json.dll Novell.Directory.Ldap.dll OpenPOP.dll Org.Mentalis.Security.dll Cynthia.Features.Business.dll Cynthia.Features.Data.dll Cynthia.Features.UI.dll Recaptcha.dll RSS.NET.dll SharpMimeTools.dll SiteOffice.Business.dll SiteOffice.Data.dll SiteOffice.ExternalMail.dll SiteOffice.UI.dll sqlite3.dll Subkismet.dll System.Web.Extensions.dll WebStore.Business.dll WebStore.Data.dll WebStore.UI.dll FirebirdSql.Data.FirebirdClient.dll Mono.Data.Sqlite.dll Mono.Security.dll Npgsql.dll TimelineNet.dll ZedGraph.dll ZedGraph.Web.dll";
            }
        }

        public static bool RunningInMediumTrust
        {
            get { return ConfigHelper.GetBoolProperty("RunningInMediumTrust", false); }
        }

        public static bool EnableOpenIdAuthentication
        {
            get { return ConfigHelper.GetBoolProperty("EnableOpenIDAuthentication", false); }
        }

        public static bool DisableRpxAuthentication
        {
            get { return ConfigHelper.GetBoolProperty("DisableRpxAuthentication", false); }
        }

        public static bool ShowLegacyOpenIDSelector
        {
            get { return ConfigHelper.GetBoolProperty("ShowLegacyOpenIDSelector", false); }
        }

        public static bool DisableGoogleTranslate
        {
            get { return ConfigHelper.GetBoolProperty("DisableGoogleTranslate", false); }
        }

        public static bool UseOpenIdRpxSettingsFromWebConfig
        {
            get { return ConfigHelper.GetBoolProperty("UseOpenIdRpxSettingsFromWebConfig", false); }
        }

        /// <summary>
        /// If true then we will store our userGuid in the rpx server and allow multiple open id identifiers to be used
        /// with a single Cynthia account
        /// </summary>
        public static bool OpenIdRpxUseMappings
        {
            get { return ConfigHelper.GetBoolProperty("OpenIdRpxUseMappings", true); }
        }


        public static string OpenIdRpxApiKey
        {
            get
            {
                if (ConfigurationManager.AppSettings["OpenIdRpxApiKey"] != null)
                {
                    return ConfigurationManager.AppSettings["OpenIdRpxApiKey"];
                }
                return string.Empty;
            }
        }

        public static string WebSnaprKey
        {
            get
            {
                if (ConfigurationManager.AppSettings["WebSnaprKey"] != null)
                {
                    return ConfigurationManager.AppSettings["WebSnaprKey"];
                }
                return string.Empty;
            }
        }

        public static string OpenIdRpxApplicationName
        {
            get
            {
                if (ConfigurationManager.AppSettings["OpenIdRpxApplicationName"] != null)
                {
                    return ConfigurationManager.AppSettings["OpenIdRpxApplicationName"];
                }
                return string.Empty;
            }
        }

        public static bool EnableWindowsLiveAuthentication
        {
            get { return ConfigHelper.GetBoolProperty("EnableWindowsLiveAuthentication", false); }
        }

        public static bool HideDisableDbAuthenticationSetting
        {
            get { return ConfigHelper.GetBoolProperty("HideDisableDbAuthenticationSetting", false); }
        }


        public static bool GloballyDisableMemberUseOfWindowsLiveMessenger
        {
            get { return ConfigHelper.GetBoolProperty("GloballyDisableMemberUseOfWindowsLiveMessenger", false); }
        }

        public static bool TestLiveMessengerDelegation
        {
            get { return ConfigHelper.GetBoolProperty("TestLiveMessengerDelegation", false); }
        }

        public static bool EncodeLiveMessengerToken
        {
            get { return ConfigHelper.GetBoolProperty("EncodeLiveMessengerToken", false); }
        }

        public static bool DebugWindowsLive
        {
            get { return ConfigHelper.GetBoolProperty("DebugWindowsLive", false); }
        }

        public static bool DebugPayPal
        {
            get { return ConfigHelper.GetBoolProperty("DebugPayPal", false); }
        }

        public static bool DebugLoginRedirect
        {
            get { return ConfigHelper.GetBoolProperty("DebugLoginRedirect", false); }
        }

        public static bool EnableTaskQueueTestLinks
        {
            get { return ConfigHelper.GetBoolProperty("EnableTaskQueueTestLinks", false); }
        }


        public static bool DisableSetup
        {
            get { return ConfigHelper.GetBoolProperty("DisableSetup", false); }
        }

        public static bool DisableDBAdminTool
        {
            get { return ConfigHelper.GetBoolProperty("DisableDBAdminTool", true); }
        }

        public static bool MaskPasswordsInUserAdmin
        {
            get { return ConfigHelper.GetBoolProperty("MaskPasswordsInUserAdmin", true); }
        }

        

        public static bool ShowProviderListInDBAdminTool
        {
            get { return ConfigHelper.GetBoolProperty("ShowProviderListInDBAdminTool", false); }
        }

        public static bool ShowEmailInMemberList
        {
            get { return ConfigHelper.GetBoolProperty("ShowEmailInMemberList", false); }
        }

        public static bool ShowForumUnsubscribeLinkInUserManagement
        {
            get { return ConfigHelper.GetBoolProperty("ShowForumUnsubscribeLinkInUserManagement", true); }
        }

        public static bool ShowRevenueInForums
        {
            get { return ConfigHelper.GetBoolProperty("ShowRevenueInForums", false); }
        }

        public static bool GetAlphaPagerCharsFromResourceFile
        {
            get { return ConfigHelper.GetBoolProperty("GetAlphaPagerCharsFromResourceFile", false); }
        }

        public static string AlphaPagerChars
        {
            get { return ConfigHelper.GetStringProperty("AlphaPagerChars", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
        }

        public static bool HideMenusOnLoginPage
        {
            get { return ConfigHelper.GetBoolProperty("HideMenusOnLoginPage", true); }
        }

        public static bool HideMenusOnSiteMap
        {
            get { return ConfigHelper.GetBoolProperty("HideMenusOnSiteMap", true); }
        }

        public static bool HidePageMenusOnSiteMap
        {
            get { return ConfigHelper.GetBoolProperty("HidePageMenusOnSiteMap", true); }
        }

        public static bool HideMenusOnRegisterPage
        {
            get { return ConfigHelper.GetBoolProperty("HideMenusOnRegisterPage", true); }
        }

        public static bool HideMenusOnPasswordRecoveryPage
        {
            get { return ConfigHelper.GetBoolProperty("HideMenusOnPasswordRecoveryPage", true); }
        }

        public static bool HideMenusOnChangePasswordPage
        {
            get { return ConfigHelper.GetBoolProperty("HideMenusOnChangePasswordPage", true); }
        }

        public static bool HideAllMenusOnProfilePage
        {
            get { return ConfigHelper.GetBoolProperty("HideAllMenusOnProfilePage", false); }
        }

        public static bool HidePageMenuOnProfilePage
        {
            get { return ConfigHelper.GetBoolProperty("HidePageMenuOnProfilePage", true); }
        }

        public static bool HidePageMenuOnMemberListPage
        {
            get { return ConfigHelper.GetBoolProperty("HidePageMenuOnMemberListPage", true); }
        }

        public static bool HideAllMenusOnMyPage
        {
            get { return ConfigHelper.GetBoolProperty("HideAllMenusOnMyPage", false); }
        }

        public static bool HidePageViewModeIfNoWorkflowItems
        {
            get { return ConfigHelper.GetBoolProperty("HidePageViewModeIfNoWorkflowItems", true); }
        }

        public static bool ShowForumPostsInMemberList
        {
            get { return ConfigHelper.GetBoolProperty("ShowForumPostsInMemberList", true); }
        }

        

        public static bool ShowLoginNameInMemberList
        {
            get { return ConfigHelper.GetBoolProperty("ShowLoginNameInMemberList", false); }
        }

        public static bool ShowUserIDInMemberList
        {
            get { return ConfigHelper.GetBoolProperty("ShowUserIDInMemberList", false); }
        }

        

        public static bool ShowModuleTitlesByDefault
        {
            get { return ConfigHelper.GetBoolProperty("ShowModuleTitlesByDefault", true); }
        }

        public static string ModuleTitleTag
        {
            get { return ConfigHelper.GetStringProperty("ModuleTitleTag", "h2"); }
        }

        public static bool EnableDeveloperMenuInAdminMenu
        {
            get { return ConfigHelper.GetBoolProperty("EnableDeveloperMenuInAdminMenu", false); }
        }

        public static bool EnableQueryTool
        {
            get { return ConfigHelper.GetBoolProperty("EnableQueryTool", false); }
        }

        public static string QueryToolOverrideConnectionString
        {
            get
            {
                if (ConfigurationManager.AppSettings["QueryToolOverrideConnectionString"] != null)
                {
                    return ConfigurationManager.AppSettings["QueryToolOverrideConnectionString"];
                }
                // default value
                return string.Empty;
            }
        }

        public static string QueryToolMsSqlTableSelectSql
        {
            get
            {
                if (ConfigurationManager.AppSettings["QueryToolMsSqlTableSelectSql"] != null)
                {
                    return ConfigurationManager.AppSettings["QueryToolMsSqlTableSelectSql"];
                }
                // default value
                return "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE OBJECTPROPERTY(object_id(TABLE_NAME), N'IsUserTable') = 1 ORDER BY TABLE_NAME";
            }
        }

        public static string QueryToolMySqlTableSelectSql
        {
            get
            {
                if (ConfigurationManager.AppSettings["QueryToolMySqlTableSelectSql"] != null)
                {
                    return ConfigurationManager.AppSettings["QueryToolMySqlTableSelectSql"];
                }
                // default value
                return "SELECT DISTINCT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA <> 'mysql' ORDER BY TABLE_NAME;";
            }
        }

        public static string QueryToolPgSqlTableSelectSql
        {
            get
            {
                if (ConfigurationManager.AppSettings["QueryToolPgSqlTableSelectSql"] != null)
                {
                    return ConfigurationManager.AppSettings["QueryToolPgSqlTableSelectSql"];
                }
                // default value
                return "SELECT table_name AS TableName FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;";
            }
        }

        public static string QueryToolSqliteTableSelectSql
        {
            get
            {
                if (ConfigurationManager.AppSettings["QueryToolSqliteTableSelectSql"] != null)
                {
                    return ConfigurationManager.AppSettings["QueryToolSqliteTableSelectSql"];
                }
                // default value
                return "SELECT Name As TableName FROM sqlite_master WHERE type = 'table' ORDER BY name;";
            }
        }

        public static string QueryToolFirebirdSqlTableSelectSql
        {
            get
            {
                if (ConfigurationManager.AppSettings["QueryToolFirebirdSqlTableSelectSql"] != null)
                {
                    return ConfigurationManager.AppSettings["QueryToolFirebirdSqlTableSelectSql"];
                }
                // default value
                return "SELECT DISTINCT TRIM(RDB$RELATION_NAME) AS TableName FROM RDB$RELATION_FIELDS WHERE RDB$SYSTEM_FLAG=0;";
            }
        }

        public static string QueryToolSqAzureTableSelectSql
        {
            get
            {
                if (ConfigurationManager.AppSettings["QueryToolSqAzureTableSelectSql"] != null)
                {
                    return ConfigurationManager.AppSettings["QueryToolSqAzureTableSelectSql"];
                }
                // default value
                return "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE OBJECTPROPERTY(object_id(TABLE_NAME), N'IsUserTable') = 1 ORDER BY TABLE_NAME";
            }
        }


        public static bool EnableLogViewer
        {
            get { return ConfigHelper.GetBoolProperty("EnableLogViewer", true); }
        }

        public static bool UseCultureOverride
        {
            get { return ConfigHelper.GetBoolProperty("UseCultureOverride", false); }
        }

        public static bool IncludeTextSizeCss
        {
            get { return ConfigHelper.GetBoolProperty("IncludeTextSizeCss", false); }
        }

        public static bool CombineJavaScript
        {
            get { return ConfigHelper.GetBoolProperty("CombineJavaScript", false); }
        }

        public static string CKEditorSkin
        {
            get
            {
                if (ConfigurationManager.AppSettings["CKEditor:Skin"] != null)
                {
                    return ConfigurationManager.AppSettings["CKEditor:Skin"];
                }
                return "kama";
            }
        }


        public static string FCKeditorSkin
        {
            get
            {
                if (ConfigurationManager.AppSettings["FCKeditor:Skin"] != null)
                {
                    return ConfigurationManager.AppSettings["FCKeditor:Skin"];
                }
                return "normal";
            }
        }

        public static bool UseSkinCssInEditor
        {
            get { return ConfigHelper.GetBoolProperty("UseSkinCssInEditor", true); }
        }

        public static string EditorCssUrlOverride
        {
            get
            {
                if (ConfigurationManager.AppSettings["EditorCssUrlOverride"] != null)
                {
                    return ConfigurationManager.AppSettings["EditorCssUrlOverride"];
                }
                return string.Empty;
            }
        }

        public static string TinyMceBasePath
        {
            get
            {
                if (ConfigurationManager.AppSettings["TinyMCE:BasePath"] != null)
                {
                    return ConfigurationManager.AppSettings["TinyMCE:BasePath"];
                }
                return "/ClientScript/tiny_mce325/";
            }
        }

        public static string TinyMceSkin
        {
            get
            {
                if (ConfigurationManager.AppSettings["TinyMCE:Skin"] != null)
                {
                    return ConfigurationManager.AppSettings["TinyMCE:Skin"];
                }
                return "default";
            }
        }

        public static string TimelineBasePath
        {
            get
            {
                if (ConfigurationManager.AppSettings["TimelineBasePath"] != null)
                {
                    return ConfigurationManager.AppSettings["TimelineBasePath"];
                }
                return "~/ClientScript/timeline/";
            }
        }

        public static string jQueryBasePath
        {
            get
            {
                if (ConfigurationManager.AppSettings["jQueryBasePath"] != null)
                {
                    return ConfigurationManager.AppSettings["jQueryBasePath"];
                }
                return "~/ClientScript/jquery126/";
            }
        }

        //public static string ExtJsBasePath
        //{
        //    get { return ConfigHelper.GetStringProperty("ExtJsBasePath", "~/ClientScript/ext-2.0.2/"); }
        //}


        public static bool UseHtml5
        {
            get { return ConfigHelper.GetBoolProperty("UseHtml5", false); }
        }

        public static bool DisableHtmlValidatorLink
        {
            get { return ConfigHelper.GetBoolProperty("DisableHtmlValidatorLink", false); }
        }

        public static bool DisableCssValidatorLink
        {
            get { return ConfigHelper.GetBoolProperty("DisableCssValidatorLink", true); }
        }
        

        public static bool CombineCSS
        {
            get { return ConfigHelper.GetBoolProperty("CombineCSS", true); }
        }

        public static bool CacheCssOnServer
        {
            get { return ConfigHelper.GetBoolProperty("CacheCssOnServer", true); }
        }

        public static bool CacheCssInBrowser
        {
            get { return ConfigHelper.GetBoolProperty("CacheCssInBrowser", true); }
        }

        public static int CssCacheInDays
        {
            get { return ConfigHelper.GetIntProperty("CssCacheInDays", 7); }
        }

        public static bool MinifyCSS
        {
            get { return ConfigHelper.GetBoolProperty("MinifyCSS", true); }
        }

        public static bool DisableASPThemes
        {
            get { return ConfigHelper.GetBoolProperty("DisableASPThemes", false); }
        }

        public static bool UsingOlderSkins
        {
            get { return ConfigHelper.GetBoolProperty("UsingOlderSkins", false); }
        }

        public static bool MenusAreResponsibleForAddingCss
        {
            get { return ConfigHelper.GetBoolProperty("MenusAreResponsibleForAddingCss", false); }
        }

        /// <summary>
        /// allows diabling the rendering of text in the menu for ths request:
        /// http://www.vivasky.com/Forums/Thread.aspx?thread=2824&mid=34&pageid=5&ItemID=4&pagenumber=1#post12578
        /// </summary>
        public static bool RenderMenuText
        {
            get { return ConfigHelper.GetBoolProperty("RenderMenuText", true); }
        }

        public static bool AllowChangingFriendlyUrlPattern
        {
            get { return ConfigHelper.GetBoolProperty("AllowChangingFriendlyUrlPattern", false); }
        }


        public static bool AllowMultipleSites
        {
            get { return ConfigHelper.GetBoolProperty("AllowMultipleSites", false); }
        }

        public static bool ShowSiteGuidInSiteSettings
        {
            get { return ConfigHelper.GetBoolProperty("ShowSiteGuidInSiteSettings", false); }
        }

        public static bool EnableSiteSettingsSmtpSettings
        {
            get { return ConfigHelper.GetBoolProperty("EnableSiteSettingsSmtpSettings", false); }
        }

        public static bool EnforceContentVersioningGlobally
        {
            get { return ConfigHelper.GetBoolProperty("EnforceContentVersioningGlobally", false); }
        }

        public static bool MaskSmtpPasswordInSiteSettings
        {
            get { return ConfigHelper.GetBoolProperty("MaskSmtpPasswordInSiteSettings", true); }
        }

        public static bool ShowSmtpEncodingOption
        {
            get { return ConfigHelper.GetBoolProperty("ShowSmtpEncodingOption", false); }
        }

        public static bool HideGoogleAnalyticsInChildSites
        {
            get { return ConfigHelper.GetBoolProperty("HideGoogleAnalyticsInChildSites", false); }
        }
        

        //public static bool AllowGravatars
        //{
        //    get { return ConfigHelper.GetBoolProperty("AllowGravatars", true); }
        //}

        public static string GravatarMaxAllowedRating
        {
            get
            {
                if (ConfigurationManager.AppSettings["GravatarMaxAllowedRating"] != null)
                {
                    return ConfigurationManager.AppSettings["GravatarMaxAllowedRating"];
                }
                // default value
                return "G";
            }
        }

        ///// <summary>
        ///// deprecated
        ///// </summary>
        //public static bool DisableOldCheesyAvatars
        //{
        //    get { return ConfigHelper.GetBoolProperty("DisableOldCheesyAvatars", true); }
        //}

        

        public static bool OnlyAdminsCanEditCheesyAvatars
        {
            get { return ConfigHelper.GetBoolProperty("OnlyAdminsCanEditCheesyAvatars", false); }
        }

        public static bool UseSslForMyPage
        {
            get { return ConfigHelper.GetBoolProperty("UseSslForMyPage", false); }
        }

        public static bool UseSslForSiteOffice
        {
            get { return ConfigHelper.GetBoolProperty("UseSslForSiteOffice", false); }
        }

        public static bool UseSslForMemberList
        {
            get { return ConfigHelper.GetBoolProperty("UseSslForMemberList", false); }
        }
        

        public static bool EnableSslInChildSites
        {
            get { return ConfigHelper.GetBoolProperty("EnableSSLInChildSites", false); }
        }

        public static bool AllowDeletingChildSites
        {
            get { return ConfigHelper.GetBoolProperty("AllowDeletingChildSites", false); }
        }

        public static bool DeleteSiteFolderWhenDeletingSites
        {
            get { return ConfigHelper.GetBoolProperty("DeleteSiteFolderWhenDeletingSites", false); }
        }

        public static bool ShowSkinRestoreButtonInSiteSettings
        {
            get { return ConfigHelper.GetBoolProperty("ShowSkinRestoreButtonInSiteSettings", false); }
        }

        public static bool AllowFileManagerInChildSites
        {
            get { return ConfigHelper.GetBoolProperty("AllowFileManagerInChildSites", false); }
        }


        public static bool AllowUserProfilePage
        {
            get { return ConfigHelper.GetBoolProperty("AllowUserProfilePage", true); }
        }

        public static bool AllowPasswordFormatChange
        {
            get { return ConfigHelper.GetBoolProperty("AllowPasswordFormatChange", true); }
        }

        /// <summary>
        /// 0 = clear, 1= hashed, 2= encrypted
        /// </summary>
        public static int InitialSitePasswordFormat
        {
            //changed default to hashed 2009-02-25
            // changed default to encrypted 2009-05-08
            //http://www.vivasky.com/Forums/Thread.aspx?thread=2902&mid=34&pageid=5&ItemID=5&pagenumber=1
            // changed back to clear text 2009-05-25 because of too many support requests where people end up
            // getting the admin user locked out
            get { return ConfigHelper.GetIntProperty("InitialSitePasswordFormat", 0); }
        }

        public static bool AllowPasswordFormatChangeInChildSites
        {
            get { return ConfigHelper.GetBoolProperty("AllowPasswordFormatChangeInChildSites", false); }
        }

        public static bool ShowSystemInformationInChildSiteAdminMenu
        {
            get { return ConfigHelper.GetBoolProperty("ShowSystemInformationInChildSiteAdminMenu", true); }
        }

        public static bool ShowConnectionErrorOnSetup
        {
            get { return ConfigHelper.GetBoolProperty("ShowConnectionErrorOnSetup", false); }
        }

        public static bool NotifyAdminsOnNewUserRegistration
        {
            get { return ConfigHelper.GetBoolProperty("NotifyAdminsOnNewUserRegistration", false); }
        }

        /// <summary>
        /// a comma separated list of email addresses to exclude when sending
        /// administrative emails including registration notifications and content workflow submissions
        /// this is for when you have admin user accounts that you do not want to receive these emails
        /// </summary>
        public static string EmailAddressesToExcludeFromAdminNotifications
        {
            get
            {
                if (ConfigurationManager.AppSettings["EmailAddressesToExcludeFromAdminNotifications"] != null)
                {
                    return ConfigurationManager.AppSettings["EmailAddressesToExcludeFromAdminNotifications"];
                }

                return string.Empty;
            }
        }

        public static bool DisableDotNetOpenMail
        {
            get { return ConfigHelper.GetBoolProperty("DisableDotNetOpenMail", false); }
        }

        public static bool ShowHistoryOnUpgradePage
        {
            get { return ConfigHelper.GetBoolProperty("ShowHistoryOnUpgradePage", false); }
        }

        public static bool UseFoldersInsteadOfHostnamesForMultipleSites
        {
            get { return ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false); }
        }

        public static bool UseSiteNameForRootBreadcrumb
        {
            get { return ConfigHelper.GetBoolProperty("UseSiteNameForRootBreadcrumb", false); }
        }

        public static bool UseRelatedSiteMode
        {
            get { return ConfigHelper.GetBoolProperty("UseRelatedSiteMode", false); }
        }

        public static bool UseRpxNowForOpenId
        {
            get { return ConfigHelper.GetBoolProperty("UseRpxNowForOpenId", false); }
        }

        public static int RelatedSiteID
        {
            get { return ConfigHelper.GetIntProperty("RelatedSiteID", 1); }
        }

        public static bool RelatedSiteModeHideRoleManagerInChildSites
        {
            get { return ConfigHelper.GetBoolProperty("RelatedSiteModeHideRoleManagerInChildSites", true); }
        }

        public static bool UseUrlReWriting
        {
            get { return ConfigHelper.GetBoolProperty("UseUrlReWriting", true); }
        }

        public static bool DisableUseOfPassedInDateForMetaWeblogApi
        {
            get { return ConfigHelper.GetBoolProperty("DisableUseOfPassedInDateForMetaWeblogApi", false); }
        }


        public static bool DisableHelpSystem
        {
            get { return ConfigHelper.GetBoolProperty("DisableHelpSystem", false); }
        }

        public static bool DisableWorkflowNotification
        {
            get { return ConfigHelper.GetBoolProperty("DisableWorkflowNotification", false); }
        }

        public static bool RenderModulePanel
        {
            get { return ConfigHelper.GetBoolProperty("RenderModulePanel", true); }
        }

        public static bool HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins
        {
            get { return ConfigHelper.GetBoolProperty("HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins", false); }
        }

        public static bool Disable301Redirector
        {
            get { return ConfigHelper.GetBoolProperty("Disable301Redirector", false); }
        }

        public static bool DisableTaskQueue
        {
            get { return ConfigHelper.GetBoolProperty("DisableTaskQueue", false); }
        }

        public static bool UsePerSiteTaskQueue
        {
            get { return ConfigHelper.GetBoolProperty("UsePerSiteTaskQueue", false); }
        }

        public static string CynthiacombinedScriptVersion
        {
            get
            {
                if (ConfigurationManager.AppSettings["CynthiacombinedScriptVersion"] != null)
                {
                    return ConfigurationManager.AppSettings["CynthiacombinedScriptVersion"];
                }
                // default value
                return string.Empty;
            }
        }

        public static bool UseGoogleCDN
        {
            get { return ConfigHelper.GetBoolProperty("UseGoogleCDN", true); }
        }

        public static string GoogleCDNjQueryVersion
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleCDNjQueryVersion"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleCDNjQueryVersion"];
                }
                // default value
                return "1.4.2";
            }
        }

        public static string GoogleCDNjQueryUIVersion
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleCDNjQueryUIVersion"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleCDNjQueryUIVersion"];
                }
                // default value
                return "1.7.2";
            }
        }



        /// <summary>
        /// In IIS 7 Integrated mode if you want to use the App Keep alive feature you need to specify
        /// the root url of your site in this setting like http://yoursiteroot/Default.aspx
        /// </summary>
        public static string AppKeepAliveUrl
        {
            get
            {
                if (ConfigurationManager.AppSettings["AppKeepAliveUrl"] != null)
                {
                    return ConfigurationManager.AppSettings["AppKeepAliveUrl"];
                }
                // default value
                return string.Empty;
            }
        }

        /// <summary>
        /// If true the MeatContentControl will render the content type meta tag. Set to false if you would rather specify it in the layout.master
        /// </summary>
        public static bool AutoSetContentType
        {
            get { return ConfigHelper.GetBoolProperty("AutoSetContentType", true); }
        }

        public static string ContentMimeType
        {
            get
            {
                if (ConfigurationManager.AppSettings["ContentMimeType"] != null)
                {
                    return ConfigurationManager.AppSettings["ContentMimeType"];
                }
                // default value
                return "application/xhtml+xml";
            }
        }

        public static string ContentEncoding
        {
            get
            {
                if (ConfigurationManager.AppSettings["ContentEncoding"] != null)
                {
                    return ConfigurationManager.AppSettings["ContentEncoding"];
                }
                // default value
                return "utf-8";
            }
        }


        public static bool UseAppKeepAlive
        {
            get { return ConfigHelper.GetBoolProperty("UseAppKeepAlive", false); }
        }

        public static int AppKeepAliveSleepMinutes
        {
            get { return ConfigHelper.GetIntProperty("AppKeepAliveSleepMinutes", 10); }
        }

        public static int AppKeepAliveMaxRunTimeMinutes
        {
            get { return ConfigHelper.GetIntProperty("AppKeepAliveMaxRunTimeMinutes", 720); }
        }


        public static bool AllowPersistentLoginCookie
        {
            get { return ConfigHelper.GetBoolProperty("AllowPersistentLoginCookie", true); }
        }
        

        public static bool AssignNewPagesParentPageSkinByDefault
        {
            get { return ConfigHelper.GetBoolProperty("AssignNewPagesParentPageSkinByDefault", true); }
        }

        public static bool AllowAnonymousUsersToViewMemberList
        {
            get { return ConfigHelper.GetBoolProperty("AllowAnonymousUsersToViewMemberList", false); }
        }

        public static bool AutoGenerateAndHideUserNamesWhenUsingEmailForLogin
        {
            get { return ConfigHelper.GetBoolProperty("AutoGenerateAndHideUserNamesWhenUsingEmailForLogin", false); }
        }

        public static bool DisablePageViewStateByDefault
        {
            get { return ConfigHelper.GetBoolProperty("DisablePageViewStateByDefault", false); }
        }

        

        public static bool DisableContentCache
        {
            get { return ConfigHelper.GetBoolProperty("DisableContentCache", true); }
        }

        public static bool RedirectHomeFromSetupPagesWhenSystemIsUpToDate
        {
            get { return ConfigHelper.GetBoolProperty("RedirectHomeFromSetupPagesWhenSystemIsUpToDate", true); }
        }

        public static bool AutoSuggestFriendlyUrls
        {
            get { return ConfigHelper.GetBoolProperty("AutoSuggestFriendlyUrls", true); }
        }

        public static bool AppendDateToBlogUrls
        {
            get { return ConfigHelper.GetBoolProperty("AppendDateToBlogUrls", false); }
        }

        public static bool AllowForcingPreferredHostName
        {
            get { return ConfigHelper.GetBoolProperty("AllowForcingPreferredHostName", false); }
        }

        public static bool ForceSingleSessionPerUser
        {
            get { return ConfigHelper.GetBoolProperty("ForceSingleSessionPerUser", false); }
        }

        /// <summary>
        /// You should not call this directly, instead use SiteUtils.SslIsAvailable()
        /// we now support separate ssl settings per site with Web.config like this:
        /// Site1-SSLIsAvailable, Site2-SSLIsAvailable etc, and trhe siteutils method resolves this
        /// </summary>
        public static bool SslisAvailable
        {
            get { return ConfigHelper.GetBoolProperty("SSLIsAvailable", false); }
        }

        /// <summary>
        /// if IIS or apache is set to require ssl for all pages then set thsi to true.
        /// </summary>
        public static bool SslIsRequiredByWebServer
        {
            get { return ConfigHelper.GetBoolProperty("SSLIsRequiredByWebServer", false); }
        }

        public static bool ForceSslOnAllPages
        {
            get { return ConfigHelper.GetBoolProperty("ForceSslOnAllPages", false); }
        }

        /// <summary>
        /// If the current reqest is using https, then a relative ulr for all menu items will resolve to https
        /// This setting enables chaning to fully qualified urls in the menus to avoid this
        /// which in turn avoids an unneeded redirect to enforce or clear ssl
        /// </summary>
        public static bool ResolveFullUrlsForMenuItemProtocolDifferences
        {
            get { return ConfigHelper.GetBoolProperty("ResolveFullUrlsForMenuItemProtocolDifferences", true); }
        }

        public static bool ForceSslOnProfileView
        {
            get { return ConfigHelper.GetBoolProperty("ForceSslOnProfileView", false); }
        }

        /// <summary>
        /// The title element of an html page should not exceed 65 chars.
        /// Ideally you should set this to true
        /// </summary>
        public static bool AutoTruncatePageTitles
        {
            get { return ConfigHelper.GetBoolProperty("AutoTruncatePageTitles", false); }
        }

        public static bool ShowRebuildReportsButton
        {
            get { return ConfigHelper.GetBoolProperty("ShowRebuildReportsButton", false); }
        }

        public static bool UseShortcutKeys
        {
            get { return ConfigHelper.GetBoolProperty("UseShortcutKeys", false); }
        }

        public static string AdminImage
        {
            get { return ConfigHelper.GetStringProperty("AdminImage", "admin.png"); }
        }

        public static string PageTreeImage
        {
            get { return ConfigHelper.GetStringProperty("PageTreeImage", "sitemap.png"); }
        }

        public static string EditContentImage
        {
            get { return ConfigHelper.GetStringProperty("EditContentImage", "pencil.png"); }
        }

        public static string EditPageFeaturesImage
        {
            get { return ConfigHelper.GetStringProperty("EditPageFeaturesImage", "page_edit.png"); }
        }

        public static string EditPageSettingsImage
        {
            get { return ConfigHelper.GetStringProperty("EditPageSettingsImage", "page_gear.png"); }
        }

        public static string EditPropertiesImage
        {
            get { return ConfigHelper.GetStringProperty("EditPropertiesImage", "wrench.png"); }
        }

        public static string DeleteLinkImage
        {
            get { return ConfigHelper.GetStringProperty("DeleteLinkImage", "trash.png"); }
        }

        public static string RSSImageFileName
        {
            get { return ConfigHelper.GetStringProperty("RSSImageFileName", "feed.png"); }
        }

        
        

        public static string NewThreadImage
        {
            get { return ConfigHelper.GetStringProperty("NewThreadImage", "folder_edit.png"); }
        }

        public static string ForumThreadImage
        {
            get { return ConfigHelper.GetStringProperty("ForumThreadImage", "folder.png"); }
        }

        

        public static bool UseIconsForAdminLinks
        {
            get { return ConfigHelper.GetBoolProperty("UseIconsForAdminLinks", true); }
        }

        public static bool UsePageImagesInSiteMap
        {
            get { return ConfigHelper.GetBoolProperty("UsePageImagesInSiteMap", false); }
        }

        public static bool TreatChildPageIndexAsSiteMap
        {
            get { return ConfigHelper.GetBoolProperty("TreatChildPageIndexAsSiteMap", false); }
        }

       
        public static bool UseTextLinksForFeatureSettings
        {
            get { return ConfigHelper.GetBoolProperty("UseTextLinksForFeatureSettings", true); }
        }

        public static bool UseSiteMailFeature
        {
            get { return ConfigHelper.GetBoolProperty("UseSiteMailFeature", false); }
        }

        public static bool LogErrorsFrom404Handler
        {
            get { return ConfigHelper.GetBoolProperty("LogErrorsFrom404Handler", false); }
        }


        public static bool TrackAuthenticatedRequests
        {
            get { return ConfigHelper.GetBoolProperty("TrackAuthenticatedRequests", true); }
        }

        public static bool TrackIPForAuthenticatedRequests
        {
            get { return ConfigHelper.GetBoolProperty("TrackIPForAuthenticatedRequests", false); }
        }

        public static string GoogleAnalyticsScriptOverrideUrl
        {
            get { return ConfigHelper.GetStringProperty("GoogleAnalyticsScriptOverrideUrl", string.Empty); }
        }

        public static bool TrackPageLoadTimeInGoogleAnalytics
        {
            get { return ConfigHelper.GetBoolProperty("TrackPageLoadTimeInGoogleAnalytics", false); }
        }

        public static bool LogGoogleAnalyticsDataToLocalWebLog
        {
            get { return ConfigHelper.GetBoolProperty("LogGoogleAnalyticsDataToLocalWebLog", false); }
        }

        public static bool SiteStatisticsShowMemberStatisticsDefault
        {
            get { return ConfigHelper.GetBoolProperty("SiteStatistics_ShowMemberStatistics_Default", true); }
        }

        public static bool SiteStatisticsShowOnlineStatisticsDefault
        {
            get { return ConfigHelper.GetBoolProperty("SiteStatistics_ShowOnlineStatistics_Default", true); }
        }

        public static bool SiteStatisticsShowOnlineMembersDefault
        {
            get { return ConfigHelper.GetBoolProperty("SiteStatistics_ShowOnlineMembers_Default", true); }
        }

        public static bool DisableFileManager
        {
            get { return ConfigHelper.GetBoolProperty("DisableFileManager", false); }
        }

        public static bool ShowFileManagerLink
        {
            get { return ConfigHelper.GetBoolProperty("ShowFileManagerLink", true); }
        }

        public static bool ShowServerPathInFileManager
        {
            get { return ConfigHelper.GetBoolProperty("ShowServerPathInFileManager", true); }
        }

        public static bool UseGreyBoxProgressForNeatUpload
        {
            get { return ConfigHelper.GetBoolProperty("UseGreyBoxProgressForNeatUpload", false); }
        }

        public static bool DisableSearchIndex
        {
            get { return ConfigHelper.GetBoolProperty("DisableSearchIndex", false); }
        }

        public static bool DisableOpenSearchAutoDiscovery
        {
            get { return ConfigHelper.GetBoolProperty("DisableOpenSearchAutoDiscovery", false); }
        }

        public static bool ShowModuleTitleInSearchResultLink
        {
            get { return ConfigHelper.GetBoolProperty("ShowModuleTitleInSearchResultLink", false); }
        }

        /// <summary>
        /// As of version 2.3.0.5, we changed the way role filtering is done in search results.
        /// We set the default to true here so that excisting indexes will not be broken.
        /// Ideaaly uoi will set this to false after rebuilding the index.
        /// Rebuilding the index will make it compatible with he new method of role filtering
        /// which prduces accurate results counts
        /// </summary>
        public static bool SearchUseBackwardCompatibilityMode
        {
            get { return ConfigHelper.GetBoolProperty("SearchUseBackwardCompatibilityMode", true); }
        }

        /// <summary>
        /// generally we should not include the page meta because it can result in duplicate results
        /// one for each instance of html content on the page because they all use the same page meta from the parent page.
        /// since page meta should reflect the content of the page it is sufficient to just index the content
        /// </summary>
        public static bool IndexPageMeta
        {
            get { return ConfigHelper.GetBoolProperty("IndexPageMeta", false); }
        }

        

        /// <summary>
        /// in a cluster, only one node should have this set to true
        /// clusters are only supported if they share a common file system (as of 2009-09-25)
        /// and in this configuration we should let just one node be responsible for indexing the content.
        /// </summary>
        public static bool IsSearchIndexingNode
        {
            get { return ConfigHelper.GetBoolProperty("IsSearchIndexingNode", true); }
        }

        /// <summary>
        /// disabled by default for backawards compatibility with existing indexes.
        /// if you set this to true in Web.config/user.config you should rebuild the index
        /// http://www.vivasky.com/rebuilding-the-search-index.aspx
        /// </summary>
        public static bool EnableSearchResultsHighlighting
        {
            get { return ConfigHelper.GetBoolProperty("EnableSearchResultsHighlighting", false); }
        }

        /// <summary>
        /// disabled by default for backawards compatibility with existing indexes.
        /// if you set this to false in Web.config/user.config you should rebuild the index
        /// http://www.vivasky.com/rebuilding-the-search-index.aspx
        /// </summary>
        public static bool DisableSearchFeatureFilters
        {
            get { return ConfigHelper.GetBoolProperty("DisableSearchFeatureFilters", true); }
        }

        
        public static string SearchableFeatureGuidsToExclude
        {
            get
            {
                if (ConfigurationManager.AppSettings["SearchableFeatureGuidsToExclude"] != null)
                {
                    return ConfigurationManager.AppSettings["SearchableFeatureGuidsToExclude"];
                }

                return string.Empty;
            }
        }

        public static bool DisableYUI
        {
            get { return ConfigHelper.GetBoolProperty("DisableYUI", false); }
        }

        public static bool AlwaysLoadYuiTabs
        {
            get { return ConfigHelper.GetBoolProperty("AlwaysLoadYuiTabs", false); }
        }

        public static bool EnableDragDropPageLayout
        {
            get { return ConfigHelper.GetBoolProperty("EnableDragDropPageLayout", false); }
        }

        public static bool OpenSearchDownloadLinksInNewWindow
        {
            get { return ConfigHelper.GetBoolProperty("OpenSearchDownloadLinksInNewWindow", true); }
        }

        public static bool DisablejQuery
        {
            get { return ConfigHelper.GetBoolProperty("DisablejQuery", false); }
        }

        public static bool DisableSwfObject
        {
            get { return ConfigHelper.GetBoolProperty("DisableSwfObject", false); }
        }

        public static bool DisablejQueryUI
        {
            get { return ConfigHelper.GetBoolProperty("DisablejQueryUI", false); }
        }

        public static bool DisableExternalCommentSystems
        {
            get { return ConfigHelper.GetBoolProperty("DisableExternalCommentSystems", false); }
        }

        public static bool DisableOomph
        {
            get { return ConfigHelper.GetBoolProperty("DisableOomph", false); }
        }

        /// <summary>
        /// http://www.websnapr.com/
        /// </summary>
        public static bool DisableWebSnapr
        {
            get { return ConfigHelper.GetBoolProperty("DisableWebSnapr", false); }
        }

        

        public static bool AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin
        {
            get { return ConfigHelper.GetBoolProperty("AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin", false); }
        }

        public static bool EnableNewsletter
        {
            get { return ConfigHelper.GetBoolProperty("EnableNewsletter", true); }
        }

        public static bool EnableContentWorkflow
        {
            get { return ConfigHelper.GetBoolProperty("EnableContentWorkflow", true); }
        }

        public static bool PromptBeforeUnsubscribeNewsletter
        {
            get { return ConfigHelper.GetBoolProperty("PromptBeforeUnsubscribeNewsletter", false); }
        }

        public static bool EnableBlogSiteMap
        {
            get { return ConfigHelper.GetBoolProperty("EnableBlogSiteMap", true); }
        }

        public static bool EnableWoopraGlobally
        {
            get { return ConfigHelper.GetBoolProperty("EnableWoopraGlobally", false); }
        }

        public static bool DisableWoopraGlobally
        {
            get { return ConfigHelper.GetBoolProperty("DisableWoopraGlobally", false); }
        }

        public static bool EnableGoogle404Enhancement
        {
            get { return ConfigHelper.GetBoolProperty("EnableGoogle404Enhancement", true); }
        }

        public static bool SuppressMenuOnBuiltIn404Page
        {
            get { return ConfigHelper.GetBoolProperty("SuppressMenuOnBuiltIn404Page", true); }
        }

        

        public static bool UseOfficeFeature
        {
            get { return ConfigHelper.GetBoolProperty("UseOfficeFeature", false); }
        }

        public static bool UseSilverlightSiteOffice
        {
            get { return ConfigHelper.GetBoolProperty("UseSilverlightSiteOffice", false); }
        }

        public static bool ForceFCKToDegradeToTextAreaInSafari
        {
            get { return ConfigHelper.GetBoolProperty("ForceFCKToDegradeToTextAreaInSafari", true); }
        }

        public static bool ForceFCKToDegradeToTextAreaInOpera
        {
            get { return ConfigHelper.GetBoolProperty("ForceFCKToDegradeToTextAreaInOpera", true); }
        }

        public static bool ForceTinyMCEInSafari
        {
            get { return ConfigHelper.GetBoolProperty("ForceTinyMCEInSafari", false); }
        }

        /// <summary>
        /// 2009-06-10 changed default from true to false as it works in testing the latest FCKeditor and Opera
        /// </summary>
        public static bool ForceTinyMCEInOpera
        {
            get { return ConfigHelper.GetBoolProperty("ForceTinyMCEInOpera", false); }
        }

        public static bool ForcePlainTextInIphone
        {
            get { return ConfigHelper.GetBoolProperty("ForcePlainTextInIphone", true); }
        }

        public static bool ForcePlainTextInAndroid
        {
            get { return ConfigHelper.GetBoolProperty("ForcePlainTextInAndroid", true); }
        }


        public static bool MapAlternatePort
        {
            get { return ConfigHelper.GetBoolProperty("MapAlternatePort", true); }
        }

        public static bool MapAlternateSSLPort
        {
            get { return ConfigHelper.GetBoolProperty("MapAlternateSSLPort", true); }
        }


        public static int SearchResultsPageSize
        {
            get { return ConfigHelper.GetIntProperty("SearchResultsPageSize", 10); }
        }

        public static int SearchResultsFragmentSize
        {
            get { return ConfigHelper.GetIntProperty("SearchResultsFragmentSize", 300); }
        }

        public static int ContentCatalogPageSize
        {
            get { return ConfigHelper.GetIntProperty("ContentCatalogPageSize", 15); }
        }

        public static int ContentStyleTemplatePageSize
        {
            get { return ConfigHelper.GetIntProperty("ContentStyleTemplatePageSize", 15); }
        }

        public static int ContentTemplatePageSize
        {
            get { return ConfigHelper.GetIntProperty("ContentTemplatePageSize", 5); }
        }

        public static bool ContentTemplateShowBodyInAdminList
        {
            get { return ConfigHelper.GetBoolProperty("ContentTemplateShowBodyInAdminList", true); }
        }

        public static bool AddSystemContentTemplatesAboveSiteTemplates
        {
            get { return ConfigHelper.GetBoolProperty("AddSystemContentTemplatesAboveSiteTemplates", true); }
        }

        public static bool AddSystemContentTemplatesBelowSiteTemplates
        {
            get { return ConfigHelper.GetBoolProperty("AddSystemContentTemplatesBelowSiteTemplates", false); }
        }

        public static bool AddSystemStyleTemplatesAboveSiteTemplates
        {
            get { return ConfigHelper.GetBoolProperty("AddSystemStyleTemplatesAboveSiteTemplates", true); }
        }

        public static bool AddSystemStyleTemplatesBelowSiteTemplates
        {
            get { return ConfigHelper.GetBoolProperty("AddSystemStyleTemplatesBelowSiteTemplates", false); }
        }

        public static int ContentRatingListPageSize
        {
            get { return ConfigHelper.GetIntProperty("ContentRatingListPageSize", 12); }
        }


        public static int MemberListPageSize
        {
            get { return ConfigHelper.GetIntProperty("MemberListPageSize", 20); }
        }

        public static int NewsletterArchivePageSize
        {
            get { return ConfigHelper.GetIntProperty("NewsletterArchivePageSize", 15); }
        }

        /// <summary>
        /// Since we send a verification email for anonymous subscribers we have to consider the possibility that a bot will
        /// submit the same email addresses over and over frequently. We don't want to be spamming people with the verification email
        /// so if we have an existing unverified subscription and it gets submitted again we will only re-send the verification
        /// if this many days have passed since it was last submitted, default is 5 days.
        /// This way in case the user lost the original verification or his email was unavailable for some reason
        /// he can get a new opportunity to confirm by submitting again.
        /// </summary>
        public static int NewsletterReVerifcationAfterDays
        {
            get { return ConfigHelper.GetIntProperty("NewsletterReVerifcationAfterDays", 5); }
        }


        public static int MinutesBetweenAnonymousRatings
        {
            get { return ConfigHelper.GetIntProperty("MinutesBetweenAnonymousRatings", 5); }
        }

        public static int NumberOfWebPartsToShowInMiniCatalog
        {
            get { return ConfigHelper.GetIntProperty("NumberOfWebPartsToShowInMiniCatalog", 15); }
        }

        public static int WebPageInfoCacheMinutes
        {
            get { return ConfigHelper.GetIntProperty("WebPageInfoCacheMinutes", 20); }
        }

        public static Guid InternalFeedSecurityBypassKey
        {
            get
            {
                if (ConfigurationManager.AppSettings["InternalFeedSecurityBypassKey"] != null)
                {
                    string sGuid = ConfigurationManager.AppSettings["InternalFeedSecurityBypassKey"];
                    if (sGuid.Length == 36)
                    {
                        Guid g = Guid.Empty;
                        try
                        {
                            g = new Guid(sGuid);
                            return g;
                        }
                        catch (FormatException) { }
                    }
                }

                return Guid.Empty;
            }
        }

        public static string PasswordRecoveryEmailTemplateFileNamePattern
        {
            get
            {
                if (ConfigurationManager.AppSettings["PasswordRecoveryEmailTemplateFileNamePattern"] != null)
                {
                    return ConfigurationManager.AppSettings["PasswordRecoveryEmailTemplateFileNamePattern"];
                }
                // default value
                return "PasswordEmailMessage.config";
            }
        }

        /// <summary>
        /// possible values, MD5 (default), SHA256, SHA512
        /// for future use
        /// currently we can only use MD5 because the password field in the db is only nvarchar(128)
        /// SHA256 requires 256 bits and SHA512 requires 512 bits
        /// we will need to change to ntext (to support SQL 2000)
        /// </summary>
        public static string HashedPasswordCryptoType
        {
            get
            {
                if (ConfigurationManager.AppSettings["HashedPasswordCryptoType"] != null)
                {
                    return ConfigurationManager.AppSettings["HashedPasswordCryptoType"];
                }
                // for backward compatibility this is the default
                return "MD5";
            }
        }

        public static string HashedPasswordRecoveryEmailTemplateFileNamePattern
        {
            get
            {
                if (ConfigurationManager.AppSettings["HashedPasswordRecoveryEmailTemplateFileNamePattern"] != null)
                {
                    return ConfigurationManager.AppSettings["HashedPasswordRecoveryEmailTemplateFileNamePattern"];
                }
                // default value
                return "HashedPasswordEmailMessage.config";
            }
        }

        public static string DefaultPageRoles
        {
            get
            {
                if (ConfigurationManager.AppSettings["DefaultPageRoles"] != null)
                {
                    return ConfigurationManager.AppSettings["DefaultPageRoles"];
                }
       
                return string.Empty;
            }
        }

        public static string DefaultContentTemplateAllowedRoles
        {
            get
            {
                if (ConfigurationManager.AppSettings["DefaultContentTemplateAllowedRoles"] != null)
                {
                    return ConfigurationManager.AppSettings["DefaultContentTemplateAllowedRoles"];
                }

                return "All Users;";
            }
        }

        public static string RecaptchaTheme
        {
            get
            {
                if (ConfigurationManager.AppSettings["RecaptchaTheme"] != null)
                {
                    return ConfigurationManager.AppSettings["RecaptchaTheme"];
                }

                return "white";
            }
        }

        public static bool ForceLowerCaseForUploadedFiles
        {
            get { return ConfigHelper.GetBoolProperty("ForceLowerCaseForUploadedFiles", true); }
        }

        public static bool ForceLowerCaseForFolderCreation
        {
            get { return ConfigHelper.GetBoolProperty("ForceLowerCaseForFolderCreation", true); }
        }

        public static bool ForceAdminsToUseMediaFolder
        {
            get { return ConfigHelper.GetBoolProperty("ForceAdminsToUseMediaFolder", false); }
        }

        public static bool AllowAdminsToUseDataFolder
        {
            get { return ConfigHelper.GetBoolProperty("AllowAdminsToUseDataFolder", false); }
        }

        public static bool UseClosestAsciiCharsForUrls
        {
            get { return ConfigHelper.GetBoolProperty("UseClosestAsciiCharsForUrls", true); }
        }

        public static bool AlwaysUrlEncode
        {
            get { return ConfigHelper.GetBoolProperty("AlwaysUrlEncode", true); }
        }

        public static bool RetryUnencodedOnUrlNotFound
        {
            get { return ConfigHelper.GetBoolProperty("RetryUnencodedOnUrlNotFound", false); }
        }

        public static bool ForceFriendlyUrlsToLowerCase
        {
            get { return ConfigHelper.GetBoolProperty("ForceFriendlyUrlsToLowerCase", true); }
        }

        public static bool ImageGalleryUseMediaFolder
        {
            get { return ConfigHelper.GetBoolProperty("ImageGalleryUseMediaFolder", true); }
        }

        public static long UserFolderDiskQuotaInMegaBytes
        {
            get { return ConfigHelper.GetLongProperty("UserFolderDiskQuotaInMegaBytes", 300); }
        }

        public static long MediaFolderDiskQuotaInMegaBytes
        {
            get { return ConfigHelper.GetLongProperty("MediaFolderDiskQuotaInMegaBytes", 6000); }
        }

        public static long AdminDiskQuotaInMegaBytes
        {
            get { return ConfigHelper.GetLongProperty("AdminDiskQuotaInMegaBytes", 12000); }
        }

        public static long UserFolderMaxSizePerFileInMegaBytes
        {
            get { return ConfigHelper.GetLongProperty("UserFolderMaxSizePerFileInMegaBytes", 10); }
        }

        public static long MediaFolderMaxSizePerFileInMegaBytes
        {
            get { return ConfigHelper.GetLongProperty("MediaFolderMaxSizePerFileInMegaBytes", 30); }
        }

        public static long AdminMaxSizePerFileInMegaBytes
        {
            get { return ConfigHelper.GetLongProperty("AdminMaxSizePerFileInMegaBytes", 2000); }
        }

        public static int UserFolderMaxNumberOfFiles
        {
            get { return ConfigHelper.GetIntProperty("UserFolderMaxNumberOfFiles", 1000); }
        }

        public static int MediaFolderMaxNumberOfFiles
        {
            get { return ConfigHelper.GetIntProperty("MediaFolderMaxNumberOfFiles", 10000); }
        }

        public static int AdminMaxNumberOfFiles
        {
            get { return ConfigHelper.GetIntProperty("AdminMaxNumberOfFiles", 100000); }
        }

        public static int UserFolderMaxNumberOfFolders
        {
            get { return ConfigHelper.GetIntProperty("UserFolderMaxNumberOfFolders", 50); }
        }

        public static int MediaFolderMaxNumberOfFolders
        {
            get { return ConfigHelper.GetIntProperty("MediaFolderMaxNumberOfFolders", 500); }
        }

        public static int AdminMaxNumberOfFolders
        {
            get { return ConfigHelper.GetIntProperty("AdminMaxNumberOfFolders", 1000); }
        }
        

        public static string AllowedMediaFileExtensions
        {
            get
            {
                if (ConfigurationManager.AppSettings["AllowedMediaFileExtensions"] != null)
                {
                    return ConfigurationManager.AppSettings["AllowedMediaFileExtensions"];
                }
                // default value
                return ".flv|.swf|.wmv|.mp3|.mp4|.asf|.asx|.avi|.mov|.mpeg|.mpg";
            }
        }

        public static string AllowedUploadFileExtensions
        {
            get
            {
                if (ConfigurationManager.AppSettings["AllowedUploadFileExtensions"] != null)
                {
                    return ConfigurationManager.AppSettings["AllowedUploadFileExtensions"];
                }
                // default value
                return ".gif|.jpg|.jpeg|.png|.flv|.swf|.wmv|.mp3|.mp4|.tif|.asf|.asx|.avi|.mov|.mpeg|.mpg|.zip|.pdf|.doc|.docx|.xls|.xlsx|.ppt|.pptx|.csv|.txt";
            }
        }

        

        public static string AllowedLessPriveledgedUserUploadFileExtensions
        {
            get
            {
                if (ConfigurationManager.AppSettings["AllowedLessPriveledgedUserUploadFileExtensions"] != null)
                {
                    return ConfigurationManager.AppSettings["AllowedLessPriveledgedUserUploadFileExtensions"];
                }
                // default value
                return ".gif|.jpg|.jpeg|.png|.zip";
            }
        }

        public static string FileSystemProvider
        {
            get
            {
                if (ConfigurationManager.AppSettings["FileSystemProvider"] != null)
                {
                    return ConfigurationManager.AppSettings["FileSystemProvider"];
                }
                // default value
                return "DiskFileSystemProvider";
            }
        }


        /// <summary>
        /// if a user is in a role that allows both uploading and deleting then they will have access to the main file manager
        /// in some cases you may want to allow users who can only upload to user specific folders to delete files from the editor file browser
        /// without giving them access to the general File Manager, to do that you could set this to true
        /// </summary>
        public static bool AllowDeletingFilesFromUserFolderWithoutDeleteRole
        {
            get { return ConfigHelper.GetBoolProperty("AllowDeletingFilesFromUserFolderWithoutDeleteRole", false); }
        }
        

        public static bool ResizeEditorUploadedImages
        {
            get { return ConfigHelper.GetBoolProperty("ResizeEditorUploadedImages", true); }
        }

        public static int ResizeImageDefaultMaxWidth
        {
            get { return ConfigHelper.GetIntProperty("ResizeImageDefaultMaxWidth", 550); }
        }

        public static int ResizeImageDefaultMaxHeight
        {
            get { return ConfigHelper.GetIntProperty("ResizeImageDefaultMaxWidth", 550); }
        }

        public static bool AvatarsCanOnlyBeUploadedByAdmin
        {
            get { return ConfigHelper.GetBoolProperty("AvatarsCanOnlyBeUploadedByAdmin", false); }
        }

        public static bool ForceSquareAvatars
        {
            get { return ConfigHelper.GetBoolProperty("ForceSquareAvatars", true); }
        }

        public static int AvatarMaxOriginalWidth
        {
            get { return ConfigHelper.GetIntProperty("AvatarMaxOriginalWidth", 800); }
        }

        public static int AvatarMaxOriginalHeight
        {
            get { return ConfigHelper.GetIntProperty("AvatarMaxOriginalHeight", 800); }
        }

        public static int AvatarMaxWidth
        {
            get { return ConfigHelper.GetIntProperty("AvatarMaxWidth", 90); }
        }

        public static int AvatarMaxHeight
        {
            get { return ConfigHelper.GetIntProperty("AvatarMaxHeight", 90); }
        }
        
        

        //public static string RolesThatCanViewMemberList
        //{
        //    get 
        //    {
        //        if (ConfigurationManager.AppSettings["RolesThatCanViewMemberList"] != null)
        //        {
        //            return ConfigurationManager.AppSettings["RolesThatCanViewMemberList"];
        //        }
        //        // default value
        //        return "Authenticated Users;"; 
        //    }
        //}

        //public static string RolesThatCanLookupUsers
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["RolesThatCanLookupUsers"] != null)
        //        {
        //            return ConfigurationManager.AppSettings["RolesThatCanLookupUsers"];
        //        }
        //        // default value
        //        return "Admins;Role Admins;Content Administrators;Store Managers;";
        //    }
        //}


        //public static string RolesThatCanViewMyPage
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["RolesThatCanViewMyPage"] != null)
        //        {
        //            return ConfigurationManager.AppSettings["RolesThatCanViewMyPage"];
        //        }
        //        // default value
        //        return "All Users;";
        //    }
        //}


        //public static string RolesThatCanUploadAndBrowse
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["RolesThatCanUploadAndBrowse"] != null)
        //        {
        //            return ConfigurationManager.AppSettings["RolesThatCanUploadAndBrowse"];
        //        }
        //        // default value
        //        return "Admins;Content Administrators;Content Publishers;Content Authors;Store Managers";
        //    }
        //}

        //public static string RolesThatCanEditContentTemplates
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["RolesThatCanEditContentTemplates"] != null)
        //        {
        //            return ConfigurationManager.AppSettings["RolesThatCanEditContentTemplates"];
        //        }
        //        // default value
        //        return string.Empty;
        //    }
        //}

        //public static string RolesThatCanManageUsers
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["RolesThatCanManageUsers"] != null)
        //        {
        //            return ConfigurationManager.AppSettings["RolesThatCanManageUsers"];
        //        }
        //        // default value
        //        return string.Empty;
        //    }
        //}

        public static string PageToRedirectToAfterSignIn
        {
            get
            {
                if (ConfigurationManager.AppSettings["PageToRedirectToAfterSignIn"] != null)
                {
                    return ConfigurationManager.AppSettings["PageToRedirectToAfterSignIn"];
                }
                // default value
                return string.Empty;
            }
        }

        public static string EditorTemplatesPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["EditorTemplatesPath"] != null)
                {
                    return ConfigurationManager.AppSettings["EditorTemplatesPath"];
                }
                // default value
                return "~/ClientScript/Cynthia-editor-templates.xml";
            }
        }

        public static string EditAreaBasePath
        {
            get
            {
                if (ConfigurationManager.AppSettings["EditAreaBasePath"] != null)
                {
                    return ConfigurationManager.AppSettings["EditAreaBasePath"];
                }
                // default value
                return "~/ClientScript/edit_area0811/";
            }
        }

        

        

        

        
        public static string RequestApprovalImage
        {
            get
            {
                if (ConfigurationManager.AppSettings["RequestApprovalImage"] != null)
                {
                    return ConfigurationManager.AppSettings["RequestApprovalImage"];
                }
                // default value
                return "~/Data/SiteImages/RequestApproval.gif";
            }
        }

        public static string ApproveContentImage
        {
            get
            {
                if (ConfigurationManager.AppSettings["ApproveContentImage"] != null)
                {
                    return ConfigurationManager.AppSettings["ApproveContentImage"];
                }
                // default value
                return "~/Data/SiteImages/ApproveChanges.gif";
            }
        }

        public static string RejectContentImage
        {
            get
            {
                if (ConfigurationManager.AppSettings["RejectContentImage"] != null)
                {
                    return ConfigurationManager.AppSettings["RejectContentImage"];
                }
                // default value
                return "~/Data/SiteImages/RejectChanges.gif";
            }
        }

        public static string CancelContentChangesImage
        {
            get
            {
                if (ConfigurationManager.AppSettings["CancelContentChangesImage"] != null)
                {
                    return ConfigurationManager.AppSettings["CancelContentChangesImage"];
                }
                // default value
                return "~/Data/SiteImages/CancelChanges.gif";
            }
        }

        public static string RobotsConfigFile
        {
            get
            {
                if (ConfigurationManager.AppSettings["RobotsConfigFile"] != null)
                {
                    return ConfigurationManager.AppSettings["RobotsConfigFile"];
                }
                // default value
                return "~/robots.config";
            }
        }

        public static string RobotsSslConfigFile
        {
            get
            {
                if (ConfigurationManager.AppSettings["RobotsSslConfigFile"] != null)
                {
                    return ConfigurationManager.AppSettings["RobotsSslConfigFile"];
                }
                // default value
                return "~/robots.ssl.config";
            }
        }
       

        public static bool ShowRebuildSearchIndexButtonToAdmins
        {
            get { return ConfigHelper.GetBoolProperty("ShowRebuildSearchIndexButtonToAdmins", false); }
        }

        public static bool ShowCustomProfilePropertiesAboveManadotoryRegistrationFields
        {
            get { return ConfigHelper.GetBoolProperty("ShowCustomProfilePropertiesAboveManadotoryRegistrationFields", false); }
        }


        public static bool AllowUserThreadBrowsing
        {
            get { return ConfigHelper.GetBoolProperty("AllowUserThreadBrowsing", true); }
        }

        public static bool ShowPageEncoding
        {
            get { return ConfigHelper.GetBoolProperty("ShowPageEncoding", false); }
        }

        public static bool ShowUseUrlSettingInPageSettings
        {
            get { return ConfigHelper.GetBoolProperty("ShowUseUrlSettingInPageSettings", false); }
        }


        public static bool ShowAdditionalMeta
        {
            get { return ConfigHelper.GetBoolProperty("ShowAdditionalMeta", false); }
        }

        public static string SetupHeaderConfigPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["SetupHeaderConfigPath"] != null)
                {
                    return ConfigurationManager.AppSettings["SetupHeaderConfigPath"];
                }
                // default value
                return "~/Setup/SetupHeader.config";
            }
        }

        
        public static string DefaultCountry
        {
            get
            {
                if (ConfigurationManager.AppSettings["DefaultCountry"] != null)
                {
                    return ConfigurationManager.AppSettings["DefaultCountry"];
                }

                return "US";
            }
        }

       

        /// <summary>
        /// valid options: TitleOnly, SitePlusTitle, TitlePlusSite
        /// generally you should not call this directly but use the corresponding method in SiteUtils
        /// </summary>
        public static string PageTitleFormatName
        {
            get
            {
                if (ConfigurationManager.AppSettings["PageTitleFormatName"] != null)
                {
                    return ConfigurationManager.AppSettings["PageTitleFormatName"];
                }

                return "SitePlusTitle";
            }
        }

        /// <summary>
        /// used to separate the site and page title when PageTitleFormatName is SitePlusTitle or TitlePlusSite
        /// generally you should not call this directly but use the corresponding method in SiteUtils
        /// </summary>
        public static string PageTitleSeparatorString
        {
            get
            {
                if (ConfigurationManager.AppSettings["PageTitleSeparatorString"] != null)
                {
                    return ConfigurationManager.AppSettings["PageTitleSeparatorString"];
                }

                return " - ";
            }
        }

        public static string GoogleMapsAPIKey
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleMapsAPIKey"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleMapsAPIKey"];
                }
                
                return string.Empty;
            }
        }



        public static string GoogleAnalyticsMemberTypeAnonymous
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAnonymous"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAnonymous"];
                }

                return "anonymous";
            }
        }

        public static string GoogleAnalyticsMemberLabel
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberLabel"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleAnalyticsMemberLabel"];
                }

                return "member-type";
            }
        }

        public static string GoogleAnalyticsMemberTypeAuthenticated
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAuthenticated"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAuthenticated"];
                }

                return "member";
            }
        }

        public static string GoogleAnalyticsMemberTypeCustomer
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeCustomer"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeCustomer"];
                }

                return "customer";
            }
        }

        public static string GoogleAnalyticsMemberTypeAdmin
        {
            get
            {
                if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAdmin"] != null)
                {
                    return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAdmin"];
                }

                return "admin";
            }
        }

        
        

        public static string SubSonicProvider
        {
            get
            {
                if (ConfigurationManager.AppSettings["SubSonicProvider"] != null)
                {
                    return ConfigurationManager.AppSettings["SubSonicProvider"];
                }

                return string.Empty;
            }
        }


        //
        //// This setting is used only in CacheHelper.cs in another project (not referencing this one)!
        //public static int SiteSettingsCacheDurationInSeconds
        //{
        //    get { return GetIntProperty("SiteSettingsCacheDurationInSeconds", 360); }
        //}

        //// This setting is used only in CacheHelper.cs in another project (not referencing this one)!
        //public static int MenuCacheDurationInSeconds
        //{
        //    get { return GetIntProperty("MenuCacheDurationInSeconds", 360); }
        //}

        //// This setting is used only in CacheHelper.cs in another project (not referencing this one)!
        //public static int DefaultModuleCacheDurationInSeconds
        //{
        //    get { return GetIntProperty("DefaultModuleCacheDurationInSeconds", 360); }
        //}

        //public static Unit ExtJsDefaultWindowHeight
        //{
        //    get { return ConfigHelper.GetUnit("ExtJsDefaultWindowHeight", Unit.Parse("700")); }
        //}

        //public static Unit ExtJsDefaultWindowWidth
        //{
        //    get { return ConfigHelper.GetUnit("ExtJsDefaultWindowWidth", Unit.Parse("100%")); }
        //}

        //public static int ExtJsDefaultWindowTop
        //{
        //    get { return ConfigHelper.GetIntProperty("ExtJsDefaultWindowTop", 200); }
        //}

        //public static int ExtJsDefaultWindowLeft
        //{
        //    get { return ConfigHelper.GetIntProperty("ExtJsDefaultWindowLeft", 0); }
        //}

    }
}
