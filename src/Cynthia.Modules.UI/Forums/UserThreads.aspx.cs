/// Author:					Joe Audette
/// Created:				2008-03-18
/// Last Modified:			2009-12-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;
using log4net;
using Cynthia.Business;
using Resources;

namespace Cynthia.Web.GroupUI
{
    public partial class GroupUserThreadsPage : CBasePage
    {
        private int userId = -1;
        //private bool isAdmin = false;
        private int pageNumber = 1;
        private int pageSize = 20;
        private int totalPages = 1;
        private string editContentImage = ConfigurationManager.AppSettings["EditContentImage"];
        private string ThreadImage = ConfigurationManager.AppSettings["GroupThreadImage"];
        
        private Double timeOffset = 0;
        private static readonly ILog log = LogManager.GetLogger(typeof(GroupThreadView));
        private SiteUser forumUser = null;

        #region Protected Properties

        protected Double TimeOffset
        {
            get { return timeOffset; }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebConfigSettings.AllowUserThreadBrowsing)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            AddConnoicalUrl();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (forumUser == null) return;

            litTitle.Text = string.Format(CultureInfo.InvariantCulture,
                GroupResources.GroupUserThreadHeading,
                forumUser.Name);

            Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(CultureInfo.InvariantCulture,
                GroupResources.UserThreadTitleFormat, forumUser.Name));

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                GroupResources.UserThreadMetaFormat, forumUser.Name);

            using (IDataReader reader = GroupThread.GetPageByUser(
                userId,
                pageNumber,
                pageSize,
                out totalPages))
            {

                string pageUrl = siteSettings.SiteRoot
                    + "/Groups/UserThreads.aspx?"
                    + "userid=" + userId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;pagenumber={0}";

                pgrTop.PageURLFormat = pageUrl;
                pgrTop.ShowFirstLast = true;
                pgrTop.CurrentIndex = pageNumber;
                pgrTop.PageSize = pageSize;
                pgrTop.PageCount = totalPages;
                pgrTop.Visible = (pgrTop.PageCount > 1);

                pgrBottom.PageURLFormat = pageUrl;
                pgrBottom.ShowFirstLast = true;
                pgrBottom.CurrentIndex = pageNumber;
                pgrBottom.PageSize = pageSize;
                pgrBottom.PageCount = totalPages;
                pgrBottom.Visible = (pgrBottom.PageCount > 1);


                rptGroups.DataSource = reader;
                rptGroups.DataBind();
            }

        }

        private void AddConnoicalUrl()
        {
            if (Page.Header == null) { return; }

            Literal link = new Literal();
            link.ID = "threadurl";
            link.Text = "\n<link rel='canonical' href='"
                + SiteRoot
                + "/Groups/UserThreads.aspx?userid="
                + userId.ToInvariantString()
                + "&amp;pagenumber=" + pageNumber.ToInvariantString()
                + "' />";

            Page.Header.Controls.Add(link);

        }


        private void PopulateLabels()
        {
            pgrTop.NavigateToPageText = GroupResources.CutePagerNavigateToPageText;
            pgrTop.BackToFirstClause = GroupResources.CutePagerBackToFirstClause;
            pgrTop.GoToLastClause = GroupResources.CutePagerGoToLastClause;
            pgrTop.BackToPageClause = GroupResources.CutePagerBackToPageClause;
            pgrTop.NextToPageClause = GroupResources.CutePagerNextToPageClause;
            pgrTop.PageClause = GroupResources.CutePagerPageClause;
            pgrTop.OfClause = GroupResources.CutePagerOfClause;

            pgrBottom.NavigateToPageText = GroupResources.CutePagerNavigateToPageText;
            pgrBottom.BackToFirstClause = GroupResources.CutePagerBackToFirstClause;
            pgrBottom.GoToLastClause = GroupResources.CutePagerGoToLastClause;
            pgrBottom.BackToPageClause = GroupResources.CutePagerBackToPageClause;
            pgrBottom.NextToPageClause = GroupResources.CutePagerNextToPageClause;
            pgrBottom.PageClause = GroupResources.CutePagerPageClause;
            pgrBottom.OfClause = GroupResources.CutePagerOfClause;

        }

        private void LoadSettings()
        {
            userId = WebUtils.ParseInt32FromQueryString("userId", -1);
            
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            timeOffset = SiteUtils.GetUserTimeOffset();

            forumUser = new SiteUser(siteSettings, userId);
            if (forumUser.UserId == -1) forumUser = null;


        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        #endregion
    }
}
