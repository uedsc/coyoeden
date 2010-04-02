<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdvnacedTools.aspx.cs" Inherits="Cynthia.Web.AdminUI.AdvnacedToolsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnl1" runat="server" CssClass="art-Post-inner panelwrapper adminmenu">
        <h2 class="moduletitle">
            <asp:Literal ID="litAdminHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <ul class="simplelist">
                <li id="liUrlManager" runat="server">
                    <asp:HyperLink ID="lnkUrlManager" runat="server" CssClass="lnkUrlManager" />
                </li>
                <li id="liRedirectManager" runat="server">
                    <asp:HyperLink ID="lnkRedirectManager" runat="server" CssClass="lnkRedirectManager" />
                </li>
                <li id="liBannedIPs" runat="server">
                    <asp:HyperLink ID="lnkBannedIPs" runat="server" CssClass="lnkBannedIPs" />
                </li>
                <li id="liFeatureAdmin" runat="server">
                    <asp:HyperLink ID="lnkFeatureAdmin" runat="server" CssClass="lnkFeatureAdmin" />
                </li>
                <li id="liWebPartAdmin" runat="server">
                    <asp:HyperLink ID="lnkWebPartAdmin" runat="server" CssClass="lnkWebPartAdmin" />
                </li>
                <li id="liTaskQueue" runat="server">
                    <asp:HyperLink ID="lnkTaskQueue" runat="server" CssClass="lnkTaskQueue" />
                </li>
                <li id="liDevTools" runat="server">
                    <asp:HyperLink ID="lnkDevTools" runat="server" CssClass="lnkDevTools" />
                </li>
                <asp:Literal ID="litSupplementalLinks" runat="server" />
            </ul>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
