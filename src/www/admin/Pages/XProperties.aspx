<%@ Page Title="" Language="C#" MasterPageFile="~/themes/admin/site.master" AutoEventWireup="true" CodeFile="XProperties.aspx.cs" Inherits="admin_Pages_XProperties" %>
<%@ Register Src="~/Views/XPropertyList.ascx" TagName="XProList" TagPrefix="vs" %>
<asp:Content ID="Content0" ContentPlaceHolderID="cphHead" Runat="Server">
<vs:SiteCSS ID="cssYPager" runat="server" CSSRelativeToRoot="Assets/css/Vivasky.YPager.css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
<vs:YPager ID="yPager1" runat="server" EdgeLinks="1" ItemsPerPage="3" OnPageChanged="yPager1_OnPageChanged" />
<vs:XProList ID="Xprolist" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFooter" Runat="Server"></asp:Content>

