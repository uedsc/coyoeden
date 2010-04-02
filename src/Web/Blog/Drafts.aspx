<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Drafts.aspx.cs" Inherits="Cynthia.Web.BlogUI.BlogDraftsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<asp:Panel id="divblog" runat="server" cssclass="blogcenter-rightnav" SkinID="plain">
        <h2 class="blogtitle"><asp:Label id="litHeader" runat="server" Visible="True" /></h2>
		<asp:repeater id="rptDrafts" runat="server"  EnableViewState="False" >
			<ItemTemplate>
			    <h3 class="blogtitle">
				<asp:HyperLink id="Title" runat="server"  SkinID="plain"
				    Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Heading").ToString()) %>' 
				    NavigateUrl='<%# SiteRoot + "/Blog/EditPost.aspx?pageid=" + PageId.ToString() + "&ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId %>'>
				</asp:HyperLink>&#160;</h3>
				<div class="blogcontent">
				<asp:Literal ID="litPubInfo" runat="server" Text='<%# Resources.BlogResources.BlogToBePublishedLabel + DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"StartDate", TimeOffset, BlogDateTimeFormat) %>' 
				Visible='<%# Convert.ToBoolean(Eval("IsPublished")) %>' />
				</div>
			</ItemTemplate>
		</asp:repeater>
    </asp:Panel>

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />