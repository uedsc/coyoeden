
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.UI
{
    
    public class SiteTitle : WebControl
    {
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

        private bool useLink = true;
        public bool UseLink
        {
            get { return useLink; }
            set { useLink = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write(String.Format("[{0}]", this.ID));
                return;
            }


            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return;

            string titleText = (overrideTitle.Length > 0) ? overrideTitle : siteSettings.SiteName;
            string titleMarkup;

            if (useLink)
            {
                string urlToUse;
                if (overrideTitle.Length > 0 && overrideUrl.Length > 0)
                {
                    urlToUse = (overrideUrl.StartsWith("~/") && siteSettings.SiteFolderName.Length > 0)
                                   ? SiteUtils.GetNavigationSiteRoot() + overrideUrl.Replace("~/", "/")
                                   : overrideUrl;
                }
                else
                {
                    if (siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX)
                    {
                        urlToUse = SiteUtils.GetNavigationSiteRoot() + "/Default.aspx";
                    }
                    else
                    {
                        urlToUse = SiteUtils.GetNavigationSiteRoot();
                    }
                }
                titleMarkup = String.Format(CultureInfo.InvariantCulture, "<a class='siteheading' href='{0}'>{1}</a>",
                                            Page.ResolveUrl(urlToUse), titleText);
            }
            else
            {
                titleMarkup = titleText;
            }

            writer.Write("<h1 class='art-Logo-name art-logo-name siteheading'>{0}</h1>", titleMarkup);
           
        }
    }
}
