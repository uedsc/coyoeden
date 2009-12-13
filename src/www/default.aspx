<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default"%>
<%@ Register Src="Views/PostList.ascx" TagName="PostList" TagPrefix="uc1" %>
<%@ Register Src="Views/HeadlineList.ascx" TagName="HeadlineList" TagPrefix="uc1" %>
<%@ MasterType TypeName="CoyoEden.UI.SiteMaster" %>
<%@ Import Namespace="CoyoEden.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeader"  Runat="Server">
<% this.Master.CssClass = UserIsAdmin?"admin home":"home"; %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody"  Runat="Server">
	<div id="m6_left">
		<uc1:HeadlineList ID="HeadlineList1" runat="server" ShowCount="5"/>
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