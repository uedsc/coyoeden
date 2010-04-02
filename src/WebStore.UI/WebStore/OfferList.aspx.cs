// Author:					Joe Audette
// Created:					2010-3-11
// Last Modified:			2010-3-11
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
using System.Globalization;
using System.Web.UI;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Cynthia.Business;
using WebStore.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace WebStore.UI
{

    public partial class OfferListPage : CBasePage
    {
        protected Store store;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private bool enableRatingsInProductList = false;
        private bool enableRatingComments = false;
        private Hashtable Settings = null;
        private Module module = null;
        private int pageId = -1;
        private int moduleId = -1;

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

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OfferListHeading);

            TitleControl.EditUrl = SiteRoot + "/WebStore/AdminDashboard.aspx";
            TitleControl.EditText = WebStoreResources.StoreManagerLink;
            if (module != null){ TitleControl.ModuleInstance = module; }
            TitleControl.CanEdit = UserCanEditModule(moduleId);

            Control c = Master.FindControl("Breadcrumbs");
            if ((c != null) && (store != null))
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                //crumbs.AddedCrumbs
                //    = "<a href='" + SiteRoot
                //    + "/WebStore/AdminDashboard.aspx?pageid="
                //    + pageId.ToString(CultureInfo.InvariantCulture)
                //    + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                //    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                //    + "</a>";
            }

            litOfferListHeading.Text = WebStoreResources.OfferListHeading;

            

           

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            store = new Store(siteSettings.SiteGuid, moduleId);
            Settings = ModuleSettings.GetModuleSettings(moduleId);
            module = GetModule(moduleId);

            lnkCart.PageID = pageId;
            lnkCart.ModuleID = moduleId;

            enableRatingsInProductList = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableContentRatingInProductListSetting", enableRatingsInProductList);

            enableRatingComments = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableRatingCommentsSetting", enableRatingComments);

            

            offerList1.Store = store;
            offerList1.PageId = pageId;
            offerList1.ModuleId = moduleId;
            offerList1.SiteRoot = SiteRoot;
            offerList1.CurrencyCulture = currencyCulture;
            offerList1.EnableRatings = enableRatingsInProductList;
            offerList1.EnableRatingComments = enableRatingComments;
            offerList1.Settings = Settings;

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
