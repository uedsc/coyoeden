/// Author:					    Joe Audette
/// Created:				    2006-04-15
///	Last Modified:              2009-06-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net;
using System.Net.Mail;
using log4net;
using Cynthia.Web.Framework;
using Cynthia.Web.Controls;
using Cynthia.Net;
using Resources;

namespace Cynthia.Web.UI.Pages
{
    
    public partial class RecoverPassword : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecoverPassword));


        protected Label EnterUserNameLabel;


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.Error += new EventHandler(RecoverPassword_Error);
            base.EnsureChildControls();
            this.PasswordRecovery1.SendingMail += new MailMessageEventHandler(PasswordRecovery1_SendingMail);
            this.PasswordRecovery1.SendMailError += new SendMailErrorEventHandler(PasswordRecovery1_SendMailError);
            if (!siteSettings.AllowPasswordRetrieval)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (WebConfigSettings.HideMenusOnPasswordRecoveryPage) { SuppressAllMenus(); }

        }



        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            if (!siteSettings.AllowPasswordRetrieval)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateLabels();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PasswordRecoveryTitle);

            MetaDescription = Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture,
                Resource.RecoverPasswordMetaDescriptionFormat,
                siteSettings.SiteName));

            EnterUserNameLabel = (Label)this.PasswordRecovery1.UserNameTemplateContainer.FindControl("lblEnterUserName");
            if (EnterUserNameLabel != null)
            {
                if ((siteSettings != null) && (siteSettings.UseEmailForLogin))
                {
                    EnterUserNameLabel.Text = Resource.EnterEmailLabel;
                }
                else
                {
                    EnterUserNameLabel.Text = Resource.EnterUserNameLabel;
                }

            }

            // UserName template
            Button userNameNextButton = (Button)PasswordRecovery1.UserNameTemplateContainer.FindControl("SubmitButton");
            if (userNameNextButton != null)
            {
                userNameNextButton.Text = Resource.PasswordRecoveryNextButton;
                SiteUtils.SetButtonAccessKey(userNameNextButton, AccessKeys.PasswordRecoveryAccessKey);
            }

            RequiredFieldValidator reqUserName
                = (RequiredFieldValidator)PasswordRecovery1.UserNameTemplateContainer.FindControl("UserNameRequired");

            if (reqUserName != null)
            {
                reqUserName.ErrorMessage = Resource.PasswordRecoveryUserNameRequiredWarning;
            }

            // Question Template
            Button questionNextButton = (Button)PasswordRecovery1.QuestionTemplateContainer.FindControl("SubmitButton");
            if (questionNextButton != null)
            {
                questionNextButton.Text = Resource.PasswordRecoveryNextButton;
                SiteUtils.SetButtonAccessKey(questionNextButton, AccessKeys.PasswordRecoveryAccessKey);
            }

            RequiredFieldValidator reqAnswer
                = (RequiredFieldValidator)PasswordRecovery1.QuestionTemplateContainer.FindControl("AnswerRequired");

            if (reqAnswer != null)
            {
                reqAnswer.ErrorMessage = Resource.PasswordRecoveryAnswerRequired;
            }

            this.PasswordRecovery1.GeneralFailureText = Resource.PasswordRecoveryGeneralFailureText;
            this.PasswordRecovery1.QuestionFailureText = Resource.PasswordRecoveryQuestionFailureText;
            this.PasswordRecovery1.UserNameFailureText = Resource.PasswordRecoveryUserNameFailureText;
            this.PasswordRecovery1.MailDefinition.Subject
                = string.Format(Resource.PasswordRecoveryEmailSubjectFormatString,
                siteSettings.SiteName);

            string emailFilename;

            if (Membership.Provider.PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                emailFilename =
                    ResourceHelper.GetFullResourceFilePath(
                        System.Globalization.CultureInfo.CurrentCulture,
                        this.GetEmailTemplatesFolder(),
                        WebConfigSettings.HashedPasswordRecoveryEmailTemplateFileNamePattern);

                this.PasswordRecovery1.MailDefinition.BodyFileName =
                    string.IsNullOrEmpty(emailFilename) ?
                    this.PasswordRecovery1.MailDefinition.BodyFileName
                        = Server.MapPath("~/Data/MessageTemplates/"
                        + ResourceHelper.GetDefaultCulture()
                        + "-" + WebConfigSettings.HashedPasswordRecoveryEmailTemplateFileNamePattern) :
                    emailFilename;

            }
            else
            {
                emailFilename =
                    ResourceHelper.GetFullResourceFilePath(
                        System.Globalization.CultureInfo.CurrentCulture,
                        this.GetEmailTemplatesFolder(),
                        WebConfigSettings.PasswordRecoveryEmailTemplateFileNamePattern);

                this.PasswordRecovery1.MailDefinition.BodyFileName =
                    string.IsNullOrEmpty(emailFilename) ?
                    this.PasswordRecovery1.MailDefinition.BodyFileName
                        = Server.MapPath("~/Data/MessageTemplates/"
                        + ResourceHelper.GetDefaultCulture()
                        + "-" + WebConfigSettings.PasswordRecoveryEmailTemplateFileNamePattern) :
                    emailFilename;
            }

            this.PasswordRecovery1.MailDefinition.IsBodyHtml = ConfigHelper.GetBoolProperty("UseHtmlBodyInPasswordRecoveryEmail", false);
        }

        void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
        {
            e.Message.Body = e.Message.Body.Replace("{SiteName}", siteSettings.SiteName);
            e.Message.Body = e.Message.Body.Replace("{SiteLink}", SiteUtils.GetNavigationSiteRoot());

            // patch from voir hillaire for TLS/SSL problem
            if (WebConfigSettings.DisableDotNetOpenMail)
            {
                SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();
                if (smtpSettings.UseSsl)//using SSL requires using smtpSettings
                {
                    try
                    {
                        SmtpClient smtpSender = new SmtpClient(smtpSettings.Server, smtpSettings.Port);
                        smtpSender.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpSender.Credentials = new NetworkCredential(smtpSettings.User, smtpSettings.Password);
                        smtpSender.EnableSsl = true;
                        smtpSender.Send(e.Message);
                        e.Cancel = true; //stop here so the "built-in" mail routine doesn't run
                    }
                    catch (SmtpException ex) 
                    {
                        log.Error("password recovery error", ex);
                    }
                    catch (InvalidOperationException ex)
                    {
                        log.Error("password recovery error", ex);
                    }
                }
            }

        }

        void PasswordRecovery1_SendMailError(object sender, SendMailErrorEventArgs e)
        {
            String logMessage = "unable to send password recovery email. Please check the system.net MailSettings section in web.config";

            log.Error(logMessage, e.Exception);
            lblMailError.Text = Resource.PasswordRecoveryMailFailureMessage;
            e.Handled = true;
            SiteLabel successLabel
                = (SiteLabel)PasswordRecovery1.SuccessTemplateContainer.FindControl("successLabel");
            if (successLabel != null) successLabel.Visible = false;

        }

        protected void RecoverPassword_Error(object sender, EventArgs e)
        {
            //try
            //{
            Exception rawException = Server.GetLastError();
            if (rawException != null)
            {
                if (rawException is CynMembershipException)
                {
                    Server.ClearError();
                    WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot());
                    return;
                }
            }
            //}
            //catch { }
        }

        private string GetEmailTemplatesFolder()
        {
            if (HttpContext.Current == null) return String.Empty;
            return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot()
                    + "/Data/MessageTemplates") + System.IO.Path.DirectorySeparatorChar;
        }
    }
}
