<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="FileManager.aspx.cs" Inherits="Cynthia.Web.AdminUI.FileManagerPage" %>

<%@ Register TagPrefix="admin" TagName="AdvFileManager" Src="~/Admin/Controls/AdvFileManager.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;
<asp:HyperLink ID="lnkFileManager" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper admin filemanager">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /> <asp:HyperLink ID="lnkAltFileManager" runat="server" CssClass="ModuleEditLink" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<admin:AdvFileManager ID="fm2" runat="server"  />
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
