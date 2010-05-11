<%@ Control Language="c#"  AutoEventWireup="false" Codebehind="BlogModule.ascx.cs" Inherits="Cynthia.Web.BlogUI.BlogModule" %>
<%@ Register TagPrefix="blog" TagName="TagList" Src="~/Blog/Controls/CategoryListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="Archives" Src="~/Blog/Controls/ArchiveListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="FeedLinks" Src="~/Blog/Controls/FeedLinksControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="StatsControl" Src="~/Blog/Controls/StatsControl.ascx" %>
<%@ Import Namespace="Resources" %>
<portal:ModulePanel ID="pnlContainer" runat="server" CssClass="module">
<asp:Panel ID="pnlWrapper" runat="server" cssclass="module_inner panelwrapper blogmodule">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<portal:UpdatePanelX ID="updBlog" UpdateMode="Conditional" runat="server" CssClass="modulecontent blogwrapper clearfix">
<ContentTemplate>
    <asp:Panel id="divNav" runat="server" cssclass="blognavright" SkinID="plain">
        <asp:calendar id="calBlogNav" runat="server" EnableViewState="false"
         CaptionAlign="Top"
         CssClass="aspcalendarmain"
         DayHeaderStyle-CssClass="aspcalendardayheader"
         DayNameFormat="FirstLetter"
         DayStyle-CssClass="aspcalendarday"
         FirstDayOfWeek="sunday"
         NextMonthText="+"
         NextPrevFormat="CustomText"
         NextPrevStyle-CssClass="aspcalendarnextprevious"
         OtherMonthDayStyle-CssClass="aspcalendarothermonth"
         PrevMonthText="-"
         SelectedDayStyle-CssClass="aspcalendarselectedday"
         SelectorStyle-CssClass="aspcalendarselector"
         ShowDayHeader="true"
         ShowGridLines="false"
         ShowNextPrevMonth="true"
         ShowTitle="true"
         TitleFormat="MonthYear"
         TitleStyle-CssClass="aspcalendartitle"
         TodayDayStyle-CssClass="aspcalendartoday"
         WeekendDayStyle-CssClass="aspcalendarweekendday"
        ></asp:calendar>
        <blog:FeedLinks id="Feeds" runat="server" />
        <asp:Panel ID="pnlStatistics" runat="server">
        <blog:StatsControl id="stats" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlCategories" Runat="server" SkinID="plain">
	       <blog:TagList id="tags" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlArchives" Runat="server" SkinID="plain">
	        <blog:Archives id="archive" runat="server" />
        </asp:Panel>  		
    </asp:Panel>
    <asp:Panel id="divblog" runat="server" cssclass="blogcenter-rightnav" SkinID="plain">
        <asp:repeater id="rptBlogs" runat="server"  SkinID="Blog" EnableViewState="False" >
	        <ItemTemplate>
				<div class="postentry">
	            <h3 class="blogtitle">
		        <asp:HyperLink SkinID="BlogTitle" id="lnkTitle" runat="server" 
		            EnableViewState="false"
		            Text='<%# DataBinder.Eval(Container.DataItem,"Heading") %>' 
		            Visible='<%# BlogUseLinkForHeading %>'  
                    NavigateUrl='<%# FormatBlogTitleUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID")))  %>'>
		        </asp:HyperLink><asp:Literal ID="litTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Heading") %>' Visible='<%#(!BlogUseLinkForHeading) %>' />&nbsp;
		        <asp:HyperLink id="editLink"  runat="server" EnableViewState="false"
		            Text="<%# EditLinkText %>" 
				    Tooltip="<%# EditLinkTooltip %>"  
		            ImageUrl='<%# EditLinkImageUrl %>' 
		            NavigateUrl='<%# this.SiteRoot + "/Blog/EditPost.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId.ToString() %>' 
		            Visible="<%# IsEditable %>" CssClass="ModuleEditLink" />
		        </h3>
		        <asp:Panel ID="pnlPost" runat="server" Visible='<%# !TitleOnly %>'>
		        <portal:CRating runat="server" ID="Rating" Enabled='<%# EnableContentRatingSetting %>' ContentGuid='<%# new Guid(Eval("BlogGuid").ToString()) %>' AllowFeedback='false' />
		        <cy:OdiogoItem id="od1" runat="server" OdiogoFeedId='<%# OdiogoFeedIDSetting %>' ItemId='<%# DataBinder.Eval(Container.DataItem,"ItemID") %>' ItemTitle = '<%# Eval("Heading") %>' />
		        <div class="blogtext"><%# FormatBlogEntry(DataBinder.Eval(Container.DataItem, "Description").ToString(), DataBinder.Eval(Container.DataItem, "Abstract").ToString(), DataBinder.Eval(Container.DataItem, "ItemUrl").ToString(), Convert.ToInt32(Eval("ItemID")))%></div>
		        <goog:LocationMap ID="gmap" runat="server" 
		        Visible='<%# ((Eval("Location").ToString().Length > 0)&&(ShowGoogleMap)) %>' 
		        Location='<%# Eval("Location") %>' 
		        GMapApiKey='<%# GmapApiKey %>' 
		        EnableMapType='<%# GoogleMapEnableMapTypeSetting %>'
		        EnableZoom='<%# GoogleMapEnableZoomSetting %>' 
		        ShowInfoWindow='<%# GoogleMapShowInfoWindowSetting %>' 
		        EnableLocalSearch='<%# GoogleMapEnableLocalSearchSetting %>' 
		        EnableDrivingDirections='<%# GoogleMapEnableDirectionsSetting %>'
		        GmapType='<%# mapType %>' 
		        ZoomLevel='<%# GoogleMapInitialZoomSetting %>'
		        MapHeight='<%# GoogleMapHeightSetting %>'
		        MapWidth='<%# GoogleMapWidthSetting %>'
		        ></goog:LocationMap>
		        
		        <cy:AddThisButton ID="addThis1" runat="server"
		         AccountId='<%# addThisAccountId %>' 
		         Visible='<%# (!HideAddThisButton) %>'
		         UseMouseOverWidget='<%# useAddThisMouseOverWidget %>' 
		         Text='<%# Resources.BlogResources.AddThisButtonAltText %>'
		         TitleOfUrlToShare='<%# DataBinder.Eval(Container.DataItem,"Heading") %>' 
		         CustomBrand='<%# addThisCustomBrand %>'
		         CustomOptions='<%# addThisCustomOptions %>'
		         CustomLogoUrl='<%# addThisCustomLogoUrl %>'
		         CustomLogoBackgroundColor='<%# addThisCustomLogoBackColor %>'
		         CustomLogoColor='<%# addThisCustomLogoForeColor %>'
		         ButtonImageUrl='<%# addThisButtonImageUrl %>'
		         UrlToShare='<%# this.SiteRoot + DataBinder.Eval(Container.DataItem,"ItemUrl").ToString().Replace("~", string.Empty)  %>'
		        />
		        </asp:Panel>
		        <div class="postmetadata">
				  <span><%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"StartDate", TimeZoneOffset, DateFormat) %></span>
				  <%if (AllowComments){%>
				  <span> | <a href="<%# FormatBlogTitleUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID")))  %>">
					<%=BlogResources.BlogCommentCountLabel%>
					<%if (ShowCommentCounts){ %>(<%#DataBinder.Eval(Container.DataItem,"CommentCount") %> )<%} %>
					</a> 
				  </span>
				  <%} %>
				  <div class="the_author">
				    <img height="20" width="20" class="avatar avatar-20 photo" src="<%= SkinBaseUrl%>img/avatar20.jpeg" alt=""/>    				    
				    <h4><a rel="external" title="" href="#"><%# FormatPostAuthor(DataBinder.Eval(Container.DataItem, "Name").ToString())%></a></h4>
				  </div>
				</div>
				</div>
	        </ItemTemplate>
        </asp:repeater>
        <portal:CCutePager ID="pgr" runat="server" />
    </asp:Panel>
    <%if (!string.IsNullOrEmpty(BlogCopyright))
	  { %>
    <div class="blogcopyright"><%=BlogCopyright %></div>
    <%} %>
    <portal:DisqusWidget ID="disqus" runat="server" />
</ContentTemplate>
</portal:UpdatePanelX>
</portal:CPanel>
</asp:Panel>
</portal:ModulePanel>
