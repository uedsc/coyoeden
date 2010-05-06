

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Profile;
using System.Web.Security;
using log4net;
using log4net.Config;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Web.Security;
using Resources;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace Cynthia.Web 
{
    public class Global : HttpApplication 
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Global));


		protected void Application_Start(Object sender, EventArgs e)
		{
            //FileInfo log4netConfig = new FileInfo("log4net.config");
            //XmlConfigurator.Configure(log4netConfig);
            //XmlConfigurator.Configure();
            //log4net.Config.XmlConfigurator.Configure();


            log.Info(Resource.ApplicationStartEventMessage);


            try
            {
                RegisterVirtualPathProvider();

            }
            catch (MissingMethodException ex)
            {   // this is broken on mono, not implemented 2006-02-04
                if (log.IsErrorEnabled) log.Error("Application_Start Could not register VirtualPathProvider, missing method in Mono", ex);

            }
            catch (SecurityException se)
            {   
                // must not be running in full trust
                if (log.IsErrorEnabled) log.Error("Application_Start Could not register VirtualPathProvider, this error is expected when running in Medium trust or lower", se);

            }
            catch (UnauthorizedAccessException ae)
            {
                // must not be running in full trust
                if (log.IsErrorEnabled) log.Error("Application_Start Could not register VirtualPathProvider, this error is expected when running in Medium trust or lower", ae);

            }

            StartOrResumeTasks();
            
		}

        private void StartOrResumeTasks()
        {
            
            // NOTE: In IIS 7 using integrated mode, HttpContext.Current will always be null in Application_Start
            // http://weblogs.asp.net/jgaylord/archive/2008/09/04/iis7-integrated-mode-and-global-asax.aspx
            if (WebConfigSettings.UseAppKeepAlive)
            {
                AppKeepAliveTask keepAlive;
                try
                {
                    try
                    {
                        if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
                        {
                            keepAlive = new AppKeepAliveTask();
                            keepAlive.UrlToRequest = WebUtils.GetSiteRoot();
                            keepAlive.MaxRunTimeMinutes = WebConfigSettings.AppKeepAliveMaxRunTimeMinutes;
                            keepAlive.MinutesToSleep = WebConfigSettings.AppKeepAliveSleepMinutes;
                            keepAlive.QueueTask();
                        }
                    }
                    catch (HttpException)
                    {
                        //this error will be thrown when using IIS 7 Integrated pipeline mode
                        //since we have no context.Request to get the site root, in IIS 7 Integrated pipeline mode
                        //we need to use an additional config setting to get the url to request for keep alive 
                        if (WebConfigSettings.AppKeepAliveUrl.Length > 0)
                        {

                            keepAlive = new AppKeepAliveTask();
                            keepAlive.UrlToRequest = WebConfigSettings.AppKeepAliveUrl;
                            keepAlive.MaxRunTimeMinutes = WebConfigSettings.AppKeepAliveMaxRunTimeMinutes;
                            keepAlive.MinutesToSleep = WebConfigSettings.AppKeepAliveSleepMinutes;
                            keepAlive.QueueTask();

                        }

                    }
                }
                catch (Exception ex)
                {
                    // if a new installation the table will not exist yet so just log and swallow
                    log.Error(ex);
                }




            }

            
            WebTaskManager.StartOrResumeTasks(true);

        }


        private void RegisterVirtualPathProvider()
        {
            // had to move this into its own method
            // in less than full trust it blows up even with a try catch if present in 
            // Application_Start, moving into a separate method works with a try catch
            HostingEnvironment.RegisterVirtualPathProvider(new CVirtualPathProvider());

        }


		protected void Application_End(Object sender, EventArgs e)
		{
			if (log.IsInfoEnabled) log.Info("Global.asax.cs Application_End" );
		}

        
        protected void Application_BeginRequest(Object sender, EventArgs e)
		{
            //this was an experiment that did not work as well as hoped
            //if ((Request.HttpMethod == "GET")&&(WebConfigSettings.CombineJavaScript))
            //{
            //    if (Request.AppRelativeCurrentExecutionFilePath.EndsWith(".aspx"))
            //    {
            //        Response.Filter = new ScriptDeferFilter(Response);
            //    }
            //}
           
        }


		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		    // update user activity at the end of each request
            // but only if the siteUser is already in the HttpContext
            // we don't want to lookup the user for little ajax requests
            // unless we have to for security checks
		    if (HttpContext.Current.User == null) 
		        return;

            SiteUtils.TrackUserActivity();

            
		}


	    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
            //2009-05-30, moved this functionality to /Components/AuthHandlerHttpModule.cs

            
        }


		protected void Application_AuthorizeRequest(Object sender, EventArgs e)
		{
			//if (log.IsDebugEnabled) log.Debug("Global.asax.cs Application_AuthorizeRequest" );
		}


        protected void Application_Error(Object sender, EventArgs e)
        {
            bool errorObtained = false;
            Exception ex = null;

            try
            {
                Exception rawException = Server.GetLastError();
                if (rawException != null)
                {
                    errorObtained = true;
                    if (rawException.InnerException != null)
                    {
                        ex = rawException.InnerException;
                    }
                    else
                    {
                        ex = rawException;
                    }
                }
            }
            catch { }

            string exceptionUrl = string.Empty;
            string exceptionIpAddress = string.Empty;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
					exceptionUrl = String.Format("{0} - {1}", CultureInfo.CurrentCulture, HttpContext.Current.Request.RawUrl);
                    exceptionIpAddress = SiteUtils.GetIP4Address();
                    
                }

            }

            if (errorObtained)
            {
                
                
                if (ex is UnauthorizedAccessException)
                {
                    // swallow this for medium trust?
					log.Error(String.Format("{0}-{1}", exceptionIpAddress, exceptionUrl), ex);
                    return;
                }

                if (ex.Message == "File does not exist.")
                {

					log.Error(String.Format("{0}-{1}", exceptionIpAddress, exceptionUrl), ex);
                    return;
                }

				log.Error(String.Format("{0}-{1}", exceptionIpAddress, exceptionUrl), ex);

                
                
            }

        }

        

		protected void Session_Start(Object sender, EventArgs e)
		{
			if (log.IsDebugEnabled) log.Debug("Global.asax.cs Session_Start" );
            IncrementUserCount();
		}


		protected void Session_End(Object sender, EventArgs e)
		{
            if (log.IsDebugEnabled) log.Debug("Global.asax.cs Session_End");
            DecrementUserCount();
		}


        private void IncrementUserCount()
        {
            String key = WebUtils.GetHostName() + "_onlineCount";
            if (Session != null)
            {
                Session["onlinecountkey"] = key;
            }
            if (log.IsDebugEnabled) log.Debug("IncrementUserCount key was " + key);

            Application.Lock();
            Application[key] = Application[key] == null ? 1 : (int)Application[key] + 1;
            Application.UnLock();
        }

        private void DecrementUserCount()
        {
            if (Session != null)
            {
                if (Session["onlinecountkey"] != null)
                {
                    String key = Session["onlinecountkey"].ToString();
                    if (key.Length > 0)
                    {
                        if (log.IsDebugEnabled) log.Debug("DecrementUserCount key was " + key);

                        Application.Lock();
                        int newCount = Application[key] == null ? 0 : (int)Application[key] - 1;
                        Application[key] = newCount > 0 ? newCount : 0;
                        Application.UnLock();
                    }
                }
            }
        }


        protected void Profile_MigrateAnonymous(Object sender, ProfileMigrateEventArgs args)
        {
            //TODO: maybe support capturing profile properties for anonymous users
            // then if they register transfer them to the new user

           
            //WebProfile anonymousProfile = new WebProfile(args.Context.Profile);
            

            //Profile.ZipCode = anonymousProfile;
            //Profile.CityAndState = anonymousProfile.CityAndState;
            //Profile.StockSymbols = anonymousProfile.StockSymbols;

            ////////
            // Delete the anonymous profile. If the anonymous ID is not 
            // needed in the rest of the site, remove the anonymous cookie.

            //ProfileManager.DeleteProfile(args.AnonymousID);
            //AnonymousIdentificationModule.ClearAnonymousIdentifier();

            // Delete the user row that was created for the anonymous user.
            //Membership.DeleteUser(args.AnonymousID, true);
        }

        #region Forms Authentication Handlers

        //public void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        //{
        //    if (FormsAuthentication.CookiesSupported)
        //    {
        //        if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
        //        {
        //            try
        //            {
                        

        //                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(
        //                  Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                        

        //                //args.User = new System.Security.Principal.GenericPrincipal(
        //                //  new Samples.AspNet.Security.MyFormsIdentity(ticket),
        //                //  new string[0]);
        //            }
        //            catch (Exception e)
        //            {
        //                // Decrypt method failed.
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new HttpException("Cookieless Forms Authentication is not " +
        //                                "supported for this application.");
        //    }
        //}

        #endregion


        

    }
}


