/// Author:					Joe Audette
/// Created:				2007-02-15
/// Last Modified:		    2009-12-17
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
    public partial class AdminOrderHistoryPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string pageNumberParam;
        //private string sortParam;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 1;

        private Store store;
        protected Double timeOffset = 0;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected string detailUrlFormat = string.Empty;
        private bool canEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadParams();

            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();

            SetupCss();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            BindGrid();

        }

        private void BindGrid()
        {
            if (store == null) return;

            litHeading.Text = string.Format(CultureInfo.InvariantCulture,
                WebStoreResources.OrderHistoryHeadingFormatString,
                store.Name);

            using (IDataReader reader = Order.GetPage(
                store.Guid,
                pageNumber,
                pageSize,
                out totalPages))
            {
                string pageUrl = SiteUtils.GetNavigationSiteRoot() + "/WebStore/AdminOrderHistory.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;pagenumber" + moduleId.ToInvariantString()
                    + "={0}";

                pgrOrders.PageURLFormat = pageUrl;
                pgrOrders.ShowFirstLast = true;
                pgrOrders.CurrentIndex = pageNumber;
                pgrOrders.PageSize = pageSize;
                pgrOrders.PageCount = totalPages;
                pgrOrders.Visible = (totalPages > 1);

                grdOrders.DataSource = reader;
                grdOrders.PageIndex = pageNumber;
                grdOrders.PageSize = pageSize;
                grdOrders.DataBind();
            }

        }

        protected string GetOrderStatus(string sGuid)
        {
            if(string.IsNullOrEmpty(sGuid)) { return WebStoreResources.OrderStatusNone; }

            if (sGuid.Length != 36) { return WebStoreResources.OrderStatusNone; }

            Guid statusGuid = new Guid(sGuid);
            return StoreHelper.GetOrderStatusName(statusGuid);

        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if ((c != null) && (store != null))
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = "<a href='" + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid="
                    + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>";
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OrderHistoryAdminLink);

            grdOrders.Columns[0].HeaderText = WebStoreResources.OrderHistoryCustomerHeading;
            grdOrders.Columns[1].HeaderText = WebStoreResources.OrderHistoryOrderTotalHeader;
            grdOrders.Columns[2].HeaderText = WebStoreResources.OrderHistoryDateHeader;
            grdOrders.Columns[3].HeaderText = WebStoreResources.OrderStatusLabel;
            


        }

        private void LoadSettings()
        {
            


            if (CurrentPage.ContainsModule(moduleId))
                store = StoreHelper.GetStore();

            pageNumberParam = "pagenumber" + this.moduleId.ToString(CultureInfo.InvariantCulture);
            pageNumber = WebUtils.ParseInt32FromQueryString(pageNumberParam, 1);

            timeOffset = SiteUtils.GetUserTimeOffset();

            detailUrlFormat = SiteUtils.GetNavigationSiteRoot() + "/WebStore/AdminOrderDetail.aspx"
                + "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                + "&order={0}";

            //if (store == null) { return; }

            //currencyCulture = ResourceHelper.GetCurrencyCulture(store.DefaultCurrency);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            //if ((store != null)&&(store.CurrencyFormatString.Length > 0)) 
            //    currencyFormat = store.CurrencyFormatString;

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            canEdit = UserCanEditModule(moduleId);

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
        }

        #endregion

    }
}
