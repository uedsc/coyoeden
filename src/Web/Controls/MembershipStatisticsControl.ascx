<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipStatisticsControl.ascx.cs" Inherits="Cynthia.Web.MembershipStatisticsControl" %>


<ul class="userstats">
        <li>
            <asp:Image ID="imgMembership" runat="server" />
            <cy:sitelabel id="Sitelabel4" runat="server" ConfigKey="SiteStatisticsMembershipLabel" UseLabelTag="false" />
        </li>
        <li>
            <ul class="userstats">
                <li>
                    <asp:Image ID="imgNewToday" runat="server" />
                    <cy:sitelabel id="Sitelabel2" runat="server" ConfigKey="SiteStatisticsNewTodayLabel" UseLabelTag="false" />
                    <asp:Label ID="lblNewUsersToday" runat="server" />
                </li>
                <li>
                    <asp:Image ID="imgNewYesterday" runat="server" />
                    <cy:sitelabel id="Sitelabel3" runat="server" ConfigKey="SiteStatisticsNewYesterdayLabel" UseLabelTag="false" />
                    <asp:Label ID="lblNewUsersYesterday" runat="server" />
                </li>
                <li>
                    <asp:Image ID="imgTotalUsers" runat="server" />
                    <cy:sitelabel id="Sitelabel5" runat="server" ConfigKey="SiteStatisticsTotalUsersLabel" UseLabelTag="false" />
                    <asp:Label ID="lblTotalUsers" runat="server" />
                </li>
                <li>
                    <asp:Image ID="imgNewestMember" runat="server" />
                    <cy:sitelabel id="Sitelabel1" runat="server" ConfigKey="SiteStatisticsNewestMemberLabel" UseLabelTag="false" /><br />&nbsp;&nbsp;
                    <asp:Label ID="lblNewestUser" runat="server" CssClass="username" />
                </li>
            </ul>
        </li>
    </ul>