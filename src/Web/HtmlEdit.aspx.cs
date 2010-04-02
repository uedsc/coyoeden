/// Author:                     Joe Audette
/// Created:                    2004-08-29
///	Last Modified:              2010-01-24

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.ContentUI 
{
	
    public partial class EditHtml : CBasePage
	{
        
		private Hashtable moduleSettings;
        private HtmlContent html;
        private bool useMultipleItems = false;
        private bool useExcerpt = false;
        private bool useMoreLink = false;
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Module module = null;
        private int itemId = -1;
        private String cacheDependencyKey;
        private string virtualRoot;
        protected Double timeOffset = 0;

        private bool HtmlEnableVersioningSetting = false;
        private bool enableContentVersioning = false;
        //private bool enableContentWorkflow = false;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 1;
        private SiteUser currentUser = null;
        private Guid restoreGuid = Guid.Empty;
        protected bool isAdmin = false;
        private ContentWorkflow workInProgress;
        private bool userCanEdit = false;
        private bool userCanEditAsDraft = false;
        private HtmlRepository repository = new HtmlRepository();


        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            SiteUtils.SetupEditor(edContent);
            //edContent.ID = "edContent";

            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SuppressPageMenu();
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            btnUpdateDraft.Click += new EventHandler(btnUpdateDraft_Click);
            //this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            //this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            grdHistory.RowCommand += new GridViewCommandEventHandler(grdHistory_RowCommand);
            grdHistory.RowDataBound += new GridViewRowEventHandler(grdHistory_RowDataBound);
            pgrHistory.Command += new CommandEventHandler(pgrHistory_Command);
            btnRestoreFromGreyBox.Click += new System.Web.UI.ImageClickEventHandler(btnRestoreFromGreyBox_Click);
            btnDeleteHistory.Click += new EventHandler(btnDeleteHistory_Click);
            
        }

        
        #endregion

        

        private void Page_Load(object sender, EventArgs e)
		{

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            SecurityHelper.DisableBrowserCache();

            LoadParams();

            userCanEdit = UserCanEditModule(moduleId);
            userCanEditAsDraft = UserCanOnlyEditModuleAsDraft(moduleId);

            if ((!userCanEdit)&&(!userCanEditAsDraft))
			{
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            
            LoadSettings();
			PopulateLabels();

			if (!Page.IsPostBack) 
			{
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

                }
				PopulateControls();
			}
		}

		

		private void PopulateControls()
		{
            if (html == null) { return; }
            this.itemId = html.ItemId;
            
            edContent.Text = html.Body;

            if ((workInProgress != null)&&(ViewMode == PageViewMode.WorkInProgress))
            {
                pnlWorkflowStatus.Visible = true;
                edContent.Text = workInProgress.ContentText;

                litRecentActionBy.Text = workInProgress.RecentActionByUserLogin;
                litRecentActionOn.Text = workInProgress.RecentActionOn.ToString();
                lnkCompareDraftToLive.Visible = true;
                lnkCompareDraftToLive.NavigateUrl = SiteRoot 
                    + "/HtmlCompare.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture) 
                    + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture) + "&d=" + workInProgress.Guid.ToString();
                
                switch (workInProgress.Status)
                {
                    case ContentWorkflowStatus.ApprovalRejected:

                        litWorkflowStatus.Text = Resource.ContentWasRejected;
                        lblRecentActionBy.ConfigKey = "RejectedBy";
                        lblRecentActionOn.ConfigKey = "RejectedOn";
                        litCreatedBy.Text = workInProgress.CreatedByUserName;
                        ltlRejectionReason.Text = workInProgress.Notes;
                        divRejection.Visible = true;

                        break;

                    case ContentWorkflowStatus.AwaitingApproval:

                        litWorkflowStatus.Text = Resource.ContentAwaitingApproval;
                        lblRecentActionBy.ConfigKey = "ContentLastEditBy";
                        lblRecentActionOn.ConfigKey = "ContentLastEditDate";
                        ltlRejectionReason.Text = string.Empty;
                        divRejection.Visible = false;

                        break;

                    case ContentWorkflowStatus.Draft:

                        litWorkflowStatus.Text = Resource.ContentEditsInProgress;
                        lblRecentActionBy.ConfigKey = "ContentLastEditBy";
                        lblRecentActionOn.ConfigKey = "ContentLastEditDate";
                        ltlRejectionReason.Text = string.Empty;
                        divRejection.Visible = false;

                        break;

                }
                
            }
            

			if((this.itemId == -1)||(!this.useMultipleItems))
			{
				this.btnDelete.Visible = false;

			}

            if (enableContentVersioning)
            {
                BindHistory();

            }

            if (restoreGuid != Guid.Empty)
            {
                ContentHistory rHistory = new ContentHistory(restoreGuid);
                if (rHistory.ContentGuid == html.ModuleGuid)
                {
                    edContent.Text = rHistory.ContentText;
                }

            }
		}



        private void SaveHtml(bool draft)
        {
            if (html == null) { return; }

            this.itemId = html.ItemId;

            html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);

            bool saveHtml = true;
            string htmlBody = edContent.Text;

            if (draft)
            {
                //ContentWorkflow wip = ContentWorkflow.GetWorkInProgress(this.module.ModuleGuid);
                //dont update the actual data, but edit/create the draft version:
                if (workInProgress != null)
                {
                    
                    if (workInProgress.Status != ContentWorkflowStatus.Draft)
                    {
                        if ((workInProgress.Status == ContentWorkflowStatus.AwaitingApproval) && (userCanEdit))
                        {
                            // do nothing, let the editor update the draft without changing the status
                        }
                        else
                        {
                            //otherwise set the status back to draft
                            workInProgress.Status = ContentWorkflowStatus.Draft;
                        }
                    }

                    workInProgress.ContentText = edContent.Text;
                    workInProgress.Save();
                }
                else
                {
                    //draft version doesn't exist - create it:
                    ContentWorkflow.CreateDraftVersion(
                        siteSettings.SiteGuid,
                        edContent.Text,
                        string.Empty,
                        -1,
                        Guid.Empty,
                        this.module.ModuleGuid,
                        currentUser.UserGuid);
                }

                //if this is a new item, then we want to save it even though we will be saving it with no html, 
                //this ensures that there is a record there from the start:
                if (html.ItemId < 1)
                {
                    saveHtml = true;
                    //save with no content as there will be no live content
                    htmlBody = String.Empty;
                }
                else
                {
                    saveHtml = false;
                }
            }

            if (saveHtml)
            {
                //update existing HTML content:                

                html.Body = htmlBody;
                html.LastModUserGuid = currentUser.UserGuid;
                if (module != null)
                {
                    html.ModuleGuid = module.ModuleGuid;
                }
                html.LastModUtc = DateTime.UtcNow;


                if (enableContentVersioning && !draft)
                {
                    html.CreateHistory(siteSettings.SiteGuid);
                }

                repository.Save(html);
            }
            CurrentPage.UpdateLastModifiedTime();
            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);

            SiteUtils.QueueIndexing();


            if (hdnReturnUrl.Value.Length > 0)
            {

                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }


        private void btnUpdate_Click(Object sender, EventArgs e)
        {
            SaveHtml(false);
        }

        private void btnUpdateDraft_Click(object sender, EventArgs e)
        {
            SaveHtml(true);
        }

        void html_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["HtmlContentIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        ///// <summary>
        ///// This is a deprecated function for when multiple items was enabled. It was poorly implemented and has been removed as of 2.3.0.0
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    //if(this.itemId > -1)
        //    if(html != null)
        //    {
        //        //HtmlContent htmlContent = new HtmlContent(this.moduleId, this.itemId);
        //        //htmlContent.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);
        //        //htmlContent.Delete();
        //        html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);
        //        repository.Delete(html);
        //        CurrentPage.UpdateLastModifiedTime();
        //        SiteUtils.QueueIndexing();
        //    }
        //    if (hdnReturnUrl.Value.Length > 0)
        //    {
        //        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
        //        return;
        //    }

        //    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        //}

        

       
        //private void btnCancel_Click(Object sender, EventArgs e)
        //{
        //    if (hdnReturnUrl.Value.Length > 0)
        //    {
        //        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
        //        return;
        //    }

        //    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        //}

        private void BindHistory()
        {
            if ((html == null))
            {
                pnlHistory.Visible = false;
                return; 
            }

            if ((module != null) && (html.ModuleGuid == Guid.Empty))
            {
                html.ModuleGuid = module.ModuleGuid;
            }


            List<ContentHistory> history = ContentHistory.GetPage(html.ModuleGuid, pageNumber, pageSize, out totalPages);
            pgrHistory.ShowFirstLast = true;
            pgrHistory.PageSize = pageSize;
            pgrHistory.PageCount = totalPages;
            pgrHistory.Visible = (this.totalPages > 1);
          
            grdHistory.DataSource = history;
            grdHistory.DataBind();

            btnDeleteHistory.Visible = (grdHistory.Rows.Count > 0);
            pnlHistory.Visible = (grdHistory.Rows.Count > 0);

        }

        void pgrHistory_Command(object sender, CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgrHistory.CurrentIndex = pageNumber;
            BindHistory();
        }

        void grdHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string g = e.CommandArgument.ToString();
            if (g.Length != 36) { return; }
            Guid historyGuid = new Guid(g);

            switch (e.CommandName)
            {
                case "RestoreToEditor":
                    ContentHistory history = new ContentHistory(historyGuid);
                    if (history.Guid == Guid.Empty) { return; }

                    edContent.Text = history.ContentText;
                    BindHistory();
                    break;

                case "DeleteHistory":
                    ContentHistory.Delete(historyGuid);
                    BindHistory();
                    break;

                default:

                    break;
            }
        }

        void grdHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //Button btnDelete = (Button)e.Row.Cells[0].FindControl("btnDelete");
            Button btnDelete = (Button)e.Row.FindControl("btnDelete");

            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.DeleteHistoryItemWarning + "');");
            }

        }

        void btnRestoreFromGreyBox_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnHxToRestore.Value.Length != 36) 
            {
                BindHistory();
                return; 
            }

            Guid h = new Guid(hdnHxToRestore.Value);

            ContentHistory history = new ContentHistory(h);
            if (history.Guid == Guid.Empty) { return; }

            edContent.Text = history.ContentText;
            BindHistory();

        }

        void btnDeleteHistory_Click(object sender, EventArgs e)
        {
            if (html == null) { return; }

            ContentHistory.DeleteByContent(html.ModuleGuid);
            BindHistory();
            updHx.Update();

        }

        private void SetupHistoryRestoreScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");
            script.Append("function LoadHistoryInEditor(hxGuid) {");

            script.Append("GB_hide();");
            //script.Append("alert(hxGuid);");

            script.Append("var hdn = document.getElementById('" + this.hdnHxToRestore.ClientID + "'); ");
            script.Append("hdn.value = hxGuid; ");
            script.Append("var btn = document.getElementById('" + this.btnRestoreFromGreyBox.ClientID + "');  ");
            script.Append("btn.click(); ");
            script.Append("}");
            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page),"gbHandler", script.ToString());

        }
        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(Resource.EditHtmlTitleFormat, GetModuleTitle(moduleId)));

            

            btnUpdate.Text = Resource.EditHtmlUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, AccessKeys.EditHtmlUpdateButtonAccessKey);
            //UIHelper.DisableButtonAfterClick(
            //    btnUpdate,
            //    Resource.ButtonDisabledPleaseWait,
            //    Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
            //    );

            

            lnkCancel.Text = Resource.EditHtmlCancelButton;
            //btnCancel.Text = Resource.EditHtmlCancelButton;
            //SiteUtils.SetButtonAccessKey(btnCancel, AccessKeys.EditHtmlCancelButtonAccessKey);

            btnDelete.Text = Resource.EditHtmlDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.EditHtmlDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.HtmlDeleteContentWarning);
            edContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            

            litVersionHistory.Text = Resource.ContentVersionHistory;

            grdHistory.Columns[0].HeaderText = Resource.CreatedDateGridHeader;
            grdHistory.Columns[1].HeaderText = Resource.ArchiveDateGridHeader;

            btnRestoreFromGreyBox.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnRestoreFromGreyBox.AlternateText = " ";

            btnDeleteHistory.Text = Resource.DeleteAllHistoryButton;
            UIHelper.AddConfirmationDialog(btnDeleteHistory, Resource.DeleteAllHistoryWarning);

            lnkCompareDraftToLive.Text = Resource.CompareDraftToLiveLink;
            lnkCompareDraftToLive.ToolTip = Resource.CompareDraftToLiveTooltip;
            lnkCompareDraftToLive.DialogCloseText = Resource.DialogCloseLink;


        }

        private void LoadParams()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            restoreGuid = WebUtils.ParseGuidFromQueryString("r", restoreGuid);
            cacheDependencyKey = "Module-" + moduleId.ToString();


        }

        private void LoadSettings()
        {
            
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            virtualRoot = WebUtils.GetApplicationRoot();
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            currentUser = SiteUtils.GetCurrentSiteUser();
            timeOffset = SiteUtils.GetUserTimeOffset();
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            module = new Module(moduleId);

            if (module.ModuleTitle.Length > 0)
            {
                litModuleTitle.Text = Server.HtmlEncode(module.ModuleTitle);
                lblHtmlSettings.Visible = false;
            }

            pageSize = WebUtils.ParseInt32FromHashtable(moduleSettings, "HtmlVersionPageSizeSetting", pageSize);
            enableContentVersioning = WebUtils.ParseBoolFromHashtable(moduleSettings, "HtmlEnableVersioningSetting", HtmlEnableVersioningSetting);

            if ((siteSettings.ForceContentVersioning)||(WebConfigSettings.EnforceContentVersioningGlobally))
            {
                enableContentVersioning = true;
            }


            useExcerpt = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "HtmlUseExcerptSetting", false);

           

            useMoreLink = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "HtmlUseMoreLinkSetting", false);

            

            useMultipleItems = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "HtmlMultipleItemsSetting", false);

           

            if((WebUser.IsAdminOrContentAdmin)||(SiteUtils.UserIsSiteEditor())) { isAdmin = true;}
            

            edContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            
            if (moduleSettings.Contains("HtmlEditorHeightSetting"))
            {
                edContent.WebEditor.Height = Unit.Parse(moduleSettings["HtmlEditorHeightSetting"].ToString());
            }

            

            divHistoryDelete.Visible = (enableContentVersioning && isAdmin);

            pnlHistory.Visible = enableContentVersioning;

            if (enableContentVersioning)
            {
                SetupHistoryRestoreScript();
            }

            html = repository.Fetch(moduleId);
            if (html == null) 
            { 
                html = new HtmlContent();
                html.ModuleId = moduleId;
                html.ModuleGuid = module.ModuleGuid;
            }

            //if (this.itemId > -1)
            //{
            //    html = repository.Fetch(moduleId, itemId);
            //}
            //else
            //{
            //    if (useMultipleItems)
            //    {
            //        html = new HtmlContent();
            //        html.ModuleId = this.moduleId;
            //        html.CreatedBy = currentUser.UserId;
            //    }
            //    else
            //    {
            //        html = repository.Fetch(moduleId);
            //    }
            //}

            if ((!userCanEdit) && (userCanEditAsDraft))
            {
                btnUpdate.Visible = false;
                btnUpdateDraft.Visible = true;
            }

            btnUpdateDraft.Text = Resource.EditHtmlUpdateDraftButton;

            if ((WebConfigSettings.EnableContentWorkflow)&&(siteSettings.EnableContentWorkflow))
            {
                workInProgress = ContentWorkflow.GetWorkInProgress(this.module.ModuleGuid);
                //bool draftOnlyAccess = UserCanOnlyEditModuleAsDraft(moduleId);

                if (workInProgress != null)
                {
                    // let editors toggle between draft and live view in the editor
                    if (userCanEdit) { SetupWorkflowControls(); }

                    switch (workInProgress.Status)
                    {
                        case ContentWorkflowStatus.Draft:

                            //there is a draft version currently available, therefore dont allow the non draft version to be edited:
                            btnUpdateDraft.Visible = true;
                            btnUpdate.Visible = false;
                            if (ViewMode == PageViewMode.WorkInProgress)
                            {
                                litModuleTitle.Text += " - " + Resource.ApprovalProcessStatusDraft;
                            }
                            break;

                        case ContentWorkflowStatus.ApprovalRejected:
                            //rejected content - allow update as draft only
                            btnUpdateDraft.Visible = true;
                            btnUpdate.Visible = false;
                            if (ViewMode == PageViewMode.WorkInProgress)
                            {
                                litModuleTitle.Text += " - " + Resource.ApprovalProcessStatusRejected;
                            }
                            break;

                        case ContentWorkflowStatus.AwaitingApproval:
                            //pending approval - dont allow any edited:
                            // 2010-01-18 let editors update the draft if they want to before approving it.
                            btnUpdateDraft.Visible = userCanEdit;
                            
                            btnUpdate.Visible = false;
                            if (ViewMode == PageViewMode.WorkInProgress)
                            {
                                litModuleTitle.Text += " - " + Resource.ApprovalProcessStatusAwaitingApproval;
                            }
                            break;
                    }
                }
                else
                {
                    //workInProgress is null there is no draft
                    if (userCanEdit)
                    {
                        btnUpdateDraft.Text = Resource.CreateDraftButton;
                        btnUpdateDraft.Visible = true;
                    }

                }

                if((userCanEdit)&&(ViewMode == PageViewMode.Live))
                {
                    btnUpdateDraft.Visible = false;
                    btnUpdate.Visible = true;
                }
               
            }

        }


        
		

	}
}
