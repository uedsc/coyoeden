<%@ Control Language="C#" AutoEventWireup="false" Codebehind="PollModule.ascx.cs"
    Inherits="PollFeature.UI.PollModule" EnableViewState="true" %>
    
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server"  CssClass="art-Post-inner panelwrapper poll">
    <portal:ModuleTitleControl ID="moduleTitle" runat="server" />
    <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
    <asp:Panel ID="pnlPoll" runat="server" CssClass="modulecontent">
        <asp:Label ID="lblQuestion" runat="server"></asp:Label>
        <asp:UpdatePanel ID="pnlPollUpdate" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:RadioButtonList ID="rblOptions" runat="server" DataTextField="Answer" DataValueField="OptionGuid"
                    AutoPostBack="true" EnableViewState="true">
                </asp:RadioButtonList>
                <portal:CDataList ID="dlResults" runat="server" DataKeyField="OptionGuid">
                    <ItemTemplate>
                        <asp:Label ID="lblOption" runat="server" 
                        Text='<%# GetOptionResultText(Eval("Order"), Eval("Answer"), Eval("Votes")) %>'></asp:Label>
                        <br /><span id="spnResultImage" runat="server"></span>
                       
                    </ItemTemplate>
                </portal:CDataList>
                <asp:Repeater ID="rptResults" runat="server" >
                <HeaderTemplate><div class="AspNet-RadioButtonList"><ul></HeaderTemplate>
                <ItemTemplate>
                        <li class="AspNet-RadioButtonList-Item"><asp:Label ID="lblOption" runat="server" 
                        Text='<%# GetOptionResultText(Eval("Order"), Eval("Answer"), Eval("Votes")) %>'></asp:Label>
                        <br /><span id="spnResultImage" runat="server"></span>
                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("OptionGuid")%>' />
                        </li>
                    </ItemTemplate>
                    <FooterTemplate></ul></div></FooterTemplate>
                </asp:Repeater>
                
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblVotingStatus" runat="server" />
                <asp:Button ID="btnShowResults" runat="server" CssClass="buttonlink"></asp:Button>
                <asp:Button ID="btnBackToVote" runat="server" CssClass="buttonlink" Visible="false"></asp:Button>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:HyperLink ID="lnkMyPollHistory" runat="server" />
    </asp:Panel>
    </portal:CPanel>
    <div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:CPanel>
