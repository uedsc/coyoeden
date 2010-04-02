<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FileManagerDialog.aspx.cs"
    Inherits="Cynthia.Web.Dialog.FileManagerDialog" %>

<%@ Register TagPrefix="admin" TagName="AdvFileManager" Src="~/Admin/Controls/AdvFileManager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" />
    <portal:IEStyleIncludes ID="IEStyleIncludes1" runat="server" IncludeHtml5Script="false" />
</head>
<body>
    <form id="form1" runat="server">
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeQtFile="true" />
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAltFileManager" runat="server" NavigateUrl="FileManagerAltDialog.aspx" CssClass="altfile" />
    </div>
    <div>
        <admin:AdvFileManager ID="fm2" runat="server" />
    </div>
    </form>
</body>
</html>
