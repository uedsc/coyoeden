/// Author:					Joe Audette
/// Created:				2008-02-07
/// Last Modified:			2009-03-18
/// ApplicationGuid:		C1541B8E-ABF0-4051-8CD2-DB84795B26BD
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Web;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Resources;


namespace Cynthia.Web.GalleryUI
{
    public partial class FolderGalleryModule : SiteModuleControl
    {
        protected bool ShowPermaLinksSetting = false;
        protected bool ShowMetaDetailsSetting = false;
        
        #region OnInit

        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            
            string pathToGallery = string.Empty;

            if (Settings.Contains("FolderGalleryRootFolder"))
            {
                pathToGallery = Settings["FolderGalleryRootFolder"].ToString();

            }
            else
            {
                pathToGallery = GetDefaultGalleryPath();
                if (pathToGallery.Length > 0) CreateDefaultFolderSetting(pathToGallery);
                    
            }

            if (pathToGallery.Length == 0)
            {
                pathToGallery = GetDefaultGalleryPath();
                if (pathToGallery.Length > 0) CreateDefaultFolderSetting(pathToGallery);
            }

            try
            {

                if (!Directory.Exists(Server.MapPath(pathToGallery)))
                    pathToGallery = GetDefaultGalleryPath();
            }
            catch (HttpException)
            {
                //thrown at Server.MapPath if the path is not a valid virtual path
                pathToGallery = GetDefaultGalleryPath();

            }

            ShowPermaLinksSetting = WebUtils.ParseBoolFromHashtable(Settings, "ShowPermaLinksSetting", ShowPermaLinksSetting);
            ShowMetaDetailsSetting = WebUtils.ParseBoolFromHashtable(Settings, "ShowMetaDetailsSetting", ShowMetaDetailsSetting);
            

            this.Album1.GalleryRootPath = pathToGallery;
            this.Album1.DoSetup();

        }

        

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            TitleControl.EditUrl = SiteRoot + "/FolderGallery/Edit.aspx";

            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

        }

        void btnMakeActive_Click(object sender, EventArgs e)
        {
            if (this.ModuleConfiguration == null) return;

            ModuleSettings.UpdateModuleSetting(
                this.ModuleConfiguration.ModuleGuid,
                this.ModuleConfiguration.ModuleId,
                "FolderGalleryRootFolder",
                Album1.Path);
        }

        private void CreateDefaultFolderSetting(string pathToGallery)
        {
            if(this.ModuleConfiguration == null)return;

            ModuleSettings.CreateModuleSetting(
                this.ModuleConfiguration.ModuleGuid,
                this.ModuleConfiguration.ModuleId,
                "FolderGalleryRootFolder",
                pathToGallery,
                "TextBox",
                string.Empty,
                string.Empty,
                string.Empty,
                100);

        }

        private string GetDefaultGalleryPath()
        {
            if (SiteSettings == null) return string.Empty;

            return "~/Data/Sites/" + SiteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
                + "/FolderGalleries/";
        }


        private void PopulateLabels()
        {
            TitleControl.EditText = FolderGalleryResources.FolderGalleryEditLink;
            
        }

        private void LoadSettings()
        {
            CSetup.EnsureFolderGalleryFolder(SiteSettings);

        }


    }
}
