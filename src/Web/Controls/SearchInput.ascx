<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchInput.ascx.cs" Inherits="Cynthia.Web.UI.SearchInput" %>

<asp:Panel id="pnlSearch" runat="server" CssClass="searchpanel">
<h2 id="heading" runat="server" style="position: absolute; left:-2000px; text-indent: -999em;"><asp:Label ID="lblSearchHeading" runat="server" AssociatedControlID="txtSearch" /></h2>
<cy:WatermarkTextBox ID="txtSearch" runat="server" CssClass="watermarktextbox" />
<asp:Button ID="btnSearch" runat="server" PostBackUrl="~/SearchResults.aspx" OnClick="btnSearch_Click" CausesValidation="false" />
<asp:ImageButton ID="btnSearch2" runat="server" OnClick="btnSearch2_Click" PostBackUrl="~/SearchResults.aspx" CausesValidation="false" Visible="false" />
</asp:Panel>