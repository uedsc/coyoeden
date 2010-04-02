<%@ Page Language="C#" ClassName="ForumView.aspx" Inherits="System.Web.UI.Page"   %>
<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>

<script runat="server">
    /// <summary>
    /// If you installed mojoportal after version 2.2.7.8, then you can safely delete this file.
    /// The ImageGallery feature was moved into the mojoPortal.Features projects and pages are located now in the Blog
    /// folder. This page is to here to support sites that upgraded by redirecting the request from the old page
    /// to the new page using a 301 (object moved permanently)
    /// 
    /// </summary>

    private int pageId = -1;
    private int moduleId = -1;
    private int itemId = -1;
    private int pageNumber = 1;
    private string siteRoot = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadParams();

        Response.Status = "301 Moved Permanently";
        Response.AddHeader("Location", siteRoot + "/ImageGallery/GalleryBrowse.aspx?pageid=" 
            + pageId.ToString(CultureInfo.InvariantCulture)
            + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
            + "&ItemID=" + itemId.ToString(CultureInfo.InvariantCulture)
            + "&pagenumber=" + pageNumber.ToString(CultureInfo.InvariantCulture)
            );
        
    }

    private void LoadParams()
    {
        pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
        moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
        itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
        pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
        
        siteRoot = WebUtils.GetSiteRoot();
    }
    


</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>GalleryBrowse</title> 
</head>
<body>
 <form id="form1" runat="server">

</form>
</body>
</html>
