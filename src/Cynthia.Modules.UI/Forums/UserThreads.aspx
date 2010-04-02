<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="UserThreads.aspx.cs" Inherits="Cynthia.Web.ForumUI.ForumUserThreadsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<cy:CornerRounderTop id="ctop1" runat="server" />
		<asp:Panel id="pnlForum" runat="server" cssclass="panelwrapper forumview">
		     <h2 class="moduletitle"><asp:Literal ID="litTitle" runat="server" /></h2>
		     <div class="modulecontent">
		    <div class="settingrow forumdesc">
		        <asp:Literal ID="litForumDescription" runat="server" />
		    </div>
		    <div class="modulepager">
		        <portal:CCutePager ID="pgrTop" runat="server" />
		        <a href="" class="ModulePager" id="lnkNewThread" runat="server"></a>
		    </div>
			<table summary='<%# Resources.ForumResources.ForumViewTableSummary %>' border="0" cellspacing="1" width="100%" cellpadding="3">
				<thead><tr class="moduletitle">
				    <th id='<%# Resources.ForumResources.ForumViewSubjectLabel %>'>
						<cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="ForumViewSubjectLabel" ResourceFile="ForumResources" UseLabelTag="false" />
					</th>
					<th id='<%# Resources.ForumResources.ForumLabel %>'>
						<cy:SiteLabel id="ForumLabel1" runat="server" ConfigKey="ForumLabel" ResourceFile="ForumResources" UseLabelTag="false" />
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
					    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/thread.gif"  %>'  />
						<a href="Thread.aspx?thread=<%# DataBinder.Eval(Container.DataItem,"ThreadID") %>&amp;mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"ForumID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
					</td>
					<td headers='<%# Resources.ForumResources.ForumLabel %>'>    	
						<a href="ForumView.aspx?mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"ForumID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Forum").ToString())%></a>		
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewStartedByLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewViewCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalViews")%>
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewReplyCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalReplies")%>
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>'>  
						<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem), "MostRecentPostDate", TimeOffset)%>
						<br /><%# DataBinder.Eval(Container.DataItem, "MostRecentPostUser")%>
					</td>
				</tr>
			</ItemTemplate>
			<alternatingItemTemplate>
				<tr class="modulealtrow">
					<td  headers='<%# Resources.ForumResources.ForumViewSubjectLabel %>'> 
					    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/thread.gif"  %>'  />
						<a href="Thread.aspx?thread=<%# DataBinder.Eval(Container.DataItem,"ThreadID") %>&amp;mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"ForumID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
					</td>
					<td headers='<%# Resources.ForumResources.ForumLabel %>'> 	
						<a href="ForumView.aspx?mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"ForumID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Forum").ToString())%></a>		
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewStartedByLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewViewCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalViews")%>
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewReplyCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalReplies")%>
					</td>
					<td headers='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>'>  
						<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem), "MostRecentPostDate", TimeOffset)%>
						<br /><%# DataBinder.Eval(Container.DataItem, "MostRecentPostUser")%>
					</td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate></tbody></FooterTemplate>
		</asp:Repeater>
			</table>
		    <div class="modulepager">
				<portal:CCutePager ID="pgrBottom" runat="server" />
				<a href="" class="ModulePager" id="lnkNewThreadBottom" runat="server"></a>
		    </div>
		    <div class="modulefooter">
		        &nbsp;
		    </div>
				</div>
		</asp:Panel>
		<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
