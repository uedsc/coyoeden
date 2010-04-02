// Author:					Joe Audette
// Created:				    2004-08-28
// Last Modified:			2010-03-19
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
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Cynthia.Web.Controls.Captcha;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.AdminUI
{
    
    public partial class SiteSettingsPage : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteSettingsPage));
        private string logoPath = string.Empty;
        private bool sslIsAvailable = false;
        private bool IsServerAdmin = false;
        private int currentSiteID = 0;
        private Guid currentSiteGuid = Guid.Empty;
        private int selectedSiteID = 0;
        private bool isAdmin = false;
        private bool allowPasswordFormatChange = false;
        private bool useFolderForSiteDetection = false;
        private SiteSettings selectedSite;
        protected string lastGroupValue = string.Empty;
        private bool enableSiteSettingsSmtpSettings = false;
        private bool maskSMTPPassword = true;
        private string requestedTab = string.Empty;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        //private bool siteIsCommerceEnabled = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebUser.IsAdminOrContentAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();
            
            if (ScriptController != null)
            {
                
                ScriptController.RegisterAsyncPostBackControl(btnAddHost);
                ScriptController.RegisterAsyncPostBackControl(rptHosts);

                ScriptController.RegisterAsyncPostBackControl(btnAddFolder);
                ScriptController.RegisterAsyncPostBackControl(rptFolderNames);

                ScriptController.RegisterAsyncPostBackControl(btnAddFeature);
                ScriptController.RegisterAsyncPostBackControl(btnAddWebPart);
                ScriptController.RegisterAsyncPostBackControl(btnRemoveFeature);
                ScriptController.RegisterAsyncPostBackControl(btnRemoveWebPart);

              
            }

            PopulateLabels();
            SetupScripts();
            

            if (!Page.IsPostBack)
            {
                ViewState["skin"] = selectedSite.Skin;
                BindGeoLists();
                PopulateControls();
                
            }

        }

        private void PopulateControls()
        {
            if (
                (this.IsServerAdmin)
                &&(WebConfigSettings.AllowMultipleSites)
                )
            {
                PopulateMultiSiteControls();
            }

            if (this.selectedSiteID == -1)
            {
                txtSiteName.Text = Resource.SiteSettingsNewSiteLabel;
            }
            else
            {
                txtSiteName.Text = selectedSite.SiteName;

                if ((!siteSettings.IsServerAdminSite) && (WebConfigSettings.HideGoogleAnalyticsInChildSites))
                {
                    divGAnalytics.Visible = false;
                }

                divSiteId.Visible = (siteSettings.IsServerAdminSite && WebConfigSettings.ShowSiteGuidInSiteSettings);
                lblSiteId.Text = selectedSite.SiteId.ToString(CultureInfo.InvariantCulture);
                lblSiteGuid.Text = selectedSite.SiteGuid.ToString();
            }

            txtSlogan.Text = selectedSite.Slogan;
            txtCompanyName.Text = selectedSite.CompanyName;
            lblPrivacySiteRoot.Text = selectedSite.SiteRoot;
            txtPrivacyPolicyUrl.Text = selectedSite.PrivacyPolicyUrl;
            txtRpxNowApiKey.Text = selectedSite.RpxNowApiKey.Trim();
            txtRpxNowApplicationName.Text = selectedSite.RpxNowApplicationName;
            lnkRpxAdmin.NavigateUrl = siteSettings.RpxNowAdminUrl;
            lnkRpxAdmin.Visible = (selectedSite.RpxNowAdminUrl.Length > 0);
            txtOpenSearchName.Text = selectedSite.OpenSearchName;
            txtMetaProfile.Text = selectedSite.MetaProfile;
           

            DirectoryInfo[] skins = SiteUtils.GetSkinList(this.selectedSite);

            ddSkins.DataSource = skins;
            ddSkins.DataBind();

            ddMyPageSkin.DataSource = skins;
            ddMyPageSkin.DataBind();

            ListItem listItem = new ListItem();
            listItem.Value = "";
            listItem.Text = Resource.PageLayoutDefaultSkinLabel;
            ddMyPageSkin.Items.Insert(0, listItem);

            listItem = ddMyPageSkin.Items.FindByValue("printerfriendly");
            if (listItem != null)
            {
                ddMyPageSkin.Items.Remove(listItem);
            }

            listItem = ddMyPageSkin.Items.FindByValue(".svn");
            if (listItem != null)
            {
                ddMyPageSkin.Items.Remove(listItem);
            }

            listItem = ddMyPageSkin.Items.FindByValue(selectedSite.MyPageSkin);
            if (listItem != null)
            {
                ddMyPageSkin.ClearSelection();
                listItem.Selected = true;
            }

            listItem = ddSkins.Items.FindByValue("printerfriendly");
            if (listItem != null)
            {
                ddSkins.Items.Remove(listItem);
            }

            listItem = ddSkins.Items.FindByValue(".svn");
            if (listItem != null)
            {
                ddSkins.Items.Remove(listItem);
            }

            ListItem item;
            if (selectedSite.Skin.Length > 0)
            {
                item = ddSkins.Items.FindByValue(selectedSite.Skin.Replace(".ascx", ""));
                if (item != null)
                {
                    ddSkins.ClearSelection();
                    item.Selected = true;
                }
            }

            item = ddAvatarSystem.Items.FindByValue(selectedSite.AvatarSystem);
            if (item != null)
            {
                ddAvatarSystem.ClearSelection();
                item.Selected = true;
            }

            item = ddCommentSystem.Items.FindByValue(selectedSite.CommentProvider);
            if (item != null)
            {
                ddCommentSystem.ClearSelection();
                item.Selected = true;
            }

            if (selectedSite.SiteGuid == siteSettings.SiteGuid)
            {
                SetupSkinPreviewScript();
            }
            else
            {
                gbSkinPreview.Visible = false;
            }

            ddLogos.DataSource = SiteUtils.GetLogoList(selectedSite);
            ddLogos.DataBind();

            if (selectedSite.Logo.Length > 0)
            {
                item = ddLogos.Items.FindByValue(selectedSite.Logo);
                if (item != null)
                {
                    ddLogos.ClearSelection();
                    item.Selected = true;
                }
                
                imgLogo.Src = ImageSiteRoot + "/Data/Sites/" + selectedSite.SiteId.ToString(CultureInfo.InvariantCulture) + "/logos/" + selectedSite.Logo;
            }

            item = ddEditorProviders.Items.FindByValue(selectedSite.EditorProviderName);
            if (item != null)
            {
                ddEditorProviders.ClearSelection();
                item.Selected = true;
            }

            item = ddNewsletterEditor.Items.FindByValue(selectedSite.NewsletterEditor);
            if (item != null)
            {
                ddNewsletterEditor.ClearSelection();
                item.Selected = true;
            }

            //item = ddEditorSkins.Items.FindByValue(selectedSite.EditorSkin.ToString());
            //if (item != null)
            //{
            //    ddEditorSkins.ClearSelection();
            //    item.Selected = true;
            //}

            item = ddDefaultFriendlyUrlPattern.Items.FindByValue(selectedSite.DefaultFriendlyUrlPattern.ToString());
            if (item != null)
            {
                ddDefaultFriendlyUrlPattern.ClearSelection();
                item.Selected = true;
            }

            item = ddCaptchaProviders.Items.FindByValue(selectedSite.CaptchaProvider);
            if (item != null)
            {
                ddCaptchaProviders.ClearSelection();
                item.Selected = true;
            }

            item = ddDefaultCountry.Items.FindByValue(selectedSite.DefaultCountryGuid.ToString());
            if (item != null)
            {
                ddDefaultCountry.ClearSelection();
                item.Selected = true;
                BindZoneList();
            }


            item = ddDefaultGeoZone.Items.FindByValue(selectedSite.DefaultStateGuid.ToString());
            if (item != null)
            {
                ddDefaultGeoZone.ClearSelection();
                item.Selected = true;
                
            }

            txtRecaptchaPrivateKey.Text = selectedSite.RecaptchaPrivateKey;
            txtRecaptchaPublicKey.Text = selectedSite.RecaptchaPublicKey;
            txtGmapApiKey.Text = selectedSite.GmapApiKey;
            txtAddThisUserId.Text = selectedSite.AddThisDotComUsername;
            txtGoogleAnayticsAccountCode.Text = selectedSite.GoogleAnalyticsAccountCode;
            txtOpenIdSelectorCode.Text = selectedSite.OpenIdSelectorId;
            chkEnableWoopra.Checked = selectedSite.EnableWoopra;
            txtIntenseDebateAccountId.Text = selectedSite.IntenseDebateAccountId;
            txtDisqusSiteShortName.Text = selectedSite.DisqusSiteShortName;

            ISettingControl currencySetting = SiteCurrencySetting as ISettingControl;
            currencySetting.SetValue(selectedSite.CurrencyGuid.ToString());

            //ISettingControl commerceReportRoles = CommerceReportRolesSetting as ISettingControl;
            //commerceReportRoles.SetValue(selectedSite.CommerceReportViewRoles);
           

            if (WebConfigSettings.EnableOpenIdAuthentication)
            {
                chkAllowOpenIDAuth.Checked = selectedSite.AllowOpenIdAuth;
            }
            else
            {
                tabOpenID.Visible = false;
            }

            if (WebConfigSettings.EnableWindowsLiveAuthentication)
            {
                chkAllowWindowsLiveAuth.Checked = selectedSite.AllowWindowsLiveAuth;
                txtWindowsLiveAppID.Text = selectedSite.WindowsLiveAppId;
                txtWindowsLiveKey.Text = selectedSite.WindowsLiveKey;

            }
            else
            {
                //tabWindowsLiveID.Visible = false;
                chkAllowWindowsLiveAuth.Checked = false;
                chkAllowWindowsLiveAuth.Enabled = false;
                txtWindowsLiveAppID.Enabled = false;
                txtWindowsLiveKey.Enabled = false;
            }

            txtAppLogoForWindowsLive.Text = selectedSite.AppLogoForWindowsLive;
            chkAllowWindowsLiveMessengerForMembers.Checked = selectedSite.AllowWindowsLiveMessengerForMembers;
            
            if (!selectedSite.UseLdapAuth)
            {
                chkAllowRegistration.Checked = selectedSite.AllowNewRegistration;
                chkSecureRegistration.Checked = selectedSite.UseSecureRegistration;
                chkAllowUserToChangeName.Checked = selectedSite.AllowUserFullNameChange;
                chkReallyDeleteUsers.Checked = selectedSite.ReallyDeleteUsers;
                chkUseEmailForLogin.Checked = selectedSite.UseEmailForLogin;

            }
            else
            {
                chkAllowRegistration.Enabled = false;
                chkSecureRegistration.Enabled = false;
                chkAllowUserToChangeName.Enabled = false;
                chkReallyDeleteUsers.Enabled = false;
                chkUseEmailForLogin.Enabled = false;
            }

            divDisableDbAuthentication.Visible = !WebConfigSettings.HideDisableDbAuthenticationSetting;
            chkDisableDbAuthentication.Checked = selectedSite.DisableDbAuth;

            chkEnableMyPageFeature.Checked = selectedSite.EnableMyPageFeature;
            chkAllowUserSkins.Checked = selectedSite.AllowUserSkins;
            chkAllowPageSkins.Checked = selectedSite.AllowPageSkins;
            chkAllowHideMenuOnPages.Checked = selectedSite.AllowHideMenuOnPages;

            chkRequireSSL.Checked = selectedSite.UseSslOnAllPages;

            //txtDefaultPageKeywords.Text = selectedSite.MetaKeyWords;
            //txtDefaultPageDescription.Text = selectedSite.MetaDescription;
            //txtDefaultPageEncoding.Text = selectedSite.MetaEncoding;
            //txtDefaultPageAdditionalMetaTags.Text = selectedSite.MetaAdditional;

            txtPreferredHostName.Text = selectedSite.PreferredHostName;
            chkForceContentVersioning.Checked = selectedSite.ForceContentVersioning;
            chkEnableContentWorkflow.Checked = selectedSite.EnableContentWorkflow;

            if (WebConfigSettings.EnforceContentVersioningGlobally)
            {
                chkForceContentVersioning.Checked = true;
                chkForceContentVersioning.Enabled = false;
            }
           
            if (!isAdmin)
            {
                ddSiteList.Visible = false;
                tabHosts.Visible = false;
                liHosts.Visible = false;
                tabFolderNames.Visible = false;
                liFolderNames.Visible = false;
                tabSiteFeatures.Visible = false;
                liSecurity.Visible = false;
                tabSecurity.Visible = false;
                liCommerce.Visible = false;
                tabCommerce.Visible = false;
                divCommerceRoles.Visible = false;
                chkForceContentVersioning.Visible = false;
                chkEnableContentWorkflow.Visible = false;
            }

            chkUseLdapAuth.Checked = selectedSite.UseLdapAuth;
            chkAutoCreateLdapUserOnFirstLogin.Checked = selectedSite.AutoCreateLdapUserOnFirstLogin;
            txtLdapServer.Text = selectedSite.SiteLdapSettings.Server;
            txtLdapPort.Text = selectedSite.SiteLdapSettings.Port.ToString();
            txtLdapDomain.Text = selectedSite.SiteLdapSettings.Domain;
            txtLdapRootDN.Text = selectedSite.SiteLdapSettings.RootDN;

            item = ddLdapUserDNKey.Items.FindByValue(selectedSite.SiteLdapSettings.UserDNKey);
            if (item != null)
            {
                ddLdapUserDNKey.ClearSelection();
                item.Selected = true;
            }

            txtSiteEmailFromAddress.Text = selectedSite.DefaultEmailFromAddress;
            
            item = ddPasswordFormat.Items.FindByValue(selectedSite.PasswordFormat.ToString(CultureInfo.InvariantCulture));

            if (item != null)
            {
                ddPasswordFormat.ClearSelection();
                item.Selected = true;
            }

            ddPasswordFormat.Enabled = allowPasswordFormatChange;

            chkAllowPasswordRetrieval.Checked = selectedSite.AllowPasswordRetrieval;
            chkRequiresQuestionAndAnswer.Checked = selectedSite.RequiresQuestionAndAnswer;
            chkAllowPasswordReset.Checked = selectedSite.AllowPasswordReset;
            txtMaxInvalidPasswordAttempts.Text = selectedSite.MaxInvalidPasswordAttempts.ToString(CultureInfo.InvariantCulture);
            txtPasswordAttemptWindowMinutes.Text = selectedSite.PasswordAttemptWindowMinutes.ToString(CultureInfo.InvariantCulture);
            txtMinimumPasswordLength.Text = selectedSite.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture);
            txtMinRequiredNonAlphaNumericCharacters.Text = selectedSite.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture);
            txtPasswordStrengthRegularExpression.Text = selectedSite.PasswordStrengthRegularExpression;

            if (
                (siteSettings.IsServerAdminSite)
                && (!selectedSite.IsServerAdminSite)
                && (isAdmin)
                && (WebConfigSettings.AllowDeletingChildSites)
                )
            {
                btnDelete.Visible = true;
           
            }

            if (
                (siteSettings.IsServerAdminSite)
                && (isAdmin)
                &&(selectedSiteID > -1)
                && (WebConfigSettings.ShowSkinRestoreButtonInSiteSettings)
                )
            {
                btnRestoreSkins.Visible = true;
            }

            if (
                (siteSettings.IsServerAdminSite)
                && (isAdmin)
                && (WebConfigSettings.AllowForcingPreferredHostName)
                )
            {
                divPreferredHostName.Visible = true;

            }
            else
            {
                divPreferredHostName.Visible = false;
            }

            if (
                (WebConfigSettings.UseRelatedSiteMode)
                &&((selectedSite.SiteId != WebConfigSettings.RelatedSiteID)||(selectedSiteID == -1))
                )
            {
                if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                {
                    liGeneralSecurity.Visible = false;
                    tabGeneralSecurity.Visible = false;
                    liLDAP.Visible = false;
                    tabLDAP.Visible = false;
                    liOpenID.Visible = false;
                    tabOpenID.Visible = false;
                    liWindowsLive.Visible = false;
                    tabWindowsLiveID.Visible = false;  
                    
                }
                else
                {
                    liGeneralSecurity.Visible = false;
                    tabGeneralSecurity.Visible = false;
                    liLDAP.Visible = false;
                    tabLDAP.Visible = false;
                    
                }

                divReallyDeleteUsers.Visible = false;

                if (IsServerAdmin)
                {
                    SetupSiteEditRolesList();
                }
            }

            if (txtRpxNowApiKey.Text.Length == 0)
            {
                UIHelper.AddConfirmationDialog(btnSetupRpx, Resource.RpxButtonConfirm);
            }

            DoTabSelection();
            PopulateMailSettings();
            BindRoles();

        }

        private void BindRoles()
        {
            chkRolesThatCanCreateRootPages.Items.Clear();
            chkGeneralBrowseAndUploadRoles.Items.Clear();
            chkUserFilesBrowseAndUploadRoles.Items.Clear();
            chkRolesThatCanEditContentTemplates.Items.Clear();
            chkRolesNotAllowedToEditModuleSettings.Items.Clear();
            chkRolesThatCanManageUsers.Items.Clear();
            chkRolesThatCanLookupUsers.Items.Clear();
            chkRolesThatCanViewMemberList.Items.Clear();
            chkRolesThatCanViewMyPage.Items.Clear();
            chkCommerceReportRoles.Items.Clear();
            chkRolesThatCanDeleteFilesInEditor.Items.Clear();

            ListItem allItem = new ListItem();
            // localize display
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";
            if (siteSettings.RolesThatCanViewMemberList.LastIndexOf(allItem.Value + ";") > -1) { allItem.Selected = true; }
            chkRolesThatCanViewMemberList.Items.Add(allItem);
            
            allItem = new ListItem();
            // localize display
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";
            if (siteSettings.RolesThatCanViewMyPage.LastIndexOf(allItem.Value + ";") > -1) { allItem.Selected = true; }
            chkRolesThatCanViewMyPage.Items.Add(allItem);
            

            using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (reader.Read())
                {

                    ListItem listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesThatCanCreateRootPages.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesThatCanCreateRootPages.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.GeneralBrowseAndUploadRoles.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkGeneralBrowseAndUploadRoles.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.UserFilesBrowseAndUploadRoles.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkUserFilesBrowseAndUploadRoles.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesThatCanEditContentTemplates.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesThatCanEditContentTemplates.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesNotAllowedToEditModuleSettings.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesNotAllowedToEditModuleSettings.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesThatCanManageUsers.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesThatCanManageUsers.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesThatCanLookupUsers.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesThatCanLookupUsers.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesThatCanViewMemberList.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesThatCanViewMemberList.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesThatCanViewMyPage.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesThatCanViewMyPage.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.CommerceReportViewRoles.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkCommerceReportRoles.Items.Add(listItem);

                    listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    if (siteSettings.RolesThatCanDeleteFilesInEditor.LastIndexOf(listItem.Value + ";") > -1) { listItem.Selected = true; }
                    chkRolesThatCanDeleteFilesInEditor.Items.Add(listItem);

                    
                    
                }
            }

            //if ((!isAdmin))
            //{
            //    //this.chkListAuthRoles.Enabled = false;
            //    //this.chkListEditRoles.Enabled = false;
            //    //this.chkListCreateChildPageRoles.Enabled = false;
            //    //this.chkDraftEditRoles.Enabled = false;
            //}

        }

        private void DoTabSelection()
        {

            switch (requestedTab)
            {
                case "oid":
                    if (tabSecurity.Visible)
                    {
                        liSecurity.Attributes.Add("class", "selected");
                        liOpenID.Attributes.Add("class", "selected");
                    }
                    else
                    {
                        liGeneral.Attributes.Add("class", "selected");
                    }

                    break;

                default:

                    liGeneral.Attributes.Add("class", "selected");

                    if (
                        (WebConfigSettings.UseRelatedSiteMode) 
                        && ((selectedSite.SiteId != WebConfigSettings.RelatedSiteID)||(selectedSiteID == -1))
                        )
                    {
                        liPermissions.Attributes.Add("class", "selected");
                        //if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                        //{
                        //    liCaptcha.Attributes.Add("class", "selected");
                        //}
                        //else
                        //{
                        //    liOpenID.Attributes.Add("class", "selected");
                        //}

                    }

                    break;

            }

        }

        private void BindGeoLists()
        {
            BindCountryList();
            BindZoneList();

        }

        private void BindCountryList()
        {
            DataTable dataTable = GeoCountry.GetList();
            ddDefaultCountry.DataSource = dataTable;
            ddDefaultCountry.DataBind();

        }

        void ddDefaultCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindZoneList();

        }

        private void BindZoneList()
        {
            if (ddDefaultCountry.SelectedIndex > -1)
            {
                Guid countryGuid = new Guid(ddDefaultCountry.SelectedValue);
                using (IDataReader reader = GeoZone.GetByCountry(countryGuid))
                {
                    ddDefaultGeoZone.DataSource = reader;
                    ddDefaultGeoZone.DataBind();
                }
            }
        }


        private void SetupSiteEditRolesList()
        {
            if (selectedSite == null) { return; }

            divSiteEditRoles.Visible = true;
            h3SiteEditRoles.Visible = true;

            using (IDataReader reader = Role.GetSiteRoles(selectedSite.SiteId))
            {
                while (reader.Read())
                {
                    ListItem editItem = new ListItem();
                    editItem.Text = reader["DisplayName"].ToString();
                    editItem.Value = reader["RoleName"].ToString();

                    if ((selectedSite.SiteRootEditRoles.LastIndexOf(editItem.Value + ";")) > -1)
                    {
                        editItem.Selected = true;
                    }

                    chkListEditRoles.Items.Add(editItem);

                }
            }

        }

        private void PopulateMultiSiteControls()
        {
            ddSiteList.Visible = true;
            using (IDataReader reader = SiteSettings.GetSiteList())
            {
                ddSiteList.DataSource = reader;
                ddSiteList.DataBind();
            }

            ddSiteList.Items.Insert(0, new ListItem(Resource.SiteSettingsNewSiteLabel, "-1"));
            ddSiteList.SelectedValue = selectedSiteID.ToString(CultureInfo.InvariantCulture);

            if (ddSiteList.Items.Count > 2)
            {
               
                if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                {
                    PopulateFolderList();
                    
                }
                else
                {
                    PopulateHostList();

                }

                if (!selectedSite.IsServerAdminSite)
                {
                    PopulateFeatures();
                    PopulateWebParts();

                }
            }


        }

        private void PopulateFeatures()
        {
            liFeatures.Visible = true;
            tabSiteFeatures.Visible = true;
            lstAllFeatures.Items.Clear();
            lstSelectedFeatures.Items.Clear();

            ListItem listItem;
            using (IDataReader reader = ModuleDefinition.GetModuleDefinitions(this.currentSiteGuid))
            {
                while (reader.Read())
                {
                    listItem = new ListItem(
                        ResourceHelper.GetResourceString(
                        reader["ResourceFile"].ToString(),
                        reader["FeatureName"].ToString()),
                        reader["Guid"].ToString());

                    lstAllFeatures.Items.Add(listItem);

                }

            }



            using (IDataReader reader = ModuleDefinition.GetModuleDefinitions(selectedSite.SiteGuid))
            {
                while (reader.Read())
                {
                    ListItem matchItem = lstAllFeatures.Items.FindByValue(reader["Guid"].ToString());
                    if (matchItem != null)
                    {
                        lstSelectedFeatures.Items.Add(matchItem);
                        lstAllFeatures.Items.Remove(matchItem);
                    }
                }
            }

            btnAddFeature.Enabled = (lstAllFeatures.Items.Count > 0);
            btnRemoveFeature.Enabled = (lstSelectedFeatures.Items.Count > 0);

        }

        private void PopulateWebParts()
        {
 #if !MONO
            liWebParts.Visible = true;
            tabWebParts.Visible = true;
            lstAllWebParts.Items.Clear();
            lstSelectedWebParts.Items.Clear();


            using (IDataReader reader = WebPartContent.SelectBySite(this.currentSiteID))
            {
                lstAllWebParts.DataSource = reader;
                lstAllWebParts.DataTextField = "ClassName";
                lstAllWebParts.DataValueField = "WebPartID";
                lstAllWebParts.DataBind();
            }

            using (IDataReader reader = WebPartContent.SelectBySite(selectedSite.SiteId))
            {
                while (reader.Read())
                {
                    ListItem matchItem = lstAllWebParts.Items.FindByText(reader["ClassName"].ToString());
                    if (matchItem != null)
                    {
                        lstAllWebParts.Items.Remove(matchItem);

                        matchItem.Value = reader["WebPartID"].ToString();
                        lstSelectedWebParts.Items.Add(matchItem);

                    }
                }
            }

            btnAddWebPart.Enabled = (lstAllWebParts.Items.Count > 0);
            btnRemoveWebPart.Enabled = (lstSelectedWebParts.Items.Count > 0);
#endif

        }

        private void PopulateHostList()
        {
            liHosts.Visible = true;
            tabHosts.Visible = true;
            using (IDataReader reader = SiteSettings.GetHostList(selectedSite.SiteId))
            {
                rptHosts.DataSource = reader;
                rptHosts.DataBind();
            }
            if (rptHosts.Items.Count > 0)
            {
                rptHosts.Visible = true;
                litHostListHeader.Text = Resource.SiteSettingsExistingHostsLabel;
            }
            else
            {
                rptHosts.Visible = false;
            }
        }

        private void PopulateFolderList()
        {

            // no folders can map to root site
            if (!selectedSite.IsServerAdminSite)
            {
                liFolderNames.Visible = true;
                tabFolderNames.Visible = true;
                //fldFolderNames.Visible = true;
                List<SiteFolder> siteFolders = SiteFolder.GetBySite(selectedSite.SiteGuid);
                rptFolderNames.DataSource = siteFolders;
                rptFolderNames.DataBind();
                if (rptFolderNames.Items.Count > 0)
                {
                    rptFolderNames.Visible = true;
                    litFolderNamesListHeading.Text = Resource.SiteSettingsExistingFolderMappingsLabel;
                }
                else
                {
                    rptFolderNames.Visible = false;
                }

            }
        }

        private void PopulateMailSettings()
        {
            if (selectedSite.SiteId == -1)
            {
                //new site
                liMailSettings.Visible = false;
                tabMailSettings.Visible = false;
                return;
            }

            if (!enableSiteSettingsSmtpSettings) { return; }

            divSMTPEncoding.Visible = WebConfigSettings.ShowSmtpEncodingOption;

   
            if (selectedSite.SMTPUser.Length > 0)
            {
                txtSMTPUser.Text = CryptoHelper.Decrypt(selectedSite.SMTPUser);
            }
            if (selectedSite.SMTPPassword.Length > 0)
            {

                txtSMTPPassword.Text = CryptoHelper.Decrypt(selectedSite.SMTPPassword);
            }
            txtSMTPServer.Text = selectedSite.SMTPServer;
            txtSMTPPort.Text = selectedSite.SMTPPort.ToString();
            chkSMTPRequiresAuthentication.Checked = selectedSite.SMTPRequiresAuthentication;
            chkSMTPUseSsl.Checked = selectedSite.SMTPUseSsl;
            txtSMTPPreferredEncoding.Text = selectedSite.SMTPPreferredEncoding;
            

        }

        private void SetMailSettings()
        {
            if (selectedSite.SiteId == -1) { return; }
            if (!enableSiteSettingsSmtpSettings) { return; }

            if (txtSMTPUser.Text.Length > 0)
            {
                selectedSite.SMTPUser = CryptoHelper.Encrypt(txtSMTPUser.Text);
            }
            else
            {
              selectedSite.SMTPUser = string.Empty;
            }
            if (txtSMTPPassword.Text.Length > 0)
            {
                selectedSite.SMTPPassword = CryptoHelper.Encrypt(txtSMTPPassword.Text);
            }
            else
            {
                if (!maskSMTPPassword)
                {
                    selectedSite.SMTPPassword = string.Empty;
                }
            }

            selectedSite.SMTPServer = txtSMTPServer.Text;
            int port = 25;
            int.TryParse(txtSMTPPort.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out port);
            selectedSite.SMTPPort = port;
            selectedSite.SMTPRequiresAuthentication = chkSMTPRequiresAuthentication.Checked;
            selectedSite.SMTPUseSsl = chkSMTPUseSsl.Checked;
            selectedSite.SMTPPreferredEncoding = txtSMTPPreferredEncoding.Text;
            
        }

        

        protected void btnSave_Click(Object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) { return; }

            bool creatingNewSite = false;
            if (this.IsServerAdmin)
            {
                if (isAdmin)
                {
                    if (ddSiteList.SelectedIndex > -1)
                    {
                        int dropDownSiteID = int.Parse(ddSiteList.SelectedValue);
                        if (dropDownSiteID == -1)
                        {	// creating a new site
                            selectedSite = new SiteSettings(dropDownSiteID);
                            creatingNewSite = true;
                        }
                    }
                }
            }
            
            selectedSite.SiteName = txtSiteName.Text;
            selectedSite.Slogan = txtSlogan.Text;
            selectedSite.CompanyName = txtCompanyName.Text;
            selectedSite.PrivacyPolicyUrl = txtPrivacyPolicyUrl.Text;
            
            selectedSite.Logo = ddLogos.SelectedValue;
            if (ddSkins.SelectedValue != "printerfriendly")
            {
                selectedSite.Skin = ddSkins.SelectedValue;
            }
            selectedSite.MyPageSkin = ddMyPageSkin.SelectedValue;
            if (ddEditorProviders.SelectedIndex > -1)
            {
                selectedSite.EditorProviderName = ddEditorProviders.SelectedValue;
            }

            if (ddNewsletterEditor.SelectedIndex > -1)
            {
                selectedSite.NewsletterEditor = ddNewsletterEditor.SelectedValue;
            }
            //selectedSite.EditorSkin = (SiteSettings.ContentEditorSkin)Enum.Parse(typeof(SiteSettings.ContentEditorSkin), ddEditorSkins.SelectedValue);

            selectedSite.AvatarSystem = ddAvatarSystem.SelectedValue;

            selectedSite.DefaultFriendlyUrlPattern = (SiteSettings.FriendlyUrlPattern)Enum.Parse(typeof(SiteSettings.FriendlyUrlPattern), ddDefaultFriendlyUrlPattern.SelectedValue);


            if (ddCaptchaProviders.SelectedIndex > -1)
            {
                selectedSite.CaptchaProvider = ddCaptchaProviders.SelectedValue;
            }

            if (ddDefaultCountry.SelectedValue.Length == 36)
            {
                selectedSite.DefaultCountryGuid = new Guid(ddDefaultCountry.SelectedValue);
            }

            if (ddDefaultGeoZone.SelectedValue.Length == 36)
            {
                selectedSite.DefaultStateGuid = new Guid(ddDefaultGeoZone.SelectedValue);
            }

            selectedSite.RecaptchaPrivateKey = txtRecaptchaPrivateKey.Text;
            selectedSite.RecaptchaPublicKey = txtRecaptchaPublicKey.Text;
            selectedSite.GmapApiKey = txtGmapApiKey.Text;
            selectedSite.AddThisDotComUsername = txtAddThisUserId.Text;
            selectedSite.GoogleAnalyticsAccountCode = txtGoogleAnayticsAccountCode.Text;
            selectedSite.OpenIdSelectorId = txtOpenIdSelectorCode.Text;
            selectedSite.CommentProvider = ddCommentSystem.SelectedValue;
            selectedSite.IntenseDebateAccountId = txtIntenseDebateAccountId.Text;
            selectedSite.DisqusSiteShortName = txtDisqusSiteShortName.Text;

            if (divWoopra.Visible)
            {
                selectedSite.EnableWoopra = chkEnableWoopra.Checked;
            }

            // keep track if password format changed then we need to update passwords to new format
            int previousPasswordFormat = selectedSite.PasswordFormat;

            if (isAdmin)
            {
                selectedSite.PreferredHostName = txtPreferredHostName.Text;

                if (WebConfigSettings.EnableOpenIdAuthentication)
                {
                    selectedSite.AllowOpenIdAuth = chkAllowOpenIDAuth.Checked;
                }
                if (WebConfigSettings.EnableWindowsLiveAuthentication)
                {
                    selectedSite.AllowWindowsLiveAuth = chkAllowWindowsLiveAuth.Checked;
                    selectedSite.WindowsLiveAppId = txtWindowsLiveAppID.Text;
                    selectedSite.WindowsLiveKey = txtWindowsLiveKey.Text;
                }

                selectedSite.DisableDbAuth = chkDisableDbAuthentication.Checked;

                selectedSite.AllowWindowsLiveMessengerForMembers = chkAllowWindowsLiveMessengerForMembers.Checked;
                selectedSite.AppLogoForWindowsLive = txtAppLogoForWindowsLive.Text;

                selectedSite.RpxNowApiKey = txtRpxNowApiKey.Text;
                selectedSite.RpxNowApplicationName = txtRpxNowApplicationName.Text;
                if (selectedSite.RpxNowApiKey.Length == 0) { selectedSite.RpxNowAdminUrl = string.Empty; }

                selectedSite.OpenSearchName = txtOpenSearchName.Text;

                selectedSite.AllowUserSkins = chkAllowUserSkins.Checked;
                selectedSite.AllowPageSkins = chkAllowPageSkins.Checked;
                selectedSite.AllowHideMenuOnPages = chkAllowHideMenuOnPages.Checked;
                selectedSite.UseSecureRegistration = chkSecureRegistration.Checked;
                selectedSite.ForceContentVersioning = chkForceContentVersioning.Checked;
                selectedSite.EnableContentWorkflow = chkEnableContentWorkflow.Checked;

                ISettingControl currencySetting = SiteCurrencySetting as ISettingControl;
                string currencyGuidString = currencySetting.GetValue();
                if (currencyGuidString.Length == 36)
                {
                    selectedSite.CurrencyGuid = new Guid(currencyGuidString);
                }

                //ISettingControl commerceReportRoles = CommerceReportRolesSetting as ISettingControl;
                selectedSite.RolesThatCanCreateRootPages = chkRolesThatCanCreateRootPages.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.CommerceReportViewRoles = chkCommerceReportRoles.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.GeneralBrowseAndUploadRoles = chkGeneralBrowseAndUploadRoles.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.UserFilesBrowseAndUploadRoles = chkUserFilesBrowseAndUploadRoles.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.RolesThatCanEditContentTemplates = chkRolesThatCanEditContentTemplates.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.RolesNotAllowedToEditModuleSettings = chkRolesNotAllowedToEditModuleSettings.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.RolesThatCanManageUsers = chkRolesThatCanManageUsers.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.RolesThatCanLookupUsers = chkRolesThatCanLookupUsers.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.RolesThatCanViewMemberList = chkRolesThatCanViewMemberList.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.RolesThatCanViewMyPage = chkRolesThatCanViewMyPage.Items.SelectedItemsToSemiColonSeparatedString();
                selectedSite.RolesThatCanDeleteFilesInEditor = chkRolesThatCanDeleteFilesInEditor.Items.SelectedItemsToSemiColonSeparatedString();

                if (sslIsAvailable)
                {
                    selectedSite.UseSslOnAllPages = chkRequireSSL.Checked;
                }

                if ((chkAllowRegistration.Enabled) && (divAllowRegistration.Visible))
                {
                    selectedSite.AllowNewRegistration = chkAllowRegistration.Checked;
                }
                else
                {
                    if (chkUseLdapAuth.Checked) { selectedSite.AllowNewRegistration = false; }

                }

                if ((chkAllowUserToChangeName.Enabled) && (divAllowUserToChangeName.Visible))
                {
                    selectedSite.AllowUserFullNameChange = chkAllowUserToChangeName.Checked;
                }

                if ((chkUseEmailForLogin.Enabled) && (divUseEmailForLogin.Visible))
                {
                    selectedSite.UseEmailForLogin = chkUseEmailForLogin.Checked;

                }

                selectedSite.AutoCreateLdapUserOnFirstLogin = chkAutoCreateLdapUserOnFirstLogin.Checked;

                if ((!selectedSite.UseLdapAuth) && (chkUseLdapAuth.Checked))
                {
                    LdapSettings testLdapSettings = new LdapSettings();
                    testLdapSettings.Server = txtLdapServer.Text;
                    testLdapSettings.Port = Convert.ToInt32(txtLdapPort.Text);
                    testLdapSettings.Domain = txtLdapDomain.Text;
                    testLdapSettings.RootDN = txtLdapRootDN.Text;
                    testLdapSettings.UserDNKey = ddLdapUserDNKey.SelectedValue;
                    if (!TestCurrentUserLdap(testLdapSettings))
                    {
                        lblErrorMessage.Text += "  " + Resource.SiteSettingsLDAPAdminUserNotFound;
                        btnSave.Text = Resource.SiteSettingsApplyChangesButton;
                        btnSave.Enabled = true;
                        return;
                    }
                }

                if (selectedSite.SiteId > 0)
                {
                    if (divUseLdap.Visible)
                    {
                        selectedSite.UseLdapAuth = chkUseLdapAuth.Checked;
                    }
                    if (divLdapServer.Visible)
                    {
                        selectedSite.SiteLdapSettings.Server = txtLdapServer.Text;
                    }
                    if (divLdapPort.Visible)
                    {
                        selectedSite.SiteLdapSettings.Port = Convert.ToInt32(txtLdapPort.Text);
                    }

                    if (divLdapDomain.Visible)
                    {
                        selectedSite.SiteLdapSettings.Domain = txtLdapDomain.Text;
                    }

                    if (divLdapRootDn.Visible)
                    {
                        selectedSite.SiteLdapSettings.RootDN = txtLdapRootDN.Text;
                    }
                    if (divLdapUserDNKey.Visible)
                    {
                        selectedSite.SiteLdapSettings.UserDNKey = ddLdapUserDNKey.SelectedValue;
                    }
                }

                if (selectedSite.UseLdapAuth)
                {
                    selectedSite.ReallyDeleteUsers = false;
                }
                else
                {
                    selectedSite.ReallyDeleteUsers = chkReallyDeleteUsers.Checked;
                }

                if (
                (allowPasswordFormatChange)
                || (selectedSite.SiteGuid == Guid.Empty) // new site
                )
                {
                    try
                    {
                        selectedSite.PasswordFormat = int.Parse(ddPasswordFormat.SelectedValue);
                    }
                    catch (ArgumentException) { }
                    catch (FormatException) { }
                }

                selectedSite.AllowPasswordRetrieval = chkAllowPasswordRetrieval.Checked;
                selectedSite.RequiresQuestionAndAnswer = chkRequiresQuestionAndAnswer.Checked;
                selectedSite.AllowPasswordReset = chkAllowPasswordReset.Checked;

                int MaxInvalidPasswordAttempts = selectedSite.MaxInvalidPasswordAttempts;
                int.TryParse(txtMaxInvalidPasswordAttempts.Text, out MaxInvalidPasswordAttempts);
                selectedSite.MaxInvalidPasswordAttempts = MaxInvalidPasswordAttempts;

                int PasswordAttemptWindowMinutes = selectedSite.PasswordAttemptWindowMinutes;
                int.TryParse(txtPasswordAttemptWindowMinutes.Text, out PasswordAttemptWindowMinutes);
                selectedSite.PasswordAttemptWindowMinutes = PasswordAttemptWindowMinutes;

                int MinRequiredPasswordLength = selectedSite.MinRequiredPasswordLength;
                int.TryParse(txtMinimumPasswordLength.Text, out MinRequiredPasswordLength);
                selectedSite.MinRequiredPasswordLength = MinRequiredPasswordLength;

                int MinRequiredNonAlphanumericCharacters = selectedSite.MinRequiredNonAlphanumericCharacters;
                int.TryParse(txtMinRequiredNonAlphaNumericCharacters.Text, out MinRequiredNonAlphanumericCharacters);
                selectedSite.MinRequiredNonAlphanumericCharacters = MinRequiredNonAlphanumericCharacters;

                selectedSite.PasswordStrengthRegularExpression = txtPasswordStrengthRegularExpression.Text;


                if (IsServerAdmin
                && (WebConfigSettings.UseRelatedSiteMode)
                && (selectedSite.SiteId != WebConfigSettings.RelatedSiteID)
                && (chkListEditRoles.Items.Count > 0)
                )
                {
                    string editRoles = string.Empty;

                    foreach (ListItem item in chkListEditRoles.Items)
                    {
                        if (item.Selected)
                        {
                            editRoles = editRoles + item.Value + ";";
                        }
                    }

                    selectedSite.SiteRootEditRoles = editRoles;

                }


            } //end isAdmin


            selectedSite.MetaProfile = txtMetaProfile.Text;
            selectedSite.DefaultEmailFromAddress = txtSiteEmailFromAddress.Text;
            selectedSite.EnableMyPageFeature = chkEnableMyPageFeature.Checked;

            SetMailSettings();

            

            selectedSite.Save();
            if (creatingNewSite)
            {
                CSetup.CreateNewSiteData(selectedSite);
            }
            CacheHelper.TouchSiteSettingsCacheDependencyFile(selectedSite.SiteId);

            CMembershipProvider CMembership = (CMembershipProvider)Membership.Provider;
            

            if (
                (!creatingNewSite)
                && (previousPasswordFormat != selectedSite.PasswordFormat)
                )
            {
                // this is not something you want to change very often
                CMembership.ChangeUserPasswordFormat(selectedSite, previousPasswordFormat);
                CacheHelper.TouchSiteSettingsCacheDependencyFile(selectedSite.SiteId);

            }

            String oldSkin = ViewState["skin"].ToString();
            if (oldSkin != selectedSite.Skin)
            {
                CacheHelper.ResetThemeCache();
            }

            if ((WebConfigSettings.UseRelatedSiteMode)&&(selectedSite.SiteId == WebConfigSettings.RelatedSiteID))
            {
                // need to propagate any security changes to all child sites
                // reset the sitesettigns cache for each site
                SiteSettings.SyncRelatedSites(selectedSite, WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites);

                // reset the sitesettings cache for each site
                CacheHelper.ClearRelatedSiteCache(selectedSite.SiteId);

            }

            String redirectUrl = SiteRoot
                + "/Admin/SiteSettings.aspx?SiteID=" + selectedSite.SiteId.ToString();

            if (selectedSite.SiteId == currentSiteID)
            {
                redirectUrl = Request.RawUrl;
            }

            WebUtils.SetupRedirect(this, redirectUrl);

            //selectedSiteID = selectedSite.SiteID;
            //PopulateControls();
            //upSiteSettings.Update();

        }


        private bool TestCurrentUserLdap(LdapSettings testLdapSettings)
        {
            String uid = Context.User.Identity.Name;
            SiteUser user = new SiteUser(this.selectedSite, uid);
            return LdapHelper.TestUser(testLdapSettings, user.LoginName, txtLdapTestPassword.Text);
        }

        protected void ddSiteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSiteID = int.Parse(ddSiteList.SelectedValue);

            string redirectUrl = SiteRoot
                + "/Admin/SiteSettings.aspx?SiteID=" + selectedSiteID.ToString();

            WebUtils.SetupRedirect(this, redirectUrl);

            //this.selectedSiteID = int.Parse(this.ddSiteList.SelectedValue);
            //PopulateControls();
            //upSiteSettings.Update();

        }

        private void btnAddFeature_Click(object sender, EventArgs e)
        {
            //if (selectedSite == null) { return; }
            if (lstAllFeatures.SelectedIndex > -1)
            {
                foreach (ListItem item in lstAllFeatures.Items)
                {
                    if(item.Selected)
                        SiteSettings.AddFeature(selectedSite.SiteGuid, new Guid(item.Value));

                }

                PopulateFeatures();
                upFeatures.Update();

            }
            else
            {
                lblFeatureMessage.Text = Resource.SiteSettingsSelectFeatureToAddWarning;
            }
        }

        private void btnRemoveFeature_Click(object sender, EventArgs e)
        {
            //if (selectedSite == null) { return; }

            if (lstSelectedFeatures.SelectedIndex > -1)
            {
                foreach (ListItem item in lstSelectedFeatures.Items)
                {
                    if (item.Selected)
                        SiteSettings.RemoveFeature(selectedSite.SiteGuid, new Guid(item.Value));

                }

                PopulateFeatures();
                upFeatures.Update();
            }
            else
            {
                lblFeatureMessage.Text = Resource.SiteSettingsSelectFeatureToRemoveWarning;
            }
        }

        protected void btnRemoveWebPart_Click(object sender, EventArgs e)
        {
            if (lstSelectedWebParts.SelectedIndex > -1)
            {
                foreach (ListItem item in lstSelectedFeatures.Items)
                {
                    if (item.Selected)
                    {
                        Guid webPartID = new Guid(item.Value);
                        WebPartContent.DeleteWebPart(webPartID);

                    }

                }

                PopulateWebParts();
                upWebParts.Update();
                
            }
            else
            {
                lblWebPartMessage.Text = Resource.SiteSettingsSelectWebPartToRemoveWarning;
            }
        }

        protected void btnAddWebPart_Click(object sender, EventArgs e)
        {
            if (lstAllWebParts.SelectedIndex > -1)
            {
                foreach (ListItem item in lstAllWebParts.Items)
                {
                    if (item.Selected)
                    {
                        Guid webPartID = new Guid(item.Value);
                        WebPartContent baseSiteWebPart = new WebPartContent(webPartID);

                        WebPartContent childSiteWebPart = new WebPartContent();
                        childSiteWebPart.SiteId = selectedSite.SiteId;
                        childSiteWebPart.SiteGuid = selectedSite.SiteGuid;
                        childSiteWebPart.AllowMultipleInstancesOnMyPage = baseSiteWebPart.AllowMultipleInstancesOnMyPage;
                        childSiteWebPart.AssemblyName = baseSiteWebPart.AssemblyName;
                        childSiteWebPart.AvailableForContentSystem = baseSiteWebPart.AvailableForContentSystem;
                        childSiteWebPart.AvailableForMyPage = baseSiteWebPart.AvailableForMyPage;
                        childSiteWebPart.ClassName = baseSiteWebPart.ClassName;
                        childSiteWebPart.Description = baseSiteWebPart.Description;
                        childSiteWebPart.ImageUrl = baseSiteWebPart.ImageUrl;
                        childSiteWebPart.Title = baseSiteWebPart.Title;
                        childSiteWebPart.Save();

                    }

                }

                PopulateWebParts();
                upWebParts.Update();
            }
            else
            {
                lblWebPartMessage.Text = Resource.SiteSettingsSelectWebPartToAddWarning;
            }

        }

        private void btnAddHost_Click(object sender, EventArgs e)
        {
            if (selectedSite == null) { return; }

            if (this.txtHostName.Text.Length == 0)
            {
                lblHostMessage.Text = Resource.SiteSettingsHostNameRequiredMessage;
                return;
            }

            try
            {
                SiteSettings.AddHost(selectedSite.SiteGuid, selectedSite.SiteId, this.txtHostName.Text.ToLower());
               
            }
            catch (DbException)
            {
                lblHostMessage.Text = Resource.SiteSettingsDuplicateHostsWarning;
            }

            PopulateHostList(); 

            upHosts.Update();
            

        }

        void rptHosts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int hostID = int.Parse(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "delete":
                    SiteSettings.RemoveHost(hostID);
                    break;

                default:

                    break;
            }

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            PopulateHostList();
            upHosts.Update();
            

        }

        void btnAddFolder_Click(object sender, EventArgs e)
        {
           
            if (txtFolderName.Text.Length > 0)
            {
                if (SiteFolder.Exists(txtFolderName.Text))
                {
                    lblFolderMessage.Text = Resource.SiteSettingsFolderNameAlreadyInUseWarning;
                    return;
                }

                if (!SiteFolder.IsAllowedFolder(txtFolderName.Text))
                {
                    lblFolderMessage.Text = Resource.SiteSettingsFolderNameNotAllowedWarning;
                    return;
                }

                if (SiteFolder.HasInvalidChars(txtFolderName.Text))
                {
                    lblFolderMessage.Text = Resource.SiteSettingsFolderNameInvalidCharsWarning;
                    return;
                }
                
                SiteFolder siteFolder = new SiteFolder();
                siteFolder.SiteGuid = selectedSite.SiteGuid;
                siteFolder.FolderName = txtFolderName.Text;
                siteFolder.Save();

                PopulateFolderList();
                upFolderNames.Update();

            }
            else
            {
                lblFolderMessage.Text = Resource.SiteSettingsFolderNameBlankWarning;
            }
        }

        void rptFolderNames_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Guid folderGuid = new Guid(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "delete":
                    SiteFolder.Delete(folderGuid);
                    break;

                default:

                    break;
            }

            PopulateFolderList();
            upFolderNames.Update();
        }


        void rptFolderNames_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ImageButton btnDelete = e.Item.FindControl("btnDeleteFolder") as ImageButton;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.SiteSettingsDeleteFolderMappingWarning);
        }


        void rptHosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ImageButton btnDelete = e.Item.FindControl("btnDeleteHost") as ImageButton;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.SiteSettingsHostDeleteWarning);
        }

        void btnRestoreSkins_Click(object sender, EventArgs e)
        {
            if (selectedSite == null) { return; }
            CSetup.CreateOrRestoreSiteSkins(selectedSite.SiteId);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        void btnDelete_Click(object sender, EventArgs e)
        {
            if (WebConfigSettings.AllowDeletingChildSites)
            {
                if ((selectedSite != null) && (!selectedSite.IsServerAdminSite))
                {
                    try
                    {
                        DeleteSiteContent(selectedSite.SiteId);

                    }
                    catch (Exception ex)
                    {
                        log.Error("error deleting site content ", ex);
                    }

                    SiteSettings.Delete(selectedSite.SiteId);
                    WebUtils.SetupRedirect(this, "SiteSettings.aspx");
                }
            }
        }

        private void DeleteSiteContent(int siteId)
        {
            if (siteId == -1) { return; }

            foreach(SitePreDeleteHandlerProvider contentDeleter in SitePreDeleteHandlerProviderManager.Providers)
            {
                
                try
                {
                    contentDeleter.DeleteSiteContent(siteId);
                }
                catch (Exception ex)
                {
                    log.Error("SiteSettings.aspx.cs.DeleteSiteContent ", ex);
                }

            }

            if (WebConfigSettings.DeleteSiteFolderWhenDeletingSites)
            {
                FolderDeleteTask task = new FolderDeleteTask();
                task.SiteGuid = siteSettings.SiteGuid;
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    task.QueuedBy = currentUser.UserGuid;
                }
                task.FolderToDelete = Server.MapPath("~/Data/Sites/" + siteId.ToInvariantString() + "/");
                task.QueueTask();

                WebTaskManager.StartOrResumeTasks();
            }

        }


        void btnSetupRpx_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) { return; }

            if (txtRpxNowApiKey.Text.Length == 0)
            {
                string redirectUrl = OpenIdRpxHelper.GetPluginApiRedirectUrl(
                    HttpContext.Current,
                    SiteRoot,
                    SiteRoot + "/Services/RpxPluginResponseHandler.ashx",
                    siteSettings.SiteGuid.ToString());

                WebUtils.SetupRedirect(this, redirectUrl);

                return;
            }

            OpenIdRpxAccountInfo rpxAccount = OpenIdRpxHelper.LookupRpxAccount(txtRpxNowApiKey.Text, false);
            if (rpxAccount == null)
            {
                WebUtils.SetupRedirect(this, OpenIdRpxHelper.GetPluginApiRedirectUrl(
                    HttpContext.Current,
                    SiteRoot,
                    SiteRoot + "/Services/RpxPluginResponseHandler.ashx",
                    siteSettings.SiteGuid.ToString()));

                return;

            }

            siteSettings.RpxNowAdminUrl = rpxAccount.AdminUrl;
            siteSettings.RpxNowApiKey = rpxAccount.ApiKey;
            siteSettings.RpxNowApplicationName = rpxAccount.Realm;
            siteSettings.Save();
            CacheHelper.TouchSiteSettingsCacheDependencyFile(siteSettings.SiteId);
            

            WebUtils.SetupRedirect(this, SiteRoot + "/Admin/SiteSettings.aspx?t=oid");

        }


        private void SetupScripts()
        {
            string logoScript = "<script type=\"text/javascript\">"
                + "function showLogo(listBox) { if(!document.images) return; "
                + "var logoPath = '" + logoPath + "'; "
                + "document.images." + imgLogo.ClientID + ".src = logoPath + listBox.value;"
                + "}</script>";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showLogo", logoScript);

            

        }

        private void SetupSkinPreviewScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("var skinset= [");
            string comma = string.Empty;

            foreach (ListItem item in ddSkins.Items)
            {

                script.Append(comma);
                script.Append("{'caption':'");
                script.Append(Server.HtmlEncode(item.Text));
                script.Append("','url':'");
                script.Append(SiteRoot + "/?skin=");
                script.Append(item.Value);
                script.Append("'");
                script.Append("}");

                comma = ",";

            }


            script.Append("];");


            gbSkinPreview.NavigateUrl = Request.RawUrl;

            gbSkinPreview.ClientClick = "return GB_showFullScreenSet(skinset, 1)";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "skinpreview", "\n<script type=\"text/javascript\">\n"
                    + script.ToString()
                    + "\n</script>");

        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuSiteSettingsLink);

            litSettingsTab.Text = Resource.SiteSettingsGeneralSettingsTab;
            litSecurityTab.Text = Resource.SiteSettingsSecurityTab;
            litCommerceTab.Text = Resource.CommerceTab;
            litGeneralSecurityTab.Text = Resource.SiteSettingsSecurityMainTab;
            litPermissionsTab.Text = Resource.SiteSettingsPermissionsTab;
            litLDAPTab.Text = Resource.SiteSettingsLdapSettingsLabel;
            litOpenIDTab.Text = Resource.SiteSettingsSecurityOpenIDTab;
            litWindowsLiveTab.Text = Resource.SiteSettingsSecurityWindowsLiveTab;
            litAntiSpamTab.Text = Resource.SiteSettingsSecurityAntiSPAMTab;
            
            litAPIKeysTab.Text = Resource.SiteSettingsApiKeysTab;

            if (enableSiteSettingsSmtpSettings)
            {
                liMailSettings.Visible = true;
                tabMailSettings.Visible = true;
            }
            else
            {
                liMailSettings.Visible = false;
                tabMailSettings.Visible = false;
            }

            if (maskSMTPPassword)
            {
                txtSMTPPassword.TextMode = TextBoxMode.Password;
            }
            else
            {
                txtSMTPPassword.TextMode = TextBoxMode.SingleLine;
            }

            litMailSettingsTab.Text = Resource.MailSettingsTab;
            litFeaturesTab.Text = Resource.SiteSettingsFeaturesAllowedLabel;
            litWebPartsTab.Text = Resource.SiteSettingsWebPartTab;

            lnkSecurity.HRef = "#" + tabSecurity.ClientID;
            lnkCommerce.HRef = "#" + tabCommerce.ClientID;
            lnkFeatures.HRef = "#" + tabSiteFeatures.ClientID;
            lnkWebParts.HRef = "#" + tabWebParts.ClientID;
            lnkHosts.HRef = "#" + tabHosts.ClientID;
            lnkFolderNames.HRef = "#" + tabFolderNames.ClientID;
            lnkLDAP.HRef = "#" + tabLDAP.ClientID;
            lnkOpenIDTab.HRef = "#" + tabOpenID.ClientID;
            lnkWindowsLive.HRef = "#" + tabWindowsLiveID.ClientID;
            lnkMailSettings.HRef = "#" + tabMailSettings.ClientID;
            lnkGeneralSecurityTab.HRef = "#" + tabGeneralSecurity.ClientID;
            lnkPermissions.HRef = "#" + tabPermissions.ClientID;

            litHostsTab.Text = Resource.SiteSettingsHostNameMappingLabel;
            litFolderNamesTab.Text = Resource.SiteSettingsFolderMappingLabel;

            btnAddWebPart.ToolTip = Resource.SiteSettignsAddWebPartTooltip;
            btnAddFeature.ToolTip = Resource.SiteSettingsAddFeatureTooltip;
            btnRemoveWebPart.ToolTip = Resource.SiteSettingsRemoveWebPartTooltip;
            btnRemoveFeature.ToolTip = Resource.SiteSettingsRemoveFeatureTooltip;

            lblFeatureMessage.Text = string.Empty;
            lblWebPartMessage.Text = string.Empty;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkSiteSettings.Text = Resource.AdminMenuSiteSettingsLink;
            lnkSiteSettings.ToolTip = Resource.AdminMenuSiteSettingsLink;
            lnkSiteSettings.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx";

            if (ddEditorProviders.Items.Count == 0)
            {
                ddEditorProviders.DataSource = EditorManager.Providers;
                ddEditorProviders.DataBind();
                foreach (ListItem providerItem in ddEditorProviders.Items)
                {
                    providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
                }
            }

            if (ddNewsletterEditor.Items.Count == 0)
            {
                ddNewsletterEditor.DataSource = EditorManager.Providers;
                ddNewsletterEditor.DataBind();
                foreach (ListItem providerItem in ddNewsletterEditor.Items)
                {
                    providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
                }
            }

            if (ddCaptchaProviders.Items.Count == 0)
            {
                ddCaptchaProviders.DataSource = CaptchaManager.Providers;
                ddCaptchaProviders.DataBind();
                foreach (ListItem providerItem in ddCaptchaProviders.Items)
                {
                    providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
                }
            }

            btnAddFolder.Text = Resource.SiteSettingsAddFolderMappingButton;
            btnAddFolder.ToolTip = Resource.SiteSettingsAddFolderMappingButton;

            btnSave.Text = Resource.SiteSettingsApplyChangesButton;
            UIHelper.DisableButtonAfterClick(
                btnSave,
                Resource.PleaseWaitButton,
                Page.ClientScript.GetPostBackEventReference(this.btnSave, string.Empty)
                );

            btnSave.ToolTip = Resource.SiteSettingsApplyChangesButton;
            btnDelete.Text = Resource.SiteSettingsDeleteSiteButton;
            btnDelete.ToolTip = Resource.SiteSettingsDeleteSiteButton;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.SiteSettingsDeleteWarning);

            imgLogo.Alt = Resource.SiteSettingsLogoAltText;
            ddLogos.Attributes.Add("onchange", "javascript:showLogo(this);");
            ddLogos.Attributes.Add("size", "6");

            if (ddPasswordFormat.Items.Count == 0)
            {
                ListItem listItem = new ListItem(Resource.SiteSettingsClearTextPasswordLabel, "0");
                ddPasswordFormat.Items.Add(listItem);
                listItem = new ListItem(Resource.SiteSettingsEncryptedPasswordLabel, "2");
                ddPasswordFormat.Items.Add(listItem);
                listItem = new ListItem(Resource.SiteSettingsHashedPasswordLabel, "1");
                ddPasswordFormat.Items.Add(listItem);
            }

            litHostListHeader.Text = Resource.SiteSettingsNoHostsFound;
            btnAddHost.Text = Resource.SiteSettingsAddHostButtonLabel;
            btnAddHost.ToolTip = Resource.SiteSettingsAddHostButtonLabel;
            litFolderNamesListHeading.Text = Resource.SiteSettingsNoFolderNames;
            divFriendlyUrlPattern.Visible = WebConfigSettings.AllowChangingFriendlyUrlPattern;

            regexMaxInvalidPasswordAttempts.ErrorMessage = Resource.MaxInvalidPasswordAttemptsRegexWarning;
            regexMinPasswordLength.ErrorMessage = Resource.MinPasswordLengthRegexWarning;
            regexPasswordAttemptWindow.ErrorMessage = Resource.PasswordAttemptWindowRegexWarning;
            regexPasswordMinNonAlphaNumeric.ErrorMessage = Resource.PasswordMinNonAlphaNumericRegexWarning;
            regexWinLiveSecret.ErrorMessage = Resource.WindowsLiveSecretKeyMinLengthWarning;

            btnRestoreSkins.Text = Resource.CopyRestoreSkinsButton;

            btnSetupRpx.Text = Resource.SetupRpxButton;
            lnkRpxAdmin.Text = Resource.RpxAdminLink;

            gbSkinPreview.Text = Resource.SkinPreviewLink;
            

            if (WebConfigSettings.EnableWoopraGlobally || WebConfigSettings.DisableWoopraGlobally) { divWoopra.Visible = false; }

            // only need to set this for first instance of helplinkbutton on the page
            

