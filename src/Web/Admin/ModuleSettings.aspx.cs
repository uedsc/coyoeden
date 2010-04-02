/// Author:                     Joe Audette
///	Last Modified:              2009-11-14
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.UI;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI 
{
    public partial class ModuleSettingsPage : CBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ModuleSettingsPage));

		private Module module;
        private ArrayList DefaultSettings;
        private bool canEdit = false;
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private string iconPath;
        private String cacheDependencyKey;
        private int pageId = -1;
        private int moduleId = -1;
        private SiteMapDataSource siteMapDataSource;
        private string skinBaseUrl = string.Empty;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            
            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeYuiTabs = true;
            IncludeYuiTabsCss = true;

            LoadSettings();
            PopulateCustomSettings();
        }

        

       
        #endregion

        private void Page_Load(object sender, EventArgs e) 
		{
            SecurityHelper.DisableBrowserCache();

			divEditUser.Visible = false;

			lblValidationSummary.Text = string.Empty;


            if (!canEdit)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

			PopulateLabels();
            SetupIconScript();
            

			if (!Page.IsPostBack)
			{
                PopulateRoleList();
				PopulateControls();
			}
		}

		


		private void PopulateControls() 
		{
			if (module.ModuleId > -1)
			{
                ModuleDefinition moduleDefinition = new ModuleDefinition(module.ModuleDefId);
                lblFeatureName.Text 
                    = ResourceHelper.GetResourceString(
                    moduleDefinition.ResourceFile, 
                    moduleDefinition.FeatureName);

                litFeatureSpecificSettingsTab.Text = string.Format(CultureInfo.InvariantCulture, Resource.FeatureSettingsTabFormat, lblFeatureName.Text);

                divCacheTimeout.Visible = (!WebConfigSettings.DisableContentCache && moduleDefinition.IsCacheable);
                
                PopulatePageList();

				moduleTitle.Text = this.module.ModuleTitle;
				cacheTime.Text = this.module.CacheTime.ToString();
				chkShowTitle.Checked = this.module.ShowTitle;
                chkHideFromAuth.Checked = this.module.HideFromAuthenticated;
                chkHideFromUnauth.Checked = this.module.HideFromUnauthenticated;
                chkAvailableForMyPage.Checked = this.module.AvailableForMyPage;
                chkAllowMultipleInstancesOnMyPage.Checked = this.module.AllowMultipleInstancesOnMyPage;
				if(this.isAdmin)
				{
					divEditUser.Visible = true;

					if(module.EditUserId > 0)
					{
						SiteUser siteUser = new SiteUser(this.siteSettings ,module.EditUserId);
						this.scUser.Text = siteUser.Name;
						this.scUser.Value = siteUser.UserId.ToString();

					}
				}

                if (this.divParentPage.Visible)
                {
                    ListItem listItem = ddPages.Items.FindByValue(this.module.PageId.ToString());
                    if (listItem != null)
                    {
                        ddPages.ClearSelection();
                        listItem.Selected = true;
                    }

                   
                }

               
                if (module.Icon.Length > 0)
                {
                    ddIcons.SelectedValue = module.Icon;
                    imgIcon.Src = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/" + module.Icon;
                        
                }
                else
                {
                    imgIcon.Src = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/blank.gif";
                }

                foreach (ListItem item in cblViewRoles.Items)
                {
                    if ((this.module.ViewRoles.LastIndexOf(item.Value + ";")) > -1)
                    {
                        item.Selected = true;
                    }
                }

                foreach (ListItem item in authEditRoles.Items)
                {
                    if ((this.module.AuthorizedEditRoles.LastIndexOf(item.Value + ";")) > -1)
                    {
                        item.Selected = true;
                    }
                }

                foreach (ListItem item in draftEditRoles.Items)
                {
                    if ((this.module.DraftEditRoles.LastIndexOf(item.Value + ";")) > -1)
                    {
                        item.Selected = true;
                    }
                }

                cblViewRoles.Enabled = isAdmin;
                authEditRoles.Enabled = isAdmin;
				

				
			}
		}

        private void PopulateRoleList()
        {
            authEditRoles.Items.Clear();
            cblViewRoles.Items.Clear();

            ListItem vAllItem = new ListItem();
            vAllItem.Text = Resource.RolesAllUsersRole;
            vAllItem.Value = "All Users";

            ListItem allItem = new ListItem();
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";

            //if (this.module.AuthorizedEditRoles.LastIndexOf("All Users") > -1)
            //{
            //    allItem.Selected = true;
            //}

            cblViewRoles.Items.Add(vAllItem);
            authEditRoles.Items.Add(allItem);

            using (IDataReader roles = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (roles.Read())
                {
                    ListItem vItem = new ListItem();
                    vItem.Text = roles["DisplayName"].ToString();
                    vItem.Value = roles["RoleName"].ToString();
                    cblViewRoles.Items.Add(vItem);

                    ListItem item = new ListItem();
                    item.Text = roles["DisplayName"].ToString();
                    item.Value = roles["RoleName"].ToString();
                    authEditRoles.Items.Add(item);

                    ListItem draftItem = new ListItem();
                    draftItem.Text = roles["DisplayName"].ToString();
                    draftItem.Value = roles["RoleName"].ToString();
                    draftEditRoles.Items.Add(draftItem);

                }
            }

            
            cblViewRoles.Enabled = isAdmin;
            authEditRoles.Enabled = isAdmin;
            draftEditRoles.Enabled = isAdmin;
            liSecurity.Visible = isAdmin;
            tabSecurity.Visible = isAdmin;
            divMyPage.Visible = isAdmin;
            divMyPageMulti.Visible = isAdmin;
            

        }

        private void PopulateCustomSettings()
        {
            // these are the Settings attached to the Module Definition
            DefaultSettings = ModuleSettings.GetDefaultSettings(this.module.ModuleDefId);
            // these are the settings attached to the module instance
            ArrayList customSettingValues = ModuleSettings.GetCustomSettingValues(this.module.ModuleId);

            foreach (CustomModuleSetting s in DefaultSettings)
            {
                bool found = false;
                foreach (CustomModuleSetting v in customSettingValues)
                {
                    if (v.SettingName == s.SettingName)
                    {
                        found = true;
                        AddSettingControl(v);

                    }
                }

                if (!found)
                {
                    // if a new module setting has been added since the
                    // last version upgrade, the code might reach this
                    // it means a Module Definition Setting was found for which there is not
                    // a Module Setting on this instance of the module, so we need to add the setting

                    ModuleSettings.CreateModuleSetting(
                        module.ModuleGuid,
                        moduleId,
                        s.SettingName,
                        s.SettingValue,
                        s.SettingControlType,
                        s.SettingValidationRegex,
                        s.ControlSrc,
                        s.HelpKey,
                        s.SortOrder);

                    // add control with default settings
                    AddSettingControl(s);
                }

            }
        }

        private void PopulatePageList()
        {
            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "Csite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            PopulateListControl(ddPages, siteMapNode, string.Empty);

        }

        private void PopulateListControl(
            ListControl listBox,
            SiteMapNode siteMapNode,
            string pagePrefix)
        {
            CSiteMapNode CNode = (CSiteMapNode)siteMapNode;

            if (!CNode.IsRootNode)
            {
                if (
                    (isAdmin)
                    || (WebUser.IsInRoles(CNode.EditRoles))
                    || (CNode.PageId == module.PageId)
                    )
                {
                    if (CNode.ParentId > -1) pagePrefix += "-";
                    ListItem listItem = new ListItem();
                    listItem.Text = pagePrefix + Server.HtmlDecode(CNode.Title);
                    listItem.Value = CNode.PageId.ToString();

                    listBox.Items.Add(listItem);
                }
            }


            foreach (SiteMapNode childNode in CNode.ChildNodes)
            {
                //recurse to populate children
                PopulateListControl(listBox, childNode, pagePrefix);

            }


        }


		private void AddSettingControl(CustomModuleSetting s)
		{
            if (s.SettingName == "WebPartModuleWebPartSetting")
            {
                // Special handling for this one
                this.divWebParts.Visible = true;
                using (IDataReader reader = WebPartContent.SelectBySite(siteSettings.SiteId))
                {
                    this.ddWebParts.DataSource = reader;
                    this.ddWebParts.DataBind();
                }
                if (s.SettingValue.Length == 36)
                {
                    ListItem listItem = ddWebParts.Items.FindByValue(s.SettingValue);
                    if (listItem != null)
                    {
                        ddWebParts.ClearSelection();
                        listItem.Selected = true;

                    }
                    
                }

            }
            else
            {
                if (s.SettingControlType == string.Empty) { return; }

                String settingLabel = s.SettingName;
                String resourceFile = "Resource";
                if (s.ResourceFile.Length > 0)
                {
                    resourceFile = s.ResourceFile;
                }

                try
                {
                    settingLabel = GetGlobalResourceObject(resourceFile, s.SettingName).ToString();
                }
                catch (NullReferenceException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("ModuleSettings.aspx.cs error getting resource for s.SettingName " + s.SettingName, ex);
                    }
                }
                
                Panel panel = new Panel();
                panel.CssClass = "settingrow";
                Literal label = new Literal();
                label.Text = "<label class='settinglabel' >" + settingLabel + "</label>";
                panel.Controls.Add(label);

                if ((s.SettingControlType == "TextBox") || (s.SettingControlType == string.Empty))
                {
                    Literal textBox = new Literal();
                    textBox.Text = "<input name=\""
                        + s.SettingName + this.moduleId.ToInvariantString()
                        + "\" type='text' class=\"forminput\" value=\"" + s.SettingValue.HtmlEscapeQuotes()
                        + "\" size=\"45\" id=\"" + s.SettingName + this.moduleId.ToInvariantString() + "\" />";

                    panel.Controls.Add(textBox);

                }

                if (s.SettingControlType == "CheckBox")
                {
                    Literal checkBox = new Literal();
                    String isChecked = String.Empty;

                    if (string.Equals(s.SettingValue, "true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isChecked = "checked";
                    }

                    checkBox.Text = "<input id='"
                        + s.SettingName + this.moduleId.ToInvariantString()
                        + "' type='checkbox' class='forminput' " + isChecked
                        + " name='" + s.SettingName + this.moduleId.ToInvariantString() + "' />";

                    panel.Controls.Add(checkBox);

                }

                if (s.SettingControlType == "ISettingControl")
                {
                    if (s.ControlSrc.Length > 0)
                    {
                        Control uc = Page.LoadControl(s.ControlSrc);
                        if (uc is ISettingControl)
                        {
                            
                            ISettingControl sc = uc as ISettingControl;
                            if(!IsPostBack)
                            sc.SetValue(s.SettingValue);

                            uc.ID = "uc" + moduleId.ToString(CultureInfo.InvariantCulture) + s.SettingName;
                            panel.Controls.Add(uc);
                        }

                    }
                    else
                    {
                        log.Error("could not add setting control for ISettingControl, missing controlsrc for " + s.SettingName);
                    }
                }

                if (s.HelpKey.Length > 0)
                {
                    CHelpLink.AddHelpLink(panel, s.HelpKey);
                }

                this.PlaceHolderAdvancedSettings.Controls.Add(panel);
            }
		
		}

        private void btnSave_Click(Object sender, EventArgs e) 
		{
            if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage about to call Page.Validate()");

			Page.Validate();
			if(Page.IsValid)
			{
                if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage about to call Page IsValid = true");

				bool ok = true;
				bool needToReIndex = false;
				int currentPageId = module.PageId;
				int newPageId = module.PageId;

	
				if (module.ModuleId > -1)
				{
					if(isAdmin)
					{
                        string viewRoles = string.Empty;

                        foreach (ListItem item in cblViewRoles.Items)
                        {
                            if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage inside loop of Role ListItems");
                            if (item.Selected == true)
                            {
                                viewRoles = viewRoles + item.Value + ";";
                            }
                        }
                        string previousViewRoles = this.module.ViewRoles;
                        this.module.ViewRoles = viewRoles;
                        if (previousViewRoles != viewRoles) { needToReIndex = true; }

						string editRoles = string.Empty;
                        if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage about to loop through Role ListItems");

						foreach(ListItem item in authEditRoles.Items) 
						{
                            if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage inside loop of Role ListItems");
							if (item.Selected == true) 
							{
								editRoles = editRoles + item.Value + ";";
							}
						}

						this.module.AuthorizedEditRoles = editRoles;

                        string draftEdits = string.Empty;
                        
                        foreach (ListItem item in draftEditRoles.Items)
                        {
                            
                            if (item.Selected == true)
                            {
                                draftEdits = draftEdits + item.Value + ";";
                            }
                        }

                        this.module.DraftEditRoles = draftEdits;
					}

                    if (tabGeneralSettings.Visible)
                    {

                        this.module.ModuleTitle = moduleTitle.Text;
                        this.module.CacheTime = int.Parse(cacheTime.Text);

                        this.module.ShowTitle = chkShowTitle.Checked;
                        this.module.AvailableForMyPage = this.chkAvailableForMyPage.Checked;
                        this.module.AllowMultipleInstancesOnMyPage = this.chkAllowMultipleInstancesOnMyPage.Checked;
                        this.module.Icon = this.ddIcons.SelectedValue;
                        this.module.HideFromAuthenticated = chkHideFromAuth.Checked;
                        this.module.HideFromUnauthenticated = chkHideFromUnauth.Checked;

                        if (this.divParentPage.Visible)
                        {
                            if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage about to check Page dropdown");
                            newPageId = int.Parse(this.ddPages.SelectedValue);
                            if (newPageId != currentPageId)
                            {
                                needToReIndex = true;
                                Module.UpdatePage(currentPageId, newPageId, this.module.ModuleId);
                            }
                            this.module.PageId = newPageId;
                        }

                        if (this.isAdmin)
                        {
                            if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage about to check user dropdown");
                            if (this.scUser.Value.Length > 0)
                            {
                                try
                                {
                                    this.module.EditUserId = int.Parse(this.scUser.Value);
                                }
                                catch (ArgumentException) { }
                                catch (FormatException) { }
                                catch (OverflowException) { }
                            }
                            else
                            {
                                this.module.EditUserId = 0;
                            }

                        }

                    }

                    if (log.IsDebugEnabled) log.Debug("ModuleSettingsPage about to Save Module");
					this.module.Save();

                    if (needToReIndex)
                    {
                        // if content is moved from 1 page to another, need to reindex both pages
                        // to keep view permissions in sync

                        IndexHelper.RebuildPageIndexAsync(CurrentPage);

                        PageSettings newPage = new PageSettings(siteSettings.SiteId, newPageId);
                        newPage.PageIndex = 0;
                        IndexHelper.RebuildPageIndexAsync(newPage);
                    }

				    ArrayList defaultSettings = ModuleSettings.GetDefaultSettings(this.module.ModuleDefId);
					foreach(CustomModuleSetting s in defaultSettings)
					{
                        if (s.SettingControlType == string.Empty) { continue; }
                        ok = true;

                        Object oSettingLabel = GetGlobalResourceObject("Resource", s.SettingName + "RegexWarning");
                        String settingLabel = String.Empty;
                        if (oSettingLabel == null)
                        {
                            settingLabel = "Regex Warning";
                        }
                        else
                        {
                            settingLabel = oSettingLabel.ToString();
                        }

                        string settingValue = string.Empty;

                        if (s.SettingName == "WebPartModuleWebPartSetting")
                        {
                            ModuleSettings.UpdateModuleSetting(this.module.ModuleGuid, moduleId, s.SettingName, ddWebParts.SelectedValue);
                        }
                        else
                        {
                            if (s.SettingControlType == "ISettingControl")
                            {
                                string controlID = "uc" + moduleId.ToString(CultureInfo.InvariantCulture) + s.SettingName;
                                Control c = PlaceHolderAdvancedSettings.FindControl(controlID);
                                if (c != null)
                                {
                                    if (c is ISettingControl)
                                    {
                                        ISettingControl isc = c as ISettingControl;
                                        settingValue = isc.GetValue();
                                    }
                                    else
                                    {
                                        ok = false;
                                    }

                                }
                                else
                                {
                                    log.Error("could not find control for " + s.SettingName);
                                    ok = false;
                                }

                            }
                            else
                            {

                                settingValue = Request.Params.Get(s.SettingName + this.moduleId.ToString(CultureInfo.InvariantCulture));

                                if (s.SettingControlType == "CheckBox")
                                {
                                    if (settingValue == "on")
                                    {
                                        settingValue = "true";
                                    }
                                    else
                                    {
                                        settingValue = "false";
                                    }
                                }
                                else
                                {
                                    if (s.SettingValidationRegex.Length > 0)
                                    {
                                        if (!Regex.IsMatch(settingValue, s.SettingValidationRegex))
                                        {
                                            ok = false;
                                            this.lblValidationSummary.Text += "<br />"
                                                + settingLabel;

                                        }
                                    }
                                }
                            }

                            if (ok)
                            {
                                ModuleSettings.UpdateModuleSetting(this.module.ModuleGuid, moduleId, s.SettingName, settingValue);
                            }
                        }
					}
				}

				if(ok)
				{
                    CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
					return;

				}
				
			}
		}

        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (module.ModuleId > -1)
            {
                DataTable pageIds = Module.GetPageModulesTable(module.ModuleId);

                foreach (DataRow row in pageIds.Rows)
                {
                    int pageId = Convert.ToInt32(row["PageID"]);
                    Module.DeleteModuleInstance(module.ModuleId, pageId);
                    IndexHelper.RebuildPageIndexAsync(new PageSettings(siteSettings.SiteId, pageId));
                }

                ModuleDefinition feature = new ModuleDefinition(module.ModuleDefId);

                if (feature.DeleteProvider.Length > 0)
                {
                    try
                    {
                        ContentDeleteHandlerProvider contentDeleter = ContentDeleteHandlerProviderManager.Providers[feature.DeleteProvider];
                        if (contentDeleter != null)
                        {
                            contentDeleter.DeleteContent(module.ModuleId, module.ModuleGuid);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to invoke content delete handler " + feature.DeleteProvider, ex);
                    }
                }

                Module.DeleteModule(module.ModuleId);
                CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
            }

            
            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        

        private void PopulatePageArray(ArrayList sitePages)
        {
            SiteMapDataSource siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "Csite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
            CSiteMapProvider.PopulateArrayList(sitePages, siteMapNode);

           
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ModuleSettingsPageTitle);
            litFeatureSpecificSettingsTab.Text = Resource.ModuleSettingsSettingsTab;
            litGeneralSettingsTab.Text = Resource.ModuleSettingsGeneralTab;
            litSecurityTab.Text = Resource.ModuleSettingsSecurityTab;

            btnSave.Text = Resource.ModuleSettingsApplyButton;
            SiteUtils.SetButtonAccessKey(btnSave, AccessKeys.ModuleSettingsApplyButtonAccessKey);

            btnDelete.Text = Resource.ModuleSettingsDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.ModuleSettingsDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.ModuleSettingsDeleteConfirm);

            lnkCancel.Text = Resource.ModuleSettingsCancelButton;

            if (!Page.IsPostBack)
            {
                FileInfo[] fileInfo = SiteUtils.GetFeatureIconList();
                this.ddIcons.DataSource = fileInfo;
                this.ddIcons.DataBind();

                ddIcons.Items.Insert(0, new ListItem(Resource.ModuleSettingsNoIconLabel, "blank.gif"));
                ddIcons.Attributes.Add("onChange", "javascript:showIcon(this);");
                ddIcons.Attributes.Add("size", "6");
            }

            scUser.ValueLabelText = Resource.ModuleSettingsEditUserIDLabel;
            scUser.DataUrl = SiteRoot + "/Services/UserDropDownXml.aspx?query=";
            scUser.ButtonImageUrl = ImageSiteRoot + "/Data/SiteImages/DownArrow.gif";

            reqCacheTime.ErrorMessage = Resource.ModuleSettingsCacheRequiredMessage;
            regexCacheTime.ErrorMessage = Resource.ModuleSettingsCacheRegexWarning;

            lnkGeneralSettingsTab.HRef = "#" + tabGeneralSettings.ClientID;
            lnkSecurityTab.HRef = "#" + tabSecurity.ClientID;

            
