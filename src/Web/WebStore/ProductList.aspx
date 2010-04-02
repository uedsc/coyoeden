<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ProductList.aspx.cs" Inherits="WebStore.UI.ProductListPage" %>

<%@ Register Src="~/WebStore/Controls/CartLink.ascx" TagPrefix="ws" TagName="CartLink" %>
<%@ Register Src="~/WebStore/Controls/ProductListControl.ascx" TagPrefix="ws" TagName="ProductList" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
        <cy:CornerRounderTop ID="ctop1" runat="server" />
        <asp:Panel ID="pnlStoreManager" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoreproductlist">
            <portal:ModuleTitleControl EditText="Edit" EditUrl="~/SampleModuleEdit.aspx" runat="server"
                ID="TitleControl" EnableViewState="false" />
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
                <asp:Panel ID="pnlProductList" runat="server" CssClass="modulecontent">
                    <div class="settingrow">
                        <ws:CartLink ID="lnkCart" runat="server" EnableViewState="false" />
                    </div>
                    <h3><asp:Literal ID="litProductListHeading" runat="server" EnableViewState="false"></asp:Literal></h3>
                    <ws:ProductList id="productList1" runat="server" Visible="true" />
                </asp:Panel>
            </portal:CPanel>
            <div class="cleared">
            </div>
        </asp:Panel>
        <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
