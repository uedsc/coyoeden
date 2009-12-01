<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default"%>
<%@ Register Src="Views/PostList.ascx" TagName="PostList" TagPrefix="uc1" %>
<%@ MasterType TypeName="CoyoEden.UI.SiteMaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeader"  Runat="Server">
<% this.Master.CssClass = UserIsAdmin?"admin home":"home"; %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody"  Runat="Server">
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
</asp:Content>