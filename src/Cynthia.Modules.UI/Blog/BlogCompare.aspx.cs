// Author:					Joe Audette
// Created:				    2009-04-09
// Last Modified:		    2009-04-10
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

namespace Cynthia.Web.BlogUI
{
    public partial class BlogCompare : CDialogBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private int itemId = -1;
        private Guid historyGuid = Guid.Empty;
        protected Double timeOffset = 0;
        protected string currentFloat = "left";
        protected string historyFloat = "right";
        //private Module module = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadParams();
            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (moduleId == -1) { return; }
            if (itemId == -1) { return; }
            //if (module == null) { return; }
            if (historyGuid == Guid.Empty) { return; }

            Blog blog = new Blog(itemId);
            if (blog.ModuleId != moduleId) { return; }

            ContentHistory history = new ContentHistory(historyGuid);
            if (history.ContentGuid != blog.BlogGuid) { return; }

            litCurrentHeading.Text = string.Format(BlogResources.CurrentVersionHeadingFormat,
                DateTimeHelper.GetTimeZoneAdjustedDateTimeString(blog.LastModUtc, timeOffset));

            litCurrentVersion.Text = blog.Description;

            litHistoryHead.Text = string.Format(BlogResources.VersionAsOfHeadingFormat,
                DateTimeHelper.GetTimeZoneAdjustedDateTimeString(history.CreatedUtc, timeOffset));

            litHistoryVersion.Text = history.ContentText;

            string onClick = "top.window.LoadHistoryInEditor('" + historyGuid.ToString() + "');  return false;";
            btnRestore.Attributes.Add("onclick", onClick);

        }

        void btnRestore_Click(object sender, EventArgs e)
        {
            // this should only fire if javascript is disabled because we put a client side on click
            string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/Blog/EditPost.aspx?mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                + "&ItemID=" + itemId.ToString(CultureInfo.InvariantCulture)
                + "&pageid=" + pageId.ToString(CultureInfo.InvariantCulture) + "&r=" + historyGuid.ToString();

            Response.Redirect(redirectUrl);
        }




        private void PopulateLabels()
        {
            btnRestore.Text = BlogResources.RestoreToEditorButton;
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();

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
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
            historyGuid = WebUtils.ParseGuidFromQueryString("h", historyGuid);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnRestore.Click += new EventHandler(btnRestore_Click);
        }
    }
}
