<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterTemplates.aspx.cs" Inherits="Cynthia.Web.ELetterUI.LetterTemplatesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span> 
    <asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;
    <asp:HyperLink ID="lnkTemplates" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlLetterHtmlTemplate" runat="server" CssClass="art-Post-inner panelwrapper letterhtmltemplate">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<cy:CGridView ID="grdLetterHtmlTemplate" runat="server"
     AllowPaging="false"
     AutoGenerateColumns="false"
     ShowHeader="false"
     ShowFooter="false" 
     CssClass="editgrid"
     DataKeyNames="Guid"
     SkinID="plain">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <a href='<%# SiteRoot + "/eletter/LetterTemplateEdit.aspx?t=" + Eval("Guid").ToString() %>' title='<%# Eval("Title") %>'><%# Eval("Title") %></a>
            </ItemTemplate>
		</asp:TemplateField>
</Columns>
 </cy:CGridView>
<div class="settingrow">
	<br /><asp:HyperLink ID="lnkAddNew" runat="server" />
</div>
<div class="modulepager">
    <portal:CCutePager ID="pgrLetterHtmlTemplate" runat="server" />
</div>
<br class="clear" />
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />