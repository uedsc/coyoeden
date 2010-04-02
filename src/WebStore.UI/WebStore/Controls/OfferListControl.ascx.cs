// Author:					Joe Audette
// Created:				    2009-07-04
// Last Modified:			2009-07-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;
using Cynthia.Web.Framework;
using WebStore.Business;

namespace WebStore.UI
{
    public partial class OfferListControl : UserControl
    {
        #region Private Properties

        private int pageId = -1;
        private int moduleId = -1;
        private int pageNumber = 1;
        private int totalPages = 1;
        private int pageSize = 15;
        private Store store = null;
        private string siteRoot = string.Empty;
        protected string teaserFileBaseUrl = string.Empty;
        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private DataSet dsOffers = null;
        private bool enableRatings = false;
        private bool enableRatingComments = false;
        private Hashtable settings = null;




        #endregion

        #region Public Properties

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public Store Store
        {
            get { return store; }
            set { store = value; }
        }

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public CultureInfo CurrencyCulture
        {
            get { return currencyCulture; }
            set { currencyCulture = value; }
        }

        public bool EnableRatings
        {
            get { return enableRatings; }
            set { enableRatings = value; }
        }

        public bool EnableRatingComments
        {
            get { return enableRatingComments; }
            set { enableRatingComments = value; }
        }

        public Hashtable Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Visible) { return; }

            LoadSettings();

            if ((!Page.IsPostBack) && (ParamsAreValid()))
            {
                BindOffers();
            }

        }

        private void BindOffers()
        {

            dsOffers = store.GetOfferPageWithProducts(
                pageNumber,
                pageSize,
                out totalPages);

            string pageUrl = SiteUtils.GetNavigationSiteRoot() + "/WebStore/OfferList.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;pagenumber={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.CurrentIndex = pageNumber;
            pgr.PageSize = pageSize;
            pgr.PageCount = totalPages;
            pgr.Visible = (totalPages > 1);

            rptOffers.DataSource = dsOffers.Tables["Offers"];
            rptOffers.DataBind();


        }

        void rptOffers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (dsOffers == null) { return; }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string offerGuid = ((DataRowView)e.Item.DataItem).Row.ItemArray[0].ToString();
                Repeater rptProducts = (Repeater)e.Item.FindControl("rptProducts");

                if (rptProducts == null) { return; }

                string whereClause = string.Format("OfferGuid = '{0}'", offerGuid);
                DataView dv = new DataView(dsOffers.Tables["OfferProducts"], whereClause, "", DataViewRowState.CurrentRows);

                rptProducts.DataSource = dv;
                rptProducts.DataBind();


            }
        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            if (Page is CBasePage)
            {
                CBasePage basePage = Page as CBasePage;
                basePage.ScriptConfig.IncludeYahooMediaPlayer = true;
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            teaserFileBaseUrl = WebUtils.GetSiteRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString()
                + "/webstoreproductpreviewfiles/";

            if (Settings != null)
            {
                pageSize = WebUtils.ParseInt32FromHashtable(Settings, "ProductListPageSize", pageSize);
            }

        }

        private bool ParamsAreValid()
        {
            if (store == null) { return false; }
            if (pageId == -1) { return false; }
            if (moduleId == -1) { return false; }


            return true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
    
            rptOffers.ItemDataBound += new RepeaterItemEventHandler(rptOffers_ItemDataBound);
        }

    }
}