/// Author:					Joe Audette
/// Created:				2008-05-15
/// Last Modified:			2009-05-02
///		
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web
{
    /// <summary>
    /// Purpose: Renders a SiteMap as xml 
    /// in google site map protocol format
    /// https://www.google.com/webmasters/tools/docs/en/protocol.html
    /// for blog posts that have friendly urls
    /// 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BlogSiteMap : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            GenerateSiteMap(context);
        }

        private void GenerateSiteMap(HttpContext context)
        {
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(20));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);

            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;

                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("urlset");
                xmlTextWriter.WriteStartAttribute("xmlns");
                xmlTextWriter.WriteValue("http://www.sitemaps.org/schemas/sitemap/0.9");
                xmlTextWriter.WriteEndAttribute();

                // add blog post urls
                if (WebConfigSettings.EnableBlogSiteMap)
                    AddBlogUrls(context, xmlTextWriter);


                xmlTextWriter.WriteEndElement(); //urlset

                //end of document
                xmlTextWriter.WriteEndDocument();
                xmlTextWriter.Close();
            }



        }

        private void AddBlogUrls(HttpContext context, XmlTextWriter xmlTextWriter)
        {
            
            string baseUrl = SiteUtils.GetNavigationSiteRoot();
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            if (siteSettings.SiteGuid == Guid.Empty) { return; }

            using(IDataReader reader = Blog.GetBlogsForSiteMap(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    xmlTextWriter.WriteStartElement("url");
                    xmlTextWriter.WriteElementString("loc", baseUrl + reader["ItemUrl"].ToString().Replace("~", string.Empty));
                    xmlTextWriter.WriteElementString(
                            "lastmod",
                            Convert.ToDateTime(reader["LastModUtc"]).ToString("u", CultureInfo.InvariantCulture).Replace(" ", "T"));

                    // maybe should use never for blog posts but in case it changes we do want to be re-indexed
                    xmlTextWriter.WriteElementString("changefreq", "monthly");

                    xmlTextWriter.WriteElementString("priority", "0.5");

                    xmlTextWriter.WriteEndElement(); //url
                }


            }


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
