/// Author:				        Joe Audette
/// Created:			        2004-09-11
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
using System.Globalization;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.ForumUI
{
    public partial class ForumEdit : CBasePage
	{
		private int moduleId = -1;
        private int itemId = -1;
        private String cacheDependencyKey;
        private string virtualRoot;
        private Double timeOffset = 0;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
            edContent.ProviderName = SiteUtils.GetEditorProviderName();
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            

            SiteUtils.SetupEditor(edContent);
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
                return;
            }

			PopulateLabels();

			if (!IsPostBack) 
			{
				if (itemId > -1)
				{
					PopulateControls();
					
				}
				else
				{
					ShowNewForumControls();
				}

                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;

                }
			}

		}

		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.ForumEditForumLabel);

            // TODO: implement
            divIsModerated.Visible = false;
            divIsActive.Visible = false;


            btnUpdate.Text = ForumResources.ForumEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, ForumResources.ForumEditUpdateButtonAccessKey);

            lnkCancel.Text = ForumResources.ForumEditCancelButton;
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            btnDelete.Text = ForumResources.ForumEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, ForumResources.ForumEditDeleteButtonAccessKey);

            
	
		}

		private void ShowNewForumControls()
		{
			this.btnDelete.Visible = false;
            this.btnUpdate.Text = ForumResources.ForumEditCreateButton;
			this.chkIsActive.Checked = true;
			this.txtSortOrder.Text = "100";
			this.txtPostsPerPage.Text = "10";
			this.txtThreadsPerPage.Text = "40";
			

		}

		private void PopulateControls()
		{
			Forum forum = new Forum(itemId);
			
			this.lblCreatedDate.Text = forum.CreatedDate.AddHours(timeOffset).ToString();
            edContent.Text = forum.Description;
			this.txtTitle.Text = forum.Title;
			this.chkIsActive.Checked = forum.IsActive;
			this.chkAllowAnonymousPosts.Checked = forum.AllowAnonymousPosts;
			this.chkIsModerated.Checked = forum.IsModerated;
			this.txtSortOrder.Text = forum.SortOrder.ToString();
			this.txtPostsPerPage.Text = forum.PostsPerPage.ToString();
			this.txtThreadsPerPage.Text = forum.ThreadsPerPage.ToString();

			
		}


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			Forum forum = new Forum(itemId);
			
            //SiteUser siteUser = new SiteUser(siteSettings, Context.User.Identity.Name);
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if(siteUser != null)
			forum.CreatedByUserId = siteUser.UserId;

            forum.ModuleId = moduleId;
            forum.Description = edContent.Text;
			forum.Title = this.txtTitle.Text;
			forum.IsActive = this.chkIsActive.Checked;
			forum.AllowAnonymousPosts = this.chkAllowAnonymousPosts.Checked;
			forum.IsModerated = this.chkIsModerated.Checked;
			forum.SortOrder = int.Parse(this.txtSortOrder.Text);
			forum.PostsPerPage = int.Parse(this.txtPostsPerPage.Text);
			forum.ThreadsPerPage = int.Parse(this.txtThreadsPerPage.Text);

			if(forum.Save())
			{
                CurrentPage.UpdateLastModifiedTime();
                CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                if (hdnReturnUrl.Value.Length > 0)
                {
                    WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                    return;
                }

                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

			}
			
			
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
            Forum.Delete(itemId);
            Forum.UpdateUserStats(-1); // updates all users
            CurrentPage.UpdateLastModifiedTime();

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

		}

        private void LoadSettings()
        {
            virtualRoot = WebUtils.GetApplicationRoot();
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            cacheDependencyKey = "Module-" + moduleId.ToString();
            timeOffset = SiteUtils.GetUserTimeOffset();
            edContent.WebEditor.ToolBar = ToolBar.Full;

            

        }
		
		
	}
}
