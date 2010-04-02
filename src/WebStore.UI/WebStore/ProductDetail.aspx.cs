﻿/// Author:					Joe Audette
/// Created:				2008-10-19
/// Last Modified:			2009-07-04
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Cynthia.Business;
using WebStore.Business;
using Resources;
using WebStore.Helpers;

namespace WebStore.UI
{
    public partial class ProductDetailPage : CBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Guid productGuid = Guid.Empty;
        private Store store = null;
        private CommerceConfiguration commerceConfig;
        private Product product;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private bool enableRatingComments = false;
        protected string teaserFileBaseUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();
            if (!UserCanViewPage(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            if ((store == null)||(store.IsClosed))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }
           

            SetupCss();
            PopulateLabels();
            if (!Page.IsPostBack)
            {
                PopulateControls();
            }

        }

        private void PopulateControls()
        {
            if (product == null) { return; }

            litHeading.Text = Server.HtmlEncode(product.Name);
            litDescription.Text = product.Description;
            MetaDescription = product.MetaDescription;
            MetaKeywordCsv = product.MetaKeywords;
            AdditionalMetaMarkup = product.CompiledMeta;

            if (product.TeaserFile.Length > 0)
            {
                lnkPreview.Text = product.TeaserFileLink;
                lnkPreview.NavigateUrl = teaserFileBaseUrl + product.TeaserFile;
                lnkPreview.Visible = true;
            }

            if (product.EnableRating)
            {
                ((CRating)Rating).ContentGuid = product.Guid;
                ((CRating)Rating).AllowFeedback = enableRatingComments;
            }

            DataTable dataTable = Offer.GetByProduct(product.Guid);
            rptOffers.DataSource = dataTable;
            rptOffers.DataBind();

        }

        void rptOffers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "addToCart":
                default:

                    Cart cart = StoreHelper.GetCart();
                    if (cart == null) { return; }

                    string strGuid = e.CommandArgument.ToString();
                    if (strGuid.Length != 36) { return; }

                    Guid offerGuid = new Guid(strGuid);
                    Offer offer = new Offer(offerGuid);

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

                    if (cart.AddOfferToCart(offer, quantity))
                    {
                        // redirect to cart page
                        WebUtils.SetupRedirect(this, SiteRoot +
                            "/WebStore/Cart.aspx?pageid="
                            + pageId.ToString()
                            + "&mid=" + moduleId.ToString()
                            + "&cart=" + cart.CartGuid.ToString());

                        return;

                    }

                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    
                    break;


            }

        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                if (product != null)
                {
                    crumbs.AddedCrumbs = "<a class='selectedpage' href='"
                        + Request.RawUrl
                        + "'>" + Server.HtmlEncode(product.Name)
                        + "</a>";
                }
            }

            if (product != null)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + product.Name);
                
            }

            ((CRating)Rating).PromptText = WebStoreResources.RatingPrompt;

            lnkCart.PageID = pageId;
            lnkCart.ModuleID = moduleId;

        }

        private void LoadSettings()
        {
            commerceConfig = SiteUtils.GetCommerceConfig();
            store = StoreHelper.GetStore();

            if (store == null) { return; }

            //currencyCulture = ResourceHelper.GetCurrencyCulture(store.DefaultCurrency);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            if (productGuid != Guid.Empty)
            {
                product = new Product(productGuid);
                //offerPrice = offer.Price;
            }

            Hashtable Settings = ModuleSettings.GetModuleSettings(moduleId);
            enableRatingComments = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableRatingCommentsSetting", enableRatingComments);

            teaserFileBaseUrl = WebUtils.GetSiteRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/webstoreproductpreviewfiles/";

            ScriptConfig.IncludeYahooMediaPlayer = true;

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            productGuid = WebUtils.ParseGuidFromQueryString("product", productGuid);

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

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            rptOffers.ItemCommand += new RepeaterCommandEventHandler(rptOffers_ItemCommand);

            AppendQueryStringToAction = false;
            SuppressPageMenu();
            SuppressGoogleAds();


        }

        

        #endregion
    }
}
