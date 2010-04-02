//  Author:                     Joe Audette
//  Created:                    2009-09-20
//	Last Modified:              2009-09-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
//using Brettle.Web.NeatUpload;

namespace Cynthia.Web.Dialog
{
    /// <summary>
    /// A dialog page for cropping images
    /// </summary>
    public partial class ImageCropperDialog : System.Web.UI.Page
    {
        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private string rootDirectory = "/Data/";
        private bool canEdit = false;
        private string sourceImageVirtualPath = string.Empty;
        private string destImageFileName = string.Empty;
        private string sourceImagePath = string.Empty;
        private string destImagePath = string.Empty;
        private string destImageUrl = string.Empty;
        private bool sourceExists = false;
        private bool isAllowedPath = false;
        private string returnUrl = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return; 
            }

            if (isAllowedPath) { SetupCropper(); }

        }

        private void SetupCropper()
        {
            cropper.SourceImagePath = sourceImageVirtualPath;
            cropper.ResultImagePath = destImageUrl;
            cropper.AllowUserToChooseCroppedFileName = true;
            cropper.AllowUserToSetFinalFileSize = true;
            cropper.InitialSelectionX = 100;
            cropper.InitialSelectionY = 100;
            cropper.InitialSelectionX2 = 50;
            cropper.InitialSelectionY2 = 50;
        }

        private void PopulateLabels()
        {
            this.Title = Resource.CropImageLink;
            lnkReturn.Text = Resource.CropperGoBackLink;
        }

        private void LoadSettings()
        {
            if (Request.QueryString["return"] != null)
            {
                returnUrl = Request.QueryString["return"];
                lnkReturn.NavigateUrl = returnUrl;
                lnkReturn.Visible = true;
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            if (WebUser.IsAdminOrContentAdmin)
            {
                //rootDirectory = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/");
                canEdit = true;

                if (WebConfigSettings.ForceAdminsToUseMediaFolder)
                {
                    rootDirectory = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/");
                }
                else
                {
                    if ((WebConfigSettings.AllowAdminsToUseDataFolder) && (WebUser.IsAdmin))
                    {
                        rootDirectory = Page.ResolveUrl("~/Data/");
                    }
                    else
                    {
                        rootDirectory = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/");
                    }
                }

            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                rootDirectory = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/");
                canEdit = true;

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null) { return; }

                rootDirectory = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/userfiles/" + currentUser.UserId.ToInvariantString() + "/");
                canEdit = true;
            }

            if (Request.QueryString["src"] != null)
            {
                sourceImageVirtualPath = Request.QueryString["src"];

                if (sourceImageVirtualPath.Length > 0)
                {
                    sourceImagePath = Server.MapPath(sourceImageVirtualPath);
                    sourceExists = File.Exists(sourceImagePath);
                    isAllowedPath = IOHelper.IsDecendentFile(rootDirectory, sourceImageVirtualPath);
                }
            }

            if (sourceImagePath.Length == 0) 
            {
                cropper.Visible = false;
                return; 
            }

            if (Request.QueryString["dest"] != null)
            {
                destImageFileName = Request.QueryString["dest"].ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                
            }

            if (destImageFileName.Length > 0)
            {
                destImagePath = Path.Combine(Path.GetDirectoryName(sourceImagePath), Path.GetFileNameWithoutExtension(destImageFileName) + Path.GetExtension(sourceImagePath));
                destImageUrl = sourceImageVirtualPath.Replace(Path.GetFileName(sourceImagePath), string.Empty) 
                    + Path.GetFileNameWithoutExtension(destImageFileName) + Path.GetExtension(sourceImagePath);
            }
            else
            {
                destImagePath = Path.Combine(Path.GetDirectoryName(sourceImagePath), Path.GetFileNameWithoutExtension(sourceImagePath) + "crop" + Path.GetExtension(sourceImagePath));
                destImageUrl = sourceImageVirtualPath.Replace(Path.GetFileName(sourceImagePath), string.Empty)
                    + Path.GetFileNameWithoutExtension(sourceImagePath) + "crop" + Path.GetExtension(sourceImagePath);
            }

            SiteUtils.SetFormAction(Page, Request.RawUrl);


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
        }
    }
}
