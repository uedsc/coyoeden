<%@ Page ValidateRequest="false" Language="c#" MaintainScrollPositionOnPostback="true"
    CodeBehind="PollChoose.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="PollFeature.UI.PollChoose" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink>
        &gt;
        <asp:HyperLink runat="server" ID="lnkPolls" CssClass="selectedcrumb"></asp:HyperLink>
    </div>
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlBlog" runat="server" CssClass="art-Post-inner panelwrapper poll">
        <h2 class="moduletitle">
            <asp:Literal ID="litHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <div class="settingrow">
                <asp:Button ID="btnRemoveCurrent" runat="server" CssClass="buttonlink" CausesValidation="false" />
                &nbsp;&nbsp;
                <asp:HyperLink ID="lnkNewPoll" runat="server"></asp:HyperLink>&nbsp;&nbsp;
            </div>
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
                    <asp:Button CommandName="Choose" runat="server" CssClass="buttonlink" ID="btnChoose"
                        Text='<%$ Resources:PollResources, PollChooseChooseAlternateText %>' CommandArgument='<%# Eval("PollGuid") %>' />
                    <asp:HyperLink ID="lnkEdit" runat="server" Text='<%$ Resources:PollResources, PollViewEditAlternateText %>'
                        NavigateUrl='<%# SiteRoot + "/Poll/PollEdit.aspx?PollGuid=" + Eval("PollGuid") + "&pageid=" + pageId + "&mid=" + moduleId %>'></asp:HyperLink>
                    <asp:Button ID="btnCopyToNewPoll" runat="server" CssClass="buttonlink" CommandName="Copy" CommandArgument='<%# Eval("PollGuid") %>'
                        Text='<%$ Resources:PollResources, PollViewCopyToNewPollButton %>' ToolTip='<%$ Resources:PollResources, PollViewCopyToNewPollToolTip %>' />
                    <hr />
                </ItemTemplate>
            </portal:CDataList>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
