/// Author:					Joe Audette
/// Created:				2007-02-24
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
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brettle.Web.NeatUpload;
using Cynthia.Web;
using Cynthia.Web.Editor;
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
    
    public partial class AdminProductEditPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Store store;
        private Guid productGuid = Guid.Empty;
        private string virtualRoot;
        private SiteUser siteUser = null;
        private string upLoadPath;
        private string teaserFileBaseUrl = string.Empty;
        private string teaserFileBasePath = string.Empty;
        //private bool showPropertiesTab = false;
        private int intSortRank1;
        private int intSortRank2;
        ContentMetaRespository metaRepository = new ContentMetaRespository();
        
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
                PopulateTaxClassList();
                PopulateStatusList();
                PopulateFullfillmentTypeList();
                PopulateProductControls();
                BindMeta();
                BindMetaLinks();
            }

            if (productGuid == Guid.Empty)
            {
                //new product
                
                btnDelete.Visible = false;
            }



        }

        private void PopulateProductControls()
        {
            if (store == null) { return; }

            Product product;
            ListItem listItem;

            if (productGuid != Guid.Empty)
            {
                product = new Product(productGuid);
                listItem = ddTaxClassGuid.Items.FindByValue(product.TaxClassGuid.ToString());
                if (listItem != null)
                {
                    ddTaxClassGuid.ClearSelection();
                    listItem.Selected = true;
                }
                
                Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.ProductEditHeading + " - " + product.Name);
                litHeading.Text += Server.HtmlEncode(" : " + product.Name);
                txtName.Text = product.Name;
                edAbstract.Text = product.Teaser;
                edDescription.Text = product.Description;
                txtMetaDescription.Text = product.MetaDescription;
                txtMetaKeywords.Text = product.MetaKeywords;
                //txtSku.Text = product.Sku.ToString();
                txtModelNumber.Text = product.ModelNumber.ToString();
                chkShowInProductList.Checked = product.ShowInProductList;
                chkEnableRating.Checked = product.EnableRating;

                listItem = ddStatus.Items.FindByValue(((int)product.Status).ToString());
                if (listItem != null)
                {
                    ddStatus.ClearSelection();
                    listItem.Selected = true;
                }

                listItem = ddFullfillmentType.Items.FindByValue(((int)product.FulfillmentType).ToString());
                if (listItem != null)
                {
                    ddFullfillmentType.ClearSelection();
                    listItem.Selected = true;
                }

                if (product.FulfillmentType == FulfillmentType.Download)
                {
                    pnlUpload.Visible = true;

                    ProductFile productFile = new ProductFile(productGuid);
                    if (productFile.ProductGuid != Guid.Empty)
                    {
                        lnkDownload.Text = productFile.FileName;
                        lnkDownload.NavigateUrl = SiteRoot + "/WebStore/ProductDownload.aspx?prod="
                            + productFile.ProductGuid.ToString();
                    }

                    
                }

                if (product.TeaserFile.Length > 0)
                {
                    lnkTeaserDownload.NavigateUrl = teaserFileBaseUrl + product.TeaserFile;
                    lnkTeaserDownload.Visible = true;
                    if (product.TeaserFileLink.Length > 0)
                    {
                        lnkTeaserDownload.Text = product.TeaserFileLink;
                    }
                    else
                    {
                        lnkTeaserDownload.Text = product.TeaserFile;
                    }
                    txtTeaserFileLinkText.Text = product.TeaserFileLink;
                }

                txtWeight.Text = product.Weight.ToString();
                txtQuantityOnHand.Text = product.QuantityOnHand.ToString(CultureInfo.InvariantCulture);
                txtSortRank1.Text = product.SortRank1.ToString(CultureInfo.InvariantCulture);
                txtSortRank2.Text = product.SortRank2.ToString(CultureInfo.InvariantCulture);

                
            }

        }


        private void PopulateTaxClassList()
        {
            using (IDataReader reader = TaxClass.GetBySite(siteSettings.SiteGuid))
            {
                ddTaxClassGuid.DataSource = reader;
                ddTaxClassGuid.DataBind();
            }

        }

        private void PopulateStatusList()
        {
            ListItem listItem = new ListItem();
            listItem.Value = "1";
            listItem.Text = WebStoreResources.ProductStatusEnumAvailable;
            ddStatus.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "2";
            listItem.Text = WebStoreResources.ProductStatusEnumDiscontinued;
            ddStatus.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "3";
            listItem.Text = WebStoreResources.ProductStatusEnumPlanned;
            ddStatus.Items.Add(listItem);
        }

        private void PopulateFullfillmentTypeList()
        {

            ListItem listItem = new ListItem();

            listItem.Value = "3";
            listItem.Text = WebStoreResources.FullfillmentTypeEnumNone;
            ddFullfillmentType.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "1";
            listItem.Text = WebStoreResources.FullfillmentTypeEnumDownload;
            ddFullfillmentType.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "2";
            listItem.Text = WebStoreResources.FullfillmentTypeEnumShipped;
            ddFullfillmentType.Items.Add(listItem);

           
        }

        private void Save()
        {
            if (store == null) { return; }

            Product product;
            if (productGuid != Guid.Empty)
            {
                product = new Product(productGuid);
            }
            else
            {
                product = new Product();
                product.StoreGuid = store.Guid;
                if (siteUser != null)
                {
                    product.CreatedBy = siteUser.UserGuid;
                    product.Created = DateTime.UtcNow;
                }
            }

            product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

            if (ddTaxClassGuid.SelectedIndex > -1)
            {
                Guid taxClassGuid = new Guid(ddTaxClassGuid.SelectedValue);
                product.TaxClassGuid = taxClassGuid;
            }

            product.Name = txtName.Text;
            product.Teaser = edAbstract.Text;
            product.Description = edDescription.Text;
            product.MetaDescription = txtMetaDescription.Text;
            product.MetaKeywords = txtMetaKeywords.Text;

            //product.Sku = txtSku.Text; //TODO: Changed in db so sku don't need to be unique, maybe change back later (IX_ws_Product)
            product.ModelNumber = txtModelNumber.Text;
            product.Status = Product.ProductStatusFromString(ddStatus.SelectedValue);

            product.FulfillmentType
                = Product.FulfillmentTypeFromString(ddFullfillmentType.SelectedValue);

            decimal weight;
            if (!decimal.TryParse(
                txtWeight.Text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out weight))
            {
                weight = 0;
            }
            product.Weight = weight;
            int qty;
            if (!int.TryParse(txtQuantityOnHand.Text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out qty))
            {
                qty = 1;
            }
            product.QuantityOnHand = qty;
            product.ShowInProductList = chkShowInProductList.Checked;
            product.EnableRating = chkEnableRating.Checked;

            product.TeaserFileLink = txtTeaserFileLinkText.Text;

			// If the sort rank inputs are left null on the product
			//		and offer creation screens, the in.Parse gets all
			//		bent out of shape so I converted to TryParse
            if (int.TryParse(txtSortRank1.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out intSortRank1))
            {
                product.SortRank1 = intSortRank1;
            }
            if (int.TryParse(txtSortRank2.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out intSortRank2))
            {
                product.SortRank2 = intSortRank2;
            }
            //product.ImageFileName = txtImageFileName.Text;
            //product.ImageFileBytes = txtImageFileBytes.Text;

            product.LastModified = DateTime.UtcNow;
            if (siteUser != null)
            {
                product.LastModifedBy = siteUser.UserGuid;
            }

            bool needToCreateFriendlyUrl = false;

            if ((product.Url.Length == 0) && (txtName.Text.Length > 0))
            {
                product.Url = "/"
                    + SiteUtils.SuggestFriendlyUrl(
                    txtName.Text + WebStoreResources.ProductUrlSuffix,
                    siteSettings);

                needToCreateFriendlyUrl = true;

            }
            else
            {

                //TODO: change url if title changed?

            }

            if (product.Save())
            {
                productGuid = product.Guid;

                if (needToCreateFriendlyUrl)
                {

                    FriendlyUrl newUrl = new FriendlyUrl();
                    newUrl.SiteId = siteSettings.SiteId;
                    newUrl.SiteGuid = siteSettings.SiteGuid;
                    newUrl.PageGuid = product.Guid;
                    newUrl.Url = product.Url.Replace("/", string.Empty);
                    newUrl.RealUrl = "~/WebStore/ProductDetail.aspx?pageid="
                        + pageId.ToString(CultureInfo.InvariantCulture)
                        + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                        + "&product=" + product.Guid.ToString();

                    newUrl.Save();

                }

            }
            SiteUtils.QueueIndexing();
            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                Save();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Product p = new Product(productGuid);

            FriendlyUrl.DeleteByPageGuid(p.Guid);

            ContentChangedEventArgs args = new ContentChangedEventArgs();
            args.IsDeleted = true;
            product_ContentChanged(p, args);

            Product.Delete(
                productGuid,
                siteUser.UserGuid,
                SiteUtils.GetIP4Address());

            SiteUtils.QueueIndexing();
            WebUtils.SetupRedirect(this, GetReturnUrl());

        }

        void product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["WebStoreProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileInput.HasFile
                && fileInput.FileName != null
                && fileInput.FileName.Trim().Length > 0)
            {
                ProductFile productFile = new ProductFile(productGuid);

                //// FIXME: the following line probably doesn't work right when uploading from
                //// Windows to Unix because GetFileName will be looking for "\" and the FileName
                //// will contain "/".
                productFile.FileName = Path.GetFileName(fileInput.FileName);
                productFile.ByteLength = (int)fileInput.ContentLength;
                productFile.Created = DateTime.UtcNow;
                productFile.CreatedBy = siteUser.UserGuid;
                productFile.ServerFileName = productGuid.ToString() + ".config";

                if (productFile.Save())
                {
                    string destPath = upLoadPath + productFile.ServerFileName;
                    if (System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Delete(destPath);
                    }
                    fileInput.MoveTo(destPath, MoveToOptions.Overwrite);

                    WebUtils.SetupRedirect(this, GetRefreshUrl());
                }

            }


        }

        void btnUploadTeaser_Click(object sender, EventArgs e)
        {

            if (teaserFileInput.HasFile
                && teaserFileInput.FileName != null
                && teaserFileInput.FileName.Trim().Length > 0)
            {
                Product product = new Product(productGuid);
                product.TeaserFile = Path.GetFileName(teaserFileInput.FileName).ToCleanFileName();
                product.TeaserFileLink = txtTeaserFileLinkText.Text;

                if (product.Save())
                {
                    string destPath = teaserFileBasePath + Path.GetFileName(teaserFileInput.FileName).ToCleanFileName();
                    if (System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Delete(destPath);
                    }
                    teaserFileInput.MoveTo(destPath, MoveToOptions.Overwrite);

                    WebUtils.SetupRedirect(this, GetRefreshUrl());
                }

            }

        }


        #region Meta Data

        private void BindMeta()
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            List<ContentMeta> meta = metaRepository.FetchByContent(product.Guid);
            grdContentMeta.DataSource = meta;
            grdContentMeta.DataBind();

            btnAddMeta.Visible = true;
        }

        void grdContentMeta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            string sGuid = e.CommandArgument.ToString();
            if (sGuid.Length != 36) { return; }

            Guid guid = new Guid(sGuid);
            ContentMeta meta = metaRepository.Fetch(guid);
            if (meta == null) { return; }

            switch (e.CommandName)
            {
                case "MoveUp":
                    meta.SortRank -= 3;
                    break;

                case "MoveDown":
                    meta.SortRank += 3;
                    break;

            }

            metaRepository.Save(meta);
            List<ContentMeta> metaList = metaRepository.FetchByContent(product.Guid);
            metaRepository.ResortMeta(metaList);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();

            BindMeta();
            upMeta.Update();


        }



        void grdContentMeta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.Delete(guid);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();
        }

        void grdContentMeta_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;

            BindMeta();

            //Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

            Button btnDeleteMeta = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMeta");
            if (btnDeleteMeta != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + WebStoreResources.ContentMetaDeleteWarning + "');");

                //if (guid == Guid.Empty) { btnDeleteMeta.Visible = false; }
            }

            upMeta.Update();
        }

        void grdContentMeta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (grid.EditIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddDirection = (DropDownList)e.Row.Cells[1].FindControl("ddDirection");
                    if (ddDirection != null)
                    {
                        if (e.Row.DataItem is ContentMeta)
                        {
                            ListItem item = ddDirection.Items.FindByValue(((ContentMeta)e.Row.DataItem).Dir);
                            if (item != null)
                            {
                                ddDirection.ClearSelection();
                                item.Selected = true;
                            }
                        }

                    }

                    if (!(e.Row.DataItem is ContentMeta))
                    {
                        //the add button was clicked so hide the delete button
                        Button btnDeleteMeta = (Button)e.Row.Cells[1].FindControl("btnDeleteMeta");
                        if (btnDeleteMeta != null) { btnDeleteMeta.Visible = false; }

                    }

                }

            }

        }

        void grdContentMeta_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;

            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            TextBox txtScheme = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtScheme");
            TextBox txtLangCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtLangCode");
            DropDownList ddDirection = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddDirection");
            TextBox txtMetaContent = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtMetaContent");

            ContentMeta meta = null;
            if (guid != Guid.Empty)
            {
                meta = metaRepository.Fetch(guid);
            }
            else
            {
                meta = new ContentMeta();
                if (siteUser != null) { meta.CreatedBy = siteUser.UserGuid; }
                meta.SortRank = metaRepository.GetNextSortRank(product.Guid);
                meta.ModuleGuid = store.ModuleGuid;
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = product.Guid;
                meta.Dir = ddDirection.SelectedValue;
                meta.LangCode = txtLangCode.Text;
                meta.MetaContent = txtMetaContent.Text;
                meta.Name = txtName.Text;
                meta.Scheme = txtScheme.Text;
                if (siteUser != null) { meta.LastModBy = siteUser.UserGuid; }
                metaRepository.Save(meta);

                product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
                product.Save();

            }

            grid.EditIndex = -1;
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();

        }

        void grdContentMeta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdContentMeta.EditIndex = -1;
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();
        }

        void btnAddMeta_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("ContentGuid", typeof(Guid));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Scheme", typeof(string));
            dataTable.Columns.Add("LangCode", typeof(string));
            dataTable.Columns.Add("Dir", typeof(string));
            dataTable.Columns.Add("MetaContent", typeof(string));
            dataTable.Columns.Add("SortRank", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["ModuleGuid"] = Guid.Empty;
            row["ContentGuid"] = Guid.Empty;
            row["Name"] = string.Empty;
            row["Scheme"] = string.Empty;
            row["LangCode"] = string.Empty;
            row["Dir"] = string.Empty;
            row["MetaContent"] = string.Empty;
            row["SortRank"] = 3;

            dataTable.Rows.Add(row);

            grdContentMeta.EditIndex = 0;
            grdContentMeta.DataSource = dataTable.DefaultView;
            grdContentMeta.DataBind();
            grdContentMeta.Columns[2].Visible = false;
            btnAddMeta.Visible = false;

            upMeta.Update();

        }

        private void BindMetaLinks()
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            List<ContentMetaLink> meta = metaRepository.FetchLinksByContent(product.Guid);

            grdMetaLinks.DataSource = meta;
            grdMetaLinks.DataBind();

            btnAddMetaLink.Visible = true;
        }

        void btnAddMetaLink_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("ContentGuid", typeof(Guid));
            dataTable.Columns.Add("Rel", typeof(string));
            dataTable.Columns.Add("Href", typeof(string));
            dataTable.Columns.Add("HrefLang", typeof(string));
            dataTable.Columns.Add("SortRank", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["ModuleGuid"] = Guid.Empty;
            row["ContentGuid"] = Guid.Empty;
            row["Rel"] = string.Empty;
            row["Href"] = string.Empty;
            row["HrefLang"] = string.Empty;
            row["SortRank"] = 3;

            dataTable.Rows.Add(row);

            grdMetaLinks.Columns[2].Visible = false;
            grdMetaLinks.EditIndex = 0;
            grdMetaLinks.DataSource = dataTable.DefaultView;
            grdMetaLinks.DataBind();
            btnAddMetaLink.Visible = false;

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (grid.EditIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (!(e.Row.DataItem is ContentMetaLink))
                    {
                        //the add button was clicked so hide the delete button
                        Button btnDeleteMetaLink = (Button)e.Row.Cells[1].FindControl("btnDeleteMetaLink");
                        if (btnDeleteMetaLink != null) { btnDeleteMetaLink.Visible = false; }

                    }

                }

            }
        }

        void grdMetaLinks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.DeleteLink(guid);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();

            grid.Columns[2].Visible = true;
            BindMetaLinks();

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdMetaLinks.EditIndex = -1;
            grdMetaLinks.Columns[2].Visible = true;
            BindMetaLinks();
            updMetaLinks.Update();
        }

        void grdMetaLinks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;

            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TextBox txtRel = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRel");
            TextBox txtHref = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHref");
            TextBox txtHrefLang = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHrefLang");

            ContentMetaLink meta = null;
            if (guid != Guid.Empty)
            {
                meta = metaRepository.FetchLink(guid);
            }
            else
            {
                meta = new ContentMetaLink();
                if (siteUser != null) { meta.CreatedBy = siteUser.UserGuid; }
                meta.SortRank = metaRepository.GetNextLinkSortRank(product.Guid);
                meta.ModuleGuid = store.ModuleGuid;
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = product.Guid;
                meta.Rel = txtRel.Text;
                meta.Href = txtHref.Text;
                meta.HrefLang = txtHrefLang.Text;

                if (siteUser != null) { meta.LastModBy = siteUser.UserGuid; }
                metaRepository.Save(meta);

                product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
                product.Save();

            }

            grid.EditIndex = -1;
            grdMetaLinks.Columns[2].Visible = true;
            BindMetaLinks();
            updMetaLinks.Update();
        }

        void grdMetaLinks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;

            BindMetaLinks();

            Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMetaLink");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + WebStoreResources.ContentMetaLinkDeleteWarning + "');");

                if (guid == Guid.Empty) { btnDelete.Visible = false; }
            }

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            string sGuid = e.CommandArgument.ToString();
            if (sGuid.Length != 36) { return; }

            Guid guid = new Guid(sGuid);
            ContentMetaLink meta = metaRepository.FetchLink(guid);
            if (meta == null) { return; }

            switch (e.CommandName)
            {
                case "MoveUp":
                    meta.SortRank -= 3;
                    break;

                case "MoveDown":
                    meta.SortRank += 3;
                    break;

            }

            metaRepository.Save(meta);
            List<ContentMetaLink> metaList = metaRepository.FetchLinksByContent(product.Guid);
            metaRepository.ResortMeta(metaList);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();

            BindMetaLinks();
            updMetaLinks.Update();
        }


        #endregion




        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = "<a href='"
                    + SiteRoot + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture) + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>&nbsp;&gt;&nbsp;<a href='"
                    + SiteRoot + "/WebStore/AdminProduct.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture) + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "' class='unselectedcrumb'>" + WebStoreResources.ProductAdministrationLink
                    + "</a>";
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.ProductEditHeading);

            litHeading.Text = WebStoreResources.ProductEditHeading;
            btnSave.Text = WebStoreResources.ProductUpdateButton;
            btnSave.ToolTip = WebStoreResources.ProductUpdateButton;

            btnDelete.Text = WebStoreResources.ProductDeleteButton;
            btnDelete.ToolTip = WebStoreResources.ProductDeleteButton;
            btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + WebStoreResources.ProductDeleteWarning + "');");

            btnUpload.Text = WebStoreResources.FileUploadButton;

            edAbstract.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edAbstract.WebEditor.Height = Unit.Parse("350px");
           
            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edDescription.WebEditor.Height = Unit.Parse("350px");

            litSettingsTab.Text = WebStoreResources.ProductSettingsTab;
            litAbstactTab.Text = WebStoreResources.ProductAbstractTab;
            litDescriptionTab.Text = WebStoreResources.ProductDescriptionTab;
            litFullfillmentTab.Text = WebStoreResources.ProductFullfillmentTypeLabel;
            litMetaTab.Text = WebStoreResources.MetaDataTab;
            

            lnkFullfillment.HRef = "#" + tabFullfillment.ClientID;

            btnAddMeta.Text = WebStoreResources.AddMetaButton;
            grdContentMeta.Columns[0].HeaderText = string.Empty;
            grdContentMeta.Columns[1].HeaderText = WebStoreResources.ContentMetaNameLabel;
            grdContentMeta.Columns[2].HeaderText = WebStoreResources.ContentMetaMetaContentLabel;

            btnAddMetaLink.Text = WebStoreResources.AddMetaLinkButton;

            grdMetaLinks.Columns[0].HeaderText = string.Empty;
            grdMetaLinks.Columns[1].HeaderText = WebStoreResources.ContentMetaRelLabel;
            grdMetaLinks.Columns[2].HeaderText = WebStoreResources.ContentMetaMetaHrefLabel;
            

            

        }

        protected string GetRefreshUrl()
        {

            string result = SiteRoot + "/WebStore/AdminProductEdit.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                + "&prod=" + productGuid.ToString();

            return result;

        }

        private string GetReturnUrl()
        {
            return SiteRoot + "/WebStore/AdminProduct.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);
        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);

            

            if (CurrentPage.ContainsModule(moduleId))
            {
                //store = StoreHelper.GetStore(siteSettings.SiteGuid, moduleId);
                store = StoreHelper.GetStore();
            }

            siteUser = SiteUtils.GetCurrentSiteUser();

            productGuid = WebUtils.ParseGuidFromQueryString("prod", productGuid);

            virtualRoot = WebUtils.GetApplicationRoot();

            upLoadPath = Server.MapPath("~/Data/Sites/" + siteSettings.SiteId.ToString()
                + "/webstoreproductfiles/");

            teaserFileBaseUrl = "~/Data/Sites/" + siteSettings.SiteId.ToString()
                + "/webstoreproductpreviewfiles/";

            if (!Directory.Exists(upLoadPath))
            {
                Directory.CreateDirectory(upLoadPath);
            }

            teaserFileBasePath = Server.MapPath(teaserFileBaseUrl);

            if (!Directory.Exists(teaserFileBasePath))
            {
                Directory.CreateDirectory(teaserFileBasePath);
            }

            if (WebConfigSettings.UseGreyBoxProgressForNeatUpload)
            {
                progressBar.Visible = false;
                gbProgressBar.Visible = true;
            }
            else
            {
                progressBar.Visible = true;
                gbProgressBar.Visible = false;
            }
           


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

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            SiteUtils.SetupEditor(edAbstract);
            SiteUtils.SetupEditor(edDescription);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnUpload.Click += new EventHandler(btnUpload_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            btnUploadTeaser.Click += new EventHandler(btnUploadTeaser_Click);
            
            SuppressPageMenu();
            SuppressGoogleAds();

            ScriptConfig.IncludeYuiTabs = true;
            IncludeYuiTabsCss = true;

            grdContentMeta.RowCommand += new GridViewCommandEventHandler(grdContentMeta_RowCommand);
            grdContentMeta.RowEditing += new GridViewEditEventHandler(grdContentMeta_RowEditing);
            grdContentMeta.RowUpdating += new GridViewUpdateEventHandler(grdContentMeta_RowUpdating);
            grdContentMeta.RowCancelingEdit += new GridViewCancelEditEventHandler(grdContentMeta_RowCancelingEdit);
            grdContentMeta.RowDeleting += new GridViewDeleteEventHandler(grdContentMeta_RowDeleting);
            grdContentMeta.RowDataBound += new GridViewRowEventHandler(grdContentMeta_RowDataBound);
            btnAddMeta.Click += new EventHandler(btnAddMeta_Click);

            grdMetaLinks.RowCommand += new GridViewCommandEventHandler(grdMetaLinks_RowCommand);
            grdMetaLinks.RowEditing += new GridViewEditEventHandler(grdMetaLinks_RowEditing);
            grdMetaLinks.RowUpdating += new GridViewUpdateEventHandler(grdMetaLinks_RowUpdating);
            grdMetaLinks.RowCancelingEdit += new GridViewCancelEditEventHandler(grdMetaLinks_RowCancelingEdit);
            grdMetaLinks.RowDeleting += new GridViewDeleteEventHandler(grdMetaLinks_RowDeleting);
            grdMetaLinks.RowDataBound += new GridViewRowEventHandler(grdMetaLinks_RowDataBound);
            btnAddMetaLink.Click += new EventHandler(btnAddMetaLink_Click);

        }

        

        #endregion

    }
}
