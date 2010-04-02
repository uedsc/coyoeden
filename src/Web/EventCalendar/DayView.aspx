<%@ Page language="c#" Codebehind="DayView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.EventCalendarUI.EventCalendarDayView" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop id="ctop1" runat="server" />
		<asp:Panel id="pnlDay" runat="server" CssClass="art-Post-inner panelwrapper eventcalendar">
		    <h2 class="moduletitle"><asp:Literal id="litDate" runat="server" /><a href="" id="lnkNewEvent" runat="server" class="ModuleEditLink"></a></h2>
		    <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
		    <div class="modulecontent">
		    <asp:repeater id="rptEvents" runat="server" EnableViewState="false">
			    <itemtemplate>
			        <h3>
			            <%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Title").ToString())%>
			            <%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.StartTime")).ToShortTimeString()%> - <%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.EndTime")).ToShortTimeString()%>
			        </h3>
			        <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# Cynthia.Web.Framework.SecurityHelper.RegexRelativeImageUrlPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
                        <%# DataBinder.Eval(Container, "DataItem.Description").ToString()%>
                    </NeatHtml:UntrustedContent>
			         
			    </itemtemplate>
		    </asp:repeater>
		    </div>
		    </portal:CPanel>
		    <div class="cleared"></div>
		</asp:Panel>
		<cy:CornerRounderBottom id="cbottom1" runat="server" />	
		</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
