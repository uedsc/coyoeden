<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EventCalendarModule.ascx.cs" Inherits="Cynthia.Web.EventCalendarUI.EventCalendar" %>
<portal:ModulePanel ID="pnlContainer" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper eventcalendar" >
    <portal:ModuleTitleControl id="Title1" runat="server"  />
    <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
    <div class="modulecontent">
    <cy:DataCalendar id="cal1"  runat="server"  
            EnableTheming='false' 
            UseAccessibleHeader="true"
            SelectionMode="Day"
            DayField="EventDate"
            CssClass="mpcalendarmain"
            DayHeaderStyle-CssClass="mpcalendardayheader"
            DayStyle-CssClass="mpcalendarday"
            NextPrevStyle-CssClass="mpcalendarnextprevious"
            OtherMonthDayStyle-CssClass="mpcalendarothermonth"
            SelectedDayStyle-CssClass="mpcalendarselectedday"
            SelectorStyle-CssClass="mpcalendarselector"
             TitleStyle-CssClass="mpcalendartitle"
             TodayDayStyle-CssClass="mpcalendartoday"
             WeekendDayStyle-CssClass="mpcalendarweekendday"
             NextPrevStyle-BorderStyle="None"
            NextPrevStyle-BorderWidth="0px"
            DayHeaderStyle-BorderStyle="None"
            DayHeaderStyle-BorderWidth="0px"
            ShowGridLines="true"
            >
	    <ItemTemplate>
		    <br />
			    <asp:HyperLink Text="<%# Resources.EventCalResources.EventCalendarEditEventLink%>" Tooltip="<%# Resources.EventCalResources.EventCalendarEditEventLink%>" 
			    id="editLink" 
			    ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
			    NavigateUrl='<%# this.SiteRoot + "/EventCalendar/EditEvent.aspx?pageid=" + PageId.ToString() + "&ItemID=" + Container.DataItem["ItemID"] + "&mid=" + ModuleId.ToString()  %>' 
			    Visible="<%# IsEditable %>" runat="server" />
			    <a href='<%# SiteRoot + "/EventCalendar/EventDetails.aspx?ItemID=" + Container.DataItem["ItemID"] + "&mid=" + Container.DataItem["ModuleID"] + "&pageid=" + PageId %>'>
			    <%# Server.HtmlEncode(Container.DataItem["Title"].ToString()) %></a>
	    </ItemTemplate>
	    <NoEventsTemplate>
		    <% if(UseFillerOnEmptyDays) {%><br /><br /><br /><% }%>
	    </NoEventsTemplate>
    </cy:DataCalendar>
    </div>
    </portal:CPanel>
    <div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</portal:ModulePanel>
