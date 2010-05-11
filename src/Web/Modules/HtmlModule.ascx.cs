

using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;
namespace Cynthia.Web.ContentUI
{

    public partial class HtmlModule : SiteModuleControl, IWorkflow
    {
        #region Properties


        protected string EditContentImage = WebConfigSettings.EditContentImage;
		
        protected Double TimeOffset = 0;
        protected bool EnableContentRatingSetting = false;
        protected bool EnableRatingCommentsSetting = false;
        protected bool IncludeSwfObject = false;
        private bool enableSlideShow = false;
        private int slideContainerHeight = 0;
        private string slideTransitions = "fade";
        private bool pauseSlideOnHover = true;
        private int slideDuration = 3000;
        private int transitionSpeed = 1000;
        private string slideContainerClass = string.Empty;
        private bool randomizeSlides = false;
        private bool useSlideClearTypeCorrections = true;
        private HtmlRepository repository = new HtmlRepository();
        private bool isAdmin = false;

        #endregion

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e) 
		{
            LoadSettings();
            PopulateControls();
			
        }

        
		private void PopulateControls()
		{
            string htmlBody = String.Empty;
            bool retrieveHtml = true;

            if (EnableWorkflow)
            {
                //use C base page to see if user has toggled draft content:
                CmsPage cmsPage = this.Page as CmsPage;
                if (cmsPage != null)
                {
                    if (cmsPage.ViewMode == PageViewMode.WorkInProgress)
                    {
                        //try to get draft content:
                        ContentWorkflow workInProgress = ContentWorkflow.GetWorkInProgress(this.ModuleGuid);
                        if (workInProgress != null)
                        {
                            Title1.WorkflowStatus = workInProgress.Status;
                            htmlBody = workInProgress.ContentText;
                            retrieveHtml = false;
                        }
                    }
                }
            }

            if (retrieveHtml)
            {
                //html not yet retrieved:
                
                HtmlContent html = repository.Fetch(ModuleId);
                if (html != null)
                {
                    htmlBody = html.Body;
                }
            }

            //see if the literal has already been added:
            Literal literalHtml = divContent.FindControl("literalHtml") as Literal;

            if (!String.IsNullOrEmpty(htmlBody))
            {
                if (literalHtml == null)
                {
                    literalHtml = new Literal();
                    literalHtml.ID = "literalHtml";
                    divContent.Controls.Add(literalHtml);
                }
                literalHtml.Text = htmlBody;
            }
            else if (literalHtml != null)
            {
                literalHtml.Text = String.Empty;
            }

		}


