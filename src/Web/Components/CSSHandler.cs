//		Author:			Joe Audette
//		Created:		2008-10-31
//		Last Modified:	2010-03-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO.Compression;
using Cynthia.Web.Framework;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using System.Xml;


namespace Cynthia.Web.UI
{
    /// <summary>
    /// the handler will combine the css files in the order they are listed in
    /// the css.config file located in the skin folder
    /// It will also remove white space (and comments?)
    /// This can improve performance as measured using the YSlow Firefox plugin
    /// </summary>
    public class CssHandler : IHttpHandler
    {
        private const string POST = "POST";
        private const bool DO_GZIP = true;
        private readonly static TimeSpan CACHE_DURATION = TimeSpan.FromDays(WebConfigSettings.CssCacheInDays);
        private readonly static Regex URL_REGEX =
            new Regex(@"url\((\""|\')?(?<path>(.*))?(\""|\')?\)", RegexOptions.Compiled);

        //private readonly static Regex URL_REGEX =
        //    new Regex(@"url\((\""|\'|)?(?<path>(.*))?(\""|\')?\)", RegexOptions.Compiled);

        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/css";

            if (context.Request.RequestType == POST) { return; }

           
            bool isCompressed = DO_GZIP && this.CanGZip(context.Request);
            //bool isCompressed = false;

            int siteId = SiteUtils.ParseSiteIdFromSkinRequestUrl();

            string skinName = "styleshout-refresh";
            if (context.Request["skin"] != null)
            {
                skinName = SiteUtils.SanitizeSkinParam(context.Request["skin"]);
            }
            string media = "screen";

            if (context.Request["media"] != null)
            {
                media = context.Request["media"];
            }

            UTF8Encoding encoding = new UTF8Encoding(false);

            if (!this.WriteFromCache(context, siteId, skinName, isCompressed))
            {
                byte[] cssBytes = GetCss(context, siteId, skinName, encoding);

                using (MemoryStream memoryStream = new MemoryStream(5000))
                {
                    using (Stream writer = isCompressed ?
                        (Stream)(new GZipStream(memoryStream, CompressionMode.Compress)) :
                        memoryStream)
                    {
                        writer.Write(cssBytes, 0, cssBytes.Length);

                        writer.Close();
                    }

                    byte[] responseBytes = memoryStream.ToArray();

                    if (WebConfigSettings.CacheCssOnServer)
                    {
                        // TODO: maybe we should cache it to disk instead of memory
                        context.Cache.Insert(
                        GetCacheKey(siteId, skinName, isCompressed, context.Request.IsSecureConnection),
                        responseBytes,
                        null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration,
                        CACHE_DURATION);
                    }

                    this.WriteBytes(responseBytes, context, isCompressed);

                }

                

            }
        }

        private void WriteBytes(byte[] bytes, HttpContext context, bool isCompressed)
        {
            if (bytes.Length == 0) { return; }

            HttpResponse response = context.Response;

            response.AppendHeader("Content-Length", bytes.Length.ToInvariantString());
            response.ContentType = "text/css";
            if (isCompressed)
                response.AppendHeader("Content-Encoding", "gzip");

            bool isIE6 = BrowserHelper.IsIE6();

            if (WebConfigSettings.CacheCssInBrowser)
            {
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetExpires(DateTime.Now.Add(CACHE_DURATION));
                context.Response.Cache.SetMaxAge(CACHE_DURATION);
                if (!isIE6)
                {
                    context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
                }
            }
            else
            {
                if ((!WebConfigSettings.CacheCssOnServer) && ((!isIE6)))
                {
                    //if both cache settings are off it must be a designer so make it easy
                    context.Response.Cache.SetExpires(new DateTime(1995, 5, 6, 12, 0, 0, DateTimeKind.Utc));
                    context.Response.Cache.SetNoStore();
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                    context.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");
   
                }
            }

            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.Flush();
        }

