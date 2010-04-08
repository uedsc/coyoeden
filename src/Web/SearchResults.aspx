<%@ Page language="c#"  ValidateRequest="false" MaintainScrollPositionOnPostback="true" EnableViewStateMac="false" Codebehind="SearchResults.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.SearchResults" %>
<%@ MasterType TypeName="Cynthia.Web.layout" %>
<asp:Content ContentPlaceHolderID="altContent1" ID="cphAlt1" runat="server">
<%this.Master.CssClass = "searchfrm"; %>
</asp:Content>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<asp:Panel ID="pnlMain" runat="server" CssClass="panelwrapper searchresults clearfix">
    <h2 class="moduletitle"><cy:SiteLabel id="SiteLabel3" runat="server" ConfigKey="SearchPageTitle" UseLabelTag="false" > </cy:SiteLabel></h2>
    <div class="modulecontent">
    <div id="divDelete" runat="server" visible="false" class="settingrow">
    <asp:Button ID="btnRebuildSearchIndex" runat="server" />
    </div>
    <div class="settingrow">
        <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="search-help" />
		<asp:label id="lblDuration" runat="server" visible="False" ></asp:label> 
		<cy:SiteLabel id="lblSeconds" runat="server" Visible="False" ConfigKey="SearchResultsSecondsLabel" UseLabelTag="false"> </cy:SiteLabel>
		<asp:label id="lblMessage" runat="server" ></asp:label> 
    </div>
    <div id="divResults" runat="server" class="settingrow">
		<cy:SiteLabel id="lblReslts" runat="server" ConfigKey="SearchResultsLabel" UseLabelTag="false"> </cy:SiteLabel>
		<asp:label id="lblFrom" runat="server" font-bold="True"></asp:label>-<asp:label id="lblTo" runat="server" font-bold="True"></asp:label>
		<cy:SiteLabel id="Sitelabel1" runat="server" ConfigKey="SearchResultsOfLabel" UseLabelTag="false"> </cy:SiteLabel>
		<asp:label id="lblTotal" runat="server" font-bold="True"></asp:label>
		<cy:SiteLabel id="lblFor" runat="server" ConfigKey="SearchResultsForLabel" UseLabelTag="false"> </cy:SiteLabel>
		<asp:label id="lblQueryText" runat="server" font-bold="True"></asp:label>
    </div>
    <asp:Panel id="pnlSearchResults" runat="server" Visible="False" CssClass="settingrow">
    <portal:CCutePager ID="pgrTop" runat="server" visible="false" />
        <asp:repeater id="rptResults" runat="server" enableviewstate="False">
		    <itemtemplate>
				    <h3>
		            <asp:hyperlink id="Hyperlink1" runat="server" 
					    navigateurl='<%# BuildUrl((Cynthia.Business.WebHelpers.IndexItem)Container.DataItem) %>'>
					    <%# FormatLinkText(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString())  %></asp:hyperlink></h3>
				    <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# Cynthia.Web.Framework.SecurityHelper.RegexRelativeImageUrlPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
                    <%# DataBinder.Eval(Container.DataItem, "Intro").ToString() %>
                    </NeatHtml:UntrustedContent>
		            
		    </itemtemplate>
	    </asp:repeater>
	    <div>&nbsp;</div>
	    <portal:CCutePager ID="pgrBottom" runat="server" Visible="false" /> 
    </asp:Panel>
    <asp:Panel ID="pnlNoResults" runat="server" Visible="False">
	    <asp:Label ID="lblNoResults" runat="server"></asp:Label>
    </asp:Panel>
    </div>
</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />