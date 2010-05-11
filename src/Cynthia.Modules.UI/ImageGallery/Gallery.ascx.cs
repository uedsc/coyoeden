/// Author:					Joe Audette
/// Created:				2004-11-28
/// Last Modified:			2010-02-12
/// 
/// compact mode = show thumbs with paging and web size of selected thumb in same view
/// normal mode = show all thumbs, no paging, web images shown in Lightbox (iBox)
/// webart mode = show 6 thumbs using lightbox and link to more page

using System;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;


namespace Cynthia.Web.GalleryUI
{
	
	public partial class GalleryControl : SiteModuleControl
    {
        #region Properties 

        protected string EditContentImage = WebConfigSettings.EditContentImage;
        private Literal imageLink;
        private Cynthia.Business.Gallery gallery;
        private string imageFolderPath;
        private int thumbsPerPage = 999;
        private int itemId = -1;
        private string ViewImagePage = "GalleryBrowse.aspx";
        private bool showXiffData = false;
        private int totalRows = 0;
        private bool UseSilverlightSlideshow = false;
        private string SlideShowTheme = "LightTheme";
        private int SlideShowWidth = 640;
        private int SlideShowHeight = 480;
        private bool SlideShowWindowlessMode = false;
        private string baseUrl = string.Empty;
        protected string thumnailBaseUrl = string.Empty;
        protected string webSizeBaseUrl = string.Empty;
        protected string fullSizeBaseUrl = string.Empty;
        
        protected int PageNumber
        {
            get
            {
                object o = ViewState["PageNumber"];
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                return 1;
            }
            set
            {
                ViewState["PageNumber"] = value;
                
            }
        }

        protected int TotalPages
        {
            get
            {
                object o = ViewState["TotalPages"];
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                return 1;
            }
            set
            {
                ViewState["TotalPages"] = value;

            }
        }

        protected bool UseGreyBox
        {
            get
            {
                object o = ViewState["UseGreyBox"];
                if (o != null)
                {
                    return Convert.ToBoolean(o);
                }
                return true;
            }
            set
            {
                ViewState["UseGreyBox"] = value;

            }
        }

        protected bool UseCompactMode
        {
            get
            {
                object o = ViewState["UseCompactMode"];
                if (o != null)
                {
                    return Convert.ToBoolean(o);
                }
                return false;
            }
            set
            {
                ViewState["UseCompactMode"] = value;

            }
        }

        #endregion

		protected void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();

            if (UseSilverlightSlideshow)
            {
                SetupSilverlight();
            }
            else
            {

                if (!Page.IsPostBack)
                {
                    PopulateControls();
                }
            }
		}

        private void SetupSilverlight()
        {
            pnlSilverlight.Visible = true;
            pnlMain.Visible = false;
            slideShow.XmlDataUrl = SiteRoot + "/Services/GalleryDataService.ashx?pageid=" + PageId.ToInvariantString()
                        + "&amp;mid=" + ModuleId.ToInvariantString();

            slideShow.Theme = SlideShowTheme;
            slideShow.Height = SlideShowHeight;
            slideShow.Width = SlideShowWidth;
            slideShow.Windowless = SlideShowWindowlessMode;
        }

        private void PopulateControls()
        {
            pager.CurrentIndex = 1;
            BindRepeater();
            BindImage();
        }

        private void BindRepeater()
        {
            DataTable dt = gallery.GetThumbsByPage(PageNumber, thumbsPerPage);

            if (dt.Rows.Count > 0)
            {
                TotalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
                itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
                totalRows = thumbsPerPage * TotalPages;
            }

            //this handles issue: when redirected back to page from edit page
            //if you deleted the last image on the page an error occurs
            //so decrement the pageNumber
            while (PageNumber > TotalPages)
            {
                PageNumber -= 1;

                dt = gallery.GetThumbsByPage(PageNumber, thumbsPerPage);
                if (dt.Rows.Count > 0)
                {
                    TotalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
                    itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
                }
            }

            if (TotalPages > 1)
            {
                if (this.RenderInWebPartMode)
                {
                    if (totalRows > this.thumbsPerPage)
                    {
                        Literal moreLink = new Literal();
                        moreLink.Text = "<a href='"
                            + SiteRoot
                            + "/" + ViewImagePage
                            + "?ItemID=" + itemId.ToInvariantString()
                            + "&amp;mid=" + ModuleId.ToInvariantString()
                            + "&amp;pageid=" + PageId.ToInvariantString()
                            + "&amp;pagenumber=" + PageNumber.ToInvariantString()
                            + "'>" + GalleryResources.GalleryWebPartMoreLink
                            + "</a>";

                        this.pnlGallery.Controls.Add(moreLink);
                        pager.Visible = false;
                    }

                }
                else
                {
                    pager.ShowFirstLast = true;
                    pager.PageSize = thumbsPerPage;
                    pager.PageCount = TotalPages;
                }
            }
            else
            {
                pager.Visible = false;
            }

            if (UseGreyBox)
            {
                SetupImageArray(dt);

            }

            this.rptGallery.DataSource = dt;
            this.rptGallery.DataBind();

        }

