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
    /// a convenience link that pnly renders if the user can edit the current cms page
    /// </summary>
    public class PageEditFeaturesLink : HyperLink
    {
        private CmsPage basePage = null;
        
        private bool ShouldRender()
        {
            if (basePage == null) { return false; }
            if (!Page.Request.IsAuthenticated) { return false; }
            if (basePage.CurrentPage == null) { return false; }
            if (basePage.CurrentPage.PageId == -1) { return false; }

            if (WebUser.IsAdminOrContentAdmin) { return true; }

            if(
                (WebUser.IsInRoles(basePage.CurrentPage.EditRoles))
                ||(basePage.CurrentPage.IsPending && WebUser.IsInRoles(basePage.CurrentPage.DraftEditOnlyRoles))
                )
            {
                return true;
            }

            return false;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            basePage = Page as CmsPage;

            Visible = ShouldRender();
            if (!Visible) { return; }
            
            if (basePage == null) { return; }

            if (CssClass.Length > 0)
            {
                CssClass = "ModuleEditLink adminlink pageeditlink " + CssClass;
            }
            else
            {
                CssClass = "ModuleEditLink adminlink pageeditlink";
            }
            ToolTip = Resource.PageLayoutTooltip;
            if (basePage.CurrentPage != null)
            {
                NavigateUrl = basePage.SiteRoot + "/Admin/PageLayout.aspx?pageid=" + basePage.CurrentPage.PageId.ToInvariantString();
            }
            
            if (basePage.UseIconsForAdminLinks)
            {
                ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.EditPageFeaturesImage);
                Text = Resource.PageContentEditText;
            }
            else
            {
                Text = Resource.PageEditLink;
            }

            
        }


    }
}
