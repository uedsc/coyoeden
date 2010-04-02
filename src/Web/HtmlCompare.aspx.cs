// Author:					Joe Audette
// Created:				    2009-04-08
// Last Modified:		    2009-04-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.ContentUI
{
    public partial class HtmlCompare : CDialogBasePage
    {

        private int pageId = -1;
        private int moduleId = -1;
        private Guid historyGuid = Guid.Empty;
        private Guid workflowGuid = Guid.Empty;
        protected Double timeOffset = 0;
        //private Module module = null;
        protected string currentFloat = "left";
        protected string historyFloat = "right";
        private bool userCanEdit = false;
        private bool userCanEditAsDraft = false;
        private HtmlRepository repository = new HtmlRepository();

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadParams();
            userCanEdit = UserCanEditModule(moduleId);
            userCanEditAsDraft = UserCanOnlyEditModuleAsDraft(moduleId);

            if ((!userCanEdit) && (!userCanEditAsDraft))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (moduleId == -1) { return; }
            //if (module == null) { return; }
            if (historyGuid != Guid.Empty) 
            {
                ShowVsHistory();
                return; 
            }

            if (workflowGuid != Guid.Empty)
            {
                ShowVsDraft();
            }
            

        }

        private void ShowVsHistory()
        {
            HtmlContent html = repository.Fetch(moduleId);
            ContentHistory history = new ContentHistory(historyGuid);
            if (history.ContentGuid != html.ModuleGuid) { return; }

            litCurrentHeading.Text = string.Format(Resource.CurrentVersionHeadingFormat,
                DateTimeHelper.GetTimeZoneAdjustedDateTimeString(html.LastModUtc, timeOffset));

            litCurrentVersion.Text = html.Body;

            litHistoryHead.Text = string.Format(Resource.VersionAsOfHeadingFormat,
                DateTimeHelper.GetTimeZoneAdjustedDateTimeString(history.CreatedUtc, timeOffset));

            litHistoryVersion.Text = history.ContentText;

            string onClick = "top.window.LoadHistoryInEditor('" + historyGuid.ToString() + "');  return false;";
            btnRestore.Attributes.Add("onclick", onClick);
        }

        private void ShowVsDraft()
        {
            HtmlContent html = repository.Fetch(moduleId);

            if (html == null) { return; }

            ContentWorkflow draftContent = ContentWorkflow.GetWorkInProgress(html.ModuleGuid);

            if (draftContent.Guid != workflowGuid) { return; }

            litCurrentHeading.Text = string.Format(Resource.CurrentDraftHeadingFormat,
                DateTimeHelper.GetTimeZoneAdjustedDateTimeString(draftContent.RecentActionOn, timeOffset));

            litCurrentVersion.Text = draftContent.ContentText;

            litHistoryHead.Text = string.Format(Resource.CurrentVersionHeadingFormat,
                DateTimeHelper.GetTimeZoneAdjustedDateTimeString(html.LastModUtc, timeOffset));

            litHistoryVersion.Text = html.Body;

            //string onClick = "top.window.LoadHistoryInEditor('" + historyGuid.ToString() + "');  return false;";
            //btnRestore.Attributes.Add("onclick", onClick);
            btnRestore.Visible = false;
        }

        void btnRestore_Click(object sender, EventArgs e)
        {
            // this should only fire if javascript is disabled because we put a client side on click
            string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/HtmlEdit.aspx?mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                + "&pageid=" + pageId.ToString(CultureInfo.InvariantCulture) + "&r=" + historyGuid.ToString();

            Response.Redirect(redirectUrl);
        }
        



        private void PopulateLabels()
        {
            btnRestore.Text = Resource.RestoreToEditorButton;
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();

            //if (moduleId > -1)
            //{
            //    module = new Module(moduleId, pageId);
            //}
            if (CultureInfo.CurrentCulture.TextInfo.IsRightToLeft)
            {
                currentFloat = "right";
                historyFloat = "left";

            }

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            historyGuid = WebUtils.ParseGuidFromQueryString("h", historyGuid);
            workflowGuid = WebUtils.ParseGuidFromQueryString("d", workflowGuid);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnRestore.Click += new EventHandler(btnRestore_Click);
        }

        
    }
}
