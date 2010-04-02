/// Author:					Joe Audette
/// Created:				2007-08-08
/// Last Modified:			2009-12-16
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI
{
    public partial class ServerInformation : CBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!WebUser.IsAdminOrContentAdminOrRoleAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            
            if (
                (!siteSettings.IsServerAdminSite)
                && (!WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu)
                )
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            
            litPlatform.Text = DatabaseHelper.DBPlatform();
            litCodeVersion.Text = DatabaseHelper.DBCodeVersion().ToString();
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
            {
                litServerTimeZone.Text = TimeZone.CurrentTimeZone.DaylightName;
            }
            else
            {
                litServerTimeZone.Text = TimeZone.CurrentTimeZone.StandardName;
            }

            double preferredOffset = DateTimeHelper.GetPreferredGmtOffset();

            litServerLocalTime.Text = DateTime.Now.ToString();
            litCurrentGMT.Text = DateTime.UtcNow.ToString();
            litServerGMTOffset.Text = DateTimeHelper.GetServerGmtOffset().ToString();
            litPreferredGMTOffset.Text = preferredOffset.ToString();
            litPreferredTime.Text = DateTime.UtcNow.AddHours(preferredOffset).ToString();

            using (IDataReader reader = DatabaseHelper.SchemaVersionGetNonCore())
            {
                grdSchemaVersion.DataSource = reader;
                grdSchemaVersion.DataBind();
            }

        }

        private void PopulateLabels()
        {
            litFeaturesHeading.Text = Resource.FeatureVersions;
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkServerInfo.Text = Resource.AdminMenuServerInfoLabel;
            lnkServerInfo.ToolTip = Resource.AdminMenuServerInfoLabel;
            lnkServerInfo.NavigateUrl = SiteRoot + "/Admin/ServerInformation.aspx";

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuServerInfoLabel);
            litHeading.Text = Resource.AdminMenuServerInfoLabel;

             grdSchemaVersion.Columns[0].HeaderText = Resource.Feature;
             grdSchemaVersion.Columns[1].HeaderText = Resource.SchemaVersion;

        }

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SuppressMenuSelection();
            SuppressPageMenu();
        }


        #endregion

    }
}
