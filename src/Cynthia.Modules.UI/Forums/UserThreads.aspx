<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="UserThreads.aspx.cs" Inherits="Cynthia.Web.GroupUI.GroupUserThreadsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<cy:CornerRounderTop id="ctop1" runat="server" />
		<asp:Panel id="pnlGroup" runat="server" cssclass="panelwrapper forumview">
		     <h2 class="moduletitle"><asp:Literal ID="litTitle" runat="server" /></h2>
		     <div class="modulecontent">
		    <div class="settingrow forumdesc">
		        <asp:Literal ID="litGroupDescription" runat="server" />
		    </div>
		    <div class="modulepager">
		        <portal:CCutePager ID="pgrTop" runat="server" />
		        <a href="" class="ModulePager" id="lnkNewThread" runat="server"></a>
		    </div>
			<table summary='<%# Resources.GroupResources.GroupViewTableSummary %>' border="0" cellspacing="1" width="100%" cellpadding="3">
				<thead><tr class="moduletitle">
				    <th id='<%# Resources.GroupResources.GroupViewSubjectLabel %>'>
						<cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="GroupViewSubjectLabel" ResourceFile="GroupResources" UseLabelTag="false" />
					</th>
					<th id='<%# Resources.GroupResources.GroupLabel %>'>
						<cy:SiteLabel id="GroupLabel1" runat="server" ConfigKey="GroupLabel" ResourceFile="GroupResources" UseLabelTag="false" />
					</th>
					<th id='<%# Resources.GroupResources.GroupViewStartedByLabel %>'>
						<cy:SiteLabel id="lblGroupStartedBy" runat="server" ConfigKey="GroupViewStartedByLabel" ResourceFile="GroupResources" UseLabelTag="false" />
					</th>
					<th id='<%# Resources.GroupResources.GroupViewViewCountLabel %>'>
						<cy:SiteLabel id="lblTotalViewsCountLabel" runat="server" ConfigKey="GroupViewViewCountLabel" ResourceFile="GroupResources" UseLabelTag="false" />
					</th>
					<th id='<%# Resources.GroupResources.GroupViewReplyCountLabel %>'>
						<cy:SiteLabel id="lblTotalRepliesCountLabel" runat="server" ConfigKey="GroupViewReplyCountLabel" ResourceFile="GroupResources" UseLabelTag="false" />
					</th >
					<th id='<%# Resources.GroupResources.GroupViewPostLastPostLabel %>'>
						<cy:SiteLabel id="lblLastPostLabel" runat="server" ConfigKey="GroupViewPostLastPostLabel" ResourceFile="GroupResources" UseLabelTag="false" />	
					</th>
				</tr></thead>
		<asp:Repeater id="rptGroups" runat="server" >
		    <HeaderTemplate><tbody></HeaderTemplate>
			<ItemTemplate>
				<tr class="modulerow">
					<td headers='<%# Resources.GroupResources.GroupViewSubjectLabel %>'> 
					    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/thread.gif"  %>'  />
						<a href="Thread.aspx?thread=<%# DataBinder.Eval(Container.DataItem,"TopicID") %>&amp;mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"GroupID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "TopicTitle").ToString())%></a>
					</td>
					<td headers='<%# Resources.GroupResources.GroupLabel %>'>    	
						<a href="GroupView.aspx?mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"GroupID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Group").ToString())%></a>		
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewStartedByLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewViewCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalViews")%>
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewReplyCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalReplies")%>
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewPostLastPostLabel %>'>  
						<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem), "MostRecentPostDate", TimeOffset)%>
						<br /><%# DataBinder.Eval(Container.DataItem, "MostRecentPostUser")%>
					</td>
				</tr>
			</ItemTemplate>
			<alternatingItemTemplate>
				<tr class="modulealtrow">
					<td  headers='<%# Resources.GroupResources.GroupViewSubjectLabel %>'> 
					    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/thread.gif"  %>'  />
						<a href="Thread.aspx?thread=<%# DataBinder.Eval(Container.DataItem,"TopicID") %>&amp;mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"GroupID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "TopicTitle").ToString())%></a>
					</td>
					<td headers='<%# Resources.GroupResources.GroupLabel %>'> 	
						<a href="GroupView.aspx?mid=<%# DataBinder.Eval(Container.DataItem,"ModuleID") %>&amp;pageid=<%# DataBinder.Eval(Container.DataItem,"PageID") %>&amp;ItemID=<%# DataBinder.Eval(Container.DataItem,"GroupID") %>">
							<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Group").ToString())%></a>		
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewStartedByLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewViewCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalViews")%>
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewReplyCountLabel %>'>  
						<%# DataBinder.Eval(Container.DataItem, "TotalReplies")%>
					</td>
					<td headers='<%# Resources.GroupResources.GroupViewPostLastPostLabel %>'>  
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
