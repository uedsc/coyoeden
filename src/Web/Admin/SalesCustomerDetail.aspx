<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SalesCustomerDetail.aspx.cs" Inherits="Cynthia.Web.AdminUI.SalesCustomerDetailPage" %>
<%@ Register Src="~/Controls/UserCommerceHistory.ascx" TagPrefix="portal" TagName="PurchaseHistory" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkCommerceReports" runat="server" NavigateUrl="~/Admin/SalesSummary.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkCustomerReport" runat="server" NavigateUrl="~/Admin/SalesSummary.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="art-Post-inner panelwrapper ">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<portal:PurchaseHistory id="purchaseHx" runat="server"></portal:PurchaseHistory>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
