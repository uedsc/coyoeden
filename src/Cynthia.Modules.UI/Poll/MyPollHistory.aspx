<%@ Page ValidateRequest="false" Language="c#" MaintainScrollPositionOnPostback="true"
    Codebehind="MyPollHistory.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="PollFeature.UI.MyPollHistory" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPollHistory" CssClass="selectedcrumb"></asp:HyperLink>
</div>
<cy:CornerRounderTop id="ctop1" runat="server" />
 <asp:Panel ID="pnlBlog" runat="server" CssClass="panelwrapper poll">
 <h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2>       
 <div class="modulecontent">       
<portal:CDataList ID="dlPolls" runat="server" DataKeyField="PollGuid">
    <ItemTemplate>
        <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("Question") %>' Font-Bold="true" />
        <portal:CDataList ID="dlResults" runat="server" DataKeyField="OptionGuid">
            <ItemTemplate>
                <asp:Label ID="lblOption" runat="server" Text='<%# GetOptionResultText(Eval("OptionGuid")) %>'></asp:Label>
                <br />
                <span id="spnResultImage" runat="server"></span>
            </ItemTemplate>
        </portal:CDataList>
        <br />
        <asp:Label ID="lblActive" runat="server" Text='<%# GetActiveText(Eval("ActiveFrom"), Eval("ActiveTo")) %>'></asp:Label>
        <br />
        <cy:SiteLabel ID="lblYouVoted" runat="server" ConfigKey="PollHistoryYouVotedLabel"
            ResourceFile="PollResources" UseLabelTag="false"> </cy:SiteLabel>
        &nbsp;<asp:Label ID="lblAnswer" runat="server" Text='<%# Eval("Answer") %>'></asp:Label>
        <hr />
    </ItemTemplate>
</portal:CDataList>
</div>    
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
