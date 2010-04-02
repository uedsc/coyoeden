/// Author:				        Joe Audette
/// Created:			        2004-09-26
///	Last Modified:              2010-01-10
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web.Security;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Configuration;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;
using Brettle.Web.NeatUpload;
using Resources;

namespace Cynthia.Web.UI.Pages
{
	
    public partial class UserProfile : CBasePage
	{
        private string userEmail = string.Empty;
        private string avatarPath = string.Empty;
        private SiteUser siteUser;
        private Double timeOffset = 0;
        //Gravatar public enum RatingType { G, PG, R, X }
        private Gravatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        private bool allowGravatars = false;
        private bool disableAvatars = true;
        private CommerceConfiguration commerceConfig = null;
        private string rpxApiKey = string.Empty;
        private string rpxApplicationName = string.Empty;
        //private string avatarBasePath = string.Empty;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
           // btnUploadAvatar.Click += new EventHandler(btnUploadAvatar_Click);

            if (WebConfigSettings.HideAllMenusOnProfilePage)
            {
                SuppressAllMenus();
            }
            else if (WebConfigSettings.HidePageMenuOnProfilePage) { SuppressPageMenu(); }

            SuppressMenuSelection();
            SetupAvatarScript();
            siteUser = SiteUtils.GetCurrentSiteUser();
            LoadSettings();
            
            PopulateProfileControls();
            if (siteUser != null)
            {
                purchaseHx.UserGuid = siteUser.UserGuid;
            }

            ScriptConfig.IncludeYuiTabs = true;
            IncludeYuiTabsCss = true;

        }

        

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            
            
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (!WebConfigSettings.AllowUserProfilePage)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

			PopulateLabels();
            //SetupAvatarScript();

