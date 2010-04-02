<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ForumModule.ascx.cs" Inherits="Cynthia.Web.ForumUI.ForumModule" %>
<%@ Register TagPrefix="forum" TagName="SearchBox" Src="~/Forums/Controls/ForumSearchBox.ascx" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper forums">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<forum:SearchBox id="sb1" runat="server" />
<asp:Panel ID="pnlForumList" runat="server">
<table summary='<%# Resources.ForumResources.ForumsTableSummary %>'  cellpadding="0" cellspacing="1" border="0" width="100%">
	<thead><tr class="moduletitle">
		<th id="tdSubscribedHead" runat="server" >
			<cy:SiteLabel id="lblSubscribed" runat="server" ConfigKey="ForumModuleSubscribedLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='<%# Resources.ForumResources.ForumModuleForumLabel %>'>
			<cy:SiteLabel id="lblForumName" runat="server" ConfigKey="ForumModuleForumLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='<%# Resources.ForumResources.ForumModuleThreadCountLabel %>'>
			<cy:SiteLabel id="lblThreadCount" runat="server" ConfigKey="ForumModuleThreadCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='<%# Resources.ForumResources.ForumModulePostCountLabel %>'>
			<cy:SiteLabel id="lblPostCount" runat="server" ConfigKey="ForumModulePostCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
		<th id='<%# Resources.ForumResources.ForumModulePostLastPostLabel %>'>
			<cy:SiteLabel id="lblLastPost" runat="server" ConfigKey="ForumModulePostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" />
		</th>
	</tr></thead>
   <asp:Repeater id="rptForums" runat="server" >
      <HeaderTemplate><tbody></HeaderTemplate>
      <ItemTemplate >
         <tr class="modulerow">
            <td id="tdSubscribed" runat="server" class="txtmed padded" Visible='<%# Request.IsAuthenticated %>'> 
                <div id="divSbubcriberCount" runat="server" visible='<%# (showSubscriberCount &&(!IsEditable)) %>'>
                   <asp:Literal ID="litSubCount" runat="server" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div> 
                <div id="divEditor" runat="server" visible='<%# IsEditable %>'>
                    <portal:GreyBoxHyperlink ID="lnkUserLookup" runat="server" ClientClick="return GB_showCenter(this.title, this.href, 530,700)" 
                    NavigateUrl='<%# this.SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
                    Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' 
                    ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div>
                <div class="forumnotify">
				<asp:HyperLink ID="lnkNotify" runat="server" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png"  %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
                 ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 </div>
            </td>
            <td headers='<%# Resources.ForumResources.ForumModuleForumLabel %>'> 
				<h3><asp:HyperLink id="editLink" runat="server"
				    Text="<%# Resources.ForumResources.ForumEditForumLabel %>" 
					Tooltip="<%# Resources.ForumResources.ForumEditForumLabel %>"
				    ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
				    NavigateUrl='<%# this.SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# IsEditable %>"  />
				<asp:HyperLink id="HyperLink3" runat="server"
				    Text="RSS" Tooltip="RSS"
				    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + RssImageFile  %>' 
				    NavigateUrl='<%# this.SiteRoot + "/Forums/RSS.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# EnableRSSAtForumLevel %>"  />
				<asp:HyperLink id="viewlink1" runat="server" SkinID="TitleLink"
				    NavigateUrl='<%# this.SiteRoot + "/Forums/ForumView.aspx?pageid=" + PageId.ToString() + "&mid=" + ModuleId + "&ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID")   %>'>
				    <%# DataBinder.Eval(Container.DataItem,"Title") %></asp:HyperLink></h3>
				<%# DataBinder.Eval(Container.DataItem,"Description").ToString() %>
            </td>
            <td headers='<%# Resources.ForumResources.ForumModuleThreadCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"ThreadCount") %>
            </td>
            <td headers='<%# Resources.ForumResources.ForumModulePostCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"PostCount") %>
            </td>
            <td headers='<%# Resources.ForumResources.ForumModulePostLastPostLabel %>'>  
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"MostRecentPostDate", TimeOffset) %>
            </td>
         </tr>
      </ItemTemplate>
      <alternatingItemTemplate>
		<tr class="modulealtrow">
            <td id="tdSubscribedAlt" runat="server" class="txtmed padded" Visible='<%# Request.IsAuthenticated %>'>  
                <div id="divSbubcriberCount" runat="server" visible='<%# (showSubscriberCount &&(!IsEditable)) %>'>
                   <asp:Literal ID="litSubCount" runat="server" Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div> 
                <div id="divEditor" runat="server" visible='<%# IsEditable %>'>
                    <portal:GreyBoxHyperlink ID="lnkUserLookup" runat="server" ClientClick="return GB_showCenter(this.title, this.href, 530, 700)" 
                    NavigateUrl='<%# this.SiteRoot + "/Forums/SubscriberDialog.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
                    Text='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' 
                    ToolTip='<%# FormatSubscriberCount(Convert.ToInt32(Eval("SubscriberCount")))%>' />
                </div>
                <div class="forumnotify">
                <asp:HyperLink ID="lnkNotify" runat="server" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png"  %>' NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" NavigateUrl='<%# notificationUrl + "#forum" + Eval("ItemID") %>' 
				 Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>'
                 ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Subscribed")) ? Resources.ForumResources.UnSubscribeLink : Resources.ForumResources.SubscribeLink %>' />
                 </div>
				
            </td>
            <td headers='<%# Resources.ForumResources.ForumModuleForumLabel %>'> 
				<h3><asp:HyperLink id="Hyperlink1" runat="server"
				    Text="<%# Resources.ForumResources.ForumEditForumLabel %>" 
					Tooltip="<%# Resources.ForumResources.ForumEditForumLabel %>" 
				    ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
				    NavigateUrl='<%# this.SiteRoot + "/Forums/EditForum.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# IsEditable %>"  />
				<asp:HyperLink id="HyperLink3" runat="server"
				    Text="RSS" Tooltip="RSS"
				    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + RssImageFile  %>' 
				    NavigateUrl='<%# this.SiteRoot + "/Forums/RSS.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() %>' 
				    Visible="<%# EnableRSSAtForumLevel %>"  />
				<asp:HyperLink id="Hyperlink2" runat="server" SkinID="TitleLink"
				    NavigateUrl='<%# this.SiteRoot + "/Forums/ForumView.aspx?pageid=" + PageId.ToString() + "&mid=" + ModuleId + "&ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") %>' >
				    <%# DataBinder.Eval(Container.DataItem,"Title") %></asp:HyperLink></h3>
				<%# DataBinder.Eval(Container.DataItem,"Description").ToString()%>
            </td>
            <td headers='<%# Resources.ForumResources.ForumModuleThreadCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"ThreadCount") %>
            </td>
            <td headers='<%# Resources.ForumResources.ForumModulePostCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"PostCount") %>
            </td>
            <td headers='<%# Resources.ForumResources.ForumModulePostLastPostLabel %>'>  
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"MostRecentPostDate", TimeOffset) %>
            </td>
         </tr>
      </AlternatingItemTemplate>
      <FooterTemplate></tbody></FooterTemplate>
   </asp:Repeater>
</table>
<div id="divEditSubscriptions" runat="server" class="settingrow forumnotification">
    <asp:HyperLink id="editSubscriptionsLink" runat="server" />
    <asp:HyperLink ID="lnkModuleRSS" runat="server" />
</div>
</asp:Panel>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
