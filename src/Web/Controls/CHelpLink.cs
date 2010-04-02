// Author:					Joe Audette
// Created:				    2009-05-01
// Last Modified:			2009-05-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.UI
{
    public class CHelpLink : GreyBoxHyperlink
    {
        private string helpKey = string.Empty;
        
        private bool helpIsEnabled = true;
        private SiteSettings siteSettings = null;


        public String HelpKey
        {
            get { return helpKey; }
            set { helpKey = value; }
        }

        
       

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            helpIsEnabled = (!WebConfigSettings.DisableHelpSystem);
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings == null) || (siteSettings.SiteGuid == Guid.Empty))
            {
                helpIsEnabled = false;
                return;
            }

            this.NavigateUrl = siteSettings.SiteRoot + "/Help.aspx?helpkey=" + Page.Server.UrlEncode(helpKey);
            this.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/help.gif");
            this.ToolTip = Resource.HelpLink;
            this.Text = Resource.HelpLink;
            this.ClientClick = "return GB_showCenter('" + Page.Server.HtmlEncode(Resource.HelpLink) + "', this.href, 600,700)";
            this.CssClass = "mhelp";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

           
            if ((helpIsEnabled)&&(helpKey.Length > 0))
            {
                base.Render(writer);
            }
           
        }

        public static void AddHelpLink(
            Panel parentControl,
            string helpkey)
        {
            Literal litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

            CHelpLink helpLinkButton = new CHelpLink();
            helpLinkButton.HelpKey = helpkey;
            parentControl.Controls.Add(helpLinkButton);

            litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

        }

    }
}
