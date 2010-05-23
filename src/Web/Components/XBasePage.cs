

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web
{
    
    public class CBasePage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CBasePage));

        protected SiteSettings siteSettings = null;
        private ContentPlaceHolder leftPane;
        private ContentPlaceHolder centerPane;
        private ContentPlaceHolder rightPane;
        private ContentPlaceHolder editPane;
        private ContentPlaceHolder altPane1;
        private ContentPlaceHolder altPane2;
        
        
        private ScriptManager scriptController;
        protected StyleSheet StyleSheetControl;
        private PageSettings currentPage = null;
        //private string skinBaseUrl = null;
        private string siteRoot = null;
        private string secureSiteRoot = null;
        private string imageSiteRoot = null;

        // changed default to false 2008-10-27
        private bool appendQueryStringToAction = false;
        private bool allowSkinOverride = false;
        private bool setMasterInBasePage = true;
        private bool forceShowLeft = false;
        private bool forceShowRight = false;
        private MetaContent metaContentControl = null;
        private ScriptLoader scriptLoader = null;
        private StyleSheetCombiner styleCombiner = null;
        private bool scriptLoaderFoundInMaster = false;


        private DropDownList ddlContentView = new DropDownList();
        private PageViewMode viewMode = PageViewMode.WorkInProgress;
        

        public ScriptLoader ScriptConfig
        {
            get
            {
                if (scriptLoader == null)
                {
                    scriptLoader = Master.FindControl("ScriptLoader1") as ScriptLoader;
                    if (scriptLoader != null){  scriptLoaderFoundInMaster = true; }
                }
                // older skins may not have the script loader so we can add it below in OnInit if scriptLoaderFoundInMaster is false
                if (scriptLoader == null) { scriptLoader = new ScriptLoader(); }

                return scriptLoader;
            }

        }

        public StyleSheetCombiner StyleCombiner
        {
            get
            {
                if (styleCombiner == null)
                {
                    styleCombiner = Master.FindControl("StyleSheetCombiner") as StyleSheetCombiner;
                    
                }
                return styleCombiner;
            }

        }


        public ContentPlaceHolder MPLeftPane
        {
            get { return leftPane; }
            set { leftPane = value; }
        }

        public ContentPlaceHolder MPContent
        {
            get { return centerPane; }
            set { centerPane = value; }
        }

        public ContentPlaceHolder MPRightPane
        {
            get { return rightPane; }
            set { rightPane = value; }
        }

        public ContentPlaceHolder AltPane1
        {
            get { return altPane1; }
            set { altPane1 = value; }
        }

        public ContentPlaceHolder AltPane2
        {
            get { return altPane2; }
            set { altPane2 = value; }
        }

        public ContentPlaceHolder MPPageEdit
        {
            get { return editPane; }
            set { editPane = value; }
        }

        public SiteSettings SiteInfo
        {
            get
            {
                EnsureSiteSettings();
                return siteSettings;
            }
        }

        public int SiteId
        {
            get {
                if (siteSettings != null)
                {
                    return siteSettings.SiteId;
                }
                return -1;
            }
        }

        public ScriptManager ScriptController
        {
            get 
            {
                if (scriptController == null)
                {
                    scriptController = (ScriptManager)Master.FindControl("ScriptManager1");
                }

                return scriptController; 
            }

        }

        public bool AppendQueryStringToAction
        {
            get { return appendQueryStringToAction; }
            set { appendQueryStringToAction = value; }
        }

        public bool AllowSkinOverride
        {
            get { return allowSkinOverride; }
            set { allowSkinOverride = value; }
            
        }

        protected bool SetMasterInBasePage
        {
            get { return setMasterInBasePage; }
            set { setMasterInBasePage = value; }
        }

        public bool IncludeColorPickerCss
        {
            get 
            {
                if (StyleCombiner != null)
                {
                    return StyleCombiner.IncludeColorPickerCss;
                }
                return false; 
            }
            set 
            {
                if (StyleCombiner != null)
                {
                    StyleCombiner.IncludeColorPickerCss = value;
                }
            }
        }

        public bool IncludeYuiTabsCss
        {
            get
            {
                if (StyleCombiner != null)
                {
                    return StyleCombiner.IncludeYuiTabs;
                }
                return false;
            }
            set
            {
                if (StyleCombiner != null)
                {
                    StyleCombiner.IncludeYuiTabs = value;
                }
            }
        }

        public bool IncludeYuiLayoutCss
        {
            get
            {
                if (StyleCombiner != null)
                {
                    return StyleCombiner.IncludeYuiLayout;
                }
                return false;
            }
            set
            {
                if (StyleCombiner != null)
                {
                    StyleCombiner.IncludeYuiLayout = value;
                }
            }
        }

        public string JQueryUIThemeName
        {
            get
            {
                if (StyleCombiner != null)
                {
                    return StyleCombiner.JQueryUIThemeName;
                }
                return string.Empty;
            }
            set
            {
                if (StyleCombiner != null)
                {
                    StyleCombiner.JQueryUIThemeName = value;
                }
            }
        }

        public bool UseIconsForAdminLinks
        {
            get
            {
                if (!WebConfigSettings.UseIconsForAdminLinks) { return false; } //previous default was true so if they changed it to false just return that

                if (StyleCombiner != null)
                {
                    return StyleCombiner.UseIconsForAdminLinks;
                }

                return true; //keeping the old default for backward compat
            }
        }

        public bool UseTextLinksForFeatureSettings
        {
            get
            {
                if (!WebConfigSettings.UseTextLinksForFeatureSettings) { return false; } //default is true so if they changed it to false just return that

                if (StyleCombiner != null)
                {
                    return StyleCombiner.UseTextLinksForFeatureSettings;
                }

                return true; 
            }
        }
        
        /// <summary>
        /// this property is deprecated, you should set Title = SiteUtils.FormatPageTitle, siteSettings, string topicTitle);
        /// </summary>
        public string PageTitle
        {
            get
            {
                PageTitle pageTitleControl = Master.FindControl("PageTitle1") as PageTitle;
                return Server.HtmlDecode(pageTitleControl == null ? this.Title : pageTitleControl.Title.Text);
            }
            set
            {
                PageTitle pageTitleControl = Master.FindControl("PageTitle1") as PageTitle;
                if (pageTitleControl == null)
                {
                    this.Title = value;
                }
                else
                {
                    pageTitleControl.Title.Text = Server.HtmlEncode(value);
                }
            }
        }

        private void EnsureMetaContentControl()
        {
            if(metaContentControl == null)
            metaContentControl = Master.FindControl("MetaContent") as MetaContent;

        }

        public string MetaKeywordCsv
        {
            get 
            {
                EnsureMetaContentControl();
                if (metaContentControl != null)
                {
                    return metaContentControl.KeywordCsv;
                }
                return string.Empty; 
            }
            set 
            {
                EnsureMetaContentControl();
                if (metaContentControl != null)
                {
                    metaContentControl.KeywordCsv = value;
                }
            }
        }

        public string MetaDescription
        {
            get
            {
                EnsureMetaContentControl();
                if (metaContentControl != null)
                {
                    return metaContentControl.Description;
                }
                return string.Empty;
            }
            set
            {
                EnsureMetaContentControl();
                if (metaContentControl != null)
                {
                    metaContentControl.Description = value;
                }
            }
        }

        
        public void AddMetaKeword(string keyword)
        {
            EnsureMetaContentControl();
            if (metaContentControl != null)
            {
                metaContentControl.AddKeword(keyword);
            }
        }

        public string AdditionalMetaMarkup
        {
            get
            {
                EnsureMetaContentControl();
                if (metaContentControl != null)
                {
                    return metaContentControl.AdditionalMetaMarkup;
                }
                return string.Empty;
            }
            set
            {
                EnsureMetaContentControl();
                if (metaContentControl != null)
                {
                    metaContentControl.AdditionalMetaMarkup = value;
                }
            }
        }

        
        public PageSettings CurrentPage
        {
            get
            {
                if (currentPage == null) currentPage = CacheHelper.GetCurrentPage();
                return currentPage;
            }
        }

        public bool UserCanViewPage()
        {
            if (CurrentPage == null) return false;
            if (WebUser.IsInRoles(CurrentPage.AuthorizedRoles)) return true;

            return false;

        }

        public bool UserCanViewPage(int moduleId)
        {
            if (CurrentPage == null) return false;
            if (!CurrentPage.ContainsModule(moduleId)) return false;
            if (WebUser.IsInRoles(CurrentPage.AuthorizedRoles)) return true;

            return false;

        }


        public bool UserCanEditPage()
        {
            if (CurrentPage == null) return false;
            if(WebUser.IsInRoles(CurrentPage.EditRoles))return true;

            return false;

        }

        public Module GetModule(int moduleId)
        {
            if (CurrentPage == null) { return null; }

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId) { return m; }
            }

            return null;
        }

        /// <summary>
        /// Returns true if the module exists on the page and the user has permission to edit the page or the module.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool UserCanEditModule(int moduleId)
        {
            if(!Request.IsAuthenticated)return false;

            if (WebUser.IsAdminOrContentAdmin) return true;

            if (SiteUtils.UserIsSiteEditor()) { return true; }

            if (CurrentPage == null) return false;

            bool moduleFoundOnPage = false;
            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId) moduleFoundOnPage = true;
            }

            if (!moduleFoundOnPage) return false;

            if (WebUser.IsInRoles(CurrentPage.EditRoles)) return true;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return false;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId)
                {
                    if (m.EditUserId == currentUser.UserId) return true;
                    if (WebUser.IsInRoles(m.AuthorizedEditRoles)) return true;
                }
            }

            return false;

        }

        public bool UserCanOnlyEditModuleAsDraft(int moduleId)
        {
            if (!Request.IsAuthenticated) return false;

            if (WebUser.IsAdminOrContentAdmin) return false;

            if (SiteUtils.UserIsSiteEditor()) { return false; }

            if (!WebConfigSettings.EnableContentWorkflow) { return false; }
            if (siteSettings == null) { return false; }
            if (!siteSettings.EnableContentWorkflow) { return false; }

            if (CurrentPage == null) return false;

            bool moduleFoundOnPage = false;
            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId) moduleFoundOnPage = true;
            }

            if (!moduleFoundOnPage) return false;

            if (WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles)) return true;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return false;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId)
                {
                    if (WebUser.IsInRoles(m.DraftEditRoles)) return true;
                }
            }

            return false;

        }

        public string GetModuleTitle(int moduleId)
        {
            string title = string.Empty;

            if (CurrentPage == null) return title;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId) title = m.ModuleTitle;
            }

            return title;

        }

        


        public bool ContainsPlaceHolder(string placeHoderId)
        {
            
            if (Master.FindControl(placeHoderId) != null) return true;


            return false;


        }

        public String SiteRoot
        {
            get
            {
                if ((siteSettings != null)&&(siteSettings.SiteFolderName.Length > 0))
                {
                    return siteSettings.SiteRoot;
                }
                if (siteRoot == null) siteRoot = WebUtils.GetSiteRoot();
                return siteRoot;
            }
        }

        public String SecureSiteRoot
        {
            get
            {
                if (!SiteUtils.SslIsAvailable()) return SiteRoot;
                if (secureSiteRoot == null) secureSiteRoot = WebUtils.GetSecureSiteRoot();
                return secureSiteRoot;
            }
        }

        public String ImageSiteRoot
        {
            get
            {
                if (imageSiteRoot == null) imageSiteRoot = WebUtils.GetSiteRoot();
                return imageSiteRoot;
            }
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (
                (Request.IsAuthenticated)
                && (WebConfigSettings.ForceSingleSessionPerUser)
            )
            {
                ForceSingleSession();
            }
        }


        override protected void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            bool isDesignTime = false;
            try
            {
                isDesignTime = this.DesignMode;
            }
            catch (NotImplementedException)
            {
                //mono needed hack as this error is raised
            }

            if (!isDesignTime && HttpContext.Current != null)
            {
                try
                {
                    if (WebConfigSettings.AllowForcingPreferredHostName)
                    {
                        EnsureSiteSettings();
                        if (
                            (siteSettings != null)
                            && (siteSettings.PreferredHostName.Length > 0)
                            )
                        {
                            string requestedHostName = WebUtils.GetHostName();
                            if (siteSettings.PreferredHostName != requestedHostName)
                            {

                                Response.Redirect("http://" + siteSettings.PreferredHostName, true);
                                return;

                            }
                        }
                        
                    }

                    SetupMasterPage();

                }
                catch (HttpException ex)
                {
                    log.Error("Error setting master page. Will try setting to default skin."
                        + CultureInfo.CurrentCulture.ToString() + " - " + SiteUtils.GetIP4Address(), ex);

                    SetupFailsafeMasterPage();
                }
            }
            

            if (setMasterInBasePage)
            {
                //StyleSheetControl = (StyleSheet)Master.FindControl("StyleSheet");
                //if (StyleSheetControl != null)
                //{
                //    StyleSheetControl.LiteralStyleSheetLink.Text
                //        = SiteUtils.GetStyleSheetLinks(allowSkinOverride);
                //}

                StyleSheetCombiner styleCombiner = (StyleSheetCombiner)Master.FindControl("StyleSheetCombiner");
                if (styleCombiner != null) { styleCombiner.AllowPageOverride = allowSkinOverride; }
            }

        }


        protected void EnsureSiteSettings()
        {
            if (siteSettings == null) siteSettings = CacheHelper.GetCurrentSiteSettings();

        }

        private void SetupMasterPage()
        {
            // default to not allow page override so the non content system pages
            // will still use the site default skin
            // even if the default content page has an alternate skin
            // this is overridden in Default.aspx.cs for content system
            // pages
            //bool allowPageOverride = false;
            
            //siteSettings = CacheHelper.GetCurrentSiteSettings();
            EnsureSiteSettings();
            if (
                (siteSettings != null)
                &&
                (siteSettings.SiteGuid != Guid.Empty)
                )
            {
                if (setMasterInBasePage)
                {
                    SiteUtils.SetMasterPage(this, siteSettings, allowSkinOverride);
                }

                
                if (WebConfigSettings.DisableASPThemes)
                {
                    this.Theme = string.Empty;
                }
                else
                {
                    this.Theme = "default";
                }
                

            }
            


            leftPane = (ContentPlaceHolder)Master.FindControl("leftContent");
            centerPane = (ContentPlaceHolder)Master.FindControl("mainContent");
            rightPane = (ContentPlaceHolder)Master.FindControl("rightContent");
            editPane = (ContentPlaceHolder)Master.FindControl("pageEditContent");
            altPane1 = (ContentPlaceHolder)Master.FindControl("altContent1");
            altPane2 = (ContentPlaceHolder)Master.FindControl("altContent2");


        }

        protected void SetupFailsafeMasterPage()
        {

            this.MasterPageFile = "~/App_MasterPages/layout.Master";

            MPLeftPane = (ContentPlaceHolder)Master.FindControl("leftContent");
            MPContent = (ContentPlaceHolder)Master.FindControl("mainContent");
            MPRightPane = (ContentPlaceHolder)Master.FindControl("rightContent");
            MPPageEdit = (ContentPlaceHolder)Master.FindControl("pageEditContent");


            AltPane1 = (ContentPlaceHolder)Master.FindControl("altContent1");
            AltPane2 = (ContentPlaceHolder)Master.FindControl("altContent2");

            StyleSheetControl = (StyleSheet)Master.FindControl("StyleSheet");



        }

        public void LoadSideContent(bool includeLeft, bool includeRight)
        {
            if (currentPage == null) { return; }

            if ((!includeLeft) && (!includeRight)) { return; }

            int leftModulesAdded = 0;
            int rightModulesAdded = 0;

            if (CurrentPage.Modules.Count > 0)
            {
                foreach (Module module in CurrentPage.Modules)
                {
                    if (ModuleIsVisible(module))
                    {

                        if ((includeLeft) && (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "leftpane")))
                        {
                            Control c = Page.LoadControl("~/" + module.ControlSource);
                            if (c == null) { continue; }

                            if (c is SiteModuleControl)
                            {
                                SiteModuleControl siteModule = (SiteModuleControl)c;
                                siteModule.SiteId = siteSettings.SiteId;
                                siteModule.ModuleConfiguration = module;
                            }

                            MPLeftPane.Controls.Add(c);
                            MPLeftPane.Visible = true;
                            MPLeftPane.Parent.Visible = true;
                            leftModulesAdded += 1;

                        }

                        if ((includeRight) && (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "rightpane")))
                        {
                            Control c = Page.LoadControl("~/" + module.ControlSource);
                            if (c == null) { continue; }

                            if (c is SiteModuleControl)
                            {
                                SiteModuleControl siteModule = (SiteModuleControl)c;
                                siteModule.SiteId = siteSettings.SiteId;
                                siteModule.ModuleConfiguration = module;
                            }

                            MPRightPane.Controls.Add(c);
                            MPRightPane.Visible = true;
                            MPRightPane.Parent.Visible = true;
                            rightModulesAdded += 1;

                        }

                    }
                }
            }

            forceShowLeft = (leftModulesAdded > 0);
            forceShowRight = (rightModulesAdded > 0);



        }

        private void SetupColumnCss(bool showLeft, bool showRight)
        {
            if ((!showLeft) && (!showRight)) { return; }

            Panel divLeft = (Panel)Master.FindControl("divLeft");
            Panel divCenter = (Panel)Master.FindControl("divCenter");
            Panel divRight = (Panel)Master.FindControl("divRight");

            if (divLeft == null) { return; }
            if (divRight == null) { return; }
            if (divCenter == null) { return; }

            divLeft.Visible = showLeft;
            divRight.Visible = showRight;

            if ((divLeft.Visible) && (!divRight.Visible))
            {
                divLeft.CssClass = "leftside left2column";
            }

            if ((divRight.Visible) && (!divLeft.Visible))
            {
                divRight.CssClass = "rightside right2column";
            }

            divCenter.CssClass =
                divLeft.Visible
                    ? (divRight.Visible ? "center-rightandleftmargins" : "center-leftmargin")
                    : (divRight.Visible ? "center-rightmargin" : "center-nomargins");

        }

        protected bool ModuleIsVisible(Module module)
        {
            if ((module.HideFromAuthenticated) && (Request.IsAuthenticated)) { return false; }
            if ((module.HideFromUnauthenticated) && (!Request.IsAuthenticated)) { return false; }

            return true;

        }

        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // a litle extra protection against CSRF attacks in forms
            // try catch added because of this post http://www.vivasky.com/Groups/Thread.aspx?thread=2819&mid=34&pageid=5&ItemID=2&pagenumber=1#post11883
            
            try
            {
                if ((Session != null) && (!string.IsNullOrEmpty(Session.SessionID)))
                {
                    ViewStateUserKey = Session.SessionID;
                }
            }
            catch (HttpException)
            { }

            if ((siteSettings != null) &&(siteSettings.MetaProfile.Length > 0) && (Page.Header != null))
            {
                Page.Header.Attributes.Add("profile", siteSettings.MetaProfile);
            }

            SetupAdminLinks();

            
            if (WebConfigSettings.WebSnaprKey.Length > 0)
            {
               
                ScriptConfig.WebSnaprKey = WebConfigSettings.WebSnaprKey;
            }
            else
            {
                if (SiteInfo != null) { ScriptConfig.WebSnaprKey = SiteInfo.WebSnaprKey; }
            }

            // older skins may not have it included in layout.master so we load it here
            if ((ScriptConfig != null)&&(!scriptLoaderFoundInMaster)) { this.Controls.Add(scriptLoader); }

            
             //this keeps the form action correct during ajax postbacks
            string formActionScript = "<script type=\"text/javascript\">"
                + "Sys.Application.add_load(function() { var form = Sys.WebForms.PageRequestManager.getInstance()._form; form._initialAction = form.action = window.location.href; }); "
                + "</script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "formactionset", formActionScript);

            

        }

        private void SetupAdminLinks()
        {

            // 2010-01-04 made it possible to add these links directly in layout.master so they can be arranged and styled easier
            if (Page.Master.FindControl("lnkAdminMenu") == null)
            {
                this.MPPageEdit.Controls.Add(new AdminMenuLink());
            }

            if (Page.Master.FindControl("lnkFileManager") == null)
            {
                this.MPPageEdit.Controls.Add(new FileManagerLink());
            }

            if (Page.Master.FindControl("lnkNewPage") == null)
            {
                this.MPPageEdit.Controls.Add(new NewPageLink());
            }

        }

        


        protected void HideViewSelector()
        {
            ddlContentView.Visible = false;
        }



        protected void SetupWorkflowControls()
        {
            if (
                (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow)
                    && (
                    (WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles)) || (WebUser.IsInRoles(CurrentPage.EditRoles))
                    )
                )
            {

                viewMode = GetUserViewMode();

                if ((WebConfigSettings.EnableContentWorkflow) && (siteSettings.EnableContentWorkflow))
                {
                    ddlContentView.EnableTheming = false;
                    ddlContentView.CssClass = "ddworkflow";
                    ddlContentView.Items.Add(new ListItem(Resource.ViewWorkInProgress, PageViewMode.WorkInProgress.ToString()));
                    ddlContentView.Items.Add(new ListItem(Resource.ViewLive, PageViewMode.Live.ToString()));

                    ListItem item = ddlContentView.Items.FindByValue(viewMode.ToString());
                    if (item != null)
                    {
                        ddlContentView.ClearSelection();
                        item.Selected = true;

                    }

                    ddlContentView.SelectedIndexChanged += new EventHandler(ddlContentView_SelectedIndexChanged);
                    ddlContentView.AutoPostBack = true;

                    if (UseIconsForAdminLinks)
                    {
                        this.MPPageEdit.Controls.Add(new LiteralControl("&nbsp;"));
                    }
                    this.MPPageEdit.Controls.Add(ddlContentView);

                }

            }

        }

        protected void ddlContentView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlContentView.SelectedIndex == -1)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            CookieHelper.SetCookie(GetViewModeCookieName(), ddlContentView.SelectedItem.Value, false);
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        protected PageViewMode GetUserViewMode()
        {
            PageViewMode viewMode = PageViewMode.WorkInProgress;

            // we let the query string value trump because we pass it in an url from the Admin workflow pages
            if (!string.IsNullOrEmpty(Request.QueryString["vm"]))
            {
                try
                {
                    viewMode = (PageViewMode)Enum.Parse(typeof(PageViewMode), Request.QueryString["vm"]);

                }
                catch (ArgumentException)
                { }

            }
            else
            {
                // if query string is not passed use the cookie
                string viewCookieValue = CookieHelper.GetCookieValue(GetViewModeCookieName());
                if (!string.IsNullOrEmpty(viewCookieValue))
                {
                    try
                    {
                        viewMode = (PageViewMode)Enum.Parse(typeof(PageViewMode), viewCookieValue);

                    }
                    catch (ArgumentException)
                    { }
                }

            }

            return viewMode;

        }

        private string GetViewModeCookieName()
        {
            string cookieName = "viewmode";
            if (siteSettings != null) { cookieName += siteSettings.SiteId.ToString(CultureInfo.InvariantCulture); }

            return cookieName;

        }


        /// <summary>
        /// Identifies whether to show Work in progress or live content. This value is switched via the dropdown at the top of the page
        /// when the site is in edit mode.
        /// Your Feature should read this and display content accordingly:
        /// </summary>
        public PageViewMode ViewMode
        {
            get { return viewMode; }

        }


        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SetupColumnCss(forceShowLeft, forceShowRight);

           
        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (!SiteUtils.UrlWasReWritten())
            {
                base.Render(writer);
                return;
            }

            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
            * 
            * Custom HtmlTextWriter to fix Form Action
            * Based on Jesse Ezell's "Fixing Microsoft's Bugs: URL Rewriting"
            * http://weblogs.asp.net/jezell/archive/2004/03/15/90045.aspx
            * 
            * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
            // this removes the action attribute from the form
            // so that it posts back correctly when using url re-writing

            string action = Request.RawUrl;

            // we need to append the query string to the form action
            // otherwise the params won't be available when the form posts back
            // example if editing an event ~/EventCalendarEdit.aspx??mid=4&pageid=3
            // if the form action is merely "EventCalendarEdit.aspx" we won't have the
            // mid and pageid params on postback. querystring is only available in get requests
            // unless the form action includes it, then its also available in post requests

            if (
                (appendQueryStringToAction)
                && (action.IndexOf("?") == -1)
                && (Request.QueryString.Count > 0)
                )
            {
                action += "?" + Request.QueryString.ToString();
            }

            if (writer.GetType() == typeof(HtmlTextWriter))
            {
                writer = new CynHtmlTextWriter(writer, action);
            }
            else if (writer.GetType() == typeof(Html32TextWriter))
            {
                writer = new CynHtml32TextWriter(writer, action);
            }

            base.Render(writer);

        }

        private void ForceSingleSession()
        {
            
            if (Context.User.Identity.AuthenticationType != "Forms") return;
           
            lock (HttpRuntime.Cache)
            {
                string requestUserSessionGUID;

                if (Session["requestGuid"] == null)
                {
                    requestUserSessionGUID = Context.User.Identity.Name + Guid.NewGuid().ToString();
                    Session["requestGuid"] = requestUserSessionGUID;
                    HttpRuntime.Cache.Insert("UserSessionID", requestUserSessionGUID);

                    return;
                }
                else
                {
                    requestUserSessionGUID = (string)Session["requestGuid"];
                    if ((string)HttpRuntime.Cache["UserSessionID"] == null)
                    {
                        HttpRuntime.Cache.Insert("UserSessionID", requestUserSessionGUID);
                    }

                    string cachedUserSessionID = (string)HttpRuntime.Cache["UserSessionID"];

                    if (cachedUserSessionID == requestUserSessionGUID)
                    {
                        
                        return;
                    }
                    else
                    {
                        // same user is logged in with another session, force this session to logout
                        FormsAuthentication.SignOut();
                        Session.Abandon();
                        return;
                    }
                }
            }
        }

        protected override void OnError(EventArgs e)
        {
            Exception lastError = Server.GetLastError();

            if ((lastError != null)&&(lastError is PathTooLongException))
            {
                //let it be handled elsewhere (global.asax.cs)
                return;
            }
            
            string exceptionUrl = string.Empty;
            string exceptionIpAddress = string.Empty;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
					exceptionUrl = String.Format("{0} - {1}", CultureInfo.CurrentCulture, HttpContext.Current.Request.RawUrl);
                    exceptionIpAddress = SiteUtils.GetIP4Address();

                }

            }

			if (lastError != null) log.Error(String.Format("{0}-{1}", exceptionIpAddress, exceptionUrl), lastError);

            

            int siteCount = DatabaseHelper.ExistingSiteCount();

            if (siteCount == 0)
            {
                Server.ClearError();

                log.Info("no sites or no database found in application error so try to redirect to Setup Page");

                try
                {
                    HttpContext.Current.Response.Clear();
					HttpContext.Current.Response.Redirect(String.Format("{0}/Setup/Default.aspx", WebUtils.GetSiteRoot()));
                }
                catch (HttpException) { }

            }

            bool upgradeNeeded = CSetup.UpgradeIsNeeded();
            
            if (upgradeNeeded)
            {
                try
                {
                    log.Info("detected need for upgrade so redirecting to setup");

                    Server.ClearError();
                    HttpContext.Current.Response.Clear();
					HttpContext.Current.Response.Redirect(String.Format("{0}/Setup/Default.aspx", WebUtils.GetSiteRoot()));
                }
                catch (HttpException) { }
            }

        }

        public void SuppressAllMenus()
        {
            Control menu = Master.FindControl("SiteMenu1");
            if (menu != null) { menu.Visible = false; }

            menu = Master.FindControl("PageMenu1");
            if (menu != null) { menu.Visible = false; }

            menu = Master.FindControl("PageMenu2");
            if (menu != null) { menu.Visible = false; }

            menu = Master.FindControl("PageMenu3");
            if (menu != null) { menu.Visible = false; }

        }

        public void SuppressMenuSelection()
        {
            SiteMenu siteMenu = (SiteMenu)Master.FindControl("SiteMenu1");
            if (siteMenu != null) siteMenu.SuppressPageSelection = true;
        }

        public void SuppressPageMenu()
        {
            PageMenuControl menu = (PageMenuControl)Master.FindControl("PageMenu1");
            if ((menu != null) && (menu.Direction == "Vertical")) menu.Visible = false;

            menu = (PageMenuControl)Master.FindControl("PageMenu2");
            if ((menu != null)&&(menu.Direction == "Vertical")) menu.Visible = false;

            menu = (PageMenuControl)Master.FindControl("PageMenu3");
            if ((menu != null) && (menu.Direction == "Vertical")) menu.Visible = false;

        }

        public void SuppressGoogleAds()
        {
            Control googleAdControl = Master.FindControl("pnlGoogle");
            if (googleAdControl != null) { googleAdControl.Visible = false; }

            googleAdControl = Master.FindControl("pnlGoogle1");
            if (googleAdControl != null) { googleAdControl.Visible = false; }

            googleAdControl = Master.FindControl("pnlGoogle2");
            if (googleAdControl != null) { googleAdControl.Visible = false; }



        }
        
    }
}
