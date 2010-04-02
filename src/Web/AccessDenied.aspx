<%@ Page CodeBehind="AccessDenied.aspx.cs" Language="c#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.AccessDeniedPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">	
<div class="accessdenied">
    <cy:SiteLabel id="lblAccessDeniedLabel" runat="server" ConfigKey="AccessDeniedLabel" CssClass="txterror"> </cy:SiteLabel>
    <p>
		<asp:Literal id="lblAccessDenied" runat="server" />
	</p>
	<p>
	    <asp:HyperLink ID="lnkHome" runat="server" />
	</p>
</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
