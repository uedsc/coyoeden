<%@ Control language="c#" Inherits="Cynthia.Web.ContentUI.HtmlModule" CodeBehind="HtmlModule.ascx.cs" AutoEventWireup="false" %>
<portal:ModulePanel ID="pnlContainer" runat="server" CssClass="module">
<asp:Panel ID="pnlWrapper" runat="server"  CssClass="module_inner htmlmodule clearfix">
<portal:ModuleTitleControl id="Title1" runat="server" EditUrl="/Modules/HtmlEdit.aspx" EnableViewState="false" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<portal:CRating runat="server" ID="Rating" Enabled="false" />
<portal:SlidePanel id="divContent" runat="server" EnableViewState="false" EnableSlideShow="false" class="slidecontainer"></portal:SlidePanel>
<div class="modulefooter"></div>
</portal:CPanel>
</asp:Panel>
</portal:ModulePanel>
