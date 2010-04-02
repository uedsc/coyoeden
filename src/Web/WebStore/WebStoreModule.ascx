<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="WebStoreModule.ascx.cs"
    Inherits="WebStore.UI.WebStoreModule" %>
<%@ Register Src="~/WebStore/Controls/CartLink.ascx" TagPrefix="ws" TagName="CartLink" %>
<%@ Register Src="~/WebStore/Controls/ProductListControl.ascx" TagPrefix="ws" TagName="ProductList" %>
<%@ Register Src="~/WebStore/Controls/OfferListControl.ascx" TagPrefix="ws" TagName="OfferList" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper webstore" EnableViewState="false">
    <portal:ModuleTitleControl EditText="Edit" EditUrl="~/SampleModuleEdit.aspx" runat="server"
        ID="TitleControl" EnableViewState="false" />
    <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
    <div class="modulecontent">
        <asp:Panel ID="pnlStore" runat="server" CssClass="store ">
            <div class="settingrow">
                <ws:CartLink ID="lnkCart" runat="server" EnableViewState="false" />
            </div>
            <div class="settingrow">
                <asp:Literal ID="litStoreDescription" runat="server" EnableViewState="false" />
            </div>
            <asp:Panel ID="pnlOfferList" runat="server" CssClass="floatpanel productlist">
                <h3>
                    <asp:Literal ID="litOfferListHeading" runat="server" EnableViewState="false"></asp:Literal></h3>
                    <ws:ProductList id="productList1" runat="server" Visible="true" />
                    <ws:OfferList id="offerList1" runat="server" Visible="false" />
                
            </asp:Panel>
            <asp:Panel ID="pnlSpecials" runat="server" CssClass="floatpanel specials">
                <h3 class="heading specialsheading">
                    <asp:Literal ID="litSpecialsHeading" runat="server" EnableViewState="false"></asp:Literal></h3>
                <asp:Repeater ID="rptSpecials" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <div>
                            <%# Eval("Name") %>
                            <a href='<%# SiteRoot + Eval("Url") %>'>
                                <%# Resources.WebStoreResources.MoreInfoLink %></a>
                            <%# string.Format(currencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %>
                            <a href='<%# SiteRoot + "/WebStore/CartAdd.aspx?offer=" + Eval("Guid") + "&pageid=" + PageId.ToString() + "&mid=" + ModuleId.ToString() %>'>
                                <%# Resources.WebStoreResources.AddToCartLink%></a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnlStoreClosed" runat="server" Visible="false" CssClass="store" EnableViewState="false">
            <asp:Literal ID="litStoredClosed" runat="server" EnableViewState="false" />
        </asp:Panel>
       
    </div>
    <div class="settingrow">&nbsp;<br /><br /></div>
    </portal:CPanel>
    <div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
</portal:CPanel>
