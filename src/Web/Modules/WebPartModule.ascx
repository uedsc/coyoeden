<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="WebPartModule.ascx.cs" Inherits="Cynthia.Web.WebPartUI.WebPartModule" %>

<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper webpartmodule">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<asp:Panel ID="pnlPlaceHolder" runat="server"></asp:Panel>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
