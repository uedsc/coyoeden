<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Comments_Default" %>
<%@ Register src="DataGrid.ascx" tagname="DataGrid" tagprefix="uc1" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>
<asp:Content ID="Content0" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">  
    <div class="settings">
        <menu:TabMenu ID="TabMenu" runat="server" />
        <uc1:DataGrid ID="DataGridComments" runat="server" />
    </div>       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFooter" Runat="Server"></asp:Content>