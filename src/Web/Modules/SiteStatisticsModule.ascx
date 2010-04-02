<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SiteStatisticsModule.ascx.cs" Inherits="Cynthia.Web.StatisticsUI.SiteStatisticsModule" %>

<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper stats">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlStats" runat="server" EnableViewState="false" CssClass="modulecontent">
<asp:Panel ID="pnlMembership" runat="server" CssClass="site-statistics floatpanel">
    <portal:MembershipStatistics id="st1" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlUsersOnline" runat="server" CssClass="site-statistics floatpanel">
    <portal:OnlineStatistics ID="ol1" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlOnlineMemberList" runat="server" CssClass="clearpanel onlinemembers">
    <portal:OnlineMemberList ID="olm1" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlUserChart" runat="server" CssClass="clearpanel membergraph">
 <zgw:zedgraphweb id="zgMembershipGrowth" runat="server" RenderMode="ImageTag"
    Width="780" Height="400"></zgw:zedgraphweb>
</asp:Panel>
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
