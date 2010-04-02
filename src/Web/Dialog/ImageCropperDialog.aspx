<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ImageCropperDialog.aspx.cs" Inherits="Cynthia.Web.Dialog.ImageCropperDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" IncludejCrop="true" />
</head>
<body class="filedialog">
    <form id="form1" runat="server">
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeJQuery="true"  IncludeOomph="false" />
    <div id="filewrapper" style="padding: 10px;  overflow:auto;">
    <asp:HyperLink ID="lnkReturn" runat="server" Visible="false" />
    <portal:ImageCropper id="cropper" runat="server" />
    </div>
    <portal:CGoogleAnalyticsScript ID="CGoogleAnalyticsScript1" runat="server" />
    </form>
</body>
</html>
