<%@ Control language="c#" Inherits="Cynthia.Web.XmlUI.XmlModule" CodeBehind="XmlModule.ascx.cs" AutoEventWireup="false" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper xmlmodule">
<portal:ModuleTitleControl id="Title1" runat="server" DisabledModuleSettingsLink="true" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
    <asp:xml id="xml1" runat="server" />
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>	
