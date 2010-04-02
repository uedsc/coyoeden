<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Admin.aspx.cs" Inherits="Cynthia.Web.ELetterUI.AdminPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span>
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlLetterInfo" runat="server" CssClass="art-Post-inner panelwrapper newsletteradmin">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<cy:CGridView ID="grdLetterInfo" runat="server"
     AllowPaging="false"
     AutoGenerateColumns="false" 
     ShowHeader="false"
     CssClass="editgrid"
     DataKeyNames="LetterInfoGuid"
     EnableTheming="false">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkEdit" runat="server"
                 Text='<%# Eval("Title") %>' Tooltip='<%# Resources.Resource.NewsletterEditLink + " " + Eval("Title") %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterInfoEdit.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkDraftList" runat="server"
                 Text='<%# Resources.Resource.NewsletterDraftListLink %>' Tooltip='<%# Resources.Resource.NewsletterDraftListLink %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterDrafts.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkArchiveList" runat="server"
                 Text='<%# Resources.Resource.NewsletterArchiveListLink %>' Tooltip='<%# Resources.Resource.NewsletterArchiveListLink %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterArchive.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkSubscribers" runat="server"
                 Text='<%# Eval("SubscriberCount") + " " + Resources.Resource.NewsletterSubscribersLink %>' Tooltip='<%# Resources.Resource.NewsletterSubscribersToolTip + " " + Eval("Title") %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterSubscribers.aspx?l=" + Eval("LetterInfoGuid").ToString() %>' ></asp:HyperLink>
                <%# string.Format(System.Globalization.CultureInfo.InvariantCulture,Resources.Resource.NewsletterUnverifiedCountFormat, Eval("UnVerifiedCount"))%>
            </ItemTemplate>
		</asp:TemplateField>
		
</Columns>
 </cy:CGridView>
<div class="modulepager" >
    <portal:CCutePager ID="pgrLetterInfo" runat="server" />
    <br /><asp:HyperLink ID="lnkAddNew" runat="server" />
    <asp:HyperLink ID="lnkManageTemplates" runat="server"></asp:HyperLink>
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
