//  Author:                     Joe Audette
//  Created:                    2009-08-16
//	Last Modified:              2010-01-24
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
using Brettle.Web.NeatUpload;


namespace Cynthia.Web.Dialog
{
    /// <summary>
    /// A dialog page for file browse and upload in TinyMCE Editor
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Index
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Custom_filebrowser
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/template
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration/theme_advanced_blockformats 
    /// http://abeautifulsite.net/notebook/58
    /// http://plugins.jquery.com/project/filetree
    /// </summary>
    public partial class FileDialog : Page
    {
        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private string rootDirectory = "/Data/";
        private string navigationRoot = string.Empty;
        private string browserType = "image";
        private string editorType = string.Empty;
        private string currentDir = string.Empty;
        private bool canEdit = false;
        private bool userCanDeleteFiles = false;
        private string allowedExtensions = string.Empty;
        private int resizeWidth = 550;
        private int resizeHeight = 550;
        private string imageCropperUrl = string.Empty;
        private string CKEditor = string.Empty;
        private string CKEditorFuncNum = string.Empty;
        private string langCode = string.Empty;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            SetupScripts();

        }


        void btnNewFolder_Click(object sender, EventArgs e)
        {
            if(!canEdit){ return; }

            if ((hdnFolder.Value.Length > 0) && (hdnFolder.Value != rootDirectory))
            {
                currentDir = hdnFolder.Value;
            }

            try
            {
                Directory.CreateDirectory(Path.Combine(GetCurrentDirectory(), Path.GetFileName(txtNewDirectory.Text).ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation)));
                txtNewDirectory.Text = "";
                WebUtils.SetupRedirect(this, GetRedirectUrl());
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

        void btnUpload_Click(object sender, EventArgs e)
        {
            if ((hdnFolder.Value.Length > 0)&&(hdnFolder.Value != rootDirectory))
            { 
                currentDir = hdnFolder.Value; 
            }
            if (!canEdit) 
            {
                WebUtils.SetupRedirect(this, navigationRoot + "/Dialog/FileDialog.aspx?ed=" + editorType + "&type=" + browserType + "&dir=" + currentDir);
                return; 
            }

            if (file != null && file.FileName != null && file.FileName.Trim().Length > 0)
            {
                string destPath = Path.Combine(GetCurrentDirectory(), Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles));
                string ext = Path.GetExtension(file.FileName);
                if (SiteUtils.IsAllowedUploadBrowseFile(ext, allowedExtensions))
                {
                    if (File.Exists(destPath))
                    {
                        File.Delete(destPath);
                    }
                    file.MoveTo(destPath, MoveToOptions.Overwrite);
                    file.Dispose();
                    if (SiteUtils.IsImageFileExtension(ext))
                    {
                        if (chkConstrainImageSize.Checked)
                        {
                            ImageHelper.ResizeImage(destPath, IOHelper.GetMimeType(ext), resizeWidth, resizeHeight);
                        }
                    }
                }
                else
                {
                    file.Dispose();
                }
            }

            WebUtils.SetupRedirect(this, GetRedirectUrl());
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            // this is using a LinkButton which I normally never use for accessibility reasons 
            // because linkbuttons don't work if javascript is disabled
            // but in this case this dialog can't work any if javascript is disabled 
            // so I'm using one
            if (userCanDeleteFiles)
            {
                string fileToDelete = string.Empty;
                if (hdnFileUrl.Value.Length > 0) { fileToDelete = hdnFileUrl.Value; }

                bool canDelete = IOHelper.IsDecendentFile(rootDirectory, fileToDelete);
                if (canDelete)
                {
                    File.Delete(Server.MapPath(fileToDelete));
                } 
            }

            if ((hdnFolder.Value.Length > 0) && (hdnFolder.Value != rootDirectory))
            {
                currentDir = hdnFolder.Value;
            }

            WebUtils.SetupRedirect(this, GetRedirectUrl());

        }

        private string GetRedirectUrl()
        {
            if(editorType == "ck")
            {
                return navigationRoot + "/Dialog/FileDialog.aspx?"
                    + "type=" + browserType 
                    + "&CKEditor=" + CKEditor
                    + "&CKEditorFuncNum=" + CKEditorFuncNum
                    + "&langCode=" + langCode
                    + "&ed=" + editorType
                    + "&dir=" + currentDir;

            }
            return navigationRoot + "/Dialog/FileDialog.aspx?ed=" + editorType + "&type=" + browserType + "&dir=" + currentDir;

        }

