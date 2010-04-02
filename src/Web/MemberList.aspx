<%@ Page language="c#" Codebehind="MemberList.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.MemberList" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlUserList" runat="server" SkinID="plain" CssClass="art-Post-inner panelwrapper memberlist" DefaultButton="btnSearchUser">
	<h2 class="moduletitle"><cy:SiteLabel id="lblMemberList" runat="server" ConfigKey="MemberListTitleLabel" UseLabelTag="false"> </cy:SiteLabel></h2>	
	<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">	
	<div class="modulecontent">
	<div class="modulesubtitle" id="divNewUser" runat="server">
	    <asp:TextBox ID="txtSearchUser" runat="server" CssClass="mediumtextbox" MaxLength="255" />
	    <portal:CButton ID="btnSearchUser" runat="server" />
		<asp:HyperLink ID="lnkNewUser" runat="server" Visible="false" /> 
		<span id="spnIPLookup" runat="server" visible="false" class="iplookup">
		    <asp:TextBox ID="txtIPAddress" runat="server" CssClass="mediumtextbox" MaxLength="50" />
	        <portal:CButton ID="btnIPLookup" runat="server" />
		</span>
	</div>	
	<div class="modulepager">
	    <asp:HyperLink ID="lnkAllUsers" runat="server" CssClass="ModulePager" />
	    <span id="spnTopPager"  runat="server" ></span>
	</div>
	<div class="AspNet-GridView">		
	<table  cellspacing="0" width="100%">
		<thead>
		<tr>
			<th id='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>' >
				<cy:SiteLabel id="lblUserNameLabel" runat="server" ConfigKey="MemberListUserNameLabel" UseLabelTag="false"> </cy:SiteLabel>
			</th>
			<th  id='<%# Resources.Resource.MemberListDateCreatedLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="SiteLabel2" runat="server" ConfigKey="MemberListDateCreatedLabel" UseLabelTag="false"> </cy:SiteLabel>
			</th>
			<th  id="thWebLink" runat="server" >
				<cy:SiteLabel id="lblUserWebSite" runat="server" ConfigKey="MemberListUserWebSiteLabel" UseLabelTag="false"> </cy:SiteLabel>
			</th>
			<th  id='<%# Resources.Resource.MemberListUserTotalPostsLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="lblTotalPosts" runat="server" ConfigKey="MemberListUserTotalPostsLabel" UseLabelTag="false"> </cy:SiteLabel>
			</th>
			<th></th>
		</tr></thead><tbody>
		<asp:Repeater id="rptUsers" runat="server" EnableViewState="False">
			<ItemTemplate>
				<tr >
					<td headers='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>'>
						
						<strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%></strong>
						<div runat="server" visible='<%# Cynthia.Web.WebConfigSettings.ShowEmailInMemberList %>'>
						&nbsp;<a href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
						 </div>
						 <div runat="server" visible='<%# Cynthia.Web.WebConfigSettings.ShowLoginNameInMemberList %>'>
						 &nbsp;<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
						 </div>
						 <div id="Div1" runat="server" visible='<%# Cynthia.Web.WebConfigSettings.ShowUserIDInMemberList %>'>
						 &nbsp;<cy:SiteLabel id="lblTotalPosts" runat="server" ConfigKey="RegisterLoginNameLabel" UseLabelTag="false"> </cy:SiteLabel>
						 <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "UserID").ToString())%>
						 </div>
						 <div id="Div4" runat="server" visible='<%# (IsAdmin && (!Convert.ToBoolean(Eval("DisplayInMemberList")))) %>' class="floatrightimage isvisible">
						 <%# Resources.Resource.HiddenUser%>
						 </div>
						 
					</td>
					<td headers='<%# Resources.Resource.MemberListDateCreatedLabel.Replace(" ", "") %>'>
					    <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DateCreated")).ToShortDateString()%>
					</td>
					<td id="tdWebLink" runat="server" visible='<%# ShowWebSiteColumn %>' >
						<a rel="nofollow"  href='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"WebSiteUrl").ToString()) %>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "WebSiteUrl").ToString())%></a>
					</td>
					<td id="tdForumPosts" runat="server" visible='<%# ShowForumPostColumn %>' headers='<%# Resources.Resource.MemberListUserTotalPostsLabel.Replace(" ", "") %>'>
						<%# DataBinder.Eval(Container.DataItem,"TotalPosts") %>
						<portal:ForumUserThreadLink id="lnkUserPosts" runat="server" UserId='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"UserID")) %>' TotalPosts='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TotalPosts")) %>' />
					</td>
					<td>
						<a href='<%# SiteRoot + "/ProfileView.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID") %>'><cy:SiteLabel id="lblViewProfile" runat="server" ConfigKey="MemberListViewProfileLabel" UseLabelTag="false"> </cy:SiteLabel></a>&nbsp;&nbsp;
					    <asp:HyperLink Text='<%# Resources.Resource.ManageUserLink %>' id="Hyperlink2" 
						    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID")   %>' 
						    Visible="<%# canManageUsers %>" runat="server" />
					</td>
				</tr>
			</ItemTemplate>
			<alternatingItemTemplate>
				<tr class="AspNet-GridView-Alternate">
					<td headers='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>'>
						<strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%></strong>
						<div id="Div3" runat="server" visible='<%# Cynthia.Web.WebConfigSettings.ShowEmailInMemberList %>'>
						&nbsp;<a href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					    </div>
					    <div id="Div2" runat="server" visible='<%# Cynthia.Web.WebConfigSettings.ShowLoginNameInMemberList %>'>
						 &nbsp;<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
						 </div>
						 <div id="Div1" runat="server" visible='<%# Cynthia.Web.WebConfigSettings.ShowUserIDInMemberList %>'>
						 &nbsp;<cy:SiteLabel id="lblTotalPosts" runat="server" ConfigKey="RegisterLoginNameLabel" UseLabelTag="false"> </cy:SiteLabel>
						 <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "UserID").ToString())%>
						 </div>
						 <div id="Div4" runat="server" visible='<%# (IsAdmin && (!Convert.ToBoolean(Eval("DisplayInMemberList")))) %>' class="floatrightimage isvisible">
						 <%# Resources.Resource.HiddenUser%>
						 </div>
					</td>
					<td headers='<%# Resources.Resource.MemberListDateCreatedLabel.Replace(" ", "") %>'>
					    <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DateCreated")).ToShortDateString()%>
					</td>
					<td id="tdWebLink2" runat="server" visible='<%# ShowWebSiteColumn %>'>
						<a rel="nofollow" href='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"WebSiteUrl").ToString()) %>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"WebSiteUrl").ToString()) %></a>
					</td>
					<td id="tdForumPosts" runat="server" visible='<%# ShowForumPostColumn %>' headers='<%# Resources.Resource.MemberListUserTotalPostsLabel.Replace(" ", "") %>'>
						<%# DataBinder.Eval(Container.DataItem,"TotalPosts") %>
						<portal:ForumUserThreadLink id="lnkUserPosts" runat="server" UserId='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"UserID")) %>' TotalPosts='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TotalPosts")) %>' />
					</td>
					<td>
						<a href='<%# SiteRoot + "/ProfileView.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID") %>'><cy:SiteLabel id="Sitelabel1" runat="server" ConfigKey="MemberListViewProfileLabel" UseLabelTag="false"> </cy:SiteLabel></a>&nbsp;&nbsp;
					    <asp:HyperLink Text='<%# Resources.Resource.ManageUserLink %>' id="Hyperlink1"  
						    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID")   %>' 
						    Visible="<%# canManageUsers %>" runat="server" />
					</td>
				</tr>
			</AlternatingItemTemplate>
		</asp:Repeater></tbody>
	</table>
	</div>	
	<div class="modulepager">
		<portal:CCutePager ID="pgrMembers" runat="server" />
	</div>
	</div>
	</portal:CPanel>
	<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
