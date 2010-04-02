/// Author:					Joe Audette
/// Created:				2007-02-15
/// Last Modified:		    2009-07-27
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{
    public partial class AdminDashboardPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Store store = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            SetupCss();
            PopulateLabels();
            EnsureReportData();
        }

        private void EnsureReportData()
        {
            if (store == null) { return; }
            if (pageId == -1) { return; }
            if (moduleId == -1) { return; }
            if (Page.IsPostBack) { return; }

            decimal storeRevenue = Order.GetAllTimeRevenueTotal(store.Guid);
            decimal reportRevenue = CommerceReport.GetAllTimeRevenueByModule(store.ModuleGuid);

            if ((storeRevenue > 0)&&(reportRevenue == 0))
            {
                CommerceReport.DeleteByModule(store.ModuleGuid);
                Order.EnsureSalesReportData(store.ModuleGuid, pageId, moduleId);
                SiteUser.UpdateTotalRevenue();
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
                //    + Page.ResolveUrl("~/WebStore/AdminDashboard.aspx")
                //    + "'>" + WebStoreResources.StoreManagerLink
                //    + "</a>";
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.StoreManagerHeading);

            litStoreManagerHeading.Text = WebStoreResources.StoreManagerHeading;
            
      
            lnkStoreSettings.Text = WebStoreResources.StoreSettingsLink;
            lnkProductAdmin.Text = WebStoreResources.ProductAdministrationLink;
            lnkOfferAdmin.Text = WebStoreResources.OfferAdministrationLink;
            lnkCategoryAdmin.Text = WebStoreResources.CategoryAdministrationLink;
            lnkDownloadTermsAdmin.Text = WebStoreResources.DownloadTermsAdministrationLink;
            lnkDiscountAdmin.Text = WebStoreResources.DiscountAdministration;
            lnkOrderEntry.Text = WebStoreResources.OrderEntry;
            lnkOrderHistory.Text = WebStoreResources.OrderHistoryAdminLink;
            lnkBrowseCarts.Text = WebStoreResources.BrowseCartsLink;
            btnRebuildReports.Text = WebStoreResources.RebuildReportsButton;
            btnRebuildReports.Visible = WebConfigSettings.ShowRebuildReportsButton && WebUser.IsAdmin;

            if ((store != null)&&(WebUser.IsInRoles(siteSettings.CommerceReportViewRoles)))
            {
                liReports.Visible = true;
                lnkReports.Text = WebStoreResources.ReportsLink;
                lnkReports.NavigateUrl = SiteRoot + "/Admin/SalesByModule.aspx?m=" + store.ModuleGuid.ToString();
            }
            
            


        }

        void btnRebuildReports_Click(object sender, EventArgs e)
        {
            //Module m = new Module(moduleId);
            //if (m.SiteGuid != siteSettings.SiteGuid) { return; }
            //CommerceReport.DeleteByModule(m.ModuleGuid);
            //Order.EnsureSalesReportData(m.ModuleGuid, pageId, m.ModuleId);
            //SiteUser.UpdateTotalRevenue();
            if (store != null)
            {
                CommerceReport.DeleteByModule(store.ModuleGuid);
                Order.EnsureSalesReportData(store.ModuleGuid, pageId, moduleId);
                SiteUser.UpdateTotalRevenue();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            if (CurrentPage.ContainsModule(moduleId))
            {
                //store = StoreHelper.GetStore(siteSettings.SiteGuid, moduleId);
                store = StoreHelper.GetStore();
            }

            lnkStoreSettings.NavigateUrl = SiteRoot + "/WebStore/AdminStoreSettings.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture) + "&mid=" + moduleId.ToInvariantString();

            lnkProductAdmin.NavigateUrl = SiteRoot + "/WebStore/AdminProduct.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture) + "&mid=" + moduleId.ToInvariantString();

            lnkOfferAdmin.NavigateUrl = SiteRoot + "/WebStore/AdminOffer.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture) + "&mid=" + moduleId.ToInvariantString();

            lnkCategoryAdmin.NavigateUrl = SiteRoot + "/WebStore/AdminCategory.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture) + "&mid=" + moduleId.ToInvariantString();

            lnkDownloadTermsAdmin.NavigateUrl = SiteRoot + "/WebStore/AdminDownloadTerms.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture) + "&mid=" + moduleId.ToInvariantString();

            lnkDiscountAdmin.NavigateUrl = SiteRoot + "/WebStore/AdminDiscounts.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture) + "&mid=" + moduleId.ToInvariantString();

            
            //lnkOrderStatusAdmin.NavigateUrl = SiteRoot + "/WebStore/AdminOrderStatus.aspx?pageid="
            //    + pageId.ToString() + "&mid=" + moduleId.ToString();

            lnkOrderEntry.NavigateUrl = SiteRoot + "/WebStore/AdminOrderEntry.aspx?pageid="
                + pageId.ToString() + "&mid=" + moduleId.ToInvariantString();

            lnkOrderHistory.NavigateUrl = SiteRoot + "/WebStore/AdminOrderHistory.aspx?pageid="
                + pageId.ToString() + "&mid=" + moduleId.ToInvariantString();

            lnkBrowseCarts.NavigateUrl = SiteRoot + "/WebStore/AdminCartBrowser.aspx?pageid="
                + pageId.ToString() + "&mid=" + moduleId.ToInvariantString();

            

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
            btnRebuildReports.Click += new EventHandler(btnRebuildReports_Click);

            SuppressPageMenu();
        }

        

        #endregion

    }
}
