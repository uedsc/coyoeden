//  Author:                     Joe Audette
//  Created:                    2009-12-30
//	Last Modified:              2010-01-01
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
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.Dialog
{
    public partial class FileManagerAltDialog : Page
    {
        private SiteSettings siteSettings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

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

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFileManagerLink);
            lnkAltFileManager.Text = Resources.Resource.FileManagerAlternateLink;
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
        }


    }
}
