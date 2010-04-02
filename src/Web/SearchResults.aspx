<%@ Page language="c#"  ValidateRequest="false" MaintainScrollPositionOnPostback="true" EnableViewStateMac="false" Codebehind="SearchResults.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.SearchResults" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlMain" runat="server" CssClass="art-Post-inner panelwrapper searchresults" DefaultButton="btnDoSearch">
    <h2 class="moduletitle"><cy:SiteLabel id="SiteLabel3" runat="server" ConfigKey="SearchPageTitle" UseLabelTag="false" > </cy:SiteLabel></h2>
    <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
    <div class="modulecontent">
    <div id="divDelete" runat="server" visible="false" class="settingrow">
    <asp:Button ID="btnRebuildSearchIndex" runat="server" />
    </div>
    <div class="settingrow">
        <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="search-help" />
        <asp:TextBox ID="txtSearchInput" runat="server" Columns="50" MaxLength="255" CssClass="widetextbox"></asp:TextBox>
        <asp:DropDownList id="ddFeatureList" runat="server" ></asp:DropDownList>
		<portal:CButton ID="btnDoSearch" runat="server" CausesValidation="false" UseSubmitBehavior="true" />
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
    </portal:CPanel>
    <div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />