// Author:                     Joe Audette
// Created:                    2006-04-27
// Last Modified:              2009-06-07
//
using System;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Resources;
using log4net;

namespace Cynthia.Web.UI.Pages
{
   
    public partial class ChangePassword : CBasePage
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(ChangePassword));


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            if (WebConfigSettings.HideMenusOnChangePasswordPage) { SuppressAllMenus(); }
            
        }

        

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            PopulateLabels();
        }

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ChangePasswordLabel);

            Button changePasswordButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ChangePasswordPushButton");
            Button cancelButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CancelPushButton");

            if (changePasswordButton != null)
            {
                changePasswordButton.Text = Resource.ChangePasswordButton;
                SiteUtils.SetButtonAccessKey(changePasswordButton, AccessKeys.ChangePasswordButtonAccessKey);
            }
            else
            {
                log.Debug("couldn't find changepasswordbutton so couldn't set label");
            }

            if (cancelButton != null)
            {
                cancelButton.Text = Resource.ChangePasswordCancelButton;
                SiteUtils.SetButtonAccessKey(cancelButton, AccessKeys.ChangePasswordCancelButtonAccessKey);
            }
            else
            {
                log.Debug("couldn't find cancelbutton so couldn't set label");
            }


            this.ChangePassword1.CancelDestinationPageUrl 
                = SiteUtils.GetNavigationSiteRoot() + "/Secure/UserProfile.aspx";

            this.ChangePassword1.ChangePasswordFailureText
                = Resource.ChangePasswordFailureText;

            CompareValidator newPasswordCompare
                = (CompareValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordCompare");

            if (newPasswordCompare != null)
            {
                newPasswordCompare.ErrorMessage
                    = Resource.ChangePasswordMustMatchConfirmMessage;
            }

            RequiredFieldValidator confirmNewPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ConfirmNewPasswordRequired");

            if (confirmNewPasswordRequired != null)
            {
                confirmNewPasswordRequired.ErrorMessage
                    = Resource.ChangePasswordConfirmPasswordRequiredMessage;
            }

            this.ChangePassword1.ContinueDestinationPageUrl 
                = SiteUtils.GetNavigationSiteRoot();

            RequiredFieldValidator newPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRequired");

            if (newPasswordRequired != null)
            {
                newPasswordRequired.ErrorMessage
                    = Resource.ChangePasswordNewPasswordRequired;
            }

            RequiredFieldValidator currentPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPasswordRequired");

            if (currentPasswordRequired != null)
            {
                currentPasswordRequired.ErrorMessage
                    = Resource.ChangePasswordCurrentPasswordRequiredWarning;
            }

            RegularExpressionValidator newPasswordRegex
                = (RegularExpressionValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRegex");

            if (newPasswordRegex != null)
            {
                // TODO: need to add this error message to SiteSettings instead
                // of getting from resource file.
                // The message should indicate how to construct a permissable 
                // password but since the regular expression is editable
                // there is no way to know the rules

                newPasswordRegex.ErrorMessage
                    = Resource.ChangePasswordPasswordRegexFailureMessage;

                newPasswordRegex.ValidationExpression 
                    = Membership.PasswordStrengthRegularExpression;

                if (Membership.PasswordStrengthRegularExpression.Length == 0)
                {
                    newPasswordRegex.Visible = false;
                    newPasswordRegex.ValidationGroup = "None";
                }
            }

            CustomValidator newPasswordRulesValidator
                = (CustomValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRulesValidator");
            if (newPasswordRulesValidator != null)
            {
                newPasswordRulesValidator.ServerValidate += new ServerValidateEventHandler(NewPasswordRulesValidator_ServerValidate);
            }

            this.ChangePassword1.SuccessTitleText = String.Empty;
            this.ChangePassword1.SuccessText = Resource.ChangePasswordSuccessText;

        }

        void NewPasswordRulesValidator_ServerValidate(
            object source, 
            ServerValidateEventArgs args)
        {
            CustomValidator validator = source as CustomValidator;
            validator.ErrorMessage = string.Empty;

            if (args.Value.Length < Membership.MinRequiredPasswordLength)
            {
                args.IsValid = false;
                validator.ErrorMessage 
                    += Resource.ChangePasswordMinimumLengthWarning 
                    + Membership.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture) + "<br />";
            }

            if (!HasEnoughNonAlphaNumericCharacters(args.Value))
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += Resource.ChangePasswordMinNonAlphanumericCharsWarning
                    + Membership.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture) + "<br />";

            }

            TextBox currentPassword
                    = (TextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPassword");

            TextBox newPassword
                    = (TextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPassword");

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser != null)
            {
                if (currentPassword != null)
                {
                    switch (Membership.Provider.PasswordFormat)
                    {
                        case MembershipPasswordFormat.Clear:
                            if (currentPassword.Text != currentUser.Password)
                            {
                                args.IsValid = false;
                                validator.ErrorMessage
                                    += Resource.ChangePasswordCurrentPasswordIncorrectWarning + "<br />";
                                    
                            }
                            break;

                        case MembershipPasswordFormat.Encrypted:

                            break;

                        case MembershipPasswordFormat.Hashed:

                            break;

                    }
                }

            }

            if ((newPassword != null) && (currentPassword != null))
            {
                if (newPassword.Text == currentPassword.Text)
                {
                    args.IsValid = false;
                    validator.ErrorMessage
                       += Resource.ChangePasswordNewMatchesOldWarning + "<br />";

                }
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
                if(!alphanumeric.Contains(c.ToString()))
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

        
    }
}
