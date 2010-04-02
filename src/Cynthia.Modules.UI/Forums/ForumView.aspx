<%@ Page language="c#" Codebehind="ForumView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.ForumUI.ForumView" %>
<%@ Register TagPrefix="forum" TagName="SearchBox" Src="~/Forums/Controls/ForumSearchBox.ascx" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> 
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<asp:Panel id="pnlForum" runat="server" cssclass="art-Post-inner panelwrapper forumview" EnableViewState="false">
     <h2 class="moduletitle"><asp:Literal ID="litForumTitle" runat="server" /></h2>
     <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
     <div class="modulecontent">
    <div class="settingrow forumdesc">
        <asp:Literal ID="litForumDescription" runat="server" />
    </div>
    <forum:SearchBox id="sb1" runat="server" />
    <asp:Panel ID="pnlNotify" runat="server" Visible="false" CssClass="forumnotify">
        <asp:HyperLink ID="lnkNotify" runat="server" ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png"  %>' NavigateUrl='<%# notificationUrl %>' 
				 Text='<%# Resources.ForumResources.SubscribeLink %>' />
                 &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" NavigateUrl='<%# notificationUrl %>' 
				 Text='<%# Resources.ForumResources.SubscribeLongLink %>'
                 ToolTip='<%# Resources.ForumResources.SubscribeLongLink %>' />
                
    </asp:Panel>
    <div class="modulepager">
        <portal:CCutePager ID="pgrTop" runat="server" />
        <a href="" class="ModulePager" id="lnkNewThread" runat="server"></a>
        <asp:HyperLink ID="lnkLogin" runat="server" CssClass="ModulePager" />
    </div>
	<table summary='<%# Resources.ForumResources.ForumViewTableSummary %>' border="0" cellspacing="1" width="100%" cellpadding="3">
		<thead><tr class="moduletitle">
		    <th id='<%# Resources.ForumResources.ForumViewSubjectLabel %>'>
				<cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="ForumViewSubjectLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumViewStartedByLabel %>'>
				<cy:SiteLabel id="lblForumStartedBy" runat="server" ConfigKey="ForumViewStartedByLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumViewViewCountLabel %>'>
				<cy:SiteLabel id="lblTotalViewsCountLabel" runat="server" ConfigKey="ForumViewViewCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumViewReplyCountLabel %>'>
				<cy:SiteLabel id="lblTotalRepliesCountLabel" runat="server" ConfigKey="ForumViewReplyCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th >
			<th id='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>'>
				<cy:SiteLabel id="lblLastPostLabel" runat="server" ConfigKey="ForumViewPostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" />	
			</th>
		</tr></thead>
<asp:Repeater id="rptForums" runat="server" >
    <HeaderTemplate><tbody></HeaderTemplate>
	<ItemTemplate>
		<tr class="modulerow">
			<td headers='<%# Resources.ForumResources.ForumViewSubjectLabel %>'> 
			    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/" + ThreadImage %>'  />
					<asp:HyperLink id="editLink" 
					Text="<%# Resources.ForumResources.ForumThreadEditLabel %>" 
					Tooltip="<%# Resources.ForumResources.ForumThreadEditLabel %>"
					ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
					NavigateUrl='<%# SiteRoot + "/Forums/EditThread.aspx?thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>' 
					Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' runat="server" />
					
					<asp:HyperLink id="HyperLink3" runat="server"
		            Text="RSS" Tooltip="RSS"
		            ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + RSSImageFileName  %>' 
		            NavigateUrl='<%# SiteRoot + "/RSS.aspx?ItemID=" + ItemId.ToString() + "&mid=" + ModuleId.ToString() + "&pageid=" + PageId.ToString() + "&thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") %>' 
		            Visible="<%# EnableRssAtThreadLevel %>"  />
					
					<a href="Thread.aspx?pageid=<%# PageId %>&amp;mid=<%# ModuleId %>&amp;ItemID=<%# ItemId %>&amp;thread=<%# DataBinder.Eval(Container.DataItem,"ThreadID") %>">
					<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
					
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewStartedByLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"StartedBy") %>
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewViewCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalViews") %>
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewReplyCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalReplies") %>
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>'>  
				<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"MostRecentPostDate", TimeOffset) %>
				<br /><%# DataBinder.Eval(Container.DataItem,"MostRecentPostUser") %>
			</td>
		</tr>
	</ItemTemplate>
	<alternatingItemTemplate>
		<tr class="modulealtrow">
			<td  headers='<%# Resources.ForumResources.ForumViewSubjectLabel %>'> 
			    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/" + ThreadImage  %>'  />
				<asp:HyperLink id="editLink" 
				Text="<%# Resources.ForumResources.ForumThreadEditLabel %>" 
				Tooltip="<%# Resources.ForumResources.ForumThreadEditLabel %>"
				ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
				NavigateUrl='<%# SiteRoot + "/Forums/EditThread.aspx?thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>' 
				Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' 
				runat="server" />
			    <asp:HyperLink id="HyperLink3" runat="server"
		            Text="RSS" Tooltip="RSS"
		            ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + RSSImageFileName  %>' 
		            NavigateUrl='<%# SiteRoot + "/Forums/RSS.aspx?ItemID=" + ItemId.ToString() + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() + "&thread=" + DataBinder.Eval(Container.DataItem,"ThreadID") %>' 
		            Visible="<%# EnableRssAtThreadLevel %>"  />
				<a href="Thread.aspx?pageid=<%# PageId %>&amp;mid=<%# ModuleId %>&amp;ItemID=<%# ItemId %>&amp;thread=<%# DataBinder.Eval(Container.DataItem,"ThreadID") %>">
					<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewStartedByLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"StartedBy") %>
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewViewCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalViews") %>
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewReplyCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalReplies") %>
			</td>
			<td headers='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>'>  
				<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"MostRecentPostDate", TimeOffset) %>
				<br /><%# DataBinder.Eval(Container.DataItem,"MostRecentPostUser") %>
			</td>
		</tr>
	</AlternatingItemTemplate>
	<FooterTemplate></tbody></FooterTemplate>
</asp:Repeater>
	</table>
    <div class="modulepager">
		<portal:CCutePager ID="pgrBottom" runat="server" EnableViewState="false" />
		<a href="" class="ModulePager" id="lnkNewThreadBottom" runat="server" EnableViewState="false"></a>
    </div>
    <div class="modulefooter">
        &nbsp;
    </div>
</div>
       </portal:CPanel>
       <div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />