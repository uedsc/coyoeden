<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminMenu.aspx.cs" Inherits="Cynthia.Web.AdminUI.AdminMenuPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlAdminMenu" runat="server" CssClass="art-Post-inner panelwrapper adminmenu">
<h2 class="moduletitle"><asp:Literal ID="litAdminHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
		<ul class="simplelist">
		    <li id="liSiteSettings" runat="server">
		        <asp:HyperLink ID="lnkSiteSettings" runat="server" CssClass="lnkSiteSettings" />
		    </li>
		    <li id="liCommerceReports" runat="server">
		        <asp:HyperLink ID="lnkCommerceReports" runat="server" CssClass="lnkCommerceReports" />
		    </li>
		    <li id="liContentManager" runat="server">
		        <asp:HyperLink ID="lnkContentManager" runat="server" CssClass="lnkContentManager" />
		    </li>
		    <li id="liContentWorkFlow" runat="server">
		        <asp:HyperLink ID="lnkContentWorkFlow" runat="server" CssClass="lnkContentWorkFlow" />
		    </li>
		    <li id="liContentTemplates" runat="server">
		        <asp:HyperLink ID="lnkContentTemplates" runat="server" CssClass="lnkContentTemplates" />
		    </li>
		    <li id="liStyleTemplates" runat="server">
		        <asp:HyperLink ID="lnkStyleTemplates" runat="server" CssClass="lnkStyleTemplates" />
		    </li>
		    <li id="liPageTree" runat="server">
		        <asp:HyperLink ID="lnkPageTree" runat="server" CssClass="lnkPageTree" />
		    </li>
		    <li id="liRoleAdmin" runat="server">
		        <asp:HyperLink ID="lnkRoleAdmin" runat="server" CssClass="lnkRoleAdmin" />
		    </li>
		    <li id="liFileManager" runat="server">
		        <asp:HyperLink ID="lnkFileManager" runat="server" CssClass="lnkFileManager" />
		    </li>
		    <li id="liMemberList" runat="server">
		        <asp:HyperLink ID="lnkMemberList" runat="server" CssClass="lnkMemberList" />
		    </li>
		    <li id="liAddUser" runat="server">
		        <asp:HyperLink ID="lnkAddUser" runat="server" CssClass="lnkAddUser" />
		    </li>
		    <li id="liNewsletter" runat="server">
		        <asp:HyperLink ID="lnkNewsletter" runat="server" CssClass="lnkNewsletter" />
		    </li>
		    <li id="liCoreData" runat="server">
		        <asp:HyperLink ID="lnkCoreData" runat="server" CssClass="lnkCoreData" />
		    </li>
		   
		    <li id="liAdvancedTools" runat="server">
		        <asp:HyperLink ID="lnkAdvancedTools" runat="server" CssClass="lnkAdvancedTools" />
		    </li>
		    <li id="liLogViewer" runat="server">
		        <asp:HyperLink ID="lnkLogViewer" runat="server" CssClass="lnkLogViewer" />
		    </li>
		    <li id="liServerInfo" runat="server">
		        <asp:HyperLink ID="lnkServerInfo" runat="server" CssClass="lnkServerInfo" />
		    </li>
		    <asp:Literal ID="litSupplementalLinks" runat="server" />
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
