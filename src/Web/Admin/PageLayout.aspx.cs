
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.AdminUI 
{

    public partial class PageLayout : CBasePage
	{
        
        private bool canEdit = false;
        private bool isSiteEditor = false;
        private int pageID = -1;
        private bool pageHasAltContent1 = false;
        private bool pageHasAltContent2 = false;
        protected string EditSettingsImage = WebConfigSettings.EditPropertiesImage;
        protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            // this page needs to use the same skin as the page in case there are extra content place holders
            //SetMasterInBasePage = false;
            AllowSkinOverride = true;
            
            base.OnPreInit(e);

            //SiteUtils.SetMasterPage(this, siteSettings, true);

            //StyleSheetCombiner styleCombiner = (StyleSheetCombiner)Master.FindControl("StyleSheetCombiner");
            //if (styleCombiner != null) { styleCombiner.AllowPageOverride = true; }

            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            this.btnCreateNewContent.Click += new EventHandler(this.btnCreateNewContent_Click);

            this.LeftUpBtn.Click += new ImageClickEventHandler(LeftUpBtn_Click);
            this.LeftDownBtn.Click += new ImageClickEventHandler(LeftDownBtn_Click);
            this.ContentUpBtn.Click += new ImageClickEventHandler(ContentUpBtn_Click);
            this.ContentDownBtn.Click += new ImageClickEventHandler(ContentDownBtn_Click);
            this.RightUpBtn.Click += new ImageClickEventHandler(RightUpBtn_Click);
            this.RightDownBtn.Click += new ImageClickEventHandler(RightDownBtn_Click);

            this.btnAlt1MoveUp.Click += new ImageClickEventHandler(btnAlt1MoveUp_Click);
            this.btnAlt1MoveDown.Click += new ImageClickEventHandler(btnAlt1MoveDown_Click);
            this.btnAlt2MoveUp.Click += new ImageClickEventHandler(btnAlt2MoveUp_Click);
            this.btnAlt2MoveDown.Click += new ImageClickEventHandler(btnAlt2MoveDown_Click);
            
            
            this.LeftEditBtn.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.ContentEditBtn.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.RightEditBtn.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.btnEditAlt1.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.btnEditAlt2.Click += new ImageClickEventHandler(this.EditBtn_Click);
            
            this.LeftDeleteBtn.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.ContentDeleteBtn.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.RightDeleteBtn.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.btnDeleteAlt1.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.btnDeleteAlt2.Click += new ImageClickEventHandler(this.DeleteBtn_Click);

           
            this.LeftRightBtn.Click += new ImageClickEventHandler(LeftRightBtn_Click);
            this.ContentLeftBtn.Click += new ImageClickEventHandler(ContentLeftBtn_Click);
            this.ContentRightBtn.Click += new ImageClickEventHandler(ContentRightBtn_Click);
            this.RightLeftBtn.Click += new ImageClickEventHandler(RightLeftBtn_Click);

            this.btnMoveAlt1ToCenter.Click += new ImageClickEventHandler(btnMoveAlt1ToCenter_Click);
            this.btnMoveAlt1ToAlt2.Click += new ImageClickEventHandler(btnMoveAlt1ToAlt2_Click);
            this.btnMoveAlt2ToAlt1.Click += new ImageClickEventHandler(btnMoveAlt2ToAlt1_Click);
            
            
            this.ContentDownToNextButton.Click += new ImageClickEventHandler(ContentDownToNextButton_Click);

            

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

            LoadSettings();
            siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (WebUser.IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(CurrentPage.EditRoles))
			{
				canEdit = true;
			}

            if ((!canEdit) || (pageID != CurrentPage.PageId))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
                return;
			}

			PopulateLabels();

			if (!Page.IsPostBack) 
			{
				PopulateControls();
			}
		}

		
		private void PopulateControls() 
		{
            lblPageName.Text = CurrentPage.PageName;

			if(pageID > -1)
			{
				pnlContent.Visible = true;

				lnkEditSettings.NavigateUrl = String.Format("{0}/Admin/PageSettings.aspx?pageid={1}", SiteRoot, pageID);

                if (CurrentPage != null)
                {
                    lnkViewPage.NavigateUrl = SiteUtils.GetCurrentPageUrl();
                }
                else
                {
                    lnkViewPage.Visible = false;
                }

                BindFeatureList();

                BindPanes();

             

			}

		}

        private void BindPanes()
        {
            leftPane.Items.Clear();
            contentPane.Items.Clear();
            rightPane.Items.Clear();
            lbAltContent1.Items.Clear();
            lbAltContent2.Items.Clear();

            BindPaneModules(leftPane, "leftPane");
            BindPaneModules(contentPane, "contentPane");
            BindPaneModules(rightPane, "rightPane");

            if (pageHasAltContent1)
            {
                BindPaneModules(lbAltContent1, "altcontent1");
            }
            else
            {
                BindPaneModules(contentPane, "altcontent1");
            }

            if (pageHasAltContent2)
            {
                BindPaneModules(lbAltContent2, "altcontent2");
            }
            else
            {
                BindPaneModules(contentPane, "altcontent2");
            }


        }

        private void BindFeatureList()
        {
            using (IDataReader reader = ModuleDefinition.GetUserModules(siteSettings.SiteId))
            {
                ListItem listItem;
                while (reader.Read())
                {
                    listItem = new ListItem(
                        ResourceHelper.GetResourceString(
                        reader["ResourceFile"].ToString(),
                        reader["FeatureName"].ToString()),
                        reader["ModuleDefID"].ToString());

                    moduleType.Items.Add(listItem);

                }

            }

        }

        private ArrayList GetPaneModules(string pane)
        {
            ArrayList paneModules = new ArrayList();

            foreach (Module module in CurrentPage.Modules)
            {
                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, pane))
                {
                    paneModules.Add(module);
                }

                if (!pageHasAltContent1)
                {
                    if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent1"))
                    {
                        paneModules.Add(module);
                    }

                }

                if (!pageHasAltContent2)
                {
                    if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent2"))
                    {
                        paneModules.Add(module);
                    }

                }
            }

            return paneModules;
        }

        private void BindPaneModules(ListControl listControl, string pane)
        {
            
            foreach (Module module in CurrentPage.Modules)
            {
                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, pane))
                {
                    ListItem listItem = new ListItem(module.ModuleTitle.Coalesce(Resource.ContentNoTitle), module.ModuleId.ToInvariantString());
                    listControl.Items.Add(listItem);
                    
                }
            }

        }


		private void OrderModules (ArrayList list) 
		{
			int i = 1;
			list.Sort();
        
			foreach (Module m in list)
			{
				// number the items 1, 3, 5, etc. to provide an empty order
				// number when moving items up and down in the list.
				m.ModuleOrder = i;
				i += 2;
			}
		}

        private void btnCreateNewContent_Click(Object sender, EventArgs e)
		{
            int moduleDefID = int.Parse(moduleType.SelectedItem.Value);
            ModuleDefinition moduleDefinition = new ModuleDefinition(moduleDefID);

			Module m = new Module();
            m.SiteId = siteSettings.SiteId;
            m.SiteGuid = siteSettings.SiteGuid;
            m.ModuleDefId = moduleDefID;
            m.FeatureGuid = moduleDefinition.FeatureGuid;
            m.Icon = moduleDefinition.Icon;
            m.CacheTime = moduleDefinition.DefaultCacheTime;
			m.PageId = pageID;
			m.ModuleTitle = moduleTitle.Text;
            m.PaneName = ddPaneNames.SelectedValue;
			m.AuthorizedEditRoles = "Admins";
            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser != null)
            {
                m.CreatedByUserId = currentUser.UserId;
            }
            m.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
			m.Save();

            CurrentPage.RefreshModules();

            ArrayList modules = GetPaneModules(m.PaneName);
			OrderModules(modules);
        
			foreach (Module item in modules) 
			{
                Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, m.PaneName);
			}

			//WebUtils.SetupRedirect(this, Request.RawUrl);
			//return;

            CurrentPage.RefreshModules();
            BindPanes();
            upLayout.Update();
        }

        #region Move Up or Down


        void LeftUpBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(leftPane, pane, direction);
            
        }

        void LeftDownBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(leftPane, pane, direction);

        }

        void ContentUpBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(contentPane, pane, direction);

        }

        void ContentDownBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(contentPane, pane, direction);

        }

        void RightUpBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(rightPane, pane, direction);

        }

        void RightDownBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(rightPane, pane, direction);

        }

        void btnAlt1MoveUp_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent1, pane, direction);
        }

        void btnAlt1MoveDown_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent1, pane, direction);

        }

        void btnAlt2MoveUp_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent2, pane, direction);
        }

        void btnAlt2MoveDown_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent2, pane, direction);

        }

        private void MoveUpDown(ListBox listbox, string pane, string direction)
        {
            
            ArrayList modules = GetPaneModules(pane);

            if (listbox.SelectedIndex != -1)
            {
                int delta;
                int selection = -1;

                // Determine the delta to apply in the order number for the module
                // within the list.  +3 moves down one item; -3 moves up one item

                if (direction == "down")
                {
                    delta = 3;
                    if (listbox.SelectedIndex < listbox.Items.Count - 1)
                        selection = listbox.SelectedIndex + 1;
                }
                else
                {
                    delta = -3;
                    if (listbox.SelectedIndex > 0)
                        selection = listbox.SelectedIndex - 1;
                }

                Module m;
                m = (Module)modules[listbox.SelectedIndex];
                m.ModuleOrder += delta;

                OrderModules(modules);

                foreach (Module item in modules)
                {
                    Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, pane);
                }
            }

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            CurrentPage.RefreshModules();
            BindPanes();
            upLayout.Update();
        }

        #endregion

        #region Move To Pane

        
        void LeftRightBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "leftPane";
            string targetPane = "contentPane";
            MoveContent(leftPane, sourcePane, targetPane);

        }

        void ContentLeftBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "contentPane";
            string targetPane = "leftPane";
            MoveContent(contentPane, sourcePane, targetPane);

        }

        void ContentRightBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "contentPane";
            string targetPane = "rightPane";
            MoveContent(contentPane, sourcePane, targetPane);

        }

        void RightLeftBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "rightPane";
            string targetPane = "contentPane";
            MoveContent(rightPane, sourcePane, targetPane);

        }

        void ContentDownToNextButton_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "contentPane";
            string targetPane = "altcontent1";
            MoveContent(contentPane, sourcePane, targetPane);

        }

        void btnMoveAlt1ToCenter_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "altcontent1";
            string targetPane = "contentPane";
            MoveContent(lbAltContent1, sourcePane, targetPane);

        }

        void btnMoveAlt1ToAlt2_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "altcontent1";
            string targetPane = "altcontent2";
            MoveContent(lbAltContent1, sourcePane, targetPane);
        }

        void btnMoveAlt2ToAlt1_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "altcontent2";
            string targetPane = "altcontent1";
            MoveContent(lbAltContent2, sourcePane, targetPane);

        }

        private void MoveContent(ListBox listBox, string sourcePane, string targetPane)
        {
           
            if (listBox.SelectedIndex != -1)
            {
                ArrayList sourceList = GetPaneModules(sourcePane);

                Module m = (Module)sourceList[listBox.SelectedIndex];
                Module.UpdateModuleOrder(pageID, m.ModuleId, 998, targetPane);

                
                CurrentPage.RefreshModules();

                ArrayList modulesSource = GetPaneModules(sourcePane);
                OrderModules(modulesSource);

                foreach (Module item in modulesSource)
                {
                    Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, sourcePane);
                }

                ArrayList modulesTarget = GetPaneModules(targetPane);
                OrderModules(modulesTarget);

                foreach (Module item in modulesTarget)
                {
                    Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, targetPane);
                }

                BindPanes();
                upLayout.Update();
            }
        }


        #endregion

        


		private void EditBtn_Click(Object sender, ImageClickEventArgs e)
		{
			string pane = ((ImageButton)sender).CommandArgument;
            ListBox _listbox = (ListBox)this.MPContent.FindControl(pane);

			if (_listbox.SelectedIndex != -1) 
			{
				int mid = Int32.Parse(_listbox.SelectedItem.Value,CultureInfo.InvariantCulture);

				WebUtils.SetupRedirect(this, String.Format("{0}/Admin/ModuleSettings.aspx?mid={1}&pageid={2}", SiteRoot, mid, pageID));
			}
		}

        
		private void DeleteBtn_Click(Object sender, ImageClickEventArgs e) 
		{
            if (sender == null) return;

			string pane = ((ImageButton)sender).CommandArgument;
			ListBox listbox = (ListBox) this.MPContent.FindControl(pane);
			
			if (listbox.SelectedIndex != -1) 
			{
                
                int mid = Int32.Parse(listbox.SelectedItem.Value);
                Module.DeleteModuleInstance(mid, pageID);
                IndexHelper.RebuildPageIndexAsync(new PageSettings(siteSettings.SiteId, pageID));

			}

			//WebUtils.SetupRedirect(this, Request.RawUrl);
            CurrentPage.RefreshModules();
            BindPanes();
            upLayout.Update();
		}

        protected Collection<DictionaryEntry> PaneList()
        {
            Collection<DictionaryEntry> paneList = new Collection<DictionaryEntry>();
            paneList.Add(new DictionaryEntry(Resource.ContentManagerCenterColumnLabel, "contentpane"));
            paneList.Add(new DictionaryEntry(Resource.ContentManagerLeftColumnLabel, "leftpane"));
            paneList.Add(new DictionaryEntry(Resource.ContentManagerRightColumnLabel, "rightpane"));

            if (pageHasAltContent1)
            {
                paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel1Label, "altcontent1"));

            }

            if (pageHasAltContent2)
            {
                paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel2Label, "altcontent2"));

            }

            return paneList;
        }

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageLayoutPageTitle);
            
            btnCreateNewContent.Text = Resource.ContentManagerCreateNewContentButton;
            btnCreateNewContent.ToolTip = Resource.ContentManagerCreateNewContentButton;
            
            SiteUtils.SetButtonAccessKey
                (btnCreateNewContent, AccessKeys.ContentManagerCreateNewContentButtonAccessKey);

            lnkEditSettings.Text = Resource.PageLayoutEditSettingsLink;
            lnkEditSettings.ToolTip = Resource.PageLayoutEditSettingsLink;
            lnkViewPage.Text = Resource.PageViewPageLink;
            lnkViewPage.ToolTip = Resource.PageViewPageLink;

            LeftUpBtn.AlternateText = Resource.PageLayoutLeftUpAlternateText;
            LeftUpBtn.ToolTip = Resource.PageLayoutLeftUpAlternateText;

            LeftRightBtn.AlternateText = Resource.PageLayoutLeftRightAlternateText;
            LeftRightBtn.ToolTip = Resource.PageLayoutLeftRightAlternateText;

            LeftDownBtn.AlternateText = Resource.PageLayoutLeftDownAlternateText;
            LeftDownBtn.ToolTip = Resource.PageLayoutLeftDownAlternateText;

            LeftEditBtn.AlternateText = Resource.PageLayoutLeftEditAlternateText;
            LeftEditBtn.ToolTip = Resource.PageLayoutLeftEditAlternateText;
			LeftEditBtn.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, EditSettingsImage);

            LeftDeleteBtn.AlternateText = Resource.PageLayoutLeftDeleteAlternateText;
            LeftDeleteBtn.ToolTip = Resource.PageLayoutLeftDeleteAlternateText;
			LeftDeleteBtn.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, DeleteLinkImage);
            UIHelper.AddConfirmationDialog(LeftDeleteBtn, Resource.PageLayoutRemoveContentWarning);

            ContentUpBtn.AlternateText = Resource.PageLayoutContentUpAlternateText;
            ContentUpBtn.ToolTip = Resource.PageLayoutContentUpAlternateText;

            ContentLeftBtn.AlternateText = Resource.PageLayoutContentLeftAlternateText;
            ContentLeftBtn.ToolTip = Resource.PageLayoutContentLeftAlternateText;

            ContentRightBtn.AlternateText = Resource.PageLayoutContentRightAlternateText;
            ContentRightBtn.ToolTip = Resource.PageLayoutContentRightAlternateText;

            ContentDownBtn.AlternateText = Resource.PageLayoutContentDownAlternateText;
            ContentDownBtn.ToolTip = Resource.PageLayoutContentDownAlternateText;

            ContentEditBtn.AlternateText = Resource.PageLayoutContentEditAlternateText;
            ContentEditBtn.ToolTip = Resource.PageLayoutContentEditAlternateText;
			ContentEditBtn.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, EditSettingsImage);

            ContentDeleteBtn.AlternateText = Resource.PageLayoutContentDeleteAlternateText;
            ContentDeleteBtn.ToolTip = Resource.PageLayoutContentDeleteAlternateText;
			ContentDeleteBtn.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, DeleteLinkImage);
            UIHelper.AddConfirmationDialog(ContentDeleteBtn, Resource.PageLayoutRemoveContentWarning);

            RightUpBtn.AlternateText = Resource.PageLayoutRightUpAlternateText;
            RightUpBtn.ToolTip = Resource.PageLayoutRightUpAlternateText;

            RightLeftBtn.AlternateText = Resource.PageLayoutRightLeftAlternateText;
            RightLeftBtn.ToolTip = Resource.PageLayoutRightLeftAlternateText;

            RightDownBtn.AlternateText = Resource.PageLayoutRightDownAlternateText;
            RightDownBtn.ToolTip = Resource.PageLayoutRightDownAlternateText;

            RightEditBtn.AlternateText = Resource.PageLayoutRightEditAlternateText;
            RightEditBtn.ToolTip = Resource.PageLayoutRightEditAlternateText;
			RightEditBtn.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, EditSettingsImage);

            RightDeleteBtn.AlternateText = Resource.PageLayoutRightDeleteAlternateText;
            RightDeleteBtn.ToolTip = Resource.PageLayoutRightDeleteAlternateText;
			RightDeleteBtn.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, DeleteLinkImage);
            UIHelper.AddConfirmationDialog(RightDeleteBtn, Resource.PageLayoutRemoveContentWarning);

            litEditNotes.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.LayoutEditNotesFormat, "<a href='" + SiteUtils.GetCurrentPageUrl() + "' title='" + Resource.LayoutViewThePageLink + "'>"
                + Resource.LayoutViewThePageLink + "</a>");

            if (!Page.IsPostBack)
            {
                moduleTitle.Text = Resource.PageLayoutDefaultNewModuleName;
            }

            if (!Page.IsPostBack)
            {
                ddPaneNames.DataSource = PaneList();
                ddPaneNames.DataBind();
            }

            lnkPageTree.Visible = WebUser.IsAdminOrContentAdmin;
            lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
            lnkPageTree.ToolTip = Resource.AdminMenuPageTreeLink;
            lnkPageTree.NavigateUrl = SiteRoot + "/Admin/PageTree.aspx";

            this.rAltPanel1.Visible = pageHasAltContent1;
            this.rAltPanel2.Visible = pageHasAltContent2;

            ContentDownToNextButton.AlternateText = Resource.PageLayoutMoveCenterToAlt1Button;
            ContentDownToNextButton.ToolTip = Resource.PageLayoutMoveCenterToAlt1Button;

            btnMoveAlt1ToCenter.AlternateText = Resource.PageLayoutMoveAlt1ToCenterButton;
            btnMoveAlt1ToCenter.ToolTip = Resource.PageLayoutMoveAlt1ToCenterButton;

            btnAlt1MoveUp.AlternateText = Resource.PageLayoutAlt1MoveUpButton;
            btnAlt1MoveUp.ToolTip = Resource.PageLayoutAlt1MoveUpButton;

            btnAlt1MoveDown.AlternateText = Resource.PageLayoutAlt1MoveDownButton;
            btnAlt1MoveDown.ToolTip = Resource.PageLayoutAlt1MoveDownButton;

            btnMoveAlt1ToAlt2.AlternateText = Resource.PageLayoutMoveAlt1ToAlt2Button;
            btnMoveAlt1ToAlt2.ToolTip = Resource.PageLayoutMoveAlt1ToAlt2Button;

            btnEditAlt1.AlternateText = Resource.PageLayoutAlt1EditButton;
            btnEditAlt1.ToolTip = Resource.PageLayoutAlt1EditButton;
			btnEditAlt1.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, EditSettingsImage);

            btnDeleteAlt1.AlternateText = Resource.PageLayoutAlt1DeleteButton;
            btnDeleteAlt1.ToolTip = Resource.PageLayoutAlt1DeleteButton;
			btnDeleteAlt1.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, DeleteLinkImage);

            btnMoveAlt2ToAlt1.AlternateText = Resource.PageLayoutMoveAlt2ToAlt1Button;
            btnMoveAlt2ToAlt1.ToolTip = Resource.PageLayoutMoveAlt2ToAlt1Button;

            btnAlt2MoveUp.AlternateText = Resource.PageLayoutAlt2MoveUpButton;
            btnAlt2MoveUp.ToolTip = Resource.PageLayoutAlt2MoveUpButton;

            btnAlt2MoveDown.AlternateText = Resource.PageLayoutAlt2MoveDownButton;
            btnAlt2MoveDown.ToolTip = Resource.PageLayoutAlt2MoveDownButton;

            btnEditAlt2.AlternateText = Resource.PageLayoutAlt2EditButton;
            btnEditAlt2.ToolTip = Resource.PageLayoutAlt2EditButton;
			btnEditAlt2.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, EditSettingsImage);

            btnDeleteAlt2.AlternateText = Resource.PageLayoutAlt2DeleteButton;
            btnDeleteAlt2.ToolTip = Resource.PageLayoutAlt2DeleteButton;
			btnDeleteAlt2.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, DeleteLinkImage);

            if (pageHasAltContent1 || pageHasAltContent2)
            {
                ContentDownToNextButton.Visible = true;
                rAltInfo.Visible = true;
                btnMoveAlt1ToAlt2.Visible = pageHasAltContent2;
            }
            else
            {
                ContentDownToNextButton.Visible = false;
                rAltInfo.Visible = false;
            }

            

        }

        private void LoadSettings()
        {
            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            pageHasAltContent1 = this.ContainsPlaceHolder("altContent1");
            pageHasAltContent2 = this.ContainsPlaceHolder("altContent2");

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            if (ScriptController != null)
            {
                ScriptController.RegisterAsyncPostBackControl(btnCreateNewContent);

                ScriptController.RegisterAsyncPostBackControl(LeftUpBtn);
                ScriptController.RegisterAsyncPostBackControl(LeftDownBtn);
                ScriptController.RegisterAsyncPostBackControl(ContentUpBtn);
                ScriptController.RegisterAsyncPostBackControl(ContentDownBtn);
                ScriptController.RegisterAsyncPostBackControl(RightUpBtn);
                ScriptController.RegisterAsyncPostBackControl(RightDownBtn);
                ScriptController.RegisterAsyncPostBackControl(btnAlt1MoveUp);
                ScriptController.RegisterAsyncPostBackControl(btnAlt1MoveDown);
                ScriptController.RegisterAsyncPostBackControl(btnAlt2MoveUp);
                ScriptController.RegisterAsyncPostBackControl(btnAlt2MoveDown);
                ScriptController.RegisterAsyncPostBackControl(LeftEditBtn);


                ScriptController.RegisterAsyncPostBackControl(LeftDeleteBtn);
                ScriptController.RegisterAsyncPostBackControl(ContentDeleteBtn);
                ScriptController.RegisterAsyncPostBackControl(RightDeleteBtn);
                ScriptController.RegisterAsyncPostBackControl(btnDeleteAlt1);
                ScriptController.RegisterAsyncPostBackControl(btnDeleteAlt2);
                ScriptController.RegisterAsyncPostBackControl(LeftRightBtn);
                ScriptController.RegisterAsyncPostBackControl(ContentLeftBtn);
                ScriptController.RegisterAsyncPostBackControl(ContentRightBtn);
                ScriptController.RegisterAsyncPostBackControl(RightLeftBtn);
                ScriptController.RegisterAsyncPostBackControl(btnMoveAlt1ToCenter);
                ScriptController.RegisterAsyncPostBackControl(btnMoveAlt1ToAlt2);
                ScriptController.RegisterAsyncPostBackControl(btnMoveAlt2ToAlt1);
            }


        }

        


		
	}
}
