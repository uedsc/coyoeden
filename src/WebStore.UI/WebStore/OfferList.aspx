<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="OfferList.aspx.cs" Inherits="WebStore.UI.OfferListPage" %>

<%@ Register Src="~/WebStore/Controls/CartLink.ascx" TagPrefix="ws" TagName="CartLink" %>
<%@ Register Src="~/WebStore/Controls/OfferListControl.ascx" TagPrefix="ws" TagName="OfferList" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
        <cy:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <asp:Panel ID="pnl1" runat="server" CssClass="art-Post-inner panelwrapper webstore">
            <portal:ModuleTitleControl EditText="Edit" EditUrl="~/SampleModuleEdit.aspx" runat="server"
                ID="TitleControl" EnableViewState="false" />
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
                <div class="modulecontent">
                    <div class="settingrow">
                        <ws:CartLink ID="lnkCart" runat="server" EnableViewState="false" />
                    </div>
                    <h3>
                        <asp:Literal ID="litOfferListHeading" runat="server" EnableViewState="false"></asp:Literal></h3>
                    <ws:Offerlist id="offerList1" runat="server" />
                </div>
            </portal:CPanel>
            <div class="cleared">
            </div>
        </asp:Panel>
        <cy:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
