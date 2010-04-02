<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ContentManagerPreview.aspx.cs" Inherits="Cynthia.Web.AdminUI.ContentManagerPreview" Title="Untitled Page" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<asp:Panel id="pnlPreview" runat="server" CssClass="contentmanagerpreview">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
    <asp:HyperLink ID="lnkContentManager" runat="server" NavigateUrl="~/Admin/ContentCatalog.aspx" />&nbsp;&gt;
	 <cy:SiteLabel id="lbl1" runat="server" ConfigKey="ContentManagerPreviewContentLabel" UseLabelTag="false" > </cy:SiteLabel>
	 <asp:Label ID="lblModuleTitle" runat="server" />
     <small><asp:hyperlink id="lnkPublish" cssclass="ModuleEditLink" EnableViewState="false" runat="server" SkinID="plain" />
     &nbsp;<asp:HyperLink ID="lnkBackToList" runat="server" Visible="false" cssclass="ModuleEditLink" SkinID="plain"></asp:HyperLink></small>
 </div>  
 <asp:Panel ID="pnlWarning" runat="server" Visible="false">
 <cy:SiteLabel id="SiteLabel1" runat="server" CssClass="txterror" ConfigKey="ContentManagerNonMultiPageFeatureWarning" UseLabelTag="false" > </cy:SiteLabel>
 </asp:Panel>
<asp:Panel ID="pnlViewModule" runat="server"></asp:Panel>

</asp:Panel>

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
