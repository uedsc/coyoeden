<%@ Control language="c#" Inherits="Cynthia.Web.ContentUI.HtmlModule" CodeBehind="HtmlModule.ascx.cs" AutoEventWireup="false" %>

<portal:ModulePanel ID="pnlContainer" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<asp:Panel ID="pnlWrapper" runat="server"  CssClass="art-Post-inner panelwrapper htmlmodule">
<portal:ModuleTitleControl id="Title1" runat="server" EditUrl="/Modules/HtmlEdit.aspx" EnableViewState="false" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<portal:CRating runat="server" ID="Rating" Enabled="false" />
<div class=" modulecontent">
<portal:SlidePanel id="divContent" runat="server" EnableViewState="false" EnableSlideShow="false" class="slidecontainer"></portal:SlidePanel>
</div>
<div class="modulefooter"></div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:CPanel>
</portal:ModulePanel>
