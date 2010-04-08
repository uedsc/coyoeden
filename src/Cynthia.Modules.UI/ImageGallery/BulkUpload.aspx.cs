// Author:					Joe Audette
// Created:					2009-09-25
// Last Modified:			2009-09-25
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
using System.IO;
using System.Web;
using Brettle.Web.NeatUpload;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Cynthia.Modules.UI.ImageGallery;
using Resources;



namespace Cynthia.Web.GalleryUI
{

    public partial class BulkUploadPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string imageFolderPath;
        private string fullSizeImageFolderPath;
        private int webImageHeightSetting = -1;
        private int webImageWidthSetting = -1;
        private int thumbNailHeightSetting = -1;
        private int thumbNailWidthSetting = -1;
        private Hashtable moduleSettings;
        private bool useSubFolders = true;
       
        private String appRoot;

        protected void Page_Load(object sender, EventArgs e)
        {
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
            }

        }

       

        void btnUpload_Click(object sender, EventArgs e)
        {
            Module module = new Module(moduleId);
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

            try
            {

                if (multiFile.Files.Length > 0)
                {

                    foreach (UploadedFile file in multiFile.Files)
                    {
                        if (file != null && file.FileName != null && file.FileName.Trim().Length > 0)
                        {
                            string ext = Path.GetExtension(file.FileName);
                            if (SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.gif|.png|.jpeg"))
                            {
                                GalleryImage galleryImage = new GalleryImage(this.moduleId, this.imageFolderPath);
                                galleryImage.ModuleGuid = module.ModuleGuid;

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


                                galleryImage.UploadUser = Context.User.Identity.Name;

                                if (siteUser != null) galleryImage.UserGuid = siteUser.UserGuid;

                                //string destPath = this.fullSizeImageFolderPath + galleryImage.ImageFile;
                                //if (File.Exists(destPath))
                                //{
                                //    File.Delete(destPath);
                                //}
                                //file.MoveTo(destPath, MoveToOptions.Overwrite);

                                // 2010-02-12 change from previous implementation with ugly guid file names
                                string newFileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

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
                                file.MoveTo(destPath, MoveToOptions.Overwrite);

                                galleryImage.ImageFile = newFileName;
                                galleryImage.WebImageFile = newFileName;
                                galleryImage.ThumbnailFile = newFileName;

                                GalleryHelper.ProcessImage(galleryImage, file.FileName);
                            }

                        }

                    }
                }
                
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

            }
            catch (UnauthorizedAccessException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (ArgumentException ex)
            {
                lblError.Text = ex.Message;
            }


        }


        private void PopulateLabels()
        {
            btnAddFile.Text = GalleryResources.AddFileButton;
            btnUpload.Text = GalleryResources.BulkUploadButton;
            lnkCancel.Text = GalleryResources.GalleryEditCancelButton;
        }

        private void LoadSettings()
        {
            //appRoot = WebUtils.GetApplicationRoot();

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            

            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            CSetup.VerifyGalleryFolders(siteSettings.SiteId, this.moduleId);

            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                imageFolderPath = HttpContext.Current.Server.MapPath("~/Data/Sites/"
                    + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/");
            }
            else
            {
                imageFolderPath = HttpContext.Current.Server.MapPath("~/Data/Sites/"
                    + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/");
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
            btnUpload.Click += new EventHandler(btnUpload_Click);

        }

        

        #endregion
    }
}
