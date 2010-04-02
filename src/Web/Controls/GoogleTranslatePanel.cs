// Author:					Joe Audette
// Created:				    2009-12-19
// Last Modified:			2009-12-19
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
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// This adds a div with javascript to enable google tranlsation of the page.
    /// There should only be one instance of this control on a given page.
    /// You should be careful about using this feature and not use it on pages containing sensitive data that you do not want to be passed to google tranlsate.
    /// </summary>
    public class GoogleTranslatePanel : Panel
    {
        private string languageCode = string.Empty;

        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }

        private bool allowSecurePageTranslation = false;

        /// <summary>
        /// This is false by default so that the widget will not be displayed on pages protected with SSL
        /// However according to the documentation translating secure pages is supported, the content will be sent to google 
        /// for tranlsation over an ssl connection ad will not be logged.
        /// </summary>
        public bool AllowSecurePageTranslation
        {
            get { return allowSecurePageTranslation; }
            set { allowSecurePageTranslation = value; }
        }

        

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (WebConfigSettings.DisableGoogleTranslate) { Visible = false; }

            // don't pass secure content to the translater
            if ((!allowSecurePageTranslation) &&(Page.Request.IsSecureConnection)) { Visible = false; }

            if (!Visible) { return; }

            SetupMainScript();
            SetupInstanceScript();

        }

        private void SetupInstanceScript()
        {
            if (languageCode.Length == 0)
            {
                CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();
                languageCode = defaultCulture.TwoLetterISOLanguageName;
            }

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">\n");

            script.Append("function googleTranslateElementInit() {");
            script.Append("new google.translate.TranslateElement({");
            script.Append("pageLanguage: '" + languageCode + "'},");
            script.Append("'" + ClientID + "');}");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                this.UniqueID,
                script.ToString());

        }

        private void SetupMainScript()
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page),
                    "gtranslate", "\n<script  src=\"http://translate.google.com/translate_a/element.js?cb=googleTranslateElementInit\" type=\"text/javascript\" ></script>");

        }

        

    }
}
