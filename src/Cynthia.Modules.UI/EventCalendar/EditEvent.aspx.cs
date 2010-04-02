/// Author:				        Joe Audette
/// Created:			        2005-04-10
///	Last Modified:              2008-11-21
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.EventCalendarUI
{
	
    public partial class EventCalendarEdit : CBasePage
	{
		private int moduleId = -1;
        private int itemId = -1;
        private String cacheDependencyKey;
        private DateTime thisDay = DateTime.Today;
        private string virtualRoot;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            SiteUtils.SetupEditor(edContent);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            //this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            base.OnInit(e);
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

            LoadSettings();

            if (!UserCanEditModule(moduleId))
			{
                SiteUtils.RedirectToEditAccessDeniedPage();
            }

			PopulateLabels();

			if (!IsPostBack) 
			{
				PopulateControls();

                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;
                }
			}
			
		}

		private void PopulateControls()
		{
            if (itemId > -1)
            {
                CalendarEvent calEvent = new CalendarEvent(itemId);
                this.txtTitle.Text = calEvent.Title;
                edContent.Text = calEvent.Description;
                this.dpEventDate.Text = calEvent.EventDate.ToShortDateString();

                ListItem item;
                item = ddStartTime.Items.FindByValue(calEvent.StartTime.ToShortTimeString());
                if (item != null)
                {
                    ddStartTime.ClearSelection();
                    item.Selected = true;
                }

                item = ddEndTime.Items.FindByValue(calEvent.EndTime.ToShortTimeString());
                if (item != null)
                {
                    ddEndTime.ClearSelection();
                    item.Selected = true;
                }

                txtLocation.Text = calEvent.Location;

            }
            else
            {
                btnDelete.Visible = false;
            }
			
		}

		private void PopulateLabels()
		{

            Title = SiteUtils.FormatPageTitle(siteSettings, EventCalResources.EditEventPageTitle);

            btnUpdate.Text = EventCalResources.EventCalendarEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, EventCalResources.EventCalendarEditUpdateButtonAccessKey);

            UIHelper.DisableButtonAfterClick(
                btnUpdate,
                EventCalResources.ButtonDisabledPleaseWait,
                Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
                );

            lnkCancel.Text = EventCalResources.EventCalendarEditCancelButton;
            //btnCancel.Text = EventCalResources.EventCalendarEditCancelButton;
            //SiteUtils.SetButtonAccessKey(btnCancel, EventCalResources.EventCalendarEditCancelButtonAccessKey);

            btnDelete.Text = EventCalResources.EventCalendarEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, EventCalResources.EventCalendarEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, EventCalResources.EventCalendarDeleteEventWarning);

            
		}


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }


	    private void btnDelete_Click(object sender, EventArgs e)
		{
			if(itemId > -1)
			{
                CalendarEvent calendarEvent = new CalendarEvent(itemId);
                calendarEvent.ContentChanged += new ContentChangedEventHandler(calendarEvent_ContentChanged);
                

                calendarEvent.Delete();
                CurrentPage.UpdateLastModifiedTime();
                SiteUtils.QueueIndexing();
			}

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        void calendarEvent_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["CalendarEventIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }
        

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			Page.Validate();
			if (Page.IsValid) 
			{
				int userId = -1;
                Guid userGuid = Guid.Empty;
				if(Request.IsAuthenticated)
				{
					//SiteUser siteUser = new SiteUser(siteSettings, Context.User.Identity.Name);
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    userId = siteUser.UserId;
                    userGuid = siteUser.UserGuid;
				}
				CalendarEvent calEvent = new CalendarEvent(itemId);
                calEvent.ContentChanged += new ContentChangedEventHandler(calendarEvent_ContentChanged);
                
                Module m = new Module(this.moduleId);
                calEvent.ModuleId = m.ModuleId;
                calEvent.ModuleGuid = m.ModuleGuid;
				calEvent.UserId = userId;
                calEvent.UserGuid = userGuid;
                calEvent.LastModUserGuid = userGuid;
				calEvent.Title = this.txtTitle.Text;
                calEvent.Description = edContent.Text;
				calEvent.EventDate = DateTime.Parse(this.dpEventDate.Text);
				calEvent.StartTime = DateTime.Parse(this.ddStartTime.SelectedValue);
				calEvent.EndTime = DateTime.Parse(this.ddEndTime.SelectedValue);
                calEvent.Location = txtLocation.Text;

               

				if(calEvent.Save())
				{
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

			}

		}


        private void LoadSettings()
        {
            virtualRoot = WebUtils.GetApplicationRoot();
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            thisDay = WebUtils.ParseDateFromQueryString("date", DateTime.Today);
            cacheDependencyKey = "Module-" + moduleId.ToString();
            edContent.WebEditor.ToolBar = ToolBar.Full;
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            if (!Page.IsPostBack)
            {
                dpEventDate.Text = thisDay.ToShortDateString();
            }

            DateTime tomorrow = thisDay.AddDays(1).AddMinutes(-15);
            ddStartTime.Items.Insert(0, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
            ddEndTime.Items.Insert(0, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
            int i = 0;
            while (thisDay < tomorrow)
            {
                i += 1;
                thisDay = thisDay.AddMinutes(15);

                ddStartTime.Items.Insert(i, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
                ddEndTime.Items.Insert(i, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));

            }
        }

		
	}
}
