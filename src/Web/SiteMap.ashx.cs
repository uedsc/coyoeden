/// Author:					Joe Audette
/// Created:				2007-07-30
/// Last Modified:			2009-07-23
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
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;


namespace Cynthia.Web.Services
{
    /// <summary>
    /// Purpose: Renders the ASP.NET SiteMap as xml 
    /// in google site map protocol format
    /// https://www.google.com/webmasters/tools/docs/en/protocol.html
    /// 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SiteMap : IHttpHandler
    {
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            GenerateSiteMap(context);
        }

        private void GenerateSiteMap(HttpContext context)
        {
            //secureSiteRoot = SiteUtils.GetSecureNavigationSiteRoot();
            //insecureSiteRoot = SiteUtils.GetNavigationSiteRoot();
            insecureSiteRoot = WebUtils.GetSiteRoot();
            secureSiteRoot = insecureSiteRoot.Replace("http://", "https://");

            //secureSiteRoot = WebUtils.GetSecureSiteRoot();
            //insecureSiteRoot = secureSiteRoot.Replace("https", "http");

            Page page = new Page();
            page.AppRelativeVirtualPath = context.Request.AppRelativeCurrentExecutionFilePath;

            context.Response.Expires = -1;
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

                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                //string siteRoot;
                //if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                //{
                //    siteRoot = WebUtils.GetSiteRoot();
                //}
                //else
                //{
                //    siteRoot = SiteUtils.GetNavigationSiteRoot();
                //}

                SiteMapDataSource siteMapDataSource = new SiteMapDataSource();

                siteMapDataSource.SiteMapProvider
                        = "Csite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

                SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
                ArrayList alreadyAddedUrls = new ArrayList();

                RenderNodesToSiteMap(
                    context,
                    page,
                    xmlTextWriter,
                    alreadyAddedUrls,
                    siteMapNode);


                xmlTextWriter.WriteEndElement(); //urlset

                //end of document
                xmlTextWriter.WriteEndDocument();
                xmlTextWriter.Close();
            }


        }

        private void RenderNodesToSiteMap(
            HttpContext context,
            Page page,
            XmlTextWriter xmlTextWriter, 
            ArrayList alreadyAddedUrls,
            SiteMapNode siteMapNode)
        {
            CSiteMapNode CNode = (CSiteMapNode)siteMapNode;
            if (!CNode.IsRootNode)
            {
                if (
                    ((CNode.Roles == null)||(WebUser.IsInRoles(CNode.Roles)))
                    &&(CNode.IncludeInSearchMap)
                    &&(!CNode.IsPending)
                    )
                {
                    // must use unique urls, google site maps can't have
                    // multiple urls like"
                    // http://SomeSite/Default.aspx?pageid=1
                    // http://SomeSite/Default.aspx?pageid=2
                    // where it only differs by query string
                    // google may stop crawling if it encounters this
                    // in a site map


                    if (IsValidUrl(CNode))
                    {
                        
                        string url;
                        if (CNode.Url.StartsWith("http"))
                        {
                            url = CNode.Url;
                        }
                        else
                        {
                            if (CNode.UseSsl)
                            {
                                url = secureSiteRoot + CNode.Url.Replace("~/", "/");
                            }
                            else
                            {
                                url = insecureSiteRoot + CNode.Url.Replace("~/", "/");
                            }
                           
                        }

                        // no duplicate urls allowed in a google site map
                        if (!alreadyAddedUrls.Contains(url))
                        {
                            alreadyAddedUrls.Add(url);

                            xmlTextWriter.WriteStartElement("url");
                            xmlTextWriter.WriteElementString("loc", url);

                            // this if is only needed because this is a new datapoint
                            // after it has been implemented in the db
                            // this if could be removed
                            if (CNode.LastModifiedUtc > DateTime.MinValue)
                            {
                                xmlTextWriter.WriteElementString(
                                    "lastmod",
                                    CNode.LastModifiedUtc.ToString("u",CultureInfo.InvariantCulture).Replace(" ", "T"));
                            }
                            
                            xmlTextWriter.WriteElementString(
                                "changefreq", 
                                CNode.ChangeFrequency.ToString().ToLower());
                            
                            xmlTextWriter.WriteElementString(
                                "priority", 
                                CNode.SiteMapPriority);

                            xmlTextWriter.WriteEndElement(); //url

                        }
                    }

                }

            }

            foreach (SiteMapNode childNode in CNode.ChildNodes)
            {
                RenderNodesToSiteMap(
                    context, 
                    page,  
                    xmlTextWriter, 
                    alreadyAddedUrls,
                    childNode);
            }

        }

        private bool IsValidUrl(CSiteMapNode CNode)
        {
            bool result = true;
            if (CNode.Url.ToLower().Contains("default.aspx?pageid="))
            {
                result = false;
            }

            if (CNode.Url.Contains("#"))
            {
                result = false;
            }


            return result;

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
