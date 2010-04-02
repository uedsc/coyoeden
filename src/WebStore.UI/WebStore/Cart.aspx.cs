/// Author:					Joe Audette
/// Created:				2007-02-13
/// Last Modified:		    2009-06-03
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web;
using Cynthia.Web.UI;
using Cynthia.Web.Framework;
using Cynthia.Business;
using Cynthia.Business.Commerce;
using Cynthia.Business.WebHelpers.PaymentGateway;
using WebStore.Business;
using Resources;
using WebStore.Helpers;
using GCheckout.Checkout;
using GCheckout.Util;

namespace WebStore.UI
{

    public partial class CartPage : CBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        private Store store;
        private CommerceConfiguration commerceConfig = null;
        private SiteUser siteUser = null;
        private Cart cart;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private bool canCheckoutWithoutAuthentication = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();
            if (!UserCanViewPage(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }
            LoadSettings();

            if ((store == null) || (store.IsClosed))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            SetupCss();
            PopulateLabels();
            ShowCart();

        }

        private void ShowCart()
        {
            if (Page.IsPostBack) { return; }

            if ((store != null)&&(cart != null))
            {
                using (IDataReader reader = cart.GetItems())
                {
                    rptCartItems.DataSource = reader;
                    rptCartItems.DataBind();
                }

                litDiscount.Text = cart.Discount.ToString("c", currencyCulture);
                pnlDiscountAmount.Visible = (cart.Discount > 0);
                litSubTotal.Text = cart.SubTotal.ToString("c", currencyCulture);
                pnlTotal.Visible = pnlDiscountAmount.Visible;
                decimal totalWithoutTax = cart.SubTotal - cart.Discount;
                litTotal.Text = totalWithoutTax.ToString("c", currencyCulture);
                
            }


        }

