/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// Author:				    Joe Audette
/// Created:			    2004-08-26
///	Last Modified:		    2007-07-08
/// 
/// 2007/04/13   Alexander Yushchenko: code refactoring, made it WebControl instead of UserControl.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.UI
{
    public class SiteLogo : WebControl
    {
        private bool useH1 = false;

        public bool UseH1
        {
            get { return useH1; }
            set { useH1 = value; }
        }

        private string overrideUrl = string.Empty;
        public string OverrideUrl
        {
            get { return overrideUrl; }
            set { overrideUrl = value; }
        }

        private string overrideTitle = string.Empty;
        public string OverrideTitle
        {
            get { return overrideTitle; }
            set { overrideTitle = value; }
        }

        private string overrideImageUrl = string.Empty;
        public string OverrideImageUrl
        {
            get { return overrideImageUrl; }
            set { overrideImageUrl = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            
            if (HttpContext.Current == null)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings == null || String.IsNullOrEmpty(siteSettings.Logo)) return;

                string urlToUse = "~/";
                string titleToUse = siteSettings.SiteName;
                string imageUrlToUse = Page.ResolveUrl("~/Data/Sites/")
                    + siteSettings.SiteId.ToString()
                    + "/logos/" + siteSettings.Logo;

                if (siteSettings.SiteFolderName.Length > 0)
                {
                    urlToUse = siteSettings.SiteRoot + "/Default.aspx";
                }

                if (useH1)
                {
                    writer.Write("<h1 class='sitelogo'>");
                }

                if (overrideUrl.Length > 0)
                {
                    if (siteSettings.SiteFolderName.Length > 0)
                    {
                        overrideUrl = SiteUtils.GetNavigationSiteRoot()
                            + overrideUrl.Replace("~/", "/");
                    }
                    urlToUse = overrideUrl;
                }

                if (overrideImageUrl.Length > 0)
                {
                    imageUrlToUse = Page.ResolveUrl(overrideImageUrl);
                }

                if (overrideTitle.Length > 0) titleToUse = overrideTitle;

                writer.Write("<a href='{0}' title='{1}'><img class='sitelogo' alt='{1}' src='{2}' /></a>",
                    Page.ResolveUrl(urlToUse),
                    titleToUse,
                    imageUrlToUse);

                if (useH1)
                {
                    writer.Write("</h1>");
                }
            }

        }
    }
}
