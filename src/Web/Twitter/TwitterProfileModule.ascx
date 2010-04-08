<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TwitterProfileModule.ascx.cs" Inherits="Cynthia.Modules.UI.TwitterProfileModule" %>

<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="art-Post-inner panelwrapper TwitterProfile">
<portal:ModuleTitleControl runat="server" id="TitleControl" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlTwitterProfile" runat="server" CssClass="modulecontent">
<portal:TwitterWidget ID="twitter" runat="server" />
<cy:SiteLabel ID="lblNoUser" runat="server" ConfigKey="NoAccountConfigured" CssClass="txterror" ResourceFile="TwitterResources" UseLabelTag="false" Visible="false" />
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
