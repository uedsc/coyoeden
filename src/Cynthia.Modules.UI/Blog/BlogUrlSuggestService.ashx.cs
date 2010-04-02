/// Author:						Joe Audette
/// Created:					2008-05-14
/// Last Modified:				2009-05-06
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.BlogUI
{
    /// <summary>
    /// A service to suggest a friendly url for a blog post
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BlogUrlSuggestService : IHttpHandler
    {
        private Double timeOffset = 0;

        public void ProcessRequest(HttpContext context)
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            SendResponse(context);
        }

        private void SendResponse(HttpContext context)
        {
            if (context == null) return;

            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();

            XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("DATA");
            string warning = string.Empty;

            if (context.Request.Params.Get("pn") != null)
            {
                String pageName = context.Request.Params.Get("pn");
                if (WebConfigSettings.AppendDateToBlogUrls) { pageName += "-" + DateTime.UtcNow.AddHours(timeOffset).ToString("yyyy-MM-dd"); }

                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

                if (siteSettings != null)
                {
                    String friendlyUrl = SiteUtils.SuggestFriendlyUrl(pageName, siteSettings);

                    if (WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrl))
                    {
                        warning = BlogResources.BlogUrlConflictWithPhysicalPageError;
                    }

                    xmlTextWriter.WriteStartElement("fn");
                    
                    xmlTextWriter.WriteString("~/" + friendlyUrl);
                    
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement("wn");
                    xmlTextWriter.WriteString(warning);
                    xmlTextWriter.WriteEndElement();

                }

            }
            else
            {
                if (context.Request.Params.Get("cu") != null)
                {
                    String enteredUrl = context.Server.UrlDecode(context.Request.Params.Get("cu"));
                    if (WebPageInfo.IsPhysicalWebPage(enteredUrl))
                    {
                        warning = BlogResources.BlogUrlConflictWithPhysicalPageError;
                    }

                    xmlTextWriter.WriteStartElement("wn");
                    xmlTextWriter.WriteString(warning);
                    xmlTextWriter.WriteEndElement();
                }


            }

            //end of document
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();

            xmlTextWriter.Close();
            //Response.End();


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
