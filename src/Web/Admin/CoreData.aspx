<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="CoreData.aspx.cs" Inherits="Cynthia.Web.AdminUI.CoreDataPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnl1" runat="server" CssClass="panelwrapper adminmenu">
        <h2 class="moduletitle">
            <asp:Literal ID="litAdminHeading" runat="server" /></h2>
        <div class="modulecontent">
            <ul class="simplelist">
                <li id="liLanguageAdmin" runat="server">
                    <asp:HyperLink ID="lnkLanguageAdmin" runat="server" />
                </li>
                <li id="liCurrencyAdmin" runat="server">
                    <asp:HyperLink ID="lnkCurrencyAdmin" runat="server" />
                </li>
                <li id="liCountryAdmin" runat="server">
                    <asp:HyperLink ID="lnkCountryAdmin" runat="server" />
                </li>
                <li id="liGeoZoneAdmin" runat="server">
                    <asp:HyperLink ID="lnklnkGeoZone" runat="server" />
                </li>
                <li id="liTaxClassAdmin" runat="server">
                    <asp:HyperLink ID="lnkTaxClassAdmin" runat="server" />
                </li>
                <li id="liTaxRateAdmin" runat="server">
                    <asp:HyperLink ID="lnkTaxRateAdmin" runat="server" />
                </li>
                <asp:Literal ID="litSupplementalLinks" runat="server" />
            </ul>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
