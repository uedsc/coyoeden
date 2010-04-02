/// Author:             	Joe Audette
/// Created:            	2006-05-01
/// Last Modified:			2010-01-10

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

using Resources;

using Module=Cynthia.Business.Module;

#if !MONO
using System.Web.UI.WebControls.WebParts;
using Cynthia.Web.WebParts;
#endif

namespace Cynthia.Web.UI.Pages
{
   
    public partial class MyPage : CBasePage
    {
#if!MONO
        private WebPartManager WebPartManager1;
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private bool isAutheticated = false;
        private SiteUser currentUser;
        private String userPageCookie = "userpagecookie";
        private string defaultUserPageCookie = "ad303a10-97d5-4194-b4a2-2988aef883eb";
       
        protected bool CanDeletePage = false;
        protected bool CanMoveLeft = false; 
        protected bool CanMoveRight = true; 
        
        private int countOfUserPages = 0;
        private bool allowView = true;
        
        
        
        #region ViewState Properties

        private Guid CurrentUserPageId
        {
            get
            {
                object obj = ViewState["CurrentUserPageID"];
                return (obj != null) ? (Guid)obj : Guid.Empty;
            }
            set
            {
                ViewState["CurrentUserPageID"] = value;
            }
        }

        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            EnsureSiteSettings();
        
            if (siteSettings.MyPageSkin.Length == 0) 
            {
                base.OnPreInit(e);
                return; 
            }

            bool isPreviewSkin = (Request.Params.Get("skin") != null);

            SetMasterInBasePage = isPreviewSkin;

            base.OnPreInit(e);

            if (isPreviewSkin) { return; }


            if (siteSettings.MyPageSkin.Length > 0)
            {
                SiteUtils.SetMasterPage(this, siteSettings, siteSettings.MyPageSkin);
            }
            else
            {
                SiteUtils.SetMasterPage(this, siteSettings, siteSettings.Skin);
            }

            MPLeftPane = (ContentPlaceHolder)Master.FindControl("leftContent");
            MPContent = (ContentPlaceHolder)Master.FindControl("mainContent");
            MPRightPane = (ContentPlaceHolder)Master.FindControl("rightContent");
            MPPageEdit = (ContentPlaceHolder)Master.FindControl("pageEditContent");
            AltPane1 = (ContentPlaceHolder)Master.FindControl("altContent1");
            AltPane2 = (ContentPlaceHolder)Master.FindControl("altContent2");

            StyleSheetCombiner style = Page.Master.FindControl("StyleSheetCombiner") as StyleSheetCombiner;
            if (style == null) { return; }

            if (siteSettings.MyPageSkin.Length > 0)
            {
                style.OverrideSkinName = siteSettings.MyPageSkin;
            }
            else
            {
                style.OverrideSkinName = siteSettings.Skin;
            }



        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (WebConfigSettings.HideAllMenusOnMyPage)
            {
                SuppressAllMenus();
            }
            else
            {
                SuppressMenuSelection();
                SuppressPageMenu();
            }
            
            this.Load += new EventHandler(this.Page_Load);
            this.cmdCatalogView.Click += new ImageClickEventHandler(cmdCatalogView_Click);
            cmdResetPersonalization.Click += new ImageClickEventHandler(cmdResetPersonalization_Click);
            
            this.cmdPersonalizationModeToggle.Click += new ImageClickEventHandler(cmdPersonalizationModeToggle_Click);

            WebPartManager1 = WebPartManager.GetCurrentWebPartManager(Page);

            this.WebPartManager1.WebPartAdded += new WebPartEventHandler(WebPartManager1_WebPartAdded);
            this.WebPartManager1.WebPartClosed += new WebPartEventHandler(WebPartManager1_WebPartClosed);
            this.WebPartManager1.WebPartDeleted += new WebPartEventHandler(WebPartManager1_WebPartDeleted);
            this.WebPartManager1.WebPartMoved += new WebPartEventHandler(WebPartManager1_WebPartMoved);
            

            this.rptUserPageMenu.ItemCommand += new RepeaterCommandEventHandler(rptUserPageMenu_ItemCommand);
            this.rptUserPageMenu.ItemCreated += new RepeaterItemEventHandler(rptUserPageMenu_ItemCreated);

