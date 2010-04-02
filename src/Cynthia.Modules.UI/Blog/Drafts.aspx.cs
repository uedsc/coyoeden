/// Author:					Joe Audette
/// Created:				2007-12-14
/// Last Modified:			2008-11-06
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.BlogUI
{
    public partial class BlogDraftsPage : CBasePage
    {
        #region Properties

        private int moduleId = 0;
        private int pageId = -1;
        private Double timeOffset = 0;
        private string blogDateTimeFormat;

        protected string BlogDateTimeFormat
        {
            get { return blogDateTimeFormat; }

        }

        protected int PageId
        {
            get { return pageId; }
        }

        protected int ModuleId
        {
            get { return moduleId; }
        }

        protected Double TimeOffset
        {
            get { return timeOffset; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();
            
            LoadSettings();

            if (!WebUser.HasEditPermissions(siteSettings.SiteId, moduleId, CurrentPage.PageId))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            using (IDataReader reader = Blog.GetDrafts(moduleId))
            {
                rptDrafts.DataSource = reader;
                rptDrafts.DataBind();
            }

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.BlogDraftsLink);
            this.litHeader.Text = BlogResources.BlogDraftsHeading;

            blogDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
            }

        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);


        }

        #endregion
    }
}