        private void LoadSettings()
        {
            Title1.EditUrl = SiteRoot + "/HtmlEdit.aspx";
            Title1.EditText = Resource.EditImageAltText;
            Title1.ToolTip = Resource.EditImageAltText;
            
            Title1.Visible = !this.RenderInWebPartMode;

            if ((WebUser.IsAdminOrContentAdmin) || (SiteUtils.UserIsSiteEditor()))
            { 
                isAdmin = true;
                Title1.IsAdminEditor = isAdmin;
            }

            if (IsEditable)
            {
				TitleUrl = String.Format("{0}/HtmlEdit.aspx?mid={1}&pageid={2}", SiteRoot, ModuleId, CurPageSettings.PageId);
            }

            TimeOffset = SiteUtils.GetUserTimeOffset();

            
            EnableContentRatingSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableContentRatingSetting", EnableContentRatingSetting);

            EnableRatingCommentsSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableRatingCommentsSetting", EnableRatingCommentsSetting);

            IncludeSwfObject = WebUtils.ParseBoolFromHashtable(
                Settings, "IncludeSwfObjectSetting", IncludeSwfObject);

            enableSlideShow = WebUtils.ParseBoolFromHashtable(
                Settings, "HtmlEnableSlideShow", enableSlideShow);

            slideContainerHeight = WebUtils.ParseInt32FromHashtable(
                Settings, "HtmlSlideContainerHeight", slideContainerHeight);

            pauseSlideOnHover = WebUtils.ParseBoolFromHashtable(
                Settings, "HtmlSlideShowPauseOnHover", pauseSlideOnHover);

            slideDuration = WebUtils.ParseInt32FromHashtable(
                Settings, "HtmlSlideDuration", slideDuration);

            transitionSpeed = WebUtils.ParseInt32FromHashtable(
               Settings, "HtmlTransitionSpeed", transitionSpeed);


            

            randomizeSlides = WebUtils.ParseBoolFromHashtable(
                Settings, "HtmlSlideShowRandomizeSlides", randomizeSlides);

            useSlideClearTypeCorrections = WebUtils.ParseBoolFromHashtable(
                Settings, "HtmlSlideShowUseExtraClearTypeCorrections", useSlideClearTypeCorrections);


            //


            if (Settings.Contains("HtmlSlideTransitions"))
            {
                slideTransitions = Settings["HtmlSlideTransitions"].ToString();
            }

            if (Settings.Contains("HtmlSlideContainerClass"))
            {
                slideContainerClass = Settings["HtmlSlideContainerClass"].ToString().Trim();
            }

            if ((IncludeSwfObject)&&(Page is CBasePage))
            {
                CBasePage p = Page as CBasePage;
                if (p != null) { p.ScriptConfig.IncludeSwfObject = true; }
            }
            
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

           
            Rating.Enabled = EnableContentRatingSetting;
            Rating.AllowFeedback = EnableRatingCommentsSetting;
            Rating.ContentGuid = ModuleGuid;
           

            pnlContainer.ModuleId = this.ModuleId;

            if (enableSlideShow)
            {
                divContent.EnableSlideShow = true;
                divContent.Random = randomizeSlides;
                divContent.CleartypeNoBg = !useSlideClearTypeCorrections;
                divContent.SlideContainerClass = slideContainerClass;
                divContent.PauseOnHover = pauseSlideOnHover;
                divContent.TransitionEffect = slideTransitions;
                divContent.TransitionInterval = slideDuration;
                divContent.Speed = transitionSpeed;

                if (slideContainerHeight > 0) { divContent.ContainerHeight = slideContainerHeight.ToInvariantString() + "px"; }
            }

        }

        void html_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["HtmlContentIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        #region IWorkflow Members

        public void SubmitForApproval()
        {
            if (CurPageSettings == null) { return; }

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { return; }

            ContentWorkflow workInProgress = ContentWorkflow.GetWorkInProgress(ModuleGuid);
            if (workInProgress == null) { return; }

            Module module = new Module(workInProgress.ModuleId);

            workInProgress.Status = ContentWorkflowStatus.AwaitingApproval;
            workInProgress.LastModUserGuid = currentUser.UserGuid;
            workInProgress.LastModUtc = DateTime.UtcNow;
            workInProgress.Save();

            if (!WebConfigSettings.DisableWorkflowNotification)
            {
                string approverRoles = CurPageSettings.EditRoles + module.AuthorizedEditRoles;

                WorkflowHelper.SendApprovalRequestNotification(
                    SiteUtils.GetSmtpSettings(),
                    SiteSettings,
                    currentUser,
                    workInProgress,
                    approverRoles,
                    SiteUtils.GetCurrentPageUrl());
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

            
        }

        public void CancelChanges()
        {
            
            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { return; }

            ContentWorkflow workInProgress = ContentWorkflow.GetWorkInProgress(ModuleGuid);
            if (workInProgress == null) { return; }

            workInProgress.Status = ContentWorkflowStatus.Cancelled;
            workInProgress.LastModUserGuid = currentUser.UserGuid;
            workInProgress.LastModUtc = DateTime.UtcNow;
            workInProgress.Save();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        public void Approve()
        {
            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { return; }

            HtmlContent html = repository.Fetch(ModuleId);
            if (html != null)
            {
                html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);
                html.ApproveContent(SiteSettings.SiteGuid, currentUser.UserGuid);
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        #endregion

    }
}
