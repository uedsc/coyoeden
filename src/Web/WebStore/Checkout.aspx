<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Checkout.aspx.cs" Inherits="WebStore.UI.CheckoutPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlCheckout" runat="server" CssClass="art-Post-inner panelwrapper webstore webstorecheckout">
        <h2 class="moduletitle"><asp:Literal ID="litCheckoutHeader" runat="server" /></h2>
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent" style="min-height: 350px;">
            <asp:Panel ID="pnlPayment" runat="server" CssClass="floatpanel">
            <asp:Panel ID="pnlOrderSummary" runat="server" >
                <h3 class="moduletitle">
                    <asp:Literal ID="litOrderSummary" runat="server" /></h3>
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
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Name")%></td>
                                <td><%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%></td>
                                <td><%# Eval("Quantity") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                </asp:Repeater>
                <div class="carttotalwrapper">
                    <asp:Panel ID="pnlSubTotal" runat="server" CssClass="settingrowtight storerow">
                        <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartSubTotalLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:Literal ID="litSubTotal" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlDiscount" runat="server" CssClass="settingrowtight storerow">
                            <cy:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartDiscountTotalLabel"
                                ResourceFile="WebStoreResources" />
                        <asp:Literal ID="litDiscount" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlShippingTotal" runat="server" CssClass="settingrowtight storerow">
                        <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartShippingTotalLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:Literal ID="litShippingTotal" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlTaxTotal" runat="server" CssClass="settingrowtight storerow">
                        <cy:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartTaxTotalLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:Literal ID="litTaxTotal" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlOrderTotal" runat="server" CssClass="settingrowtight storerow">
                        <cy:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartOrderTotalLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:Literal ID="litOrderTotal" runat="server" />
                    </asp:Panel>
                </div>
            </asp:Panel>
            
                <fieldset id="frmCardInput" runat="server" class="clear">
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCardTypeGuid" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardTypeLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddCardType" runat="server">
                            <asp:ListItem Value="Visa" Selected="true">Visa</asp:ListItem>
                            <asp:ListItem Value="MasterCard">MasterCard</asp:ListItem>
                            <asp:ListItem Value="AMEX">AMEX</asp:ListItem>
                            <asp:ListItem Value="Discover">Discover</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCardOwner" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardOwnerFirstNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardOwnerFirstName" Columns="20" runat="server" MaxLength="100" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardOwnerLastNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardOwnerLastName" Columns="20" runat="server" MaxLength="100" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCardNumber" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardNumberLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardNumber" Columns="20" runat="server" MaxLength="255" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCardExpires" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardExpiresLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddExpireMonth" runat="server">
                            <asp:ListItem Value="01" Text="01"></asp:ListItem>
                            <asp:ListItem Value="02" Text="02"></asp:ListItem>
                            <asp:ListItem Value="03" Text="03"></asp:ListItem>
                            <asp:ListItem Value="04" Text="04"></asp:ListItem>
                            <asp:ListItem Value="05" Text="05"></asp:ListItem>
                            <asp:ListItem Value="06" Text="06"></asp:ListItem>
                            <asp:ListItem Value="07" Text="07"></asp:ListItem>
                            <asp:ListItem Value="08" Text="08"></asp:ListItem>
                            <asp:ListItem Value="09" Text="09"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            <asp:ListItem Value="11" Text="11"></asp:ListItem>
                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddExpireYear" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCardSecurityCode" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardSecurityCodeLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardSecurityCode" Columns="10" runat="server" MaxLength="50" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                        <portal:CButton ID="btnMakePayment" runat="server" />&nbsp;&nbsp;
                        <br />
                        <asp:Label ID="lblMessage" runat="server" CssClass="txterror"></asp:Label>
                    </div>
                </fieldset>
                <portal:CGCheckoutButton ID="btnGoogleCheckout" runat="server" /> 
                <asp:ImageButton ID="btnPayPal" runat="server" ImageUrl="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" />
                <asp:Literal ID="litPayPalFormVariables" runat="server" />
                
            </asp:Panel>
            
            <asp:Panel ID="pnlAddress" runat="server" CssClass="floatpanel">
            <asp:Panel ID="pnlBillingAddress" runat="server">
                    <h3>
                        <asp:Literal ID="litBillingHeader" runat="server" /></h3>
                    <asp:Literal ID="litBillingName" runat="server" />
                    <asp:Literal ID="litBillingCompany" runat="server" />
                    <asp:Literal ID="litBillingAddress1" runat="server" />
                    <asp:Literal ID="litBillingAddress2" runat="server" />
                    <asp:Literal ID="litBillingSuburb" runat="server" />
                    <asp:Literal ID="litBillingCity" runat="server" />
                    <asp:Literal ID="litBillingState" runat="server" />
                    <asp:Literal ID="litBillingPostalCode" runat="server" />
                    <asp:Literal ID="litBillingCountry" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlShippingAddress" runat="server" Visible="false">
                    <h3>
                        <asp:Literal ID="litShippingHeader" runat="server" /></h3>
                    <asp:Literal ID="litShippingName" runat="server" />
                    <asp:Literal ID="litShippingCompany" runat="server" />
                    <asp:Literal ID="litShippingAddress1" runat="server" />
                    <asp:Literal ID="litShippingAddress2" runat="server" />
                    <asp:Literal ID="litShippingSuburb" runat="server" />
                    <asp:Literal ID="litShippingCity" runat="server" />
                    <asp:Literal ID="litShippingState" runat="server" />
                    <asp:Literal ID="litShippingPostalCode" runat="server" />
                    <asp:Literal ID="litShippingCountry" runat="server" />
                </asp:Panel>
            </asp:Panel>
            <div class="settingrow">
            <portal:CommerceTestModeWarning ID="commerceWarning" runat="server" />
            <br /><asp:Label ID="lblGoogleMessage" runat="server" CssClass="txterror" Visible="false"></asp:Label>
            </div>
            <div class="settingrow">
            <portal:InsecurePageWarning ID="InsecurePageWarning1" runat="server" />
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
