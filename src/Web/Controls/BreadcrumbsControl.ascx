<%@ Control Language="c#" AutoEventWireup="False" Codebehind="BreadcrumbsControl.ascx.cs" Inherits="Cynthia.Web.UI.BreadcrumbsControl" %>

<asp:Panel ID="pnlWrapper" runat="server">
<portal:SiteMapPath ID="breadCrumbsControl" runat="server" Visible="false">
<RootNodeTemplate>
<asp:HyperLink ID="lnkRoot" runat="server" NavigateUrl='<%# siteRoot %>' Text='<%# rootLinkText %>' CssClass='<%# CssClass %>' />
</RootNodeTemplate>
<NodeTemplate>
<asp:HyperLink ID="lnkNode" runat="server" NavigateUrl='<%# siteRoot + Eval("Url").ToString().Replace("~/", "/") %>' Text='<%# Eval("Title") %>' CssClass='<%# CssClass %>' />
</NodeTemplate>
<CurrentNodeTemplate>
<asp:HyperLink ID="lnkCurrent" runat="server" NavigateUrl='<%#  siteRoot + Eval("Url").ToString().Replace("~/", "/") %>' Text='<%# Eval("Title") %>' CssClass='<%# CurrentPageCssClass %>' />
</CurrentNodeTemplate>
</portal:SiteMapPath>
<asp:Literal ID="childCrumbs" runat="server" />
</asp:Panel>