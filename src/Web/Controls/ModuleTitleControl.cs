
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{
    public class ModuleTitleControl : WebControl, INamingContainer
    {
        #region Constructors

        public ModuleTitleControl()
		{
            if (HttpContext.Current == null) { return; }

			EnsureChildControls();

            
		}

		#endregion

        #region Control Declarations

        protected Literal litModuleTitle;
        protected HyperLink lnkModuleSettings;
        protected HyperLink lnkModuleEdit;
        protected ImageButton ibPostDraftContentForApproval;
        protected ImageButton ibApproveContent;
        protected HyperLink lnkRejectContent;
        protected ImageButton ibCancelChanges;
        protected ClueTipHelpLink statusLink;

        #endregion

        private string literalExtraMarkup = string.Empty;
        private bool disabledModuleSettingsLink = false;
        private Module module = null;

        private string editUrl = string.Empty;
        private string editText = string.Empty;
        //private bool useHTag = true;
        private bool canEdit = false;
        private bool forbidModuleSettings = false;
        private bool showEditLinkOverride = false;
        private bool enableWorkflow = false;
        private SiteModuleControl siteModule = null;
        private ContentWorkflowStatus workflowStatus = ContentWorkflowStatus.None;
        private string siteRoot = string.Empty;
        private bool isAdminEditor = false;
        private bool useHeading = true;

        private string columnId = UIHelper.CenterColumnId;
        private string artHeader = UIHelper.ArtisteerPostMetaHeader;
        private string artHeadingCss = UIHelper.ArtPostHeader;

        #region Public Properties

        public Module ModuleInstance
        {
            get { return module; }
            set { module = value; }
        }

        public string LiteralExtraMarkup
        {
            get { return literalExtraMarkup; }
            set { literalExtraMarkup = value; }
        }

        public string EditUrl
        {
            get { return editUrl; }
            set { editUrl = value; }

            
        }

        public string EditText
        {
            get { return editText; }
            set { editText = value; }
           
        }

        public bool UseHeading
        {
            get { return useHeading; }
            set { useHeading = value; }

        }

        public bool DisabledModuleSettingsLink
        {
            get { return disabledModuleSettingsLink; }
            set { disabledModuleSettingsLink = value; }
        }

        public bool CanEdit
        {
            get { return canEdit; }
            set { canEdit = value; }
            
        }

        public bool IsAdminEditor
        {
            get { return isAdminEditor; }
            set { isAdminEditor = value; }

        }

        public bool ShowEditLinkOverride
        {
            get { return showEditLinkOverride; }
            set { showEditLinkOverride = value; }
            
        }

        public ContentWorkflowStatus WorkflowStatus
        {
            get { return workflowStatus; }
            set { workflowStatus = value; }
        }

        private bool renderArtisteer = false;

        public bool RenderArtisteer
        {
            get { return renderArtisteer; }
            set { renderArtisteer = value; }
        }

        private bool useLowerCaseArtisteerClasses = false;

        public bool UseLowerCaseArtisteerClasses
        {
            get { return useLowerCaseArtisteerClasses; }
            set { useLowerCaseArtisteerClasses = value; }
        }

        #endregion

        private SiteModuleControl GetParentAsSiteModelControl(Control child)
        {
            if (HttpContext.Current == null) { return null; }

            if (child.Parent == null)
            {
                return null;
            }
            else if (child.Parent is SiteModuleControl)
            {
                return child.Parent as SiteModuleControl;
            }
            else
            {
                return GetParentAsSiteModelControl(child.Parent);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
           
            if (HttpContext.Current == null) 
            {
                writer.Write(String.Format("[{0}]", ID));
                return;
            }
            else
            {
                if ((useHeading)&&(renderArtisteer))
                {

                    writer.Write(String.Format("<div class=\"{0}\">\n", artHeader));

                    if ((artHeader == UIHelper.ArtisteerBlockHeader)||(artHeader == UIHelper.ArtisteerBlockHeaderLower))
                    {
                        writer.Write("<div class=\"l\"></div>");
                        writer.Write("<div class=\"r\"></div>");
                        writer.Write("<div class=\"art-header-tag-icon\">");
                        writer.Write("<div class=\"t\">");
                    }

                }
                /*
                if (module != null)
                {
                    writer.Write(String.Format("<a id='module{0}'></a>", module.ModuleId.ToString(CultureInfo.InvariantCulture)));
                }
                 */ 

                string headingTag = WebConfigSettings.ModuleTitleTag;

                if ((useHeading)&&(headingTag.Length > 0))
                {
                    writer.WriteBeginTag(headingTag);
                    writer.WriteAttribute("class", artHeadingCss + " moduletitle");
                    writer.Write(HtmlTextWriter.TagRightChar);
                }
                

                litModuleTitle.RenderControl(writer);

                if (CanEdit)
                {
                    if (!forbidModuleSettings)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkModuleSettings.RenderControl(writer);
                    }

                    if (ibCancelChanges != null && ibCancelChanges.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibCancelChanges.RenderControl(writer);
                    }

                    if (ibPostDraftContentForApproval != null && ibPostDraftContentForApproval.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibPostDraftContentForApproval.RenderControl(writer);
                    }

                    if (lnkRejectContent != null && lnkRejectContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkRejectContent.RenderControl(writer);
                    }

                    if (ibApproveContent != null && ibApproveContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibApproveContent.RenderControl(writer);
                    }

                    if (statusLink != null && statusLink.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        statusLink.ToolTip = Resource.WorkflowStatus;
                        statusLink.RenderControl(writer);
                    }
                }

                if (
                    (lnkModuleEdit != null)
                    && (EditUrl != null)
                    && (EditText != null)
                    )
                {
                    writer.Write(HtmlTextWriter.SpaceChar);
                    lnkModuleEdit.RenderControl(writer);
                }

                if (literalExtraMarkup.Length > 0)
                {
                    writer.Write(literalExtraMarkup);
                }

                if ((useHeading)&&(headingTag.Length > 0))
                {
                    writer.WriteEndTag(headingTag);
                }

                if ((useHeading) && (renderArtisteer))
                {
                    writer.Write("</div>");
                    if ((artHeader == UIHelper.ArtisteerBlockHeader) || (artHeader == UIHelper.ArtisteerBlockHeaderLower))
                    {
                        writer.Write("</div>");
                        writer.Write("</div>");
                    }
                }


            }
            
            
        }

        void ibApproveContent_Click(object sender, ImageClickEventArgs e)
        {
            SiteModuleControl siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.Approve();

        }

        protected void ibPostDraftContentForApproval_Click(object sender, ImageClickEventArgs e)
        {
            SiteModuleControl siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.SubmitForApproval();
           
        }

        protected void ibCancelChanges_Click(object sender, ImageClickEventArgs e)
        {
            SiteModuleControl siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.CancelChanges();
           
        }

       

        protected override void OnPreRender(EventArgs e)
        {
           
            base.OnPreRender(e);
            if (HttpContext.Current == null) { return; }

            if ((useHeading) && (renderArtisteer)) 
            { 
                columnId = this.GetColumnId();

                if (useLowerCaseArtisteerClasses)
                {
                    artHeader = UIHelper.ArtisteerPostMetaHeaderLower;
                    artHeadingCss = UIHelper.ArtPostHeaderLower;

                }

                switch (columnId)
                {
                    case UIHelper.LeftColumnId:
                    case UIHelper.RightColumnId:

                        if (useLowerCaseArtisteerClasses)
                        {
                            if ((artHeader == UIHelper.ArtisteerPostMetaHeader)||(artHeader == UIHelper.ArtisteerPostMetaHeaderLower))
                            {
                                artHeader = UIHelper.ArtisteerBlockHeaderLower;
                            }
                        }
                        else
                        {
                            if (artHeader == UIHelper.ArtisteerPostMetaHeader)
                            {
                                artHeader = UIHelper.ArtisteerBlockHeader;
                            }
                        }

                        artHeadingCss = string.Empty;

                        break;

                    case UIHelper.CenterColumnId:
                    default:


                        break;

                }
            }
            
            Initialize();
            

        }

        private void Initialize()
        {
            if (HttpContext.Current == null) { return; }
            
            
            
            siteModule = GetParentAsSiteModelControl(this);

            bool useTextLinksForFeatureSettings = true;
            CBasePage basePage = Page as CBasePage;
            if (basePage != null)
            {
                useTextLinksForFeatureSettings = basePage.UseTextLinksForFeatureSettings;
            }

            if (siteModule != null)
            {
                module = siteModule.ModuleConfiguration;
                CanEdit = siteModule.IsEditable;
                enableWorkflow = siteModule.EnableWorkflow;
                forbidModuleSettings = siteModule.ForbidModuleSettings;
                  
            }

            if (module != null)
            {
                if (module.ShowTitle)
                {
                    litModuleTitle.Text = Page.Server.HtmlEncode(module.ModuleTitle);
                }
                else
                {
                    useHeading = false;
                }

                if (CanEdit)
                {
                   
                    if (!disabledModuleSettingsLink)
                    {
                        lnkModuleSettings.Visible = true;
                        lnkModuleSettings.Text = Resource.SettingsLink;
                        lnkModuleSettings.ToolTip = Resource.ModuleEditSettings;

                        if (!useTextLinksForFeatureSettings)
                        {
                            lnkModuleSettings.ImageUrl = Page.ResolveUrl(String.Format("~/Data/SiteImages/{0}", WebConfigSettings.EditPropertiesImage));
                        }
                        else
                        {
                            // if its a text link make it small like the edit link
                            lnkModuleSettings.CssClass = "ModuleEditLink";
                        }

                        siteRoot = SiteUtils.GetNavigationSiteRoot();

                        lnkModuleSettings.NavigateUrl = String.Format("{0}/Admin/ModuleSettings.aspx?mid={1}&pageid={2}", siteRoot, module.ModuleId.ToInvariantString(), module.PageId.ToInvariantString());

                        if ((enableWorkflow) && (siteModule != null) && (siteModule is IWorkflow))
                        {
                            SetupWorkflowControls();
                            
                        }

                    }

                }

                if (
                    ((CanEdit) || (ShowEditLinkOverride))
                    && ((EditText != null) && (EditUrl != null)))
                {

                    lnkModuleEdit.Text = EditText;
                    if (this.ToolTip.Length > 0)
                    {
                        lnkModuleEdit.ToolTip = this.ToolTip;
                    }
                    else
                    {
                        lnkModuleEdit.ToolTip = EditText;
                    }
                    lnkModuleEdit.NavigateUrl = String.Format("{0}?mid={1}&pageid={2}", EditUrl, module.ModuleId.ToInvariantString(), module.PageId.ToInvariantString());

                    if (!useTextLinksForFeatureSettings)
                    {
                        lnkModuleEdit.ImageUrl = Page.ResolveUrl(String.Format("~/Data/SiteImages/{0}", WebConfigSettings.EditContentImage));
                    }

                }
                else {
                    lnkModuleEdit.Visible = false;
                }

            }
        }

        private void SetupWorkflowControls()
        {
            if (HttpContext.Current == null) { return; }

            if (siteModule == null) { return; }
            if (module == null) { return; }

           
            CmsPage cmsPage = this.Page as CmsPage;
            if ((cmsPage != null) && (cmsPage.ViewMode == PageViewMode.WorkInProgress))
            {
                switch (workflowStatus)
                {
                    case ContentWorkflowStatus.Draft:

                        ibPostDraftContentForApproval.ImageUrl = Page.ResolveUrl(WebConfigSettings.RequestApprovalImage);
                        ibPostDraftContentForApproval.ToolTip = Resource.RequestApprovalToolTip;
                        ibPostDraftContentForApproval.Visible = true;
                        statusLink.HelpKey = "workflowstatus-draft-help";
                        break;

                    case ContentWorkflowStatus.AwaitingApproval:

                        //if (WebUser.IsAdminOrContentAdminOrContentPublisher)
                        if(
                            (cmsPage.CurrentPage != null)
                            &&(
                            (isAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles)) || (WebUser.IsInRoles(this.module.AuthorizedEditRoles))
                            )
                            )
                        {
                            //add in the reject and approve links:                                            
                            ibApproveContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.ApproveContentImage);
                            ibApproveContent.Visible = true;
                            ibApproveContent.ToolTip = Resource.ApproveContentToolTip;

                            lnkRejectContent.NavigateUrl =
                                String.Format("{0}/Admin/RejectContent.aspx?mid={1}&pageid={2}", siteRoot, module.ModuleId.ToInvariantString(), module.PageId.ToInvariantString());

                            lnkRejectContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.RejectContentImage);
                            lnkRejectContent.ToolTip = Resource.RejectContentToolTip;
                            lnkRejectContent.Visible = true;
                        }

                        statusLink.HelpKey = "workflowstatus-awaitingapproval-help";

                        break;

                    case ContentWorkflowStatus.ApprovalRejected:
                        statusLink.HelpKey = "workflowstatus-rejected-help";
                        break;

                    
                }

                if (
                    (workflowStatus != ContentWorkflowStatus.Cancelled)
                    && (workflowStatus != ContentWorkflowStatus.Approved)
                    && (workflowStatus != ContentWorkflowStatus.None)
                    )
                {
                    //allow changes to be cancelled:                                            
                    ibCancelChanges.ImageUrl = Page.ResolveUrl(WebConfigSettings.CancelContentChangesImage);
                    ibCancelChanges.ToolTip = Resource.CancelChangesToolTip;
                    ibCancelChanges.Visible = true;
                }

            }
        }


        protected override void CreateChildControls()
        {
            if (HttpContext.Current == null) { return; }

            litModuleTitle = new Literal();
            lnkModuleSettings = new HyperLink();
            lnkModuleSettings.CssClass = "modulesettingslink";
            lnkModuleEdit = new HyperLink();
            
            lnkModuleEdit.CssClass = "ModuleEditLink";
            lnkModuleEdit.SkinID = "plain";


            ibPostDraftContentForApproval = new ImageButton();
            ibPostDraftContentForApproval.ID = "lbPostDraftContentForApproval";
            ibPostDraftContentForApproval.CssClass = "ModulePostDraftForApprovalLink";
            ibPostDraftContentForApproval.SkinID = "plain";
            ibPostDraftContentForApproval.Visible = false;
            ibPostDraftContentForApproval.Click += new ImageClickEventHandler(ibPostDraftContentForApproval_Click);
            this.Controls.Add(ibPostDraftContentForApproval);

            ibApproveContent = new ImageButton();
            ibApproveContent.ID = "ibApproveContent";
            ibApproveContent.CssClass = "ModuleApproveContentLink";
            ibApproveContent.SkinID = "plain";
            ibApproveContent.Visible = false;
            ibApproveContent.Click += new ImageClickEventHandler(ibApproveContent_Click);
            this.Controls.Add(ibApproveContent);

            lnkRejectContent = new HyperLink();
            lnkRejectContent.ID = "ibRejectContent";
            lnkRejectContent.CssClass = "ModuleRejectContentLink";
            lnkRejectContent.SkinID = "plain";
            lnkRejectContent.Visible = false;

            ibCancelChanges = new ImageButton();
            ibCancelChanges.ID = "ibCancelChanges";
            ibCancelChanges.CssClass = "ModuleCancelChangesLink";
            ibCancelChanges.SkinID = "plain";
            ibCancelChanges.Visible = false;
            UIHelper.AddConfirmationDialog(ibCancelChanges, Resource.CancelContentChangesButtonWarning);
            ibCancelChanges.Click += new ImageClickEventHandler(ibCancelChanges_Click);
            this.Controls.Add(ibCancelChanges);

            statusLink = new ClueTipHelpLink();
            
            this.Controls.Add(statusLink);


        }

        

    }
}
