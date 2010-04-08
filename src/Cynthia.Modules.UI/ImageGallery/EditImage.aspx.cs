/// Author:				        Joe Audette
/// Created:			        2004-11-28
///	Last Modified:              2010-02-12

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using System.IO;
using System.Web;
using Brettle.Web.NeatUpload;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Cynthia.Modules.UI.ImageGallery;
using Resources;

namespace Cynthia.Web.GalleryUI
{
    public partial class GalleryImageEdit : CBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(GalleryImageEdit));

		private string imageFolderPath;
        private string fullSizeImageFolderPath;
        private int moduleId = -1;
        private int itemId = -1;
        private int webImageHeightSetting = -1;
        private int webImageWidthSetting = -1;
        private int thumbNailHeightSetting = -1;
        private int thumbNailWidthSetting = -1;
        private Hashtable moduleSettings;
        private string thumbnailBaseUrl = string.Empty;
        private String cacheDependencyKey;
        private String appRoot;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            
            SiteUtils.SetupEditor(edDescription);
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            //this.btnCancel.Click += new EventHandler(btnCancel_Click);
            
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadParams();
            

            if (!UserCanEditModule(moduleId))
			{
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            LoadSettings();
			PopulateLabels();
			
			if (!IsPostBack) 
			{
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;
                }

				if (itemId > -1)
				{
					PopulateControls();
					
				}
				else
				{
					this.btnDelete.Visible = false;
				}
			}

		}

		
		private void PopulateControls()
		{
			if (log.IsDebugEnabled) log.Debug("in PopulateControls");

			if(moduleId > -1)
			{
				if(itemId > -1)
				{
					GalleryImage galleryImage = new GalleryImage(moduleId, itemId, imageFolderPath);
					txtCaption.Text = Server.HtmlDecode(galleryImage.Caption);
					edDescription.Text = galleryImage.Description;
					txtDisplayOrder.Text = galleryImage.DisplayOrder.ToString();
                    imgThumb.Src = thumbnailBaseUrl + galleryImage.ThumbnailFile;

				}

			}

		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			if (log.IsDebugEnabled) log.Debug("in btnUpdate_Click");
		    
            if (!Page.IsValid) return;
            if (log.IsDebugEnabled) log.Debug("Page.IsValid");
		    
            GalleryImage galleryImage;
		    if(moduleId > -1)
		    {
		        if(itemId > -1)
		        {
		            galleryImage = new GalleryImage(moduleId, itemId, imageFolderPath);
		        }
		        else
		        {
		            galleryImage = new GalleryImage(moduleId, imageFolderPath);
		        }

                Module module = new Module(moduleId);
                galleryImage.ModuleGuid = module.ModuleGuid;

                galleryImage.ContentChanged += new ContentChangedEventHandler(galleryImage_ContentChanged);

		        int displayOrder;
		        if (!Int32.TryParse(txtDisplayOrder.Text, out displayOrder))
		        {
		            displayOrder = -1;
		        }

		        if (displayOrder > -1)
		        {
		            galleryImage.DisplayOrder = displayOrder;
		        }

		        if (webImageHeightSetting > -1)
		        {
		            galleryImage.WebImageHeight = webImageHeightSetting;
		        }

		        if (webImageWidthSetting > -1)
		        {
		            galleryImage.WebImageWidth = webImageWidthSetting;
		        }

		        if (thumbNailHeightSetting > -1)
		        {
		            galleryImage.ThumbNailHeight = thumbNailHeightSetting;
		        }

		        if (thumbNailWidthSetting > -1)
		        {
		            galleryImage.ThumbNailWidth = thumbNailWidthSetting;
		        }

		        galleryImage.Description = edDescription.Text;
		        galleryImage.Caption = txtCaption.Text;
		        galleryImage.UploadUser = Context.User.Identity.Name;
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null) galleryImage.UserGuid = siteUser.UserGuid;

		        if (flImage.HasFile && flImage.FileName != null && flImage.FileName.Trim().Length > 0)
		        {
                    string ext = Path.GetExtension(flImage.FileName);
                    if (!SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.gif|.png|.jpeg"))
                    {
                        lblMessage.Text = GalleryResources.InvalidFile;
                        
                        return;
                    }
                    // 2010-02-12 change from previous implementation with ugly guid file names
                    string newFileName = Path.GetFileName(flImage.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

                    if (galleryImage.ImageFile == newFileName)
                    {
                        // an existing gallery image delete the old one
                        if (File.Exists(fullSizeImageFolderPath + newFileName))
                        {
                            File.Delete(fullSizeImageFolderPath + newFileName);
                        }
                    }
                    else
                    {
                        // this is a new galleryImage instance, make sure we don't use the same file name as any other instance
                        int i = 1;
                        while (File.Exists(fullSizeImageFolderPath + newFileName))
                        {
                            newFileName = i.ToInvariantString() + newFileName;
                            i += 1;
                        }

                    }

                    string destPath = fullSizeImageFolderPath + newFileName;
		            flImage.MoveTo(destPath,MoveToOptions.Overwrite);

                    galleryImage.ImageFile = newFileName;
                    galleryImage.WebImageFile = newFileName;
                    galleryImage.ThumbnailFile = newFileName;

		            //galleryImage.ProcessImage(flImage.FileName);
                    GalleryHelper.ProcessImage(galleryImage, flImage.FileName);

                    CurrentPage.UpdateLastModifiedTime();
		            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    SiteUtils.QueueIndexing();
		            if (ViewState["UrlReferrer"] != null)
		            {
		                WebUtils.SetupRedirect(this, (string)ViewState["UrlReferrer"]);
		                return;
		            }
                        
		        }
		        else
		        {	//updating a previously uploaded image
		            if(itemId > -1)
		            {
		                if(galleryImage.Save())
		                {
                            CurrentPage.UpdateLastModifiedTime();
		                    CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                            SiteUtils.QueueIndexing();
                            if (hdnReturnUrl.Value.Length > 0)
                            {
                                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                                return;
                            }

                            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                                
		                }
		            }
		        }	
		    }
		}

        

        void galleryImage_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["GalleryImageIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if(moduleId > -1)
			{
				if(itemId > -1)
				{
					GalleryImage galleryImage = new GalleryImage(moduleId, itemId, imageFolderPath);
                    galleryImage.ContentChanged += new ContentChangedEventHandler(galleryImage_ContentChanged);
                   
                    galleryImage.DeleteImages();
                    galleryImage.Delete();
                    CurrentPage.UpdateLastModifiedTime();
                    CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    SiteUtils.QueueIndexing();

				}
			}

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, GalleryResources.EditImagePageTitle);

            btnUpdate.Text = GalleryResources.GalleryEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, GalleryResources.GalleryEditUpdateButtonAccessKey);

            lnkCancel.Text = GalleryResources.GalleryEditCancelButton;
            //btnCancel.Text = GalleryResources.GalleryEditCancelButton;
            //SiteUtils.SetButtonAccessKey(btnCancel, GalleryResources.GalleryEditCancelButtonAccessKey);

            btnDelete.Text = GalleryResources.GalleryEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, GalleryResources.GalleryEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, GalleryResources.GalleryDeleteImageWarning);

            imgThumb.Src = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");

            regexFile.ErrorMessage = GalleryResources.OnlyImageFilesAllowed;

            
        }

        private void LoadSettings()
        {
            appRoot = WebUtils.GetApplicationRoot();

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            

            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            CSetup.VerifyGalleryFolders(siteSettings.SiteId, moduleId);

            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                imageFolderPath = HttpContext.Current.Server.MapPath("~/Data/Sites/" 
                    + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/");

                thumbnailBaseUrl = ImageSiteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/Thumbnails/"; 
            }
            else
            {
                imageFolderPath = HttpContext.Current.Server.MapPath("~/Data/Sites/"
                    + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/");

                thumbnailBaseUrl = ImageSiteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/Thumbnails/"; 
            }

            fullSizeImageFolderPath = imageFolderPath + "FullSizeImages" + Path.DirectorySeparatorChar;

            

            
            webImageHeightSetting = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "GalleryWebImageHeightSetting", -1);

            webImageWidthSetting = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "GalleryWebImageWidthSetting", -1);

            thumbNailHeightSetting = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "GalleryThumbnailHeightSetting", -1);

            thumbNailWidthSetting = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "GalleryThumbnailWidthSetting", -1);

            
            edDescription.WebEditor.ToolBar = ToolBar.Full;

            

           
        }

 
        private void LoadParams()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);

            cacheDependencyKey = "Module-" + moduleId.ToString();

        }

	}
}
