<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContentWorkflow.aspx.cs" Inherits="Cynthia.Web.AdminUI.ContentWorkflowPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server"  />
<asp:Panel id="pnl1" runat="server" CssClass="art-Post-inner panelwrapper adminmenu">
<h2 class="moduletitle"><asp:Literal ID="litAdminHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
		<ul class="simplelist">		    
		    <li id="liAwaitingApproval" runat="server">
		        <asp:HyperLink ID="lnkAwaitingApproval" runat="server" />
		    </li>	
		    <li id="liRejectedContent" runat="server">
		        <asp:HyperLink ID="lnkRejectedContent" runat="server" />
		    </li>
		    <li id="liPendingPages" runat="server">
		        <asp:HyperLink ID="lnkPendingPages" runat="server" />
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
