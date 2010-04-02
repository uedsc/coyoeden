// Author:					Joe Audette
// Created:				    2009-04-08
// Last Modified:		    2009-08-01
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
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web
{
    public class CDialogBasePage : Page
    {

        private PageSettings currentPage = null;
        private SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;

        public SiteSettings CurrentSite
        {
            get
            {
                EnsureSiteSettings();
                return siteSettings;
            }
        }

        public PageSettings CurrentPage
        {
            get
            {
                if (currentPage == null) currentPage = CacheHelper.GetCurrentPage();
                return currentPage;
            }
        }

        public string SiteRoot
        {
            get
            {
                if (siteRoot.Length == 0) { siteRoot = SiteUtils.GetNavigationSiteRoot(); }
                return siteRoot;
            }
        }

        protected void EnsureSiteSettings()
        {
            if (siteSettings == null) siteSettings = CacheHelper.GetCurrentSiteSettings();

        }


        /// <summary>
        /// Returns true if the module exists on the page and the user has permission to edit the page or the module.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool UserCanEditModule(int moduleId)
        {
            if (!Request.IsAuthenticated) return false;

            if (WebUser.IsAdminOrContentAdmin) return true;

            if (SiteUtils.UserIsSiteEditor()) { return true; }

            if (CurrentPage == null) return false;

            bool moduleFoundOnPage = false;
            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId) moduleFoundOnPage = true;
            }

            if (!moduleFoundOnPage) return false;

            if (WebUser.IsInRoles(CurrentPage.EditRoles)) return true;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return false;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId)
                {
                    if (m.EditUserId == currentUser.UserId) return true;
                    if (WebUser.IsInRoles(m.AuthorizedEditRoles)) return true;
                }
            }

            return false;

        }

        public Module GetModule(int moduleId)
        {
            if (CurrentPage == null) return null;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId) { return m; }
            }

            return null;
        }

        public bool UserCanOnlyEditModuleAsDraft(int moduleId)
        {
            if (!Request.IsAuthenticated) return false;

            if (WebUser.IsAdminOrContentAdmin) return false;

            if (SiteUtils.UserIsSiteEditor()) { return false; }

            if (!WebConfigSettings.EnableContentWorkflow) { return false; }
            if (CurrentSite == null) { return false; }
            if (!CurrentSite.EnableContentWorkflow) { return false; }

            if (CurrentPage == null) return false;

            bool moduleFoundOnPage = false;
            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId) moduleFoundOnPage = true;
            }

            if (!moduleFoundOnPage) return false;

            if (WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles)) return true;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return false;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId)
                {
                    if (WebUser.IsInRoles(m.DraftEditRoles)) return true;
                }
            }

            return false;

        }

    }
}