        private byte[] GetCss(HttpContext context, int siteId, string skinName, Encoding encoding)
        {
            
            string basePath = HttpContext.Current.Server.MapPath(
                "~/Data/Sites/" + siteId.ToInvariantString()
                + "/skins/" + skinName + "/");

            string siteRoot = WebUtils.GetSiteRoot();

            string skinImageBasePath = siteRoot + "/Data/Sites/" + siteId.ToInvariantString()
                + "/skins/" + skinName + "/";

            StringBuilder cssContent = new StringBuilder();

            if (File.Exists(basePath + "style.config"))
            {
                using (XmlReader reader = new XmlTextReader(new StreamReader(basePath + "style.config")))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (("file" == reader.Name)&&(reader.NodeType != XmlNodeType.EndElement))
                        {
                            // config based css for things like YUI where the folder changes per version
                            string csswebconfigkey = reader.GetAttribute("csswebconfigkey");
                            string imagebasewebconfigkey = reader.GetAttribute("imagebasewebconfigkey");

                            if ((!string.IsNullOrEmpty(csswebconfigkey))&&(csswebconfigkey.Contains("YUI"))) { csswebconfigkey = string.Empty; }

                            if((WebConfigSettings.UseGoogleCDN)&&(!string.IsNullOrEmpty(csswebconfigkey)))
                            {
                                if(csswebconfigkey.Contains("YUI")){ csswebconfigkey = string.Empty;}
                            }

                            // full virtual path option for things that don't move like oomph
                            string cssVPath = reader.GetAttribute("cssvpath");
                            string imageBaseVPath = reader.GetAttribute("imagebasevpath");

                            if ((!string.IsNullOrEmpty(csswebconfigkey)) && (!string.IsNullOrEmpty(imagebasewebconfigkey)))
                            {
                                if (
                                    (ConfigurationManager.AppSettings[csswebconfigkey] != null)
                                    && (ConfigurationManager.AppSettings[imagebasewebconfigkey] != null)
                                 )
                                {

                                    string cssFullPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[csswebconfigkey]);
                                    string imageBasePath = ConfigurationManager.AppSettings[imagebasewebconfigkey];
                                    if (File.Exists(cssFullPath))
                                    {
                                        FileInfo file = new FileInfo(cssFullPath);
                                        using (StreamReader sr = file.OpenText())
                                        {
                                            string fileContent = sr.ReadToEnd();
                                            sr.Close();

                                            string css = URL_REGEX.Replace(fileContent,
                                                new MatchEvaluator(delegate(Match m)
                                                {
                                                    string imgPath = m.Groups["path"].Value.TrimStart('\'').TrimEnd('\'').TrimStart('"').TrimEnd('"');

                                                    if (!imgPath.StartsWith("http://"))
                                                    {
                                                        return "url('" + siteRoot + imageBasePath // First put the prefix for the images
                                                            + imgPath // the relative URL as it is in the CSS
                                                            + "')";
                                                    }
                                                    else
                                                    {
                                                        return "url('" + imgPath + "')";
                                                    }
                                                }));

                                            cssContent.Append(css);

                                        }

                                    }


                                }


                            }
                            else if ((!string.IsNullOrEmpty(cssVPath)) && (!string.IsNullOrEmpty(imageBaseVPath)))
                            {
                                string cssFilePath;
                                if (cssVPath.StartsWith("/"))
                                {
                                    cssFilePath = HttpContext.Current.Server.MapPath("~" + cssVPath);
                                }
                                else
                                {
                                    cssFilePath = HttpContext.Current.Server.MapPath(cssVPath);
                                }
                                if (File.Exists(cssFilePath))
                                {
                                    FileInfo file = new FileInfo(cssFilePath);
                                    using (StreamReader sr = file.OpenText())
                                    {
                                        string fileContent = sr.ReadToEnd();
                                        sr.Close();

                                        string css = URL_REGEX.Replace(fileContent,
                                            new MatchEvaluator(delegate(Match m)
                                            {
                                                string imgPath = m.Groups["path"].Value.TrimStart('\'').TrimEnd('\'').TrimStart('"').TrimEnd('"');

                                                if (!imgPath.StartsWith("http://"))
                                                {
                                                    return "url('" + siteRoot + imageBaseVPath // First put the prefix for the images
                                                        + imgPath // the relative URL as it is in the CSS
                                                        + "')";
                                                }
                                                else
                                                {
                                                    return "url('" + imgPath + "')";
                                                }
                                            }));

                                        cssContent.Append(css);

                                    }

                                }

                            }
                            else
                            {

                                string cssFile = reader.ReadElementContentAsString();

                                if (File.Exists(basePath + cssFile))
                                {
                                    FileInfo file = new FileInfo(basePath + cssFile);
                                    using (StreamReader sr = file.OpenText())
                                    {
                                        string fileContent = sr.ReadToEnd();
                                        sr.Close();

                                        string css = URL_REGEX.Replace(fileContent,
                                            new MatchEvaluator(delegate(Match m)
                                            {
                                                string imgPath = m.Groups["path"].Value.TrimStart('\'').TrimEnd('\'').TrimStart('"').TrimEnd('"');

                                                if (!imgPath.StartsWith("http://"))
                                                {
                                                    return "url('" + skinImageBasePath // First put the prefix for the images
                                                        + imgPath // the relative URL as it is in the CSS
                                                        + "')";
                                                }
                                                else
                                                {
                                                    return "url('" + imgPath + "')";
                                                }
                                            }));

                                        cssContent.Append(css);
                                        //cssContent.Append(fileContent);

                                    }

                                }

                            }
                          
                        }
                    }
                }
                

            }




            if ((WebConfigSettings.CacheCssOnServer) && (WebConfigSettings.MinifyCSS))
            {
                // this method is expensive (7.87 seconds as measured by ANTS Profiler
                // we do cache so its not called very often
                return encoding.GetBytes(CssMinify.Minify(cssContent.ToString()));
            }

            return encoding.GetBytes(cssContent.ToString());
        }

        private bool WriteFromCache(HttpContext context, int siteId, string skinName, bool isCompressed)
        {
            if (!WebConfigSettings.CacheCssOnServer) { return false; }

            byte[] responseBytes = context.Cache[GetCacheKey(siteId, skinName, isCompressed, context.Request.IsSecureConnection)] as byte[];

            if (null == responseBytes) { return false; }
            if (responseBytes.Length == 0) { return false; }

            this.WriteBytes(responseBytes, context, isCompressed);
            return true;

            
        }

        

        private bool CanGZip(HttpRequest request)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding) &&
                 (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
                return true;
            return false;
        }

        private string GetCacheKey(int siteId, string skinName, bool isCompressed, bool isSecure)
        {
            return "CssHandler." + siteId.ToString(CultureInfo.InvariantCulture) + skinName + "." + isCompressed + "." + isSecure ;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
