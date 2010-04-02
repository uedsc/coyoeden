/// Author:					Joe Audette
/// Created:				2008-06-14
/// Last Modified:			2009-06-21
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using Cynthia.Web.Framework;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.AdminUI
{
    public partial class ContentWorkflowPage : CBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!WebUser.IsAdminOrContentAdminOrContentPublisher)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
                return;
            }

            
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuContentWorkflowLabel);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCurrentPage.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/ContentWorkflow.aspx";

            litAdminHeading.Text = Resource.AdminMenuContentWorkflowLabel;

            lnkAwaitingApproval.Text = Resource.AwaitingApprovalHeading;
            lnkAwaitingApproval.NavigateUrl = SiteRoot + "/Admin/ContentAwaitingApproval.aspx";

            lnkRejectedContent.Text = Resource.RejectedContentHeading;
            lnkRejectedContent.NavigateUrl = SiteRoot + "/Admin/RejectedContent.aspx";

            lnkPendingPages.Text = Resource.PendingPagesTitle;
            lnkPendingPages.NavigateUrl = SiteRoot + "/Admin/PendingPages.aspx";

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
