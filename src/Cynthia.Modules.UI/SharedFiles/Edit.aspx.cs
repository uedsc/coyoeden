/// Author:					    Joe Audette
/// Created:				    2005-01-09
/// Last Modified:			    2009-12-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.IO;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Brettle.Web.NeatUpload;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.SharedFilesUI
{
	
    public partial class SharedFilesEdit : CBasePage
	{
        
        private int pageId = 0;
        private int moduleId = 0;
        private string strItem = string.Empty;
        private int itemId = 0;
        private string type = string.Empty;
        private string upLoadPath;
        private string historyPath;
        private String cacheDependencyKey;
        private Double timeOffset = 0; 

        protected Double TimeOffset
        {
            get{return timeOffset;}  
        }
		

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            this.btnUpload.Click += new EventHandler(btnUpload_Click);
            this.btnUpdateFile.Click += new EventHandler(btnUpdateFile_Click);
            this.btnDeleteFile.Click += new EventHandler(btnDeleteFile_Click);
            this.btnUpdateFolder.Click += new EventHandler(btnUpdateFolder_Click);
            this.btnDeleteFolder.Click += new EventHandler(btnDeleteFolder_Click);
            
            grdHistory.RowCommand += new GridViewCommandEventHandler(grdHistory_RowCommand);

            SiteUtils.SetupEditor(edDescription);
            
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

			if(!IsPostBack)
			{
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancelFile.NavigateUrl = hdnReturnUrl.Value;
                    lnkCancelFolder.NavigateUrl = hdnReturnUrl.Value;

                }
                
				PopulateControls();
			}


		}

		private void PopulateControls()
		{
			if((this.moduleId > 0)&&(this.itemId > 0)&&((this.type == "folder")||(this.type == "file")))
			{
				if(this.type == "folder")
				{
					PopulateFolderControls();
				}

				if(this.type == "file")
				{
					PopulateFileControls();
				}
			}
			else
			{
				this.pnlNotFound.Visible = true;
				this.pnlFolder.Visible = false;
				this.pnlFile.Visible = false;

			}
		}

		private void PopulateFolderControls()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, SharedFileResources.SharedFilesEditFolderLabel);

			SharedFileFolder folder = new SharedFileFolder(this.moduleId, this.itemId);
			if((folder.FolderId > 0)&&(folder.ModuleId == this.moduleId))
			{
				this.pnlNotFound.Visible = false;
				this.pnlFile.Visible = false;
				this.pnlFolder.Visible = true;

				this.txtFolderName.Text = folder.FolderName;

                using (IDataReader reader = SharedFileFolder.GetSharedModuleFolders(folder.ModuleId))
                {
                    this.ddFolderList.DataSource = reader;
                    this.ddFolderList.DataBind();
                }
				this.ddFolderList.Items.Insert(0,new ListItem("Root","-1"));
				this.ddFolderList.SelectedValue = folder.ParentId.ToString();

                // prevent a folder from being its own parent
                ListItem item = this.ddFolderList.Items.FindByText(folder.FolderName);
                if(item != null)
                this.ddFolderList.Items.Remove(item);
			}
			else
			{
				this.pnlNotFound.Visible = true;
				this.pnlFolder.Visible = false;
				this.pnlFile.Visible = false;
			}
		}

		private void PopulateFileControls()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, SharedFileResources.SharedFilesEditLabel);

			SharedFile file = new SharedFile(this.moduleId, this.itemId);
			if((file.ItemId > 0)&&(file.ModuleId == this.moduleId))
			{
				this.pnlNotFound.Visible = false;
				this.pnlFolder.Visible = false;
				this.pnlFile.Visible = true;

                using (IDataReader reader = SharedFileFolder.GetSharedModuleFolders(file.ModuleId))
                {
                    this.ddFolders.DataSource = reader;
                    this.ddFolders.DataBind();
                }
				this.ddFolders.Items.Insert(0,new ListItem("Root","-1"));
				this.ddFolders.SelectedValue = file.FolderId.ToString();

				this.lblUploadDate.Text = file.UploadDate.AddHours(timeOffset).ToString();
				SiteUser uploadUser = new SiteUser(siteSettings, file.UploadUserId);
				this.lblUploadBy.Text = uploadUser.Name;
				this.lblFileSize.Text = file.SizeInKB.ToString();
				this.txtFriendlyName.Text = file.FriendlyName;
                edDescription.Text = file.Description;

                using (IDataReader reader = file.GetHistory())
                {
                    grdHistory.DataSource = reader;
                    grdHistory.DataBind();
                }

			}
			else
			{
				pnlNotFound.Visible = true;
				pnlFolder.Visible = false;
				pnlFile.Visible = false;
			}


		}

		private void PopulateLabels()
		{
            progressBar.AddTrigger(this.btnUpload);

            btnUpload.Text = SharedFileResources.FileManagerUploadButton;
            btnUpdateFile.Text = SharedFileResources.SharedFilesUpdateButton;
            btnDeleteFile.Text = SharedFileResources.SharedFilesDeleteButton;

            lnkCancelFile.Text = SharedFileResources.SharedFilesCancelButton;
            lnkCancelFolder.Text = SharedFileResources.SharedFilesCancelButton;

            UIHelper.AddConfirmationDialog(btnDeleteFile, SharedFileResources.FileManagerDeleteConfirm);
            btnUpdateFolder.Text = SharedFileResources.SharedFilesUpdateButton;
            btnDeleteFolder.Text = SharedFileResources.SharedFilesDeleteButton;
            UIHelper.AddConfirmationDialog(btnDeleteFolder, SharedFileResources.FileManagerFolderDeleteConfirm);

            grdHistory.Columns[0].HeaderText = SharedFileResources.FileManagerFileNameLabel;
            grdHistory.Columns[1].HeaderText = SharedFileResources.FileManagerSizeLabel;
            grdHistory.Columns[2].HeaderText = SharedFileResources.SharedFilesEditUploadDateLabel;
            grdHistory.Columns[3].HeaderText = SharedFileResources.SharedFilesArchiveDateLabel;

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
		}


		private void btnUpload_Click(object sender, EventArgs e)
		{
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null) return;

			SharedFile sharedFile = new SharedFile(this.moduleId, this.itemId);
            sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);
            if((sharedFile.ItemId > 0)&&(sharedFile.ModuleId == this.moduleId))
			{
                
					if (file1.HasFile && file1.FileName != null && file1.FileName.Trim().Length > 0)
					{
						//SharedFile sharedFile = new SharedFile();
						sharedFile.CreateHistory(upLoadPath,historyPath);
						sharedFile.ModuleId = this.moduleId;
                        if (sharedFile.ModuleGuid == Guid.Empty)
                        {
                            Module m = new Module(moduleId);
                            sharedFile.ModuleGuid = m.ModuleGuid;

                        }
						sharedFile.OriginalFileName = file1.FileName;
						// FIXME: the following line probably doesn't work right when uploading from
						// Windows to Unix because GetFileName will be looking for "\" and the FileName
						// will contain "/".
						sharedFile.FriendlyName = Path.GetFileName(file1.FileName);
                        sharedFile.SizeInKB = (int)(file1.ContentLength/1024);
                        
                        //sharedFile.FolderID = this.CurrentFolderID;
						sharedFile.UploadUserId = siteUser.UserId;
						
						if(sharedFile.Save())
						{
							string destPath = upLoadPath + sharedFile.ServerFileName;
							if (File.Exists(destPath))
							{
								File.Delete(destPath);
							}
							file1.MoveTo(destPath,MoveToOptions.Overwrite);
						}
						
					}

                    CurrentPage.UpdateLastModifiedTime();
                    CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    SiteUtils.QueueIndexing();
					WebUtils.SetupRedirect(this, Request.RawUrl);
                    
					
                
			}
		}

        void sharedFile_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["SharedFilesIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

		private void btnUpdateFile_Click(object sender, EventArgs e)
		{
			SharedFile file = new SharedFile(this.moduleId, this.itemId);
            file.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

            if((file.ItemId > 0)&&(file.ModuleId == this.moduleId))
			{
				file.FriendlyName = this.txtFriendlyName.Text;
                file.Description = edDescription.Text;
				file.FolderId = int.Parse(this.ddFolders.SelectedValue);
                if (file.FolderId > -1)
                {
                    SharedFileFolder folder = new SharedFileFolder(this.moduleId,file.FolderId);
                    file.FolderGuid = folder.FolderGuid;
                }
                if (file.ModuleGuid == Guid.Empty)
                {
                    Module m = new Module(moduleId);
                    file.ModuleGuid = m.ModuleGuid;

                }
                

				file.Save();

			}
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

		private void btnDeleteFile_Click(object sender, EventArgs e)
		{
            SharedFile sharedFile = new SharedFile(moduleId, itemId);
            sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);
            
            sharedFile.CreateHistory(this.upLoadPath, this.historyPath);
            //SharedFile.DeleteSharedFile(this.moduleId,this.itemId, this.upLoadPath);
            sharedFile.Delete(this.upLoadPath);
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

        

		private void btnUpdateFolder_Click(object sender, EventArgs e)
		{
			SharedFileFolder folder = new SharedFileFolder(this.moduleId, this.itemId);
			if((folder.FolderId > 0)&&(folder.ModuleId == this.moduleId))
			{
				folder.FolderName = this.txtFolderName.Text;
				folder.ParentId = int.Parse(this.ddFolderList.SelectedValue);
				folder.Save();

			}

            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
			
		}

		private void btnDeleteFolder_Click(object sender, EventArgs e)
		{
			//SharedFile sharedFile = new SharedFile(moduleId, itemId);
			//sharedFile.CreateHistory(this.upLoadPath, this.historyPath);
			//SharedFile.DeleteSharedFile(this.itemId);
			SharedFileFolder.DeleteSharedFileFolder(this.itemId);

            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        void grdHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandArgument is Int32)) { return; }

            int archiveID = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "restore":
                    SharedFile.RestoreHistoryFile(archiveID, this.upLoadPath, this.historyPath);
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;

                case "download":

                    SharedFileHistory historyFile = SharedFile.GetHistoryFile(archiveID);
                    if (historyFile != null)
                    {
                        string fileType = Path.GetExtension(historyFile.FriendlyName).Replace(".", string.Empty);
                        if (string.Equals(fileType, "pdf", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //this will display the pdf right in the browser
                            Page.Response.AddHeader("Content-Disposition", "filename=" + historyFile.FriendlyName);
                        }
                        else
                        {
                            // other files just use file save dialog
                            Page.Response.AddHeader("Content-Disposition", "attachment; filename=" + historyFile.FriendlyName);
                        }

                        //Page.Response.AddHeader("Content-Length", documentFile.DocumentImage.LongLength.ToString());

                        Page.Response.ContentType = "application/" + fileType;
                        Page.Response.Buffer = false;
                        Page.Response.BufferOutput = false;
                        Response.TransmitFile(historyPath + historyFile.ServerFileName);
                        Response.End();
                    }

                    break;

            }

        }

       
        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["ItemID"]))
            {
                strItem = HttpContext.Current.Request.Params["ItemID"];
            }

            timeOffset = SiteUtils.GetUserTimeOffset();
        }

        private void LoadSettings()
        {
            cacheDependencyKey = "Module-" + moduleId.ToString();

            upLoadPath = Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/SharedFiles/");

            historyPath = Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/Sites/"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/SharedFiles/History/");

            lnkCancelFile.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            lnkCancelFolder.NavigateUrl = lnkCancelFile.NavigateUrl;

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

            //this page handles both folders and files
            //expected strItem examples are 23~folder and 13~file
            //the number portion is the ItemID of the folder or file
            if (strItem.IndexOf("~") > -1)
            {
                try
                {
                    char[] separator = { '~' };
                    string[] args = strItem.Split(separator);
                    this.itemId = int.Parse(args[0]);
                    type = args[1];
                }
                catch { };

            }
        }


		
	}
}
