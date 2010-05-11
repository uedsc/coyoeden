

using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;
using Cynthia.Business.WebHelpers;
using WebStore.Business;

namespace WebStore.UI
{
    
    public partial class WebStoreModule : SiteModuleControl
    {
        protected ModuleTitleControl TitleControl;
        protected Store store;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private bool enableRatingsInProductList = false;
        private bool enableRatingComments = false;

        const string GroupByProduct = "GroupByProduct";
        const string GroupByOffer = "GroupByOffer";

        private string groupingMode = GroupByProduct;
        

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Site != null && this.Site.DesignMode) { return; }

            this.Load += new EventHandler(Page_Load);

            LoadSettings();
            productList1.Store = store;
            productList1.PageId = PageId;
            productList1.ModuleId = ModuleId;
            productList1.SiteRoot = SiteRoot;
            productList1.CurrencyCulture = currencyCulture;
            productList1.EnableRatings = enableRatingsInProductList;
            productList1.EnableRatingComments = enableRatingComments;
            productList1.Settings = Settings;


            offerList1.Store = store;
            offerList1.PageId = PageId;
            offerList1.ModuleId = ModuleId;
            offerList1.SiteRoot = SiteRoot;
            offerList1.CurrencyCulture = currencyCulture;
            offerList1.EnableRatings = enableRatingsInProductList;
            offerList1.EnableRatingComments = enableRatingComments;
            offerList1.Settings = Settings;
            

        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Site != null && this.Site.DesignMode) { return; }

            SetupCss();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            TitleControl.EditUrl = SiteRoot + "/WebStore/AdminDashboard.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            //if (
            //    (store != null)
            //    && (WebUser.IsInRoles(store.StoreConfigRoles))
            //    )
            //{
            //    TitleControl.ShowEditLinkOverride = true;
            //}

            if (store.Guid == Guid.Empty)
            {
                // store not created yet
                pnlStore.Visible = false;
                pnlStoreClosed.Visible = true;

                return;

            }

            if (store.IsClosed)
            {
                litStoredClosed.Text = store.ClosedMessage;
                pnlStoreClosed.Visible = true;
                pnlStore.Visible = false;
                return;
            }

            litStoreDescription.Text = store.Description;

            //PopulateOfferList();
            PopulateSpecials(); 

        }

        private void PopulateSpecials()
        {
            using (IDataReader reader = Offer.GetTop10Specials(store.Guid))
            {
                rptSpecials.DataSource = reader;
                rptSpecials.DataBind();
            }

            pnlSpecials.Visible = (rptSpecials.Items.Count > 0);

            

        }


        

        private void PopulateLabels()
        {
            litOfferListHeading.Text = WebStoreResources.OfferListHeading;

            litSpecialsHeading.Text = WebStoreResources.CurrentSpecialsHeading;
            lnkCart.PageID = PageId;
            lnkCart.ModuleID = ModuleId;
            //lnkCart.Text = "Cart";
            //lnkCart.NavigateUrl = Page.ResolveUrl(
            //    "~/WebStore/Cart.aspx?pageid=" + PageID.ToString()
            //    + "&mid=" + ModuleID.ToString());

            TitleControl.EditText = WebStoreResources.StoreManagerLink;
            

            litStoredClosed.Text = WebStoreResources.StoreClosedDefaultMessage;

        }

        private void LoadSettings()
        {
            store = new Store(SiteSettings.SiteGuid, ModuleId);

            enableRatingsInProductList = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableContentRatingInProductListSetting", enableRatingsInProductList);

            enableRatingComments = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableRatingCommentsSetting", enableRatingComments);


            if (Settings.Contains("ProductListGroupingSetting"))
            {
                groupingMode = Settings["ProductListGroupingSetting"].ToString();
            }

            switch (groupingMode)
            {
                case GroupByOffer:
                    this.offerList1.Visible = true;
                    this.productList1.Visible = false;

                    break;

                default:

                    this.offerList1.Visible = false;
                    this.productList1.Visible = true;

                    break;

            }
            currencyCulture = ResourceHelper.GetCurrencyCulture(SiteSettings.GetCurrency().Code);

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
       
    }
}