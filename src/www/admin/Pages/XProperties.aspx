<%@ Page Title="" Language="C#" MasterPageFile="~/themes/admin/site.master" AutoEventWireup="true" CodeFile="XProperties.aspx.cs" Inherits="admin_Pages_XProperties" %>
<%@ Register Src="~/Views/XPropertyList.ascx" TagName="XProList" TagPrefix="vs" %>
<asp:Content ID="Content0" ContentPlaceHolderID="cphHead" Runat="Server">
<vs:SiteCSS ID="css960" runat="server" CSSRelativeToRoot="assets/css/960.css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
<div class="container_12">
<div class="grid_3">
	<vs:XProList ID="Xprolist" runat="server" />
</div>
<div class="grid_9"></div>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFooter" Runat="Server"></asp:Content>

