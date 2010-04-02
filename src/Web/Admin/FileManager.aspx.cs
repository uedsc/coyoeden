/// Last Modifed: 2009-12-17

using System;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI
{
    public partial class FileManagerPage : CBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // if the user has no upload permissions the file manager control will handle blocking access
            // but upload permissions doesn't guarantee delete permission
            // only users who are trusted to delete should be able to use the file manager
            if (
                WebConfigSettings.DisableFileManager
                || (!WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor))
                )
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
           
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFileManagerLink);
            litHeading.Text = Resource.AdminMenuFileManagerLink;
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkFileManager.Text = Resource.AdminMenuFileManagerLink;
            lnkFileManager.ToolTip = Resource.AdminMenuFileManagerLink;
            lnkFileManager.NavigateUrl = SiteRoot + "/Admin/FileManager.aspx";

            lnkAltFileManager.Text = Resource.FileManagerAlternateLink;
            lnkAltFileManager.NavigateUrl = SiteRoot + "/Admin/FileManagerAlt.aspx";
            
           
        }

        private void LoadSettings()
        {
           
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            this.Load += new EventHandler(Page_Load);
       
            SuppressMenuSelection();
            SuppressPageMenu();
    
        }

       
        #endregion
    }
}
