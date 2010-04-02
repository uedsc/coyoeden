<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FileManagerAltDialog.aspx.cs" Inherits="Cynthia.Web.Dialog.FileManagerAltDialog" %>
<%@ Register TagPrefix="admin" TagName="FileManager" Src="~/Admin/Controls/FileManager.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeQtFile="true" />
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAltFileManager" runat="server" NavigateUrl="FileManagerDialog.aspx" CssClass="altfile" />
    </div>
    <div>
        <admin:FileManager ID="fm1" runat="server" />
    </div>
    </form>
</body>
</html>
