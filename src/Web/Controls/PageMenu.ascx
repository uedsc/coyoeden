<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="PageMenu.ascx.cs" Inherits="Cynthia.Web.UI.PageMenuControl" %>
<cy:CornerRounderTop id="topRounder" runat="server" />
<asp:UpdatePanel ID="upMenu" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:PlaceHolder ID="menuPlaceHolder" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
<cy:CornerRounderBottom id="bottomRounder" runat="server" />
