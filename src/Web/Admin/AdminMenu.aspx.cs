/// Author:					Joe Audette
/// Created:				2007-04-29
/// Last Modified:			2009-07-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI
{
    
    public partial class AdminMenuPage : CBasePage
    {
        private bool IsAdminOrContentAdmin = false;
        private bool IsAdmin = false;
        private bool isSiteEditor = false;
        private bool isCommerceReportViewer = false;
        private CommerceConfiguration commerceConfig = null;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (
                (!WebUser.IsAdminOrContentAdminOrRoleAdminOrNewsletterAdmin)
                &&(!isSiteEditor)
                && (!isCommerceReportViewer)
                )
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
                return;
            }

            SecurityHelper.DisableBrowserCache();
            
            PopulateLabels();
            PopulateControls();
            
            
        }

        private void PopulateControls()
        {
            BuildAdditionalMenuListItems();

        }

        private void BuildAdditionalMenuListItems()
        {
            if (siteSettings == null) return;

            ContentAdminLinksConfiguration linksConfig 
                = ContentAdminLinksConfiguration.GetConfig(siteSettings.SiteId);

            StringBuilder addedLinks = new StringBuilder();
            foreach (ContentAdminLink link in linksConfig.AdminLinks)
            {
                if (
                    (link.VisibleToRoles.Length == 0)
                    ||(WebUser.IsInRoles(link.VisibleToRoles))
                    )
                {
                    addedLinks.Append("\n<li>");
                    addedLinks.Append("<a ");
                    string title = ResourceHelper.GetResourceString(link.ResourceFile, link.ResourceKey);
                    addedLinks.Append("title='" + title + "' ");
                    string url = link.Url;
                    if (url.StartsWith("~/"))
                    {
                        url = SiteRoot + "/" + url.Replace("~/", string.Empty);
                    }
                    addedLinks.Append("href='" + url + "'>" + title + "</a>");
                    addedLinks.Append("</li>");
                }

            }

            litSupplementalLinks.Text = addedLinks.ToString();
        }
        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuHeading);

            litAdminHeading.Text = Resource.AdminMenuHeading;

            liSiteSettings.Visible = IsAdminOrContentAdmin;
            lnkSiteSettings.Text = Resource.AdminMenuSiteSettingsLink;
            lnkSiteSettings.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx";

           
            liCommerceReports.Visible = (isCommerceReportViewer &&(commerceConfig != null)&&(commerceConfig.IsConfigured));
            lnkCommerceReports.Text = Resource.CommerceReportsLink;
            lnkCommerceReports.NavigateUrl = SiteRoot + "/Admin/SalesSummary.aspx";

            liContentManager.Visible = (IsAdminOrContentAdmin || isSiteEditor);
            lnkContentManager.Text = Resource.AdminMenuContentManagerLink;
            lnkContentManager.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";

            liContentWorkFlow.Visible = (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow);
            lnkContentWorkFlow.Visible = siteSettings.EnableContentWorkflow && WebUser.IsAdminOrContentAdminOrContentPublisher;
            lnkContentWorkFlow.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkContentWorkFlow.NavigateUrl = SiteRoot + "/Admin/ContentWorkflow.aspx";

            liContentTemplates.Visible = (IsAdminOrContentAdmin || isSiteEditor || (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)));
            lnkContentTemplates.Text = Resource.ContentTemplates;
            lnkContentTemplates.NavigateUrl = SiteRoot + "/Admin/ContentTemplates.aspx";

            liStyleTemplates.Visible = (IsAdminOrContentAdmin || isSiteEditor || (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)));
            lnkStyleTemplates.Text = Resource.ContentStyleTemplates;
            lnkStyleTemplates.NavigateUrl = SiteRoot + "/Admin/ContentStyles.aspx";

            

            liPageTree.Visible = (IsAdminOrContentAdmin||isSiteEditor);
            lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
            lnkPageTree.NavigateUrl = SiteRoot + "/Admin/PageTree.aspx";

            liRoleAdmin.Visible = (WebUser.IsRoleAdmin || IsAdmin);
            lnkRoleAdmin.Text = Resource.AdminMenuRoleAdminLink;
            lnkRoleAdmin.NavigateUrl = SiteRoot + "/Admin/RoleManager.aspx";

            if ((WebConfigSettings.UseRelatedSiteMode)&&(WebConfigSettings.RelatedSiteModeHideRoleManagerInChildSites))
            {
                if (siteSettings.SiteId != WebConfigSettings.RelatedSiteID) { liRoleAdmin.Visible = false; }
            }

            liFileManager.Visible = IsAdminOrContentAdmin;
            lnkFileManager.Text = Resource.AdminMenuFileManagerLink;
            lnkFileManager.NavigateUrl = SiteRoot + "/Admin/FileManager.aspx";
            if (
                    (!siteSettings.IsServerAdminSite)
                    && (!WebConfigSettings.AllowFileManagerInChildSites)
                    )
            {
                liFileManager.Visible = false;
            }

            if (WebConfigSettings.DisableFileManager)
            {
                liFileManager.Visible = false;
            }

            lnkMemberList.Text = Resource.MemberListLink;
            lnkMemberList.NavigateUrl = SiteRoot + "/MemberList.aspx";

            liAddUser.Visible = IsAdmin;
            lnkAddUser.Text = Resource.MemberListAddUserLabel;
            lnkAddUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userId=-1";

            if (WebConfigSettings.EnableNewsletter)
            {
                liNewsletter.Visible = (IsAdmin || isSiteEditor || WebUser.IsNewsletterAdmin);
                lnkNewsletter.Text = Resource.AdminMenuNewsletterAdminLabel;
                lnkNewsletter.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

                //liTaskQueue.Visible = IsAdmin || WebUser.IsNewsletterAdmin;
                //lnkTaskQueue.Text = Resource.TaskQueueMonitorHeading;
                //lnkTaskQueue.NavigateUrl = SiteRoot + "/Admin/TaskQueueMonitor.aspx";

            }
            else
            {
                liNewsletter.Visible = false;
                //liTaskQueue.Visible = false;
            }

            liCoreData.Visible = (IsAdminOrContentAdmin && siteSettings.IsServerAdminSite);
            lnkCoreData.Text = Resource.CoreDataAdministrationLink;
            lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            liAdvancedTools.Visible = (IsAdminOrContentAdmin || isSiteEditor);
            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";
            

            liServerInfo.Visible = IsAdminOrContentAdmin && (siteSettings.IsServerAdminSite || WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu);
            lnkServerInfo.Text = Resource.AdminMenuServerInfoLabel;
            lnkServerInfo.NavigateUrl = SiteRoot + "/Admin/ServerInformation.aspx";

            liLogViewer.Visible = IsAdmin && siteSettings.IsServerAdminSite && WebConfigSettings.EnableLogViewer;
            lnkLogViewer.Text = Resource.AdminMenuServerLogLabel;
            lnkLogViewer.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";

        }


        private void LoadSettings()
        {
            IsAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
            IsAdmin = WebUser.IsAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            commerceConfig = SiteUtils.GetCommerceConfig();
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
