/// Author:					Joe Audette
/// Created:				2008-02-08
/// Last Modified:			2009-11-03
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using Cynthia.Web.Framework;
using Cynthia.Business.WebHelpers;
using Cynthia.Business;
using Brettle.Web.NeatUpload;
using Resources;

namespace Cynthia.Web.GalleryUI
{
   
    public partial class FolderGalleryEditPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        
        private Hashtable moduleSettings = null;
        private string basePath = string.Empty;
        //private bool userIdIsEditUserId = false;
        private bool allowEditUsersToChangeFolderPath = true;
        private bool allowEditUsersToUpload = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();

            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            
            lblBasePath.Text = basePath;
            

            if (Page.IsPostBack) return;

            if (moduleSettings.Contains("FolderGalleryRootFolder"))
            {
                txtFolderName.Text = moduleSettings["FolderGalleryRootFolder"].ToString().Replace(basePath, string.Empty);
            }

        }

        

        void btnSave_Click(object sender, EventArgs e)
        {
            Module m = new Module(moduleId);

            string newPath = basePath + txtFolderName.Text;
            try
            {
                if (!Directory.Exists(Server.MapPath(newPath)))
                {
                    lblError.Text = FolderGalleryResources.FolderGalleryFolderNotExistsMessage;
                    return;
                }
            }
            catch (HttpException)
            {
                //thrown at Server.MapPath if the path is not a valid virtual path
                txtFolderName.Text = string.Empty;
                lblError.Text = FolderGalleryResources.FolderGalleryFolderNotExistsMessage;
                return;

            }


            ModuleSettings.UpdateModuleSetting(
                m.ModuleGuid,
                m.ModuleId,
                "FolderGalleryRootFolder",
                newPath);

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        void btnUpload_Click(object sender, EventArgs e)
        {
            string pathToGallery = basePath;

            if (moduleSettings.Contains("FolderGalleryRootFolder"))
            {
                pathToGallery = moduleSettings["FolderGalleryRootFolder"].ToString();

            }


            

            try
            {

                if (multiFile.Files.Length > 0)
                {
                    
                    foreach (UploadedFile file in multiFile.Files)
                    {
                        if (file != null && file.FileName != null && file.FileName.Trim().Length > 0)
                        {
                            string ext = Path.GetExtension(file.FileName);
                            if (SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.gif|.png"))
                            {
                                string destPath = Path.Combine(Server.MapPath(pathToGallery), Path.GetFileName(file.FileName));
                                if (File.Exists(destPath))
                                {
                                    File.Delete(destPath);
                                }
                                file.MoveTo(destPath, MoveToOptions.Overwrite);
                            }
                        }
                       
                    }
                }

                //InputFile[] files = new InputFile[] { file1, file2, file3, file4, file5, file6, file7, file8, file9, file10 };
                //foreach (InputFile file in files)
                //{
                //    if (file != null && file.FileName != null && file.FileName.Trim().Length > 0 && IsImageFile(file))
                //    {
                //        string destPath = Path.Combine(Server.MapPath(pathToGallery), Path.GetFileName(file.FileName));
                //        if (File.Exists(destPath))
                //        {
                //            File.Delete(destPath);
                //        }
                //        file.MoveTo(destPath, MoveToOptions.Overwrite);
                //    }
                //}

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

        private bool IsImageFile(InputFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                    return true;

                default:
                    return false;

            }


        }

        //void btnCancel_Click(object sender, EventArgs e)
        //{
        //    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        //}


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, FolderGalleryResources.EditPageTitle);

            btnSave.Text = FolderGalleryResources.FolderGallerySaveButton;
            lnkCancel.Text = FolderGalleryResources.FolderGalleryCancelButton;
            //btnCancel.Text = FolderGalleryResources.FolderGalleryCancelButton;
            lblError.Text = string.Empty;

            btnUpload.Text = FolderGalleryResources.FolderGalleryUploadImagesButton;
            regexUpload.ErrorMessage = FolderGalleryResources.AllowedExtensionsMessage;
            btnAddFile.Text = FolderGalleryResources.AddFileButton;

        }

        private void LoadSettings()
        {
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            CSetup.EnsureFolderGalleryFolder(siteSettings);

            pageId = WebUtils.ParseInt32FromQueryString("PageId", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            basePath = "~/Data/Sites/" 
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/FolderGalleries/";

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            allowEditUsersToChangeFolderPath = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "AllowEditUsersToChangeFolderPath", allowEditUsersToChangeFolderPath);

            allowEditUsersToUpload = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "AllowEditUsersToUpload", allowEditUsersToUpload);

            if (!WebUser.IsAdminOrContentAdmin)
            {
                pnlUpload.Visible = allowEditUsersToUpload;
                pnlEdit.Visible = allowEditUsersToChangeFolderPath;
            }

           
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            //this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnUpload.Click += new EventHandler(btnUpload_Click);


        }

        

        

        #endregion
    }
}
