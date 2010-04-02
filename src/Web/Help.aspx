<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Help.aspx.cs" Inherits="Cynthia.Web.UI.Pages.Help" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlHelp" runat="server">
        <asp:Literal ID="litEditLink" runat="server" />
        <asp:Literal ID="litHelp" runat="server" />
    </asp:Panel>
    <portal:CGoogleAnalyticsScript ID="CGoogleAnalyticsScript1" runat="server" />
    </form>
</body>
</html>