        void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            if(StoreHelper.ApplyDiscount(store, cart, txtDiscountCode.Text, out errorMessage))
            {
                WebUtils.SetupRedirect(this, Request.RawUrl);
                return;
            }
            lblDiscountError.Text = errorMessage;

        }

        private void SetupPayPalStandardForm()
        {
            if (cart == null) { return; }
            //if (siteUser == null) { return; }

            if (cart.UserGuid == Guid.Empty)
            {
                if (siteUser != null)
                {
                    cart.UserGuid = siteUser.UserGuid;
                    cart.Save();
                }
            }

            //if ((cart.CartOffers.Count == 0)||(cart.OrderTotal == 0))
            //{
            //    litOr.Visible = false;
            //    btnPayPal.Visible = false;
            //    return; 
            //}

            PayPalLog payPalLog = StoreHelper.EnsurePayPalStandardCheckoutLog(cart, store, SiteRoot, pageId, moduleId);
            if (payPalLog == null) return;

            litPayPalFormVariables.Text = StoreHelper.GetCartUploadFormFields(
                payPalLog.RowGuid,
                cart,
                store,
                commerceConfig);

            btnPayPal.PostBackUrl = commerceConfig.PayPalStandardUrl;
        }

        void btnPayPal_Click(object sender, ImageClickEventArgs e)
        {
            if (commerceConfig.PayPalUsePayPalStandard)
            {
                DoPayPalStandardCheckout();
            }
            else
            {
                DoPayPalExpressCeckout();
            }
        }

        

        private void DoPayPalExpressCeckout()
        {
            PayPalExpressGateway gateway
                = new PayPalExpressGateway(
                    commerceConfig.PayPalAPIUsername,
                    commerceConfig.PayPalAPIPassword,
                    commerceConfig.PayPalAPISignature,
                    commerceConfig.PayPalStandardEmailAddress);

            gateway.UseTestMode = commerceConfig.PaymentGatewayUseTestMode;

            gateway.MerchantCartId = cart.CartGuid.ToString();
            gateway.ChargeTotal = cart.OrderTotal;

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            gateway.ReturnUrl = siteRoot + "/Services/PayPalReturnHandler.ashx";
            gateway.CancelUrl = siteRoot + Request.RawUrl;
            //Currency currency = new Currency(store.DefaultCurrencyId);
            //gateway.CurrencyCode = currency.Code;
            gateway.CurrencyCode = siteSettings.GetCurrency().Code;
            gateway.OrderDescription = store.Name + " " + WebStoreResources.OrderHeading;

            gateway.BuyerEmail = cart.OrderInfo.CustomerEmail;
            gateway.ShipToFirstName = cart.OrderInfo.DeliveryFirstName;
            gateway.ShipToLastName = cart.OrderInfo.DeliveryLastName;
            gateway.ShipToAddress = cart.OrderInfo.DeliveryAddress1;
            gateway.ShipToAddress2 = cart.OrderInfo.DeliveryAddress2;
            gateway.ShipToCity = cart.OrderInfo.DeliveryCity;
            gateway.ShipToState = cart.OrderInfo.DeliveryState;
            gateway.ShipToCountry = cart.OrderInfo.DeliveryCountry;
            gateway.ShipToPostalCode = cart.OrderInfo.DeliveryPostalCode;
            gateway.ShipToPhone = cart.OrderInfo.CustomerTelephoneDay;

            // this tells paypal to use the shipping address we pass in
            // rather than what the customer has on file
            // when we implement shippable products we'll do shipping calculations before
            // sending the user to paypal
            //gateway.OverrideShippingAddress = true;

            //commented out the above, we want user to be able to populate shipping info from their paypal account

            bool executed = gateway.CallSetExpressCheckout();
            if (executed)
            {
                //TODO: log the raw response
                if (gateway.PayPalExpressUrl.Length > 0)
                {
                    // record the gateway.PayPalToken
                    PayPalLog payPalLog = new PayPalLog();
                    payPalLog.RawResponse = gateway.RawResponse;
                    payPalLog.ProviderName = "WebStorePayPalHandler";
                    payPalLog.ReturnUrl = siteRoot + Request.RawUrl;
                    payPalLog.Token = HttpUtility.UrlDecode(gateway.PayPalToken);
                    payPalLog.RequestType = "SetExpressCheckout";
                    
                    cart.SerializeCartOffers();
                    payPalLog.SerializedObject = SerializationHelper.SerializeToString(cart);

                    payPalLog.CartGuid = cart.CartGuid;
                    payPalLog.SiteGuid = store.SiteGuid;
                    payPalLog.StoreGuid = store.Guid;
                    payPalLog.UserGuid = cart.UserGuid;

                    payPalLog.Save();

                    Response.Redirect(gateway.PayPalExpressUrl);

                }
                
            }
            
        }

        /// <summary>
        /// This is really a fallback method, it will not be executed in normal use because we are setting the postbackurl of the button
        /// to post directly to paypal, so this method should not fire unless somehow the page is manipulated to postback to itself.
        /// In this case, we just consolidate the cart into a buy now button.
        /// </summary>
        private void DoPayPalStandardCheckout()
        {
            PayPalLog payPalLog = new PayPalLog();

            payPalLog.ProviderName = "WebStorePayPalHandler";
            payPalLog.PDTProviderName = "WebStorePayPalPDTHandlerProvider";
            payPalLog.IPNProviderName = "WebStorePayPalIPNHandlerProvider";
            payPalLog.ReturnUrl = SiteRoot +
                                "/WebStore/OrderDetail.aspx?pageid="
                                + pageId.ToString(CultureInfo.InvariantCulture)
                                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                                + "&orderid=" + cart.CartGuid.ToString();

            payPalLog.RequestType = "StandardCheckout";

            cart.SerializeCartOffers();
            payPalLog.SerializedObject = SerializationHelper.SerializeToString(cart);
            payPalLog.CartGuid = cart.CartGuid;
            payPalLog.SiteGuid = store.SiteGuid;
            payPalLog.StoreGuid = store.Guid;
            payPalLog.UserGuid = cart.UserGuid;
            payPalLog.CartTotal = cart.OrderTotal;
            payPalLog.CurrencyCode = siteSettings.GetCurrency().Code;

            payPalLog.Save();

            string payPalStandardUrl = StoreHelper.GetBuyNowUrl(
                payPalLog.RowGuid,
                cart,
                store,
                commerceConfig);


            WebUtils.SetupRedirect(this, payPalStandardUrl);

        }

        private void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (cart == null) { return; }

            string strGuid = e.CommandArgument.ToString();
            if (strGuid.Length != 36) { return; }

            Guid itemGuid = new Guid(strGuid);

            switch (e.CommandName)
            {
                case "updateQuantity":

                    int quantity = 1;
                    TextBox txtQty = e.Item.FindControl("txtQuantity") as TextBox;
                    if (txtQty != null)
                    {
                        try
                        {
                            int.TryParse(txtQty.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out quantity);
                        }
                        catch (ArgumentException) { }
                    }
                    cart.UpdateCartItemQuantity(itemGuid, quantity);

                    break;

                case "delete":

                    cart.DeleteItem(itemGuid);
                    cart.ResetCartOffers();
                    cart.RefreshTotals();
                    cart.Save();
                   
                    break;

            }

            StoreHelper.EnsureValidDiscounts(store, cart);

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        void btnGoogleCheckout_Click(object sender, ImageClickEventArgs e)
        {
            if (
                (store != null)
                && (cart != null)
                )
            { //&& (IsValidForCheckout()) ?

                int cartTimeoutInMinutes = 30;
                
                CheckoutShoppingCartRequest Req = new CheckoutShoppingCartRequest(
                    commerceConfig.GoogleMerchantID,
                    commerceConfig.GoogleMerchantKey,
                    commerceConfig.GoogleEnvironment,
                    siteSettings.GetCurrency().Code,
                    cartTimeoutInMinutes);


                foreach (CartOffer cartOffer in cart.CartOffers)
                {

                    Req.AddItem(
                        cartOffer.Name,
                        string.Empty,
                        cartOffer.OfferPrice,
                        cartOffer.Quantity);
                }

                //Req.AddMerchantCalculatedShippingMethod
                //Req.AnalyticsData
                //Req.ContinueShoppingUrl
                //Req.EditCartUrl

                //Req.RequestInitialAuthDetails
                //Req.AddParameterizedUrl

                // we need to serialize the cart and it items to xml here
                // so when we get it back from google
                // we can validate against the existing cart
                // as its possible items were added to the cart
                // after we passed the user to google

                //Req.MerchantPrivateData = cart.CartGuid.ToString();
                //cart.SerializeCartOffers();
                //Req.MerchantPrivateData = SerializationHelper.SerializeToSoap(cart);

                cart.SerializeCartOffers();
                MerchantData merchantData = new MerchantData();
                merchantData.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
                merchantData.SerializedObject = SerializationHelper.RemoveXmlDeclaration(SerializationHelper.SerializeToString(cart));
                Req.MerchantPrivateData = SerializationHelper.RemoveXmlDeclaration(SerializationHelper.SerializeToString(merchantData));
                Req.RequestBuyerPhoneNumber = true;

                // flat rate shipping example
                //Req.AddFlatRateShippingMethod("UPS Ground", 5);

                //Add a rule to tax all items at 7.5% for Ohio
                //Req.AddStateTaxRule("NC", .15, true);
                //TODO: lookup tax 

                GCheckoutResponse Resp = Req.Send();

                if (Resp.IsGood)
                {
                    Response.Redirect(Resp.RedirectUrl, true);
                }
                else
                {
                    lblMessage.Text = Resp.ErrorMessage;
                }
            }

        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                //crumbs.AddedCrumbs = "<a href='"
                //    + Page.ResolveUrl("~/WebStore/AdminDashboard.aspx?pageid=" + PageID.ToString() + "&mid=" + ModuleID.ToString())
                //    + "'>" + WebStoreResources.StoreManagerLink
                //    + "</a>";
            }
            Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + WebStoreResources.CartHeader);
            litCartHeader.Text = WebStoreResources.CartHeader;

            lnkCheckout.Text = "Proceed To Checkout";
            lnkCheckout.NavigateUrl = Page.ResolveUrl(
                "~/WebStore/ConfirmOrder.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture));

            lnkKeepShopping.Text = WebStoreResources.CartKeepShoppingLink;
            lnkKeepShopping.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            litOr.Text = WebStoreResources.LiteralOr;
            btnApplyDiscount.Text = WebStoreResources.ApplyDiscountButton;

            lblDiscountError.Text = string.Empty;
        }

        private void LoadParams()
        {
            
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, moduleId);


        }

        private void ConfigureCheckoutButtons()
        {
            bool shouldShowPayPal = ShouldShowPayPal();
            bool shouldShowGoogle = ShouldShowGoogle();

            btnPayPal.Visible = shouldShowPayPal;
            btnGoogleCheckout.Visible = shouldShowGoogle;
            litOr.Visible = (shouldShowPayPal || shouldShowGoogle);

            if (shouldShowGoogle)
            {
                if ((!commerceConfig.Is503TaxExempt)&&(cart.HasDonations()))
                {
                    lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
                    lblGoogleMessage.Visible = true;
                    btnGoogleCheckout.Enabled = false;
                    if (!Request.IsAuthenticated)
                    {
                        PaymentAcceptanceMark mark = (PaymentAcceptanceMark)pam1;
                        mark.SuppressGoogleCheckout = true;
                    }
                }
                
            }

            if((shouldShowPayPal)&&(commerceConfig.PayPalUsePayPalStandard))
            {
                SetupPayPalStandardForm();
            }
            //if we can't process cards internally there is no reason to go to the ConfirmOrder.aspx page 
            //and the order can be processed wtithout the user signing in or if the user is already signed in
            if (
                (!commerceConfig.CanProcessStandardCards)
                && ((Request.IsAuthenticated) || (canCheckoutWithoutAuthentication))
                )
            {
                lnkCheckout.Visible = false;
            }

           
        }

        

        private void LoadSettings()
        {
            commerceConfig = SiteUtils.GetCommerceConfig();
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            if (Request.IsAuthenticated)
            {
                siteUser = SiteUtils.GetCurrentSiteUser();
            }

            if (CurrentPage.ContainsModule(moduleId))
            {
                store = StoreHelper.GetStore();
            }

            if (store == null)
            {
                lnkCheckout.Visible = false;
                pnlDiscountCode.Visible = false;
                return;
            }

            if (
                (StoreHelper.UserHasCartCookie(store.Guid))
                || (Request.IsAuthenticated)
                )
            {
                cart = StoreHelper.GetCart();
            }

            if (cart == null) 
            {
                pnlDiscountCode.Visible = false; 
                lnkCheckout.Visible = false;
                return; 
            }

            if ((cart.LastModified < DateTime.UtcNow.AddDays(-1)) && (cart.DiscountCodesCsv.Length > 0))
            {
                StoreHelper.EnsureValidDiscounts(store, cart);
            }

            if (store != null)
            {
                canCheckoutWithoutAuthentication = store.CanCheckoutWithoutAuthentication(cart);
            }

            // disable till I finish
            //canCheckoutWithoutAuthentication = false;

            ConfigureCheckoutButtons();

            int countOfDiscountCodes = Discount.GetCountOfActiveDiscountCodes(store.ModuleGuid);
            pnlDiscountCode.Visible = (countOfDiscountCodes > 0);

            // don't show the discount code panel if the cart is empty
            if (cart.SubTotal == 0) 
            {
                lnkCheckout.Visible = false;
                //litOr.Visible = false;
                //btnPayPal.Visible = false;
                //btnGoogleCheckout.Visible = false;
                pnlDiscountCode.Visible = false; 
            }

            // kill switch to disable discount codes (doesn't prevent use of ones already in the cart but prevents new uses
            bool disableDiscounts = false;
            ConfigHelper.GetBoolProperty("WebStoreDisabledDiscounts", disableDiscounts);
            if (disableDiscounts) { pnlDiscountCode.Visible = false; }
                
          
            //if (!Page.IsPostBack)
            //{
            //    if ((commerceConfig.PayPalIsEnabled) && (commerceConfig.PayPalUsePayPalStandard))
            //    {
            //        if (Request.IsAuthenticated)
            //        {
            //            siteUser = SiteUtils.GetCurrentSiteUser();
            //            SetupPayPalStandardForm();
            //        }
            //        else
            //        {
            //            //TODO: if the cart has no download items allow checkout without registration/sign in

            //            // we need the user to be signed in before we send them to paypal if using PayPal Standard
            //            // because we want to return them to their order summary and that requires login
            //            // so we need to know who the user is before sending them to PayPal
            //            litOr.Visible = false;
            //            btnPayPal.Visible = false;
            //            btnGoogleCheckout.Visible = false;
            //        }
            //    }
            //}

            


            //if (!Request.IsAuthenticated)
            //{
                

            //    if (commerceConfig.GoogleCheckoutIsEnabled)
            //    {
            //        if (
            //        (!commerceConfig.Is503TaxExempt)
            //        && (cart != null)
            //        && (cart.HasDonations())
            //        )
            //        {
            //            lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
            //            lblGoogleMessage.Visible = true;
            //            PaymentAcceptanceMark mark = (PaymentAcceptanceMark)pam1;
            //            mark.SuppressGoogleCheckout = true;

            //            btnGoogleCheckout.Visible = true;
            //            btnGoogleCheckout.Enabled = false;
            //        }
            //    }
            //}
            //else
            //{
            //    if (
            //        (!commerceConfig.Is503TaxExempt)
            //        && (cart != null)
            //        && (cart.HasDonations())
            //        && (commerceConfig.GoogleCheckoutIsEnabled)
            //        )
            //    {
            //        btnGoogleCheckout.Visible = true;
            //        btnGoogleCheckout.Enabled = false;
            //        lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
            //        lblGoogleMessage.Visible = true;
            //    }


            //}

            
            

        }

        private bool ShouldShowPayPal()
        {
            if (store == null) { return false; }
            if (cart == null) { return false; }
            if (cart.SubTotal == 0) { return false; }
            if (cart.OrderTotal == 0) { return false; }
            if (commerceConfig == null) { return false; }
            if ((!Request.IsAuthenticated) && (!canCheckoutWithoutAuthentication)) { return false; }
            if (!commerceConfig.PayPalIsEnabled) { return false; }



            return true;
        }

        private bool ShouldShowGoogle()
        {
            if (store == null) { return false; }
            if (cart == null) { return false; }
            if (cart.SubTotal == 0) { return false; }
            if (cart.OrderTotal == 0) { return false; }
            if (commerceConfig == null) { return false; }
            if ((!Request.IsAuthenticated) && (!canCheckoutWithoutAuthentication)) { return false; }
            if (!commerceConfig.GoogleCheckoutIsEnabled) { return false;}

            return true;
        }

        protected virtual void SetupCss()
        {
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("stylewebstore") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "stylewebstore";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl()
                    + "stylewebstore.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }
            
        }


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            this.rptCartItems.ItemCommand += new RepeaterCommandEventHandler(rptCartItems_ItemCommand);

            btnPayPal.Click += new ImageClickEventHandler(btnPayPal_Click);
            btnGoogleCheckout.Click += new ImageClickEventHandler(btnGoogleCheckout_Click);
            btnApplyDiscount.Click += new EventHandler(btnApplyDiscount_Click);

            SuppressPageMenu();
            SuppressGoogleAds();
        }

        

        

        

        #endregion
    }
}
