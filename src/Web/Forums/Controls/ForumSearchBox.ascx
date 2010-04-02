<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForumSearchBox.ascx.cs" Inherits="Cynthia.Web.ForumUI.ForumSearchBox" %>
<asp:Panel ID="pnlForumSearch" runat="server" CssClass="settingrow forumsearch" DefaultButton="btnSearch">
<asp:TextBox ID="txtSearch" runat="server" CssClass="widetextbox forumsearchbox" />
<portal:CButton ID="btnSearch" runat="server" ValidationGroup="forumsearch" />
<asp:RequiredFieldValidator ID="reqSearchText" runat="server" ControlToValidate="txtSearch" Display="Dynamic" ValidationGroup="forumsearch" />
</asp:Panel>
