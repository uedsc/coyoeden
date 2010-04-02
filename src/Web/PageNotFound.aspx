<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="PageNotFound.aspx.cs" Inherits="Cynthia.Web.PageNotFoundPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnl1" runat="server" CssClass="panelwrapper ">
        <div>
            <cy:SiteLabel ID="lbl404Message" runat="server" ConfigKey="PageNotFoundMessage" UseLabelTag="false" CssClass="txterror" />
            <cy:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="PageNotFoundPleaseTry" UseLabelTag="false" CssClass="txterror" />
            <asp:HyperLink ID="lnkSiteMap" runat="server" CssClass="txterror" />
            
        </div>
        <asp:Panel ID="pnlGoogle404Enhancement" runat="server">
        <script type="text/javascript">
            var GOOG_FIXURL_LANG = '<%= CultureCode %>';
            var GOOG_FIXURL_SITE = '<%= SiteNavigationRoot %>';
        </script>
        <script type="text/javascript" src="http://linkhelp.clients.google.com/tbproxy/lh/wm/fixurl.js"></script>
        </asp:Panel>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
