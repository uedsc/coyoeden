/// Author:					Joe Audette
/// Created:				2008-04-06
/// Last Modified:			2010-03-17
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
using Cynthia.Business;
using Cynthia.Business.Commerce;
using Cynthia.Business.WebHelpers;
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{
    public partial class AdminOrderDetailPage : CBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Store store;
        protected Double timeOffset = 0;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected Guid orderGuid = Guid.Empty;
        private Order order = null;
        private SiteUser orderUser = null;
        bool allowDeletingOrders = false;
        private bool canEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();

            //string allowedRoles = "Admins;";
            //if (store != null)
            //{
            //    allowedRoles = store.StoreConfigRoles;
            //}
            //StoreSecurity.SecureAdminPage(this, allowedRoles);

            SetupCss();
            PopulateLabels();

            if (Page.IsPostBack) { return; }

            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            if ((store != null) && (order != null))
            {
                if (orderUser != null)
                {
                    lblSiteUser.Text = orderUser.LoginName + " " + orderUser.Name + " " + orderUser.Email;
                    if (WebUser.IsAdmin)
                    {
                        lnkUser.Text = WebStoreResources.ManageUserLink;
                        lnkUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userid=" + orderUser.UserId.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        lnkUser.Text = WebStoreResources.UserProfileLink;
                        lnkUser.NavigateUrl = SiteRoot + "/ProfileView.aspx?userid=" + orderUser.UserId.ToString(CultureInfo.InvariantCulture);
                    }
                }

                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName);
                lblOrderId.Text = order.OrderGuid.ToString();
                ISettingControl setting = orderStatusControl as ISettingControl;
                setting.SetValue(order.StatusGuid.ToString());

                litSubTotal.Text = order.SubTotal.ToString("c", currencyCulture);
                litDiscount.Text = order.Discount.ToString("c", currencyCulture);
                litShippingTotal.Text = order.ShippingTotal.ToString("c", currencyCulture);
                litTaxTotal.Text = order.TaxTotal.ToString("c", currencyCulture);
                litOrderTotal.Text = order.OrderTotal.ToString("c", currencyCulture);

                pnlDiscount.Visible = (order.Discount > 0);
                pnlShippingTotal.Visible = (order.ShippingTotal > 0);
                pnlTaxTotal.Visible = (order.TaxTotal > 0);

                if ((order.ShippingTotal == 0) && (order.TaxTotal == 0) && (order.Discount == 0))
                {
                    pnlSubTotal.Visible = false;
                }

                lblCustomerAddressLine1.Text = order.CustomerAddressLine1;
                lblCustomerAddressLine2.Text = order.CustomerAddressLine2;
                lblCustomerCity.Text = order.CustomerCity;
                lblCustomerCompany.Text = order.CustomerCompany;
                lblCustomerCountry.Text = order.CustomerCountry;
                lblCustomerEmail.Text = order.CustomerEmail;
                lblCustomerGeoZone.Text = order.CustomerState;
                lblCustomerName.Text = order.CustomerFirstName + " " + order.CustomerLastName;
                lblCustomerPostalCode.Text = order.CustomerPostalCode;
                lblCustomerSuburb.Text = order.CustomerSuburb;
                lblCustomerTelephoneDay.Text = order.CustomerTelephoneDay;
                lblCustomerTelephoneNight.Text = order.CustomerTelephoneNight;
                lblDiscountCodes.Text = order.DiscountCodesCsv;
                lblCustomData.Text = order.CustomData;

                using (IDataReader reader = order.GetProducts())
                {
                    rptOrderItems.DataSource = reader;
                    rptOrderItems.DataBind();
                }

                grdDownloadTickets.DataSource = order.GetDownloadTickets();
                grdDownloadTickets.DataBind();
                pnlDownloadTickets.Visible = (grdDownloadTickets.Rows.Count > 0);

                litPaymentMethod.Text = ResourceHelper.GetResourceString("WebStoreResources", order.PaymentMethod);

                Control c = Page.LoadControl("~/Controls/GCheckoutLogList.ascx");
                if (c != null)
                {
                    GCheckoutLogList googleLog = c as GCheckoutLogList;
                    googleLog.CartGuid = order.OrderGuid;
                    pnlCheckoutLog.Controls.Add(googleLog);
                }

                c = Page.LoadControl("~/Controls/PayPalLogList.ascx");
                if (c != null)
                {
                    PayPalLogList logList = c as PayPalLogList;
                    logList.CartGuid = order.OrderGuid;
                    pnlCheckoutLog.Controls.Add(logList);
                }

                c = Page.LoadControl("~/Controls/AuthorizeNetLogList.ascx");
                if (c != null)
                {
                    AuthorizeNetLogList logList = c as AuthorizeNetLogList;
                    logList.CartGuid = order.OrderGuid;
                    pnlCheckoutLog.Controls.Add(logList);
                }
                c = Page.LoadControl("~/Controls/PlugNPayLogList.ascx");
                if (c != null)
                {
                    PlugNPayLogList logList = c as PlugNPayLogList;
                    logList.CartGuid = order.OrderGuid;
                    pnlCheckoutLog.Controls.Add(logList);
                }



            }


        }

        

        void btnSaveStatusChange_Click(object sender, EventArgs e)
        {
            if (order != null)
            {
                ISettingControl setting = orderStatusControl as ISettingControl;
                string status = setting.GetValue();
                if (status.Length == 36)
                {
                    Guid newStatusGuid = new Guid(status);
                    order.StatusGuid = newStatusGuid;
                    order.Save();

                    if ((newStatusGuid == OrderStatus.OrderStatusCancelledGuid) || (newStatusGuid == OrderStatus.OrderStatusNoneGuid))
                    {
                        FullfillDownloadTicket.DeleteByOrder(order.OrderGuid);
                        CommerceReport.DeleteOrder(order.OrderGuid);

                    }

                }

            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        void btnDelete_Click(object sender, EventArgs e)
        {
            CommerceReport.DeleteOrder(order.OrderGuid);
            FullfillDownloadTicket.DeleteByOrder(order.OrderGuid);
            Order.Delete(orderGuid);

            string redirectUrl = "AdminOrderHistory.aspx?pageid="
                    + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

            WebUtils.SetupRedirect(this, redirectUrl);


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
                    + "</a>"
                    + "&nbsp;&gt;&nbsp;"
                    + "<a href='" + SiteRoot
                    + "/WebStore/AdminOrderHistory.aspx?pageid="
                    + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "' class='unselectedcrumb'>" + WebStoreResources.OrderHistoryAdminLink
                    + "</a>";

            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OrderDetail);
            litItemsHeader.Text = WebStoreResources.OrderDetailItemsHeader;
            btnSaveStatusChange.Text = WebStoreResources.OrderStatusButton;
            btnDelete.Text = WebStoreResources.DeleteOrderButton;

            UIHelper.AddConfirmationDialog(btnDelete, WebStoreResources.DeleteOrderWarning);

            litDownloadTicketsHeading.Text = WebStoreResources.DownloadTicketsHeading;

            grdDownloadTickets.Columns[0].HeaderText = WebStoreResources.DownloadTicketsProductHeading;
            grdDownloadTickets.Columns[1].HeaderText = WebStoreResources.DownloadTicketAllowedHeading;
            grdDownloadTickets.Columns[2].HeaderText = WebStoreResources.DownloadTicketDownloadsHeading;
        }

        private void LoadSettings()
        {
            
            allowDeletingOrders = ConfigHelper.GetBoolProperty("WebStoreAllowDeletingOrders", allowDeletingOrders);

            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            if (CurrentPage.ContainsModule(moduleId))
                store = StoreHelper.GetStore();

            if (store == null) { return; }

            //currencyCulture = ResourceHelper.GetCurrencyCulture(store.DefaultCurrency);

            if (orderGuid != Guid.Empty)
            {
                order = new Order(orderGuid);
               
                orderUser = new SiteUser(siteSettings, order.UserGuid);

                if (order.StoreGuid != store.Guid)
                    order = null;
            }

            
            timeOffset = SiteUtils.GetUserTimeOffset();

            btnDelete.Visible = allowDeletingOrders;

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            orderGuid = WebUtils.ParseGuidFromQueryString("order", orderGuid);
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

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnDelete.Click += btnDelete_Click;
            this.btnSaveStatusChange.Click += new EventHandler(btnSaveStatusChange_Click);

            SuppressPageMenu();


        }

        

        #endregion
    }
}
