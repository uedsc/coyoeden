//	Created:			    2010-01-04
//	Last Modified:		    2010-01-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// a convenience link for the Administration menu. The link renders only for those in roles that can use the admin menu
    /// </summary>
    public class AdminMenuLink : HyperLink
    {
        
        private string relativeUrl = "/Admin/AdminMenu.aspx";
        private CBasePage basePage = null;

        private bool ShouldRender()
        {
            if (basePage == null) { return false; }
            if (!Page.Request.IsAuthenticated) { return false; }

            // initialize to default values
            ToolTip = Resource.AdminMenuLink;
            if (basePage.UseIconsForAdminLinks)
            {
                ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.AdminImage);
                Text = Resource.AdminMenuLink;
            }
            else
            {
                Text = Resource.AdminLink;
            }

            if (WebUser.IsAdminOrContentAdminOrRoleAdmin) { return true; }

            if (basePage.CurrentPage == null) { return false; }
            if (WebUser.IsInRoles(basePage.CurrentPage.CreateChildPageRoles))
            {
                //overide for non admins
                relativeUrl = "/Admin/PageTree.aspx";
                ToolTip = Resource.PageTreeTitle;
                if (basePage.UseIconsForAdminLinks)
                {
                    ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.PageTreeImage);
                    Text = Resource.PageTreeLink;
                }
                else
                {
                    Text = Resource.PageListLink;
                }

                return true;
            }

            if (!WebConfigSettings.UseRelatedSiteMode) { return false; }

            if (basePage.SiteInfo == null) { return false; }
            // in related sites mode usersin site eidotrs role can use admin menu
            if (WebUser.IsInRoles(basePage.SiteInfo.SiteRootEditRoles)) { return true; }

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
                CssClass = "ModuleEditLink adminlink adminmenulink " + CssClass;
            }
            else
            {
                CssClass = "ModuleEditLink adminlink adminmenulink";
            }
            //ToolTip = Resource.AdminMenuLink;
            NavigateUrl = basePage.SiteRoot + relativeUrl;
            //if (basePage.UseIconsForAdminLinks)
            //{
            //    ImageUrl = Page.ResolveUrl("~/Data/SiteImages/admin.png");
            //    Text = Resource.AdminMenuLink;
            //}
            //else
            //{
            //    Text = Resource.AdminLink;
            //}
        }

    }
}
