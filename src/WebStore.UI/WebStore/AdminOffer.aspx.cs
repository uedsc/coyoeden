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
using Resources;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{
    public partial class AdminOfferPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string pageNumberParam;
        //private string sortParam;
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        //private string sort = "Name";
        private Store store;
        

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
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (store == null) { return; }

            if (!Page.IsPostBack)
            {
                BindGrid();
            }

        }

        

        private void BindGrid()
        {
            if (store == null) { return; }

            using (IDataReader reader = Offer.GetPage(store.Guid, pageNumber, pageSize, out totalPages))
            {
               
                string pageUrl = WebUtils.GetSiteRoot() + "/WebStore/AdminOffer.aspx"
                    + "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;pagenumber" + this.moduleId.ToString(CultureInfo.InvariantCulture)
                    + "={0}";

                pgrOffer.PageURLFormat = pageUrl;
                pgrOffer.ShowFirstLast = true;
                pgrOffer.CurrentIndex = pageNumber;
                pgrOffer.PageSize = pageSize;
                pgrOffer.PageCount = totalPages;
                pgrOffer.Visible = (totalPages > 1);

                grdOffer.DataSource = reader;
                grdOffer.PageIndex = pageNumber;
                grdOffer.PageSize = pageSize;
                grdOffer.DataBind();
            }

        }

        //private void grdOffer_Sorting(object sender, GridViewSortEventArgs e)
        //{
      
        //    String redirectUrl = WebUtils.GetSiteRoot()
        //        + "/WebStore/AdminOffer.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
        //        + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
        //        + "&pagenumber"
        //        + moduleId.ToString(CultureInfo.InvariantCulture)
        //        + "=" + pageNumber.ToString(CultureInfo.InvariantCulture)
        //        + "&sort"
        //        + moduleId.ToString(CultureInfo.InvariantCulture)
        //        + "=" + e.SortExpression;

        //    WebUtils.SetupRedirect(this, redirectUrl);

        //}

        //private void btnAddNew_Click(object sender, EventArgs e)
        //{
        //    String redirectUrl = SiteRoot
        //        + "/WebStore/AdminOfferEdit.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
        //        + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

        //    WebUtils.SetupRedirect(this, redirectUrl);
        //}

        protected string BuildOfferQueryString(string offerID)
        {

            string result = "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                + "&offer=" + offerID;

            return result;

        }

        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = "<a href='" + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToString() + "&mid=" + moduleId.ToString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>";
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OfferAdministrationLink);
            litHeading.Text = WebStoreResources.OfferAdministrationLink;

           
            grdOffer.Columns[1].HeaderText = WebStoreResources.OfferNameLabel;
            grdOffer.Columns[2].HeaderText = WebStoreResources.OfferIsVisibleLabel;
            grdOffer.Columns[3].HeaderText = WebStoreResources.OfferIsSpecialLabel;


            lnkAddNew.Text = WebStoreResources.OfferAddNewButton;
            

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            if (CurrentPage.ContainsModule(moduleId))
            {
                //store = StoreHelper.GetStore(siteSettings.SiteGuid, moduleId);
                store = StoreHelper.GetStore();
            }
            
            pageNumberParam = "pagenumber" + this.moduleId.ToString(CultureInfo.InvariantCulture);
            pageNumber = WebUtils.ParseInt32FromQueryString(pageNumberParam, 1);

            lnkAddNew.NavigateUrl = SiteRoot
                + "/WebStore/AdminOfferEdit.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

            //sortParam = "sort" + this.moduleId.ToString(CultureInfo.InvariantCulture);

            //if (Page.Request.Params[sortParam] != null)
            //{
            //    sort = Page.Request.Params[sortParam];
            //}

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
            //this.grdOffer.Sorting += new GridViewSortEventHandler(grdOffer_Sorting);
            
            //this.btnAddNew.Click += new EventHandler(btnAddNew_Click);

            SuppressPageMenu();

        }

        #endregion

    }
}