        private void SetupImageArray(DataTable dt)
        {
            if (dt == null) { return; }
            if (dt.Rows.Count == 0) { return; }

            StringBuilder script = new StringBuilder();

            script.Append("var imset" + ModuleId.ToString(CultureInfo.InvariantCulture) + " = [");
            string comma = string.Empty;

            foreach (DataRow row in dt.Rows)
            {

                script.Append(comma);
                script.Append("{'caption':'");
                script.Append(Server.HtmlEncode(row["Caption"].ToString()).HtmlEscapeQuotes());
                script.Append("','url':'");
                script.Append(ImageSiteRoot + webSizeBaseUrl);
                script.Append(row["WebImageFile"].ToString());
                script.Append("'");
                script.Append("}");

                comma = ",";

            }


            script.Append("];");

            gbLauncher.Visible = true;
            gbLauncher.NavigateUrl = Request.RawUrl;
            gbLauncher.ToolTip = GalleryResources.LaunchGalleryLink;
            gbLauncher.ClientClick = "return GB_showImageSet(imset" + ModuleId.ToInvariantString() + ", 1)";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "img" + ModuleId.ToInvariantString(), "\n<script type=\"text/javascript\">\n"
                    + script.ToString()
                    + "\n</script>");


        }

        private void BindImage()
        {
            if (!UseCompactMode) { return; }
            if (itemId == -1) { return; }

            imageLink = new Literal();
           
            GalleryImage galleryImage = new GalleryImage(ModuleId, itemId, this.imageFolderPath);

            imageLink.Text = "<a onclick=\"window.open(this.href,'_blank');return false;\"  "
                + " title='" + Server.HtmlEncode(GalleryResources.GalleryWebImageAltText)
                + "' href='" + ImageSiteRoot
                + fullSizeBaseUrl
                + galleryImage.ImageFile + "' ><img src='"
                + ImageSiteRoot
                + webSizeBaseUrl
                + galleryImage.WebImageFile + "' alt='"
                + Server.HtmlEncode(GalleryResources.GalleryWebImageAltText) + "' /></a>";

            
            pnlGallery.Controls.Clear();
            pnlGallery.Controls.Add(imageLink);
            lblCaption.Text = Page.Server.HtmlEncode(galleryImage.Caption);
            lblDescription.Text = galleryImage.Description;

            if ((showXiffData) && (galleryImage.MetaDataXml.Length > 0))
            {
                xmlMeta.DocumentContent = galleryImage.MetaDataXml;
                string xslPath = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/ImageGallery/GalleryMetaData.xsl");
                xmlMeta.TransformSource = xslPath;

            }
            
        }

        

        void rptGallery_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "setimage":
                    itemId = Convert.ToInt32(e.CommandArgument);
                    BindImage();

                    pager.CurrentIndex = PageNumber;
                    pager.ShowFirstLast = true;
                    pager.PageSize = thumbsPerPage;
                    pager.PageCount = TotalPages;
                    upGallery.Update();
                    break;
            }

        }

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            PageNumber = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = PageNumber;
       
            BindRepeater();
            BindImage();

            upGallery.Update();
        }

       
        private void LoadSettings()
        {
            gallery = new Cynthia.Business.Gallery(ModuleId);
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            Title1.EditUrl = SiteRoot + "/ImageGallery/EditImage.aspx";
            Title1.EditText = GalleryResources.GalleryAddImageLabel;
            Title1.Visible = !this.RenderInWebPartMode;

            if (IsEditable)
            {
                Title1.LiteralExtraMarkup = "&nbsp;<a href='"
                        + SiteRoot
                        + "/ImageGallery/BulkUpload.aspx?pageid=" + PageId.ToInvariantString()
                        + "&amp;mid=" + ModuleId.ToInvariantString()
                        + "' class='ModuleEditLink' title='" + GalleryResources.BulkUploadLink + "'>" + GalleryResources.BulkUploadLink + "</a>";
            }

            if (this.ModuleConfiguration != null)
            {
                Title = this.ModuleConfiguration.ModuleTitle;
                Description = this.ModuleConfiguration.FeatureName;
            }

            UseSilverlightSlideshow = WebUtils.ParseBoolFromHashtable(
                Settings, "UseSilverlightSlideshow", UseSilverlightSlideshow);

            SlideShowWindowlessMode = WebUtils.ParseBoolFromHashtable(
                Settings, "SlideShowWindowlessMode", SlideShowWindowlessMode);

            if (Settings.Contains("SlideShowTheme"))
            {
                SlideShowTheme = Settings["SlideShowTheme"].ToString();
            }

            SlideShowHeight = WebUtils.ParseInt32FromHashtable(
                Settings, "SlideShowHeight", SlideShowHeight);

            SlideShowWidth = WebUtils.ParseInt32FromHashtable(
                Settings, "SlideShowWidth", SlideShowWidth);

            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                baseUrl = "/Data/Sites/" + SiteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + ModuleId.ToInvariantString() + "/"; 
            }
            else
            {
                baseUrl = "/Data/Sites/" + SiteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + ModuleId.ToInvariantString() + "/"; 
            }

            thumnailBaseUrl = baseUrl + "Thumbnails/";
            webSizeBaseUrl = baseUrl + "WebImages/";
            fullSizeBaseUrl = baseUrl + "FullSizeImages/";

            imageFolderPath = HttpContext.Current.Server.MapPath("~" + baseUrl);

            thumbsPerPage = WebUtils.ParseInt32FromHashtable(
                Settings, "GalleryThumbnailsPerPageSetting", thumbsPerPage);

            UseCompactMode = WebUtils.ParseBoolFromHashtable(
                Settings, "GalleryCompactModeSetting", UseCompactMode);
            
            if (RenderInWebPartMode)
            {
                UseCompactMode = false;
                UseSilverlightSlideshow = false;
                thumbsPerPage = 6;
            }

            if (UseCompactMode)
            {
                UseGreyBox = false;
                pnlImageDetails.Visible = false;
            }
            else
            {
                UseGreyBox = true;

                if (!RenderInWebPartMode)
                {
                    thumbsPerPage = 999;
                }
            }

            showXiffData = WebUtils.ParseBoolFromHashtable(
                Settings, "GalleryShowTechnicalDataSetting", false);

            
        }

        private int imageArrayIndex = 1;

        public String GetThumnailImageLink(
            String itemId,
            String thumbnailFile,
            String webImageFile,
            String caption)
        {
            
            String link = string.Empty;

            if (UseGreyBox)
            {
                link = "<a onclick='return GB_showImageSet(imset" + ModuleId.ToInvariantString() + "," + imageArrayIndex.ToInvariantString() + ")' "
                    + "title='" + Server.HtmlEncode(caption)
                    + "' href='"
                    + ImageSiteRoot + webSizeBaseUrl
                    + webImageFile + "'>"
                    + GetThumnailMarkup(thumbnailFile, caption)
                    + "</a>";

                imageArrayIndex++;

            }

            return link;


        }

        //protected String GetFullSizeUrl(String imageFile)
        //{
        //    //return GetImageRoot() + "/FullSizeImages/" + imageFile;
        //    return GetImageRoot() + "/WebImages/" + imageFile;

        //}

        protected String GetThumnailUrl(String thumbnailFile)
        {
            return ImageSiteRoot + thumnailBaseUrl + thumbnailFile;

        }

        protected String GetThumnailMarkup(String thumbnailFile, String caption)
        {
            return "<img  src='" + ImageSiteRoot + thumnailBaseUrl
                + thumbnailFile + "' alt='" + Server.HtmlEncode(caption) + "' />";

        }

        //protected string  GetImageRoot()
        //{
        //    if (useSubFolders)
        //    {
        //        return ImageSiteRoot + "/Data/Sites/" + SiteId.ToInvariantString()
        //            + "/GalleryImages/" + ModuleId.ToInvariantString();
        //    }
        //    else
        //    {
        //        return ImageSiteRoot + "/Data/Sites/" + SiteId.ToInvariantString() + "/GalleryImages";
        //    }
        //}

       

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            pager.Command += new CommandEventHandler(pager_Command);
            rptGallery.ItemCommand += new RepeaterCommandEventHandler(rptGallery_ItemCommand);
            Page.EnableViewState = true;
        }

    }
}