            this.btnNewPage.Click += new EventHandler(btnNewPage_Click);
            this.btnCancelAddPage.Click += new EventHandler(btnCancelAddPage_Click);
            this.btnChangeName.Click += new EventHandler(btnChangeName_Click);
            this.btnCancelChangeName.Click += new EventHandler(btnCancelChangeName_Click);

            RegisterScripts();

            if (Request.IsAuthenticated)
            {
                if (Request.Params.Get("addpart") != null)
                {
                    AddWebPart();
                }
                else
                {
                    if (Request.Params.Get("addmpart") != null)
                    {
                        AddModulePart();
                    }
                }
            }

            
        }

        

        

        #endregion

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((SiteUtils.SslIsAvailable()) && (WebConfigSettings.UseSslForMyPage))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }
            
            if (!siteSettings.EnableMyPageFeature) { allowView = false; }

            if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMyPage)) { allowView = false; }

            if (!allowView)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }
            
                
            //WebPartManager1 = WebPartManager.GetCurrentWebPartManager(Page);

            if (Request.IsAuthenticated)
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                
                isAutheticated = true;
                if (WebUser.IsAdminOrContentAdmin)
                {
                    isAdmin = true;
                }
                isSiteEditor = SiteUtils.UserIsSiteEditor();

            }

            SetupCss();
            EnsureUserPage();
            PopulateLabels();
          
            if (Request.IsAuthenticated)
            {
                // TODO: to support anonymous session
                // personalization need to figure out how thy are doing it
                // at pageflakes to enable edit mode when unauthenticated
                if (!IsPostBack)
                {
                    BindUserMenu();
                    WebPartManager1.DisplayMode = WebPartManager.EditDisplayMode;

                    // I don't want the zone titles to display unless
                    // in catalog view. If set to String.Emtpy it doesn't
                    // make them blank but shows the server side id instead
                    // setting to a space works for clearing it
                    LeftWebPartZone.HeaderText = " ";
                    CenterWebPartZone.HeaderText = " ";
                    RightWebPartZone.HeaderText = " ";
                    CatalogZone1.HeaderText = " ";
                }
            }

            if (!IsPostBack)
            {
                this.pnlAddPage.Visible = false;
                this.pnlChangeName.Visible = false;
            }

                
            
        }

        private void EnsureUserPage()
        {
            // check if the user has a page cookie, if so nothing to do
            // the personalization provider will populate
            if (CookieHelper.CookieExists(userPageCookie))
            {
                return;
            }
            else
            {
                //prevent a redirect loop if the user agent isn't taking cookies
                if (Request.Params["c"] != null) { return; }
            }

            // else

            if (currentUser != null)
            {
                String userPageIDString = UserPage.GetDefaultPagePath(
                    currentUser.UserGuid,
                    siteSettings,
                    Resource.MyPageDefaultUserPageName,
                    defaultUserPageCookie);

                CookieHelper.SetPersistentCookie(userPageCookie, userPageIDString);

            }
            else
            {
                // unauthenticated user with no cookie
                // set the default cookie
                CookieHelper.SetPersistentCookie(userPageCookie, defaultUserPageCookie);

            }

            Response.Redirect(SiteRoot + "/MyPage.aspx?c=t", true);

        }

        private void BindUserMenu()
        {
            
            if (currentUser != null)
            {
                DataTable dataTable = UserPage.GetUserPageMenu(currentUser.UserGuid);
                countOfUserPages = dataTable.Rows.Count;
                if (countOfUserPages == 0)
                {
                   
                    String userPageIDString = UserPage.GetDefaultPagePath(
                        currentUser.UserGuid,
                        siteSettings,
                        Resource.MyPageDefaultUserPageName,
                        defaultUserPageCookie);

                    CookieHelper.SetPersistentCookie(userPageCookie, userPageIDString);

                    dataTable = UserPage.GetUserPageMenu(currentUser.UserGuid);

                }

                this.rptUserPageMenu.DataSource = dataTable;
                this.rptUserPageMenu.DataBind();
  
            }

        }

        protected void rptUserPageMenu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            this.pnlAddPage.Visible = false;
            this.pnlChangeName.Visible = false;

            String argString = e.CommandArgument.ToString();
            Guid userPageID = Guid.Empty;
            if (argString.Length == 36)
            {
                userPageID = new Guid(argString);
                CurrentUserPageId = userPageID;
            }
            switch (e.CommandName)
            {
                case "selectpage":
                    // CPersonalizationProvider will adjust the path variable
                    // based on cookie to retrieve personalizarion
                    // for the correct user page
                    if (argString.Length > 0)
                    {
                        CookieHelper.SetPersistentCookie(userPageCookie, argString);
                    }
                    WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
                    break;

                case "changename":
                    if (userPageID != Guid.Empty)
                    {
                        this.pnlChangeName.Visible = true;
                        UserPage userPage = new UserPage(userPageID);
                        this.txtCurrentPageName.Text = userPage.PageName;
                    }
                    
                    break;

                case "moveright":
                    if (userPageID != Guid.Empty)
                    {
                        UserPage userPage = new UserPage(userPageID);
                        userPage.MoveDown();

                    }
                    WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
                    break;

                case "moveleft":
                    if (userPageID != Guid.Empty)
                    {
                        UserPage userPage = new UserPage(userPageID);
                        userPage.MoveUp();

                    }
                    WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
                    break;

                case "remove":
                    if (userPageID != Guid.Empty)
                    {
                        UserPage.DeleteUserPage(userPageID);
                    }
                    WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
                    break;

                case "addpage":
                    this.pnlAddPage.Visible = true;

                    break;


            }

        }

        protected void rptUserPageMenu_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item)||(e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (e.Item.ItemIndex > 0)
                {
                    CanMoveLeft = true;
                    CanDeletePage = true;
                }

                if (e.Item.ItemIndex == (this.countOfUserPages - 1))
                {
                    CanMoveRight = false;
                }

                
            }

        }


       
        protected void btnNewPage_Click(object sender, EventArgs e)
        {
            if (currentUser != null)
            {
                UserPage userPage = new UserPage();
                userPage.UserGuid = currentUser.UserGuid;
                userPage.SiteId = siteSettings.SiteId;
                userPage.SiteGuid = siteSettings.SiteGuid;
                userPage.PageName = this.txtNewPage.Text;
                userPage.PagePath = Guid.NewGuid().ToString();
                userPage.PageOrder = 3;
                userPage.Save();
            }

            WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
           
        }

        protected void btnCancelAddPage_Click(object sender, EventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
        }

        
        protected void btnChangeName_Click(object sender, EventArgs e)
        {
            if (this.CurrentUserPageId != Guid.Empty)
            {
                UserPage userPage = new UserPage(this.CurrentUserPageId);
                userPage.PageName = this.txtCurrentPageName.Text;
                userPage.Save();

            }

            WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
        }

        protected void btnCancelChangeName_Click(object sender, EventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + Request.RawUrl);
        }


        private void AddWebPart()
        {
            String webPartIDString = Request.Params.Get("addpart");
            if (webPartIDString.Length == 36)
            {
                Guid webPartID = new Guid(webPartIDString);

                WebPartContent webPartContent = new WebPartContent(webPartID);
                if ((webPartContent.WebPartId != Guid.Empty)&&(webPartContent.AvailableForMyPage))
                {
                    if (HttpContext.Current != null)
                    {
                        String path = HttpContext.Current.Server.MapPath("~/bin")
                            + Path.DirectorySeparatorChar + webPartContent.AssemblyName + ".dll";
                        Assembly assembly = Assembly.LoadFrom(path);
                        Type type = assembly.GetType(webPartContent.ClassName, true, true);
                        WebPart webPart = Activator.CreateInstance(type) as WebPart;
                        if (webPart != null)
                        {
                            WebPartContent.UpdateCountOfUseOnMyPage(webPartContent.WebPartId, 1);
                            webPart.ID = Guid.NewGuid().ToString();

                            CWebPartManager CManager = (CWebPartManager)this.WebPartManager1;
                            CManager.AddWebPart(webPart, this.CenterWebPartZone, 0);
                            CManager.SetDirty();
                        }
                    }

                }

            }
        }


        private void AddModulePart()
        {
            String moduleIDString = Request.Params.Get("addmpart");
            Module module = new Module(int.Parse(moduleIDString));
            //TODO: we could support filtering by module.ViewRoles
            if (module.AvailableForMyPage)
            {
                SiteModuleControl siteModule = Page.LoadControl("~/" + module.ControlSource) as SiteModuleControl;
                if (siteModule != null)
                {
                    siteModule.SiteId = siteSettings.SiteId;
                    siteModule.ID = "module" + module.ModuleId.ToString();

                    siteModule.ModuleConfiguration = module;
                    siteModule.RenderInWebPartMode = true;

                    WebPart webPart = WebPartManager1.CreateWebPart(siteModule);

                    siteModule.ModuleId = module.ModuleId;
                    Module.UpdateCountOfUseOnMyPage(module.ModuleId, 1);

                    CWebPartManager CManager = (CWebPartManager)this.WebPartManager1;
                    CManager.AddWebPart(webPart, this.CenterWebPartZone, 0);
                    CManager.SetDirty();
                }
            }
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MyPageLink);

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionMyPageFormat, siteSettings.SiteName);

           
            cmdCatalogView.Visible = isAutheticated;
            cmdPersonalizationModeToggle.Visible = (isAdmin || isSiteEditor);
            cmdCatalogView.ImageUrl = ImageSiteRoot + "/Data/SiteImages/add.png";
            cmdCatalogView.AlternateText = Resource.WebPartManagerCatalogTooltip;
            cmdCatalogView.ToolTip = Resource.WebPartManagerCatalogTooltip;

            cmdResetPersonalization.Visible = isAutheticated;
            cmdResetPersonalization.ImageUrl = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/trash.gif";
            cmdResetPersonalization.AlternateText = Resource.WebPartManagerResetButton;
            cmdResetPersonalization.ToolTip = Resource.WebPartManagerResetButton;
            
            //cmdPersonalizationModeToggle.AlternateText
            //    = Resource.MyPageToggleScopeLabel;

            WebPartManager1.DeleteWarning = Resource.MyPageDeleteWarning;
            
            LeftWebPartZone.CloseVerb.Text = Resource.WebPartCloseVerbText;
            LeftWebPartZone.CloseVerb.Description = Resource.WebPartCloseVerbDescription;
            LeftWebPartZone.CloseVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/close.png";
            LeftWebPartZone.DeleteVerb.Text = Resource.WebPartDeleteVerbText;
            LeftWebPartZone.DeleteVerb.Description = Resource.WebPartDeleteVerbDescription;
            LeftWebPartZone.DeleteVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/del.png";
            LeftWebPartZone.MinimizeVerb.Text = Resource.WebPartMinimizeVerbText;
            LeftWebPartZone.MinimizeVerb.Description = Resource.WebPartMinimizeVerbDescription;
            LeftWebPartZone.MinimizeVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/min.png";
            LeftWebPartZone.RestoreVerb.Text = Resource.WebPartRestoreVerbText;
            LeftWebPartZone.RestoreVerb.Description = Resource.WebPartRestoreVerbDescription;
            LeftWebPartZone.RestoreVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/max.png";
            LeftWebPartZone.EditVerb.Text = Resource.WebPartEditVerbText;
            LeftWebPartZone.EditVerb.Description = Resource.WebPartEditVerbDescription;
            LeftWebPartZone.EditVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/editsettings.png";

            LeftWebPartZone.HelpVerb.Text = Resource.WebPartHelpVerbText;
            LeftWebPartZone.HelpVerb.Description = Resource.WebPartHelpVerbDescription;
            LeftWebPartZone.ExportVerb.Text = Resource.WebPartExportVerbText;
            LeftWebPartZone.ExportVerb.Description = Resource.WebPartExportVerbDescription;
            LeftWebPartZone.EditVerb.Text = Resource.WebPartEditVerbText;
            LeftWebPartZone.EditVerb.Description = Resource.WebPartEditVerbDescription;
            LeftWebPartZone.TitleBarVerbButtonType = ButtonType.Image;
            LeftWebPartZone.EmptyZoneText = Resource.WebPartEmptyZoneText;

            CenterWebPartZone.CloseVerb.Text = Resource.WebPartCloseVerbText;
            CenterWebPartZone.CloseVerb.Description = Resource.WebPartCloseVerbDescription;
            CenterWebPartZone.CloseVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/close.png";
            CenterWebPartZone.DeleteVerb.Text = Resource.WebPartDeleteVerbText;
            CenterWebPartZone.DeleteVerb.Description = Resource.WebPartDeleteVerbDescription;
            CenterWebPartZone.DeleteVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/del.png";
            CenterWebPartZone.MinimizeVerb.Text = Resource.WebPartMinimizeVerbText;
            CenterWebPartZone.MinimizeVerb.Description = Resource.WebPartMinimizeVerbDescription;
            CenterWebPartZone.MinimizeVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/min.png";
            CenterWebPartZone.RestoreVerb.Text = Resource.WebPartRestoreVerbText;
            CenterWebPartZone.RestoreVerb.Description = Resource.WebPartRestoreVerbDescription;
            CenterWebPartZone.RestoreVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/max.png";
            CenterWebPartZone.EditVerb.Text = Resource.WebPartEditVerbText;
            CenterWebPartZone.EditVerb.Description = Resource.WebPartEditVerbDescription;
            CenterWebPartZone.EditVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/editsettings.png";
            CenterWebPartZone.HelpVerb.Text = Resource.WebPartHelpVerbText;
            CenterWebPartZone.HelpVerb.Description = Resource.WebPartHelpVerbDescription;
            CenterWebPartZone.ExportVerb.Text = Resource.WebPartExportVerbText;
            CenterWebPartZone.ExportVerb.Description = Resource.WebPartExportVerbDescription;
            CenterWebPartZone.EditVerb.Text = Resource.WebPartEditVerbText;
            CenterWebPartZone.EditVerb.Description = Resource.WebPartEditVerbDescription;
            CenterWebPartZone.TitleBarVerbButtonType = ButtonType.Image;
            CenterWebPartZone.EmptyZoneText = Resource.WebPartEmptyZoneText;

            RightWebPartZone.CloseVerb.Text = Resource.WebPartCloseVerbText;
            RightWebPartZone.CloseVerb.Description = Resource.WebPartCloseVerbDescription;
            RightWebPartZone.CloseVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/close.png";
            RightWebPartZone.DeleteVerb.Text = Resource.WebPartDeleteVerbText;
            RightWebPartZone.DeleteVerb.Description = Resource.WebPartDeleteVerbDescription;
            RightWebPartZone.DeleteVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/del.png";
            RightWebPartZone.MinimizeVerb.Text = Resource.WebPartMinimizeVerbText;
            RightWebPartZone.MinimizeVerb.Description = Resource.WebPartMinimizeVerbDescription;
            RightWebPartZone.MinimizeVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/min.png";
            RightWebPartZone.RestoreVerb.Text = Resource.WebPartRestoreVerbText;
            RightWebPartZone.RestoreVerb.Description = Resource.WebPartRestoreVerbDescription;
            RightWebPartZone.RestoreVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/max.png";
            RightWebPartZone.EditVerb.Text = Resource.WebPartEditVerbText;
            RightWebPartZone.EditVerb.Description = Resource.WebPartEditVerbDescription;
            RightWebPartZone.EditVerb.ImageUrl = ImageSiteRoot + "/Data/SiteImages/editsettings.png";
            RightWebPartZone.HelpVerb.Text = Resource.WebPartHelpVerbText;
            RightWebPartZone.HelpVerb.Description = Resource.WebPartHelpVerbDescription;
            RightWebPartZone.ExportVerb.Text = Resource.WebPartExportVerbText;
            RightWebPartZone.ExportVerb.Description = Resource.WebPartExportVerbDescription;
            RightWebPartZone.EditVerb.Text = Resource.WebPartEditVerbText;
            RightWebPartZone.EditVerb.Description = Resource.WebPartEditVerbDescription;
            RightWebPartZone.TitleBarVerbButtonType = ButtonType.Image;
            RightWebPartZone.EmptyZoneText = Resource.WebPartEmptyZoneText;
            
            CatalogZone1.HeaderCloseVerb.Visible = false;
            CatalogZone1.CloseVerb.Visible = false;

            CatalogZone1.AddVerb.Text = Resource.WebPartAddVerbText;
            CatalogZone1.AddVerb.Description = Resource.WebPartAddVerbDescription;
            CatalogZone1.EmptyZoneText = Resource.WebPartEmptyCatalogZoneText;
            CatalogZone1.InstructionText = String.Empty;
            //CatalogZone1.SelectTargetZoneText = 

            EditorZone1.HeaderText = Resource.WebPartEditorHeaderText;
            EditorZone1.InstructionText = Resource.WebPartEditorInstructionsText;
 
            EditorZone1.ApplyVerb.Text = Resource.WebPartEditorApplyVerbText;
            EditorZone1.ApplyVerb.Description = Resource.WebPartEditorApplyVerbDescription;
            EditorZone1.CancelVerb.Text = Resource.WebPartEditorCancelVerbText;
            EditorZone1.CancelVerb.Description = Resource.WebPartEditorCancelVerbDescription;
            EditorZone1.HeaderCloseVerb.Text = Resource.WebPartEditorHeaderCloseVerbText;
            EditorZone1.HeaderCloseVerb.Description = Resource.WebPartEditorHeaderCloseVerbDescription;
            EditorZone1.OKVerb.Text = Resource.WebPartEditorOKVerbText;
            EditorZone1.OKVerb.Description = Resource.WebPartEditorOKVerbDescription;
            

            PageCatalogPart1 = (PageCatalogPart)CatalogZone1.FindControl("PageCatalogPart1");
            if (PageCatalogPart1 != null)
            {
                PageCatalogPart1.Description = Resource.WebPartPageCatalogTitle;
                PageCatalogPart1.Title = Resource.WebPartPageCatalogDescription;
            }

            
          
            if (WebPartManager1.Personalization.Scope == PersonalizationScope.User)
            {
                cmdPersonalizationModeToggle.ImageUrl = ImageSiteRoot
                    + "/Data/SiteImages/scope_user.png";

                cmdPersonalizationModeToggle.AlternateText = Resource.WebPartManagerToggleFromUserModeTooltip;
                cmdPersonalizationModeToggle.ToolTip = Resource.WebPartManagerToggleFromUserModeTooltip;
            }
            else
            {
                cmdPersonalizationModeToggle.ImageUrl = ImageSiteRoot
                    + "/Data/SiteImages/scope_shared.png";

                cmdPersonalizationModeToggle.AlternateText = Resource.WebPartManagerToggleToUserModeTooltip;
                cmdPersonalizationModeToggle.ToolTip = Resource.WebPartManagerToggleToUserModeTooltip;
            }


            btnNewPage.Text = Resource.MyPageNewPageButton;
            btnCancelAddPage.Text = Resource.MyPageCancelNewPageButton;
            btnChangeName.Text = Resource.MyPageRenamePageButton;
            btnCancelChangeName.Text = Resource.MyPageCancelRenamePageButton;
            

        }

        private void SetupCss()
        {
            // older skins have this
            //StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            //if (stylesheet != null)
            //{
            //    if (stylesheet.FindControl("mypage") == null)
            //    {
            //        Literal cssLink = new Literal();
            //        cssLink.ID = "mypage";
            //        cssLink.Text = "\n<link href='"
            //        + SiteUtils.GetSkinBaseUrl()
            //        + "mypage.css' type='text/css' rel='stylesheet' media='screen' />";

            //        stylesheet.Controls.Add(cssLink);
            //    }
            //}

            Control head = Page.Master.FindControl("Head1");
            if (head != null)
            {
                try
                {
                    if (head.FindControl("mypage") == null)
                    {
                        Literal cssLink = new Literal();
                        cssLink.ID = "mypage";
                        cssLink.Text = "\n<link href='"
                        + SiteUtils.GetSkinBaseUrl(this)
                        + "mypage.css' type='text/css' rel='stylesheet' media='screen' />";

                        head.Controls.Add(cssLink);
                    }
                }
                catch (HttpException) { }
            }

            
            
        }

        protected String GetCssClass(String pagePath)
        {
            String userPageCookieValue = String.Empty;
            if (CookieHelper.CookieExists(userPageCookie))
            {
                userPageCookieValue = CookieHelper.GetCookieValue(userPageCookie);
            }

            if (userPageCookieValue == pagePath)
            {
                return "userpagemenu-Selected";

            }

            return "userpagemenu";

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            Panel p = Page.Master.FindControl("divLeft") as Panel;
            if (p != null) { p.CssClass = "left-mypage"; }

            p = Page.Master.FindControl("divRight") as Panel;
            if (p != null) { p.CssClass = "right-mypage"; }

            p = Page.Master.FindControl("divCenter") as Panel;
            if (p != null) { p.CssClass = "center-mypage"; }
        }


        private void RegisterScripts()
        {
            //Utility.RegisterScripts(Page);
            //Page.ClientScript.RegisterClientScriptInclude(
            //    GetType(), 
            //    GetType().ToString(), 
            //    Page.ResolveUrl("~/ClientScript/MyPageMenu.js"));
        }

        #region WebPartManager Panel Events

        void cmdResetPersonalization_Click(object sender, ImageClickEventArgs e)
        {
            WebPartManager1.Personalization.ResetPersonalizationState();
            if (WebPartManager1.Personalization.Scope == PersonalizationScope.User)
            {
                PersonalizationHelper.ResetPersonalizationBlob(
                    siteSettings, 
                    WebPartManager1, 
                    CurrentUserPageId.ToString(), 
                    Context.User.Identity.Name);
            }

            WebUtils.SetupRedirect(this, SiteRoot + "/MyPage.aspx");

        }

        void cmdPersonalizationModeToggle_Click(object sender, ImageClickEventArgs e)
        {
            
            WebPartManager1.Personalization.ToggleScope();

            if (WebPartManager1.Personalization.Scope == PersonalizationScope.User)
            {
                cmdPersonalizationModeToggle.ImageUrl = ImageSiteRoot
                    + "/Data/SiteImages/scope_user.png";

                cmdPersonalizationModeToggle.AlternateText = Resource.WebPartManagerToggleFromUserModeTooltip;
                cmdPersonalizationModeToggle.ToolTip = Resource.WebPartManagerToggleFromUserModeTooltip;
            }
            else
            {
                cmdPersonalizationModeToggle.ImageUrl = ImageSiteRoot
                    + "/Data/SiteImages/scope_shared.png";

                cmdPersonalizationModeToggle.AlternateText = Resource.WebPartManagerToggleToUserModeTooltip;
                cmdPersonalizationModeToggle.ToolTip = Resource.WebPartManagerToggleToUserModeTooltip;
            }

           
        }

       
        void cmdCatalogView_Click(object sender, ImageClickEventArgs e)
        {
            if (WebPartManager1.DisplayMode == WebPartManager.CatalogDisplayMode)
            {
                // this gets us out of postback and into browse mode
                WebUtils.SetupRedirect(this, SiteRoot + "/MyPage.aspx");
            }
            else
            {
                WebPartManager1.DisplayMode = WebPartManager.CatalogDisplayMode;

                cmdCatalogView.ImageUrl = ImageSiteRoot
                    + "/Data/SiteImages/mypage_accept.png";

                cmdCatalogView.AlternateText = Resource.WebPartManagerCloseCatalogTooltip;
                cmdCatalogView.ToolTip = Resource.WebPartManagerCloseCatalogTooltip;

                LeftWebPartZone.HeaderText = Resource.MyPageLeftSideLabel;
                CenterWebPartZone.HeaderText = Resource.MyPageCenterLabel;
                RightWebPartZone.HeaderText = Resource.MyPageRightSideLabel;

               
            }
        }


        void WebPartManager1_WebPartMoved(object sender, WebPartEventArgs e)
        {

            WebUtils.SetupRedirect(this, SiteRoot + "/MyPage.aspx");
        }

        void WebPartManager1_WebPartDeleted(object sender, WebPartEventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/MyPage.aspx");
        }

        void WebPartManager1_WebPartClosed(object sender, WebPartEventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/MyPage.aspx");
        }

        void WebPartManager1_WebPartAdded(object sender, WebPartEventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/MyPage.aspx");
        }


        //protected void cmdBrowseView_Click(object sender, EventArgs e)
        //{
        //    WebPartManager1.DisplayMode = WebPartManager.BrowseDisplayMode;

        //}
        

        
        //protected void cmdConnectView_Click(object sender, EventArgs e)
        //{
        //    WebPartManager1.DisplayMode = WebPartManager.ConnectDisplayMode;

        //}

        

        
        #endregion

#endif

    }
}

