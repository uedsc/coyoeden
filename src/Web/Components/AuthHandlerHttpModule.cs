//previously this functionality was in Global.asax.cs
// moved here 2009-05-30
// Last Modified 2009-11-28

using System;
using System.Data.Common;
using System.IO;
using System.Web;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Business.WebHelpers.UserRegisteredHandlers;
using Cynthia.Web.Framework;
using Cynthia.Web.Security;

namespace Cynthia.Web
{
    public class AuthHandlerHttpModule : IHttpModule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthHandlerHttpModule));


        public void Init(HttpApplication application)
        {
            
            application.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);
        }

        void application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (log.IsDebugEnabled) log.Debug("AuthHandlerHttpModule Application_AuthenticateRequest");

            if (sender == null) return;

            HttpApplication app = (HttpApplication)sender;

            if (
                (app.Request.Path.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".axd", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith("thumbnailservice.ashx", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.Contains("csshandler.ashx"))
                    || (app.Request.Path.EndsWith("/Data/", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.StartsWith("/Data/", StringComparison.InvariantCultureIgnoreCase))
                    ||(app.Request.Path.Contains("GCheckoutNotificationHandler.ashx"))
                    )
            {
                return;

            }



            if (app.Request.IsAuthenticated)
            {
                if (log.IsDebugEnabled) log.Debug("IsAuthenticated == true");

                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                bool useFolderForSiteDetection = WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites;

                // Added by Haluk Eryuksel - 2006-01-23
                // support for Windows authentication
                if (
                    (app.User.Identity.AuthenticationType == "NTLM")
                    || (app.User.Identity.AuthenticationType == "Negotiate")
                    // || ( Context.User.Identity.AuthenticationType == "Windows" )
                    )
                {
                    //Added by Benedict Chan - 2008-08-05
                    //Added Cookie here so that we don't have to check the users in every page, also to authenticate under NTLM with "useFolderForSiteDetection == true"
                    string cookieName = "siteguid" + siteSettings.SiteGuid;
                    if (!CookieHelper.CookieExists(cookieName))
                    {
                        bool existsInDB;
                        existsInDB = SiteUser.LoginExistsInDB(siteSettings.SiteId, app.Context.User.Identity.Name);

                        if (!existsInDB)
                        {
                            SiteUser u = new SiteUser(siteSettings);
                            u.Name = app.Context.User.Identity.Name;
                            u.LoginName = app.Context.User.Identity.Name;
                            u.Email = "";
                            u.Password = SiteUser.CreateRandomPassword(7);
                            u.Save();
                            NewsletterHelper.ClaimExistingSubscriptions(u);

                            UserRegisteredEventArgs args = new UserRegisteredEventArgs(u);
                            OnUserRegistered(args);
                           
                        }

                        SiteUser siteUser = new SiteUser(siteSettings, app.Context.User.Identity.Name);
                        CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), true);

                        //Copied logic from SiteLogin.cs  Since we will skip them if we use NTLM
                        if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
                        {
                            SiteUtils.SetSkinCookie(siteUser);
                        }

                        // track user ip address
                        try
                        {
                            UserLocation userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address());
                            userLocation.SiteGuid = siteSettings.SiteGuid;
                            userLocation.Hostname = app.Request.UserHostName;
                            userLocation.Save();
                            log.Info("Set UserLocation : " + app.Request.UserHostName + ":" + SiteUtils.GetIP4Address());
                        }
                        catch (Exception ex)
                        {
                            log.Error(SiteUtils.GetIP4Address(), ex);
                        }
                    }

                    //End-Added by Benedict Chan

                }
                // End-Added by Haluk Eryuksel


                if ((useFolderForSiteDetection) && (!WebConfigSettings.UseRelatedSiteMode))
                {
                    // replace GenericPrincipal with custom one
                    //string roles = string.Empty;
                    if (!(app.Context.User is CIdentity))
                    {
                        app.Context.User = new CPrincipal(app.Context.User);
                    }
                }

            }


        }


        private void OnUserRegistered(UserRegisteredEventArgs e)
        {
            foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
            {
                handler.UserRegisteredHandler(null, e);
            }
        }
       


        


        


        public void Dispose() { }
    }
}
