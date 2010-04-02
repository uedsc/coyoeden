<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="OfferDetail.aspx.cs" Inherits="WebStore.UI.OfferDetailPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <cy:YUIPanel ID="pnlOfferDetail" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoreofferdetail">
        <h2 class="moduletitle">
            <asp:Literal ID="litHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <div class="settingrow">
                <strong>
                    <asp:Label ID="lblPrice" runat="server" /></strong>
                <asp:HyperLink ID="lnkAddToCart" runat="server" />
                <portal:CGCheckoutButton ID="btnGoogleCheckout" runat="server" Visible="false" /> 
            </div>
            <div class="settingrow" id="divOfferDescription" runat="server">
                <asp:Literal ID="litOfferDescription" runat="server" />
            </div>
            <asp:Panel ID="pnlProducts" runat="server">
                <asp:Repeater ID="rptProducts" runat="server">
                    <ItemTemplate>
                        <div id="divName" runat="server" Visible='<%# offerHasMoreThanOneProduct %>'>
                            <asp:HyperLink ID="lnkPreview" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("Name") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                            <asp:Literal ID="litName" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length == 0) %>' Text='<%# Eval("Name") %>' />
                        </div>
                        <div>
                             <%# Eval("Description") %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </cy:YUIPanel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
