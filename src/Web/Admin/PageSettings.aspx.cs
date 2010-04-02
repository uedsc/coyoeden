// Author:						Joe Audette
// Created:					    2004-11-14
// Last Modified:               2010-02-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Business.WebHelpers.PageEventHandlers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI
{
	
    public partial class PageProperties : CBasePage
	{
        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.applyBtn.Click += new EventHandler(this.Apply_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);

            grdContentMeta.RowCommand += new GridViewCommandEventHandler(grdContentMeta_RowCommand);
            grdContentMeta.RowEditing += new GridViewEditEventHandler(grdContentMeta_RowEditing);
            grdContentMeta.RowUpdating += new GridViewUpdateEventHandler(grdContentMeta_RowUpdating);
            grdContentMeta.RowCancelingEdit += new GridViewCancelEditEventHandler(grdContentMeta_RowCancelingEdit);
            grdContentMeta.RowDeleting += new GridViewDeleteEventHandler(grdContentMeta_RowDeleting);
            grdContentMeta.RowDataBound += new GridViewRowEventHandler(grdContentMeta_RowDataBound);
            btnAddMeta.Click += new EventHandler(btnAddMeta_Click);

            grdMetaLinks.RowCommand += new GridViewCommandEventHandler(grdMetaLinks_RowCommand);
            grdMetaLinks.RowEditing += new GridViewEditEventHandler(grdMetaLinks_RowEditing);
            grdMetaLinks.RowUpdating += new GridViewUpdateEventHandler(grdMetaLinks_RowUpdating);
            grdMetaLinks.RowCancelingEdit += new GridViewCancelEditEventHandler(grdMetaLinks_RowCancelingEdit);
            grdMetaLinks.RowDeleting += new GridViewDeleteEventHandler(grdMetaLinks_RowDeleting);
            grdMetaLinks.RowDataBound += new GridViewRowEventHandler(grdMetaLinks_RowDataBound);
            btnAddMetaLink.Click += new EventHandler(btnAddMetaLink_Click);

            ScriptConfig.IncludeYuiTabs = true;
            IncludeYuiTabsCss = true;

            SuppressPageMenu();

            JQueryUIThemeName = "base";
        }

        
        #endregion

		private static readonly ILog log = LogManager.GetLogger(typeof(PageProperties));

        private string iconPath;
        private bool sslIsAvailable = false;
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private bool canEdit = false;
        private bool canEditDraftOnly = false;
        private PageSettings pageSettings = null;
        private PageSettings parentPage = null;
        private int pageId = -1;
        private int startPageId = -1;
        private bool autosuggestFriendlyUrls = WebConfigSettings.AutoSuggestFriendlyUrls;
        private SiteMapDataSource siteMapDataSource;
        ContentMetaRespository metaRepository = new ContentMetaRespository();
        private SiteUser currentUser = null;

		private void Page_Load(object sender, EventArgs e)
		{
           
            SecurityHelper.DisableBrowserCache();

            LoadSettings();
            
			if(pageId > -1)
			{
                pageSettings = new PageSettings(siteSettings.SiteId, pageId);

                
			}
			else
			{
                pageSettings = new PageSettings();
                if (startPageId > -1)
                {
                    // we'll inherit some defaults from parent
                    parentPage = new PageSettings(siteSettings.SiteId, startPageId);
                    if (parentPage.PageId == -1) { parentPage = null; }

                    if (parentPage != null) 
                    { 
                        // by default inherit setting from parent
                        pageSettings.AuthorizedRoles = parentPage.AuthorizedRoles;
                        pageSettings.EditRoles = parentPage.EditRoles;
                        pageSettings.CreateChildPageRoles = parentPage.CreateChildPageRoles; 

                    }
                }
                else
                {
                    // new root level page
                    pageSettings.AuthorizedRoles = WebConfigSettings.DefaultPageRoles;
                }
                        
                btnDelete.Visible = false;
                lblPageNameLayout.ConfigKey = "PageSettingsCreateNewPageLabel";

			}

            pnlMeta.Visible = (pageSettings.PageGuid != Guid.Empty);

            canEdit = UserCanEdit();
            canEditDraftOnly = UserCanEditDraftOnly();

            if (!canEdit && !canEditDraftOnly)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

			PopulateLabels();
            SetupScripts();

           
			if (!Page.IsPostBack) 
			{
				PopulateControls();
                BindMeta();
                BindMetaLinks();
			}
		}

		

		private void PopulateControls() 
		{
            ddIcons.DataSource = SiteUtils.GetFeatureIconList();
            ddIcons.DataBind();
            ddIcons.Items.Insert(0, new ListItem(Resource.ModuleSettingsNoIconLabel, "blank.gif"));
            ddIcons.Attributes.Add("onchange", "javascript:showIcon(this);");
            ddIcons.Attributes.Add("size", "6");
            SetupIconScript(this.imgIcon);

            PopulatePageList();
            PopulateChangeFrequencyDropdown();

            ListItem listItem;
            
            if (
                (isAdmin)
                ||(isSiteEditor)
                ||(WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages))
                || ((pageSettings.PageId != -1)&&(pageSettings.ParentId == -1))
                )
            {
                ddPages.Items.Insert(0, new ListItem(
                    Resource.PageSettingsRootLabel, "-1"));
            }
            else
            {

                if ((ddPages.Items.Count == 0) && (pageId == -1))
                {
                    //this user has no permission to edit child pages beneath any pages
                    SiteUtils.RedirectToEditAccessDeniedPage();
                }

            }

            // pre-select the parent for new pages
            if (startPageId > -1)
            {
                listItem = ddPages.Items.FindByValue(startPageId.ToString(CultureInfo.InvariantCulture));
                if (listItem != null)
                {
                    ddPages.ClearSelection();
                    listItem.Selected = true;
                }

                //if user can only save in draft, and this is a new page, then mark as pending;
                chkIsPending.Checked = canEditDraftOnly;
                chkIsPending.Enabled = !canEditDraftOnly;

            }
            else
            {
                chkIsPending.Checked = pageSettings.IsPending;
                //if not new, then allow chkIsPending to be enabled if not actually checked:
                chkIsPending.Enabled = !canEditDraftOnly || !chkIsPending.Checked;
            }

            // default to monthly
            listItem = ddChangeFrequency.Items.FindByValue("Monthly");
            if (listItem != null)
            {
                ddChangeFrequency.ClearSelection();
                listItem.Selected = true;
            }
		
			if(pageId > -1)
			{
                
                listItem = ddChangeFrequency.Items.FindByValue(pageSettings.ChangeFrequency.ToString());
                if (listItem != null)
                {
                    ddChangeFrequency.ClearSelection();
                    listItem.Selected = true;
                }
                
                listItem = ddSiteMapPriority.Items.FindByValue(pageSettings.SiteMapPriority);
                if (listItem != null)
                {
                    ddSiteMapPriority.ClearSelection();
                    listItem.Selected = true;
                }



                lnkEditContent.NavigateUrl = SiteRoot + "/Admin/PageLayout.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture);

                this.lblPageName.Text = pageSettings.PageName;
                ddPages.ClearSelection();
                this.ddPages.SelectedIndex
                    = ddPages.Items.IndexOf(ddPages.Items.FindByValue(pageSettings.ParentId.ToString(CultureInfo.InvariantCulture)));
                
                txtPageName.Text = pageSettings.PageName;
                txtPageTitle.Text = pageSettings.PageTitle;
                hdnPageName.Value = txtPageName.Text;
                chkRequireSSL.Checked = pageSettings.RequireSsl;
                chkShowBreadcrumbs.Checked = pageSettings.ShowBreadcrumbs;
                chkShowChildPageBreadcrumbs.Checked = pageSettings.ShowChildPageBreadcrumbs;
                chkShowHomeCrumb.Checked = pageSettings.ShowHomeCrumb;
                txtPageKeywords.Text = pageSettings.PageMetaKeyWords;
                txtPageDescription.Text = pageSettings.PageMetaDescription;
                txtPageEncoding.Text = pageSettings.PageMetaEncoding;
                //txtPageAdditionalMetaTags.Text = pageSettings.PageMetaAdditional;

                chkUseUrl.Checked = pageSettings.UseUrl;
                txtUrl.Text = pageSettings.Url;
                chkNewWindow.Checked = pageSettings.OpenInNewWindow;
                chkShowChildMenu.Checked = pageSettings.ShowChildPageMenu;
                chkIncludeInMenu.Checked = pageSettings.IncludeInMenu;
                chkIncludeInSiteMap.Checked = pageSettings.IncludeInSiteMap;
                chkAllowBrowserCache.Checked = pageSettings.AllowBrowserCache;
                chkHideAfterLogin.Checked = pageSettings.HideAfterLogin;
                chkEnableComments.Checked = pageSettings.EnableComments;

                lnkViewPage.NavigateUrl = SiteUtils.GetCurrentPageUrl();
                chkIncludeInSearchEngineSiteMap.Checked = pageSettings.IncludeInSearchMap;
                txtCannonicalOverride.Text = pageSettings.CanonicalOverride;

                ListItem item = ddIcons.Items.FindByValue(pageSettings.MenuImage);
                if (item != null)
                {
                    ddIcons.ClearSelection();
                    ddIcons.Items.FindByValue(pageSettings.MenuImage).Selected = true;
                    imgIcon.Src = iconPath + pageSettings.MenuImage;
                }

                
                    
                if (
                    (autosuggestFriendlyUrls)
                    &&(txtUrl.Text == String.Empty)
                    )
                {
                    String friendlyUrl = SiteUtils.SuggestFriendlyUrl(txtPageName.Text, siteSettings);

                    if (WebConfigSettings.AlwaysUrlEncode)
                    {
                        txtUrl.Text = "~/" + Server.UrlEncode(friendlyUrl);
                    }
                    else
                    {
                        txtUrl.Text = "~/" + friendlyUrl;
                    }
                }


			}

			if(siteSettings.AllowPageSkins)
			{
                DirectoryInfo[] directories = SiteUtils.GetSkinList(siteSettings);
                ddSkins.DataSource = directories;
                ddSkins.DataBind();

				listItem = new ListItem();
				listItem.Value = "";
                listItem.Text = Resource.PageLayoutDefaultSkinLabel;
				ddSkins.Items.Insert(0, listItem);
                if (pageSettings.Skin.Length > 0)
                {
                    listItem = ddSkins.Items.FindByValue(pageSettings.Skin);
                    if (listItem != null)
                    {
                        ddSkins.ClearSelection();
                        listItem.Selected = true;
                    }
                }
                else
                {
                    if (
                        (pageId == -1)
                        && (WebConfigSettings.AssignNewPagesParentPageSkinByDefault)
                        )
                    {
                        
                        if (parentPage != null)
                        {
                            listItem = ddSkins.Items.FindByValue(parentPage.Skin);
                            if (listItem != null)
                            {
                                ddSkins.ClearSelection();
                                listItem.Selected = true;
                            }

                        }
                    }
           
                }
			}
			else
			{
				divSkin.Visible = false;
			}

			if(siteSettings.AllowHideMenuOnPages)
			{
                chkHideMainMenu.Checked = pageSettings.HideMainMenu;
				
			}
			else
			{
				divHideMenu.Visible = false;
			}

            BindRoles(pageSettings);

            


		}

        private void BindRoles(PageSettings pageSettings)
        {

            chkListAuthRoles.Items.Clear();

            ListItem allItem = new ListItem();
            // localize display
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";

            if (pageSettings.AuthorizedRoles.LastIndexOf("All Users") > -1)
            {
                allItem.Selected = true;
            }

            chkListAuthRoles.Items.Add(allItem);

            chkListEditRoles.Items.Clear();
            chkListCreateChildPageRoles.Items.Clear();


            using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (reader.Read())
                {

                    ListItem listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();

                    ListItem editItem = new ListItem();
                    editItem.Text = reader["DisplayName"].ToString();
                    editItem.Value = reader["RoleName"].ToString();

                    ListItem draftItem = new ListItem();
                    draftItem.Text = reader["DisplayName"].ToString();
                    draftItem.Value = reader["RoleName"].ToString();

                    ListItem childItem = new ListItem();
                    childItem.Text = reader["DisplayName"].ToString();
                    childItem.Value = reader["RoleName"].ToString();


                    if (
                        (pageSettings.AuthorizedRoles.LastIndexOf(listItem.Value + ";") > -1)
                        || ((isSiteEditor) && (siteSettings.SiteRootEditRoles.LastIndexOf(listItem.Value + ";") > -1))
                        )
                    {
                        listItem.Selected = true;
                    }

                    if (
                        (pageSettings.EditRoles.LastIndexOf(editItem.Value + ";") > -1)
                        || ((isSiteEditor) && (siteSettings.SiteRootEditRoles.LastIndexOf(listItem.Value + ";") > -1))
                        )
                    {
                        editItem.Selected = true;
                    }

                    if (
                        (pageSettings.DraftEditOnlyRoles.LastIndexOf(draftItem.Value + ";") > -1)
                        || ((isSiteEditor) && (siteSettings.SiteRootDraftEditRoles.LastIndexOf(listItem.Value + ";") > -1))
                        )
                    {
                        draftItem.Selected = true;
                    }

                    if (
                        (pageSettings.CreateChildPageRoles.LastIndexOf(childItem.Value + ";") > -1)
                        || ((isSiteEditor) && (siteSettings.SiteRootEditRoles.LastIndexOf(listItem.Value + ";") > -1))
                        )
                    {
                        childItem.Selected = true;
                    }


                    chkListAuthRoles.Items.Add(listItem);
                    chkListEditRoles.Items.Add(editItem);
                    chkDraftEditRoles.Items.Add(draftItem);
                    chkListCreateChildPageRoles.Items.Add(childItem);
                }
            }

            if ((!isAdmin) && (!isSiteEditor))
            {
                this.chkListAuthRoles.Enabled = false;
                this.chkListEditRoles.Enabled = false;
                this.chkListCreateChildPageRoles.Enabled = false;
                this.chkDraftEditRoles.Enabled = false;
            }

        }

        private bool UserCanEdit()
        {
            if(isAdmin){ return true;}
            if (WebUser.IsInRoles(pageSettings.EditRoles)) { return true; }
            if(isSiteEditor){ return true;}

            if (startPageId > -1)
            {
                PageSettings parentPage = new PageSettings(siteSettings.SiteId, startPageId);
                if (WebUser.IsInRoles(parentPage.CreateChildPageRoles)) { return true; }

            }


            return false;

        }

        private bool UserCanEditDraftOnly()
        {
            if (isAdmin) { return false; }
            if (isSiteEditor) { return false; }
            if (WebUser.IsInRoles(pageSettings.DraftEditOnlyRoles)) { return true; }
            return false;
        }

        private void PopulateChangeFrequencyDropdown()
        {
            // TODO: localize display

            ListItem listItem = new ListItem(
                Resource.PageChangeFrequencyAlways, "Always");
            ddChangeFrequency.Items.Add(listItem);

            listItem = new ListItem(
                Resource.PageChangeFrequencyHourly, "Hourly");
            ddChangeFrequency.Items.Add(listItem);

            listItem = new ListItem(
                Resource.PageChangeFrequencyDaily, "Daily");
            ddChangeFrequency.Items.Add(listItem);

            listItem = new ListItem(
                Resource.PageChangeFrequencyWeekly, "Weekly");
            ddChangeFrequency.Items.Add(listItem);

            listItem = new ListItem(
                Resource.PageChangeFrequencyMonthly, "Monthly");
            ddChangeFrequency.Items.Add(listItem);

            listItem = new ListItem(
                Resource.PageChangeFrequencyYearly, "Yearly");
            ddChangeFrequency.Items.Add(listItem);

            listItem = new ListItem(
                Resource.PageChangeFrequencyNever, "Never");
            ddChangeFrequency.Items.Add(listItem);
            

        }

        private void PopulatePageList()
        {
            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "Csite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            PopulateListControl(ddPages, siteMapNode, string.Empty);

        }

        private void PopulateListControl(
            ListControl listBox,
            SiteMapNode siteMapNode,
            string pagePrefix)
        {
            CSiteMapNode CNode = (CSiteMapNode)siteMapNode;

            if (
                (!CNode.IsRootNode)
                //&&(CNode.IncludeInMenu) 
                // commented out 2010-02-18 at request but I have a vague recollection there was an issue that required this
                // it may have only been a concern it would cause support issues when people create new pages but then can't find them because
                // they are invisible.
                )
            {
                if (
                    (isAdmin)
                    || (isSiteEditor)
                    || (WebUser.IsInRoles(CNode.CreateChildPageRoles))
                    || (pageSettings.ParentId == CNode.PageId)
                    )
                {
                    // don't let children of this page be a choice for this page parent, its circular and causes an error
                    if ((CNode.ParentId != pageId)||(pageId == -1))
                    {
                        if (CNode.ParentId > -1) pagePrefix += "-";
                        ListItem listItem = new ListItem();
                        listItem.Text = pagePrefix + Server.HtmlDecode(CNode.Title);
                        listItem.Value = CNode.PageId.ToString();

                        listBox.Items.Add(listItem);
                    }
                }
            }


            foreach (SiteMapNode childNode in CNode.ChildNodes)
            {
                //recurse to populate children
                PopulateListControl(listBox, childNode, pagePrefix);

            }


        }

        

        private void Apply_Click(Object sender, EventArgs e) 
		{
            bool pageNewlyCreated = pageId == -1;
            if (SavePageData())
            {
                WebUtils.SetupRedirect(
                    this, String.Format(CultureInfo.InvariantCulture, "{0}/Admin/PageSettingsSaved.ashx?pageid={1}&pagenewlycreated={2}",
                    SiteRoot, pageId, pageNewlyCreated));

               
            }
		}


        void btnDelete_Click(object sender, EventArgs e)
        {
            if (pageSettings == null) return;

            metaRepository.DeleteByContent(pageSettings.PageGuid);
            Module.DeletePageModules(pageSettings.PageId);
            PageSettings.DeletePage(pageSettings.PageId);
            FriendlyUrl.DeleteByPageGuid(pageSettings.PageGuid);
            // needed for older versions where not every url has a pageguid
            FriendlyUrl.DeleteUrlByPageId(pageSettings.PageId);
            IndexHelper.ClearPageIndexAsync(pageSettings);
            CacheHelper.ResetSiteMapCache(siteSettings.SiteId);
            WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
        }


		private bool SavePageData()
		{
            Page.Validate();
            if (!Page.IsValid) { return false; ; }

            bool result = true;
			bool reIndexPage = false;
            bool clearIndex = false;
			int newParentID;

            if (!Int32.TryParse(ddPages.SelectedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out newParentID))
            {
                newParentID = -1;
            }

            PageSettings parentPage
                = new PageSettings(siteSettings.SiteId, newParentID);

            pageSettings.ParentGuid = parentPage.PageGuid;

            pageSettings.SiteId = siteSettings.SiteId;
            pageSettings.SiteGuid = siteSettings.SiteGuid;

            if (((pageSettings.PageId != newParentID) && (pageSettings.ParentId != newParentID))
                || (pageSettings.PageId == -1))
			{
                pageSettings.ParentId = newParentID;
                pageSettings.PageOrder = PageSettings.GetNextPageOrder(pageSettings.SiteId, newParentID);
			}

			if(isAdmin)
			{
                string authorizedRoles = chkListAuthRoles.Items.SelectedItemsToSemiColonSeparatedString();
				
                if (pageSettings.AuthorizedRoles != authorizedRoles)
				{
                    pageSettings.AuthorizedRoles = authorizedRoles;
					reIndexPage = true;
				}
                pageSettings.EditRoles = chkListEditRoles.Items.SelectedItemsToSemiColonSeparatedString();
                pageSettings.CreateChildPageRoles = chkListCreateChildPageRoles.Items.SelectedItemsToSemiColonSeparatedString(); ;
                pageSettings.DraftEditOnlyRoles = chkDraftEditRoles.Items.SelectedItemsToSemiColonSeparatedString();

			}

            if ((!isAdmin)&&(pageId == -1))
            {
                if (newParentID > -1)
                {
                    // by default inherit permissions from parent
                    pageSettings.AuthorizedRoles = parentPage.AuthorizedRoles;
                    pageSettings.EditRoles = parentPage.EditRoles;
                    pageSettings.CreateChildPageRoles = parentPage.CreateChildPageRoles;
                    pageSettings.DraftEditOnlyRoles = parentPage.DraftEditOnlyRoles;

                    if (WebUser.IsInRoles(parentPage.EditRoles))
                    {
                        pageSettings.EditRoles = parentPage.EditRoles;
                    }
                    else
                    {
                        pageSettings.EditRoles = parentPage.CreateChildPageRoles;
                    }
   
                }
  
            }
			

            pageSettings.PageName = SecurityHelper.RemoveMarkup(txtPageName.Text);
            pageSettings.PageTitle = txtPageTitle.Text;
			
			if(this.sslIsAvailable)
			{
                pageSettings.RequireSsl = chkRequireSSL.Checked;
			}
            pageSettings.AllowBrowserCache = chkAllowBrowserCache.Checked;
            pageSettings.ShowBreadcrumbs = chkShowBreadcrumbs.Checked;
            pageSettings.ShowChildPageBreadcrumbs = chkShowChildPageBreadcrumbs.Checked;
            pageSettings.ShowHomeCrumb = chkShowHomeCrumb.Checked;

            if ((WebConfigSettings.IndexPageMeta) && (pageSettings.PageMetaKeyWords != txtPageKeywords.Text)) 
            { reIndexPage = true; }
            pageSettings.PageMetaKeyWords = txtPageKeywords.Text;

            if ((WebConfigSettings.IndexPageMeta) && (pageSettings.PageMetaDescription != txtPageDescription.Text))
            { reIndexPage = true; }

            pageSettings.PageMetaDescription = txtPageDescription.Text;
            pageSettings.PageMetaEncoding = txtPageEncoding.Text;
            //pageSettings.PageMetaAdditional = txtPageAdditionalMetaTags.Text;
            pageSettings.UseUrl = chkUseUrl.Checked;
            
            pageSettings.OpenInNewWindow = chkNewWindow.Checked;
            pageSettings.ShowChildPageMenu = chkShowChildMenu.Checked;
            pageSettings.IncludeInMenu = chkIncludeInMenu.Checked;
            pageSettings.IncludeInSiteMap = chkIncludeInSiteMap.Checked;
            pageSettings.MenuImage = ddIcons.SelectedValue;
            pageSettings.HideAfterLogin = chkHideAfterLogin.Checked;
            pageSettings.IncludeInSearchMap = chkIncludeInSearchEngineSiteMap.Checked;
            pageSettings.CanonicalOverride = txtCannonicalOverride.Text;
            pageSettings.EnableComments = chkEnableComments.Checked;

			if(siteSettings.AllowPageSkins)
			{
                pageSettings.Skin = ddSkins.SelectedValue;
			}

			if(siteSettings.AllowHideMenuOnPages)
			{
                pageSettings.HideMainMenu = chkHideMainMenu.Checked;

			}

            String friendlyUrlString = txtUrl.Text.Replace("~/", String.Empty);

            //when using extensionless urls lets not allow a trailing slash
            //if the user enters on in the browser we can resolve it to the page
            //but its easier if we store them consistently in the db without the /
            if((friendlyUrlString.EndsWith("/"))&&(!friendlyUrlString.StartsWith("http")))
            {
                friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);
            }

            FriendlyUrl friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

            if (
                ((friendlyUrl.FoundFriendlyUrl) && (friendlyUrl.PageGuid != pageSettings.PageGuid))
                &&(pageSettings.Url != txtUrl.Text)
                && (!txtUrl.Text.StartsWith("http"))
                )
            {
                lblError.Text = Resource.PageUrlInUseErrorMessage;
                return false;
            }

            string oldUrl = pageSettings.Url.Replace("~/", string.Empty);
            string newUrl = friendlyUrlString;
            if ((txtUrl.Text.StartsWith("http"))||(txtUrl.Text == "~/"))
            {
                pageSettings.Url = txtUrl.Text;
            }
            else if(friendlyUrlString.Length > 0)
            {
                pageSettings.Url = "~/" + friendlyUrlString;
            }
            else if (friendlyUrlString.Length == 0)
            {
                pageSettings.Url = string.Empty;
            }
            

            pageSettings.ChangeFrequency = (PageChangeFrequency)Enum.Parse(typeof(PageChangeFrequency), ddChangeFrequency.SelectedValue);
            pageSettings.SiteMapPriority = ddSiteMapPriority.SelectedValue;

            

            
            if (pageSettings.PageId == -1)
            {
                pageSettings.PageCreated += new PageCreatedEventHandler(pageSettings_PageCreated);

                // no need to index new page until content is added
                reIndexPage = false;
            }

            if ((divIsPending.Visible) && (chkIsPending.Enabled))
            {
                if ((pageSettings.IsPending) && (!chkIsPending.Checked)) 
                { 
                    // page changed from draft to published so need to index
                    reIndexPage = true; 
                }

                if ((!pageSettings.IsPending) && (chkIsPending.Checked))
                {
                    //changed from published back to draft
                    //need to clear the search index 
                    reIndexPage = false;
                    clearIndex = true;
                }

                pageSettings.IsPending = chkIsPending.Checked;
            }

            pageSettings.LastModifiedUtc = DateTime.UtcNow;

            bool saved = pageSettings.Save();
            pageId = pageSettings.PageId;

            //if page was renamed url will change, if url changes we need to redirect from the old url to the new with 301
            // don't create a redirect for external urls, ie starting with "http"
            if (
                (oldUrl.Length > 0)
                && (newUrl.Length > 0) 
                && (!SiteUtils.UrlsMatch(oldUrl, newUrl))
                &&(!oldUrl.StartsWith("http"))
                &&(!newUrl.StartsWith("http"))
                )
            {
                //worry about the risk of a redirect loop if the page is restored to the old url again
                // don't create it if a redirect for the new url exists
                if (
                    (!RedirectInfo.Exists(siteSettings.SiteId, oldUrl))
                    && (!RedirectInfo.Exists(siteSettings.SiteId, newUrl))
                    )
                {
                    RedirectInfo redirect = new RedirectInfo();
                    redirect.SiteGuid = siteSettings.SiteGuid;
                    redirect.SiteId = siteSettings.SiteId;
                    redirect.OldUrl = oldUrl;
                    redirect.NewUrl = newUrl;
                    redirect.Save();
                }
                // since we have created a redirect we don't need the old friendly url
                FriendlyUrl oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);
                if ((oldFriendlyUrl.FoundFriendlyUrl) && (oldFriendlyUrl.PageGuid == pageSettings.PageGuid))
                {
                    FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
                }

            }

            if (
                ((txtUrl.Text.EndsWith(".aspx"))||siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageName)
                && (txtUrl.Text.StartsWith("~/"))
                )
            {
                
                if (!friendlyUrl.FoundFriendlyUrl)
                {
                    if (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString))
                    {
                        FriendlyUrl newFriendlyUrl = new FriendlyUrl();
                        newFriendlyUrl.SiteId = siteSettings.SiteId;
                        newFriendlyUrl.SiteGuid = siteSettings.SiteGuid;
                        newFriendlyUrl.PageGuid = pageSettings.PageGuid;
                        newFriendlyUrl.Url = friendlyUrlString;
                        newFriendlyUrl.RealUrl = "~/Default.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture);
                        newFriendlyUrl.Save();
                    }
                }

            }
            
            CacheHelper.TouchSiteSettingsCacheDependencyFile();
            CacheHelper.ResetSiteMapCache();

            if (saved && reIndexPage)
            {
                pageSettings.PageIndex = CurrentPage.PageIndex;
                IndexHelper.RebuildPageIndexAsync(pageSettings);
                SiteUtils.QueueIndexing();
            }
            else if (saved && clearIndex)
            {
                IndexHelper.ClearPageIndexAsync(pageSettings);
            }

		    return result;
		}

        void pageSettings_PageCreated(object sender, PageCreatedEventArgs e)
        {
            // this is a hook so that custom code can be fired when pages are created
            // implement a PageCreatedEventHandlerPovider and put a config file for it in
            // /Setup/ProviderConfig/pagecreatedeventhandlers
            try
            {
                foreach (PageCreatedEventHandlerPovider handler in PageCreatedEventHandlerPoviderManager.Providers)
                {
                    handler.PageCreatedHandler(sender, e);
                }
            }
            catch (TypeInitializationException ex)
            {
                log.Error(ex);
            }
        }

        #region Meta Data

        private void BindMeta()
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            List<ContentMeta> meta = metaRepository.FetchByContent(pageSettings.PageGuid);
            grdContentMeta.DataSource = meta;
            grdContentMeta.DataBind();

            btnAddMeta.Visible = true;
        }

        void grdContentMeta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            GridView grid = (GridView)sender;
            string sGuid = e.CommandArgument.ToString();
            if (sGuid.Length != 36) { return; }

            Guid guid = new Guid(sGuid);
            ContentMeta meta = metaRepository.Fetch(guid);
            if (meta == null) { return; }

            switch (e.CommandName)
            {
                case "MoveUp":
                    meta.SortRank -= 3;
                    break;

                case "MoveDown":
                    meta.SortRank += 3;
                    break;

            }

            metaRepository.Save(meta);
            List<ContentMeta> metaList = metaRepository.FetchByContent(pageSettings.PageGuid);
            metaRepository.ResortMeta(metaList);

            pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
            pageSettings.Save();

            BindMeta();
            upMeta.Update();


        }



        void grdContentMeta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.Delete(guid);

            pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
            pageSettings.Save();
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();
        }

        void grdContentMeta_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;

            BindMeta();

            //Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

            Button btnDeleteMeta = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMeta");
            if (btnDeleteMeta != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.ContentMetaDeleteWarning + "');");

                //if (guid == Guid.Empty) { btnDeleteMeta.Visible = false; }
            }

            upMeta.Update();
        }

        void grdContentMeta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (grid.EditIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddDirection = (DropDownList)e.Row.Cells[1].FindControl("ddDirection");
                    if (ddDirection != null)
                    {
                        if (e.Row.DataItem is ContentMeta)
                        {
                            ListItem item = ddDirection.Items.FindByValue(((ContentMeta)e.Row.DataItem).Dir);
                            if (item != null)
                            {
                                ddDirection.ClearSelection();
                                item.Selected = true;
                            }
                        }

                    }

                    if (!(e.Row.DataItem is ContentMeta))
                    {
                        //the add button was clicked so hide the delete button
                        Button btnDeleteMeta = (Button)e.Row.Cells[1].FindControl("btnDeleteMeta");
                        if (btnDeleteMeta != null) { btnDeleteMeta.Visible = false; }

                    }

                }

            }

        }

        void grdContentMeta_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            GridView grid = (GridView)sender;

            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            TextBox txtScheme = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtScheme");
            TextBox txtLangCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtLangCode");
            DropDownList ddDirection = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddDirection");
            TextBox txtMetaContent = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtMetaContent");

            ContentMeta meta = null;
            if (guid != Guid.Empty)
            {
                meta = metaRepository.Fetch(guid);
            }
            else
            {
                meta = new ContentMeta();
                if (currentUser != null) { meta.CreatedBy = currentUser.UserGuid; }
                meta.SortRank = metaRepository.GetNextSortRank(pageSettings.PageGuid);
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = pageSettings.PageGuid;
                meta.Dir = ddDirection.SelectedValue;
                meta.LangCode = txtLangCode.Text;
                meta.MetaContent = txtMetaContent.Text;
                meta.Name = txtName.Text;
                meta.Scheme = txtScheme.Text;
                if (currentUser != null) { meta.LastModBy = currentUser.UserGuid; }
                metaRepository.Save(meta);

                pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
                pageSettings.Save();

            }

            grid.EditIndex = -1;
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();

        }

        void grdContentMeta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdContentMeta.EditIndex = -1;
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();
        }

        void btnAddMeta_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("ContentGuid", typeof(Guid));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Scheme", typeof(string));
            dataTable.Columns.Add("LangCode", typeof(string));
            dataTable.Columns.Add("Dir", typeof(string));
            dataTable.Columns.Add("MetaContent", typeof(string));
            dataTable.Columns.Add("SortRank", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["ModuleGuid"] = Guid.Empty;
            row["ContentGuid"] = Guid.Empty;
            row["Name"] = string.Empty;
            row["Scheme"] = string.Empty;
            row["LangCode"] = string.Empty;
            row["Dir"] = string.Empty;
            row["MetaContent"] = string.Empty;
            row["SortRank"] = 3;

            dataTable.Rows.Add(row);

            grdContentMeta.EditIndex = 0;
            grdContentMeta.DataSource = dataTable.DefaultView;
            grdContentMeta.DataBind();
            grdContentMeta.Columns[2].Visible = false;
            btnAddMeta.Visible = false;

            upMeta.Update();

        }

        private void BindMetaLinks()
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            List<ContentMetaLink> meta = metaRepository.FetchLinksByContent(pageSettings.PageGuid);

            grdMetaLinks.DataSource = meta;
            grdMetaLinks.DataBind();

            btnAddMetaLink.Visible = true;
        }

        void btnAddMetaLink_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("ContentGuid", typeof(Guid));
            dataTable.Columns.Add("Rel", typeof(string));
            dataTable.Columns.Add("Href", typeof(string));
            dataTable.Columns.Add("HrefLang", typeof(string));
            dataTable.Columns.Add("SortRank", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["ModuleGuid"] = Guid.Empty;
            row["ContentGuid"] = Guid.Empty;
            row["Rel"] = string.Empty;
            row["Href"] = string.Empty;
            row["HrefLang"] = string.Empty;
            row["SortRank"] = 3;

            dataTable.Rows.Add(row);

            grdMetaLinks.Columns[2].Visible = false;
            grdMetaLinks.EditIndex = 0;
            grdMetaLinks.DataSource = dataTable.DefaultView;
            grdMetaLinks.DataBind();
            btnAddMetaLink.Visible = false;

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (grid.EditIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (!(e.Row.DataItem is ContentMetaLink))
                    {
                        //the add button was clicked so hide the delete button
                        Button btnDeleteMetaLink = (Button)e.Row.Cells[1].FindControl("btnDeleteMetaLink");
                        if (btnDeleteMetaLink != null) { btnDeleteMetaLink.Visible = false; }

                    }

                }

            }
        }

        void grdMetaLinks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.DeleteLink(guid);

            pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
            pageSettings.Save();

            grid.Columns[2].Visible = true;
            BindMetaLinks();

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdMetaLinks.EditIndex = -1;
            grdMetaLinks.Columns[2].Visible = true;
            BindMetaLinks();
            updMetaLinks.Update();
        }

        void grdMetaLinks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            GridView grid = (GridView)sender;

            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TextBox txtRel = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRel");
            TextBox txtHref = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHref");
            TextBox txtHrefLang = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHrefLang");

            ContentMetaLink meta = null;
            if (guid != Guid.Empty)
            {
                meta = metaRepository.FetchLink(guid);
            }
            else
            {
                meta = new ContentMetaLink();
                if (currentUser != null) { meta.CreatedBy = currentUser.UserGuid; }
                meta.SortRank = metaRepository.GetNextLinkSortRank(pageSettings.PageGuid);
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = pageSettings.PageGuid;
                meta.Rel = txtRel.Text;
                meta.Href = txtHref.Text;
                meta.HrefLang = txtHrefLang.Text;

                if (currentUser != null) { meta.LastModBy = currentUser.UserGuid; }
                metaRepository.Save(meta);

                pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
                pageSettings.Save();

            }

            grid.EditIndex = -1;
            grdMetaLinks.Columns[2].Visible = true;
            BindMetaLinks();
            updMetaLinks.Update();
        }

        void grdMetaLinks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;

            BindMetaLinks();

            Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMetaLink");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.ContentMetaLinkDeleteWarning + "');");

                if (guid == Guid.Empty) { btnDelete.Visible = false; }
            }

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (pageSettings == null) { return; }
            if (pageSettings.PageGuid == Guid.Empty) { return; }

            GridView grid = (GridView)sender;
            string sGuid = e.CommandArgument.ToString();
            if (sGuid.Length != 36) { return; }

            Guid guid = new Guid(sGuid);
            ContentMetaLink meta = metaRepository.FetchLink(guid);
            if (meta == null) { return; }

            switch (e.CommandName)
            {
                case "MoveUp":
                    meta.SortRank -= 3;
                    break;

                case "MoveDown":
                    meta.SortRank += 3;
                    break;

            }

            metaRepository.Save(meta);
            List<ContentMetaLink> metaList = metaRepository.FetchLinksByContent(pageSettings.PageGuid);
            metaRepository.ResortMeta(metaList);

            pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
            pageSettings.Save();

            BindMetaLinks();
            updMetaLinks.Update();
        }

        #endregion

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageSettingsPageTitle);

            litSettingsTab.Text = Resource.PageSettingsMainSettingsTab;
            litSecurityTab.Text = Resource.PageSettingsSecurityTab;
            litMetaSettingsTab.Text = Resource.PageSettingsMetaDataTab;
            litSEOTab.Text = Resource.PageSettingsSearchEngineOptimizationSettingsLabel;

            //lnkSSLTab.HRef = "#" + tabSSL.ClientID;
            //litSSLTab.Text = Resource.PageSettingsSSLTab;
            //litViewRolesTab.Text = Resource.PageLayoutAuthorizedRolesLabel;
            //litEditRolesTab.Text = Resource.PageLayoutEditRolesLabel;
            //litDraftEditRolesTab.Text = Resource.PageLayoutDraftEditRolesLabel;
            //litChildPageRolesTab.Text = Resource.PageLayoutCreateChildPageRolesLabel;
            //lnkDraftEditRoles.HRef = "#" + tabDraftEditRoles.ClientID;


            imgIcon.Alt = Resource.PageSettingsMenuImageAtlText;

            reqPageName.ErrorMessage = Resource.PageNameRequiredWarning;
            regexUrl.ErrorMessage = Resource.FriendlyUrlRegexWarning;

            if (pageId > -1)
            {
                applyBtn.Text = Resource.PageSettingsSaveButton;
                SiteUtils.SetButtonAccessKey(applyBtn, AccessKeys.PageSettingsSaveButtonAccessKey);

                btnDelete.Text = Resource.PageSettingsDeleteButton;
                SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.PageSettingsDeleteButtonAccessKey);
                UIHelper.AddConfirmationDialog(btnDelete, Resource.PageSettingsDeleteWarning);

                lnkEditContent.Text = Resource.AddFeaturesToPageLink;
                lnkViewPage.Text = Resource.PageViewPageLink;
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageSettingsNewPageTitle);
                applyBtn.Text = Resource.PageSettingsCreateNewPageButton;
                SiteUtils.SetButtonAccessKey(applyBtn, AccessKeys.PageSettingsSaveButtonAccessKey);

                if (!IsPostBack)
                {
                    txtPageName.Text = Resource.PageLayoutNewPageLabel;
                    chkIncludeInMenu.Checked = true;

                    if (autosuggestFriendlyUrls)
                    {
                        String friendlyUrl
                            = SiteUtils.SuggestFriendlyUrl(txtPageName.Text, siteSettings);

                        txtUrl.Text = "~/" + friendlyUrl;
                        chkUseUrl.Checked = true;
                    }
                }
            }

            //liSSL.Visible = false;
            tabSSL.Visible = false;

            if (!siteSettings.UseSslOnAllPages)
            {
                if (SiteUtils.SslIsAvailable())
                {
                    sslIsAvailable = true;
                   // liSSL.Visible = true;
                    tabSSL.Visible = true;
                }
            }

           

            //litUrlConflictWarning.Text = Resource.PageSettingsPhysicalUrlWarning;

            lnkPageTree.Visible = WebUser.IsAdminOrContentAdmin;
            lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
            lnkPageTree.ToolTip = Resource.AdminMenuPageTreeLink;
            lnkPageTree.NavigateUrl = SiteRoot + "/Admin/PageTree.aspx";

            btnAddMeta.Text = Resource.AddMetaButton;
            grdContentMeta.Columns[0].HeaderText = string.Empty;
            grdContentMeta.Columns[1].HeaderText = Resource.ContentMetaNameLabel;
            grdContentMeta.Columns[2].HeaderText = Resource.ContentMetaMetaContentLabel;

            btnAddMetaLink.Text = Resource.AddMetaLinkButton;

            grdMetaLinks.Columns[0].HeaderText = string.Empty;
            grdMetaLinks.Columns[1].HeaderText = Resource.ContentMetaRelLabel;
            grdMetaLinks.Columns[2].HeaderText = Resource.ContentMetaMetaHrefLabel;

        }

        private void SetupIconScript(HtmlImage imgIcon)
        {
            string logoScript = "<script type=\"text/javascript\">"
                + "function showIcon(listBox) { if(!document.images) return; "
                + "var iconPath = '" + iconPath + "'; "
                + "document.images." + imgIcon.ClientID + ".src = iconPath + listBox.value;"
                + "}</script>";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showIcon", logoScript);

        }


	    private void LoadSettings()
        {
            isAdmin = WebUser.IsAdminOrContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            startPageId = WebUtils.ParseInt32FromQueryString("start", -1);
            currentUser = SiteUtils.GetCurrentSiteUser();
            iconPath = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/";

            //divAdditionalMeta.Visible = WebConfigSettings.ShowAdditionalMeta;
            divPageEncoding.Visible = WebConfigSettings.ShowPageEncoding;


            h3DraftRoles.Visible = (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow);
            divIsPending.Visible = h3DraftRoles.Visible;
            divDraftRoles.Visible = h3DraftRoles.Visible;
            pnlComments.Visible = (!WebConfigSettings.DisableExternalCommentSystems);
            divUseUrl.Visible = WebConfigSettings.ShowUseUrlSettingInPageSettings;
            
        }

        

	    private void SetupScripts()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa", "<script type=\"text/javascript\" src=\""
                    + ResolveUrl("~/ClientScript/sarissa/sarissa.js") + "\"></script>");
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa_ieemu_xpath"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa_ieemu_xpath", "<script type=\"text/javascript\" src=\""
                    + ResolveUrl("~/ClientScript/sarissa/sarissa_ieemu_xpath.js") + "\"></script>");
            }

            if (autosuggestFriendlyUrls)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("friendlyurlsuggest"))
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "friendlyurlsuggest", "<script type=\"text/javascript\" src=\""
                        + ResolveUrl("~/ClientScript/friendlyurlsuggest_v2.js") + "\"></script>");
                }

                string hookupInputScript = "<script type=\"text/javascript\">"
                    + "new UrlHelper( "
                    + "document.getElementById('" + this.txtPageName.ClientID + "'),  "
                    + "document.getElementById('" + this.txtUrl.ClientID + "'), "
                    + "document.getElementById('" + this.hdnPageName.ClientID + "'), "
                    + "document.getElementById('" + this.spnUrlWarning.ClientID + "'), "
                    + "\"" + SiteRoot + "/Services/FriendlyUrlSuggestXml.aspx" + "\""
                    + ");</script>";

                if (!Page.ClientScript.IsStartupScriptRegistered(this.UniqueID + "urlscript"))
                {
                    this.Page.ClientScript.RegisterStartupScript(
                        this.GetType(), 
                        this.UniqueID + "urlscript", hookupInputScript);
                }
            }

        }
	}
}
