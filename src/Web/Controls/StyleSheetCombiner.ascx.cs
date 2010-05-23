
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// This control can (and should) be used instead of StyleSheet.ascx
    /// it will use a handler url, the handler will combine the css files in the order they are listed in
    /// the css.config file located in the skin folder
    /// It will also remove white space (and comments?)
    /// This can improve performance as measured using the YSlow Firefox plugin
    /// </summary>
    public partial class StyleSheetCombiner : UserControl
    {
        private bool loadSkinCss = true;
        private bool allowPageOverride = false;
        private string skinBaseUrl = string.Empty;
        private bool includeColorPickerCss = false;
        private string overrideSkinName = string.Empty;
        private string protocol = "http";

        private bool includeYuiLayout = false;
        private bool includeYuiReset = false;
        private bool useFullYuiSam = false;
        private bool includeYuiAccordion = false;
        private bool includeYuiTreeView = false;
        private string treeViewStyle = string.Empty;
        private bool includeYuiMenu = false;
        private bool includejQueryUI = true;
        private string jQueryUIThemeName = "base";
        private bool includeYuiTabs = false;

        private bool includejCrop = false;

        private bool useIconsForAdminLinks = true;
        /// <summary>
        /// this property is not used directly by this control but the base page and cms page detect ths setting and use it
        /// so it allows configuring this per skin
        /// </summary>
        public bool UseIconsForAdminLinks
        {
            get { return useIconsForAdminLinks; }
            set { useIconsForAdminLinks = value; }
        }

        private bool useTextLinksForFeatureSettings = true;
        /// <summary>
        /// this property is not used directly by this control but the base page and cms page detect ths setting and use it
        /// so it allows configuring this per skin
        /// </summary>
        public bool UseTextLinksForFeatureSettings
        {
            get { return useTextLinksForFeatureSettings; }
            set { useTextLinksForFeatureSettings = value; }
        }

        /// <summary>
        /// valid options are: base, black-tie, blitzer, cupertino, dot-luv, excite-bike, hot-sneaks, humanity, mint-choc,
        /// redmond, smoothness, south-street, start, swanky-purse, trontastic, ui-darkness, ui-lightness, vader
        /// </summary>
        public string JQueryUIThemeName
        {
            get { return jQueryUIThemeName; }
            set { jQueryUIThemeName = value; }
        }

        public bool IncludejQueryUI
        {
            get { return includejQueryUI; }
            set { includejQueryUI = value; }
        }

        public bool IncludejCrop
        {
            get { return includejCrop; }
            set { includejCrop = value; }
        }

        public bool AllowPageOverride
        {
            get { return allowPageOverride; }
            set { allowPageOverride = value; }
        }

        public bool LoadSkinCss
        {
            get { return loadSkinCss; }
            set { loadSkinCss = value; }
        }

        //public string CSSConfigFile
        //{
        //    get { return cssConfigFile; }
        //    set { cssConfigFile = value; }
        //}

        public string OverrideSkinName
        {
            get { return overrideSkinName; }
            set { overrideSkinName = value; }
        }

        public bool IncludeColorPickerCss
        {
            get { return includeColorPickerCss; }
            set { includeColorPickerCss = value; }
        }

        public bool IncludeYuiLayout
        {
            get { return includeYuiLayout; }
            set { includeYuiLayout = value; }
        }

        public bool IncludeYuiReset
        {
            get { return includeYuiReset; }
            set { includeYuiReset = value; }
        }

        public bool IncludeYuiTabs
        {
            get { return includeYuiTabs; }
            set { includeYuiTabs = value; }
        }

        public bool IncludeYuiAccordion
        {
            get { return includeYuiAccordion; }
            set { includeYuiAccordion = value; }
        }

        public bool IncludeYuiTreeView
        {
            get { return includeYuiTreeView; }
            set { includeYuiTreeView = value; }
        }

        /// <summary>
        /// valid options are empty for default, "folders", and "menu"
        /// </summary>
        public string TreeViewStyle
        {
            get { return treeViewStyle; }
            set { treeViewStyle = value; }
        }

        public bool IncludeYuiMenu
        {
            get { return includeYuiMenu; }
            set { includeYuiMenu = value; }
        }

        private bool includeTwitterCss = false;
        public bool IncludeTwitterCss
        {
            get { return includeTwitterCss; }
            set { includeTwitterCss = value; }
        }

        private bool includeGreyBoxCss = false;
        public bool IncludeGreyBoxCss
        {
            get { return includeGreyBoxCss; }
            set { includeGreyBoxCss = value; }
        }
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebConfigSettings.AlwaysLoadYuiTabs) { IncludeYuiTabs = true; }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current.Request.IsSecureConnection) { protocol = "https"; }

            SetupYuiCss();

            if (includejQueryUI) { SetupjQueryUICss(); }
            if (includejCrop) { SetupjCropCss(); }
            if (includeGreyBoxCss) { SetupGreyBox(); }
            if (includeTwitterCss) { SetupTwitter(); }


            if (!loadSkinCss) { return; }

            if (overrideSkinName.Length > 0)
            {
                skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(overrideSkinName);
                

            }
            else
            {
                skinBaseUrl = SiteUtils.GetSkinBaseUrl(allowPageOverride, Page);
                

            }

            if (WebConfigSettings.CombineCSS)
            {
                SetupCombinedCssUrl();
            }
            else
            {
                AddCssLinks();
            }

            
            
        }

        

        private void SetupCombinedCssUrl()
        {
            Literal cssLink = new Literal();

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string siteParam = "&amp;s=-1";
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null) { siteParam = string.Format("&amp;s={0}&amp;v={1}&amp;t={2}", siteSettings.SiteId.ToInvariantString(), Server.UrlEncode(DatabaseHelper.DBCodeVersion().ToString()), Assembly.GetExecutingAssembly().GetName().Version); }

            if (overrideSkinName.Length > 0)
            {
                cssLink.Text = string.Format("\n<link rel='stylesheet'  type='text/css' href='{0}/csshandler.ashx?skin={1}{2}' />", siteRoot, overrideSkinName, siteParam);
            }
            else
            {
                cssLink.Text = string.Format("\n<link rel='stylesheet' type='text/css' href='{0}/csshandler.ashx?skin={1}{2}' />", siteRoot, SiteUtils.GetSkinName(allowPageOverride, Page), siteParam);
            }

            this.Controls.Add(cssLink);

        }

        private void SetupGreyBox()
        {
            Literal cssLink = new Literal();
            cssLink.ID = "gb_styles";
            cssLink.Text = string.Format("\n<link rel='stylesheet' type='text/css' href='{0}' media='all' />", Page.ResolveUrl("~/ClientScript/greybox/gb_styles.css"));
            Controls.Add(cssLink);
        }

        private void SetupTwitter()
        {
            Literal cssLink = new Literal();
            cssLink.ID = "twittercsss";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='http://widgets.twimg.com/j/1/widget.css' />";
            Controls.Add(cssLink);

        }



        

        private void SetupjQueryUICss()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            string jQueryUIBasePath;
            if ((WebConfigSettings.UseGoogleCDN) || (ConfigurationManager.AppSettings["jQueryUIBasePath"] == null))
            {
                jQueryUIBasePath = protocol + "://ajax.googleapis.com/ajax/libs/jqueryui/" + WebConfigSettings.GoogleCDNjQueryUIVersion + "/";
            }
            else
            {
                jQueryUIBasePath = Page.ResolveUrl(ConfigurationManager.AppSettings["jQueryUIBasePath"]);
            }


            Literal cssLink = new Literal();
            cssLink.ID = "jqueryui-css";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
            + jQueryUIBasePath + "themes/" + jQueryUIThemeName + "/ui.all.css' />";
            this.Controls.Add(cssLink);


        }

        private void SetupjCropCss()
        {
            Literal cssLink = new Literal();
            cssLink.ID = "jcrop-css";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='" + Page.ResolveUrl("~/ClientScript/jcrop/jquery.Jcrop.css") + "' />";
            this.Controls.Add(cssLink);
        }

        private void AddCssLinks()
        {
            
            string configFilePath;
            if (overrideSkinName.Length > 0)
            {
                configFilePath = Server.MapPath(SiteUtils.DetermineSkinBaseUrl(SiteUtils.SanitizeSkinParam(overrideSkinName)) + "style.config");
            }
            else
            {
                configFilePath = Server.MapPath(SiteUtils.DetermineSkinBaseUrl(allowPageOverride, false, Page) + "style.config");
            }

            if (File.Exists(configFilePath))
            {
                using (XmlReader reader = new XmlTextReader(new StreamReader(configFilePath)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (("file" == reader.Name) && (reader.NodeType != XmlNodeType.EndElement))
                        {
                            string csswebconfigkey = reader.GetAttribute("csswebconfigkey");
                            string cssVPath = reader.GetAttribute("cssvpath");

                            if ((!string.IsNullOrEmpty(csswebconfigkey)))
                            {
                                if ((ConfigurationManager.AppSettings[csswebconfigkey] != null))
                                {
                                    AddCssLink(Page.ResolveUrl(ConfigurationManager.AppSettings[csswebconfigkey]));
                                }
                            }
                            else if ((!string.IsNullOrEmpty(cssVPath)))
                            {
                                AddCssLink(Page.ResolveUrl(cssVPath));
                            }
                            else
                            {
                                string cssFile = reader.ReadElementContentAsString();
                                AddCssLink(skinBaseUrl + cssFile);

                            }
                        }
                    }
                }
            }
            else
            {
                // style.config is missing
                AddCssLink(Page.ResolveUrl(ConfigurationManager.AppSettings["YUITabCss"]));
                AddCssLink(skinBaseUrl + "stylelayout.css");
                AddCssLink(skinBaseUrl + "stylecolors.css");
                AddCssLink(skinBaseUrl + "styleimages.css");
                AddCssLink(skinBaseUrl + "styleborders.css");
                AddCssLink(skinBaseUrl + "style-gridview.css");
                AddCssLink(skinBaseUrl + "styletext.css");
                AddCssLink(skinBaseUrl + "stylemenu.css");
                AddCssLink(skinBaseUrl + "styletreeview.css");
                AddCssLink(skinBaseUrl + "blogmodule.css");
                AddCssLink(skinBaseUrl + "forummodule.css");
                AddCssLink(skinBaseUrl + "rssmodule.css");
                AddCssLink(skinBaseUrl + "styleformwizard.css");
                AddCssLink(skinBaseUrl + "aspcalendar.css");
                AddCssLink(skinBaseUrl + "mpdatacalendar.css");
                AddCssLink(skinBaseUrl + "ibox.css");
            }

        }

        

        private void AddCssLink(string cssUrl)
        {
            Literal cssLink = new Literal();
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='" + cssUrl + "' />";
            this.Controls.Add(cssLink);

        }

        private void SetupYuiCss()
        {
            if (WebConfigSettings.DisableYUI) { return; }

            string yuiVersion = "2.6.0";

            if (ConfigurationManager.AppSettings["GoogleCDNYUIVersion"] != null)
            {
                yuiVersion = ConfigurationManager.AppSettings["GoogleCDNYUIVersion"];
            }

            string yuiBasePath;
            if ((WebConfigSettings.UseGoogleCDN) || (ConfigurationManager.AppSettings["YUIBasePath"] == null))
            {
                yuiBasePath = protocol + "://ajax.googleapis.com/ajax/libs/yui/" + yuiVersion + "/build/";
            }
            else
            {
                yuiBasePath = Page.ResolveUrl(ConfigurationManager.AppSettings["YUIBasePath"]);
            }

            string yuiAddOnBasePath = Page.ResolveUrl("~/ClientScript/yuiaddons/");
            if (ConfigurationManager.AppSettings["YUIAddOnsBasePath"] != null)
            {
                yuiAddOnBasePath = Page.ResolveUrl(ConfigurationManager.AppSettings["YUIAddOnsBasePath"]);
            }


            Literal cssLink;

            if (includeYuiLayout)
            {
                if (includeYuiReset)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-reset-fonts-grids";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "reset-fonts-grids/reset-fonts-grids.css' />";
                    this.Controls.Add(cssLink);
                }

                if (!useFullYuiSam)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-container";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "container/assets/skins/sam/container.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "yui-resize";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "resize/assets/skins/sam/resize.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "yui-layout";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "layout/assets/skins/sam/layout.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "yui-button";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "button/assets/skins/sam/button.css' />";
                    this.Controls.Add(cssLink);

                }

            }

            if (useFullYuiSam)
            {
                cssLink = new Literal();
                cssLink.ID = "yui-samskin";
                cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "assets/skins/sam/skin.css' />";
                this.Controls.Add(cssLink);

            }
            else
            {
                // always include at least tabs
                if (includeYuiTabs)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-tabview";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "tabview/assets/skins/sam/tabview.css' />";
                    this.Controls.Add(cssLink);
                }

                if (includeColorPickerCss)
                {

                    cssLink = new Literal();
                    cssLink.ID = "yui-colorpickercss";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "colorpicker/assets/skins/sam/colorpicker.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "dd-windowcss";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + Page.ResolveUrl("~/ClientScript/ddwindow/dhtmlwindow.css")
                         + "' />";
                    this.Controls.Add(cssLink);
                }

                if (includeYuiTreeView)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-treeview-css";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "treeview/assets/skins/sam/treeview.css' />";
                    this.Controls.Add(cssLink);

                    if (treeViewStyle == "folders")
                    {
                        cssLink = new Literal();
                        cssLink.ID = "yui-treeview-folder-css";
                        cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + Page.ResolveUrl("~/Data/style/treeviewcss/folders/tree.css") + "' />";
                        this.Controls.Add(cssLink);
                    }

                    if (treeViewStyle == "menu")
                    {
                        cssLink = new Literal();
                        cssLink.ID = "yui-treeview-menu-css";
                        cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + Page.ResolveUrl("~/Data/style/treeviewcss/menu/tree.css") + "' />";
                        this.Controls.Add(cssLink);
                    }

                }

                if (includeYuiMenu)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-menu-css";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "menu/assets/skins/sam/menu.css' />";
                    this.Controls.Add(cssLink);
                }

            }





            if (includeYuiAccordion)
            {
                cssLink = new Literal();
                cssLink.ID = "yui-accordioncss";
                cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                + yuiAddOnBasePath + "accordionview/assets/skins/sam/accordionview.css' />";
                this.Controls.Add(cssLink);
            }

        }
    }
}