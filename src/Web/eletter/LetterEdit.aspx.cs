/// Author:					Joe Audette
/// Created:				2007-09-23
/// Last Modified:			2010-01-18
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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;
using Cynthia.Web.Editor;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Net;
using Resources;

namespace Cynthia.Web.ELetterUI
{
    
    public partial class LetterEditPage : CBasePage
    {
        private LetterInfo letterInfo = null;
        private Letter letter = null;
        private Guid letterInfoGuid = Guid.Empty;
        private Guid letterGuid = Guid.Empty;
        private SiteUser currentUser = null;
        private bool isSiteEditor = false;
        private string imageSiteRoot = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor)&&(!WebUser.IsNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (letterInfo == null) return;

            lnkDraftList.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsLetterUnsentLettersHeadingFormatString,
                letterInfo.Title);

            if ((letter!= null)&&(letter.SendClickedUtc > DateTime.MinValue))
            {
                lnkDraftList.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsLetterPreviousLettersHeadingFormatString,
                letterInfo.Title);

                lnkDraftList.ToolTip = Resource.NewsLetterArchiveLettersHeading;
                lnkDraftList.NavigateUrl = SiteRoot + "/eletter/LetterArchive.aspx?l=" + letterInfoGuid.ToString();

            }

            if (Page.IsPostBack) return;

            PopulateTemplateList();

            if (letter == null) return;

            //btnSendToList.Enabled = WebUser.IsInRoles(letterInfo.RolesThatCanSend);
            if (letter.SendCompleteUtc > DateTime.MinValue)
            {
                btnSave.Enabled = false;
                btnSendToList.Enabled = false;
                // once a letter has been sent only admin can delete it.
                btnDelete.Enabled = WebUser.IsAdmin;
            }

            if (Page.IsPostBack) return;

            litHeading.Text = letter.Subject;
            txtSubject.Text = letter.Subject;
            edContent.Text = letter.HtmlBody;
            txtPlainText.Text = letter.TextBody;


        }

        

        private void PopulateTemplateList()
        {
            List<LetterHtmlTemplate> LetterHtmlTemplateList = LetterHtmlTemplate.GetAll(siteSettings.SiteGuid);
            if (LetterHtmlTemplateList.Count == 0)
            {
                CSetup.CreateDefaultLetterTemplates(siteSettings.SiteGuid);

                LetterHtmlTemplateList = LetterHtmlTemplate.GetAll(siteSettings.SiteGuid);

            }

            ddTemplates.DataSource = LetterHtmlTemplateList;
            ddTemplates.DataBind();


        }

        void btnSave_Click(object sender, EventArgs e)
        {
            
            Page.Validate();
            if (!Page.IsValid) return;

            SaveLetter();

            if (letter != null)
            {
                string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
                    + "&letter=" + letter.LetterGuid.ToString();

                WebUtils.SetupRedirect(this, redirectUrl);

            }


        }

        private void SaveLetter()
        {
            if (letter == null) return;
            if (currentUser == null) return;
            // no edits after sending
            if (letter.SendCompleteUtc > DateTime.MinValue) return;

            letter.HtmlBody = SiteUtils.ChangeRelativeUrlsToFullyQuailifiedUrls(SiteRoot, imageSiteRoot, edContent.Text);
            letter.TextBody = txtPlainText.Text;
            letter.Subject = txtSubject.Text;
            letter.LastModBy = currentUser.UserGuid;
            if (letter.LetterGuid == Guid.Empty)
            {
                // new letter
                letter.LetterInfoGuid = letterInfoGuid;
                letter.CreatedBy = currentUser.UserGuid;
            }

            letter.Save();
            letterGuid = letter.LetterGuid;

        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (letter == null) return;
            if (currentUser == null) return;

            Letter.Delete(letterGuid);

            //string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
            //        + "&letter=" + letter.LetterGuid.ToString();

            string redirectUrl = SiteRoot + "/eletter/Admin.aspx";

            WebUtils.SetupRedirect(this, redirectUrl);

            

        }

        void btnSendPreview_Click(object sender, EventArgs e)
        {
            string baseUrl = WebUtils.GetHostRoot();
            if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                // in folder based sites the relative urls in the editor will already have the folder name
                // so we want to use just the raw site root not the navigation root
                baseUrl = WebUtils.GetSiteRoot();
            }

            string content = SiteUtils.ChangeRelativeUrlsToFullyQuailifiedUrls(baseUrl, ImageSiteRoot, edContent.Text);
            // TODO: validate email
            Email.SendEmail(
                SiteUtils.GetSmtpSettings(),
                siteSettings.DefaultEmailFromAddress,
                txtPreviewAddress.Text,
                string.Empty,
                string.Empty,
                txtSubject.Text,
                content,
                true,
                "Normal");

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            
        }

        void btnSaveAsTemplate_Click(object sender, EventArgs e)
        {
            SaveLetter();

            LetterHtmlTemplate template = new LetterHtmlTemplate();
            template.SiteGuid = siteSettings.SiteGuid;
            template.Title = txtNewTemplateName.Text;
            template.Html = edContent.Text;
            template.Save();

            string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
                     + "&letter=" + letter.LetterGuid.ToString();

            WebUtils.SetupRedirect(this, redirectUrl);
            
        }

        void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            if (ddTemplates.Items.Count == 0) return;

            if (ddTemplates.SelectedValue.Length != 36) return;

            Guid templateGuid = new Guid(ddTemplates.SelectedValue);
            LetterHtmlTemplate template = new LetterHtmlTemplate(templateGuid);
            edContent.Text = template.Html;
            SaveLetter();

            string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
                    + "&letter=" + letter.LetterGuid.ToString();

            WebUtils.SetupRedirect(this, redirectUrl);
              
            
        }

        void btnSendToList_Click(object sender, EventArgs e)
        {
            SaveLetter();
            
            if (!LetterIsValidForSending()) return;

            if (letter.SendCompleteUtc > DateTime.MinValue) return;

            // TODO: implement approval process
            letter.ApprovedBy = currentUser.UserGuid;
            letter.IsApproved = true;

            string baseUrl = WebUtils.GetHostRoot();
            if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                // in folder based sites the relative urls in the editor will already have the folder name
                // so we want to use just the raw site root not the navigation root
                baseUrl = WebUtils.GetSiteRoot();
            }

            string content = SiteUtils.ChangeRelativeUrlsToFullyQuailifiedUrls(baseUrl, ImageSiteRoot, letter.HtmlBody);
            letter.HtmlBody = content;

            SaveLetter();

            letter.TrackSendClicked();


            LetterSendTask letterSender = new LetterSendTask();
            letterSender.SiteGuid = siteSettings.SiteGuid;
            letterSender.QueuedBy = currentUser.UserGuid;
            letterSender.LetterGuid = letter.LetterGuid;
            letterSender.UnsubscribeLinkText = Resource.NewsletterUnsubscribeLink;
            letterSender.UnsubscribeUrl = SiteRoot + "/eletter/Unsubscribe.aspx";
            letterSender.NotificationFromEmail = siteSettings.DefaultEmailFromAddress;
            letterSender.NotifyOnCompletion = true;
            letterSender.NotificationToEmail = currentUser.Email;

            SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();
            letterSender.User = smtpSettings.User;
            letterSender.Password = smtpSettings.Password;
            letterSender.Server = smtpSettings.Server;
            letterSender.Port = smtpSettings.Port;
            letterSender.RequiresAuthentication = smtpSettings.RequiresAuthentication;
            letterSender.UseSsl = smtpSettings.UseSsl;
            letterSender.PreferredEncoding = smtpSettings.PreferredEncoding;
            
            //localizing the task name can cause deserialization errors
            //letterSender.TaskName = string.Format(CultureInfo.InvariantCulture,
            //        Resource.NewsletterTaskNameFormatString, letterInfo.Title);

            //letterSender.NotificationSubject = string.Format(CultureInfo.InvariantCulture,
            //    Resource.TaskQueueCompletedTaskNotificationFormatString, letterSender.TaskName);

            //letterSender.StatusCompleteMessage = Resource.NewsletterStatusCompleteMessage;
            //letterSender.StatusQueuedMessage = Resource.NewsletterStatusQueuedMessage;
            //letterSender.StatusStartedMessage = Resource.NewsletterStatusStartedMessage;
            //letterSender.StatusRunningMessage = Resource.NewsletterStatusRunningMessageFormatString;
            //letterSender.TaskCompleteMessage = string.Format(CultureInfo.InvariantCulture,
            //    Resource.TaskQueueCompletedTaskNotificationFormatString, letterSender.TaskName);

            letterSender.QueueTask();

            string redirectUrl = SiteRoot + "/eletter/SendProgress.aspx?l=" + letterInfoGuid.ToString()
                + "&letter=" + letterGuid.ToString()
                + "&t=" + letterSender.TaskGuid.ToString();

            WebTaskManager.StartOrResumeTasks();

            WebUtils.SetupRedirect(this, redirectUrl);



        }

        void btnGeneratePlainText_Click(object sender, EventArgs e)
        {
            if (letter == null) return;
            //SaveLetter();

            txtPlainText.Text = SecurityHelper.RemoveMarkup(UIHelper.ConvertHtmlBreaksToTextBreaks(letter.HtmlBody));
            SaveLetter();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private bool LetterIsValidForSending()
        {
            bool result = true;

            if (letter == null)result = false;

            lblErrorMessage.Text = string.Empty;

            if (!letter.HtmlBody.Contains(Letter.UnsubscribeToken))
            {
                lblErrorMessage.Text += Resource.NewsletterUnsubscribeTokenNotFoundMessage;
                result = false;
            }

            if (!letter.TextBody.Contains(Letter.UnsubscribeToken))
            {
                lblErrorMessage.Text += Resource.NewsletterPlainTextUnsubscribeTokenNotFoundMessage;
                result = false;
            }

            if (letter.TextBody.Length == 0)
            {
                lblErrorMessage.Text += Resource.NewsletterTextVersionRequiredWarning;
                result = false;
            }

            return result;


        }

        


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";


            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.ToolTip = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

            lnkDraftList.Text = Resource.NewsLetterUnsentLettersHeading;
            lnkDraftList.ToolTip = Resource.NewsLetterUnsentLettersHeading;
            lnkDraftList.NavigateUrl = SiteRoot + "/eletter/LetterDrafts.aspx?l=" + letterInfoGuid.ToString();

            lnkManageTemplates.Text = Resource.LetterEditManageTemplatesLink;
            lnkManageTemplates.ToolTip = Resource.LetterEditManageTemplatesToolTip;
            lnkManageTemplates.NavigateUrl = SiteRoot + "/eletter/LetterTemplates.aspx";


            litHtmlTab.Text = Resource.HtmlTab;
            litPlainTextTab.Text = Resource.PlainTextTab;


            edContent.WebEditor.ToolBar = ToolBar.Newsletter;
            edContent.WebEditor.FullPageMode = true;
            edContent.WebEditor.EditorCSSUrl = string.Empty;

            edContent.WebEditor.Width = Unit.Percentage(100);
            edContent.WebEditor.Height = Unit.Pixel(800);

            btnSave.Text = Resource.NewsLetterSaveLetterButton;
            btnSave.ToolTip = Resource.NewsLetterSaveLetterButton;

            btnDelete.Text = Resource.NewsLetterDeleteLetterButton;
            btnDelete.ToolTip = Resource.NewsLetterDeleteLetterButton;
            //SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.NewsLetterEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.NewsLetterDeleteLetterButtonWarning);

            btnSendToList.Text = Resource.NewsLetterSendToSubscribersButton;
            btnSendToList.ToolTip = Resource.NewsLetterSendToSubscribersButton;
            UIHelper.AddConfirmationDialog(btnSendToList, Resource.NewsLetterSendToListButtonWarning);
            
            btnSendPreview.Text = Resource.NewsLetterSendPreviewButton;
            btnSendPreview.ToolTip = Resource.NewsLetterSendPreviewButton;

            btnLoadTemplate.Text = Resource.NewsLetterLoadHtmlTemplateButton;
            btnLoadTemplate.ToolTip = Resource.NewsLetterLoadHtmlTemplateButton;

            btnSaveAsTemplate.Text = Resource.NewsLetterSaveCurrentAsTemplateButton;
            btnSaveAsTemplate.ToolTip = Resource.NewsLetterSaveCurrentAsTemplateButtonToolTip;

            btnGeneratePlainText.Text = Resource.NewsletterGeneratePlainTextButton;
            btnGeneratePlainText.ToolTip = Resource.NewsletterGeneratePlainTextButton;

            UIHelper.AddConfirmationDialog(btnGeneratePlainText, Resource.NewsletterGeneratePlainTextButtonWarning);
            
            

        }

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", Guid.Empty);
            imageSiteRoot = WebUtils.GetSiteRoot();

            

            if (letterInfoGuid == Guid.Empty) return;

            letterInfo = new LetterInfo(letterInfoGuid);

            if (letterInfo.SiteGuid != siteSettings.SiteGuid)
            {
                letterInfo = null;
                letterInfoGuid = Guid.Empty;
            }

            letterGuid = WebUtils.ParseGuidFromQueryString("letter", Guid.Empty);

            if (letterGuid == Guid.Empty)
            {
                letter = new Letter();
            }
            else
            {
                letter = new Letter(letterGuid);

                if (letter.LetterInfoGuid != letterInfo.LetterInfoGuid)
                {
                    letterGuid = Guid.Empty;
                    letter = null;


                }
            }

            spnAdmin.Visible = WebUser.IsAdminOrContentAdmin;

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnLoadTemplate.Click += new EventHandler(btnLoadTemplate_Click);
            this.btnSaveAsTemplate.Click += new EventHandler(btnSaveAsTemplate_Click);
            this.btnSendPreview.Click += new EventHandler(btnSendPreview_Click);
            this.btnSendToList.Click += new EventHandler(btnSendToList_Click);
            this.btnGeneratePlainText.Click += new EventHandler(btnGeneratePlainText_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            IncludeYuiTabsCss = true;
            ScriptConfig.IncludeYuiTabs = true;
            
        }


        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            SiteUtils.SetupNewsletterEditor(edContent);
            edContent.WebEditor.UseFullyQualifiedUrlsForResources = true;
        }
        

        

        #endregion
    }
}
