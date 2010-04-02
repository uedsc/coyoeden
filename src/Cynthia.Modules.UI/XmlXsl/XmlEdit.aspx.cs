///	Last Modified:              2008-11-09
/// 
/// TODO: add upload xml and xsl features

using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.XmlUI 
{
	
    public partial class EditXml : CBasePage 
	{
        private int moduleId = -1;
        private String cacheDependencyKey;

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.updateButton.Click += new EventHandler(this.UpdateBtn_Click);
            //this.cancelButton.Click += new EventHandler(this.CancelBtn_Click);
            
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

            LoadParams();

            if (!UserCanEditModule(moduleId))
			{
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
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

		private void PopulateLabels()
		{
            updateButton.Text = XmlResources.EditXmlUpdateButton;
            SiteUtils.SetButtonAccessKey(updateButton, XmlResources.EditXmlUpdateButtonAccessKey);

            lnkCancel.Text = XmlResources.EditXmlCancelButton;
            
            
        }

		private void PopulateControls()
		{
			this.ddXml.DataSource = GetXmlList();
			this.ddXml.DataBind();

            ListItem listItem = new ListItem(XmlResources.XmlNoFileSelected, string.Empty);
            ddXml.Items.Insert(0, listItem);

			this.ddXsl.DataSource = GetXslList();
			this.ddXsl.DataBind();
            ddXsl.Items.Insert(0, listItem);
			
			if (moduleId > -1) 
			{
				Hashtable settings = ModuleSettings.GetModuleSettings(moduleId);
				string xml = string.Empty;
				string xsl = string.Empty;
                
				if(settings.Contains("XmlModuleXmlSourceSetting"))
				{
					xml = settings["XmlModuleXmlSourceSetting"].ToString();	
				}

				if(settings.Contains("XmlModuleXslSourceSetting"))
				{
					xsl = settings["XmlModuleXslSourceSetting"].ToString();
				}
				if(xml.Length > 0)
				{
                    //this.ddXml.SelectedValue = xml;
                    listItem = ddXml.Items.FindByValue(xml);
                    if (listItem != null)
                    {
                        ddXml.ClearSelection();
                        listItem.Selected = true;
                    }
					
				}

				if(xsl.Length > 0)
				{
					//this.ddXsl.SelectedValue = xsl;
                    listItem = ddXsl.Items.FindByValue(xsl);
                    if (listItem != null)
                    {
                        ddXsl.ClearSelection();
                        listItem.Selected = true;
                    }
				}

			}

		}

        protected FileInfo[] GetXmlList()
        {
            string p = WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString() + "/xml";
            string filePath = HttpContext.Current.Server.MapPath(p);

            if (Directory.Exists(filePath))
            {
                return new DirectoryInfo(filePath).GetFiles("*.xml");
            }

            return null;
        }

        protected FileInfo[] GetXslList()
        {
            string p = WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString() + "/xsl";
            string filePath = HttpContext.Current.Server.MapPath(p);

            if (Directory.Exists(filePath))
            {
                return new DirectoryInfo(filePath).GetFiles("*.xsl");
            }

            return null;
        }


	    void UpdateBtn_Click(Object sender, EventArgs e)
		{
            Module m = new Module(moduleId);

            ModuleSettings.UpdateModuleSetting(
                m.ModuleGuid,
                m.ModuleId, 
                "XmlModuleXmlSourceSetting", 
                this.ddXml.SelectedValue);

            ModuleSettings.UpdateModuleSetting(
                m.ModuleGuid,
                m.ModuleId, 
                "XmlModuleXslSourceSetting", 
                this.ddXsl.SelectedValue);

            CurrentPage.UpdateLastModifiedTime();
            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }

       
        //void CancelBtn_Click(Object sender, EventArgs e)
        //{
        //    if (hdnReturnUrl.Value.Length > 0)
        //    {
        //        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
        //        return;
        //    }

        //    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        //}


        private void LoadParams()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            cacheDependencyKey = "Module-" + moduleId.ToString();
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

        }
        
    }
}
