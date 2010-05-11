///	Author:					Joe Audette
///	Created:				2004-12-16
/// Last Modified:			2010-02-01

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Net;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Resources;
using log4net;

namespace Cynthia.Web.ContactUI
{
	
	public partial class ContactForm : SiteModuleControl
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ContactForm));
        private bool useSpamBlocking = false;
        private string subjectPrefix = string.Empty;
        private bool appendIPToMessageSetting = true;
        private string editorHeight = "350";
        private string emailReceiveAddresses = string.Empty;
        private string emailReceiveAliases = string.Empty;
        private List<string> emailAddresses = null;
        private List<string> emailAliases = null;
        private string emailBccAddresses = string.Empty;
        private bool useInputAsFromAddress = false;
        private bool keepMessages = true;
        

        
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += Page_Load;
            this.btnSend.Click += btnSend_Click;
            Page.EnableViewState = true;

            SiteUtils.SetupEditor(edMessage);
            LoadSettings();
            PopulateLabels();
            
            
        }

        
        protected void Page_Load(object sender, EventArgs e)
		{
            BindToList();
            PopulateControls();
		}

        private void PopulateControls()
        {
            pnlContainer.ModuleId = ModuleId;

            if (!Page.IsPostBack)
            {
                if (Request.IsAuthenticated)
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if (siteUser != null)
                    {
                        this.txtEmail.Text = siteUser.Email;
                        this.txtName.Text = siteUser.Name;
                    }
                }
                //edMessage.Text = "your message here";

            }
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            //Page.Validate("Contact");
            bool isValid = true;
            reqEmail.Validate();
            if (!reqEmail.IsValid) { isValid = false; }
            regexEmail.Validate();
            if (!regexEmail.IsValid) { isValid = false; }

            if ((isValid) && (edMessage.Text.Length > 0))
            {
                if ((useSpamBlocking) && (divCaptcha.Visible))
                {
                    if (!captcha.IsValid) return;
                }

                if (subjectPrefix.Length == 0)
                {
                    subjectPrefix = Title;
                }

                StringBuilder message = new StringBuilder();
                message.Append(txtName.Text + "<br />");
                message.Append(txtEmail.Text + "<br /><br />");
                message.Append(edMessage.Text + "<br /><br />");
                if (appendIPToMessageSetting)
                {
                    message.Append("HTTP_USER_AGENT: " + Page.Request.ServerVariables["HTTP_USER_AGENT"] + "<br />");
                    message.Append("REMOTE_HOST: " + Page.Request.ServerVariables["REMOTE_HOST"] + "<br />");
                    message.Append("REMOTE_ADDR: " + SiteUtils.GetIP4Address() + "<br />");
                    message.Append("LOCAL_ADDR: " + Page.Request.ServerVariables["LOCAL_ADDR"] + "<br />");

                }

                Module m = new Module(ModuleId);
                if ((keepMessages) &&(m.ModuleGuid != Guid.Empty))
                {
                    ContactFormMessage contact = new ContactFormMessage();
                    contact.ModuleGuid = m.ModuleGuid;
                    contact.SiteGuid = SiteSettings.SiteGuid;
                    contact.Message = message.ToString();
                    contact.Subject = subjectPrefix + ": " + txtSubject.Text;
                    contact.UserName = txtName.Text;
                    contact.Email = txtEmail.Text;
                    contact.CreatedFromIpAddress = SiteUtils.GetIP4Address();

                    if (Request.IsAuthenticated)
                    {
                        SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                        if (currentUser != null)
                            contact.UserGuid = currentUser.UserGuid;

                    }

                    contact.Save();


                }

                string fromAddress = SiteSettings.DefaultEmailFromAddress;
                if (useInputAsFromAddress) { fromAddress = txtEmail.Text; }

                if (emailAddresses != null)
                {
                    SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();

                    if ((pnlToAddresses.Visible) && (ddToAddresses.SelectedIndex > -1))
                    {
                        string to = emailAddresses[ddToAddresses.SelectedIndex];
                        try
                        {
                            Email.SendEmail(
                                smtpSettings,
                                fromAddress,
                                txtEmail.Text,
                                to,
                                string.Empty,
                                emailBccAddresses,
                                subjectPrefix + ": " + this.txtSubject.Text,
                                message.ToString(),
                                true,
                                "Normal");

                        }
                        catch (Exception ex)
                        {
                            log.Error("error sending email from address was " + fromAddress + " to address was " + to, ex);
                        }

                    }
                    else
                    {

                        foreach (string to in emailAddresses)
                        {
                            try
                            {
                                Email.SendEmail(
                                    smtpSettings,
                                    fromAddress,
                                    txtEmail.Text,
                                    to,
                                    string.Empty,
                                    emailBccAddresses,
                                    subjectPrefix + ": " + this.txtSubject.Text,
                                    message.ToString(),
                                    true,
                                    "Normal");

                            }
                            catch (Exception ex)
                            {
                                log.Error("error sending email from address was " + fromAddress + " to address was " + to, ex);
                            }

                        }
                    }


                }


                this.lblMessage.Text = ContactFormResources.ContactFormThankYouLabel;
                this.pnlSend.Visible = false;


            }
            else
            {
                if (edMessage.Text.Length == 0)
                {
                    lblMessage.Text = ContactFormResources.ContactFormEmptyMessageWarning;
                }
                else
                {
                    lblMessage.Text = "invalid";
                }

            }

            btnSend.Text = ContactFormResources.ContactFormSendButtonLabel;
            btnSend.Enabled = true;
        }

        

        private void PopulateLabels()
        {
            btnSend.Text = ContactFormResources.ContactFormSendButtonLabel;
            SiteUtils.SetButtonAccessKey
                (btnSend, ContactFormResources.ContactFormSendButtonAccessKey);

            
            this.reqEmail.ErrorMessage = ContactFormResources.ContactFormValidAddressLabel;
            this.regexEmail.ErrorMessage = ContactFormResources.ContactFormValidAddressLabel;

            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

          
            Title1.EditUrl = string.Empty;
            Title1.EditText = string.Empty;
            Title1.ToolTip = string.Empty;
            if (IsEditable)
            {
                CBasePage basePage = Page as CBasePage;
                if (basePage != null)
                {
                    basePage.ScriptConfig.IncludeGreyBox = true;
                    if (keepMessages)
                    {
                        Title1.LiteralExtraMarkup =
                                "&nbsp;<a href='"
                                + SiteRoot
                                + "/ContactForm/MessageListDialog.aspx?pageid=" + PageId.ToInvariantString()
                                + "&amp;mid=" + ModuleId.ToInvariantString()
                                + "' class='ModuleEditLink' title='" + ContactFormResources.ContactFormViewMessagesLink + "' "
                                + "onclick='GB_showFullScreen(this.title, this.href); return false;'"
                                + ">" + ContactFormResources.ContactFormViewMessagesLink + "</a>";
                    }
                }
            }


            

        }

        private void BindToList()
        {
            if (Page.IsPostBack) { return; }
            if (!pnlToAddresses.Visible) { return; }
            if (emailAddresses == null) { return; }
            if (emailAddresses.Count <= 1) { return; }
            if (emailAliases == null) { emailAliases = new List<string>(); }

            List<string> bindList = new List<string>();
            int index = 0;
            foreach (string a in emailAddresses)
            {
                if ((index + 1) <= emailAliases.Count)
                {
                    bindList.Add(emailAliases[index]);
                }
                else
                {
                    bindList.Add(a);
                }
                index += 1;
            }

            index = 0;
            foreach (string a in bindList)
            {
                ListItem item = new ListItem(a, index.ToInvariantString());
                ddToAddresses.Items.Add(item);
                index += 1;
            }

        }


        private void LoadSettings()
        {
            if (Settings.Contains("ContactFormEditorHeightSetting"))
            {
                editorHeight = Settings["ContactFormEditorHeightSetting"].ToString();
            }

            if (Settings.Contains("ContactFormSubjectPrefixSetting"))
            {
                subjectPrefix = Settings["ContactFormSubjectPrefixSetting"].ToString().Trim();
            }

            if (Settings.Contains("ContactFormEmailSetting"))
            {
                emailReceiveAddresses = Settings["ContactFormEmailSetting"].ToString().Trim();
                emailAddresses = emailReceiveAddresses.SplitOnChar('|');
            }

            if (Settings.Contains("ContactFormEmailAliasSetting"))
            {
                emailReceiveAliases = Settings["ContactFormEmailAliasSetting"].ToString().Trim();
                
            }

            emailAliases = emailReceiveAliases.SplitOnChar('|');

            if (Settings.Contains("ContactFormEmailBccSetting"))
            {
                emailBccAddresses = Settings["ContactFormEmailBccSetting"].ToString();
            }

            if ((emailAddresses != null) && (emailAddresses.Count > 1))
            {
                pnlToAddresses.Visible = true;
                
            }


            useSpamBlocking = WebUtils.ParseBoolFromHashtable(Settings, "ContactFormUseCommentSpamBlocker", false);

            appendIPToMessageSetting = WebUtils.ParseBoolFromHashtable(Settings, "AppendIPToMessageSetting", appendIPToMessageSetting);
            keepMessages = WebUtils.ParseBoolFromHashtable(Settings, "KeepMessagesInDatabase", keepMessages);
            useInputAsFromAddress = WebUtils.ParseBoolFromHashtable(Settings, "UseInputAddressAsFromAddress", useInputAsFromAddress);
            

            edMessage.WebEditor.ToolBar = ToolBar.AnonymousUser;
            edMessage.WebEditor.Height = Unit.Parse(editorHeight);

            vSummary.ValidationGroup = "Contact" + ModuleId.ToInvariantString();
            reqEmail.ValidationGroup = "Contact" + ModuleId.ToInvariantString();
            regexEmail.ValidationGroup = "Contact" + ModuleId.ToInvariantString();
            btnSend.ValidationGroup = "Contact" + ModuleId.ToInvariantString();



            if (!useSpamBlocking)
            {
                captcha.Enabled = false;
                captcha.Visible = false;
                divCaptcha.Visible = false;
                //pnlSend.Controls.Remove(divCaptcha);
            }
            else
            {
                captcha.ProviderName = SiteSettings.CaptchaProvider;
                captcha.Captcha.ControlID = "captcha" + ModuleId;
                captcha.RecaptchaPrivateKey = SiteSettings.RecaptchaPrivateKey;
                captcha.RecaptchaPublicKey = SiteSettings.RecaptchaPublicKey;

            }

        }

        

    }
}
