<%@ Page Language="C#" ClassName="BlogArchiveView.aspx" Inherits="System.Web.UI.Page"   %>
<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>


<script runat="server">
    /// <summary>
    /// If you installed mojoportal after version 2.2.7.8, then you can safely delete this file.
    /// The Blog feature was moved into the mojoPortal.Features projects and pages are located now in the Blog
    /// folder. This page is to here to support sites that upgraded by redirecting the request from the old page
    /// to the new page using a 301 (object moved permanently)
    /// 
    /// </summary>

    private int pageId = -1;
    private int moduleId = -1;
    private int month = DateTime.UtcNow.Month;
    private int year = DateTime.UtcNow.Year;
    private string siteRoot = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadParams();

        Response.Status = "301 Moved Permanently";
        Response.AddHeader("Location", siteRoot + "/Blog/ViewArchive.aspx?pageid=" 
            + pageId.ToString(CultureInfo.InvariantCulture)
            + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
            + "&year=" + year.ToString(CultureInfo.InvariantCulture)
            + "&month=" + month.ToString(CultureInfo.InvariantCulture)
            );  
    }

    private void LoadParams()
    {
        
        pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
        moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
        month = WebUtils.ParseInt32FromQueryString("month", month);
        year = WebUtils.ParseInt32FromQueryString("year", year);
        siteRoot = WebUtils.GetSiteRoot();
    }
    
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>BlogArchiveView</title> 
</head>
<body>
 <form id="form1" runat="server">
    <h1>BlogArchiveView</h1>
</form>
</body>
</html>