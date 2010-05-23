/// Author:				        Dean Brettle
/// Created:			        2005-09-06
///	Last Modified:              2009-06-27
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
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;
using Cynthia.Modules;

namespace Cynthia.Web.GroupUI
{
	
    public partial class GroupThreadEdit : CBasePage
	{
		private int threadId = -1;
		private SiteUser siteUser;
		private GroupThread forumThread;
        private bool isSiteEditor = false;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            base.OnInit(e);

            SuppressPageMenu();
        }

        #endregion


        private void Page_Load(object sender, EventArgs e)
		{
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!WebUser.IsAdminOrContentAdmin) && (!isSiteEditor))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }
              
            SecurityHelper.DisableBrowserCache();

            LoadParams();
            forumThread = new GroupThread(threadId);
            
            GroupThreadIndexBuilderProvider indexBuilder 
                = (GroupThreadIndexBuilderProvider)IndexBuilderManager.Providers["GroupThreadIndexBuilderProvider"];
            
            if (indexBuilder != null)
            {
                forumThread.ThreadMoved += new GroupThread.ThreadMovedEventHandler(indexBuilder.ThreadMovedHandler);
            }

            siteUser = SiteUtils.GetCurrentSiteUser();
			
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

        

		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, GroupResources.GroupThreadEditLabel);
            btnUpdate.Text = GroupResources.GroupThreadUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, GroupResources.GroupEditUpdateButtonAccessKey);

            lnkCancel.Text = GroupResources.GroupThreadCancelButton;
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            btnDelete.Text = GroupResources.GroupThreadDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, GroupResources.GroupEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, GroupResources.GroupDeleteThreadWarning);

            
		}

		private void PopulateControls()
		{
			Group forum = new Group(forumThread.GroupId);
			this.txtSubject.Text = forumThread.Subject;
            using (IDataReader reader = Group.GetGroups(forum.ModuleId, siteUser.UserId))
            {
                this.ddGroupList.DataSource = reader;
                this.ddGroupList.DataBind();
            }
			this.ddGroupList.SelectedValue = forumThread.GroupId.ToString();
			
		}

		


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			forumThread.GroupId = int.Parse(this.ddGroupList.SelectedValue);
			forumThread.Subject = this.txtSubject.Text;
			forumThread.UpdateThread();

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
			GroupThread.Delete(this.threadId);
            Group.UpdateUserStats(-1); // updates all users

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        private void LoadParams()
        {
            threadId = WebUtils.ParseInt32FromQueryString("thread", -1);

           
        }

		
	}
}