        private string GetCurrentDirectory()
        {
            string hiddenCurrentFolder = hdnFolder.Value;

            if (string.IsNullOrEmpty(hiddenCurrentFolder)) { return Server.MapPath(rootDirectory); }
            if (!Directory.Exists(Server.MapPath(hiddenCurrentFolder))) { return Server.MapPath(rootDirectory); }

            if (IOHelper.IsDecendentDirectory(rootDirectory, hiddenCurrentFolder))
            {
                return Server.MapPath(hiddenCurrentFolder);
            }

            return Server.MapPath(rootDirectory);
        }

        

        private void PopulateLabels()
        {
            this.Title = Resource.FileBrowser;
            litHeading.Text = Server.HtmlEncode(Resource.FileBrowseDialogHeading);
            btnSubmit.Text = Resource.SelectButton;
            btnNewFolder.Text = Resource.FileBrowserCreateFolderButton;
            btnUpload.Text = Resource.FileManagerUploadButton;
            regexFile.ErrorMessage = Resource.FileTypeNotAllowed;
            reqFile.ErrorMessage = Resource.NoFileSelectedWarning;
            requireFolder.ErrorMessage = Resource.FolderNameRequired;
            regexFolder.ValidationExpression = SecurityHelper.GetMaxLengthRegexValidationExpression(150);
            regexFolder.ErrorMessage = Resource.FolderName150Limit;
            litCreateFolder.Text = Server.HtmlEncode(Resource.FileBrowserCreateFolderHeading);
            litUpload.Text = Server.HtmlEncode(Resource.FileBrowserUploadHeading);
            litFolderInstructions.Text = Server.HtmlEncode(Resource.FileBrowserCreateFolderInstructions);
            litUploadInstructions.Text = Server.HtmlEncode(Resource.FileBrowserUploadInstructions);
            litFileSelectInstructions.Text = Server.HtmlEncode(Resource.FileBrowserSelectFileInstructions);

            chkConstrainImageSize.Text = Resource.FileBrowserResizeForWeb;

            lnkImageCropper.Text = Resource.CropImageLink;
            btnDelete.Text = "Delete";
            UIHelper.AddConfirmationDialog(btnDelete, "Are you sure you want to delete this file?");
            btnDelete.Visible = userCanDeleteFiles;

            if (Head1.FindControl("treecss") == null)
            {
                Literal cssLink = new Literal();
                cssLink.ID = "forumcss";
                cssLink.Text = "\n<link href='"
                + Page.ResolveUrl("~/ClientScript/jqueryFileTree/jqueryFileTree.css")
                + "' type='text/css' rel='stylesheet' media='screen' />";

                Head1.Controls.Add(cssLink);
            }

        }

        private void LoadSettings()
        {
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            userCanDeleteFiles = WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor);

            if (WebUser.IsAdminOrContentAdmin)
            {
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
                
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                regexFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(allowedExtensions);
                canEdit = true;
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                rootDirectory = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/");
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                regexFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(allowedExtensions);
                canEdit = true;

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null) { return; }

