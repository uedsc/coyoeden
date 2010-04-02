/// Author:				        Joe Audette
/// Created:			        2004-11-28
/// Last Modified:		        2010-02-12

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.GalleryUI
{
    public partial class GalleryBrowse : CBasePage
	{
        private Literal imageLink = new Literal();
        private string imageFolderPath;
        private int pageNumber = 1;
        private int totalPages = 1;
        private int pageId = -1;
        private int moduleId = -1;
        private int itemId = -1;
        private Hashtable moduleSettings;
        //private string pageNumberParam;
        private bool showTechnicalData = false;
        protected string webSizeBaseUrl = string.Empty;
        protected string fullSizeBaseUrl = string.Empty;

        private static readonly ILog log = LogManager.GetLogger(typeof(GalleryBrowse));

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
            SuppressPageMenu();
            SuppressMenuSelection();
        }

        #endregion

        private void Page_Load(object sender, System.EventArgs e)
		{
            GetRequestParams();

            if (!UserCanViewPage())
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, GalleryResources.BrowseGalleryPageTitle);

            LoadSettings();
			ShowImage();

		}

		private void ShowImage()
		{
            if (moduleId == -1) { return; }
            
            Gallery gallery = new Gallery(moduleId);
            DataTable dt = gallery.GetWebImageByPage(pageNumber);

            if (dt.Rows.Count > 0)
            {
                itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
                totalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
            }

            
            showTechnicalData = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GalleryShowTechnicalDataSetting", false);

            if (itemId == -1) { return; }
                
			Literal topPageLinks = new Literal();
			string pageUrl = SiteRoot
				+ "/ImageGallery/GalleryBrowse.aspx?"
                + "pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture) 
				+ "&amp;pagenumber=";

			topPageLinks.Text = UIHelper.GetPagerLinksWithPrevNext(
                pageUrl,1, 
                this.totalPages, 
                this.pageNumber, 
                "modulepager", 
                "SelectedPage");

			this.spnTopPager.Controls.Add(topPageLinks);
			
			GalleryImage galleryImage = new GalleryImage(moduleId, itemId, this.imageFolderPath);

			
            imageLink.Text = "<a onclick=\"window.open(this.href,'_blank');return false;\"  href='" + ImageSiteRoot 
				+ fullSizeBaseUrl + galleryImage.ImageFile + "' ><img  src='"
                + ImageSiteRoot + webSizeBaseUrl
				+ galleryImage.WebImageFile + "' alt='"
                + Resources.GalleryResources.GalleryWebImageAltText + "' /></a>";

			
			
			this.pnlGallery.Controls.Add(imageLink);
            this.lblCaption.Text = Server.HtmlEncode(galleryImage.Caption);
			this.lblDescription.Text = galleryImage.Description;

			if(showTechnicalData)
			{
				if(galleryImage.MetaDataXml.Length > 0)
				{
                    xmlMeta.DocumentContent = galleryImage.MetaDataXml;
                    string xslPath = System.Web.HttpContext.Current.Server.MapPath(SiteRoot + "/ImageGallery/GalleryMetaData.xsl");
                    xmlMeta.TransformSource = xslPath;
				}
			}

				

		}

        private void LoadSettings()
        {
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            string baseUrl;
            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                baseUrl = "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }
            else
            {
                baseUrl = "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }

            
            webSizeBaseUrl = baseUrl + "WebImages/";
            fullSizeBaseUrl = baseUrl + "FullSizeImages/";

            imageFolderPath = HttpContext.Current.Server.MapPath("~" + baseUrl);
           
            
           
        }

        private void GetRequestParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

        }

	}
}