#if MONO
            divMyPage.Visible = false;
            divMyPageMulti.Visible = false;
#endif


        }


	    private void SetupIconScript()
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
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            cacheDependencyKey = "Module-" + moduleId.ToString();
            iconPath = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/";
            skinBaseUrl = SiteUtils.GetSkinBaseUrl(this);
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();


            if ((WebUser.IsAdminOrContentAdmin) || (isSiteEditor))
            {
                canEdit = true;
                isAdmin = true;
                lnkEditContent.Visible = true;
                lnkEditContent.Text = Resource.ContentManagerViewEditContentLabel;
                lnkEditContent.NavigateUrl = SiteRoot
                    + "/Admin/ContentManagerPreview.aspx?mid=" + this.moduleId.ToString(CultureInfo.InvariantCulture);

                lnkPublishing.Visible = true;
                lnkPublishing.Text = Resource.ContentManagerPublishingContentLink;
                lnkPublishing.NavigateUrl = SiteRoot
                    + "/Admin/ContentManager.aspx?mid=" + this.moduleId.ToString(CultureInfo.InvariantCulture);

            }
            else
            {
                bool hideOtherTabs = WebConfigSettings.HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins;
                if (hideOtherTabs)
                {
                    liGeneralSettings.Visible = false;
                    liSecurity.Visible = false;
                    tabGeneralSettings.Visible = false;
                    tabSecurity.Visible = false;
                }

            }

            

            divCacheTimeout.Visible = !WebConfigSettings.DisableContentCache;
            pnlDraftEditRoles.Visible = (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow);

            if (pageId > -1)
            {
                this.divParentPage.Visible = true;
                module = new Module(this.moduleId, pageId);
            }
            else
            {
                module = new Module(this.moduleId);
            }

            if (!canEdit)
            {
                if (
                    (WebUser.IsInRoles(module.AuthorizedEditRoles))
                    || (WebUser.IsInRoles(module.DraftEditRoles))
                    || (WebUser.IsInRoles(CurrentPage.EditRoles))
                    || (WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles))
                    )
                {
                    canEdit = true;
                }
            }

            if (!canEdit)
            {
                if (module.EditUserId > 0)
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if (module.EditUserId == siteUser.UserId)
                    {
                        canEdit = true;
                    }
                }
            }

            if (module.SiteGuid != siteSettings.SiteGuid)
            {
                canEdit = false;
            }

            if (canEdit &&(!isAdmin) &&(WebUser.IsInRoles(siteSettings.RolesNotAllowedToEditModuleSettings)))
            { canEdit = false; }

        }

        
        
		
	}
}
