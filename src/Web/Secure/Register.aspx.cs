///	Last Modified:              2009-12-19
/// 
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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Business.WebHelpers.UserRegisteredHandlers;
using Cynthia.Web.Configuration;
using Cynthia.Web.Framework;
using Resources;
using log4net;

namespace Cynthia.Web.UI.Pages
{
	
    public partial class Register : CBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Register));

        private string rpxApiKey = string.Empty;
        private string rpxApplicationName = string.Empty;
        private Panel pnlProfile;
        private Double timeOffset = 0;
        private bool showWindowsLive = false;
        private bool showOpenId = false;
        private bool showRpx = false;
        

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.RegisterUser.EnableTheming = false;
            this.AppendQueryStringToAction = false;
        }
        
        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.RegisterUser.CreatedUser += new EventHandler(RegisterUser_CreatedUser);
            this.RegisterUser.CreatingUser += new LoginCancelEventHandler(RegisterUser_CreatingUser);

            if (WebConfigSettings.HideMenusOnRegisterPage) { SuppressAllMenus(); }

           // HookupRegistrationEventHandlers();


        }

        #endregion

        

        private void Page_Load(object sender, EventArgs e)
		{
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();


            if (!siteSettings.AllowNewRegistration)
            {
                Response.Redirect(SiteRoot, false);
            }

            LoadSettings();
            PopulateLabels();

            if (Request.IsAuthenticated)
            {
                pnlRegisterWrapper.Visible = false;
                pnlAuthenticated.Visible = true;
                return;
            }

            PopulateRequiredProfileControls();
            if (!IsPostBack) SetInitialFocus();

		
		}

        private void SetInitialFocus()
        {
            if ((siteSettings.UseEmailForLogin) && (WebConfigSettings.AutoGenerateAndHideUserNamesWhenUsingEmailForLogin))
            {
                TextBox txtEmail
                    = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
                if (txtEmail != null)
                {
                    txtEmail.Focus();
                }
            }
            else
            {
                TextBox txtUserName
                    = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
                if (txtUserName != null)
                {
                    txtUserName.Focus();
                }
            }
        }

        private void PopulateRequiredProfileControls()
        {
            if (pnlProfile != null)
            {
                CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
                if (profileConfig != null)
                {
                    foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                    {
                        if (propertyDefinition.RequiredForRegistration)
                        {
                            CProfilePropertyDefinition.SetupPropertyControl(
                                this,
                                pnlProfile, 
                                propertyDefinition,
                                timeOffset,
                                SiteRoot);
                        }

                    }
                }

                
            }


        }


        void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                e.Cancel = true;
            }
        }


        
        void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            TextBox txtEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
            TextBox txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

            if (txtEmail == null) { return; }
            if (txtUserName == null) { return; }

            SiteUser siteUser;

            if (siteSettings.UseEmailForLogin)
            {
                siteUser = new SiteUser(siteSettings, txtEmail.Text);
            }
            else
            {
                siteUser = new SiteUser(siteSettings, txtUserName.Text);
            }

            if (siteUser.UserId == -1) return;

            if (pnlProfile != null)
            {
                CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();

                // set default values first
                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    CProfilePropertyDefinition.SavePropertyDefault(siteUser, propertyDefinition);
                }

                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (propertyDefinition.RequiredForRegistration)
                    {
                        CProfilePropertyDefinition.SaveProperty(
                            siteUser,
                            pnlProfile,
                            propertyDefinition,
                            timeOffset);
                    }
                }



            }

           
            // track user ip address
            UserLocation userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address());
            userLocation.SiteGuid = siteSettings.SiteGuid;
            userLocation.Hostname = Page.Request.UserHostName;
            userLocation.Save();

            CacheHelper.TouchMembershipStatisticsCacheDependencyFile();

            if (!siteSettings.UseSecureRegistration)
            {
                if (siteSettings.UseEmailForLogin)
                {
                    FormsAuthentication.SetAuthCookie(
                        siteUser.Email, false);

                }
                else
                {
                    FormsAuthentication.SetAuthCookie(
                        siteUser.LoginName, false);

                }

                if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                {
                    string cookieName = "siteguid" + siteSettings.SiteGuid;
                    CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), false);
                }

                siteUser.UpdateLastLoginTime();


            }

            

            UserRegisteredEventArgs u = new UserRegisteredEventArgs(siteUser);
            OnUserRegistered(u);

        }

        #region Events

        //private void HookupRegistrationEventHandlers()
        //{
        //    // this is a hook so that custom code can be fired when pages are created
        //    // implement a PageCreatedEventHandlerPovider and put a config file for it in
        //    // /Setup/ProviderConfig/pagecreatedeventhandlers
        //    //try
        //    //{
        //    //    foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
        //    //    {
        //    //        this.UserRegistered += handler.UserRegisteredHandler;
        //    //    }
        //    //}
        //    //catch (TypeInitializationException ex)
        //    //{
        //    //    log.Error(ex);
        //    //}

        //}

        //public event UserRegistreredEventHandler UserRegistered;

        protected void OnUserRegistered(UserRegisteredEventArgs e)
        {
            foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
            {
                handler.UserRegisteredHandler(null, e);
            }

            //if (UserRegistered != null)
            //{
            //    UserRegistered(this, e);
            //}
        }

        #endregion

        void PasswordRulesValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator validator = source as CustomValidator;
            validator.ErrorMessage = string.Empty;

            if (args.Value.Length < Membership.MinRequiredPasswordLength)
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += Resource.RegisterPasswordMinLengthWarning
                    + Membership.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture) + "<br />";
            }

            if (!HasEnoughNonAlphaNumericCharacters(args.Value))
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += Resource.RegisterPasswordMinNonAlphaCharsWarning
                    + Membership.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture) + "<br />";

            }

        }

        private bool HasEnoughNonAlphaNumericCharacters(string newPassword)
        {
            bool result = false;
            string alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] passwordChars = newPassword.ToCharArray();
            int nonAlphaNumericCharCount = 0;
            foreach (char c in passwordChars)
            {
                if (!alphanumeric.Contains(c.ToString()))
                {
                    nonAlphaNumericCharCount += 1;
                }
            }

            if (nonAlphaNumericCharCount >= Membership.MinRequiredNonAlphanumericCharacters)
            {
                result = true;
            }

            return result;
        }



        private void PopulateLabels()
        {
            this.RegisterUser.ContinueButtonStyle.Font.Bold = true;
            this.RegisterUser.CreateUserButtonStyle.Font.Bold = true;

           
            
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterLink);

            litAlreadyAuthenticated.Text = Resource.AlreadyRegisteredMessage;

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionRegistrationPageFormat, siteSettings.SiteName);

          
            

            RequiredFieldValidator userNameRequired
                = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserNameRequired");

            userNameRequired.ErrorMessage = Resource.RegisterLoginNameRequiredMessage;

            RequiredFieldValidator emailRequired
                = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRequired");

            emailRequired.ErrorMessage = Resource.RegisterEmailRequiredMessage;

            RegularExpressionValidator emailRegex
                = (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRegex");
            emailRegex.ValidationExpression = SecurityHelper.RegexEmailValidationPattern;
            emailRegex.ErrorMessage = Resource.RegisterEmailRegexMessage;

            CustomValidator passwordRulesValidator
                = (CustomValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRulesValidator");

          
            RequiredFieldValidator passwordRequired
                = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRequired");

            passwordRequired.ErrorMessage = Resource.RegisterPasswordRequiredMessage;

            // hookup event to handle validation
            passwordRulesValidator.ServerValidate += new ServerValidateEventHandler(PasswordRulesValidator_ServerValidate);


            RegularExpressionValidator passwordRegex
                = (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRegex");

            passwordRegex.ErrorMessage = Resource.RegisterPasswordRegexWarning;

            RequiredFieldValidator confirmPasswordRequired
                = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmPasswordRequired");

            confirmPasswordRequired.ErrorMessage = Resource.RegisterConfirmPasswordRequiredMessage;

            CompareValidator passwordCompare
                = (CompareValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordCompare");

            passwordCompare.ErrorMessage = Resource.RegisterComparePasswordWarning;

            RequiredFieldValidator questionRequired
                = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("QuestionRequired");

            questionRequired.ErrorMessage = Resource.RegisterSecurityQuestionRequiredMessage;

            RequiredFieldValidator answerRequired
                = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("AnswerRequired");

            answerRequired.ErrorMessage = Resource.RegisterSecurityAnswerRequiredMessage;

            this.RegisterUser.RequireEmail = true;

            this.RegisterUser.CreateUserButtonText = Resource.RegisterButton;
            this.RegisterUser.CancelButtonText = Resource.RegisterCancelButton;
            
            if (WebConfigSettings.UseShortcutKeys)
            {
                this.RegisterUser.AccessKey = AccessKeys.RegisterAccessKey;
                this.RegisterUser.CreateUserButtonText +=
                    SiteUtils.GetButtonAccessKeyPostfix(this.RegisterUser.AccessKey);
                this.RegisterUser.ContinueButtonText +=
                    SiteUtils.GetButtonAccessKeyPostfix(this.RegisterUser.AccessKey);
            }

            this.RegisterUser.InvalidQuestionErrorMessage = Resource.RegisterInvalidQuestionErrorMessage;
            this.RegisterUser.InvalidAnswerErrorMessage = Resource.RegisterInvalidAnswerErrorMessage;
            this.RegisterUser.InvalidEmailErrorMessage = Resource.RegisterEmailRegexMessage;

            this.RegisterUser.StartNextButtonText = Resource.RegisterButton;

            this.RegisterUser.DuplicateEmailErrorMessage
                = Resource.RegisterDuplicateEmailMessage;

            this.RegisterUser.DuplicateUserNameErrorMessage
                = Resource.RegisterDuplicateUserNameMessage;

            if (Membership.Provider.PasswordStrengthRegularExpression.Length == 0)
            {
                passwordRegex.Visible = false;
            }

            if (!Membership.Provider.RequiresQuestionAndAnswer)
            {
               
                HtmlContainerControl divQuestion
                = (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divQuestion");

                divQuestion.Visible = false;
                questionRequired.Visible = false;

                HtmlContainerControl divAnswer
                = (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divAnswer");

                divAnswer.Visible = false;
                answerRequired.Visible = false;


            }

            litOr.Text = Resource.LiteralOr;

            Button continueButton =
                (Button)CompleteWizardStep1.ContentTemplateContainer.FindControl("ContinueButton");

            continueButton.Text  = Resource.RegisterContinueButton;

            Literal completeMessage =
                (Literal)CompleteWizardStep1.ContentTemplateContainer.FindControl("CompleteMessage");

            completeMessage.Text = "";
            if (siteSettings.UseSecureRegistration)
            {
                this.RegisterUser.LoginCreatedUser = false;
                completeMessage.Text = Resource.RegistrationRequiresEmailConfirmationMessage;
            }
            else
            {
                this.RegisterUser.LoginCreatedUser = true;
                completeMessage.Text = Resource.RegisterCompleteMessage;

            }

            HtmlContainerControl divAgreement
                = (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divAgreement");

            Literal agreement = new Literal();
            agreement.Text = ResourceHelper.GetMessageTemplate("RegisterLicense.config");
            divAgreement.Controls.Add(agreement);

            
            lnkOpenIDRegistration.Text = Resource.OpenIDRegistrationLink;
            lnkOpenIDRegistration.ToolTip = Resource.OpenIDRegistrationLink;

            lnkWindowsLiveID.Text = Resource.WindowsLiveIDRegistrationLink;
            lnkWindowsLiveID.ToolTip = Resource.WindowsLiveIDRegistrationLink;

            litThirdPartyAuthHeading.Text = Resource.ThirdPartyRegistrationHeading;

            

            if ((siteSettings.UseEmailForLogin) && (WebConfigSettings.AutoGenerateAndHideUserNamesWhenUsingEmailForLogin))
            {
                Panel userNamePanel
                = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlUserName");

                TextBox txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

                if (userNamePanel != null) { userNamePanel.Attributes.Add("style", "display:none;"); }
                userNameRequired.Enabled = false;
                userNameRequired.Visible = false;
                if (txtUserName != null) { txtUserName.Text = "nothing"; }
            }

        }

        
        


        private void LoadSettings()
        {
            if (WebConfigSettings.AllowUserProfilePage)
            {
                this.RegisterUser.FinishDestinationPageUrl
                    = SiteRoot + "/Secure/UserProfile.aspx";

                this.RegisterUser.ContinueDestinationPageUrl
                    = SiteRoot + "/Secure/UserProfile.aspx";

                this.RegisterUser.EditProfileUrl = SiteRoot + "/Secure/UserProfile.aspx";
            }
            else
            {
                this.RegisterUser.FinishDestinationPageUrl = SiteRoot ;

                this.RegisterUser.ContinueDestinationPageUrl = SiteRoot;

                this.RegisterUser.EditProfileUrl = SiteRoot;

            }

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

            //string returnUrlParam = Page.Request.Params.Get("returnurl");
            //if (!String.IsNullOrEmpty(returnUrlParam))
            //{
            //    string redirectUrl = Page.ResolveUrl(Page.Server.UrlDecode(returnUrlParam));
            //    this.RegisterUser.FinishDestinationPageUrl = redirectUrl;
            //    this.RegisterUser.ContinueDestinationPageUrl = redirectUrl;

            //}

            

            if (ViewState["returnurl"] != null)
            {
                this.RegisterUser.ContinueDestinationPageUrl = ViewState["returnurl"].ToString();
            }

            if (Request.Params.Get("returnurl") != null)
            {
                string returnUrlParam = Page.Request.Params.Get("returnurl");
                if (!String.IsNullOrEmpty(returnUrlParam))
                {
                    returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);
                    string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam)));
                    if ((redirectUrl.StartsWith(SiteRoot)) || (redirectUrl.StartsWith(SiteRoot.Replace("https://", "http://"))))
                    {
                        this.RegisterUser.ContinueDestinationPageUrl = redirectUrl;
                    }
                }

                
            }

            timeOffset = SiteUtils.GetUserTimeOffset();

            if (WebConfigSettings.ShowCustomProfilePropertiesAboveManadotoryRegistrationFields)
            {
                pnlProfile = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlRequiredProfilePropertiesUpper");
            }
            else
            {
                pnlProfile = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlRequiredProfileProperties");
            }

            showRpx = ((!WebConfigSettings.DisableRpxAuthentication) && (rpxApiKey.Length > 0));
            
            showOpenId = (
                (WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth)
                
                );

            string wlAppId = siteSettings.WindowsLiveAppId;
            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
            {
                wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
                if (wlAppId.Length == 0) { wlAppId = siteSettings.WindowsLiveAppId; }
            }

            showWindowsLive
                = WebConfigSettings.EnableWindowsLiveAuthentication
                && siteSettings.AllowWindowsLiveAuth
                && (wlAppId.Length > 0);

            if (IsPostBack)
            {
                showOpenId = false;
                showWindowsLive = false;
                showRpx = false;
            }

            pnlThirdPartyAuth.Visible = (showOpenId || showWindowsLive || showRpx);
            divLiteralOr.Visible = (showOpenId && showWindowsLive);
            pnlOpenID.Visible = showOpenId;
            pnlWindowsLiveID.Visible = showWindowsLive;
            pnlRpx.Visible = showRpx;

            //if ((!WebConfigSettings.DisableRpxAuthentication)&&(rpxApiKey.Length > 0))
            //{
            //    //pnlOpenID.Visible = true;
            //    rpxLink.Visible = true;
            //    lnkOpenIDRegistration.Visible = false;
            //}

            if (siteSettings.DisableDbAuth) { pnlStandardRegister.Visible = false; }


        }
        
    
        
        
	}
}
