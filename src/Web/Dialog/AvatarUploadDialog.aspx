<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="AvatarUploadDialog.aspx.cs" Inherits="Cynthia.Web.Dialog.AvatarUploadDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" IncludejCrop="true" />
</head>
<body>
    <form id="form1" runat="server">
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeJQuery="true" IncludeOomph="false" />
     
     <portal:ImageCropper id="cropper" runat="server" />
    
    <asp:Label ID="lblMaxAvatarSize" runat="server" />
    <NeatUpload:InputFile ID="avatarFile" Style="width: 220px; height: 23px" runat="server" />
    <NeatUpload:ProgressBar ID="progressBar" runat="server">
        <cy:SiteLabel ID="SiteLabel5" runat="server" ConfigKey="CheckProgressText"> </cy:SiteLabel>
    </NeatUpload:ProgressBar>
    <asp:Button ID="btnUploadAvatar" runat="server" Text="Upload" ValidationGroup="avatar">
    </asp:Button>
    <asp:RegularExpressionValidator ID="regexAvatarFile" ControlToValidate="avatarFile" Display="Dynamic"
        EnableClientScript="True" runat="server" ValidationGroup="avatar" />
    </form>
</body>
</html>
