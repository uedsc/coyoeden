<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnlineMemberListControl.ascx.cs" Inherits="Cynthia.Web.UI.OnlineMemberListControl" %>

<asp:Repeater ID="rptOnlineMembers" runat="server">
 <HeaderTemplate>
 <ul class="userstats">
    <li>
        <cy:SiteLabel id="slMembersOnline" runat="server"  ConfigKey="OnlineMembersLabel" UseLabelTag="false" />
    </li>
    <li><ul class="userstats">
 </HeaderTemplate>
 <ItemTemplate>
 <li class="whoson" >
 <%# GetAvatarUrl(DataBinder.Eval(Container.DataItem, "UserID"), DataBinder.Eval(Container.DataItem, "Name").ToString(), DataBinder.Eval(Container.DataItem, "AvatarUrl").ToString())%>
 <cy:Gravatar ID="grav1" runat="server" Email='<%# Eval("Email") %>' MaxAllowedRating='<%# MaxAllowedGravatarRating %>' Visible='<%# allowGravatars %>' Size="60"  /><br />
 <asp:HyperLink Text="Edit" id="Hyperlink2" 
	ImageUrl='<%# Page.ResolveUrl("~/Data/SiteImages/user_edit.png") %>' 
	NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx" + "?userid=" + DataBinder.Eval(Container.DataItem,"UserID") %>' 
	Visible="<%# IsAdmin %>" runat="server" />
 <%# SiteUtils.GetProfileLink(Page, DataBinder.Eval(Container.DataItem,"UserID"),DataBinder.Eval(Container.DataItem,"Name")) %>
 </li>
 </ItemTemplate>
 <FooterTemplate>
 </ul>
 </li>
 </ul>
 </FooterTemplate>
</asp:Repeater>
