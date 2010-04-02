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
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// a convenience link for the File manager. The link renders only for those in roles that can use it
    /// </summary>
    public class FileManagerLink : GreyBoxHyperlink
    {
        private CBasePage basePage = null;

        private bool ShouldRender()
        {
            if (basePage == null) { return false; }
            if (!Page.Request.IsAuthenticated) { return false; }
            if (!WebConfigSettings.ShowFileManagerLink) { return false; }
            if (WebConfigSettings.DisableFileManager) { return false; }
            if (basePage.SiteInfo == null) { return false; }

            // only roles that can delete can use file manager
            if (!WebUser.IsInRoles(basePage.SiteInfo.RolesThatCanDeleteFilesInEditor)) { return false; }

            if (
                (WebUser.IsInRoles(basePage.SiteInfo.UserFilesBrowseAndUploadRoles))
                || (WebUser.IsInRoles(basePage.SiteInfo.GeneralBrowseAndUploadRoles))
                ) 
            { return true; }

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
                CssClass = "ModuleEditLink adminlink filemanlink " + CssClass;
            }
            else
            {
                CssClass = "ModuleEditLink adminlink filemanlink";
            }

            Text = Resource.AdminMenuFileManagerLink;
            ToolTip = Resource.AdminMenuFileManagerLink;
            NavigateUrl = basePage.SiteRoot + "/Dialog/FileManagerDialog.aspx";
            if (basePage.UseIconsForAdminLinks)
            {
                ImageUrl = Page.ResolveUrl("~/Data/SiteImages/folder_explore.png");
               
            }
            ClientClick = "GB_showFullScreen(this.title, this.href); return false;";
            DialogCloseText = Resource.CloseDialogButton;
            
        }


    }

}
