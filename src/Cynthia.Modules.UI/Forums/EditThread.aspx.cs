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
using Cynthia.Features;

namespace Cynthia.Web.ForumUI
{
	
    public partial class ForumThreadEdit : CBasePage
	{
		private int threadId = -1;
		private SiteUser siteUser;
		private ForumThread forumThread;
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
            forumThread = new ForumThread(threadId);
            
            ForumThreadIndexBuilderProvider indexBuilder 
                = (ForumThreadIndexBuilderProvider)IndexBuilderManager.Providers["ForumThreadIndexBuilderProvider"];
            
            if (indexBuilder != null)
            {
                forumThread.ThreadMoved += new ForumThread.ThreadMovedEventHandler(indexBuilder.ThreadMovedHandler);
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
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.ForumThreadEditLabel);
            btnUpdate.Text = ForumResources.ForumThreadUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, ForumResources.ForumEditUpdateButtonAccessKey);

            lnkCancel.Text = ForumResources.ForumThreadCancelButton;
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            btnDelete.Text = ForumResources.ForumThreadDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, ForumResources.ForumEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, ForumResources.ForumDeleteThreadWarning);

            
		}

		private void PopulateControls()
		{
			Forum forum = new Forum(forumThread.ForumId);
			this.txtSubject.Text = forumThread.Subject;
            using (IDataReader reader = Forum.GetForums(forum.ModuleId, siteUser.UserId))
            {
                this.ddForumList.DataSource = reader;
                this.ddForumList.DataBind();
            }
			this.ddForumList.SelectedValue = forumThread.ForumId.ToString();
			
		}

		


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			forumThread.ForumId = int.Parse(this.ddForumList.SelectedValue);
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
			ForumThread.Delete(this.threadId);
            Forum.UpdateUserStats(-1); // updates all users

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
