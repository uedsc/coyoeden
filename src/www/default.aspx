<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default"%>
<%@ Register Src="Views/PostList.ascx" TagName="PostList" TagPrefix="uc1" %>
<%@ MasterType TypeName="CoyoEden.UI.SiteMaster" %>
<%@ Import Namespace="CoyoEden.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeader"  Runat="Server">
<% this.Master.CssClass = UserIsAdmin?"admin home":"home"; %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody"  Runat="Server">
	<div id="m6_left">
		<div class="headlines">
			<h3>本期主题精选集</h3>
			<% var total = 0; %>
			<% var items = Post.GetPosts(true, 5, out total); %>
			<%items.ForEach(x =>
			{ %>
			
			<%}); %>
		</div>
		<div id="divError" runat="Server" />
		<uc1:PostList ID="PostList1" runat="server" />
		<blog:PostCalendar runat="server" ID="calendar" 
		EnableViewState="false"
		ShowPostTitles="true" 
		BorderWidth="0"
		NextPrevStyle-CssClass="header"
		CssClass="calendar" 
		WeekendDayStyle-CssClass="weekend" 
		OtherMonthDayStyle-CssClass="other" 
		UseAccessibleHeader="true" 
		Visible="false" 
		Width="100%" /> 
	</div>
	<div id="m6_right">
		 <blog:WidgetZone ID="WidgetZone1" runat="server" Name="cy_WidgetZone0" />
	</div>   
</asp:Content>