<%@ Page Language="C#" ClassName="Index.aspx" Inherits="System.Web.UI.Page"   %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>
<%@ Import Namespace="mojoPortal.Web.Controls" %>
<%@ Import Namespace="mojoPortal.Web.Editor" %>
<%@ Import Namespace="mojoPortal.Net" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>

<script runat="server">
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2008-11-26
    /// Last Modified:			2008-12-06
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    private SiteSettings siteSettings = null;
    private string cultureCode = "en";
    private string layoutDirection = "ltr";
    private string siteRoot = string.Empty;
    private string siteName = string.Empty;
    private bool sslIsAvailable = false;
    private string userName = string.Empty;
    private bool allowPageView = false;
    private bool enableMyPage = false;
    


    protected void Page_Load(object sender, EventArgs e)
    {
        //SecurityHelper.DisableBrowserCache();
        
        if (WebConfigSettings.SslisAvailable)
        {
            WebUtils.ForceSsl();
            sslIsAvailable = true;
        }

        if (
                (WebConfigSettings.UseOfficeFeature)
                &&(WebConfigSettings.UseSilverlightSiteOffice)
                )
        {
            allowPageView = true;
        }

        allowPageView = true;
       
        if (!allowPageView)
        {
            SiteUtils.RedirectToAccessDeniedPage();
            return;
        }

        LoadSettings();
    }

    private void LoadSettings()
    {
        // we pass in the browser culture setting as determined by the executing thread.
        // this way we can localize Silverlight the same as the rest of the site.
        cultureCode = Thread.CurrentThread.CurrentUICulture.Name;
        if (Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
        {
            layoutDirection = "rtl";
        }
        if (sslIsAvailable)
        {
            siteRoot = SiteUtils.GetSecureNavigationSiteRoot();
        }
        else
        {
            siteRoot = SiteUtils.GetNavigationSiteRoot();
        }

        if (Request.IsAuthenticated)
        {
            userName = Context.User.Identity.Name;
        }

        siteSettings = CacheHelper.GetCurrentSiteSettings();
        if (siteSettings != null)
        {
            siteName = siteSettings.SiteName;
            enableMyPage = siteSettings.EnableMyPageFeature;
        }

        // "key1=value1,key2=value3,key3=value3"
        slmojo.InitParameters = "culture=" + cultureCode
            + ",layoutDirection=" + layoutDirection
            + ",siteRoot=" + siteRoot
            + ",ssl=" + sslIsAvailable.ToString()
            + ",enableMyPage=" + enableMyPage.ToString()
            + ",user=" + userName
            + ",siteName=" + siteName;
        
    }

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head id="Head1" runat="server">
    <title>mojoPortal Silverlight Application Framework</title>
<script type="text/javascript" src="/ClientScript/firebug/firebug.js"></script>
<script type="text/javascript" src="/ClientScript/google/gears/gears_init.js"></script>
<script type="text/javascript">
        
        var slhost;
        var slGears;
        
        function HookupSilverlight() {
            slhost = document.getElementById("slmojo");
            slGears = slhost.content.slgears;
            //slGears.LogScriptError('hello round trip');
        }
        
        function createDb() {
            if (!window.google || !google.gears) {
                return; 
            }
            try {
                var db = google.gears.factory.create('beta.database');
                slGears.ReceiveGearsDb(db);
            }
            catch (ex) {
                slGears.LogScriptError(createDBFailureMessage + ex.message);
            }
        }

        function getFieldName(gearsResultSet, fieldIndex) {
            if (!gearsResultSet) { return; }
            return gearsResultSet.fieldName(fieldIndex);
        }

        function getFieldValue(gearsResultSet, fieldIndex) {
            if (!gearsResultSet) { return; }
            return gearsResultSet.field(fieldIndex);
        }
        
        function convertArgs(args){
            return args;
        }

        function createJsArray(length) {
            var ar = new Array(length);
            return ar;
        }

        function assignArray(ar, index, val) {
            if (!ar) { alert('array was null in assignArray'); return; }
            ar[index] = val;
        }

        function passArrayTest(ar) {
            if (ar) {
                alert(ar.length);
                for (i = 0; i < ar.length; i++) {
                    alert(ar[i]);
                }
            }
        }

        function logToConsole(message) {
            console.log(message);
        }
 
    </script>
</head>
<body style="height:100%;margin:0;">
    <form id="form1" runat="server" style="height:100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div  style="height:100%;">
            <asp:Silverlight ID="slmojo" runat="server" 
            Source="~/ClientBin/mojoPortal.Silverlight.Framework.xap" 
            MinimumVersion="2.0.31005.0" 
            Width="100%" Height="100%" />
        </div>
    </form>
</body>
</html>