                rootDirectory = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/userfiles/" + currentUser.UserId.ToInvariantString() + "/");
                allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
                regexFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(allowedExtensions);
                canEdit = true;
                if(!userCanDeleteFiles)
                {
                    // user is not in a role that can delete files but config setting alows delete from user specific folder anyway
                    userCanDeleteFiles = WebConfigSettings.AllowDeletingFilesFromUserFolderWithoutDeleteRole;
                }
            }

           

            

            resizeWidth = WebConfigSettings.ResizeImageDefaultMaxWidth;
            resizeHeight = WebConfigSettings.ResizeImageDefaultMaxHeight;


            pnlUpload.Visible = canEdit;

            if (Request.QueryString["ed"] != null)
            {
                editorType = Request.QueryString["ed"];
            }

            string requestedType = "image";
            if (Request.QueryString["type"] != null)
            {
                requestedType = Request.QueryString["type"];
            }

            if (Request.QueryString["dir"] != null)
            {
                currentDir = Request.QueryString["dir"];
                //if (!IOHelper.IsDecendentDirectory(rootDirectory, currentDir)) { currentDir = string.Empty; }
            }


            if (Request.QueryString["CKEditor"] != null)
            {
                CKEditor = Request.QueryString["CKEditor"];
            }

            if (Request.QueryString["CKEditorFuncNum"] != null)
            {
                CKEditorFuncNum = Request.QueryString["CKEditorFuncNum"];
            }

            if (Request.QueryString["langCode"] != null)
            {
                langCode = Request.QueryString["langCode"];
            }

            

            switch (requestedType)
            {
                case "media":
                    browserType = "media";
                    break;

                case "file":
                    browserType = "file";
                    break;

                case "image":
                default:
                    browserType = "image";
                    break;

            }

            navigationRoot = SiteUtils.GetNavigationSiteRoot();

            lnkRoot.Text = rootDirectory;
            lnkRoot.ToolTip = rootDirectory;
            lnkRoot.NavigateUrl = navigationRoot + "/Dialog/FileDialog.aspx?type=" + browserType;


            if (!Page.IsPostBack)
            {
                hdnFolder.Value = rootDirectory;
                if (currentDir.Length > 0)
                {
                    hdnFolder.Value = currentDir;
                }

                txtMaxWidth.Text = resizeWidth.ToInvariantString();
                txtMaxHeight.Text = resizeHeight.ToInvariantString();
            }
            else
            {
                int.TryParse(txtMaxWidth.Text, out resizeWidth);
                int.TryParse(txtMaxHeight.Text, out resizeHeight);
            }

            imageCropperUrl = navigationRoot + "/Dialog/ImageCropperDialog.aspx";
            lnkImageCropper.NavigateUrl = imageCropperUrl;
            
        }

        private void SetupScripts()
        {
            SetupMainScript();
            SetupjQueryFileTreeScript();

        }

        private void SetupMainScript()
        {
            switch (editorType)
            {
                case "tmc":
                    SetupTinyMce();
                    break;

                case "ck":
                    SetupCKeditor();
                    break;

                case "fck":
                    SetupFCKeditor();
                    break;

                default:
                    //do nothing
                    break;
            }
        }

        private void SetupCKeditor()
        {
         
            btnSubmit.Attributes.Add("onclick", "ckSubmit(); return false; ");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function ckSubmit () {");

            script.Append("var URL = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            //script.Append("alert(URL);");
            script.Append("var CKEditorFuncNum = window.location.href.replace(/.*CKEditorFuncNum=(\\d+).*/,\"$1\")||alert('Error: lost CKEditorFuncNum param from url'+window.location.href)||1;");

            //script.Append("alert(CKEditorFuncNum);");
            //script.Append("var CKEditorFuncNum = " + CKEditorFuncNum + ";");
            // not sure why need to call this 2x but otherwise after an upload it fails to preview the selected image
            script.Append("window.opener.CKEDITOR.tools.callFunction(CKEditorFuncNum, URL);");
            script.Append("window.opener.CKEDITOR.tools.callFunction(CKEditorFuncNum, URL);");
            
            script.Append("window.close();");

            script.Append("}");

           
            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "cksubmit",
                script.ToString());

            SetupScrollFix();

        }

        private void SetupFCKeditor()
        {
            
            btnSubmit.Attributes.Add("onclick", "fckSubmit(); return false; ");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function fckSubmit () {");

            script.Append("var URL = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            //script.Append("alert(URL);");

            script.Append("window.opener.SetUrl(URL);");
            script.Append("window.close();");
            script.Append("window.opener.focus();");

            script.Append("}");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "fcksubmit",
                script.ToString());

            SetupScrollFix();

        }

        /// <summary>
        /// fixes an issue in CKeditor and FCKeditor where it was not possible to scroll the page and folderlist could be clipped at the bottom
        /// not needed for TinyMCE so added with script instead of style declaration
        /// </summary>
        private void SetupScrollFix()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

           
            script.Append("$(document).ready(function () {");
            script.Append("$('#filewrapper').attr({ 'style': 'height:595px; overflow:auto;  padding: 10px;' });");
            script.Append(" });");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "scrollfix",
                script.ToString());
        }

        private void SetupTinyMce()
        {
            btnSubmit.Attributes.Add("onclick", "FileBrowserDialogue.mySubmit(); return false; ");

            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                "tinymcemain",
                "<script type=\"text/javascript\" src=\""
                + ResolveUrl(WebConfigSettings.TinyMceBasePath + "tiny_mce_popup.js") + "\"></script>");


            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            //script.Append("var win = tinyMCEPopup.getWindowArg('window');");
            //script.Append("var input = tinyMCEPopup.getWindowArg('input');");
            //script.Append("var res = tinyMCEPopup.getWindowArg('resizable');");
            //script.Append("var inline = tinyMCEPopup.getWindowArg('inline');");

            script.Append("var FileBrowserDialogue = { ");
            script.Append("init : function () {");
            // Here goes your code for setting your custom things onLoad.

            //Remove TinyMCE's default popup CSS
            script.Append("var allLinks = document.getElementsByTagName('link');");
            script.Append("allLinks[allLinks.length-1].parentNode.removeChild(allLinks[allLinks.length-1]);");

            script.Append("},");

            script.Append("mySubmit : function () { ");

            // Here goes your code to insert the retrieved URL value into the original dialogue window.
            //script.Append("alert('hey');");

            script.Append("var URL = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            script.Append("var win = tinyMCEPopup.getWindowArg('window');");

            // insert information now
            script.Append("win.document.getElementById(tinyMCEPopup.getWindowArg('input')).value = URL;");

            // are we an image browser
            script.Append("if (typeof(win.ImageDialog) != \"undefined\") {");
            // we are, so update image dimensions
            script.Append("if (win.ImageDialog.getImageData){ ");
            script.Append("win.ImageDialog.getImageData(); }");

            // and preview if necessary
            script.Append("if (win.ImageDialog.showPreviewImage) {");
            script.Append("win.ImageDialog.showPreviewImage(URL); }");

            script.Append("}");

            // close popup window
            script.Append("tinyMCEPopup.close();");

            script.Append("}");
           

            script.Append("};");

            script.Append("tinyMCEPopup.onInit.add(FileBrowserDialogue.init, FileBrowserDialogue);");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "tmcsubmit",
                script.ToString());

        }

        private void SetupjQueryFileTreeScript()
        {
            //http://abeautifulsite.net/notebook/58
            //http://plugins.jquery.com/project/filetree

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$(document).ready(function() {");

            script.Append("$('#" + lnkImageCropper.ClientID + "').hide(); ");

            if (userCanDeleteFiles)
            {
                script.Append("$('#" + btnDelete.ClientID + "').hide(); ");
            }

            script.Append("$('#" + pnlFileTree.ClientID + "').fileTree({");
            script.Append("root: '" + rootDirectory + "'");

            if (currentDir.Length > 0)
            {
                script.Append(",currentDir : '" + currentDir + "'");
            }

            script.Append(",loadMessage:'" + Resource.AjaxLoadingMessage.HtmlEscapeQuotes() + "'");
            script.Append(",multiFolder: false");
            script.Append(", script: '" + navigationRoot + "/Services/jqueryFileTreeMediaBrowser.ashx?type=" + browserType + "&amp;dir=" + currentDir + "'");
            script.Append("}, function(file) {");

            //script.Append("alert(file);");
            script.Append("document.getElementById('" + hdnFileUrl.ClientID + "').value = file; ");
            script.Append("document.getElementById('" + txtSelection.ClientID + "').value = file; ");
            if (userCanDeleteFiles)
            {
                script.Append("$('#" + btnDelete.ClientID + "').show(); ");
            }

            if (browserType == "image")
            {
                script.Append("document.getElementById('" + imgPreview.ClientID + "').src = file; ");
                script.Append("var imageCropperUrl = '" + imageCropperUrl + "'; ");
                script.Append("var selDir = document.getElementById('" + hdnFolder.ClientID + "').value; ");
                script.Append("var returnUrl = encodeURIComponent('" + navigationRoot + "/Dialog/FileDialog.aspx?ed=" + editorType + "&type=" + browserType + "&dir=' + selDir) ; ");
                //script.Append("alert(returnUrl);");
                script.Append("$('#" + lnkImageCropper.ClientID + "').attr('href',imageCropperUrl + '?src=' + file + '&return=' + returnUrl); ");
                script.Append("$('#" + lnkImageCropper.ClientID + "').show(); ");
            }
            else
            {
                script.Append("$('#" + lnkImageCropper.ClientID + "').hide(); ");
            }

            script.Append("}, function(folder) {");
            
            //script.Append("alert(folder);");
            
            script.Append("document.getElementById('" + hdnFolder.ClientID + "').value = folder; ");
            script.Append("if(folder == 'root'){");
            script.Append("document.getElementById('" + hdnFolder.ClientID + "').value = '" + rootDirectory + "'; ");
            script.Append("}");

            //script.Append("document.getElementById('" + imgPreview.ClientID + "').src = file; ");
            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                script.Append("if(folder == '/Pages/'){");
                script.Append("$('#" + pnlUpload.ClientID + "').hide();");
                script.Append("}else{");
                script.Append("$('#" + pnlUpload.ClientID + "').show();");
                script.Append("}");
            }

            script.Append("}");

            script.Append(");");
            script.Append("});");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "jqftinstance",
                script.ToString());

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnNewFolder.Click += new EventHandler(btnNewFolder_Click);
            btnUpload.Click += new EventHandler(btnUpload_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        

        

        


    }
}
