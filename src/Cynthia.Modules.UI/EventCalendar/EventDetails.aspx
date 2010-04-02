<%@ Page language="c#" Codebehind="EventDetails.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.EventCalendarUI.EventCalendarViewEvent" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop id="ctop1" runat="server" />
		<asp:Panel id="pnlEvent" runat="server" CssClass="panelwrapper eventcalendar">
		    <h2 class="moduletitle">
		        <asp:Label id="lblTitle" runat="server"></asp:Label> -
		        <asp:Label id="lblDate" runat="server"></asp:Label>
		    </h2>
		    <div class="modulecontent">
		        <cy:SiteLabel id="Sitelabel1" runat="server" ConfigKey="EventCalendarEditStartTimeLabel" ResourceFile="EventCalResources"> </cy:SiteLabel>
				<asp:Label id="lblStartTime" runat="server"></asp:Label>
				<br />
				<cy:SiteLabel id="Sitelabel2" runat="server" ConfigKey="EventCalendarEditEndTimeLabel" ResourceFile="EventCalResources"> </cy:SiteLabel>
				<asp:Label id="lblEndTime" runat="server"></asp:Label>
				<br /><br />
				<div>
				<asp:Literal id="litDescription" runat="server" />
				</div>
				<goog:LocationMap ID="gmap" runat="server" CssClass="gmap_event"></goog:LocationMap>
		    </div>
		</asp:Panel>
	<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

