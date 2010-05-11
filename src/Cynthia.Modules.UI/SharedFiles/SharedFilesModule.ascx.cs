///	Author:				    Joe Audette
///	Created:				2005-01-05
/// Last Modified:			2010-01-10

using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brettle.Web.NeatUpload;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.SharedFilesUI
{
	public partial class SharedFilesModule : SiteModuleControl 
	{
        
        private string imgroot;
        private String filePath;
        private FileInfo[] iconList;
        protected Double TimeOffset = 0;
        private bool showDownloadCountToAllUsers = false;
        // showDescription is false by default because a common scenario is to paste the full text
        // content from a pdf or word doc so that it is indexed in the search index and the file can be found in search
        // displaying this large text would not be pretty
        // probably should add a new field for abstract or brief description at some point.
        private bool showDescription = false;
        private bool showSize = true;
        private bool showModified = true;
        private bool showUploadedBy = true;
        private bool showObjectCount = true;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        private string defaultSort = "ASC";
        

        protected int CurrentFolderId
        {
            get
            {
                int i = -1;
                int.TryParse(hdnCurrentFolderId.Value, out i);
                return i;
           
            }
            set
            {
                hdnCurrentFolderId.Value = value.ToInvariantString();

            }
        }

       
        protected void Page_Load(object sender, EventArgs e)
		{
			
            LoadSettings();
			PopulateLabels();
            iconList = SiteUtils.GetFileIconList();

            
            if (ScriptController != null)
            {
                // register the button to do a full postback
                ScriptController.RegisterPostBackControl(btnUpload);
                //ScriptController.RegisterAsyncPostBackControl(btnNewFolder);
                //ScriptController.RegisterAsyncPostBackControl(btnDelete);
                //ScriptController.RegisterAsyncPostBackControl(dgFile);

            }

            

            if ((!Page.IsPostBack)&&(!Page.IsAsync))
			{
                //progressBar.AddTrigger(this.btnUpload);
				BindData();
			}

		}

        


		
		
		private void BindData() 
		{
			if(CurrentFolderId > -1)
			{
				SharedFileFolder folder = new SharedFileFolder(this.ModuleId, CurrentFolderId);
				this.lblCurrentDirectory.Text = folder.FolderName;
                this.btnGoUp.Visible = true;
			}
			else
			{
				this.lblCurrentDirectory.Text = string.Empty;
                this.btnGoUp.Visible = false;
			}

            DataView dv = new DataView(SharedFileFolder.GetFoldersAndFiles(ModuleId, CurrentFolderId));

            dv.Sort = "filename" + " " + defaultSort;
            dgFile.DataSource = dv;

			//dgFile.DataSource = SharedFileFolder.GetFoldersAndFiles(ModuleId, CurrentFolderId);

			//dgFile.DataKeyField = "ID";
			dgFile.DataBind();
			lblCounter.Text = dgFile.Rows.Count.ToString() + " "
                + SharedFileResources.FileManagerObjectsLabel;

        }

        
        #region Grid Events 


        protected void dgFile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgType;
                if (e.Row.RowIndex == dgFile.EditIndex)
                {
                    imgType = (Image)e.Row.Cells[1].FindControl("imgEditType");
                }
                else
                {
                    imgType = (Image)e.Row.Cells[1].FindControl("imgType");

                }

                if (imgType != null)
                {
                    int type = int.Parse(DataBinder.Eval(e.Row.DataItem, "type", "{0}"));
                    if (type == 0)
                    {
                        //type is folder
                        imgType.ImageUrl = imgroot + "folder.png";
                    }
                    else
                    {
                        // type is file
                        string name = DataBinder.Eval(e.Row.DataItem, "OriginalFileName", "{0}").Trim();
                        string imgFile = Path.GetExtension(name).ToLower().Replace(".", "") + ".png";

                        if (IconExists(imgFile))
                        {
                            imgType.ImageUrl = imgroot + "Icons/" + imgFile;
                        }
                        else
                        {
                            imgType.ImageUrl = imgroot + "Icons/unknown.png";
                        }

                        Control c = e.Row.FindControl("downloadLink");
                        if (c != null)
                        {
                            HyperLink downloadLink = c as HyperLink;
                            downloadLink.Attributes.Add("onclick", "window.open(this.href,'_blank');return false;");
                        }

                        c = e.Row.FindControl("fileLink");
                        if (c != null)
                        {
                            HyperLink fileLink = c as HyperLink;
                            fileLink.Attributes.Add("onclick", "window.open(this.href,'_blank');return false;");
                        }

                    }
                }
            }

        }

        

        protected void dgFile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ItemClicked")
            {
                
                string keys = e.CommandArgument.ToString();
                char[] separator = { '~' };
                string[] args = keys.Split(separator);
                string type = args[1];
                dgFile.EditIndex = -1;

                if (type == "folder")
                {
                    CurrentFolderId = int.Parse(args[0]);
                    BindData();
                    upFiles.Update();
                    return;

                }

                // this isn't used since we changed to a link to download.aspx
                if (type == "file")
                {
                    int fileID = int.Parse(args[0]);
                    SharedFile sharedFile = new SharedFile(this.ModuleId, fileID);

                    sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

                    string downloadPath = Page.Server.MapPath(WebUtils.GetApplicationRoot()
                        + "/Data/Sites/" + this.SiteId.ToString(CultureInfo.InvariantCulture)
                        + "/SharedFiles/") + sharedFile.ServerFileName;

                    string fileType = Path.GetExtension(sharedFile.OriginalFileName).Replace(".", string.Empty);
                    string mimeType = SiteUtils.GetMimeType(fileType);
                    Page.Response.ContentType = mimeType;

                    if (SiteUtils.IsNonAttacmentFileType(fileType))
                    {
                        //this will display the pdf right in the browser
                        Page.Response.AddHeader("Content-Disposition", "filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName.Replace(" ", string.Empty), Encoding.UTF8) + "\"");
                    }
                    else
                    {
                        // other files just use file save dialog
                        Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName.Replace(" ", string.Empty), Encoding.UTF8) + "\"");
                    }

                    //Page.Response.AddHeader("Content-Length", documentFile.DocumentImage.LongLength.ToString());


                    Page.Response.Buffer = false;
                    Page.Response.BufferOutput = false;
                    Page.Response.TransmitFile(downloadPath);
                    Page.Response.End();
                }


            }


        }

        

        protected void dgFile_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                TextBox txtEditName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtEditName");
                if (txtEditName.Text.Trim().Length < 1)
                    return;

                
                string keys = grid.DataKeys[e.RowIndex].Value.ToString();
                char[] separator = { '~' };
                string[] args = keys.Split(separator);
                string type = args[1];

                if (type == "folder")
                {
                    int folderID = int.Parse(args[0]);
                    SharedFileFolder folder = new SharedFileFolder(this.ModuleId, folderID);
                    //Module m = new Module(ModuleId);
                    //folder.ModuleGuid = m.ModuleGuid;
                    folder.FolderName = Path.GetFileName(txtEditName.Text);
                    folder.Save();

                }

                if (type == "file")
                {
                    int fileID = int.Parse(args[0]);
                    SharedFile sharedFile = new SharedFile(this.ModuleId, fileID);
                    sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);
                    sharedFile.FriendlyName = Path.GetFileName(txtEditName.Text);
                    sharedFile.Save();

                }

                dgFile.EditIndex = -1;
                BindData();

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            upFiles.Update();


        }

        protected void dgFile_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgFile.EditIndex = e.NewEditIndex;
            BindData();
            upFiles.Update();
        }

        
        protected void dgFile_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgFile.EditIndex = -1;
            BindData();
            upFiles.Update();
        }

        protected void dgFile_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["SortBy"] == null)
            {
                ViewState["SortBy"] = "ASC";
            }
            else if (ViewState["SortBy"].ToString().Equals("ASC"))
            {
                ViewState["SortBy"] = "DESC";
            }
            else
            {
                ViewState["SortBy"] = "ASC";
            }

            DataView dv = new DataView(SharedFileFolder.GetFoldersAndFiles(ModuleId, CurrentFolderId));

            dv.Sort = e.SortExpression + " " + ViewState["SortBy"];
            dgFile.DataSource = dv;
            dgFile.DataBind();
            upFiles.Update();

        }



        #endregion

        protected void btnGoUp_Click(object sender, ImageClickEventArgs e)
		{
			MoveUp();
            upFiles.Update();
			
		}
		private void MoveUp()
		{
			if(CurrentFolderId > 0)
			{
				SharedFileFolder folder = new SharedFileFolder(ModuleId, CurrentFolderId);
                CurrentFolderId = folder.ParentId;
                BindData();
			}
			else
			{
                lblError.Text = SharedFileResources.RootDirectoryReached;
			} 
		}

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {

            bool yes = false;
            foreach (GridViewRow dgi in dgFile.Rows)
            {
                CheckBox chkChecked = (CheckBox)dgi.Cells[0].FindControl("chkChecked");
                if ((chkChecked != null)&&(chkChecked.Checked))
                {
                    yes = true;
                    DeleteItem(dgi);
                }
            }

            if (yes)
            {
                dgFile.PageIndex = 0;
                dgFile.EditIndex = -1;
                BindData();
            }


            upFiles.Update();
        }

        private void DeleteItem(GridViewRow e)
		{
			string keys = dgFile.DataKeys[e.RowIndex].Value.ToString();
			char[] separator = {'~'};
			string[] args = keys.Split(separator);
			string type = args[1];

			if(type == "folder")
			{
				int folderID = int.Parse(args[0]);
				SharedFileFolder folder = new SharedFileFolder(this.ModuleId, folderID);
                folder.DeleteAllFiles(this.filePath);
				SharedFileFolder.DeleteSharedFileFolder(folderID);

			}

			if(type == "file")
			{
				int fileID = int.Parse(args[0]);
                SharedFile sharedFile = new SharedFile(this.ModuleId, fileID);
                sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);
                sharedFile.Delete(this.filePath);

			}
			
		}

		

		protected void btnNewFolder_Click(object sender, EventArgs e)
		{
			try
			{
                if (txtNewDirectory.Text.Length > 0)
                {
                    SharedFileFolder folder = new SharedFileFolder();
                    folder.ParentId = CurrentFolderId;
                    folder.ModuleId = ModuleId;
                    Module m = new Module(ModuleId);
                    folder.ModuleGuid = m.ModuleGuid;
                    folder.FolderName = Path.GetFileName(txtNewDirectory.Text);
                    if (folder.Save())
                    {
                        BindData();
                    }
                }
			}
			catch(Exception ex)
			{
				lblError.Text = ex.Message;
			}

            upFiles.Update();
		}

        

		

		protected void btnUpload_Click(object sender, EventArgs e)
		{ 
            string upLoadPath = Page.Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/Sites/"
                + SiteId.ToString(CultureInfo.InvariantCulture) + "/SharedFiles/");
            
            if (!Directory.Exists(upLoadPath))
            {
                Directory.CreateDirectory(upLoadPath);
            }

            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

            if (multiFile.Files.Length > 0)
            {

                foreach (UploadedFile file in multiFile.Files)
                {
                    if (file != null && file.FileName != null && file.FileName.Trim().Length > 0)
                    {
                        SharedFile sharedFile = new SharedFile();
                        Module m = new Module(ModuleId);

                        sharedFile.ModuleId = ModuleId;
                        sharedFile.ModuleGuid = m.ModuleGuid;
                        sharedFile.OriginalFileName = file.FileName;
                        sharedFile.FriendlyName = Path.GetFileName(file.FileName);
                        sharedFile.SizeInKB = (int)(file.ContentLength / 1024);
                        sharedFile.FolderId = CurrentFolderId;
                        if (CurrentFolderId > -1)
                        {
                            SharedFileFolder folder = new SharedFileFolder(ModuleId, CurrentFolderId);
                            sharedFile.FolderGuid = folder.FolderGuid;
                        }
                        sharedFile.UploadUserId = siteUser.UserId;
                        sharedFile.UserGuid = siteUser.UserGuid;
                        sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

                        if (sharedFile.Save())
                        {
                            string destPath = Path.Combine(upLoadPath, sharedFile.ServerFileName);
                            if (File.Exists(destPath))
                            {
                                File.Delete(destPath);
                            }
                            file.MoveTo(destPath, MoveToOptions.Overwrite);
                        }
                    }
                }
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
           
		}


        void sharedFile_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["SharedFilesIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private bool IconExists(String iconFileName)
        {
            bool result = false;
            if (this.iconList != null)
            {
                foreach (FileInfo f in this.iconList)
                {
                    if (f.Name == iconFileName)
                    {
                        result = true;
                    }
                }
            }

            return result;

        }

        private void PopulateLabels()
        {
            UIHelper.AddConfirmationDialog(btnDelete, SharedFileResources.FileManagerDeleteConfirm);

            dgFile.Columns[1].HeaderText = SharedFileResources.FileManagerFileNameLabel;
            dgFile.Columns[2].HeaderText = SharedFileResources.FileDescription;
            dgFile.Columns[3].HeaderText = SharedFileResources.FileManagerSizeLabel;
            dgFile.Columns[4].HeaderText = SharedFileResources.DownloadCountLabel;
            dgFile.Columns[5].HeaderText = SharedFileResources.FileManagerModifiedLabel;
            dgFile.Columns[6].HeaderText = SharedFileResources.SharedFilesUploadedByLabel;


            dgFile.Columns[2].Visible = showDescription;
            dgFile.Columns[3].Visible = showSize;
            dgFile.Columns[4].Visible = IsEditable || showDownloadCountToAllUsers;
            dgFile.Columns[5].Visible = showModified;
            dgFile.Columns[6].Visible = showUploadedBy;
            dgFile.Columns[7].Visible = IsEditable;

            pnlUpload.Visible = IsEditable;
            tblNewFolder.Visible = this.IsEditable;
            imgroot = ImageSiteRoot + "/Data/SiteImages/";
            btnDelete.ImageUrl = imgroot + "trash.png";
            btnGoUp.ImageUrl = imgroot + "arrow_up.png";

            btnUpload.Text = SharedFileResources.FileManagerUploadButton;
            btnGoUp.ToolTip = SharedFileResources.FileManagerGoUp;
            btnGoUp.AlternateText = SharedFileResources.FileManagerGoUp;
            btnNewFolder.Text = SharedFileResources.FileManagerNewFolderButton;
            btnDelete.ToolTip = SharedFileResources.FileManagerDelete;
            btnDelete.AlternateText = SharedFileResources.FileManagerDelete;
            btnDelete.Visible = this.IsEditable;
            btnAddFile.Text = SharedFileResources.AddFileButton;

            if (ModuleConfiguration != null)
            {
                Title = this.ModuleConfiguration.ModuleTitle;
                Description = this.ModuleConfiguration.FeatureName;
            }

        }


        private void LoadSettings()
        {
            lblError.Text = String.Empty;

            TimeOffset = SiteUtils.GetUserTimeOffset();
            filePath = Page.Server.MapPath("~/Data/Sites/" + SiteSettings.SiteId.ToInvariantString() + "/SharedFiles/");

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

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }


            showDownloadCountToAllUsers = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowDownloadCountToAllUsers", showDownloadCountToAllUsers);

            showDescription = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowDescription", showDescription);

            showSize = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowSize", showSize);

            showModified = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowModified", showModified);

            showUploadedBy = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowUploadedBy", showUploadedBy);

            showObjectCount = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowObjectCount", showObjectCount);


            trObjectCount.Visible = showObjectCount;

            if (Settings.Contains("DefaultSortSetting"))
            {
                defaultSort = Settings["DefaultSortSetting"].ToString();
            }
           
            
        }

        protected override void OnInit(EventArgs e)
        {
            Page.EnableViewState = true;
            base.OnInit(e);
            //this.Load += new EventHandler(Page_Load);
            //this.btnUpload.Click += new EventHandler(btnUpload_Click);
            //this.btnGoUp.Click += new ImageClickEventHandler(this.btnGoUp_Click);
            //this.btnDelete.Click += new ImageClickEventHandler(this.btnDelete_Click);
            //this.dgFile.ItemCommand += new DataGridCommandEventHandler(this.dgFile_ItemCommand);
            //this.dgFile.CancelCommand += new DataGridCommandEventHandler(this.dgFile_CancelCommand);
            //this.dgFile.EditCommand += new DataGridCommandEventHandler(this.dgFile_EditCommand);
            //this.dgFile.SortCommand += new DataGridSortCommandEventHandler(this.dgFile_SortCommand);
            //this.dgFile.UpdateCommand += new DataGridCommandEventHandler(this.dgFile_UpdateCommand);
            //this.dgFile.ItemDataBound += new DataGridItemEventHandler(this.dgFile_ItemDataBound);
            //this.btnNewFolder.Click += new EventHandler(this.btnNewFolder_Click);
            
            
            
        }

    }
}
