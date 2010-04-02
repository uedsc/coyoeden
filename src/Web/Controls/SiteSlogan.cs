//	Created:			    2010-03-19
//	Last Modified:		    2010-03-19
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
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.UI
{
    public class SiteSlogan : Literal
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            if (siteSettings.Slogan.Length == 0) { this.Visible = false; return; }
            this.Text = Page.Server.HtmlEncode(siteSettings.Slogan);

        }

    }
}
