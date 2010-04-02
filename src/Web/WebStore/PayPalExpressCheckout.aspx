<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="PayPalExpressCheckout.aspx.cs" Inherits="WebStore.UI.PayPalExpressCheckoutPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnl1" runat="server" CssClass="art-Post-inner panelwrapper webstore webstorecheckout">
        <h2 class="moduletitle">
            <asp:Literal ID="litCheckoutHeader" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <asp:Panel ID="pnlRequireLogin" runat="server" CssClass="clearpanel " >
                <div><strong><asp:Literal ID="litLoginInstructions" runat="server" /></strong></div>
                <div class="floatpanel">
                    <asp:Literal ID="litLoginPrompt" runat="server" /><br />
                    <asp:HyperLink ID="lnkLogin" runat="server" Text="Login" />
                </div>
                <div class="floatpanel">
                    <asp:Literal ID="litRegisterPrompt" runat="server" /><br />
                    <asp:HyperLink ID="lnkRegister" runat="server" Text="Register" />
                </div>
                <div class="clearpanel">&nbsp;</div>
            </asp:Panel>
            
            <fieldset style="clear:both;">
                <asp:Panel ID="pnlOrderSummary" runat="server" CssClass="floatpanel">
                    <h3 class="moduletitle">
                        <asp:Literal ID="litOrderSummary" runat="server" /></h3>
                    <asp:Repeater ID="rptCartItems" runat="server">
                        <ItemTemplate>
                            <div>
                                <%# Eval("Name") %>
                                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="carttotalwrapper">
                        <asp:Panel ID="pnlSubTotal" runat="server" CssClass="storerow">
                            <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="storelabel" ConfigKey="CartSubTotalLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:Literal ID="litSubTotal" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="pnlShippingTotal" runat="server" CssClass="storerow">
                            <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="storelabel" ConfigKey="CartShippingTotalLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:Literal ID="litShippingTotal" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="pnlTaxTotal" runat="server" CssClass="storerow">
                            <cy:SiteLabel ID="SiteLabel6" runat="server" CssClass="storelabel" ConfigKey="CartTaxTotalLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:Literal ID="litTaxTotal" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="pnlOrderTotal" runat="server" CssClass="storerow">
                            <cy:SiteLabel ID="SiteLabel7" runat="server" CssClass="storelabel" ConfigKey="CartOrderTotalLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:Literal ID="litOrderTotal" runat="server" />
                        </asp:Panel>
                    </div>
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
                        <asp:Image ID="imgPayPal" runat="server" ImageUrl="~/Data/SiteImages/paypal.gif"
                        AlternateText="PayPal" />
                        <asp:Literal ID="litPayPalEmail" runat="server" />
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
                    <div class="settingrow">
                        <portal:CButton ID="btnMakePayment" runat="server" />&nbsp;&nbsp;
                        <br />
                        <portal:CLabel ID="lblMessage" runat="server" CssClass="txterror" />
                    </div>
                </asp:Panel>
            </fieldset>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
