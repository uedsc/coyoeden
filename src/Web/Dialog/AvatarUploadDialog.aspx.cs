//  Author:                     Joe Audette
//  Created:                    2009-09-21
//	Last Modified:              2009-09-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Brettle.Web.NeatUpload;
using Resources;

namespace Cynthia.Web.Dialog
{
    /// <summary>
    /// a page to upload and crop user avatars
    /// </summary>
    public partial class AvatarUploadDialog : Page
    {
        private int userId = -1;
        private bool disableAvatars = true;
        private bool canEdit = false;
        private string avatarBasePath = string.Empty;
        private SiteUser currentUser = null;
        private SiteUser selectedUser = null;
        private SiteSettings siteSettings = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if(disableAvatars)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateLabels();

            //if (!Page.IsPostBack)
            //{
                PopulateControls();
            //}

        }

        private void PopulateControls()
        {
            if (selectedUser == null) { return; }

            if (selectedUser.AvatarUrl.Length > 0)
            {
                cropper.ResultImagePath = avatarBasePath + selectedUser.AvatarUrl;
                string fullSizeFileName = GetFullSizeImageName();
                if (fullSizeFileName.Length > 0)
                {
                    cropper.SourceImagePath = avatarBasePath + fullSizeFileName;
                }
            }

            if (WebConfigSettings.ForceSquareAvatars) { cropper.AspectRatio = 1; }

            cropper.MinWidth = WebConfigSettings.AvatarMaxWidth;
            cropper.MinHeight = WebConfigSettings.AvatarMaxHeight;

            cropper.FinalMaxWidth = WebConfigSettings.AvatarMaxWidth;
            cropper.FinalMaxHeight = WebConfigSettings.AvatarMaxHeight;

            cropper.InitialSelectionX = 100;
            cropper.InitialSelectionY = 100;
            cropper.InitialSelectionX2 = cropper.InitialSelectionX + cropper.FinalMaxWidth;
            cropper.InitialSelectionY2 = cropper.InitialSelectionY + cropper.FinalMaxHeight;
        }

        private string GetFullSizeImageName()
        {
            string fullSizeFileName = string.Empty;
            if (selectedUser.AvatarUrl.Length > 0)
            {
                fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize" + Path.GetExtension(selectedUser.AvatarUrl);
                if(File.Exists(Server.MapPath(avatarBasePath + fullSizeFileName))){ return fullSizeFileName;}
            }

            fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize.jpg";
            if (File.Exists(Server.MapPath(avatarBasePath + fullSizeFileName))) { return fullSizeFileName; }

            fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize.gif";
            if (File.Exists(Server.MapPath(avatarBasePath + fullSizeFileName))) { return fullSizeFileName; }

            fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize.png";
            if (File.Exists(Server.MapPath(avatarBasePath + fullSizeFileName))) { return fullSizeFileName; }

            
            return string.Empty;

        }

        void btnUploadAvatar_Click(object sender, EventArgs e)
        {
            if (selectedUser == null) { return; }


            if (avatarFile != null && avatarFile.FileName != null && avatarFile.FileName.Trim().Length > 0)
            {
                string destFolder = Server.MapPath(avatarBasePath);

                if (!Directory.Exists(destFolder)) { Directory.CreateDirectory(destFolder); }
                string newFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize" + Path.GetExtension(avatarFile.FileName).ToLower();

                string destPath = Path.Combine(destFolder, newFileName);
                string ext = Path.GetExtension(avatarFile.FileName);
                if (SiteUtils.IsAllowedUploadBrowseFile(ext, SiteUtils.ImageFileExtensions()))
                {
                    if (File.Exists(destPath))
                    {
                        File.Delete(destPath);
                    }
                    avatarFile.MoveTo(destPath, MoveToOptions.Overwrite);
                    avatarFile.Dispose();

                    // limit the size of the full size image to something reasonable
                    ImageHelper.ResizeImage(destPath, IOHelper.GetMimeType(ext), WebConfigSettings.AvatarMaxOriginalWidth, WebConfigSettings.AvatarMaxOriginalHeight);

                    //create initial crop
                    string croppedFileName = "user" + selectedUser.UserId.ToInvariantString() + Path.GetExtension(avatarFile.FileName).ToLower();
                    string destCropFile = Path.Combine(destFolder, croppedFileName);

                    File.Copy(destPath, destCropFile, true);
                    if (WebConfigSettings.ForceSquareAvatars)
                    {
                        ImageHelper.ResizeAndSquareImage(destCropFile, IOHelper.GetMimeType(ext), WebConfigSettings.AvatarMaxWidth);
                    }
                    else
                    {
                        ImageHelper.ResizeImage(destCropFile, IOHelper.GetMimeType(ext), WebConfigSettings.AvatarMaxWidth, WebConfigSettings.AvatarMaxHeight);
                    }

                    selectedUser.AvatarUrl = croppedFileName;
                    selectedUser.Save();


                }
                else
                {
                    avatarFile.Dispose();
                }
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void PopulateLabels()
        {
            btnUploadAvatar.Text = Resource.UploadAvatarButton;
            regexAvatarFile.ErrorMessage = Resource.FileTypeNotAllowed;
            regexAvatarFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(SiteUtils.ImageFileExtensions());
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            avatarBasePath = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/");
            userId = WebUtils.ParseInt32FromQueryString("u", true, userId);
            currentUser = SiteUtils.GetCurrentSiteUser();
            if ((currentUser != null) && (currentUser.UserId == userId) && (userId != -1)) 
            {
                selectedUser = currentUser;
                canEdit = true; 
            }

            if (WebUser.IsAdmin) 
            { 
                canEdit = true;
                if ((selectedUser == null) && (userId != -1))
                {
                    selectedUser = new SiteUser(siteSettings, userId);
                    if (selectedUser.UserId != userId) { selectedUser = null; }
                }
            }
           

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    
                    disableAvatars = true;
                    break;

                case "internal":
                    
                    disableAvatars = false;
                   
                    if ((WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)&&(!WebUser.IsAdmin))
                    {
                        canEdit = false;  
                    }

                    break;

                case "none":
                default:
                    
                    disableAvatars = true;
                    break;

            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            btnUploadAvatar.Click += new EventHandler(btnUploadAvatar_Click);
        }

    }
}
