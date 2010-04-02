///	Created:			    2005-03-24
///	Last Modified:		    2009-10-18
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
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{
    
    public class RegisterLink : WebControl
    {
        // these separator properties are deprecated
        // it is recommended not to use these properties
        // but instead to use Cynthia.Web.Controls.SeparatorControl
        private bool useLeftSeparator = false;
        public bool UseLeftSeparator
        {
            get { return useLeftSeparator; }
            set { useLeftSeparator = value; }
        }

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = "topnavitem";
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (CssClass.Length == 0) CssClass = "sitelink";
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings == null) || (!siteSettings.AllowNewRegistration) || (siteSettings.UseLdapAuth)) { return; }
            if (Page.Request.IsAuthenticated) { return; }

            
            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string urlToUse = siteRoot + "/Secure/Register.aspx";

            if (siteSettings.DisableDbAuth)
            {
                if ((!siteSettings.AllowOpenIdAuth) && (!siteSettings.AllowWindowsLiveAuth)) { return; } //everything is disabled so render nothing

                if (!siteSettings.AllowOpenIdAuth)
                {
                    urlToUse = siteRoot + "/Secure/RegisterWithWindowsLiveID.aspx";
                }

                if ((!siteSettings.AllowWindowsLiveAuth)&&(siteSettings.RpxNowApiKey.Length == 0))
                {
                    urlToUse = siteRoot + "/Secure/RegisterWithOpenID.aspx";
                }
            }

            if (UseLeftSeparator) writer.Write("<span class='accent'>|</span> ");

            if (renderAsListItem)
            {
                //writer.Write("<li class='" + listItemCSS + "'>");
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }

            

            if(SiteUtils.SslIsAvailable())
            {
                urlToUse = urlToUse.Replace("http:","https:");
            }

            string returnUrlParam = Page.Request.Params.Get("returnurl");
            if (!String.IsNullOrEmpty(returnUrlParam))
            {
                urlToUse += "?returnurl=" + SecurityHelper.RemoveAngleBrackets(returnUrlParam);
            }

            //writer.Write(string.Format(
            //                 " <a href='{0}' title='{1}' class='"
            //                 + CssClass + "'>{1}</a>",
            //                 Page.ResolveUrl(urlToUse),
            //                 Resource.RegisterLink));

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);
            //writer.WriteAttribute("title", Resource.RegisterLink);
            writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEncodedText(Resource.RegisterLink);
            writer.WriteEndTag("a");



            if (renderAsListItem) writer.WriteEndTag("li");
            
            
            
        }

    }
}
