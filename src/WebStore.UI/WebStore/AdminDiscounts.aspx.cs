// Author:					Joe Audette
// Created:					2009-03-03
// Last Modified:			2009-12-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{

    public partial class AdminDiscountsPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Store store = null;
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 1;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            BindGrid();

        }

        private void BindGrid()
        {
            if (store == null) { return; }

            using (IDataReader reader = Discount.GetPage(store.ModuleGuid, pageNumber, pageSize, out totalPages))
            {
                if (totalPages > 1)
                {

                    string pageUrl = SiteRoot + "/WebStore/AdminDiscounts.aspx"
                        + "?pageid=" + pageId.ToInvariantString()
                        + "&amp;mid=" + moduleId.ToInvariantString()
                        + "&amp;pagenumber" + this.moduleId.ToInvariantString()
                        + "={0}";

                    pgrDiscounts.Visible = true;
                    pgrDiscounts.PageURLFormat = pageUrl;
                    pgrDiscounts.ShowFirstLast = true;
                    pgrDiscounts.CurrentIndex = pageNumber;
                    pgrDiscounts.PageSize = pageSize;
                    pgrDiscounts.PageCount = totalPages;

                }
                else
                {
                    pgrDiscounts.Visible = false;
                }

                grdDiscount.DataSource = reader;
                grdDiscount.PageIndex = pageNumber;
                grdDiscount.PageSize = pageSize;
                grdDiscount.DataBind();
            }

        }


        protected string BuildQueryString(string discountGuid)
        {

            string result = "?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&d=" + discountGuid;

            return result;

        }

        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = "<a href='"
                    + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>";
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.DiscountAdministration);
            litHeading.Text = WebStoreResources.DiscountAdministration;
            lnkNewDiscount.Text = WebStoreResources.NewDiscountLink;

            grdDiscount.Columns[0].HeaderText = " ";
            grdDiscount.Columns[1].HeaderText = WebStoreResources.DiscountDescriptionHeading;
            grdDiscount.Columns[2].HeaderText = WebStoreResources.DiscountCodeHeading;

            
        }

        private void LoadSettings()
        {
            if (CurrentPage.ContainsModule(moduleId))
            {
                //store = StoreHelper.GetStore(siteSettings.SiteGuid, moduleId);
                store = StoreHelper.GetStore();
            }

            lnkNewDiscount.NavigateUrl
                = SiteRoot
                + "/WebStore/AdminDiscountEdit.aspx?pageid=" 
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);


        }

        #endregion
    }
}

