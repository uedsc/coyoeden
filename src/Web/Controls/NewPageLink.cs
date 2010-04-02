//	Created:			    2010-01-04
//	Last Modified:		    2010-01-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// a convenience link for the new page. The link renders only for those in roles that can add pages below the current page
    /// </summary>
    public class NewPageLink : HyperLink
    {
        private CBasePage basePage = null;

        private bool ShouldRender()
        {
            if (basePage == null) { return false; }
            if (!Page.Request.IsAuthenticated) { return false; }

            if (WebUser.IsAdminOrContentAdminOrRoleAdmin) { return true; }

            if (WebConfigSettings.UseRelatedSiteMode)
            {
                if (basePage.SiteInfo == null) { return false; }
                // in related sites mode usersin site eidotrs role can use admin menu
                if (WebUser.IsInRoles(basePage.SiteInfo.SiteRootEditRoles)) { return true; }
            }

            if (basePage.CurrentPage == null) { return false; }
            if (WebUser.IsInRoles(basePage.CurrentPage.CreateChildPageRoles)) { return true; }

            return false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            basePage = Page as CBasePage;
            
            Visible = ShouldRender();
            if (!Visible) { return; }

            if (basePage == null) { return; }

            if (CssClass.Length > 0)
            {
                CssClass = "ModuleEditLink adminlink newpagelink " + CssClass;
            }
            else
            {
                CssClass = "ModuleEditLink adminlink newpagelink";
            }

            ToolTip = Resource.AddPageTooltip;
            if (
                basePage.CurrentPage != null)
            {
                NavigateUrl = basePage.SiteRoot + "/Admin/PageSettings.aspx?start=" + basePage.CurrentPage.PageId.ToInvariantString();
            }
            else
            {
                NavigateUrl = basePage.SiteRoot + "/Admin/PageSettings.aspx";
            }

            if (basePage.UseIconsForAdminLinks)
            {
                ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + ConfigurationManager.AppSettings["NewPageImage"]);
                Text = Resource.PagesAddButton;
            }
            else
            {
                Text = Resource.AddPageLink;
            }
        }


    }
}
