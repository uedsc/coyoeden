<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="User_controls_xdashboard_Default" Title="Extensions" MasterPageFile="~/themes/admin/site.master"%>
<%@ Reference Control = "Extensions.ascx" %>
<%@ Reference Control = "Editor.ascx" %>
<%@ Reference Control = "Settings.ascx" %>
<asp:Content ID="Content0" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
<br />
<div class="settings">
    <asp:HiddenField ID="args" runat="server" />
    <div>
        <asp:PlaceHolder ID="ucPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFooter" Runat="Server"></asp:Content>
