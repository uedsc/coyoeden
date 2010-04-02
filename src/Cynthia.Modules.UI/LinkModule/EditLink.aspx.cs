/// Author:					    Joe Audette
/// Created:				    2004-12-24
///	Last Modified:              2009-06-20

using System;
using System.Collections;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.LinksUI 
{
    public partial class EditLinks : CBasePage
	{
		private Hashtable moduleSettings;
        private bool useDescription = false;
        private int moduleId = 0;
        private int itemId = -1;
        private String cacheDependencyKey;
        private bool descriptionOnly = false;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            
            SiteUtils.SetupEditor(edDescription);
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.updateButton.Click += new EventHandler(this.UpdateBtn_Click);
            //this.cancelButton.Click += new EventHandler(this.CancelBtn_Click);
            this.deleteButton.Click += new EventHandler(this.DeleteBtn_Click);
            

            SuppressPageMenu();
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

            LoadSettings();
			PopulateLabels();

            if (!IsPostBack) 
			{
				PopulateControls();
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

                }
            }
        }

        private void PopulateControls()
        {
            if (itemId > -1)
            {
                Link linkItem = new Link(itemId);

                txtTitle.Text = linkItem.Title;
                
                edDescription.Text = linkItem.Description;
                

                String linkProtocol = "http://";
                if (linkItem.Url.StartsWith("https://"))
                {
                    linkProtocol = "https://";
                }
                if (linkItem.Url.StartsWith("~/"))
                {
                    linkProtocol = "~/";
                }

                ListItem listItem = ddProtocol.Items.FindByValue(linkProtocol);
                if (listItem != null)
                {
                    ddProtocol.ClearSelection();
                    listItem.Selected = true;
                }

                txtUrl.Text = linkItem.Url.Replace(linkProtocol, String.Empty);
                txtViewOrder.Text = linkItem.ViewOrder.ToString();

                if (linkItem.Target == "_blank")
                {
                    chkUseNewWindow.Checked = true;
                }

                SiteUser linkUser = new SiteUser(siteSettings, linkItem.CreatedByUser);

            }
            else
            {
                this.deleteButton.Visible = false;
            }

        }

        private void UpdateBtn_Click(Object sender, EventArgs e)
		{
            if (Page.IsValid) 
			{
				Link linkItem = new Link(itemId);
                linkItem.ContentChanged += new ContentChangedEventHandler(linkItem_ContentChanged);

                Module module = new Module(moduleId);
                linkItem.ModuleGuid = module.ModuleGuid;
				linkItem.ModuleId = this.moduleId;
				linkItem.Title = this.txtTitle.Text;
                if (chkUseNewWindow.Checked)
                {
                    linkItem.Target = "_blank";
                }
                else
                {
                    linkItem.Target = String.Empty;
                }
				
                linkItem.Description = edDescription.Text;
				

				linkItem.Url = ddProtocol.SelectedValue + this.txtUrl.Text.Replace("https://", String.Empty).Replace("http://", String.Empty).Replace("~/",String.Empty);
				
                linkItem.ViewOrder = int.Parse(this.txtViewOrder.Text);
                SiteUser linkUser = SiteUtils.GetCurrentSiteUser();
                if (linkUser != null)
                {
                    linkItem.CreatedByUser = linkUser.UserId;
                    linkItem.UserGuid = linkUser.UserGuid;
                }
				
				if(linkItem.Save())
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

        void linkItem_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["LinksIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }


        private void DeleteBtn_Click(Object sender, EventArgs e)
		{
            if (itemId != -1) 
			{
                Link link = new Link(itemId);
                link.ContentChanged += new ContentChangedEventHandler(linkItem_ContentChanged);
                link.Delete();
                CurrentPage.UpdateLastModifiedTime();
                CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                SiteUtils.QueueIndexing();
            }

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }


        //private void CancelBtn_Click(Object sender, EventArgs e)
        //{
        //    if (hdnReturnUrl.Value.Length > 0)
        //    {
        //        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
        //        return;
        //    }

        //    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        //}

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, LinkResources.EditLinkPageTitle);

            this.reqTitle.ErrorMessage = LinkResources.LinksTitleRequiredHelp;
            this.reqUrl.ErrorMessage = LinkResources.LinksUrlRequiredHelp;
            this.reqViewOrder.ErrorMessage = LinkResources.LinksViewOrderRequiredHelp;

            if (descriptionOnly) { reqUrl.Enabled = false; }

            updateButton.Text = LinkResources.EditLinksUpdateButton;
            SiteUtils.SetButtonAccessKey(updateButton, LinkResources.EditLinksUpdateButtonAccessKey);

            lnkCancel.Text = LinkResources.EditLinksCancelButton;
            //cancelButton.Text = LinkResources.EditLinksCancelButton;
            //SiteUtils.SetButtonAccessKey(cancelButton, LinkResources.EditLinksCancelButtonAccessKey);

            deleteButton.Text = LinkResources.EditLinksDeleteButton;
            SiteUtils.SetButtonAccessKey(deleteButton, LinkResources.EditLinksDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(deleteButton, LinkResources.LinksDeleteLinkWarning);

            ListItem listItem = ddProtocol.Items.FindByValue("~/");
            if (listItem != null)
            {
                listItem.Text = LinkResources.LinksEditRelativeLinkLabel;
            }

        }

        private void LoadParams()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);

        }

        private void LoadSettings()
        {
            cacheDependencyKey = "Module-" + moduleId.ToString();

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            useDescription = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "LinksShowDescriptionSetting", false);

            descriptionOnly = WebUtils.ParseBoolFromHashtable(
                    moduleSettings, "LinksShowOnlyDescriptionSetting", descriptionOnly);

            //divDescription.Visible = useDescription;

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
        }
       
    }
}
