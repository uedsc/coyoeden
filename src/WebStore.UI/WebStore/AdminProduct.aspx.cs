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
    public partial class AdminProductPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string pageNumberParam;
        private string sortParam;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 0;
        private string sort = "Name";
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
            if (!Page.IsPostBack)
            {
                BindGrid();
            }

        }

        

        private void BindGrid()
        {
            if (store == null) { return; }
            

            DataTable dt = Product.GetPageForAdminList(
                store.Guid,
                pageNumber,
                pageSize,
                out totalPages);

            //if (dt.Rows.Count > 0)
            //{
            //    totalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
            //}

            DataView dv = dt.DefaultView;
            dv.Sort = sort;

            if (this.totalPages > 1)
            {
                string pageUrl = WebUtils.GetSiteRoot() + "/WebStore/AdminProduct.aspx"
                    + "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;sort" + this.moduleId.ToString(CultureInfo.InvariantCulture)
                    + "=" + this.sort
                    + "&amp;pagenumber" + this.moduleId.ToString(CultureInfo.InvariantCulture)
                    + "={0}";

                pgrProduct.Visible = true;
                pgrProduct.PageURLFormat = pageUrl;
                pgrProduct.ShowFirstLast = true;
                pgrProduct.CurrentIndex = pageNumber;
                pgrProduct.PageSize = pageSize;
                pgrProduct.PageCount = totalPages;

            }
            else
            {
                pgrProduct.Visible = false;
            }

            grdProduct.DataSource = dv;
            grdProduct.PageIndex = pageNumber;
            grdProduct.PageSize = pageSize;
            grdProduct.DataBind();

        }

        private void grdProduct_Sorting(object sender, GridViewSortEventArgs e)
        {

            String redirectUrl = WebUtils.GetSiteRoot()
                + "/WebStore/AdminProduct.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                + "&pagenumber"
                + moduleId.ToString(CultureInfo.InvariantCulture)
                + "=" + pageNumber.ToString(CultureInfo.InvariantCulture)
                + "&sort"
                + moduleId.ToString(CultureInfo.InvariantCulture)
                + "=" + e.SortExpression;

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        //private void btnAddNew_Click(object sender, EventArgs e)
        //{
        //    String redirectUrl = WebUtils.GetSiteRoot()
        //        + "/WebStore/AdminProductEdit.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
        //        + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

        //    WebUtils.SetupRedirect(this, redirectUrl);
        //}


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = "<a href='" + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture) + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>";
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.ProductAdministrationLink);
            litHeading.Text = WebStoreResources.ProductAdministrationLink;

            

            grdProduct.Columns[1].HeaderText = WebStoreResources.ProductModelNumberLabel;
            grdProduct.Columns[2].HeaderText = WebStoreResources.ProductNameLabel;

            lnkNewProduct.Text = WebStoreResources.ProductGridAddNewButton;

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

            sortParam = "sort" + this.moduleId.ToString(CultureInfo.InvariantCulture);

            if (Request.Params[sortParam] != null)
            {
                sort = Request.Params[sortParam];
            }

            lnkNewProduct.NavigateUrl = SiteRoot +"/WebStore/AdminProductEdit.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

        }

        protected string BuildProductQueryString(string productID)
        {

            string result = "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                + "&prod=" + productID;

            return result;

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
            this.grdProduct.Sorting += new GridViewSortEventHandler(grdProduct_Sorting);
            //this.btnAddNew.Click += new EventHandler(btnAddNew_Click);
            
            SuppressPageMenu();
        }

        #endregion

    }
}
