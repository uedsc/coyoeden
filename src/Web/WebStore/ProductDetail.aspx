<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ProductDetail.aspx.cs" Inherits="WebStore.UI.ProductDetailPage" %>
<%@ Register Src="~/WebStore/Controls/CartLink.ascx" TagPrefix="ws" TagName="CartLink" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="settingrow">
   <ws:CartLink ID="lnkCart" runat="server" EnableViewState="false" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<cy:YUIPanel ID="pnlProductDetail" runat="server" CssClass="art-Post-inner hproduct hreview  panelwrapper webstore webstoreofferdetail">
        <h2 class="fn moduletitle"><asp:Literal ID="litHeading" runat="server" EnableViewState="false" /></h2>  
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">  
        <div class="modulecontent productdetail">
            <div class="productratingwrapper">
            <portal:CRating runat="server" ID="Rating"  ShowPrompt="true" />
            </div>
                <asp:Panel ID="pnlOffers" runat="server" CssClass="clearpanel">
                    <asp:Repeater ID="rptOffers" runat="server">
                        <HeaderTemplate>
                        <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="offercontainer">
                                <td>
                                    <%# Eval("ProductListName") %>
                                    <asp:HyperLink ID="lnkOfferDetail" runat="server" EnableViewState="false" Visible='<%# Convert.ToBoolean(Eval("ShowDetailLink")) %>' NavigateUrl='<%# SiteRoot + Eval("Url") %>' Text='<%# Resources.WebStoreResources.OfferDetailLink %>' />
                                </td>
                                <td>
                                <span class="price"><%# string.Format(currencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
                                </td>
                                <td>
                                <asp:TextBox ID="txtQuantity" runat="server" Text="1" Columns="3" />
                                </td>
                                <td>
                                <asp:Button ID="btnAddToCart" runat="server" Text='<%# Resources.WebStoreResources.AddToCartLink%>' CommandName="addToCart" CommandArgument='<%# Eval("Guid") %>' CausesValidation="false" CssClass="addtocartbutton" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <div class="description settingrow" id="divOfferDescription" runat="server" EnableViewState="false">
                    <asp:Literal ID="litDescription" runat="server" EnableViewState="false" />
                </div>
                <div class="settingrow">
                <asp:HyperLink ID="lnkPreview" runat="server" Visible='false'   />
                </div>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </cy:YUIPanel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
