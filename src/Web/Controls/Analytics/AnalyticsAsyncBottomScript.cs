//	Author:				Joe Audette
//	Created:			2010-03-15
//	Last Modified:		2010-03-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Controls.google;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// This control is used in conjunction with AnalyticsAsyncTopScript and in replacement for CGoogleAnalyticsScript
    /// if you want to use the async loading approach described here:
    /// http://code.google.com/apis/analytics/docs/tracking/asyncUsageGuide.html#SplitSnippet
    /// 
    /// Remove CGoogleAnalyticsScript from your layout.master and replace with AnalyticsAsyncBottomScript just before the closing form element
    /// Add AnalyticsAsyncTopScript to layout.master just below the opening body element
    /// </summary>
    public class AnalyticsAsyncBottomScript : WebControl
    {
        private SiteSettings siteSettings = null;
        private string googleAnalyticsProfileId = string.Empty;
        private string overrideScriptUrl = string.Empty;


        /// <summary>
        /// if you want to host the script locally put the path for the src=
        /// </summary>
        public string OverrideScriptUrl
        {
            get { return overrideScriptUrl; }
            set { overrideScriptUrl = value; }
        }

        private void SetupMainScript(HtmlTextWriter writer)
        {
            writer.Write("\n<script type=\"text/javascript\"> ");
            writer.Write("\n");
            writer.Write("(function() {");
            writer.Write("\n");
            writer.Write("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true; ");
            writer.Write("\n");

            if (overrideScriptUrl.Length > 0)
            {
                writer.Write("ga.src = '" + overrideScriptUrl + "';");
            }
            else
            {
                writer.Write("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
            }

            writer.Write("\n");
            writer.Write("(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(ga);");

            writer.Write("\n");
            writer.Write("})();");

            writer.Write("\n");
            writer.Write("</script>");

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (HttpContext.Current == null) { return; }

            this.EnableViewState = false;

            if (WebConfigSettings.GoogleAnalyticsScriptOverrideUrl.Length > 0)
            {
                overrideScriptUrl = WebConfigSettings.GoogleAnalyticsScriptOverrideUrl;
            }

            // let Web.config setting trump site settings. this meets my needs where I want to track the demo site but am letting people login as admin
            // this way if the remove or change it in site settings it still uses my profile id
            if (ConfigurationSettings.AppSettings["GoogleAnalyticsProfileId"] != null)
            {
                googleAnalyticsProfileId = ConfigurationSettings.AppSettings["GoogleAnalyticsProfileId"].ToString();
                return;

            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null) && (siteSettings.GoogleAnalyticsAccountCode.Length > 0))
            {
                googleAnalyticsProfileId = siteSettings.GoogleAnalyticsAccountCode;

            }
            
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (googleAnalyticsProfileId.Length == 0) { return; }

                SetupMainScript(writer);
            }

        }

    }
}