#if MONO
                divMyPage.Visible = false;

#endif

        }

        

        private void LoadSettings()
        {
            lblErrorMessage.Text = String.Empty;
            isAdmin = WebUser.IsAdmin;
            useFolderForSiteDetection = WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites;

            if (Request.QueryString["t"] != null)
            {
                requestedTab = Request.QueryString["t"];
            }

            if (SiteUtils.SslIsAvailable())
            {
                this.sslIsAvailable = true;
                this.divSSL.Visible = true;
            }
            else
            {
                this.divSSL.Visible = false;
            }

            lblSiteRoot.Text = SiteRoot;
            if (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
            {
                divLiveMessenger.Visible = false;
            }

            divApprovalsWorkflow.Visible = WebConfigSettings.EnableContentWorkflow;

            enableSiteSettingsSmtpSettings = WebConfigSettings.EnableSiteSettingsSmtpSettings;
            maskSMTPPassword = WebConfigSettings.MaskSmtpPasswordInSiteSettings;

            divOpenIDSelector.Visible = WebConfigSettings.ShowLegacyOpenIDSelector;

            IsServerAdmin = siteSettings.IsServerAdminSite;

            //divAdditionalMeta.Visible = WebConfigSettings.ShowAdditionalMeta;
            //divPageEncoding.Visible = WebConfigSettings.ShowPageEncoding;
            //txtDefaultPageEncoding.Text = CSetup.DefaultPageEncoding;

            

            currentSiteID = siteSettings.SiteId;
            currentSiteGuid = siteSettings.SiteGuid;

            selectedSiteID = siteSettings.SiteId;

            if ((IsServerAdmin) 
                && (Page.Request.Params.Get("SiteID") != null)
                )
            {
                selectedSiteID 
                    = WebUtils.ParseInt32FromQueryString("SiteID", selectedSiteID);

            }

            if ((selectedSiteID != siteSettings.SiteId)
                && (selectedSiteID > 0))
            {
                selectedSite = new SiteSettings((selectedSiteID));
                
            }
            else
            {
                selectedSite = siteSettings;
            }

            logoPath = ImageSiteRoot
                + "/Data/Sites/" + selectedSite.SiteId.ToString() + "/logos/";

            imgLogo.Src = ImageSiteRoot
                + "/Data/Sites/" + selectedSite.SiteId.ToString() 
                + "/logos/blank.gif";

            allowPasswordFormatChange
                = ((IsServerAdmin && WebConfigSettings.AllowPasswordFormatChange) || ((IsServerAdmin) && (selectedSiteID == -1)));

            if ((!IsServerAdmin) && (!WebConfigSettings.AllowPasswordFormatChangeInChildSites))
            {
                allowPasswordFormatChange = false;
            }

            if (!WebConfigSettings.AllowMultipleSites)
            {
                this.IsServerAdmin = false;
            }

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }
            

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnAddFeature.Click += new EventHandler(btnAddFeature_Click);
            this.btnRemoveFeature.Click += new EventHandler(btnRemoveFeature_Click);
            this.btnAddHost.Click += new EventHandler(btnAddHost_Click);
            this.btnAddFolder.Click += new EventHandler(btnAddFolder_Click);
            this.rptHosts.ItemCommand += new RepeaterCommandEventHandler(rptHosts_ItemCommand);
            this.rptHosts.ItemDataBound += new RepeaterItemEventHandler(rptHosts_ItemDataBound);
            this.rptFolderNames.ItemCommand += new RepeaterCommandEventHandler(rptFolderNames_ItemCommand);
            this.rptFolderNames.ItemDataBound += new RepeaterItemEventHandler(rptFolderNames_ItemDataBound);
            this.btnAddWebPart.Click += new EventHandler(btnAddWebPart_Click);
            this.btnRemoveWebPart.Click += new EventHandler(btnRemoveWebPart_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            btnRestoreSkins.Click += new EventHandler(btnRestoreSkins_Click);
            btnSetupRpx.Click += new EventHandler(btnSetupRpx_Click);

            ddDefaultCountry.SelectedIndexChanged += new EventHandler(ddDefaultCountry_SelectedIndexChanged);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeYuiTabs = true;
            IncludeYuiTabsCss = true;

            JQueryUIThemeName = "base";
        }

        

        

        

   
        #endregion
    }
}
