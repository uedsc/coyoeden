/// Author:					Joe Audette
/// Created:				2007-03-06
/// Last Modified:			2009-12-08
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
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{
    public partial class OfferDetailPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Guid offerGuid = Guid.Empty;
        private Store store = null;
        private Offer offer = null;
        private decimal offerPrice = 0;
        protected bool offerHasMoreThanOneProduct = true;
        private CommerceConfiguration commerceConfig;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
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

            if ((store == null) || (store.IsClosed))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            SetupCss();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (offer == null) { return; }

            if (!offer.IsVisible)
            {
                lnkAddToCart.Visible = false;

                if (!UserCanEditModule(moduleId)) { return; }
            }

            //if (offer.IsDonation)
            //{
            //    if ((commerceConfig.GoogleCheckoutIsEnabled) && (!commerceConfig.Is503TaxExempt))
            //    {
            //        btnGoogleCheckout.Visible = true;
            //        btnGoogleCheckout.Enabled = false;
            //    }


            //}

            litHeading.Text = Server.HtmlEncode(offer.Name);
            litOfferDescription.Text = offer.Description;
            lblPrice.Text = offerPrice.ToString("c", currencyCulture);
            lnkAddToCart.NavigateUrl = SiteRoot + "/WebStore/CartAdd.aspx?offer=" 
                + offer.Guid.ToString() 
                + "&pageid=" + pageId.ToString(CultureInfo.InvariantCulture) 
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

            // if there is just one product in the offer only show the product description, not thhe offer description
            offerHasMoreThanOneProduct = (OfferProduct.GetCountByOffer(offer.Guid) > 1);
            //divOfferDescription.Visible = offerHasMoreThanOneProduct;

            MetaDescription = offer.MetaDescription;
            MetaKeywordCsv = offer.MetaKeywords;
            AdditionalMetaMarkup = offer.CompiledMeta;

            using (IDataReader reader = OfferProduct.GetReaderByOffer(offer.Guid))
            {
                rptProducts.DataSource = reader;
                rptProducts.DataBind();
            }

        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                if (offer != null)
                {
                    crumbs.AddedCrumbs = "<a class='selectedpage' href='"
                        + Request.RawUrl
                        + "'>" + Server.HtmlEncode(offer.Name)
                        + "</a>";
                }
            }

            if (offer != null)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, offer.Name);
                lnkAddToCart.Text = WebStoreResources.AddToCartLink;
            }

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            offerGuid = WebUtils.ParseGuidFromQueryString("offer", offerGuid);

        }

        private void LoadSettings()
        {
            commerceConfig = SiteUtils.GetCommerceConfig();
            store = StoreHelper.GetStore();
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
          
            if((store != null)&&(offerGuid != Guid.Empty))
            {
                offer = new Offer(offerGuid);
                offerPrice = offer.Price;
            }

            teaserFileBaseUrl = WebUtils.GetSiteRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/webstoreproductpreviewfiles/";

            ScriptConfig.IncludeYahooMediaPlayer = true;
           
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

            SuppressPageMenu();
            SuppressGoogleAds();

        }

        #endregion
    }
}
