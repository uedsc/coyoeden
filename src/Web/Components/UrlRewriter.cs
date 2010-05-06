

using System;
using System.Data;
using System.Text;
using System.Web;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web
{
	
	public class UrlRewriter : IHttpModule
	{
        private static readonly ILog log
            = LogManager.GetLogger(typeof(UrlRewriter));


		public void Init(HttpApplication app)
		{
			app.BeginRequest += new EventHandler(this.UrlRewriter_BeginRequest);
            
		}

        

		public void Dispose() {}

		protected  void UrlRewriter_BeginRequest(object sender, EventArgs e)
		{
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
                    || (app.Request.Path.EndsWith("csshandler.ashx", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith("/Data/", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.StartsWith("/Data/", StringComparison.InvariantCultureIgnoreCase))
                    )
            {
                return;

            }

            if (WebConfigSettings.UseUrlReWriting)
            {
                try
                {
                    RewriteUrl(app);
                }
                catch (InvalidOperationException ex)
                {
                    log.Error(ex);
                }
                catch (System.Data.Common.DbException ex)
                {
                    log.Error(ex);
                }

            }
		}

        private static void RewriteUrl(HttpApplication app)
        {
            if (app == null) return;

            string requestPath = app.Request.Path;
            //if (requestPath == "/") { return; }
            
            bool useFolderForSiteDetection = WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites;

            string virtualFolderName;
            if (useFolderForSiteDetection)
            {
                virtualFolderName = VirtualFolderEvaluator.VirtualFolderName();
            }
            else
            {
                virtualFolderName = string.Empty;
            }

            bool setClientFilePath = true;
            

            if (
                (useFolderForSiteDetection)
                && (virtualFolderName.Length > 0)
                )
            {
                setClientFilePath = false;

                // requesting root of folderbased site like /folder1/
                // don't re-write it
                if (requestPath.EndsWith(virtualFolderName + "/"))
                {
                    return;
                }
            }
            
            

            // Remove extended information after path, such as for Web services 
            // or bogus /default.aspx/default.aspx
            string pathInfo = app.Request.PathInfo;
            if (pathInfo != string.Empty)
            {
                requestPath = requestPath.Substring(0, requestPath.Length - pathInfo.Length);
            }

            // 2006-01-25 : David Neal : Updated URL checking, Fixes for sites where Cynthia 
            // is running at the root and for bogus default document URLs
            // Get the relative target URL without the application root
            string appRoot = WebUtils.GetApplicationRoot();

            if (requestPath.Length == appRoot.Length) { return; }

            string targetUrl = requestPath.Substring(appRoot.Length + 1);
            //if (targetUrl.Length == 0) return;
            if(StringHelper.IsCaseInsensitiveMatch(targetUrl, "default.aspx"))return;

            if (useFolderForSiteDetection)
            {
                if (targetUrl.StartsWith(virtualFolderName + "/"))
                {
                    targetUrl = targetUrl.Remove(0, virtualFolderName.Length + 1);
                }
            }


            if (!WebConfigSettings.Disable301Redirector)
            {
                try
                {
                    // check if the requested url is supposed to redirect
                    string redirectUrl = GetRedirectUrl(targetUrl);
                    if (redirectUrl.Length > 0)
                    {
                        Do301Redirect(app, redirectUrl);
                        return;
                    }
                }
                catch (NullReferenceException ex)
                {
                    // this can happen on a new installation so we catch and log it
                    log.Error(ex);
                }
            }

            
            FriendlyUrl friendlyUrl = null;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            //this will happen on a new installation
            if (siteSettings == null) { return; }
            
            if (
            (useFolderForSiteDetection)
            && (virtualFolderName.Length > 0)
            )
            {
                
                //int siteID = SiteSettings.GetSiteIDFromFolderName(virtualFolderName);
                friendlyUrl = new FriendlyUrl(siteSettings.SiteId, targetUrl);
            }
            else
            {
                if (siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageName)
                {
                    //when using extensionless urls we consistently store them without a trailing slash
                    if (targetUrl.EndsWith("/"))
                    {

                        targetUrl = targetUrl.Substring(0, targetUrl.Length - 1);
                        setClientFilePath = false;
                    }
                }

                if (WebConfigSettings.AlwaysUrlEncode)
                {
                    friendlyUrl = new FriendlyUrl(WebUtils.GetHostName(), HttpUtility.UrlEncode(targetUrl));

                    //in case existing pages are not url encoded since this setting was added 2009-11-15, try again without encoding
                    
                    if (!friendlyUrl.FoundFriendlyUrl) 
                    {
                        if (WebConfigSettings.RetryUnencodedOnUrlNotFound)
                        {
                            friendlyUrl = new FriendlyUrl(WebUtils.GetHostName(), targetUrl);
                        }
                    }
                    
                }
                else
                {
                    friendlyUrl = new FriendlyUrl(WebUtils.GetHostName(), targetUrl);
                }
            }

            if (
                (friendlyUrl == null)
                ||(!friendlyUrl.FoundFriendlyUrl)
                )
            {
                if (
                (useFolderForSiteDetection)
                && (virtualFolderName.Length > 0)
                )
                {
                    SiteUtils.TrackUrlRewrite();

                    //2009-03-01 same bug as above
                    //string pathToUse = requestPath.Replace(virtualFolderName + "/", string.Empty);
                    string pathToUse = requestPath.Remove(0, virtualFolderName.Length + 1);
                    
                    app.Context.RewritePath(
                        pathToUse,
                        string.Empty,
                        app.Request.QueryString.ToString(),
                        setClientFilePath);

                }
                else
                {
                    return;
                }


            }

            string queryStringToUse = string.Empty;
            string realPageName = string.Empty;

            
            if (friendlyUrl.RealUrl.IndexOf('?') > 0)
            {
                realPageName = friendlyUrl.RealUrl.Substring(0, friendlyUrl.RealUrl.IndexOf('?'));
                queryStringToUse = friendlyUrl.RealUrl.Substring(friendlyUrl.RealUrl.IndexOf('?') + 1);
            }
            else // Added by Christian Fredh 10/30/2006
            {
                realPageName = friendlyUrl.RealUrl;
            }

            if (log.IsDebugEnabled)
            {
                log.Debug("Rewriting URL to " + friendlyUrl.RealUrl);
            }


            if ((realPageName != null) && (!String.IsNullOrEmpty(realPageName)))
            {
                if (queryStringToUse == null)
                {
                    queryStringToUse = String.Empty;
                }

                StringBuilder originalQueryString = new StringBuilder();

                // get any additional params besides pageid
                string separator = string.Empty;
                foreach (string key in app.Request.QueryString.AllKeys)
                {
                    if (key != "pageid")
                    {
                        originalQueryString.Append( separator + key + "="
                            + app.Request.QueryString.Get(key));
                        
                        if(separator.Length == 0)separator = "&";

                    }
                }

                if (originalQueryString.Length > 0)
                {
                    if (queryStringToUse.Length == 0)
                    {
                        queryStringToUse = originalQueryString.ToString();
                    }
                    else
                    {
                        queryStringToUse += "&" + originalQueryString.ToString();
                    }
                }

                SiteUtils.TrackUrlRewrite();
                //log.Info("re-writing to " + realPageName);
                app.Context.RewritePath(realPageName, string.Empty, queryStringToUse, setClientFilePath);
            }

            
        }

        /// <summary>
        /// note the expected targetUrl and returned url are not fully qualified, but relative without a /
        /// </summary>
        /// <param name="targetUrl"></param>
        /// <returns></returns>
        private static string GetRedirectUrl(string targetUrl)
        {
            //lookup if this url is to be redirected, if found return the new url
            string newUrl = string.Empty;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            using (IDataReader reader = RedirectInfo.GetBySiteAndUrl(siteSettings.SiteId, targetUrl))
            {
                if (reader.Read())
                {
                    newUrl = reader["NewUrl"].ToString();
                }
            }

            return newUrl;
        }

        private static void Do301Redirect(HttpApplication app,  string newUrl)
        {

            string siteRoot = SiteUtils.GetNavigationSiteRoot();

            app.Context.Response.Status = "301 Moved Permanently";
			app.Context.Response.AddHeader("Location", String.Format("{0}/{1}", siteRoot, newUrl));

        }



       
		
	}
}
