<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="AdminDashboard.aspx.cs" Inherits="WebStore.UI.AdminDashboardPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop id="ctop1" runat="server" />
    <asp:Panel ID="pnlStoreManager" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoreadmin">
        <h2 class="moduletitle heading"><asp:Literal ID="litStoreManagerHeading" runat="server" /></h2>
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
        <ul class="simplelist">
            <li>
                <asp:HyperLink ID="lnkStoreSettings" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkProductAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkOfferAdmin" runat="server" />
            </li>
            <li id="liCategories" runat="server" Visible="false" >
                <asp:HyperLink ID="lnkCategoryAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkDownloadTermsAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkDiscountAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkOrderEntry" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkOrderHistory" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkBrowseCarts" runat="server" />
            </li>
            <li id="liReports" runat="server" Visible="false" >
                <asp:HyperLink ID="lnkReports" runat="server" />
                <portal:CButton ID="btnRebuildReports" runat="server"  CausesValidation="false" Visible="false" />
                
            </li>
        </ul>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom id="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
