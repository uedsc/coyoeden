/// Author:					Joe Audette
/// Created:				2007-09-23
/// Last Modified:			2009-12-16
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
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.ELetterUI
{
    public partial class LetterSubscribersPage : CBasePage
    {
        private LetterInfo letterInfo;
        private Guid letterInfoGuid = Guid.Empty;
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        private Double timeOffset = 0;
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private SubscriberRepository subscriptions = new SubscriberRepository();
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

        /// <summary>
        /// time offset for the current user
        /// </summary>
        protected Double TimeOffset
        {
            get { return timeOffset; }
        }

        /// <summary>
        /// True if the current user is in Admins role
        /// </summary>
        protected bool IsAdmin
        {
            get { return isAdmin; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (letterInfoGuid == Guid.Empty) return;

            letterInfo = new LetterInfo(letterInfoGuid);

            lnkThisPage.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsletterSubscriberListHeadingFormatString,
                letterInfo.Title);

            lnkThisPage.ToolTip = lnkThisPage.Text;

            litHeading.Text = lnkThisPage.Text;

            Title = litHeading.Text;
            
            

            if (Page.IsPostBack) return;

            BindGrid();

        }

        private void BindGrid()
        {

            using (IDataReader reader = subscriptions.GetPage(letterInfoGuid, pageNumber, pageSize, out totalPages))
            {
                grdSubscribers.DataSource = reader;
                grdSubscribers.PageIndex = pageNumber;
                grdSubscribers.PageSize = pageSize;
                grdSubscribers.DataBind();

                string pageUrl = SiteRoot + "/eletter/LetterSubscribers.aspx?l="
                    + letterInfoGuid.ToString() + "&amp;pagenumber={0}";

                pgrLetterSubscriber.PageURLFormat = pageUrl;
                pgrLetterSubscriber.ShowFirstLast = true;
                pgrLetterSubscriber.CurrentIndex = pageNumber;
                pgrLetterSubscriber.PageSize = pageSize;
                pgrLetterSubscriber.PageCount = totalPages;
                pgrLetterSubscriber.Visible = (totalPages > 1);

            }


            
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            using (IDataReader reader = subscriptions.Search(letterInfoGuid, txtSearchInput.Text))
            {
                grdSubscribers.DataSource = reader;
                grdSubscribers.DataBind();
            }
            
            pgrLetterSubscriber.Visible = false;

           
        }

        void btnDeleteUnVerified_Click(object sender, EventArgs e)
        {
            int daysOld = 90;
            int.TryParse(txtDaysOld.Text, out daysOld);
            DateTime cutOffDate = DateTime.UtcNow;
            if (daysOld > 0) { cutOffDate = DateTime.UtcNow.AddDays(-daysOld); }

            subscriptions.DeleteUnverified(letterInfoGuid, cutOffDate);

            WebUtils.SetupRedirect(this, SiteRoot + "/eletter/LetterSubscribers.aspx?l=" + letterInfoGuid.ToString());

        }

        void grdSubscribers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteSubscriber":

                    string strG = e.CommandArgument.ToString();
                    if (strG.Length == 36)
                    {
                        Guid subscriptionGuid = new Guid(strG);
                        LetterSubscriber s = subscriptions.Fetch(subscriptionGuid);
                        if (s != null) { subscriptions.Delete(s); }

                        LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

                        WebUtils.SetupRedirect(this, Request.RawUrl);
                    }

                    break;
            }

        }

        void grdSubscribers_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            Control c = e.Row.FindControl("btnDelete");
            if (c != null)
            {
                ImageButton btnDelete = c as ImageButton;
                UIHelper.AddConfirmationDialog(btnDelete, Resource.NewsletterSubscriberDeleteWarning);

            }
        }


        protected bool ShowUserLink(string userGuid)
        {
            if (!isAdmin) { return false; }

            if (userGuid != "00000000-0000-0000-0000-000000000000") { return true; }


            return false;

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
            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/LetterSubscribers.aspx?l=" + letterInfoGuid.ToString();

            btnSearch.Text = Resource.NewsletterSubscriberSearchButton;
            btnDeleteUnVerified.Text = Resource.NewsletterDeleteUnverifiedButton;
            UIHelper.AddConfirmationDialog(btnDeleteUnVerified, Resource.NewsletterDeletedUnverifiedWarning);

            grdSubscribers.Columns[0].HeaderText = Resource.NewsletterSubscriberListNameHeading;
            grdSubscribers.Columns[1].HeaderText = Resource.NewsletterSubscriberListEmailHeading;
            grdSubscribers.Columns[2].HeaderText = Resource.NewsletterSubscriberListHtmlFormatHeading;
            grdSubscribers.Columns[3].HeaderText = Resource.NewsletterSubscriberListBeginDateHeading;
            grdSubscribers.Columns[4].HeaderText = Resource.NewsletterSubscriberListIsVerifiedHeading;
            // grdSubscribers.Columns[4].HeaderText = Resource.

        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", Guid.Empty);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            isAdmin = WebUser.IsAdmin;
            spnAdmin.Visible = WebUser.IsAdminOrContentAdmin;

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            grdSubscribers.RowCreated += new System.Web.UI.WebControls.GridViewRowEventHandler(grdSubscribers_RowCreated);
            grdSubscribers.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(grdSubscribers_RowCommand);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnDeleteUnVerified.Click += new EventHandler(btnDeleteUnVerified_Click);
            SuppressMenuSelection();
            SuppressPageMenu();

            
        }

        

        

        
        #endregion
    }
}
