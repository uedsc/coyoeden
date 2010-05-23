/// Author:					    Joe Audette
/// Created:				    2004-08-29
/// Last Modified:			    2010-03-14
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Business.WebHelpers.UserRegisteredHandlers;
using Cynthia.Web.Configuration;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;
using Resources;
using Brettle.Web.NeatUpload;

namespace Cynthia.Web.AdminUI 
{
    public partial class ManageUsers : CBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ManageUsers));
        private Guid userGuid = Guid.Empty;
        private int pageID = -1;
        private int userID = -1;
        private string AvatarPath = string.Empty;
        private SiteUser siteUser = null;
        private SiteUser currentUser = null;
        protected Double TimeOffset = 0;
        private bool isAdmin = false;
       
        //Gravatar public enum RatingType { G, PG, R, X }
        private Gravatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        private bool allowGravatars = false;
        private bool disableAvatars = true;
        private CommerceConfiguration commerceConfig = null;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

		
        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.addExisting.Click += new EventHandler(this.AddRole_Click);
            this.userRoles.ItemDataBound += new DataListItemEventHandler(userRoles_ItemDataBound);
            this.userRoles.ItemCommand += new DataListCommandEventHandler(this.UserRoles_ItemCommand);
            this.btnUnlockUser.Click += new EventHandler(btnUnlockUser_Click);
            this.btnLockUser.Click += new EventHandler(btnLockUser_Click);
            this.btnConfirmEmail.Click += new EventHandler(btnConfirmEmail_Click);
            //btnUploadAvatar.Click += new EventHandler(btnUploadAvatar_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeYuiTabs = true;
            IncludeYuiTabsCss = true;

            LoadSettings();


            if (this.userID > -1)
            {
                siteUser = new SiteUser(siteSettings, this.userID);
                newsLetterPrefs.UserGuid = siteUser.UserGuid;
                purchaseHx.UserGuid = siteUser.UserGuid;
                purchaseHx.ShowAdminOrderLink = true;
            }
            else
            {
                if (userGuid != Guid.Empty)
                {
                    siteUser = new SiteUser(siteSettings, userGuid);
                    newsLetterPrefs.UserGuid = siteUser.UserGuid;
                    purchaseHx.UserGuid = siteUser.UserGuid;
                    purchaseHx.ShowAdminOrderLink = true;

                }
            }
        }

        

        

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (userID == -1)
            {
                if (!WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers))
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

            }
            else
            {
                if (WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers) && !isAdmin)
                {
                    // only admins can edit admins
                    if (siteUser.IsInRoles("Admins"))
                    {
                        SiteUtils.RedirectToEditAccessDeniedPage();
                        return;
                    }
                    HideNonAdminControls();
                }
                else
                {
                    if (!isAdmin)
                    {
                        SiteUtils.RedirectToEditAccessDeniedPage();
                        return;
                    }
                }
            }

            SetupAvatarScript();

			this.divUserGuid.Visible = false;
            divProfileApproved.Visible = false;
            divApprovedForGroups.Visible = false;
            
            divOpenID.Visible = ((WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth) || siteSettings.RpxNowApiKey.Length > 0);

            divWindowsLiveID.Visible = WebConfigSettings.EnableWindowsLiveAuthentication && siteSettings.AllowWindowsLiveAuth;
           

            PopulateProfileControls();

            //if (!allowGravatars)
            //{
            //    if (disableOldAvatars)
            //    {
            //        divAvatarUrl.Visible = false;
            //    }
            //    else
            //    {
            //        AvatarPath = ImageSiteRoot + "/Data/Sites/" + siteSettings.SiteId.ToString() + "/avatars/";
            //    }
            //}

			PopulateLabels();
            //SetupAvatarScript();

            if (!IsPostBack)
			{
                PopulateControls();
            }

            
        }

	
		private void PopulateControls()
		{
            if (!siteSettings.RequiresQuestionAndAnswer)
            {
                divSecurityQuestion.Visible = false;
                divSecurityAnswer.Visible = false;
            }

            if ((siteUser != null)&&(siteUser.UserId > -1))
			{
                spnTitle.InnerText = Resource.ManageUsersTitleLabel + " " + siteUser.Name;
                
                txtName.Text = siteUser.Name;
                this.txtLoginName.Text = siteUser.LoginName;

                lnkAvatarUpload.ClientClick = "return GB_showPage('" + Page.Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture, Resource.UploadAvatarForUserFormat, siteUser.Name)) + "', this.href, GBCallback)";
                txtEmail.Text = siteUser.Email;
                txtOpenIDURI.Text = siteUser.OpenIdUri;
                txtWindowsLiveID.Text = siteUser.WindowsLiveId;
                txtLiveMessengerCID.Text = siteUser.LiveMessengerId;
                chkEnableLiveMessengerOnProfile.Checked = siteUser.EnableLiveMessengerOnProfile;
                gravatar1.Email = siteUser.Email;
                gravatar1.MaxAllowedRating = MaxAllowedGravatarRating;

                if (siteUser.LastActivityDate > DateTime.MinValue)
                {
                    this.lblLastActivityDate.Text = siteUser.LastActivityDate.AddHours(TimeOffset).ToString();
                }

                if (siteUser.LastLoginDate > DateTime.MinValue)
                {
                    this.lblLastLoginDate.Text = siteUser.LastLoginDate.AddHours(TimeOffset).ToString();
                }

                if (siteUser.LastPasswordChangedDate > DateTime.MinValue)
                {
                    this.lblLastPasswordChangeDate.Text = siteUser.LastPasswordChangedDate.AddHours(TimeOffset).ToString();
                }

                if (siteUser.LastLockoutDate > DateTime.MinValue)
                {
                    this.lblLastLockoutDate.Text = siteUser.LastLockoutDate.AddHours(TimeOffset).ToString();
                }
                this.lblFailedPasswordAttemptCount.Text = siteUser.FailedPasswordAttemptCount.ToString();
                this.lblFailedPasswordAnswerAttemptCount.Text = siteUser.FailedPasswordAnswerAttemptCount.ToString();
                this.chkIsLockedOut.Checked = siteUser.IsLockedOut;
                btnLockUser.Visible = !siteUser.IsLockedOut;
                btnUnlockUser.Visible = siteUser.IsLockedOut;

                if (siteSettings.UseSecureRegistration)
                {
                    if (siteUser.RegisterConfirmGuid == Guid.Empty)
                    {
                        chkEmailIsConfirmed.Checked = true;
                        btnConfirmEmail.Enabled = false;
                    }
                }
                else
                {
                    divEmailConfirm.Visible = false;
                }
                
                this.txtComment.Text = siteUser.Comment;
                this.txtPasswordQuestion.Text = siteUser.PasswordQuestion;
                this.txtPasswordAnswer.Text = siteUser.PasswordAnswer;

               
                if (!siteSettings.UseLdapAuth)
                {
                    if (siteSettings.PasswordFormat == 0)
                    { //Clear
                        this.txtPassword.Text = siteUser.Password;

                    }
                    else if (siteSettings.PasswordFormat == 2)
                    {
                        try
                        {
                            CMembershipProvider CMembership = (CMembershipProvider)Membership.Provider;
                            this.txtPassword.Text = CMembership.UnencodePassword(siteUser.Password, MembershipPasswordFormat.Encrypted);
                        }
                        catch (FormatException ex)
                        {
                            log.Error("Error decoding password for user " + siteUser.Email + " on manage users page.", ex);
                            // TODO: should we generate a random password and fix it here?
                        }
                    }

                }

                
               
                lblCreatedDate.Text = siteUser.DateCreated.AddHours(TimeOffset).ToString();
                lblUserGuid.Text = siteUser.UserGuid.ToString();
                lblTotalPosts.Text = siteUser.TotalPosts.ToString();
                lnkUserPosts.UserId = siteUser.UserId;
                lnkUserPosts.TotalPosts = siteUser.TotalPosts;
                lnkUnsubscribeFromGroups.NavigateUrl = SiteRoot + "/Groups/UnsubscribeGroup.aspx?ue=" + Page.Server.UrlEncode(siteUser.Email);

                chkProfileApproved.Checked = siteUser.ProfileApproved;
                chkApprovedForGroups.Checked = siteUser.ApprovedForGroups;
                chkTrusted.Checked = siteUser.Trusted;
                chkDisplayInMemberList.Checked = siteUser.DisplayInMemberList;

                //ListItem listItem;

                if ((!allowGravatars)&&(!disableAvatars))
                {
                    if (siteUser.AvatarUrl.Length > 0)
                    {
                        //listItem = ddAvatars.Items.FindByValue(siteUser.AvatarUrl);
                        //if (listItem != null)
                        //{
                        //    ddAvatars.ClearSelection();
                        //    listItem.Selected = true;

                        //}

                        imgAvatar.Src = ImageSiteRoot + "/Data/Sites/"
                            + siteSettings.SiteId.ToInvariantString() + "/useravatars/" + siteUser.AvatarUrl;
                    }
                    else
                    {
                        imgAvatar.Src = ImageSiteRoot + "/Data/SiteImages/1x1.gif";
                           
                    }
                }

                using (IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, siteUser.UserId))
                {
                    userRoles.DataSource = reader;
                    userRoles.DataBind();
                }

                using (IDataReader reader = Role.GetRolesUserIsNotIn(siteSettings.SiteId, siteUser.UserId))
                {
                    allRoles.DataSource = reader;
                    allRoles.DataBind();
                }

                if (allRoles.Items.Count == 0)
                {
                    allRoles.Enabled = false;
                    addExisting.Enabled = false;
                    addExisting.Text = Resource.ManageUsersUserIsInAllRolesMessage;
                   
                }

                List<UserLocation> userLocations = UserLocation.GetByUser(siteUser.UserGuid);
                grdUserLocation.DataSource = userLocations;
                grdUserLocation.DataBind();

			}
			else
			{
                spnTitle.InnerText = Resource.ManageUsersAddUserLabel;
				HideExtendedProfileControls();
			}

		}

        private void HideNonAdminControls()
        {
            
            liRoles.Visible = false;
            tabRoles.Visible = false;
            divDisplayInMemberList.Visible = false;
            liProfile.Visible = false;
            tabProfile.Visible = false;
            liLocation.Visible = false;
            tabLocation.Visible = false;
            liOrderHistory.Visible = false;
            tabOrderHistory.Visible = false;
        }

        private void PopulateProfileControls()
        {
            if (siteUser == null) { return; }
            if (!isAdmin) { return; }
            
            CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {

                    if (
                        (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                        ||(siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            CProfilePropertyDefinition.SetupPropertyControl(
                                this,
                                pnlProfileProperties,
                                propertyDefinition,
                                propValue.ToString(),
                                TimeOffset,
                                SiteRoot);
                        }
                        else
                        {
                            CProfilePropertyDefinition.SetupPropertyControl(
                                this,
                                pnlProfileProperties,
                                propertyDefinition,
                                propertyDefinition.DefaultValue,
                                TimeOffset,
                                SiteRoot);
                        }
                    }
                }
            }

           
        }

        //void btnUploadAvatar_Click(object sender, EventArgs e)
        //{
        //    if (siteUser == null) { return; }

        //    if (avatarFile != null && avatarFile.FileName != null && avatarFile.FileName.Trim().Length > 0)
        //    {
        //        string destFolder = Server.MapPath("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/");

        //        if (!Directory.Exists(destFolder)) { Directory.CreateDirectory(destFolder); }
        //        string newFileName = "user" + siteUser.UserId.ToInvariantString() + Path.GetExtension(avatarFile.FileName).ToLower();

        //        string destPath = Path.Combine(destFolder, newFileName);
        //        string ext = Path.GetExtension(avatarFile.FileName);
        //        if (SiteUtils.IsAllowedUploadBrowseFile(ext, SiteUtils.ImageFileExtensions()))
        //        {
        //            if (File.Exists(destPath))
        //            {
        //                File.Delete(destPath);
        //            }
        //            avatarFile.MoveTo(destPath, MoveToOptions.Overwrite);
        //            avatarFile.Dispose();

        //            if (WebConfigSettings.ForceSquareAvatars)
        //            {
        //                ImageHelper.ResizeAndSquareImage(destPath, IOHelper.GetMimeType(ext), WebConfigSettings.AvatarMaxWidth);
        //            }
        //            else
        //            {
        //                ImageHelper.ResizeImage(destPath, IOHelper.GetMimeType(ext), WebConfigSettings.AvatarMaxWidth, WebConfigSettings.AvatarMaxHeight);
        //            }

        //            siteUser.AvatarUrl = newFileName;
        //            siteUser.Save();


        //        }
        //        else
        //        {
        //            avatarFile.Dispose();
        //        }
        //    }

        //    WebUtils.SetupRedirect(this, Request.RawUrl);
        //}

     
        private void btnUpdate_Click(Object sender, EventArgs e)
		{
            Page.Validate();
            if (Page.IsValid)
            {
                if (this.userID > -1)
                {
                    UpdateUser();
                }
                else
                {
                    CreateUser();
                }
            }
			
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if((siteUser != null)&&(this.userID > -1))
			{
                siteUser.DeleteUser();
				WebUtils.SetupRedirect(this, SiteRoot + "/MemberList.aspx");
                return;

			}

		}

		private void UpdateUser()
		{
            if (siteUser == null) { return; }

            
            if (
                (siteUser.Email != txtEmail.Text)
                && (SiteUser.EmailExistsInDB(siteSettings.SiteId, txtEmail.Text))
                )
            {
                lblErrorMessage.Text = Resource.DuplicateEmailMessage;
                return;
            }

            if (
                (siteUser.LoginName != txtLoginName.Text)
                && (SiteUser.LoginExistsInDB(siteSettings.SiteId, txtLoginName.Text))
                )
            {
                lblErrorMessage.Text = Resource.DuplicateUserNameMessage;
                return;
            }

            siteUser.Name = txtName.Text;
            siteUser.LoginName = txtLoginName.Text;
            siteUser.Email = txtEmail.Text;

            if (divOpenID.Visible)
            {
                siteUser.OpenIdUri = txtOpenIDURI.Text;
            }

            if (!siteSettings.UseLdapAuth)
            {
                if (txtPassword.Text.Length > 0)
                {
                    CMembershipProvider CMembership = (CMembershipProvider) Membership.Provider;
                    siteUser.Password = CMembership.EncodePassword(txtPassword.Text, siteSettings);
                }
            }

            siteUser.ProfileApproved = chkProfileApproved.Checked;
            siteUser.ApprovedForGroups = chkApprovedForGroups.Checked;
            siteUser.Trusted = chkTrusted.Checked;
            siteUser.DisplayInMemberList = chkDisplayInMemberList.Checked;
            //siteUser.AvatarUrl = ddAvatars.SelectedValue;

            // this could also be in profile system
            siteUser.Comment = this.txtComment.Text;
            siteUser.PasswordQuestion = this.txtPasswordQuestion.Text;
            siteUser.PasswordAnswer = this.txtPasswordAnswer.Text;
            siteUser.WindowsLiveId = txtWindowsLiveID.Text;
            siteUser.LiveMessengerId = txtLiveMessengerCID.Text;
            siteUser.EnableLiveMessengerOnProfile = chkEnableLiveMessengerOnProfile.Checked;

            if (siteUser.Save())
            {
                CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();

                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {

                    CProfilePropertyDefinition.SaveProperty(
                        siteUser,
                        pnlProfileProperties,
                        propertyDefinition,
                        TimeOffset);
                }

                
                if ((currentUser != null) && (currentUser.UserId == siteUser.UserId))
                {
                    if ((siteSettings.UseEmailForLogin) && (siteUser.Email != currentUser.Email))
                    {
                        FormsAuthentication.SetAuthCookie(siteUser.Email, false);
                    }

                    if ((!siteSettings.UseEmailForLogin) && (siteUser.LoginName != currentUser.LoginName))
                    {
                        FormsAuthentication.SetAuthCookie(siteUser.LoginName, false);
                    }

                }

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            
		}

		private void CreateUser()
		{
			
            if (SiteUser.EmailExistsInDB(siteSettings.SiteId, txtEmail.Text))
            {
                lblErrorMessage.Text = Resource.DuplicateEmailMessage;
                return ;
            }

            if (SiteUser.LoginExistsInDB(siteSettings.SiteId, txtLoginName.Text))
            {
                lblErrorMessage.Text = Resource.DuplicateUserNameMessage;
                return;
            }

            SiteUser user = new SiteUser(siteSettings);
			user.Name = txtName.Text;
			user.LoginName = txtLoginName.Text;
			user.Email = txtEmail.Text;

            CMembershipProvider CMembership = (CMembershipProvider)Membership.Provider;
		    user.Password = CMembership.EncodePassword(txtPassword.Text, siteSettings);

			if(user.Save())
			{
                user.PasswordQuestion = this.txtPasswordQuestion.Text;
                user.PasswordAnswer = this.txtPasswordAnswer.Text;
                user.Save();

                CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
                // set default values
                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    CProfilePropertyDefinition.SavePropertyDefault(user, propertyDefinition);
                }

                CacheHelper.TouchMembershipStatisticsCacheDependencyFile();

                UserRegisteredEventArgs u = new UserRegisteredEventArgs(user);
                OnUserRegistered(u);

				WebUtils.SetupRedirect(this, SiteRoot 
                    + "/Admin/ManageUsers.aspx?userId=" + user.UserId.ToString() 
					+ "&username=" + user.Email + "&pageid=" + pageID);
				return;
 
			}

		}

        protected void OnUserRegistered(UserRegisteredEventArgs e)
        {
            foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
            {
                handler.UserRegisteredHandler(null, e);
            }

           
        }

        protected void btnUnlockUser_Click(object sender, EventArgs e)
        {
            if (this.userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                user.UnlockAccount();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
        }

        protected void btnLockUser_Click(object sender, EventArgs e)
        {
            if (this.userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                user.LockoutAccount();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
        }

		private void AddRole_Click(Object sender, EventArgs e) 
		{
            if (this.userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                int roleID = int.Parse(allRoles.SelectedItem.Value, CultureInfo.InvariantCulture);
                Role role = new Role(roleID);
                Role.AddUser(roleID, userID, role.RoleGuid, user.UserGuid);
                
            }
			
			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


        private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e) 
		{
            int roleID = Convert.ToInt32(userRoles.DataKeys[e.Item.ItemIndex]);
			Role.RemoveUser(roleID,userID);
            userRoles.EditItemIndex = -1;
			WebUtils.SetupRedirect(this, Request.RawUrl);
			return;

        }


        void userRoles_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnRemoveRole = e.Item.FindControl("btnRemoveRole") as ImageButton;
            UIHelper.AddConfirmationDialog(btnRemoveRole, Resource.ManageUsersRemoveRoleWarning);
        }


        void btnConfirmEmail_Click(object sender, EventArgs e)
        {
            if (this.userID > 0)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                SiteUser.ConfirmRegistration(user.RegisterConfirmGuid);
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
            
        }

        private void PopulateLabels()
        {

            if (userID > -1)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ManageUsersPageTitle);
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ManageUsersAddUserLabel);
            }

            litSecurityTab.Text = Resource.ManageUsersSecurityTab;
            litProfileTab.Text = Resource.ManageUsersProfileTab;
            litOrderHistoryTab.Text = Resource.CommerceOrderHistoryTab;
            litNewsletterTab.Text = Resource.ManageUsersNewslettersTab;
            litRolesTab.Text = Resource.ManageUsersRolesTab;
            litActivityTab.Text = Resource.ManageUsersActivityTab;
            litLocationTab.Text = Resource.ManageUsersLocationTab;

            lnkProfile.HRef = "#" + tabProfile.ClientID;
            lnkOrderHistory.HRef = "#" + tabOrderHistory.ClientID;
            lnkNewsletter.HRef = "#" + tabNewsletters.ClientID;
            lnkRoles.HRef = "#" + tabRoles.ClientID;
            lnkActivity.HRef = "#" + tabActivity.ClientID;
            lnkLocation.HRef = "#" + tabLocation.ClientID;

            liNewsletters.Visible = WebConfigSettings.EnableNewsletter;
            tabNewsletters.Visible = liNewsletters.Visible;
            newsLetterPrefs.Visible = liNewsletters.Visible;

            btnUnlockUser.Text = Resource.UserUnlockUserButton;
            btnUnlockUser.ToolTip = Resource.UserUnlockUserButton;
            SiteUtils.SetButtonAccessKey(btnUnlockUser, AccessKeys.UserUnlockUserButtonAccessKey);

            btnLockUser.Text = Resource.UserLockUserButton;
            btnLockUser.ToolTip = Resource.UserLockUserButton;
            SiteUtils.SetButtonAccessKey(btnLockUser, AccessKeys.UserLockUserButtonAccessKey);

            btnConfirmEmail.Text = Resource.ManageUsersConfirmEmailButton;

            btnUpdate.Text = Resource.ManageUsersUpdateButton;
            btnUpdate.ToolTip = Resource.ManageUsersUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, AccessKeys.ManageUsersUpdateButtonAccessKey);

            btnDelete.Text = Resource.ManageUsersDeleteButton;
            btnDelete.ToolTip = Resource.ManageUsersDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.ManageUsersDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.ManageUsersDeleteUserWarning);

            addExisting.Text = Resource.ManageUsersAddToRoleButton;
            addExisting.ToolTip = Resource.ManageUsersAddToRoleButton;
            SiteUtils.SetButtonAccessKey(addExisting, AccessKeys.ManageUsersAddToRoleButtonAccessKey);

            lnkUnsubscribeFromGroups.Text = Resource.ManageUsersUnsubscribeGroupsLink;

            lnkAvatarUpload.Text = Resource.UploadAvatarLink;

            //regexAvatarFile.ErrorMessage = Resource.FileTypeNotAllowed;
            //regexAvatarFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(SiteUtils.ImageFileExtensions());

            if (!(this.userID > 0))
            {
                this.btnUnlockUser.Enabled = false;
            }

            if (!IsPostBack)
            {
                //if ((!allowGravatars)&&(!disableOldAvatars))
                //{
                //    ddAvatars.DataSource = SiteUtils.GetAvatarList(this.siteSettings);
                //    ddAvatars.DataBind();

                //    ddAvatars.Items.Insert(0, new ListItem(Resource.UserProfileNoAvatarLabel, "blank.gif"));
                //    ddAvatars.Attributes.Add("onChange", "javascript:showAvatar(this);");
                //    ddAvatars.Attributes.Add("size", "6");
                //}

                txtPasswordQuestion.Text = Resource.ManageUsersDefaultSecurityQuestion;
                txtPasswordAnswer.Text = Resource.ManageUsersDefaultSecurityAnswer;
            }

            rfvName.ErrorMessage = Resource.UserProfileNameRequired;
            rfvLoginName.ErrorMessage = Resource.UserProfileLoginNameRequired;
            regexEmail.ErrorMessage = Resource.UserProfileEmailValidation;
            rfvEmail.ErrorMessage = Resource.UserProfileEmailRequired;

            if (siteSettings.UseLdapAuth)
            {
                this.divPassword.Visible = false;
            }

            if (allowGravatars)
            {
                gravatar1.Visible = true;
                //ddAvatars.Visible = false;
                imgAvatar.Visible = false;
                avatarHelp.Visible = false;
                lnkAvatarUpload.Visible = false;
                //avatarFile.Visible = false;
                //btnUploadAvatar.Visible = false;
                //progressBar.Visible = false;
                //regexAvatarFile.Visible = false;
                //regexAvatarFile.Enabled = false;
            }
            else
            {
                gravatar1.Visible = false;

                if (disableAvatars)
                {
                    divAvatarUrl.Visible = false;
                    imgAvatar.Visible = false;
                    lnkAvatarUpload.Visible = false;
                    avatarHelp.Visible = false;
                    //avatarFile.Visible = false;
                    //btnUploadAvatar.Visible = false;
                    //progressBar.Visible = false;
                    //regexAvatarFile.Visible = false;
                    //regexAvatarFile.Enabled = false;
                }
                else
                {
                    //lblMaxAvatarSize.Text = string.Format(Resource.AvatarMaxSizeLabelFormat, WebConfigSettings.AvatarMaxWidth.ToInvariantString(), WebConfigSettings.AvatarMaxHeight.ToInvariantString());
                    imgAvatar.Visible = true;
                    avatarHelp.Visible = true;
                    //avatarFile.Visible = true;
                    //btnUploadAvatar.Visible = true;
                    //progressBar.Visible = true;
                    //regexAvatarFile.Visible = true;
                    //regexAvatarFile.Enabled = true;
                }

            }

            //if (allowGravatars)
            //{
            //    ddAvatars.Visible = false;
            //    imgAvatar.Visible = false;
            //    avatarHelp.Visible = false;
            //}
            //else
            //{
            //    gravatar1.Visible = false;
            //    if (disableOldAvatars)
            //    {
            //        divAvatarUrl.Visible = false;
            //    }
                
            //}

            grdUserLocation.Columns[0].HeaderText = Resource.ManageUsersLocationGridIPAddressHeading;
            grdUserLocation.Columns[1].HeaderText = Resource.ManageUsersLocationGridHostnameHeading;
            grdUserLocation.Columns[2].HeaderText = Resource.ManageUsersLocationGridISPHeading;
            grdUserLocation.Columns[3].HeaderText = Resource.ManageUsersLocationGridContinentHeading;
            grdUserLocation.Columns[4].HeaderText = Resource.ManageUsersLocationGridCountryHeading;
            grdUserLocation.Columns[5].HeaderText = Resource.ManageUsersLocationGridRegionHeading;
            grdUserLocation.Columns[6].HeaderText = Resource.ManageUsersLocationGridCityHeading;
            grdUserLocation.Columns[7].HeaderText = Resource.ManageUsersLocationGridTimeZoneHeading;
            grdUserLocation.Columns[8].HeaderText = Resource.ManageUsersLocationGridCaptureCountHeading;
            grdUserLocation.Columns[9].HeaderText = Resource.ManageUsersLocationGridFirstCaptureHeading;
            grdUserLocation.Columns[10].HeaderText = Resource.ManageUsersLocationGridLastCaptureHeading;
            
           
        }

        protected void HideExtendedProfileControls()
        {
            tabActivity.Visible = false;
            tabProfile.Visible = false;
            tabRoles.Visible = false;
            liProfile.Visible = false;
            liOrderHistory.Visible = false;
            tabOrderHistory.Visible = false;
            liNewsletters.Visible = false;
            liRoles.Visible = false;
            liActivity.Visible = false;
            liLocation.Visible = false;


            divCreatedDate.Visible = false;
            divUserGuid.Visible = false;
            divTotalPosts.Visible = false;
            divProfileApproved.Visible = false;
            divApprovedForGroups.Visible = false;
            divTrusted.Visible = false;
            divDisplayInMemberList.Visible = false;
            divAvatarUrl.Visible = false;
            divRoles.Visible = false;
            divUserRoles.Visible = false;
            btnUpdate.Text = Resource.ManageUsersCreateButton;
            btnDelete.Visible = false;
            divLiveMessenger.Visible = false;
            
            this.divLastActivity.Visible = false;
            this.divLastLogin.Visible = false;
            this.divPasswordChanged.Visible = false;
            this.divLockoutDate.Visible = false;
            divEmailConfirm.Visible = false;
            this.divFailedPasswordAttempt.Visible = false;
            this.divFailedPasswordAnswerAttempt.Visible = false;
            this.divLockout.Visible = false;
            this.divComment.Visible = false;
            this.divOpenID.Visible = false;
            this.divWindowsLiveID.Visible = false;
            this.tabNewsletters.Visible = false;
            newsLetterPrefs.Visible = false;
            this.tabLocation.Visible = false;


        }

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();
            isAdmin = WebUser.IsAdmin;
            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            userID = WebUtils.ParseInt32FromQueryString("userid", true, userID);
            TimeOffset = SiteUtils.GetUserTimeOffset();
            userGuid = WebUtils.ParseGuidFromQueryString("u", Guid.Empty);

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = true;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    lnkAvatarUpload.NavigateUrl = SiteRoot + "/Dialog/AvatarUploadDialog.aspx?u=" + userID.ToInvariantString();
                    lnkAvatarUpload.ClientClick = "return GB_showPage('" + Page.Server.HtmlEncode(Resource.UploadAvatarAdminHeading) + "', this.href, GBCallback)";
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }


            lnkUnsubscribeFromGroups.Visible = WebConfigSettings.ShowGroupUnsubscribeLinkInUserManagement;

            if (WebConfigSettings.MaskPasswordsInUserAdmin)
            {
                txtPassword.TextMode = TextBoxMode.Password;
            }

            regexEmail.ValidationExpression = SecurityHelper.RegexEmailValidationPattern;

            if (WebConfigSettings.UseRelatedSiteMode)
            {
                divTotalPosts.Visible = false;
            }

            commerceConfig = SiteUtils.GetCommerceConfig();
            if (!commerceConfig.IsConfigured)
            {
                liOrderHistory.Visible = false;
                tabOrderHistory.Visible = false;
            }

            string wlAppId = siteSettings.WindowsLiveAppId;
            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
            {
                wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
                if (wlAppId.Length == 0) { wlAppId = siteSettings.WindowsLiveAppId; }
            }

            if (
                (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
                || (!siteSettings.AllowWindowsLiveMessengerForMembers)
                || (wlAppId.Length == 0)
                )
            {
                divLiveMessenger.Visible = false;
            }
           
        }

        private void SetupAvatarScript()
        {

            string callback = "<script type=\"text/javascript\">"
                + "function GBCallback() { "
                + " window.location.reload(true); "
                + "}</script>";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "gbcallback", callback);
        }
        

        //private void SetupAvatarScript()
        //{
        //    if (allowGravatars) { return; }

        //    string logoScript = "<script type=\"text/javascript\">"
        //        + "function showAvatar(listBox) { if(!document.images) return; "
        //        + "var avaterPath = '" + AvatarPath + "'; "
        //        + "document.images." + imgAvatar.ClientID + ".src = avaterPath + listBox.value;"
        //        + "}</script>";
            
        //    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showAvatar", logoScript);

        //}


        
	}
}




