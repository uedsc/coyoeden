<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TwitterSearchModule.ascx.cs" Inherits="Cynthia.Modules.UI.TwitterSearchModule" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="art-Post-inner panelwrapper TwitterSearch">
<portal:ModuleTitleControl  runat="server" id="TitleControl" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlTwitterSearch" runat="server" CssClass="modulecontent">
<portal:TwitterWidget ID="twitter" runat="server" />
<cy:SiteLabel ID="lblNoSearch" runat="server" ConfigKey="NoSearchTermsConfigured" CssClass="txterror" ResourceFile="TwitterResources" UseLabelTag="false" Visible="false" />
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
