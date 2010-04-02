

using System;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// 
    /// http://code.google.com/apis/ajaxlibs/
    /// 
    /// </summary>
    public class ScriptLoader : WebControl
    {
        
        private string protocol = "http";

        private bool includeJQuery = true;
        private bool includeYuiTabs = false;
        private bool includeYuiDataTable = false;
        private bool includeYuiDom = false;
        private bool includeYuiDataSource = false;
        private bool includeYuiJson = false;
        private bool includeYuiGet = false;
        private bool includeSlider = false;
        private bool includeColorPicker = false;
        private bool includeYuiLayout = false;
        private bool includeYuiAccordion = false;
        private bool includeYuiTreeView = false;
        private bool includeYuiMenu = false;

        private bool includejQueryUICore = true;
        private bool includejQueryAccordion = true;
        private bool includejQueryFileTree = false;

        private bool includeOomph = true;

        private bool includejQueryLayout = false;
        private bool includejQueryHoverIntent = false;
        private bool includejQueryMetaData = false;
        private bool includejQueryFlipText = false;
        private bool includejQueryExtruder = false;

        private string webSnaprKey = string.Empty;

        private bool includeYahooMediaPlayer = false;
        private bool includeClueTip = false;

        private bool includeSwfObject = false;

        private bool includeImpromtu = false;
        private bool includeQtFile = false;



        private bool includeGreyBox = false;

        private bool includeGoogleGeoLocator = false;
        private bool includeGoogleMaps = false;
        private bool includeGoogleSearch = false;

        #region Public Properties


        public bool IncludeGoogleGeoLocator
        {
            get { return includeGoogleGeoLocator; }
            set
            {
                includeGoogleGeoLocator = value;
                if (includeGoogleGeoLocator) { includeGoogleMaps = true; }
            }
        }

        public bool IncludeGoogleMaps
        {
            get { return includeGoogleMaps; }
            set { includeGoogleMaps = value; }
        }

        public bool IncludeGoogleSearch
        {
            get { return includeGoogleSearch; }
            set { includeGoogleSearch = value; }
        }

        public string WebSnaprKey
        {
            get { return webSnaprKey; }
            set { webSnaprKey = value; }
        }

        public bool IncludeClueTip
        {
            get { return includeClueTip; }
            set 
            { 
                includeClueTip = value;
                if (includeClueTip) { includeJQuery = true; }
            }
        }

        public bool IncludeOomph
        {
            get { return includeOomph; }
            set { includeOomph = value; }
        }

        public bool IncludeYahooMediaPlayer
        {
            get { return includeYahooMediaPlayer; }
            set { includeYahooMediaPlayer = value; }
        }

        public bool IncludeSwfObject
        {
            get { return includeSwfObject; }
            set { includeSwfObject = value; }
        }

        public bool IncludeJQuery
        {
            get { return includeJQuery; }
            set { includeJQuery = value; }
        }

        public bool IncludejQueryUICore
        {
            get { return includejQueryUICore; }
            set { includejQueryUICore = value; }
        }


        public bool IncludejQueryAccordion
        {
            get { return includejQueryAccordion; }
            set 
            { 
                includejQueryAccordion = value;
                if (includejQueryAccordion) { includejQueryUICore = true; }
            }
        }

        public bool IncludejQueryFileTree
        {
            get { return includejQueryFileTree; }
            set
            {
                includejQueryFileTree = value;
                if (includejQueryFileTree) { includeJQuery = true; }
            }
        }

        public bool IncludejQueryLayout
        {
            get { return includejQueryLayout; }
            set
            {
                includejQueryLayout = value;
                if (includejQueryLayout) { includeJQuery = true; includejQueryUICore = true; }
            }
        }

        public bool IncludeQtFile
        {
            get { return includeQtFile; }
            set { 
                includeQtFile = value;
                if (includeQtFile) { includeImpromtu = true; includeJQuery = true; includejQueryUICore = true; }
            }
        }

        public bool IncludeImpromtu
        {
            get { return includeImpromtu; }
            set
            {
                includeImpromtu = value;
                if (includeImpromtu) { includeJQuery = true; includejQueryUICore = true; }
            }
        }

        public bool IncludejQueryExtruder
        {
            get { return includejQueryExtruder; }
            set
            {
                includejQueryExtruder = value;
                if (includejQueryExtruder)
                {
                    includeJQuery = true;
                    includejQueryHoverIntent = true;
                    includejQueryMetaData = true;
                    includejQueryFlipText = true;
                }
            }
        }

        public bool IncludejQueryHoverIntent
        {
            get { return includejQueryHoverIntent; }
            set
            {
                includejQueryHoverIntent = value;
                if (includejQueryHoverIntent) { includeJQuery = true; }
            }
        }

        public bool IncludejQueryMetaData
        {
            get { return includejQueryMetaData; }
            set
            {
                includejQueryMetaData = value;
                if (includejQueryMetaData) { includeJQuery = true; }
            }
        }

        public bool IncludejQueryFlipText
        {
            get { return includejQueryFlipText; }
            set
            {
                includejQueryFlipText = value;
                if (includejQueryFlipText) { includeJQuery = true; }
            }
        }

        public bool IncludeGreyBox
        {
            get { return includeGreyBox; }
            set { includeGreyBox = value; }
        }

        public bool IncludeYuiLayout
        {
            get { return includeYuiLayout; }
            set 
            { 
                includeYuiLayout = value;
                if (includeYuiLayout) { includeYuiDom = true; }
            }
        }

        public bool IncludeColorPicker
        {
            get { return includeColorPicker; }
            set
            {
                includeColorPicker = value;
                if (includeColorPicker)
                {
                    includeYuiDom = true;
                    includeSlider = true;
                    
                }
            }
        }

        public bool IncludeYuiTreeView
        {
            get { return includeYuiTreeView; }
            set
            {
                includeYuiTreeView = value;
                if (includeYuiTreeView)
                {
                    includeYuiDom = true;

                }
            }
        }
        

        public bool IncludeYuiTabs
        {
            get { return includeYuiTabs; }
            set 
            { 
                includeYuiTabs = value;
                if (includeYuiTabs)
                {
                    includeYuiDom = true;
                    //includeYuiElement = true;
                    //includeYuiConnection = true;
                }
            }
        }

        public bool IncludeYuiDataTable
        {
            get { return includeYuiDataTable; }
            set 
            { 
                includeYuiDataTable = value;
                if (includeYuiDataTable)
                {
                    includeYuiDom = true;
                    includeYuiDataSource = true;
                    
                }
            }
        }

        public bool IncludeYuiAccordion
        {
            get { return includeYuiAccordion; }
            set 
            { 
                includeYuiAccordion = value;
                if (includeYuiAccordion) { includeYuiDom = true; }
            }
        }

        public bool IncludeYuiMenu
        {
            get { return includeYuiMenu; }
            set
            {
                includeYuiMenu = value;
                if (includeYuiMenu)
                {
                    includeYuiDom = true;

                }
            }
        }

        #endregion


        protected override void Render(HtmlTextWriter writer)
        {
            //base.RenderContents(writer);
            //no need to render anything just setup scripts and css
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (WebConfigSettings.AlwaysLoadYuiTabs) { IncludeYuiTabs = true; }
            
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return ; }
            if (HttpContext.Current.Request == null) { return ; }

            if (HttpContext.Current.Request.IsSecureConnection) { protocol = "https"; }

            SetupScripts();
        }

        private void SetupScripts()
        {
            SetupCynthiacombined();

            if (!WebConfigSettings.DisablejQuery) 
            { 
                SetupJQuery();
                if (!WebConfigSettings.DisablejQueryUI) { SetupJQueryUI(); }
                if (includejQueryFileTree) { SetupJQueryFileTree(); }
                //if (includeInterface) { SetupInterface(); }
            }
            
            if (!WebConfigSettings.DisableYUI) { SetupYui(); }

            if ((includeOomph)&&(!WebConfigSettings.DisableOomph))
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "oomph", "\n<script  src=\""
                    + Page.ResolveUrl("~/ClientScript/oomph/oomph.min.js") + "\" type=\"text/javascript\" ></script>");

            }

            if (!WebConfigSettings.DisableWebSnapr && (webSnaprKey.Length > 0)) { SetupWebSnapr(); }

            if (includeYahooMediaPlayer) { SetupYahooMediaPlayer(); }

            if (includeSwfObject) { SetupSwfObject(); }

            SetupBrowserSpecificScripts();

            if (includeGreyBox) { SetupGreyBox(); }

            SetupGoogleAjax();
           
        }

        

        #region jQuery

        private string GetJQueryBasePath()
        {
            
            if (WebConfigSettings.UseGoogleCDN)
            {
                return protocol + "://ajax.googleapis.com/ajax/libs/jquery/" + WebConfigSettings.GoogleCDNjQueryVersion + "/";
            }

            if (ConfigurationManager.AppSettings["jQueryBasePath"] != null)
            {
                string jqueryBasePath = ConfigurationManager.AppSettings["jQueryBasePath"];
                return Page.ResolveUrl(jqueryBasePath);

            }

            return string.Empty;
        }

        private void SetupJQuery()
        {

            string jqueryBasePath = GetJQueryBasePath();

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            "jquery", "\n<script src=\""
            + jqueryBasePath + "jquery.min.js" + "\" type=\"text/javascript\" ></script>");

            
        }

        //private void SetupInterface()
        //{
        //    //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
        //    //         "jqinterface", "\n<script src=\""
        //    //         + Page.ResolveUrl("~/ClientScript/jqCynthia/interface.js") + "\" type=\"text/javascript\"></script>");

        //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
        //             "iutil", "\n<script src=\""
        //             + Page.ResolveUrl("~/ClientScript/jqCynthia/iutil.js") + "\" type=\"text/javascript\"></script>");

        //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
        //             "fisheye", "\n<script src=\""
        //             + Page.ResolveUrl("~/ClientScript/jqCynthia/fisheye.js") + "\" type=\"text/javascript\"></script>");
        //}

        private void SetupJQueryFileTree()
        {

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqfiletree", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqueryFileTree/jqueryFileTree.js") + "\" type=\"text/javascript\"></script>");


        }



        private string GetJQueryUIBasePath()
        {
          
            if (WebConfigSettings.UseGoogleCDN)
            {
                return protocol + "://ajax.googleapis.com/ajax/libs/jqueryui/" + WebConfigSettings.GoogleCDNjQueryUIVersion + "/";
            }

            if (ConfigurationManager.AppSettings["jQueryUIBasePath"] != null)
            {
                string jqueryBasePath = ConfigurationManager.AppSettings["jQueryUIBasePath"];
                return Page.ResolveUrl(jqueryBasePath);

            }

            return string.Empty;
        }

        private void SetupJQueryUI()
        {

            string jqueryUIBasePath = GetJQueryUIBasePath();

            if (includejQueryUICore)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "jqueryui-core", "\n<script src=\""
                + jqueryUIBasePath + "jquery-ui.min.js" + "\" type=\"text/javascript\" ></script>");
            }

            if (includejQueryAccordion)
            {
                // this also includes jqueryui tabs
                string initAutoScript = " $('div.C-accordion').accordion(); $('div.C-accordion-nh').accordion({autoHeight: false});  $('div.C-tabs').tabs(); ";

                Page.ClientScript.RegisterStartupScript(typeof(Page),
                   "jui-init", "\n<script type=\"text/javascript\" >"
                   + initAutoScript + "</script>");
            }

            if (includeClueTip) { SetupClueTip(); }

            if (includeImpromtu)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "jqprompt", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqCynthia/jquery-impromptu.min.js") + "\" type=\"text/javascript\"></script>");
            }

            if (includeQtFile)
            {
                CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();
                
                bool loadedQtLangFile = false;
                if(defaultCulture.TwoLetterISOLanguageName != "en")
                {
                    if (File.Exists(HostingEnvironment.MapPath("~/ClientScript/jqCynthia/" + defaultCulture.TwoLetterISOLanguageName + ".qtfile.js")))
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "qtfilelocalize", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqCynthia/" + defaultCulture.TwoLetterISOLanguageName + ".qtfile.js") + "\" type=\"text/javascript\"></script>");

                        loadedQtLangFile = true;
                    }
                }

                if (!loadedQtLangFile)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "qtfilelocalize", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqCynthia/en.qtfile.js") + "\" type=\"text/javascript\"></script>");
                }


                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "qtfile", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqCynthia/cynthiaqtfile.js") + "\" type=\"text/javascript\"></script>");
            }

            if (includejQueryHoverIntent)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqhoverintent", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqCynthia/jquery.hoverIntent.min.js") + "\" type=\"text/javascript\"></script>");
            }

            if (includejQueryMetaData)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqmetadata", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqCynthia/jquery.metadata.js") + "\" type=\"text/javascript\"></script>");
            }

            if (includejQueryFlipText)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqfliptext", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqCynthia/jquery.mb.flipText.min.js") + "\" type=\"text/javascript\"></script>");
            }

            if (includejQueryExtruder)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqextruder", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqCynthia/cynthia-mbExtruder.js") + "\" type=\"text/javascript\"></script>");

            }

            if (includejQueryLayout)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqlayout", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqCynthia/jquery.layout.min.js") + "\" type=\"text/javascript\"></script>");

            }

        }

        private void SetupClueTip()
        {

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "cluetip", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/jqCynthia/jquery.cluetip.js") + "\" type=\"text/javascript\"></script>");

            string initAutoScript = "$(document).ready(function () { $('a.cluetiplink').cluetip({attribute:'href', topOffset:25, leftOffset:25}); }); ";

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                   "cluetip-init", "\n<script type=\"text/javascript\" >"
                   + initAutoScript + "</script>");


        }

        #endregion



        
        #region swfobject

        private string GetSwfObjectUrl()
        {
            string version = "2.2";
            if (ConfigurationManager.AppSettings["GoogleCDNSwfObjectVersion"] != null)
            {
                version = ConfigurationManager.AppSettings["GoogleCDNSwfObjectVersion"];
            }

            if (WebConfigSettings.UseGoogleCDN)
            {
                return protocol + "://ajax.googleapis.com/ajax/libs/swfobject/" + version + "/swfobject.js";
            }

            if (ConfigurationManager.AppSettings["SwfObjectUrl"] != null)
            {
                string surl = ConfigurationManager.AppSettings["SwfObjectUrl"];
                return Page.ResolveUrl(surl);

            }

            return string.Empty;
        }

        private void SetupSwfObject()
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (WebConfigSettings.DisableSwfObject) { return; }

            //http://ajax.googleapis.com/ajax/libs/swfobject/2.2/swfobject.js

            string swfUrl = GetSwfObjectUrl();
            if (swfUrl.Length > 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "swfobject", "\n<script src=\"" + swfUrl + "\" type=\"text/javascript\" ></script>");
            }


        }


        #endregion

        private void SetupWebSnapr()
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }

            // this script doesn't support https as far as I know
            if (HttpContext.Current.Request.IsSecureConnection) { return; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            "websnapr", "\n<script src=\"http://bubble.websnapr.com/"
            + webSnaprKey + "/swh/" + "\" type=\"text/javascript\" ></script>");

        }

        private void SetupYahooMediaPlayer()
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (HttpContext.Current.Request.IsSecureConnection) { return; } //https not supported

            //yahoo media player doesn't seem to work on localhost so usethe delicious one
            if (HttpContext.Current.Request.Url.ToString().Contains("localhost"))
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "dmedia", "\n<script type=\"text/javascript\" src=\"http://static.delicious.com/js/playtagger.js\"></script>\n");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "yahoomedia", "\n<script type=\"text/javascript\" src=\"http://mediaplayer.yahoo.com/js\"></script>\n");
            }

            //<script type="text/javascript" src="http://static.delicious.com/js/playtagger.js"></script>


        }

        private void SetupGreyBox()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "gbVar", "\n<script  type=\"text/javascript\">"
                    + "var GB_ROOT_DIR = '" + Page.ResolveUrl("~/ClientScript/greybox/") + "'; var GBCloseText = '" + Resource.CloseDialogButton + "';" + " </script>");

            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "GreyBoxJs", "\n<script  src=\""
            //        + Page.ResolveUrl(scriptBaseUrl + "gbcombined.js") + "\" type=\"text/javascript\" ></script>");

            //The commented version above is the preferred syntax, the uncommented one below is the deprecated version.
            //There is a reason we are using the deprecated version, greybox is registered also by NeatUpload using the deprecated
            //syntax, the reason they use the older syntax is because they also support .NET v1.1
            //We use the old syntax here for compatibility with NeatUpload so that it does not get registered more than once
            // on pages that use NeatUpload. Otherwise we would have to always modify our copy of NeatUpload.
            Page.RegisterClientScriptBlock("GreyBoxJs", "\n<script  src=\""
                    + Page.ResolveUrl("~/ClientScript/greybox/gbcombined.js") + "\" type=\"text/javascript\" ></script>");
        }

        private void SetupCynthiacombined()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "Cynthiacombined", "\n<script  src=\""
                    + Page.ResolveUrl(String.Format("~/ClientScript/Cynthiacombined{0}.js", WebConfigSettings.CynthiacombinedScriptVersion)) 
                    + "\" type=\"text/javascript\" ></script>");

        }


        private void SetupGoogleAjax()
        {
            if ((!includeGoogleMaps) && (!includeGoogleSearch)) { return; }
            string googleApiKey = SiteUtils.GetGmapApiKey();
            if (string.IsNullOrEmpty(googleApiKey)) { return; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "gajaxmain", String.Format("\n<script  src=\"{0}://www.google.com/jsapi?key={1}\" type=\"text/javascript\" ></script>", protocol, googleApiKey));

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">\n");

            if (includeGoogleMaps)
            {
                script.Append("google.load(\"maps\", \"2\");");
            }
            if (includeGoogleSearch)
            {
                script.Append("google.load(\"search\", \"1\");");
            }

            if ((includeGoogleGeoLocator) && (Page.Request.IsAuthenticated))
            {
                script.Append("\n function trackLocation() { ");
                script.Append("var location = google.loader.ClientLocation; ");
                // this method is in Cynthiacombined.js
                script.Append("if(location != null){ trackUserLocation(location); }");
                //script.Append("else{alert('location was null'); }");

                script.Append("}");

                script.Append("google.setOnLoadCallback(trackLocation);");
            }

            script.Append("\n</script>");


            Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "gloader",
                script.ToString());


        }



        private void SetupBrowserSpecificScripts()
        {
#if !MONO
            string loweredBrowser = string.Empty;

            if (HttpContext.Current.Request.UserAgent != null)
            {
                loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();
            }

            if (loweredBrowser.Contains("webkit"))
            {
                //this fixes some ajax updatepanel issues in webkit
                //http://forums.asp.net/p/1252014/2392110.aspx
                try
                {
                    ScriptReference scriptReference = new ScriptReference();
                    scriptReference.Path = Page.ResolveUrl("~/ClientScript/AjaxWebKitFix.js");
                    ScriptManager ajax = ScriptManager.GetCurrent(Page);
                    if (ajax != null)
                    {
                        ajax.Scripts.Add(scriptReference);
                    }
                }
                catch (TypeLoadException)
                { }// this can happen if SP1 is not installed for .NET 3.5
            }
#endif

        }


        #region YUI

        private string GetYuiBasePath()
        {
            string yuiVersion = "2.6.0";

            if (ConfigurationManager.AppSettings["GoogleCDNYUIVersion"] != null)
            {
                yuiVersion = ConfigurationManager.AppSettings["GoogleCDNYUIVersion"];
            }

            if (WebConfigSettings.UseGoogleCDN)
            {
                return String.Format("{0}://ajax.googleapis.com/ajax/libs/yui/{1}/build/", protocol, yuiVersion);
            }

            if (ConfigurationManager.AppSettings["YUIBasePath"] != null)
            {
                string yuiBasePath = ConfigurationManager.AppSettings["YUIBasePath"];
                return Page.ResolveUrl(yuiBasePath);
            }

            return string.Empty;
        }


        private void SetupYui()
        {

            string scriptBaseUrl = GetYuiBasePath();
            string yuiAddOnBaseUrl = Page.ResolveUrl("~/ClientScript/yuiaddons/");
            if (ConfigurationManager.AppSettings["YUIAddOnsBasePath"] != null)
            {
                yuiAddOnBaseUrl = Page.ResolveUrl(ConfigurationManager.AppSettings["YUIAddOnsBasePath"]);
            }

            if (includeYuiDom)
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-utilities", "\n<script src=\""
                    + scriptBaseUrl + "utilities/utilities.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeYuiLayout)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-container-js", "\n<script src=\""
                    + scriptBaseUrl + "container/container-min.js" + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-resize-js", "\n<script src=\""
                    + scriptBaseUrl + "resize/resize-min.js" + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-layout-js", "\n<script src=\""
                    + scriptBaseUrl + "layout/layout-min.js" + "\" type=\"text/javascript\"></script>");

            }

            if (includeSlider)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-slider", "\n<script src=\""
                    + scriptBaseUrl + "slider/slider-min.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeColorPicker)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-colorpicker", "\n<script src=\""
                    + scriptBaseUrl + "colorpicker/colorpicker-min.js" + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "dd-window", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/ddwindow/dhtmlwindow.js") + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "dd-colorpicker", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/ddcolorpicker.js") + "\" type=\"text/javascript\"></script>");
            }

            if (includeYuiDataSource)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-datasource", "\n<script src=\""
                    + scriptBaseUrl + "datasource/datasource-min.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeYuiJson)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-json", "\n<script src=\""
                    + scriptBaseUrl + "json/json-min.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeYuiGet)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-get", "\n<script src=\""
                    + scriptBaseUrl + "get/get-min.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeYuiDataTable)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-datatable", "\n<script src=\""
                    + scriptBaseUrl + "datatable/datatable-min.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeYuiTabs)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-tabview", "\n<script src=\""
                    + scriptBaseUrl + "tabview/tabview-min.js" + "\" type=\"text/javascript\"></script>");

                string initTabsScript = "$('div.yui-navset').each(function(n){var myTabs = new YAHOO.widget.TabView(this);})";

                Page.ClientScript.RegisterStartupScript(typeof(Page),
                   "yui-tabinit", "\n<script type=\"text/javascript\" >"
                   + initTabsScript + "</script>");
            }

            if (includeYuiAccordion)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-accordionview", "\n<script src=\""
                    + yuiAddOnBaseUrl + "accordionview/accordionview-min.js" + "\" type=\"text/javascript\"></script>");

            }

            if (includeYuiTreeView)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-treeview-js", "\n<script src=\""
                    + scriptBaseUrl + "treeview/treeview-min.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeYuiMenu)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-menu-js", "\n<script src=\""
                    + scriptBaseUrl + "menu/menu-min.js" + "\" type=\"text/javascript\"></script>");
            }

        }

        #endregion


    }
}
