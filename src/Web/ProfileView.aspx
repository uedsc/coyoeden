<%@ Page language="c#" Codebehind="ProfileView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.ProfileView" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<cy:CornerRounderTop id="ctop1" runat="server" />
		<asp:Panel id="pnlProfile" runat="server" CssClass="profileview">
		    <div class="modulecontent">
		        <div class="settingrow">
		            <cy:SiteLabel id="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel" > </cy:SiteLabel>
		            &nbsp;<asp:Label id="lblCreatedDate" runat="server" ></asp:Label>
		        </div>
		        <div id="divGroupPosts" runat="server" class="settingrow">
		            <cy:SiteLabel id="lblTotalPostsLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersTotalPostsLabel" > </cy:SiteLabel>
		            &nbsp;<asp:Label id="lblTotalPosts" runat="server" ></asp:Label>
		            <portal:GroupUserThreadLink id="lnkUserPosts" runat="server"  />
		        </div>
		        <div class="settingrow">
		            <cy:SiteLabel id="lblUserNameLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersUserNameLabel" > </cy:SiteLabel>
		            &nbsp;<asp:Label id="lblUserName" runat="server" ></asp:Label>
		        </div>
		        <div id="divAvatar" runat="server" class="settingrow">
		            <cy:SiteLabel id="lblAvatar" runat="server" CssClass="settinglabel" ConfigKey="UserProfileAvatarLabel" > </cy:SiteLabel>
		            <cy:Gravatar ID="gravatar1" runat="server"   />
		            &nbsp;<img alt="" src="" id="imgAvatar" hspace="15"  runat="server" />
		        </div>
		        <div id="divLiveMessenger" runat="server" visible="false" class="settingrow messengerpanel">
		            <cy:SiteLabel id="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" > </cy:SiteLabel>
		            <portal:LiveMessengerControl ID="chat1" runat="server" SkinID="profile"
		                    Width="400"
		                    Height="300"
                            Invitee=""
                            InviteeDisplayName=""
                            OverrideCulture=""
                            UseTheme="false"
                            ThemName=""
                            
                        />
		        </div>
		        <asp:Panel ID="pnlProfileProperties" runat="server"></asp:Panel>
		        <div style="clear:left;">&nbsp;
		            <asp:Label ID="lblMessage" runat="server" CssClass="txterror" />
		        </div>
		    </div>
		    <div class="modulefooter">&nbsp;</div>
		</asp:Panel>
	<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
