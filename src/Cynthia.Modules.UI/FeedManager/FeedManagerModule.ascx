<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FeedManagerModule.ascx.cs" Inherits="Cynthia.Web.FeedUI.FeedManagerModule" %>
<%@ Register TagPrefix="NeatHtml" Namespace="Brettle.Web.NeatHtml" Assembly="Brettle.Web.NeatHtml" %>
<portal:ModulePanel ID="pnlContainer" runat="server" CssClass="module">
<asp:Panel ID="pnlWrapper" runat="server" CssClass="module_inner panelwrapper rssfeedmodule">
<portal:ModuleTitleControl ID="Title1" runat="server" />
<portal:UpdatePanelX ID="updPnlRSSA" UpdateMode="Conditional" runat="server" CssClass="modulecontent rsswrapper clearfix">
<ContentTemplate>
    <asp:Panel ID="divNav" runat="server" CssClass="rssnavright" SkinID="plain">
        <asp:Label ID="lblFeedListName" Font-Bold="True" runat="server"></asp:Label>
        <a id="lnkAggregateRSS" href="~/FeedManager/FeedAggregate.aspx" runat="server">
            <img alt="RSS" id="imgAggregateRSS" src="/images/xml.gif" runat="server" /></a>
        <br />
        <portal:CDataList ID="dlstFeedList" runat="server" EnableViewState="false">
            <ItemTemplate>
                <asp:HyperLink ID="editLink" runat="server" Text="<%# Resources.FeedResources.EditImageAltText%>"
                    ToolTip="<%# Resources.FeedResources.EditImageAltText%>" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                    NavigateUrl='<%# this.SiteRoot + "/FeedManager/FeedEdit.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId.ToString()  %>'
                    Visible="<%# IsEditable %>" />
                <asp:HyperLink ID="Hyperlink2" runat="server" Visible="<%# LinkToAuthorSite %>" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Url")%>'>
					<%# DataBinder.Eval(Container, "DataItem.Author")%>
                </asp:HyperLink>
                <asp:Button ID="Button1" runat="server" Visible="<%# UseFeedListAsFilter %>" 
                    CommandName="filter" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ItemID")%>' Text='<%# DataBinder.Eval(Container, "DataItem.Author")%>' CssClass="buttonlink" />
                <asp:HyperLink ID="Hyperlink3" runat="server" Visible="<%# ShowIndividualFeedLinks %>"
                    ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.RssUrl")%>'>
                </asp:HyperLink>&nbsp;&nbsp;
            </ItemTemplate>
        </portal:CDataList>
    </asp:Panel>
    <asp:Panel ID="divFeedEntries" runat="server" CssClass="rsscenter-rightnav" SkinID="plain">
        <asp:Literal ID="lblFeedHeading" runat="server" Visible="false" />
        <asp:Repeater ID="rptEntries" runat="server" 
            onitemcommand="rptEntries_ItemCommand">
            <ItemTemplate>
            <asp:ImageButton CommandName="Confirm" CommandArgument='<%#DataBinder.Eval(Container, "DataItem.EntryHash") + "_" + Convert.ToString(DataBinder.Eval(Container, "DataItem.Confirmed")) %>' 
            ID="ConfirmBtn" runat="server" ImageUrl='<%# ConfirmImage + DataBinder.Eval(Container, "DataItem.Confirmed") + ".png"%>'
            visible='<%# EnableInPlaceEditing %>' AlternateText='<%# Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Confirmed"))?Resources.FeedResources.EntryPublishTrueAlternateText:Resources.FeedResources.EntryPublishFalseAlternateText %>' 
            />
                <div class='<%#string.Format("rssentry rssfeedentry{0}",DataBinder.Eval(Container, "DataItem.Confirmed")) %>' id="divFeedEntry" runat="server" >
                <h3 class="rsstitle">
                   <asp:HyperLink ID="Hyperlink4" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Link")%>'>
					<%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Title").ToString())%>
                   </asp:HyperLink>
                </h3>
                <div class="rssdate rounded">
					<span class="date"><%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"),"dd")%>th</span>
					<br />
					<%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"),"MMM")%>
                </div>
                <div class="rssfeedname" id="div2" runat="server" visible='<%# RSSAggregatorShowFeedNameBeforeContent %>'>
                    <asp:HyperLink ID="Hyperlink6" runat="server"  NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
											<%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.FeedName").ToString())%>
                    </asp:HyperLink>
                </div>
                <div class="rsstext" id="divFeedBody" runat="server" visible='<%# ShowItemDetail %>'>
                    <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                        ClientScriptUrl="~/ClientScript/NeatHtml.js" Visible='<%# useNeatHtml %>'>
                        <%# RSSAggregatorUseExcerpt ? UIHelper.CreateExcerpt(DataBinder.Eval(Container, "DataItem.Description").ToString(), RSSAggregatorExcerptLength, RSSAggregatorExcerptSuffix) : DataBinder.Eval(Container, "DataItem.Description").ToString()%>
                        
                    </NeatHtml:UntrustedContent>
                    <div id="unfilteredContent" runat="server" visible='<%# (!useNeatHtml) %>'>
                         <%# RSSAggregatorUseExcerpt ? UIHelper.CreateExcerpt(DataBinder.Eval(Container, "DataItem.Description").ToString(), RSSAggregatorExcerptLength, RSSAggregatorExcerptSuffix) : DataBinder.Eval(Container, "DataItem.Description").ToString()%>
                        
                    </div>
                </div>
                <div class="rssfeedname" id="div1" runat="server" visible='<%# RSSAggregatorShowFeedName %>'>
                    <asp:HyperLink ID="Hyperlink5" runat="server"  NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
											<%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.FeedName").ToString())%>
                    </asp:HyperLink>
                </div>
                <div class="postmetadata">
				  <span>
					<%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"),"tt")%>&nbsp;<%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"),"hh:mm:ss")%> @<%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"),"yyyy")%> | 
				  </span>
				  <span><a href="<%# DataBinder.Eval(Container, "DataItem.Link")%>">0 Comments</a> </span>
				  <%if (RSSAggregatorShowAuthor){%>
				  <div class="the_author">
				    <img height="20" width="20" class="avatar avatar-20 photo" src="<%= SkinBaseUrl%>img/avatar20.jpeg" alt=""/>    				    
				    <h4><a rel="external" title="" href="<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>"><%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Author").ToString())%></a></h4>
				  </div>
				  <%} %>
				</div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        
        <cy:DataCalendar id="dataCal1"  runat="server" Visible="false"
        EnableTheming='false' 
        UseAccessibleHeader="true"
        SelectionMode="Day"
        DayField="PubDate"
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
		    <asp:HyperLink ID="lnkItemUrl" runat="server" NavigateUrl='<%# Container.DataItem["Link"] %>' Text='<%# Container.DataItem["Title"] %>' />
											
    </ItemTemplate>
    <NoEventsTemplate>
	    <% if(UseFillerOnEmptyDays) {%><br /><br /><br /><% }%>
    </NoEventsTemplate>
</cy:DataCalendar>
        
    </asp:Panel>
    <portal:CCutePager ID="pgrRptEntries" runat="server" />
</ContentTemplate>
</portal:UpdatePanelX>
</asp:Panel>
</portal:ModulePanel>
