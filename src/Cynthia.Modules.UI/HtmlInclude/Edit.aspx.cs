using System;
using System.Collections;
using System.IO;
using System.Web;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.ContentUI
{
	/// <summary>
	/// Author:                     Joe Audette
	/// Created:                    2005-11-19
    ///	Last Modified:              2008-11-21
    /// 
	/// </summary>
    public partial class HtmlIncludeEdit : CBasePage
	{
        
		int moduleID = -1;
        private String cacheDependencyKey;

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
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

            moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);
			
            if (!UserCanEditModule(moduleID))
			{
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            cacheDependencyKey = "Module-" + moduleID.ToString();

			PopulateLabels();

			if (!IsPostBack) 
			{
				PopulateControls();
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();

                }
			}
		}

		private void PopulateLabels()
		{
            btnUpdate.Text = HtmlIncludeResources.EditHtmlFragmentUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, HtmlIncludeResources.EditHtmlFragmentUpdateButtonAccessKey);

            btnCancel.Text = HtmlIncludeResources.EditHtmlFragmentCancelButton;
            SiteUtils.SetButtonAccessKey(btnCancel, HtmlIncludeResources.EditHtmlFragmentCancelButtonAccessKey);

            
		}

		private void PopulateControls()
		{
			this.ddInclude.DataSource = GetFragmentList();
			this.ddInclude.DataBind();

			if (moduleID > -1) 
			{
				Hashtable settings = ModuleSettings.GetModuleSettings(moduleID);
				string includeFile = string.Empty;
				
				if(settings.Contains("HtmlFragmentSourceSetting"))
				{
					includeFile = settings["HtmlFragmentSourceSetting"].ToString();	
				}

				if(includeFile.Length > 0)
				{
					this.ddInclude.SelectedValue = includeFile;
				}

			}

		}


		protected  FileInfo[] GetFragmentList()
		{
			string filePath = string.Empty;
			
			string p = WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString() + "/htmlfragments";
			filePath = HttpContext.Current.Server.MapPath(p);
			
			if(Directory.Exists(filePath))
			{
				DirectoryInfo dir = new DirectoryInfo(filePath);
				
				return dir.GetFiles();
			}

			return null;
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
            Module m = new Module(moduleID);
            
			ModuleSettings.UpdateModuleSetting(m.ModuleGuid, m.ModuleId, "HtmlFragmentSourceSetting", this.ddInclude.SelectedValue);
            CurrentPage.UpdateLastModifiedTime();
            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

			

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


	
	}
}
