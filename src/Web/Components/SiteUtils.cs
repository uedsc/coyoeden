
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Net;
using Cynthia.Web.UI;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;
using Cynthia.Web.Editor;
using Resources;
using System.Linq;
namespace Cynthia.Web
{
    /// <summary>
    ///
    /// </summary>
    public static class SiteUtils
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteUtils));


        public static bool RunningOnMono()
        {
            return SystemX.Utils.IsMono;
        }

        /// <summary>
        /// compares 2 urls, if running on Mono does a case sensitive match
        /// else it does caseinsenitive match
        /// returns false if either string isnullorempty
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        public static bool UrlsMatch(string url1, string url2)
        {
            if (string.IsNullOrEmpty(url1)) { return false; }
            if (string.IsNullOrEmpty(url2)) { return false; }

            if (RunningOnMono())
            {
                return (url1 == url2);
            }

            return string.Equals(url1, url2, StringComparison.InvariantCultureIgnoreCase);

        }

        public static String SuggestFriendlyUrl(
            String pageName,
            SiteSettings siteSettings)
        {
            String friendlyUrl = CleanStringForUrl(pageName);
            if (WebConfigSettings.AlwaysUrlEncode)
            {
                friendlyUrl = HttpUtility.UrlEncode(friendlyUrl);
            }
            

            switch (siteSettings.DefaultFriendlyUrlPattern)
            {
                case SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX:
                    friendlyUrl += ".aspx";
                    break;

            }

            int i = 1;
            while (FriendlyUrl.Exists(siteSettings.SiteId, friendlyUrl))
            {
                friendlyUrl = i.ToString() + friendlyUrl;
            }



            if (WebConfigSettings.ForceFriendlyUrlsToLowerCase) { return friendlyUrl.ToLower(); }

            return friendlyUrl;
        }

        private static String CleanStringForUrl(String input)
        {
            String outputString = RemovePunctuation(input).Replace(" - ", "-").Replace(" ", "-").Replace("/", String.Empty).Replace("\"", String.Empty).Replace("'", String.Empty).Replace("#", String.Empty).Replace("~", String.Empty).Replace("`", String.Empty).Replace("@", String.Empty).Replace("$", String.Empty).Replace("*", String.Empty).Replace("^", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty).Replace("+", String.Empty).Replace("=", String.Empty).Replace("%", String.Empty).Replace(">", String.Empty).Replace("<", String.Empty);

            if (WebConfigSettings.UseClosestAsciiCharsForUrls) { return outputString.ToAsciiIfPossible(); }

            return outputString;

        }

        private static String RemovePunctuation(String input)
        {
            String outputString = String.Empty;
            if (input != null)
            {
                outputString = input.Replace(".", String.Empty).Replace(",", String.Empty).Replace(":", String.Empty).Replace("?", String.Empty).Replace("!", String.Empty).Replace(";", String.Empty).Replace("&", String.Empty).Replace("{", String.Empty).Replace("}", String.Empty).Replace("[", String.Empty).Replace("]", String.Empty);
            }
            return outputString;
        }

        public static void SetupEditor(EditorControl editor)
        {
            SetupEditor(editor, WebConfigSettings.UseSkinCssInEditor);
        }
        
        /// <summary>
        /// You should pass your editor to this method during pre-init or init
        /// </summary>
        /// <param name="editor"></param>
        public static void SetupEditor(EditorControl editor, bool useSkinCss)
        {
            if (HttpContext.Current == null) return;
            if (HttpContext.Current.Request == null) return;
            if (editor == null) return;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return;

            string providerName = siteSettings.EditorProviderName;


            string loweredBrowser = string.Empty;

            if(HttpContext.Current.Request.UserAgent != null)
            {
                loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();
            }
           
            
            if (
                (loweredBrowser.Contains("safari"))
                && (WebConfigSettings.ForceTinyMCEInSafari)
                )
            {
                providerName = "TinyMCEProvider";
            }

            if (
                (loweredBrowser.Contains("opera"))
                && (WebConfigSettings.ForceTinyMCEInOpera)
                )
            {
                providerName = "TinyMCEProvider";
            }

            if (
                (loweredBrowser.Contains("iphone"))
                && (WebConfigSettings.ForcePlainTextInIphone)
                )
            {
                providerName = "TextAreaProvider";
            }

            if (
                (loweredBrowser.Contains("android"))
                && (WebConfigSettings.ForcePlainTextInAndroid)
                )
            {
                providerName = "TextAreaProvider";
            }

            string siteRoot = null;
            if (siteSettings.SiteFolderName.Length > 0)
            {
                siteRoot = siteSettings.SiteRoot;
            }
            if (siteRoot == null) siteRoot = WebUtils.GetSiteRoot();

            editor.ProviderName = providerName;
            editor.WebEditor.SiteRoot = siteRoot;
            //editor.WebEditor.SkinName = siteSettings.EditorSkin.ToString();
            if (useSkinCss)
            {
                editor.WebEditor.EditorCSSUrl = GetEditorStyleSheetUrl();
            }

            CultureInfo defaultCulture = ResourceHelper.GetDefaultCulture();
            if (defaultCulture.TextInfo.IsRightToLeft)
            {
                editor.WebEditor.TextDirection = Direction.RightToLeft;
            }



        }

        /// <summary>
        /// You should pass your editor to this method during pre-init or init
        /// </summary>
        /// <param name="editor"></param>
        public static void SetupNewsletterEditor(EditorControl editor)
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (editor == null) return;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            string providerName = siteSettings.NewsletterEditor;

            string loweredBrowser = string.Empty;

            if (HttpContext.Current.Request.UserAgent != null)
            {
                loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();
            }

            if (
                (loweredBrowser.Contains("iphone"))
                && (WebConfigSettings.ForcePlainTextInIphone)
                )
            {
                providerName = "TextAreaProvider";
            }

            if (
                (loweredBrowser.Contains("android"))
                && (WebConfigSettings.ForcePlainTextInAndroid)
                )
            {
                providerName = "TextAreaProvider";
            }

            string siteRoot = null;
            if (siteSettings.SiteFolderName.Length > 0)
            {
                siteRoot = siteSettings.SiteRoot;
            }
            if (siteRoot == null) siteRoot = WebUtils.GetSiteRoot();

            editor.ProviderName = providerName;
            editor.WebEditor.SiteRoot = siteRoot;
            
            CultureInfo defaultCulture = ResourceHelper.GetDefaultCulture();
            if (defaultCulture.TextInfo.IsRightToLeft)
            {
                editor.WebEditor.TextDirection = Direction.RightToLeft;
            }



        }

        public static string GetIP4Address()
        {
            string ip4Address = string.Empty;
            if (HttpContext.Current == null) { return ip4Address; }
            if (HttpContext.Current.Request == null) { return ip4Address; }
            if (HttpContext.Current.Request.UserHostAddress == null) { return ip4Address; }

            try
            {
                IPAddress ip = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);
                if (ip.AddressFamily.ToString() == "InterNetwork") { return ip.ToString(); }
            }
            catch (FormatException)
            { }
            catch (ArgumentNullException) { }

            try
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        ip4Address = IPA.ToString();
                        break;
                    }
                }
            }
            catch (ArgumentException)
            { }
            catch (System.Net.Sockets.SocketException) { }

            if (ip4Address != string.Empty)
            {
                return ip4Address;
            }

            //this part makes no sense it would get the local server ip address
            try
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        ip4Address = IPA.ToString();
                        break;
                    }
                }
            }
            catch (ArgumentException)
            { }
            catch (System.Net.Sockets.SocketException) { }

            return ip4Address;
        }

        public static string BuildStylesListForTinyMce()
        {
            StringBuilder styles = new StringBuilder();

            string comma = string.Empty;

            if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates)
            {
                styles.Append("FloatPanel=floatpanel,Image on Right=floatrightimage,Image on Left=floatleftimage");
                comma = ",";
            }

            
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null)
            {
                using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
                {
                    while (reader.Read())
                    {
                        styles.Append(String.Format("{0}{1}={2}", comma, reader["Name"], reader["CssClass"]));
                        comma = ",";
                    }
                }

            }

            if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates)
            {
                styles.Append(comma + "FloatPanel=floatpanel,Image on Right=floatrightimage,Image on Left=floatleftimage");
            }

            return styles.ToString();
        }

        public static bool IsImageFileExtension(string fileExtension)
        {
            List<string> allowedExtensions = new List<string>();
            allowedExtensions.Add(".jpg");
            allowedExtensions.Add(".jpeg");
            allowedExtensions.Add(".png");
            allowedExtensions.Add(".gif");

            foreach (string ext in allowedExtensions)
            {
                if (string.Equals(fileExtension, ext, StringComparison.InvariantCultureIgnoreCase)) { return true; }
            }

            return false;

        }

        public static string ImageFileExtensions()
        {
            return ".gif|.jpg|.jpeg|.png";
        }

        public static bool IsAllowedMediaFile(this FileInfo fileInfo)
        {
            List<string> allowedExtensions = StringHelper.SplitOnPipes(WebConfigSettings.AllowedMediaFileExtensions);
            foreach (string ext in allowedExtensions)
            {
                if (string.Equals(fileInfo.Extension, ext, StringComparison.InvariantCultureIgnoreCase)) { return true; }
            }

            return false;

        }

        public static bool IsAllowedUploadBrowseFile(this FileInfo fileInfo, string allowedExtensions)
        {
            List<string> allowed = StringHelper.SplitOnPipes(allowedExtensions);
            foreach (string ext in allowed)
            {
                if (string.Equals(fileInfo.Extension, ext, StringComparison.InvariantCultureIgnoreCase)) { return true; }
            }

            return false;
        }

        public static bool IsAllowedUploadBrowseFile(string fileExtension, string allowedExtensions)
        {
            List<string> allowed = StringHelper.SplitOnPipes(allowedExtensions);
            foreach (string ext in allowed)
            {
                if (string.Equals(fileExtension, ext, StringComparison.InvariantCultureIgnoreCase)) { return true; }
            }

            return false;
        }
        

        public static string GetEditorStyleSheetUrl()
        {
            if (WebConfigSettings.EditorCssUrlOverride.Length > 0) { return WebConfigSettings.EditorCssUrlOverride; }

            string editorCss = "csshandler.ashx";
            if (WebConfigSettings.UsingOlderSkins)
            {
                editorCss = "style.css";
            }

            string skinName = "styleshout-techmania";

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            string basePath = String.Format("{0}/Data/Sites/{1}/skins/", WebUtils.GetSiteRoot(), siteSettings.SiteId.ToString(CultureInfo.InvariantCulture));

            //SiteSettings siteSettings = (SiteSettings) HttpContext.Current.Items["SiteSettings"];

            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (siteSettings != null)
            {
                // very old skins were .ascx
                skinName = siteSettings.Skin.Replace(".ascx", string.Empty);

                if (siteSettings.AllowUserSkins)
                {
                    string skinCookieName = "CUserSkin" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

                    if (CookieHelper.CookieExists(skinCookieName))
                    {
                        string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
                        if (cookieValue.Length > 0)
                        {
                            skinName = cookieValue.Replace(".ascx", string.Empty);
                        }
                    }
                }

                if ((currentPage != null) && (siteSettings.AllowPageSkins))
                {
                    if (currentPage.Skin.Length > 0)
                    {
                        skinName = currentPage.Skin.Replace(".ascx", string.Empty);

                    }
                }



                if (HttpContext.Current.Request.Params.Get("skin") != null)
                {
                    skinName = HttpContext.Current.Request.Params.Get("skin");

                }

                if (editorCss == "csshandler.ashx")
                {
                    return String.Format("{0}{1}/{2}?skin={1}", basePath, skinName, editorCss);
                }

                return String.Format("{0}{1}/{2}", basePath, skinName, editorCss);

            }

            return "/Data/Sites/1/skins/styleshout-tecmania/style.css";
        }


        public static void SetButtonAccessKey(Button button, string accessKey)
        {
            if (!WebConfigSettings.UseShortcutKeys) return;

            button.AccessKey = accessKey;
            button.Text += GetButtonAccessKeyPostfix(accessKey);
        }


        public static string GetButtonAccessKeyPostfix(string accessKey)
        {
            if (HttpContext.Current == null) return String.Empty;

            string browser = HttpContext.Current.Request.Browser.Browser;
            string browserAccessKey = browser.ToLower().Contains("opera")
                                          ? AccessKeys.BrowserOperaAccessKey
                                          : AccessKeys.BrowserAccessKey;

            return string.Format(CultureInfo.InvariantCulture," [{0}+{1}]", browserAccessKey, accessKey);
        }

        /// <summary>
        /// this method is deprecated
        /// </summary>
        [Obsolete("This method is obsolete. You should use if(!Request.IsAuthenticated) SiteUtils.RedirectToLogin(PageorControl); return;")]
        public static void AllowOnlyAuthenticated()
        {
            if (HttpContext.Current == null) return;
            if (!HttpContext.Current.Request.IsAuthenticated) RedirectToLoginPage();
        }

        [Obsolete("This method is obsolete. You should use if(!Request.IsAuthenticated) SiteUtils.RedirectToLogin(PageorControl); return;")]
        public static void AllowOnlyAuthenticated(Control pageOrControl)
        {
            if (HttpContext.Current == null) return;
            if (!HttpContext.Current.Request.IsAuthenticated) RedirectToLoginPage(pageOrControl);
        }

        [Obsolete("This method is obsolete. You should use if(!WebUser.IsAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
        public static void AllowOnlyAdmin()
        {
            if (HttpContext.Current == null) return;
            AllowOnlyAuthenticated();
            if (!HttpContext.Current.Request.IsAuthenticated) return;
            if (!WebUser.IsAdmin) RedirectToEditAccessDeniedPage();
        }

        [Obsolete("This method is obsolete. You should use if(!WebUser.IsAdminOrRoleAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
        public static void AllowOnlyAdminAndRoleAdmin()
        {
            if (HttpContext.Current == null) return;
            AllowOnlyAuthenticated();
            if (!HttpContext.Current.Request.IsAuthenticated) return;
            if ((!WebUser.IsAdmin) && (!WebUser.IsRoleAdmin))
            {
                RedirectToEditAccessDeniedPage();
            }
        }

        [Obsolete("This method is obsolete. You should use if(!WebUser.IsAdminOrContentAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
        public static void AllowOnlyAdminAndContentAdmin()
        {
            if (HttpContext.Current == null) return;
            AllowOnlyAuthenticated();
            if (!HttpContext.Current.Request.IsAuthenticated) return;
            if (!WebUser.IsAdminOrContentAdmin) RedirectToEditAccessDeniedPage();
        }

        [Obsolete("This method is obsolete. You should use if(!WebUser.IsAdminOrContentAdminOrRoleAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
        public static void AllowOnlyAdminAndContentAdminAndRoleAdmin()
        {
            if (HttpContext.Current == null) return;
            AllowOnlyAuthenticated();
            if (!HttpContext.Current.Request.IsAuthenticated) return;
            if (!WebUser.IsAdminOrContentAdminOrRoleAdmin) RedirectToEditAccessDeniedPage();
        }

        [Obsolete("This method is obsolete. You should use RedirectToLoginPage(pageOrControl); return;")]
        public static void RedirectToLoginPage()
        {
            HttpContext.Current.Response.Redirect
                (string.Format(CultureInfo.InvariantCulture,"{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
                               GetNavigationSiteRoot(),
                               HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)),
                 true);
        }

        public static void RedirectToLoginPage(Control pageOrControl)
        {
            string redirectUrl 
                = string.Format(CultureInfo.InvariantCulture,"{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
                               GetNavigationSiteRoot(),
                               HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
            
            WebUtils.SetupRedirect(pageOrControl, redirectUrl);
        }

        public static void RedirectToLoginPage(Control pageOrControl, string returnUrl)
        {
            string redirectUrl
                = string.Format(CultureInfo.InvariantCulture, "{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
                               GetNavigationSiteRoot(),
                               HttpUtility.UrlEncode(returnUrl));

            WebUtils.SetupRedirect(pageOrControl, redirectUrl);
        }

        public static void RedirectToLoginPage(Control pageOrControl, bool useHardRedirect)
        {
            if (!useHardRedirect)
            {
                RedirectToLoginPage(pageOrControl);
                return;
            }

            string redirectUrl
                = string.Format(CultureInfo.InvariantCulture, "{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
                               GetNavigationSiteRoot(),
                               HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));

            pageOrControl.Page.Response.Redirect(redirectUrl);

            //WebUtils.SetupRedirect(pageOrControl, redirectUrl);
        }

        public static string GetLoginRelativeUrl()
        {
            if (ConfigurationManager.AppSettings["LoginPageRelativeUrl"] != null)
                return ConfigurationManager.AppSettings["LoginPageRelativeUrl"];

            return "/Secure/Login.aspx";

        }

        public static void RedirectToUrl(string url)
        {
            if (HttpContext.Current == null) { return; }
          
            HttpContext.Current.Response.RedirectLocation = url;
            HttpContext.Current.Response.StatusCode = 302;
            HttpContext.Current.Response.StatusDescription = "Redirecting to " + url;
            HttpContext.Current.Response.Write("Redirecting to " + url);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }

        public static void RedirectToEditAccessDeniedPage()
        {
            HttpContext.Current.Response.Redirect(GetNavigationSiteRoot() + "/EditAccessDenied.aspx", true);
            //RedirectToUrl(GetNavigationSiteRoot() + "/EditAccessDenied.aspx");
        }


        public static void RedirectToAccessDeniedPage()
        {
            //HttpContext.Current.Response.Redirect(GetNavigationSiteRoot() + "/AccessDenied.aspx", false);
            RedirectToUrl(GetNavigationSiteRoot() + "/AccessDenied.aspx");
        }

        public static void RedirectToAccessDeniedPage(Control pageOrControl)
        {
            string redirectUrl
                = GetNavigationSiteRoot() + "/AccessDenied.aspx";

            WebUtils.SetupRedirect(pageOrControl, redirectUrl);
        }

        public static void RedirectToAccessDeniedPage(Control pageOrControl, bool useHardRedirect)
        {
            if (!useHardRedirect)
            {
                RedirectToAccessDeniedPage(pageOrControl);
                return;
            }

            string redirectUrl
                = GetNavigationSiteRoot() + "/AccessDenied.aspx";

            pageOrControl.Page.Response.Redirect(redirectUrl);

            //WebUtils.SetupRedirect(pageOrControl, redirectUrl);
        }


        public static void RedirectToDefault()
        {
            //HttpContext.Current.Response.Redirect(GetNavigationSiteRoot() + "/default.aspx", false);
            RedirectToUrl(GetNavigationSiteRoot() + "/Default.aspx");
        }


        public static void SetFormAction(Page page, string action)
        {
            page.Form.Action = action;
        }

        public static void SetMasterPage(
            Page page, 
            SiteSettings siteSettings,
            bool allowOverride)
        {
            String skinFolder;
            String skinName;
            PageSettings currentPage = CacheHelper.GetCurrentPage();

            if (
                (HttpContext.Current != null)
                &&(page != null)
                &&(siteSettings != null)
                )
            {

                skinFolder = String.Format("~/Data/Sites/{0}/skins/", siteSettings.SiteId.ToString(CultureInfo.InvariantCulture));
                skinName = siteSettings.Skin.Replace(".ascx", "") + "/layout.Master";

                // implement user skins
                if (siteSettings.AllowUserSkins)
                {
                    string skinCookieName = "CUserSkin" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
                    if (CookieHelper.CookieExists(skinCookieName))
                    {
                        string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
                        if (File.Exists(HttpContext.Current.Server.MapPath(skinFolder + cookieValue.Replace(".ascx", "") + "/layout.Master")))
                        {
                            skinName = cookieValue.Replace(".ascx", "") + "/layout.Master";

                        }

                    }
                }

                // implement per page skins
                if (
                    (allowOverride)
                    &&(siteSettings.AllowPageSkins)
                    )
                {
                    if (
                        (currentPage != null)
                        &&(currentPage.Skin.Length > 0)
                        )
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(skinFolder + currentPage.Skin.Replace(".ascx", "") + "/layout.Master")))
                        {
                            skinName = currentPage.Skin.Replace(".ascx", "") + "/layout.Master";
                        }
                    }
                }


                // implement skin preview using querystring param
                if (HttpContext.Current.Request.Params.Get("skin") != null)
                {
                    string previewSkin = SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin"));

                    if (!previewSkin.EndsWith("/layout.ascx"))
                    {
                        previewSkin += "/layout.Master";
                    }


                    if (File.Exists(HttpContext.Current.Server.MapPath(skinFolder + previewSkin)))
                    {
                        skinName = previewSkin;

                    }

                }
            }
            else
            {
                // hard coded only at design time, at runtime we get this from siteSettings
                skinFolder = "~/Data/Sites/1/skins/";
                skinName = "styleshout-techmania/layout.Master";

            }

            page.MasterPageFile = skinFolder + skinName;

        }

        public static string SanitizeSkinParam(string skinName)
        {
            if (string.IsNullOrEmpty(skinName)) { return skinName; }

            // protected against this xss attack
            // ?skin=1%00'"><ScRiPt%20%0a%0d>alert(403326057258)%3B</ScRiPt>
            return skinName.Replace("%", string.Empty).Replace(" ", string.Empty).Replace(">", string.Empty).Replace("<", string.Empty).Replace("'", string.Empty).Replace("\"", string.Empty) ;
            

        }

        public static void SetMasterPage(
            Page page,
            SiteSettings siteSettings,
            string skinName)
        {
            
            if(HttpContext.Current == null) { return; }
            if (page == null) { return; }
            if (siteSettings == null) { return; }
            if(string.IsNullOrEmpty(skinName)) { return;}

            string masterPagePath = String.Format("~/Data/Sites/{0}/skins/{1}/layout.master", siteSettings.SiteId.ToString(CultureInfo.InvariantCulture), SanitizeSkinParam(skinName));

            page.MasterPageFile = masterPagePath;

        }


        public static void SetSkinCookie(SiteUser siteUser)
        {
            if (siteUser == null) return;
            string skinCookieName = "CUserSkin" + siteUser.SiteId.ToString(CultureInfo.InvariantCulture);
            HttpCookie cookie = HttpContext.Current.Request.Cookies[skinCookieName];
            bool setCookie = (cookie == null) || (cookie.Value != siteUser.Skin);
            if (setCookie)
            {
                HttpCookie userSkinCookie = new HttpCookie(skinCookieName, siteUser.Skin) { Expires = DateTime.Now.AddYears(1) };
                HttpContext.Current.Response.Cookies.Add(userSkinCookie);

            }
        }


        public static void SetDisplayNameCookie(string displayName)
        {
            if (String.IsNullOrEmpty(displayName)) return;

            HttpCookie cookie = HttpContext.Current.Request.Cookies["DisplayName"];
            bool setCookie = (cookie == null) || (cookie.Value != displayName);
            if (setCookie)
            {
                HttpCookie userNameCookie = new HttpCookie("DisplayName", HttpUtility.HtmlEncode(displayName)) { Expires = DateTime.Now.AddYears(1) };
                HttpContext.Current.Response.Cookies.Add(userNameCookie);
            }
        }


       
        public static FileInfo[] GetLogoList(SiteSettings siteSettings)
        {
            if (siteSettings == null) return null;

            string logoPath = HttpContext.Current.Server.MapPath
                (String.Format("{0}/Data/Sites/{1}/logos/", WebUtils.GetApplicationRoot(), siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)));

            DirectoryInfo dir = new DirectoryInfo(logoPath);
            return dir.Exists ? dir.GetFiles() : null;
        }

        public static FileInfo[] GetContentTemplateImageList(SiteSettings siteSettings)
        {
            if (siteSettings == null) return null;

            string filePath = HttpContext.Current.Server.MapPath
                (String.Format("{0}/Data/Sites/{1}/htmltemplateimages/", WebUtils.GetApplicationRoot(), siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)));

            DirectoryInfo dir = new DirectoryInfo(filePath);
            return dir.Exists ? dir.GetFiles() : null;
        }

        public static Gravatar.RatingType GetMaxAllowedGravatarRating()
        {
            switch (WebConfigSettings.GravatarMaxAllowedRating)
            {
                case "PG":
                    return Gravatar.RatingType.PG;

                case "R":
                    return Gravatar.RatingType.R;

                case "X":
                    return Gravatar.RatingType.X;


            }

            return Gravatar.RatingType.G;
        }

        


        public static FileInfo[] GetAvatarList(SiteSettings siteSettings)
        {
            if (siteSettings == null) return null;

            string p = String.Format("{0}/Data/Sites/{1}/avatars", WebUtils.GetApplicationRoot(), siteSettings.SiteId.ToString(CultureInfo.InvariantCulture));
            string avatarPath = HttpContext.Current.Server.MapPath(p);

            DirectoryInfo dir = new DirectoryInfo(avatarPath);
            return dir.Exists ? dir.GetFiles("*.gif") : null;
        }


        public static FileInfo[] GetFeatureIconList()
        {
            string p = WebUtils.GetApplicationRoot() + "/Data/SiteImages/FeatureIcons";
            string filePath = HttpContext.Current.Server.MapPath(p);

            //HttpContext.Current.Request.PhysicalApplicationPath;
                

            DirectoryInfo dir = new DirectoryInfo(filePath);
            return dir.Exists ? dir.GetFiles("*.*") : null;
        }


        public static FileInfo[] GetFileIconList()
        {
            string p = WebUtils.GetApplicationRoot() + "/Data/SiteImages/Icons";
            string filePath = HttpContext.Current.Server.MapPath(p);

            DirectoryInfo dir = new DirectoryInfo(filePath);
            return dir.Exists ? dir.GetFiles("*.png") : null;
        }

        /// <summary>
        /// deprecated, better to just call IOHelper.GetMimeType
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetMimeType(string fileExtension)
        {
            return IOHelper.GetMimeType(fileExtension);
        }

        /// <summary>
        /// deprecated, better to just call IOHelper.IsNonAttacmentFileType
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static bool IsNonAttacmentFileType(string fileExtension)
        {
            return IOHelper.IsNonAttacmentFileType(fileExtension);
        }

        public static string GetSiteSystemFolder()
        {
            if (HttpContext.Current == null) { return string.Empty; }
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return string.Empty; }

            return HttpContext.Current.Server.MapPath(String.Format("~/Data/Sites/{0}/systemfiles/", siteSettings.SiteId.ToInvariantString()));


        }

        public static DirectoryInfo[] GetSkinList(SiteSettings siteSettings)
        {
            if (siteSettings == null) return null;

            string skinPath = HttpContext.Current.Server.MapPath
                (String.Format("{0}/Data/Sites/{1}/skins/", WebUtils.GetApplicationRoot(), siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)));

            DirectoryInfo dir = new DirectoryInfo(skinPath);
            return dir.Exists ? dir.GetDirectories().Where(x=>!x.Name.ToLower().Contains("svn")).ToArray() : null;
        }

        public static FileInfo[] GetCodeTemplateList()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/DevAdmin/CodeTemplates");

            DirectoryInfo dir = new DirectoryInfo(filePath);
            return dir.Exists ? dir.GetFiles("*.aspx") : null;
        }


        public static SmtpSettings GetSmtpSettings()
        {
            SmtpSettings smtpSettings = new SmtpSettings();

            if (WebConfigSettings.EnableSiteSettingsSmtpSettings)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings != null)
                {
                    if (siteSettings.SMTPUser.Length > 0)
                    {
                        smtpSettings.User = CryptoHelper.Decrypt(siteSettings.SMTPUser);
                    }

                    if (siteSettings.SMTPPassword.Length > 0)
                    {
                        smtpSettings.Password = CryptoHelper.Decrypt(siteSettings.SMTPPassword);
                    }

                    smtpSettings.Server = siteSettings.SMTPServer;
                    smtpSettings.Port = siteSettings.SMTPPort;
                    smtpSettings.RequiresAuthentication = siteSettings.SMTPRequiresAuthentication;
                    smtpSettings.UseSsl = siteSettings.SMTPUseSsl;
                    smtpSettings.PreferredEncoding = siteSettings.SMTPPreferredEncoding;


                }

            }
            else
            {
                if (ConfigurationManager.AppSettings["SMTPUser"] != null)
                {
                    smtpSettings.User = ConfigurationManager.AppSettings["SMTPUser"];
                }

                if (ConfigurationManager.AppSettings["SMTPPassword"] != null)
                {
                    smtpSettings.Password = ConfigurationManager.AppSettings["SMTPPassword"];
                }
                if (ConfigurationManager.AppSettings["SMTPServer"] != null)
                {
                    smtpSettings.Server = ConfigurationManager.AppSettings["SMTPServer"];
                }

                smtpSettings.Port = ConfigHelper.GetIntProperty("SMTPPort", 25);

                bool byPassContext = true;
                smtpSettings.RequiresAuthentication = ConfigHelper.GetBoolProperty("SMTPRequiresAuthentication", false, byPassContext); ;
                smtpSettings.UseSsl = ConfigHelper.GetBoolProperty("SMTPUseSsl", false, byPassContext);

                if (
               (ConfigurationManager.AppSettings["SmtpPreferredEncoding"] != null)
               && (ConfigurationManager.AppSettings["SmtpPreferredEncoding"].Length > 0)
               )
                {
                    smtpSettings.PreferredEncoding = ConfigurationManager.AppSettings["SmtpPreferredEncoding"];
                }

            }

            return smtpSettings;
        }

        /// <summary>
        /// deprecated, you should pass in the Page
        /// </summary>
        /// <returns></returns>
        public static string GetSkinBaseUrl()
        {
            return GetSkinBaseUrl(null);
        }

        public static string GetSkinBaseUrl(Page page)
        {
            bool allowPageOverride = true;
            return GetSkinBaseUrl(allowPageOverride, page);
        }


        public static string GetSkinBaseUrl(bool allowPageOverride, Page page)
        {
            if (allowPageOverride) return GetSkinBaseUrlWithOverride(page);

            return GetSkinBaseUrlNoOverride(page);
        }

        private static string GetSkinBaseUrlNoOverride(Page page)
        {
            if (HttpContext.Current == null) return String.Empty;

            string baseUrl = HttpContext.Current.Items["skinBaseUrlfalse"] as string;
            if (baseUrl == null)
            {
                baseUrl = DetermineSkinBaseUrl(false, page);
                if (baseUrl != null)
                    HttpContext.Current.Items["skinBaseUrlfalse"] = baseUrl;
            }
            return baseUrl;


        }

        private static string GetSkinBaseUrlWithOverride(Page page)
        {
            if (HttpContext.Current == null) return String.Empty;

            string baseUrl = HttpContext.Current.Items["skinBaseUrltrue"] as string;
            if (baseUrl == null)
            {
                baseUrl = DetermineSkinBaseUrl(true, page);
                if (baseUrl != null)
                    HttpContext.Current.Items["skinBaseUrltrue"] = baseUrl;
            }
            return baseUrl;


        }

        private static string DetermineSkinBaseUrl(bool allowPageOverride, Page page)
        {
            bool fullUrl = true;

            return DetermineSkinBaseUrl(allowPageOverride, fullUrl, page);

            
        }

        public static string DetermineSkinBaseUrl(string skinName)
        {
            if (string.IsNullOrEmpty(skinName)) { return WebUtils.GetSiteRoot() + "/Data/Skins/styleshout-techmania/"; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return WebUtils.GetSiteRoot() + "/Data/Skins/styleshout-techmania/"; }


            string skinUrl = String.Format("{0}/Data/Sites/{1}/skins/{2}/", WebUtils.GetSiteRoot(), siteSettings.SiteId.ToString(CultureInfo.InvariantCulture), skinName);

            return skinUrl;

        }

        public static string DetermineSkinBaseUrl(bool allowPageOverride, bool fullUrl, Page page)
        {
            string skinFolder;
            string siteRoot = string.Empty;

            if (fullUrl)
            {
                siteRoot = WebUtils.GetSiteRoot();
                skinFolder = siteRoot + "/Data/Sites/1/skins/";
            }
            else
            {
                skinFolder = "/Data/Sites/1/skins/";
            }

            string currentSkin = "styleshout-techmania/";

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            PageSettings currentPage = CacheHelper.GetCurrentPage();

            if (siteSettings != null)
            {
                currentSkin = siteSettings.Skin + "/";

                if (siteSettings.AllowUserSkins)
                {
                    string skinCookieName = "CUserSkin" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
                    if (CookieHelper.CookieExists(skinCookieName))
                    {
                        string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
                        if (cookieValue.Length > 0)
                        {
                            currentSkin = cookieValue + "/";
                        }
                    }
                }

                if (
                    (allowPageOverride)
                    && (currentPage != null)
                    && (siteSettings.AllowPageSkins)
                    && ((page != null)
                        && (!(page is NonCmsBasePage))
                       )
                    )
                {
                    if (currentPage.Skin.Length > 0)
                    {
                        currentSkin = currentPage.Skin + "/";

                    }
                }

                skinFolder = siteRoot + "/Data/Sites/"
                    + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/skins/";


                if (HttpContext.Current.Request.Params.Get("skin") != null)
                {
                    currentSkin = SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin")) + "/";
                }
            }

            return skinFolder + currentSkin;
        }

        public static int ParseSiteIdFromSkinRequestUrl()
        {
            int siteId = -1;

            if (HttpContext.Current == null) { return siteId; }
            if (HttpContext.Current.Request == null) { return siteId; }

            if (
                (HttpContext.Current.Request.RawUrl.IndexOf("Data/Sites/") == -1)
                || (HttpContext.Current.Request.RawUrl.IndexOf("/skins/") == -1)
                )
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings != null) { return siteSettings.SiteId; }

            }

            string tagged = HttpContext.Current.Request.RawUrl.Replace("/Sites/", "|").Replace("/skins/", "|");
            try
            {
                string strId = tagged.Substring(tagged.IndexOf("|") + 1, tagged.LastIndexOf("|") - tagged.IndexOf("|") - 1);

                //log.Info("Parsing siteId to " + strId);

                int.TryParse(strId, NumberStyles.Integer, CultureInfo.InvariantCulture, out siteId);
            }
            catch (ArgumentOutOfRangeException)
            {
                log.Error("Could not parse siteid from skin url so using SiteSettings.");
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings != null) { return siteSettings.SiteId; }
            
            }

            return siteId;


        }

        public static string GetSkinName(bool allowPageOverride, Page page)
        {
            
            string currentSkin = "styleshout-refresh";

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            PageSettings currentPage = CacheHelper.GetCurrentPage();

            if (siteSettings != null)
            {
                currentSkin = siteSettings.Skin;

                if (siteSettings.AllowUserSkins)
                {
                    string skinCookieName = "CUserSkin" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
                    if (CookieHelper.CookieExists(skinCookieName))
                    {
                        string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
                        if (cookieValue.Length > 0)
                        {
                            currentSkin = cookieValue;
                        }
                    }
                }

                if (
                    (allowPageOverride)
                    && (currentPage != null)
                    && (siteSettings.AllowPageSkins)
                     && ((page != null) && (!(page is NonCmsBasePage)))
                    )
                {
                    if (currentPage.Skin.Length > 0)
                    {
                        currentSkin = currentPage.Skin;

                    }
                }

                if (HttpContext.Current.Request.Params.Get("skin") != null)
                {
                    currentSkin = SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin"));
                }

            }

            return currentSkin;
        }

        public static string GetStyleSheetUrl(Page page)
        {
            bool allowPageOverride = false;
            string cssUrl = String.Format("{0}csshandler.ashx?skin={1}&amp;config=style.config", SiteUtils.GetSkinBaseUrl(allowPageOverride, page), SiteUtils.GetSkinName(allowPageOverride, page));

            return cssUrl;
        }

        public static string ChangeRelativeUrlsToFullyQuailifiedUrls(string navigationSiteRoot, string imageSiteRoot, string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent)) { return htmlContent; }
            if (string.IsNullOrEmpty(navigationSiteRoot)) { return htmlContent; }
            if (string.IsNullOrEmpty(imageSiteRoot)) { return htmlContent; }

            return htmlContent.Replace("href=\"/", String.Format("href=\"{0}/", navigationSiteRoot)).Replace("href='/", String.Format("href='{0}/", navigationSiteRoot)).Replace("src=\"/", String.Format("src=\"{0}/", imageSiteRoot)).Replace("src='/", String.Format("src='{0}/", imageSiteRoot));

        }

        public static string GetRegexRelativeImageUrlPatern()
        {
            return String.Format("^{0}/.*[_a-zA-Z0-9]+\\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$", WebUtils.GetSiteRoot());
        }
        

        public static string GetNavigationSiteRoot()
        {
            if (HttpContext.Current == null) return string.Empty;

            if (HttpContext.Current.Items["navigationRoot"] != null)
            {
                return HttpContext.Current.Items["navigationRoot"].ToString();
            }
            
            string navigationRoot = WebUtils.GetSiteRoot();
            bool useFolderForSiteDetection = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);

            if (useFolderForSiteDetection)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

                if ((siteSettings != null)
                    && (siteSettings.SiteFolderName.Length > 0))
                {
                    navigationRoot = siteSettings.SiteRoot;
                }
            }

            HttpContext.Current.Items["navigationRoot"] = navigationRoot;
            
            return navigationRoot;
           
        }

        public static string GetSecureNavigationSiteRoot()
        {
            if (HttpContext.Current == null) return string.Empty;

            if (HttpContext.Current.Items["securenavigationRoot"] != null)
            {
                return HttpContext.Current.Items["securenavigationRoot"].ToString();
            }

            string navigationRoot = WebUtils.GetSecureSiteRoot();
            bool useFolderForSiteDetection = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);

            if (useFolderForSiteDetection)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

                if ((siteSettings != null)
                    && (siteSettings.SiteFolderName.Length > 0))
                {
                    navigationRoot = String.Format("{0}/{1}", navigationRoot, siteSettings.SiteFolderName);
                }
            }

            HttpContext.Current.Items["securenavigationRoot"] = navigationRoot;

            return navigationRoot;

        }

        public static string GetCurrentPageUrl()
        {
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            return GetPageUrl(currentPage);

        }

        private static string PageTitleFormatName()
        {
            if (HttpContext.Current == null) { return string.Empty; }

            if (HttpContext.Current.Items["PageTitleFormatName"] != null)
            {
                return HttpContext.Current.Items["PageTitleFormatName"].ToString();
            }

            HttpContext.Current.Items["PageTitleFormatName"] = WebConfigSettings.PageTitleFormatName;

            return HttpContext.Current.Items["PageTitleFormatName"].ToString();

        }

        private static string PageTitleSeparatorString()
        {
            if (HttpContext.Current == null) { return string.Empty; }

            if (HttpContext.Current.Items["PageTitleSeparatorString"] != null)
            {
                return HttpContext.Current.Items["PageTitleSeparatorString"].ToString();
            }

            HttpContext.Current.Items["PageTitleSeparatorString"] = WebConfigSettings.PageTitleSeparatorString;

            return HttpContext.Current.Items["PageTitleSeparatorString"].ToString();

        }

        public const string TitleFormat_TitleOnly = "TitleOnly";
        public const string TitleFormat_SitePlusTitle = "SitePlusTitle";
        public const string TitleFormat_TitlePlusSite = "TitlePlusSite";

        public static string FormatPageTitle(SiteSettings siteSettings, string topicTitle)
        {
            if (siteSettings == null) { return HttpUtility.HtmlEncode(topicTitle); }

            //TODO: could/should make this driven by site settings instead of web.config

            string pageTitle;
            switch (PageTitleFormatName())
            {
                case TitleFormat_TitleOnly:
                    pageTitle = topicTitle;
                    break;
                case TitleFormat_TitlePlusSite:
                    pageTitle = topicTitle + PageTitleSeparatorString() + siteSettings.SiteName;
                    break;
                case TitleFormat_SitePlusTitle:
                default:
                    pageTitle = siteSettings.SiteName + PageTitleSeparatorString() + topicTitle;
                    break;
            }

            if ((pageTitle.Length > 65) && (WebConfigSettings.AutoTruncatePageTitles))
            {
                pageTitle = UIHelper.CreateExcerpt(pageTitle, 65);
            }

            return HttpUtility.HtmlEncode(pageTitle);

        }

        public static string GetPageUrl(PageSettings pageSettings)
        {
            string navigationRoot = string.Empty;
            bool useFolderForSiteDetection = ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (
                (siteSettings == null)
                ||(pageSettings == null)
                ||(pageSettings.PageId == -1)
                ||(pageSettings.SiteId != siteSettings.SiteId)
                )
            {
                navigationRoot = WebUtils.GetSiteRoot();
                return navigationRoot;
            }

            if (pageSettings.Url.StartsWith("http")) return pageSettings.Url;

            string resolvedUrl;

            if (pageSettings.UseUrl)
            {
                
                if ((pageSettings.Url.StartsWith("~/")) && (pageSettings.Url.EndsWith(".aspx")))
                {
                    if (pageSettings.UrlHasBeenAdjustedForFolderSites)
                    {
                        resolvedUrl = pageSettings.Url.Replace("~/", "/");
                    }
                    else
                    {
                        resolvedUrl = siteSettings.SiteRoot
                            + pageSettings.Url.Replace("~/", "/");
                    }

                }
                else
                {
                    resolvedUrl = pageSettings.Url;
                }

            }
            else
            {
                resolvedUrl = siteSettings.SiteRoot
                    + "/Default.aspx?pageid="
                    + pageSettings.PageId.ToString(CultureInfo.InvariantCulture);
                
            }



            return resolvedUrl;

        }

        public static string GetFileAttachmentUploadPath()
        {
            if (HttpContext.Current == null) { return string.Empty; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null) { return string.Empty; }

            return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/Attachments/");

        }

        public static void EnsureFileAttachmentFolder(SiteSettings siteSettings)
        {
            if (siteSettings == null) { return; }
            if (HttpContext.Current == null) { return; }

            string filePath = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/Attachments/");

            if (!Directory.Exists(filePath))
            {
                try
                {
                    Directory.CreateDirectory(filePath);
                }
                catch (IOException ex)
                {
                    log.Error(String.Format("failed to create path for file attachments {0} ", filePath), ex);
                }
            }

        }

        public static bool SslIsAvailable()
        {
            if (WebConfigSettings.SslisAvailable) { return true; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return false; }
            string key = String.Format("Site{0}-SSLIsAvailable", siteSettings.SiteId.ToInvariantString());
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigHelper.GetBoolProperty(key, false);
            }
           
            return false;

        }

        public static void ForceSsl()
        {
            if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                bool proxyPreventsSSLDetection;
                bool.TryParse(ConfigurationManager.AppSettings["ProxyPreventsSSLDetection"], out proxyPreventsSSLDetection);
                // proxyPreventsSSLDetection is false if parsing failed for any reason

                if (!proxyPreventsSSLDetection)
                {
                    string pageUrl = HttpContext.Current.Request.Url.ToString();
                    if (pageUrl.StartsWith("http:"))
                    {
                        string secureUrl = WebUtils.GetSecureSiteRoot()
                            + HttpContext.Current.Request.RawUrl;
                        
                        HttpContext.Current.Response.Redirect(secureUrl, true);
                    }
                }
            }
            else
            {
                if (!WebConfigSettings.EnableSslInChildSites)
                {
                    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                    if (!siteSettings.IsServerAdminSite) return;

                }
                WebUtils.ForceSsl();
            }
        }

        public static void ClearSsl()
        {
            //if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            //{
            string pageUrl = HttpContext.Current.Request.Url.ToString();
            if (pageUrl.StartsWith("https:"))
            {
                string insecureUrl = WebUtils.GetInSecureSiteRoot()
                    + HttpContext.Current.Request.RawUrl;

                HttpContext.Current.Response.Redirect(insecureUrl, true);
            }
            //}
            //else
            //{
                
            //    WebUtils.ClearSsl();
            //}
        }

        

        public static string GetEditorProviderName()
        {
            string providerName = "FCKeditorProvider";

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null)
            {
                providerName = siteSettings.EditorProviderName;
            }


            if (HttpContext.Current != null)
            {
                string loweredBrowser = HttpContext.Current.Request.Browser.Browser.ToLower();
                // FCKeditor doesn't work in safari or opera
                // so just force TinyMCE
                if (
                    (loweredBrowser.Contains("safari"))
                    &&(WebConfigSettings.ForceTinyMCEInSafari)
                    )
                {
                    providerName = "TinyMCEProvider";
                }

                if (
                    (loweredBrowser.Contains("opera"))
                    && (WebConfigSettings.ForceTinyMCEInOpera)
                    )
                {
                    providerName = "TinyMCEProvider";
                }

            }

            //TODO: could have user preferences

            return providerName;
        }

        public static void SetCurrentSkinBaseUrl(SiteSettings siteSettings)
        {
            string siteRoot = WebUtils.GetSiteRoot();
            string skinFolder = siteRoot + "/Data/Sites/1/skins/";
            string currentSkin = "jwv1/";


            if (siteSettings != null)
            {
                currentSkin = siteSettings.Skin + "/";

                if (siteSettings.AllowUserSkins)
                {
                    string skinCookieName = "CUserSkin" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

                    if (CookieHelper.CookieExists(skinCookieName))
                    {
                        string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
                        if (cookieValue.Length > 0)
                        {
                            currentSkin = cookieValue + "/";
                        }
                    }
                }

                //if (siteSettings.AllowPageSkins)
                //{
                //    if (siteSettings.ActivePage.Skin.Length > 0)
                //    {
                //        currentSkin = siteSettings.ActivePage.Skin + "/";

                //    }
                //}

                skinFolder = String.Format("{0}/Data/Sites/{1}/skins/", siteRoot, siteSettings.SiteId.ToString(CultureInfo.InvariantCulture));


                //if (HttpContext.Current.Request.Params.Get("skin") != null)
                //{
                //    currentSkin = HttpContext.Current.Request.Params.Get("skin") + "/";

                //}

                siteSettings.SkinBaseUrl = skinFolder + currentSkin;
            }


        }


        #region Url Rewrite tracking

        public static void TrackUrlRewrite()
        {
            if (HttpContext.Current == null) return;

            HttpContext.Current.Items["urlwasrewritten"] = true;

        }

        public static bool UrlWasReWritten()
        {
            if (HttpContext.Current.Items["urlwasrewritten"] != null)
            {
                return true;
            }

            return false;

        }


        #endregion


        //public static string GetRoleCookieName()
        //{
        //    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
        //    return GetRoleCookieName(siteSettings);
        //}

        public static string GetRoleCookieName(SiteSettings siteSettings)
        {
            String hostName = WebUtils.GetHostName();
            if (WebConfigSettings.UseRelatedSiteMode)
            {
                return hostName + "portalroles";
            }

            if (siteSettings == null) { return hostName + "portalroles1"; }

            return String.Format("{0}portalroles{1}", hostName, siteSettings.SiteId.ToString(CultureInfo.InvariantCulture));

        }

        public static void RedirectToSignOut()
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Redirect(SiteUtils.GetNavigationSiteRoot() + "/Logoff.aspx", true);

        }

        #region Current User

        public static SiteUser GetCurrentSiteUser()
        {
            bool bypassAuthCheck = false;

            SiteUser currentUser = GetCurrentSiteUser(bypassAuthCheck);

            if ((currentUser != null)&&(currentUser.IsLockedOut))
            {
                RedirectToSignOut();
                return null;
            }

            return currentUser;
            
        }

        public static SiteUser GetCurrentSiteUser(bool bypassAuthCheck)
        {
            if (HttpContext.Current == null) return null;

           
            if (
                bypassAuthCheck
                || (HttpContext.Current.Request.IsAuthenticated)
                )
            {
                if (HttpContext.Current.Items["CurrentUser"] != null)
                {
                    return (SiteUser)HttpContext.Current.Items["CurrentUser"];
                }

                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings != null)
                {
                    SiteUser siteUser = new SiteUser(siteSettings, HttpContext.Current.User.Identity.Name);
                    if (siteUser.UserId > -1)
                    {
                        HttpContext.Current.Items["CurrentUser"] = siteUser;

                        return siteUser;
                    }
                }
            }

            return null;
        }

        public static string SuggestLoginNameFromEmail(int siteId, string email)
        {
            string login = email.Substring(0, email.IndexOf("@"));
            int offset = 1;
            while (SiteUser.LoginExistsInDB(siteId, login))
            {
                login = login + offset.ToString(CultureInfo.InvariantCulture);
                offset += 1;
            }

            return login;
        }

        public static SiteUser CreateMinimalUser(SiteSettings siteSettings, string email, bool includeInMemberList, string adminComments)
        {
            if (siteSettings == null)
            {
                throw new ArgumentException("a valid siteSettings object is required for this method");
            }
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("a valid email address is required for this method");
            }

            if (!Email.IsValidEmailAddressSyntax(email))
            {
                throw new ArgumentException("a valid email address is required for this method");
            }

            //first make sure he doesn't exist
            SiteUser siteUser = SiteUser.GetByEmail(siteSettings, email);
            if ((siteUser != null)&&(siteUser.UserGuid != Guid.Empty)) { return siteUser; }

            siteUser = new SiteUser(siteSettings);
            siteUser.Email = email;
            string login = SuggestLoginNameFromEmail(siteSettings.SiteId, email);
            //int offset = 1;
            //while (SiteUser.LoginExistsInDB(siteSettings.SiteId, login))
            //{
            //    login = login + offset.ToString(CultureInfo.InvariantCulture);
            //    offset += 1;
            //}

            siteUser.LoginName = login;
            siteUser.Name = login;
            siteUser.Password = SiteUser.CreateRandomPassword(7);
            CMembershipProvider m = Membership.Provider as CMembershipProvider;
            if (m != null)
            {
                siteUser.Password = m.EncodePassword(siteUser.Password, siteSettings);
            }

            siteUser.ProfileApproved = true;
            siteUser.DisplayInMemberList = includeInMemberList;
            siteUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
            siteUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;

            if (!string.IsNullOrEmpty(adminComments)) { siteUser.Comment = adminComments; }
            
            siteUser.Save();

            Role.AddUserToDefaultRoles(siteUser);

            return siteUser;

        }

        

        public static bool UserIsSiteEditor()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if(siteSettings != null)
            {
                return  (WebConfigSettings.UseRelatedSiteMode) && (WebUser.IsInRoles(siteSettings.SiteRootEditRoles));
            }

            return false;
        }

        public static bool UserCanEditModule(int moduleId)
        {
            if (HttpContext.Current == null) return false;
            if (!HttpContext.Current.Request.IsAuthenticated) return false;

            if (WebUser.IsAdminOrContentAdmin) return true;

            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (currentPage == null) return false;

            bool moduleFoundOnPage = false;
            foreach (Module m in currentPage.Modules)
            {
                if (m.ModuleId == moduleId) moduleFoundOnPage = true;
            }

            if (!moduleFoundOnPage) return false;

            if (WebUser.IsInRoles(currentPage.EditRoles)) return true;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return false;

            foreach (Module m in currentPage.Modules)
            {
                if (m.ModuleId == moduleId)
                {
                    if (m.EditUserId == currentUser.UserId) return true;
                    if (WebUser.IsInRoles(m.AuthorizedEditRoles)) return true;
                }
            }

            return false;

        }


        public static void TrackUserActivity()
        {
            if (HttpContext.Current == null) return;
            if (HttpContext.Current.Request == null) return;

            

            if (
                (HttpContext.Current.User.Identity.IsAuthenticated)
                &&(WebConfigSettings.TrackAuthenticatedRequests)
                )
            {
                if (
                (HttpContext.Current.Request.Path.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith(".axd", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith("upgrade.aspx", StringComparison.InvariantCultureIgnoreCase))
                    || (HttpContext.Current.Request.Path.EndsWith("setup/default.aspx", StringComparison.InvariantCultureIgnoreCase))
                    )
                {
                    return;

                }

                //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                //if (siteSettings != null)
                //{
                bool bypassAuthCheck = false;
                SiteUser siteUser = GetCurrentSiteUser(bypassAuthCheck);
                    //SiteUser siteUser = new SiteUser(siteSettings, HttpContext.Current.User.Identity.Name);
                    if ((siteUser != null) && (siteUser.UserId > -1))
                    {
                        siteUser.UpdateLastActivityTime();

                        if (HttpContext.Current.Request == null) return;

                        if (WebConfigSettings.TrackIPForAuthenticatedRequests)
                        {
                            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                            if (siteSettings == null) return;

                            // track user ip address
                            UserLocation userLocation = new UserLocation(
                                siteUser.UserGuid,
                                GetIP4Address());

                            userLocation.SiteGuid = siteSettings.SiteGuid;
                            userLocation.Hostname = HttpContext.Current.Request.UserHostName;
                            userLocation.Save();


                        }
                    }
                //}
            }

        }


        #endregion

        public static void QueueIndexing()
        {
            if (!WebConfigSettings.IsSearchIndexingNode) { return;}

            if (IndexWriterTask.IsRunning()) { return; }

            IndexWriterTask indexWriter = new IndexWriterTask();
            
            indexWriter.StoreContentForResultsHighlighting = WebConfigSettings.EnableSearchResultsHighlighting;

            // Commented out 2009-01-24
            // seems to cause errors for some languages if we localize this
            // perhaps because the background thread is not running on the same culture as the
            // web ui which is driven by browser language preferecne.
            // if we do localize it we should localize to the site default culture, not the user's
            //indexWriter.TaskName = Resource.IndexWriterTaskName;
            //indexWriter.StatusCompleteMessage = Resource.IndexWriterTaskCompleteMessage;
            //indexWriter.StatusQueuedMessage = Resource.IndexWriterTaskQueuedMessage;
            //indexWriter.StatusStartedMessage = Resource.IndexWriterTaskStartedMessage;
            //indexWriter.StatusRunningMessage = Resource.IndexWriterTaskRunningFormatString;

            indexWriter.QueueTask();

            WebTaskManager.StartOrResumeTasks();

        }

        public static String GetFullPathToThemeFile()
        {
            string pathToFile = null;

            if (HttpContext.Current != null)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                pathToFile = HttpContext.Current.Server.MapPath(
                    String.Format("~/Data/Sites/{0}/skins/{1}/theme.skin", siteSettings.SiteId.ToString(CultureInfo.InvariantCulture), siteSettings.Skin));

            }

            return pathToFile;
        }


        public static int GetCurrentPageDepth(SiteMapNode rootNode)
        {
            
            PageSettings pageSettings = CacheHelper.GetCurrentPage();
            if ((pageSettings == null)
                ||(pageSettings.ParentId == -1)
                )
            {
                return 0;
            }

            SiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
            if (currentPageNode == null) { return 0; }

            if(!(currentPageNode is CSiteMapNode)){ return 0;}

            CSiteMapNode node = currentPageNode as CSiteMapNode;
            return node.Depth;
            
            
        }

        public static String GetActivePageValuePath(SiteMapNode rootNode, int offSet)
        {
            String valuePath = String.Empty;

            PageSettings pageSettings = CacheHelper.GetCurrentPage();

            SiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
            if (currentPageNode == null) { return string.Empty; }

            if (!(currentPageNode is CSiteMapNode)) { return string.Empty; }

            CSiteMapNode node = currentPageNode as CSiteMapNode;


            valuePath = node.PageGuid.ToString();

            while ((node != null) && (node.ParentId > -1))
            {
                if (node.ParentNode != null)
                {
                    node = node.ParentNode as CSiteMapNode;

                    valuePath = String.Format("{0}|{1}", node.PageGuid, valuePath);
                }
                
            }

            
            if (offSet > 0)
            {
                for (int i = 1; i <= offSet; i++)
                {
                    if (valuePath.IndexOf("|") > -1)
                    {
                        valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
                    }

                }

            }

            return valuePath;
        }

        /// <summary>
        /// this overload was added to supp-ort menu highlighting on physical .aspx pages added to the menu
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="offSet"></param>
        /// <param name="currentUrl"></param>
        /// <returns></returns>
        public static String GetActivePageValuePath(SiteMapNode rootNode, int offSet, string currentUrl)
        {
            string valuePath = string.Empty;

            SiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, currentUrl);
            if (currentPageNode == null) { return string.Empty; }

            if (!(currentPageNode is CSiteMapNode)) { return string.Empty; }

            CSiteMapNode node = currentPageNode as CSiteMapNode;


            valuePath = node.PageGuid.ToString();

            while ((node != null) && (node.ParentId > -1))
            {
                if (node.ParentNode != null)
                {
                    node = node.ParentNode as CSiteMapNode;

                    valuePath = String.Format("{0}|{1}", node.PageGuid, valuePath);
                }

            }


            if (offSet > 0)
            {
                for (int i = 1; i <= offSet; i++)
                {
                    if (valuePath.IndexOf("|") > -1)
                    {
                        valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
                    }

                }

            }

            return valuePath;


           // return valuePath;
        }
        

        public static String GetPageMenuActivePageValuePath(SiteMapNode rootNode)
        {

            String valuePath = GetActivePageValuePath(rootNode, 0);
            
            // need to remove the topmost level from value path
            // which is from the beginning to the first separator
            if (valuePath.IndexOf("|") > -1)
            {
                valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
            }


            return valuePath;
        }


        public static bool TopPageHasChildren(SiteMapNode rootNode)
        {
            return TopPageHasChildren(rootNode, 0);
        }

        public static CSiteMapNode GetCurrentPageSiteMapNode(SiteMapNode rootNode)
        {
            if (rootNode == null) { return null; }
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (currentPage == null) { return null; }

            return GetSiteMapNodeForPage(rootNode, currentPage);

        }

        public static CSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, PageSettings pageSettings)
        {
            if (rootNode == null) { return null; }
            if (pageSettings == null) { return null; }
            if(!(rootNode is CSiteMapNode)){ return null;}

            foreach (SiteMapNode childNode in rootNode.ChildNodes)
            {
                if (!(childNode is CSiteMapNode)) { return null; }

                CSiteMapNode node = childNode as CSiteMapNode;
                if (node.PageId == pageSettings.PageId) { return node; }

                CSiteMapNode foundNode = GetSiteMapNodeForPage(node, pageSettings);
                if (foundNode != null) { return foundNode; }


            }

            return null;

        }

        /// <summary>
        /// this overload was added to implement support for menu highlighting in inline code .aspx pages
        /// added to the menu
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="currentUrl"></param>
        /// <returns></returns>
        public static CSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, string currentUrl)
        {
            if (rootNode == null) { return null; }
            if (string.IsNullOrEmpty(currentUrl)) { return null; }
            if (!(rootNode is CSiteMapNode)) { return null; }

            foreach (SiteMapNode childNode in rootNode.ChildNodes)
            {
                if (!(childNode is CSiteMapNode)) { return null; }

                CSiteMapNode node = childNode as CSiteMapNode;
                if (string.Equals(node.Url.Replace("~", string.Empty),currentUrl, StringComparison.InvariantCultureIgnoreCase)) { return node; }

                CSiteMapNode foundNode = GetSiteMapNodeForPage(node, currentUrl);
                if (foundNode != null) { return foundNode; }


            }

            return null;

        }

        public static CSiteMapNode GetTopLevelParentNode(SiteMapNode siteMapNode)
        {
            if (siteMapNode == null) { return null; }
            if (!(siteMapNode is CSiteMapNode)) { return null; }

            CSiteMapNode node = siteMapNode as CSiteMapNode;

            if (node.ParentId < 0) { return node; }

            while ((node != null)&&(node.ParentId > -1))
            {
                if (node.ParentNode != null)
                {
                    node = node.ParentNode as CSiteMapNode;
                }
                else
                {
                    return node;
                }
            }

            return node;


        }


        public static bool NodeHasVisibleChildrenAtDepth(CSiteMapNode node, int depth)
        {
            bool recurse = true;
            return NodeHasVisibleChildrenAtDepth(node, depth, recurse);
        }


        public static bool NodeHasVisibleChildrenAtDepth(CSiteMapNode node, int depth, bool recurse)
        {
            foreach (SiteMapNode cNode in node.ChildNodes)
            {
                if (!(cNode is CSiteMapNode)) { return false; }

                CSiteMapNode childNode = cNode as CSiteMapNode;
                if (childNode.Depth >= depth)
                {
                    if ((childNode.IncludeInMenu)&&(WebUser.IsInRoles(childNode.ViewRoles))) { return true; }
                }
                else
                {
                    if (recurse)
                    {
                        if (NodeHasVisibleChildrenAtDepth(childNode, depth, recurse)) { return true; }
                    }
                }
                
            }

            return false;
        }

        /// <summary>
        /// A helper method for determining if the top level page has children at this offsetlevel
        /// used to determine whether to show or hide left or right div if it contains a menu.
        /// </summary>
        /// <param name="startingNodeOffset"></param>
        /// <returns></returns>
        public static bool TopPageHasChildren(SiteMapNode rootNode, int startingNodeOffset)
        {
            if (rootNode == null) { return false; }

            PageSettings pageSettings = CacheHelper.GetCurrentPage();
            
            if (pageSettings == null) return false;

            if (
                (pageSettings.ParentId == -1)
                && (startingNodeOffset > 0)
                )
            {
                return false;
            }

            if (
                (pageSettings.ParentId > -1)
                && (pageSettings.IncludeInMenu) //added 2009-05-06
                && (startingNodeOffset == 0)
                )
            {
                return true;
            }

            CSiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
            if (currentPageNode == null) { return false; }

            if (startingNodeOffset >= 2)
            {
                if ((currentPageNode.Depth >= startingNodeOffset)&&(currentPageNode.IncludeInMenu)) { return true; }

                bool recurse = false;
                CSiteMapNode parent;

                if (NodeHasVisibleChildrenAtDepth(currentPageNode, startingNodeOffset, recurse)) { return true; }

                if (currentPageNode.ParentNode != null)
                {
                    parent = currentPageNode.ParentNode as CSiteMapNode;
                    if (parent.Depth >= startingNodeOffset)
                    {
                        return NodeHasVisibleChildrenAtDepth(parent, startingNodeOffset, recurse);
                    }
                }

                return false;

            }

            CSiteMapNode topParent = GetTopLevelParentNode(currentPageNode);
            if (topParent == null) { return false; }

            if (NodeHasVisibleChildrenAtDepth(topParent, startingNodeOffset)) { return true; }

            return false;

            
        }

        public static String GetStartUrlForPageMenu(SiteMapNode rootNode, int startingNodeOffset)
        {
           
            PageSettings pageSettings = CacheHelper.GetCurrentPage();

            SiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
            if (currentPageNode == null) { return string.Empty; }

            if (startingNodeOffset == 0) 
            {
                SiteMapNode startingNode = GetTopLevelParentNode(currentPageNode);
                if (startingNode == null) { return string.Empty; }

                return startingNode.Url; 
            }

            //work our way up from the current page to the parent node at the offset depth
            CSiteMapNode n = currentPageNode as CSiteMapNode;

            while ((n.ParentNode != null) && (n.Depth > startingNodeOffset))
            {
                n = n.ParentNode as CSiteMapNode;
            }

            
            return n.Url;


        }

        // TODO: this really shouldn't be here as it is specific to vivasky.com
        // purpose is to not show unsecure image on ssl pages
        public static string GetSiteScoreImageMarkup()
        {

            string markup = "<img src='http://sitescore.silktide.com/index.php?siteScoreUrl=http%3A%2F%2Fwww.vivasky.com' "
                + "alt='Silktide Sitescore for this website' style='border: 0' />";

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
                {
                    markup = String.Empty;
                }
            }

            return markup;

        }

        public static string GetProfileLink(object objUserId, object userName)
        {
            string result = string.Empty;
            if (objUserId != null)
            {
                int userId = Convert.ToInt32(objUserId, CultureInfo.InvariantCulture);

                if (userName.ToString().Length > 0)
                {
                    result = String.Format("<a  href='{0}/ProfileView.aspx?userid={1}'>{2}</a>", GetNavigationSiteRoot(), userId.ToString(CultureInfo.InvariantCulture), userName);
                }
            }

            return result;

        }

        public static string GetProfileLink(Page page, object objUserId, object userName)
        {
            string result = string.Empty;
            if (objUserId != null)
            {
                int userId = Convert.ToInt32(objUserId, CultureInfo.InvariantCulture);

                if (userName.ToString().Length > 0)
                {
                    result = String.Format("<a  href='{0}/ProfileView.aspx?userid={1}'>{2}</a>", GetNavigationSiteRoot(), userId.ToString(CultureInfo.InvariantCulture), userName);
                }
            }

            return result;

        }

        public static string GetProfileAvatarLink(
            Page page, 
            object objUserId, 
            int siteId,
            String avatar,
            String toolTip)
        {
            string result = string.Empty;
            if (objUserId != null)
            {
                int userId = Convert.ToInt32(objUserId);

               
                    if ((avatar == null) || (avatar == String.Empty))
                    {
                        avatar = "blank.gif";
                    }
                    String avaterImageMarkup = String.Format("<img title='{0}'  alt='{0}' src='{1}' />", toolTip, page.ResolveUrl(String.Format("~/Data/Sites/{0}/useravatars/{1}", siteId.ToString(CultureInfo.InvariantCulture), avatar)));

                    result = String.Format("<a title='{0}' href='{1}/ProfileView.aspx?userid={2}'>{3}</a>", toolTip, GetNavigationSiteRoot(), userId.ToString(CultureInfo.InvariantCulture), avaterImageMarkup);
                
            }

            return result;

        }

        public static string GetGmapApiKey()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings.GmapApiKey.Length > 0)
                return siteSettings.GmapApiKey;

            return WebConfigSettings.GoogleMapsAPIKey;


        }

        public static CultureInfo GetDefaultCulture()
        {
            
            if (WebConfigSettings.UseCultureOverride)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if ((siteSettings != null) && (siteSettings.SiteId > -1))
                {
                    string siteCultureKey = String.Format("site{0}culture", siteSettings.SiteId.ToString(CultureInfo.InvariantCulture));

                    if (ConfigurationManager.AppSettings[siteCultureKey] != null)
                    {
                        try
                        {
                            string cultureName = ConfigurationManager.AppSettings[siteCultureKey];

                            // change these neutral cultures which cannot be used to reasonable specific cultures
                            if (cultureName == "zh-CHS") { cultureName = "zh-CN"; }
                            if (cultureName == "zh-CHT") { cultureName = "zh-HK"; }

                            CultureInfo siteCulture = new CultureInfo(cultureName);
                            return siteCulture;
                        }
                        catch { }
                    }
                }
            }

            return ResourceHelper.GetDefaultCulture();

        }

        public static Guid GetDefaultCountry()
        {

            // US
            Guid defaultCountry = new Guid("a71d6727-61e7-4282-9fcb-526d1e7bc24f");

            // TODO: add config setting, siteSetting ?

            return defaultCountry;

        }

        public static string GetTimeZoneLabel(double timeOffset)
        {
            string key = "TZ" + timeOffset.ToString(CultureInfo.InvariantCulture).Replace(".", string.Empty).Replace("-", "minus");
            return ResourceHelper.GetResourceString("TimeZoneResources", key);

        }
        

        public static double GetUserTimeOffset()
        {
            Double timeOffset = DateTimeHelper.GetPreferredGmtOffset();

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    SiteUser siteUser = GetCurrentSiteUser();
                    if (siteUser != null)
                    {
                        timeOffset = siteUser.TimeOffsetHours;
                    }

                }

            }

            return timeOffset;

        }


        public static CommerceConfiguration GetCommerceConfig()
        {
            if (HttpContext.Current == null) return null;

            if (HttpContext.Current.Items["commerceConfig"] != null)
            {
                return (CommerceConfiguration)HttpContext.Current.Items["commerceConfig"];
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if ((siteSettings == null) || (siteSettings.SiteGuid == Guid.Empty)) return null;

            CommerceConfiguration commerceConfig = new CommerceConfiguration(siteSettings);

            HttpContext.Current.Items.Add("commerceConfig", commerceConfig);

            return commerceConfig;


        }

        /// <summary>
        /// get virtual path of a view by its name
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static string GetViewPath(string viewName) {
            var curSettings = CacheHelper.GetCurrentSiteSettings();
            var vpath = string.Format("~/data/sites/{0}/skins/{1}/{2}.ascx", curSettings.SiteId, curSettings.Skin, viewName);
            if (!File.Exists(HttpContext.Current.Server.MapPath(vpath)))
            {
                vpath = string.Format("~/controls/{0}.ascx", viewName);
            }
            return vpath;
        }

    }
}
