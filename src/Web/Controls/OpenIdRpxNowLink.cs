// Author:		        Joe Audette
// Created:             2009-05-14
// Last Modified:       2010-03-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// a wrapper control for the rpx widget
    /// </summary>
    public class OpenIdRpxNowLink : HyperLink
    {
        private SiteSettings siteSettings = null;
        private string realm = string.Empty;
        private string tokenUrl = string.Empty;
        private bool embed = true;
        private bool useOverlay = false;
        private string siteRoot = string.Empty;
        private string overrideText = string.Empty;

        public string OverrideText
        {
            get { return overrideText; }
            set { overrideText = value; }
        }

        public bool Embed
        {
            get { return embed; }
            set { embed = value; }
        }

        public bool UseOverlay
        {
            get { return useOverlay; }
            set { useOverlay = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Visible) { return; }

            base.OnLoad(e);

            LoadSettings();
            SetupScript();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(realm)) { return; }
            if(string.IsNullOrEmpty(tokenUrl)){ return; }

            if (embed)
            {
                writer.Write("<iframe src=\"");
                writer.Write("https://" + realm + "/openid/embed?token_url=" + Page.Server.UrlEncode(tokenUrl)
                    + "&amp;language_preference=" + GetLanguageCode() + "\"");
                writer.Write(" scrolling=\"no\" frameBorder=\"no\" style=\"width:400px;height:240px;\"></iframe>");
                // frameborder is not valid html 5
                //writer.Write(" scrolling=\"no\" style=\"width:400px;height:240px;\"></iframe>");

            }
            else
            {
                base.Render(writer);
            }

            
        }

        private void SetupScript()
        {
            if (siteSettings == null) { return; }
            if (string.IsNullOrEmpty(realm)) { return; }
            if (string.IsNullOrEmpty(tokenUrl)) { return; }

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                    "rpxmain", "\n<script  src=\"https://rpxnow.com/openid/v2/widget\" type=\"text/javascript\" ></script>");

            string overlay = "false";
            if (useOverlay) { overlay = "true"; }

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                    "rpxsetup", "\n<script  type=\"text/javascript\">"
                    + " RPXNOW.token_url = \"" + tokenUrl 
                    + "\"; RPXNOW.realm = \"" + realm 
                    + "\"; RPXNOW.overlay = " + overlay + "; RPXNOW.language_preference = '"
                    + GetLanguageCode() + "'; </script>");

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            if (siteSettings.SiteRoot.Length > 0)
            {
                siteRoot = siteSettings.SiteRoot;
            }
            else
            {
                siteRoot = SiteUtils.GetNavigationSiteRoot();
            }

            tokenUrl = siteRoot + "/Secure/OpenIdRpxHandler.aspx";

            realm = siteSettings.RpxNowApplicationName;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
                {
                    realm = WebConfigSettings.OpenIdRpxApplicationName;
                }

            }

            this.Text = Resource.OpenIDLoginButton;

            if (overrideText.Length > 0) { this.Text = overrideText; }

            this.CssClass = "rpxnow openid_login";
            this.Attributes.Add("onclick", "return false;");
            this.NavigateUrl = "https://" + realm + ".rpxnow.com/openid/v2/signin?token_url=" + Page.Server.UrlEncode(tokenUrl);

        }

        private string GetLanguageCode()
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                    // these are the supported languages as of 2009-05-15

                case "ro":
                    return "ro";

                case "hu":
                    return "hu";

                case "it":
                    return "it";

                case "sv":
                    return "sv-SE";

                case "ja":
                    return "ja";

                case "bg":
                    return "bg";

                case "pl":
                    return "pl";

                case "ru":
                    return "ru";

                case "de":
                    return "de";

                case "pt":
                    return "pt-BR";

                case "vi":
                    return "vi";

                case "cs":
                    return "cs";

                case "zh":
                    return "zh";

                case "fr":
                    return "fr";

                case "sr":
                    return "sr";

                case "nl":
                    return "nl";

                case "ko":
                    return "ko";

                case "el":
                    return "el";

                case "es":
                    return "es";

                case "da":
                    return "da";

              
            }

            return "en";
        }


    }
}
