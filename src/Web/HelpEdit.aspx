<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpEdit.aspx.cs" Inherits="Cynthia.Web.UI.Pages.HelpEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divEditor" runat="server">
        <cye:EditorControl id="edContent" runat="server"> </cye:EditorControl>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        <asp:HyperLink ID="lnkCancel" runat="server" />
        <portal:SessionKeepAliveControl id="ka1" runat="server" />
    </div>
    
    <portal:CGoogleAnalyticsScript ID="CGoogleAnalyticsScript1" runat="server" />
    </form>
</body>
</html>
