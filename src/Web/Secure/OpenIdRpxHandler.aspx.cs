// Author:					Joe Audette
// Created:					2009-05-15
// Last Modified:			2009-12-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.Security;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Business.WebHelpers.UserSignInHandlers;
using Cynthia.Business.WebHelpers.UserRegisteredHandlers;
using Cynthia.Net;
using Cynthia.Web.Configuration;
using Cynthia.Web.Framework;
using log4net;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// Handles redirect from rpxnow.com authentication, receives and process the auth token
    /// </summary>
    public partial class OpenIdRpxHandlerPage : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenIdRpxHandlerPage));
        
        private string tokenUrl = string.Empty;
        private string authToken = string.Empty;
        private string rpxApiKey = string.Empty;
        private string rpxBaseUrl = string.Empty;
        private Double timeOffset = 0;
        private Collection<CProfilePropertyDefinition>
            requiredProfileProperties = new Collection<CProfilePropertyDefinition>();

        private string returnUrlCookieName = string.Empty;
        private string returnUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            LoadSettings();
            PopulateLabels();

            if (authToken.Length == 0)
            {
                if (Request.IsAuthenticated)
                {
                    Response.Redirect(SiteRoot);
                    return;
                }

                if (!IsPostBack)
                {
                    Response.Redirect(SiteRoot + "/Secure/Login.aspx");
                    return;
                }
            }
            else
            {
                ProcessToken();
            }

        }

        private void ProcessToken()
        {
            OpenIdRpxHelper rpxHelper = new OpenIdRpxHelper(rpxApiKey, rpxBaseUrl);
            OpenIdRpxAuthInfo authInfo = rpxHelper.AuthInfo(authToken, tokenUrl);

            if ((authInfo == null) || (!authInfo.IsValid))
            {
                Response.Redirect(SiteRoot + "/Secure/Login.aspx");
                return;
            }

            if (Request.IsAuthenticated)
            {
                HandleAuthenticatedUser(rpxHelper, authInfo);
                return;
            }

            Guid userGuid = Guid.Empty;
            SiteUser user = null;

            //first find a site user by email
            if ((authInfo.Email.Length > 0))
            {
                user = SiteUser.GetByEmail(siteSettings, authInfo.Email);

            }

            if (authInfo.PrimaryKey.Length == 36)
            {
                try
                {
                    userGuid = new Guid(authInfo.PrimaryKey);
                }
                catch (FormatException) { }
                catch (OverflowException) { }
            }

            if ((user == null)&&(userGuid == Guid.Empty))
            {
                userGuid = SiteUser.GetUserGuidFromOpenId(
                    siteSettings.SiteId,
                    authInfo.Identifier);
            }

            if ((user == null) && (userGuid != Guid.Empty))
            {
                user = new SiteUser(siteSettings, userGuid);
                if (user.SiteGuid != siteSettings.SiteGuid) { user = null; }
            }

            if (user == null)
            {
                // not an existing user
                if (siteSettings.AllowNewRegistration)
                {
                    HandleNewUser(rpxHelper, authInfo);
                }
                else
                {
                    WebUtils.SetupRedirect(this, SiteRoot);
                    return;

                }
            }
            else
            {
                bool needToSave = false;
                if ((siteSettings.UseSecureRegistration)&& (user.RegisterConfirmGuid != Guid.Empty))
                {
                    if (authInfo.VerifiedEmail.Length > 0)
                    {
                        user.SetRegistrationConfirmationGuid(Guid.Empty);
                        user.Email = authInfo.VerifiedEmail;
                        needToSave = true;

                    }

                }

                if (user.OpenIdUri.Length == 0)
                {
                    user.OpenIdUri = authInfo.Identifier;
                    needToSave = true;
                }

                if (needToSave) { user.Save(); }

                if (WebConfigSettings.OpenIdRpxUseMappings)
                {
                    if ((authInfo.PrimaryKey.Length == 0) || (authInfo.PrimaryKey != user.UserGuid.ToString()))
                    {
                        rpxHelper.Map(authInfo.Identifier, user.UserGuid.ToString());
                    }
                }


                SignInUser(user);

            }

        }

        private void SignInUser(SiteUser user)
        {
            if (
                (siteSettings.UseSecureRegistration)
                && (user.RegisterConfirmGuid != Guid.Empty)
                )
            {
                
                Notification.SendRegistrationConfirmationLink(
                    SiteUtils.GetSmtpSettings(),
                    ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
                    siteSettings.DefaultEmailFromAddress,
                    user.Email,
                    siteSettings.SiteName,
                    SiteRoot+ "/ConfirmRegistration.aspx?ticket=" +
                    user.RegisterConfirmGuid.ToString());


                log.Info("User " + user.Name + " tried to login but email address is not confirmed.");

                lblError.Text = Resource.RegistrationRequiresEmailConfirmationMessage;

                return;
                
            }

            if (user.IsLockedOut)
            {

                log.Info("User " + user.Name + " tried to login but account is locked.");

                lblError.Text = Resource.LoginAccountLockedMessage;

                return;
            }


            if (siteSettings.UseEmailForLogin)
            {
                FormsAuthentication.SetAuthCookie(
                    user.Email, true);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(
                    user.LoginName, true);
            }

            if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                string cookieName = "siteguid" + siteSettings.SiteGuid;
                CookieHelper.SetCookie(cookieName, user.UserGuid.ToString(), true);
            }

            user.UpdateLastLoginTime();

            // track user ip address
            UserLocation userLocation = new UserLocation(user.UserGuid, SiteUtils.GetIP4Address());
            userLocation.SiteGuid = siteSettings.SiteGuid;
            userLocation.Hostname = Request.UserHostName;
            userLocation.Save();

            UserSignInEventArgs u = new UserSignInEventArgs(user);
            OnUserSignIn(u);

            if (CookieHelper.CookieExists(returnUrlCookieName))
            {
                returnUrl = CookieHelper.GetCookieValue(returnUrlCookieName);
                CookieHelper.ExpireCookie(returnUrlCookieName);
            }

            if (returnUrl.Length > 0)
            {
                WebUtils.SetupRedirect(this, returnUrl);
                return;

            }

            WebUtils.SetupRedirect(this, SiteRoot);
            return;

        }

        private void HandleAuthenticatedUser(OpenIdRpxHelper rpxHelper, OpenIdRpxAuthInfo authInfo)
        {
            // user is already authenticated so must be updating open id in profile

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();

            if (currentUser == null)
            {
                Response.Redirect(SiteRoot);
                return;
            }

            rpxHelper.Map(authInfo.Identifier, currentUser.UserGuid.ToString());

            currentUser.OpenIdUri = authInfo.Identifier;
            currentUser.Save();

            Response.Redirect(SiteRoot + "/Secure/UserProfile.aspx?t=i");
            

        }

        private void HandleNewUser(OpenIdRpxHelper rpxHelper, OpenIdRpxAuthInfo authInfo)
        {
            if (!IsValidForUserCreation(authInfo))
            {
                PromptForNeededInfo(rpxHelper, authInfo);
                return;

            }

            string loginName = string.Empty;

            if ((authInfo.PreferredUsername.Length > 0) && (!SiteUser.LoginExistsInDB(siteSettings.SiteId, authInfo.PreferredUsername)))
            {
                loginName = authInfo.PreferredUsername;
            }

            if (loginName.Length == 0) { loginName = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, authInfo.Email); }

            string name = loginName;

            if (authInfo.DisplayName.Length > 0)
            {
                name = authInfo.DisplayName;
            }

            bool emailIsVerified = (authInfo.VerifiedEmail == authInfo.Email);

            SiteUser newUser = CreateUser(
                    authInfo.Identifier,
                    authInfo.Email,
                    loginName,
                    name,
                    emailIsVerified);

            SignInUser(newUser);

        }

        private void PromptForNeededInfo(OpenIdRpxHelper rpxHelper, OpenIdRpxAuthInfo authInfo)
        {
          
            if (Email.IsValidEmailAddressSyntax(authInfo.Email))
            {
                divEmailInput.Visible = false;
                divEmailDisplay.Visible = true;
                litEmail.Text = authInfo.Email;
            }
            else
            {
                divEmailInput.Visible = true;
                divEmailDisplay.Visible = false;
            }

            pnlNeededProfileProperties.Visible = true;
            pnlOpenID.Visible = false;
            
            litOpenIDURI.Text = authInfo.Identifier;
            hdnIdentifier.Value = authInfo.Identifier;
            hdnPreferredUsername.Value = authInfo.PreferredUsername;
            hdnDisplayName.Value = authInfo.DisplayName;

            if (authInfo.ProviderName.Length > 0)
            {
                litHeading.Text = string.Format(CultureInfo.InvariantCulture, Resource.RpxRegistrationHeadingFormat, authInfo.ProviderName);
            }
            
            PopulateRequiredProfileControls();
            
            litInfoNeededMessage.Text = Resource.OpenIDAdditionalInfoNeededMessage;


        }

        private void PopulateRequiredProfileControls()
        {
            foreach (CProfilePropertyDefinition propertyDefinition in requiredProfileProperties)
            {
                CProfilePropertyDefinition.SetupPropertyControl(
                    this,
                    pnlRequiredProfileProperties,
                    propertyDefinition,
                    timeOffset,
                    SiteRoot);
            }

        }

        void btnCreateUser_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) { return; }

            if (hdnIdentifier.Value.Length == 0)
            {   // form manipulation if this is missing
                Response.Redirect(SiteRoot + "/Secure/Register.aspx");
                return;
            }
           
            string email = txtEmail.Text;
            string loginName = string.Empty;

            if ((hdnPreferredUsername.Value.Length > 0) && (!SiteUser.LoginExistsInDB(siteSettings.SiteId, hdnPreferredUsername.Value)))
            {
                loginName = hdnPreferredUsername.Value;
            }

            if (loginName.Length == 0) { loginName = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, email); }

            string name = loginName;

            if (hdnDisplayName.Value.Length > 0)
            {
                name = hdnDisplayName.Value;
            }
            

            if (SiteUser.EmailExistsInDB(siteSettings.SiteId, email))
            {
                lblError.Text = Resource.RegisterDuplicateEmailMessage;
            }
            else
            {
                bool emailIsVerified = false;
                SiteUser newUser = CreateUser(
                    hdnIdentifier.Value, 
                    email, 
                    loginName, 
                    name,
                    emailIsVerified);

                SignInUser(newUser);
            }

            
        }

        private SiteUser CreateUser(
            string openId,
            string email,
            string loginName,
            string name,
            bool emailIsVerified)
        {
            SiteUser newUser = new SiteUser(siteSettings);
            newUser.Email = email;

            if (loginName.Length > 50) loginName = loginName.Substring(0, 50);

            int i = 1;
            while (SiteUser.LoginExistsInDB(
                siteSettings.SiteId, loginName))
            {
                loginName += i.ToString();
                if (loginName.Length > 50) loginName = loginName.Remove(40, 1);
                i++;

            }
            if ((name == null) || (name.Length == 0)) name = loginName;
            newUser.LoginName = loginName;
            newUser.Name = name;
            //newUser.Password = SiteUser.CreateRandomPassword(7);
            CMembershipProvider CMembership = (CMembershipProvider)Membership.Provider;
            newUser.Password = CMembership.EncodePassword(SiteUser.CreateRandomPassword(7), siteSettings);
            newUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
            newUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
            newUser.OpenIdUri = openId;
            newUser.Save();
            if (siteSettings.UseSecureRegistration)
            {
                if (!emailIsVerified)
                {
                    newUser.SetRegistrationConfirmationGuid(Guid.NewGuid());
                }
            }

            CProfileConfiguration profileConfig
                = CProfileConfiguration.GetConfig();

            // set default values first
            foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
            {
                CProfilePropertyDefinition.SavePropertyDefault(
                    newUser, propertyDefinition);
            }

            foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
            {
                if (propertyDefinition.RequiredForRegistration)
                {
                    CProfilePropertyDefinition.SaveProperty(
                        newUser,
                        pnlRequiredProfileProperties,
                        propertyDefinition,
                        timeOffset);
                }
            }

            // track user ip address
            UserLocation userLocation = new UserLocation(newUser.UserGuid, SiteUtils.GetIP4Address());
            userLocation.SiteGuid = siteSettings.SiteGuid;
            userLocation.Hostname = Page.Request.UserHostName;
            userLocation.Save();

            UserRegisteredEventArgs u = new UserRegisteredEventArgs(newUser);
            OnUserRegistered(u);

            CacheHelper.TouchMembershipStatisticsCacheDependencyFile();

            // we'll map them next time they login
            //OpenIdRpxHelper rpxHelper = new OpenIdRpxHelper(rpxApiKey, rpxBaseUrl);
            //rpxHelper.Map(openId, newUser.UserGuid.ToString());

            NewsletterHelper.ClaimExistingSubscriptions(newUser);

            return newUser;

            

        }

        #region Events

        //private void HookupRegistrationEventHandlers()
        //{
        //    // this is a hook so that custom code can be fired when pages are created
        //    // implement a PageCreatedEventHandlerPovider and put a config file for it in
        //    // /Setup/ProviderConfig/pagecreatedeventhandlers
        //    try
        //    {
        //        foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
        //        {
        //            this.UserRegistered += handler.UserRegisteredHandler;
        //        }
        //    }
        //    catch (TypeInitializationException ex)
        //    {
        //        log.Error(ex);
        //    }

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


        public event UserSignInEventHandler UserSignIn;

        protected void OnUserSignIn(UserSignInEventArgs e)
        {
            if (UserSignIn != null)
            {
                UserSignIn(this, e);
            }
        }

        private bool IsValidForUserCreation(OpenIdRpxAuthInfo authInfo)
        {
            bool result = true;

            if (authInfo == null) { return false; }

            if (String.IsNullOrEmpty(authInfo.Email)) { return false; }


            if (!Email.IsValidEmailAddressSyntax(authInfo.Email)) { return false; }

            CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
            if (profileConfig.HasRequiredCustomProperties()) { result = false; }

            return result;

        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterWithOpenIDLink);

            litHeading.Text = Resource.OpenIDRegistrationHeading;

            EmailRequired.ErrorMessage = Resource.RegisterEmailRequiredMessage;

            Literal agreement = new Literal();
            agreement.Text = ResourceHelper.GetMessageTemplate("RegisterLicense.config");
            divAgreement.Controls.Add(agreement);
            btnCreateUser.Text = Resource.RegisterButton;

        }

        private void LoadSettings()
        {
            if (Request.Params.Get("token") != null) { authToken = Request.Params.Get("token"); }

            tokenUrl = SiteRoot + "/Secure/OpenIdRpxHandler.aspx";
            rpxBaseUrl = "https://rpxnow.com";
            rpxApiKey = siteSettings.RpxNowApiKey;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
                {
                    rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
                }

            }

            regexEmail.ValidationExpression = SecurityHelper.RegexEmailValidationPattern;
            regexEmail.ErrorMessage = Resource.RegisterEmailRegexMessage;

            returnUrlCookieName = "returnurl" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            
        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnCreateUser.Click += new EventHandler(btnCreateUser_Click);

        }

        #endregion
    }
}