			if (!IsPostBack)
			{
				PopulateControls();
			}
		}
		

		private void PopulateControls()
		{
            
            this.lnkChangePassword.NavigateUrl = SiteRoot + "/Secure/ChangePassword.aspx";
            this.lnkChangePassword.Text = Resource.UserChangePasswordLabel;

            if (siteSettings.AllowUserSkins)
            {
                this.ddSkins.DataSource = SiteUtils.GetSkinList(this.siteSettings);
                this.ddSkins.DataBind();

                ListItem listItem;
                listItem = this.ddSkins.Items.FindByValue("printerfriendly");
                if (listItem != null)
                {
                    this.ddSkins.Items.Remove(listItem);
                }

                listItem = this.ddSkins.Items.FindByValue(".svn");
                if (listItem != null)
                {
                    this.ddSkins.Items.Remove(listItem);
                }

                listItem = new ListItem();
                listItem.Value = "";
                listItem.Text = Resource.PageLayoutDefaultSkinLabel;
                this.ddSkins.Items.Insert(0, listItem);

                if (siteUser != null)
                {
                    if (siteUser.Skin.Length > 0)
                    {
                        listItem = ddSkins.Items.FindByValue(siteUser.Skin);
                        if (listItem != null)
                        {
                            ddSkins.ClearSelection();
                            listItem.Selected = true;
                        }

                    }
                }
            }
			
			if(siteUser != null)
			{
                txtName.Text = siteUser.Name;
                lblLoginName.Text = siteUser.LoginName;
                txtEmail.Text = siteUser.Email;
                gravatar1.Email = siteUser.Email;
                lblOpenID.Text = siteUser.OpenIdUri;
                txtPasswordQuestion.Text = siteUser.PasswordQuestion;
                txtPasswordAnswer.Text = siteUser.PasswordAnswer;
                lblCreatedDate.Text = siteUser.DateCreated.AddHours(timeOffset).ToString();
                lblTotalPosts.Text = siteUser.TotalPosts.ToString();
                lnkUserPosts.UserId = siteUser.UserId;
                lnkUserPosts.TotalPosts = siteUser.TotalPosts;
                lnkPublicProfile.NavigateUrl = SiteRoot + "/ProfileView.aspx?userid=" + siteUser.UserId.ToString(CultureInfo.InvariantCulture);

                if (divLiveMessenger.Visible)
                {
                    WindowsLiveLogin wl = WindowsLiveHelper.GetWindowsLiveLogin();
                    WindowsLiveMessenger m = new WindowsLiveMessenger(wl);

                    if (WebConfigSettings.TestLiveMessengerDelegation)
                    {
                        lnkAllowLiveMessenger.NavigateUrl = m.ConsentOptInUrl;
                    }
                    else
                    {
                        lnkAllowLiveMessenger.NavigateUrl = m.NonDelegatedSignUpUrl;
                    }

                    if (siteUser.LiveMessengerId.Length > 0)
                    {
                        chkEnableLiveMessengerOnProfile.Checked = siteUser.EnableLiveMessengerOnProfile;
                        chkEnableLiveMessengerOnProfile.Enabled = true;
                    }
                    else
                    {
                        chkEnableLiveMessengerOnProfile.Checked = false;
                        chkEnableLiveMessengerOnProfile.Enabled = false;
                    }
                }


                if ((!allowGravatars)&&(!disableAvatars))
                {
                    if (siteUser.AvatarUrl.Length > 0)
                    {
                        //if (!WebConfigSettings.OnlyAdminsCanEditCheesyAvatars)
                        //{
                        //    ddAvatars.SelectedValue = siteUser.AvatarUrl;
                        //}
                        
                        imgAvatar.Src = avatarPath + siteUser.AvatarUrl;
                    }
                    else
                    {
                        imgAvatar.Src = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
                    }
                }

                
			}
            

            DoTabSelection();

            
			
		}

        private void PopulateProfileControls()
        {
            if (siteUser == null) { return; }
            
            CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (
                         (propertyDefinition.VisibleToUser)
                      &&    (
                            (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                            ||(siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            if (propertyDefinition.EditableByUser)
                            {
                                CProfilePropertyDefinition.SetupPropertyControl(
                                    this,
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propValue.ToString(),
                                    timeOffset,
                                    SiteRoot);
                            }
                            else
                            {
                                CProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propValue.ToString(),
                                    timeOffset);
                            }
                        }
                        else
                        {
                            if (propertyDefinition.EditableByUser)
                            {
                                CProfilePropertyDefinition.SetupPropertyControl(
                                    this,
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propertyDefinition.DefaultValue,
                                    timeOffset,
                                    SiteRoot);
                            }
                            else
                            {
                                CProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propertyDefinition.DefaultValue,
                                    timeOffset);
                            }

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
			if(siteUser != null)
			{
				Page.Validate();
				if(Page.IsValid)
				{
					UpdateUser();
				}
			}
			
			
		}

		private void UpdateUser()
		{
            userEmail = siteUser.Email;

            if (
                (siteUser.Email != txtEmail.Text)
                && (SiteUser.EmailExistsInDB(siteSettings.SiteId, txtEmail.Text))
                )
            {
                lblErrorMessage.Text = Resource.DuplicateEmailMessage;
                return;
            }

            siteUser.Name = txtName.Text;
            siteUser.Email = txtEmail.Text;
            if (pnlSecurityQuestion.Visible)
            {
                siteUser.PasswordQuestion = this.txtPasswordQuestion.Text;
                siteUser.PasswordAnswer = this.txtPasswordAnswer.Text;
            }
            else
            {
                //in case it is ever changed later to require password question and answer after making it not required
                // we need to ensure there is some question and answer.
                if (siteUser.PasswordQuestion.Length == 0) 
                { 
                    siteUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
                    siteUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
                }


            }


            if (siteUser.LiveMessengerId.Length > 0)
            {
                siteUser.EnableLiveMessengerOnProfile = chkEnableLiveMessengerOnProfile.Checked;
            }
            else
            {
                siteUser.EnableLiveMessengerOnProfile = false;
            }

			if(siteSettings.AllowUserSkins)
			{
                if (ddSkins.SelectedValue != "printerfriendly")
                {
                    siteUser.Skin = ddSkins.SelectedValue;
                }
			}

            //if ((!disableOldAvatars)&&(!WebConfigSettings.OnlyAdminsCanEditCheesyAvatars))
            //{ siteUser.AvatarUrl = ddAvatars.SelectedValue; }
            
            if (siteUser.Save())
			{
                CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();

                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (
                        (propertyDefinition.EditableByUser)
                         && (
                              (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                              ||(WebUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        CProfilePropertyDefinition.SaveProperty(
                            siteUser, 
                            pnlProfileProperties, 
                            propertyDefinition,
                            timeOffset);
                    }
                }

                siteUser.UpdateLastActivityTime();
                if ((userEmail != siteUser.Email)&&(siteSettings.UseEmailForLogin))
				{
                    FormsAuthentication.SetAuthCookie(siteUser.Email, false);
				}

                SiteUtils.SetSkinCookie(siteUser);
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}

		}

        

        private void LoadSettings()
        {
            
            avatarPath = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/");

            //lnkAvatarUpload.Visible = false;

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = true;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    if (siteUser != null)
                    {
                        lnkAvatarUpload.NavigateUrl = SiteRoot + "/Dialog/AvatarUploadDialog.aspx?u=" + siteUser.UserId.ToInvariantString();
                        lnkAvatarUpload.ClientClick = "return GB_showPage('" + Page.Server.HtmlEncode(Resource.UploadAvatarHeading) + "', this.href, GBCallback)";
                    }

                    if (WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)
                    {
                        lnkAvatarUpload.Visible = false;
                        //lblMaxAvatarSize.Visible = false;
                        //avatarFile.Visible = false;
                       // progressBar.Visible = false;
                        //btnUploadAvatar.Visible = false;
                       // regexAvatarFile.Visible = false;
                       // regexAvatarFile.Enabled = false;
                        avatarHelp.Visible = false;
                    }

                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            

            if (siteSettings.AllowUserSkins)
            {
                this.divSkin.Visible = true;
            }
            else
            {
                this.divSkin.Visible = false;
            }

            if (siteSettings.UseLdapAuth)
            {
                this.lnkChangePassword.Visible = false;
            }

            timeOffset = SiteUtils.GetUserTimeOffset();

            divOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth;

            rpxApiKey = siteSettings.RpxNowApiKey;
            rpxApplicationName = siteSettings.RpxNowApplicationName;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
                {
                    rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
                }

                if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
                {
                    rpxApplicationName = WebConfigSettings.OpenIdRpxApplicationName;
                }

            }

            if (rpxApiKey.Length > 0)
            {
                lnkOpenIDUpdate.Visible = false;
                rpxLink.Visible = divOpenID.Visible;
            }

            if (WebConfigSettings.RunningInMediumTrust)
            {
                divOpenID.Visible = false;
            }

            if (
                (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
                ||(!siteSettings.AllowWindowsLiveMessengerForMembers)
                || ((siteSettings.WindowsLiveAppId.Length == 0) && (ConfigurationManager.AppSettings["GlobalWindowsLiveAppKey"] == null))
                )
            {
                divLiveMessenger.Visible = false;
            }

            int countOfNewsLetters = LetterInfo.GetCount(siteSettings.SiteGuid);

            liNewsletters.Visible = (WebConfigSettings.EnableNewsletter && (countOfNewsLetters > 0));
            tabNewsletters.Visible = liNewsletters.Visible;
            newsLetterPrefs.Visible = liNewsletters.Visible;

            commerceConfig = SiteUtils.GetCommerceConfig();
            if (!commerceConfig.IsConfigured)
            {
                liOrderHistory.Visible = false;
                tabOrderHistory.Visible = false;

            }

        }

       
        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ProfileLink);
            litSecurityTab.Text = Resource.UserProfileSecurityTab;
            litProfileTab.Text = Resource.UserProfileProfileTab;
            litOrderHistoryTab.Text = Resource.CommerceOrderHistoryTab;
            litNewsletterTab.Text = Resource.UserProfileNewslettersTab;
            lnkProfile.HRef = "#" + tabProfile.ClientID;
            lnkNewsletter.HRef = "#" + tabNewsletters.ClientID;
            lnkOrderHistory.HRef = "#" + tabOrderHistory.ClientID;

            lnkAllowLiveMessenger.Text = Resource.EnableLiveMessengerLink;

            btnUpdate.Text = Resource.UserProfileUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, AccessKeys.UserProfileSaveButtonAccessKey);

            

            //if ((!allowGravatars)&&(!disableOldAvatars)&&(!Page.IsPostBack))
            //{
            //    ddAvatars.DataSource = SiteUtils.GetAvatarList(this.siteSettings);
            //    ddAvatars.DataBind();

            //    ddAvatars.Items.Insert(0, new ListItem(Resource.UserProfileNoAvatarLabel, "blank.gif"));
            //    ddAvatars.Attributes.Add("onChange", "javascript:showAvatar(this);");
            //    ddAvatars.Attributes.Add("size", "6");
            //}

            lnkPublicProfile.DialogCloseText = Resource.DialogCloseLink;
            lnkPublicProfile.Text = Resource.PublicProfileLink;

            rfvName.ErrorMessage = Resource.UserProfileNameRequired;
            regexEmail.ErrorMessage = Resource.UserProfileEmailValidation;
            rfvEmail.ErrorMessage = Resource.UserProfileEmailRequired;

            //regexAvatarFile.ErrorMessage = Resource.FileTypeNotAllowed;
            //regexAvatarFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(SiteUtils.ImageFileExtensions());

            lnkAvatarUpload.Text = Resource.UploadAvatarLink;
           

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
                    divAvatar.Visible = false;
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
                    //lblMaxAvatarSize.Text = string.Format(Resource.AvatarMaxSizeLabelFormat, WebConfigSettings.AvatarMaxWidth.ToInvariantString(), WebConfigSettings.AvatarMaxHeight.ToInvariantString());
                    imgAvatar.Visible = true;

                    if (!WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)
                    {
                        avatarHelp.Visible = true;
                        //avatarFile.Visible = true;
                        //btnUploadAvatar.Visible = true;
                        //progressBar.Visible = true;
                        //regexAvatarFile.Visible = true;
                        //regexAvatarFile.Enabled = true;
                    }
                }
               
            }

            lnkOpenIDUpdate.Text = Resource.OpenIDUpdateButton;
            lnkOpenIDUpdate.ToolTip = Resource.OpenIDUpdateButton;
            lnkOpenIDUpdate.NavigateUrl = SiteRoot + "/Secure/UpdateOpenID.aspx";

            rpxLink.OverrideText = Resource.OpenIDUpdateButton;

            pnlSecurityQuestion.Visible = siteSettings.RequiresQuestionAndAnswer;
            

        }

        private void DoTabSelection()
        {
            

            if (siteUser == null)
            {
                liSecurity.Attributes.Add("class", "selected");
                return; 
            }

            string t = string.Empty;

            if (Request.QueryString["t"] != null) { t = Request.QueryString["t"]; }

            switch (t)
            {
                case "i":

                    liSecurity.Attributes.Add("class", "selected");
                    break;

                case "o":

                    if (liOrderHistory.Visible)
                    {
                        liOrderHistory.Attributes.Add("class", "selected");
                    }
                    else
                    {
                        liSecurity.Attributes.Add("class", "selected");
                    }
                    break;

                case "p":

                    if (liProfile.Visible)
                    {
                        liProfile.Attributes.Add("class", "selected");
                    }
                    else
                    {
                        liSecurity.Attributes.Add("class", "selected");
                    }
                    break;

                default:

                    if (liNewsletters.Visible)
                    {
                        liNewsletters.Attributes.Add("class", "selected");
                    }
                    else
                    {
                        liSecurity.Attributes.Add("class", "selected");
                    }
                    
                    break;

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

	}
}
