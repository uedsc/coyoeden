<%@ Page language="c#" Codebehind="ViewArchive.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.BlogUI.BlogArchiveView" %>
<%@ Register TagPrefix="blog" TagName="ArchiveView" Src="~/Blog/Controls/ArchiveViewControl.ascx" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:ModulePanel ID="pnlContainer" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<blog:ArchiveView id="ArchiveView1" runat="server" />
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</portal:ModulePanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
