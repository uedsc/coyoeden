<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ArchiveViewControl.ascx.cs" Inherits="Cynthia.Web.BlogUI.ArchiveViewControl" %>
<%@ Register TagPrefix="blog" TagName="TagList" Src="~/Blog/Controls/CategoryListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="Archives" Src="~/Blog/Controls/ArchiveListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="FeedLinks" Src="~/Blog/Controls/FeedLinksControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="StatsControl" Src="~/Blog/Controls/StatsControl.ascx" %>
<asp:Panel id="pnlBlog" runat="server" CssClass="blogwrapper">
<div class="modulecontent">
    <asp:Panel id="divNav" runat="server" cssclass="blognavright" SkinID="plain">
		<blog:FeedLinks id="Feeds" runat="server" />
		<asp:Panel ID="pnlStatistics" runat="server">
		<blog:StatsControl id="stats" runat="server" />
		</asp:Panel>
		<asp:Panel ID="pnlCategories" Runat="server">
			<blog:TagList id="tags" runat="server" />
		</asp:Panel>
		<asp:Panel ID="pnlArchives" Runat="server">
			<blog:Archives id="archive" runat="server" />
		</asp:Panel>
    </asp:Panel>
    <asp:Panel id="divblog" runat="server" cssclass="blogcenter-rightnav" SkinID="plain">
        <h2 class="blogtitle"><asp:Label id="litHeader" runat="server" Visible="True" /></h2>
		<asp:repeater id="dlArchives" runat="server"  EnableViewState="False" >
			<ItemTemplate>
			    <h3 class="blogtitle">
				<asp:HyperLink id="Title" runat="server"  SkinID="plain"
				    Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Heading").ToString()) %>' 
				    NavigateUrl='<%# FormatBlogUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>'>
				</asp:HyperLink>&#160;</h3>
				<div class="blogcontent">
				<asp:HyperLink id="Hyperlink1" runat="server" 
				    Text='<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"StartDate", TimeOffset, BlogDateTimeFormat) %>' 
				    NavigateUrl='<%# FormatBlogUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>'>
				</asp:HyperLink>&#160;
				<asp:HyperLink id="Hyperlink2" runat="server" Text='<%# FeedBackLabel + "(" + DataBinder.Eval(Container.DataItem,"CommentCount") + ")" %>' 
				Visible='<%# AllowComments %>' 
				NavigateUrl='<%# FormatBlogUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>'>
				</asp:HyperLink>&#160;
				</div>
			</ItemTemplate>
		</asp:repeater>
    </asp:Panel>
    <asp:Label id="lblCopyright" Runat="server" CssClass="txtcopyright"></asp:Label>
 </div>
</asp:Panel>
