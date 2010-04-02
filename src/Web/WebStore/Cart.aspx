<%@ Page Language="C#" AutoEventWireup="false"  MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Cart.aspx.cs" Inherits="WebStore.UI.CartPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlStoreManager" runat="server" CssClass="art-Post-inner panelwrapper webstore webstorecart">
        <h2 class="moduletitle">
            <asp:Literal ID="litCartHeader" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <asp:Panel ID="pnlCartItems" runat="server" CssClass="cart">
                <asp:Repeater ID="rptCartItems" runat="server">
                    <HeaderTemplate>
                        <table class="cartgrid">
                            <tr>
                                <th>
                                    <%# Resources.WebStoreResources.CartItemsHeading%>
                                </th>
                                <th>
                                    <%# Resources.WebStoreResources.CartPriceHeading%>
                                </th>
                                <th>
                                    <%# Resources.WebStoreResources.CartQuantityHeading%>
                                </th>
                                <th>&nbsp;</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Name") %></td>
                            <td><%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%></td>
                            <td><asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' Columns="4" /></td>
                            <td>
                                <portal:CButton ID="btnUpdateQuantity" runat="server" Text='<%# Resources.WebStoreResources.UpdateQuantityButton %>' CommandName="updateQuantity" CommandArgument='<%# Eval("ItemGuid") %>' CausesValidation="false" CssClass="cartbutton" />
                                <portal:CButton ID="btnDelete" runat="server" CssClass="cartbutton" CommandArgument='<%# Eval("ItemGuid") %>'
                                    CommandName="delete" Text='<%# Resources.WebStoreResources.DeleteCartItemButton %>' CausesValidation="false" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </table>
                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
            <hr />
            <asp:Panel ID="pnlSubTotal" runat="server" CssClass="settingrowtight  carttotalwrapper storerow">
                <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartSubTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litSubTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlDiscountAmount" runat="server" CssClass="settingrowtight carttotalwrapper storerow">
                <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartDiscountTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litDiscount" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlTotal" runat="server" CssClass="settingrowtight carttotalwrapper storerow">
                <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litTotal" runat="server" />
            </asp:Panel>
           
            <asp:Panel ID="pnlDiscountCode" runat="server" CssClass="settingrow">   
                <cy:SiteLabel ID="SiteLabel4" runat="server" CssClass="storelabel" ConfigKey="CartDiscountCodeLabel" ResourceFile="WebStoreResources" />
                <asp:TextBox ID="txtDiscountCode" runat="server" />
                <portal:CButton ID="btnApplyDiscount" runat="server"  />
                <asp:Label ID="lblDiscountError" runat="server" CssClass="txterror" />        
             </asp:Panel>
            
            <div class="settingrow checkoutlinks">
                &nbsp;<asp:HyperLink ID="lnkCheckout" runat="server" CssClass="checkoutlink" />
                &nbsp;<asp:HyperLink ID="lnkKeepShopping" runat="server" CssClass="keepshopping" />
            </div>
            <div class="settingrow">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="litOr" runat="server" Visible="false" />
            </div>
            <div class="settingrow">
                <asp:ImageButton ID="btnPayPal" runat="server" ImageUrl="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif"
                    AlternateText="Checkout with PayPal" Visible="false" />
                    <portal:CGCheckoutButton ID="btnGoogleCheckout" runat="server" Visible="false" /><br />
                    <asp:Label ID="lblMessage" runat="server" CssClass="txterror"></asp:Label>
                <asp:Literal ID="litPayPalFormVariables" runat="server" />
            </div>
            <div class="settingrow">
            <asp:Label ID="lblGoogleMessage" runat="server" CssClass="txterror" Visible="false"></asp:Label> 
            </div>
            <portal:CommerceTestModeWarning ID="commerceWarning" runat="server" />
            <div class="clearpanel">
                <portal:PaymentAcceptanceMark ID="pam1" runat="server" />
            </div>
            
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
