/// Author:					    Joe Audette
/// Created:				    2007-01-28
/// Last Modified:			    2009-05-30
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Globalization;
using System.Text;
using System.Web;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using log4net;

namespace Cynthia.Web
{
   
    public partial class SharedFilesDownload : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SharedFilesDownload));

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            
            base.OnInit(e);
        }
        #endregion

        private int pageID = -1;
        private int moduleID = -1;
        private int fileID = -1;
        private SharedFile sharedFile = null;
        


        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableDownloadCache();
            //Server.ScriptTimeout

            if (
                (LoadAndCheckParams())
                && (UserHasPermission())
                )
            {
                DownloadFile();
            }
            else
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                    return;
                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

            }

        }

        private void DownloadFile()
        {
            if (
                (CurrentPage != null)
                &&(sharedFile != null)
                )
            {
                string downloadPath = Page.Server.MapPath(WebUtils.GetApplicationRoot() 
                    + "/Data/Sites/" + this.CurrentPage.SiteId.ToString(CultureInfo.InvariantCulture) + "/SharedFiles/") 
                    + sharedFile.ServerFileName;

                if (File.Exists(downloadPath))
                {
                    FileInfo fileInfo = new System.IO.FileInfo(downloadPath);
                    Page.Response.AppendHeader("Content-Length", fileInfo.Length.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    log.Error("Shared File Not Found. User tried to download file " + downloadPath);
                    return;
                }

                string fileType = Path.GetExtension(sharedFile.FriendlyName).Replace(".", string.Empty);

                string mimeType = SiteUtils.GetMimeType(fileType);
                Page.Response.ContentType = mimeType;

                if (SiteUtils.IsNonAttacmentFileType(fileType))
                {
                    //this will display the pdf right in the browser
                    Page.Response.AddHeader("Content-Disposition", "filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName, Encoding.UTF8) + "\"");
                }
                else
                {
                    // other files just use file save dialog
                    Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName, Encoding.UTF8) + "\"");
                }

                //Page.Response.AddHeader("Content-Length", documentFile.DocumentImage.LongLength.ToString());

                try
                {
                    Page.Response.Buffer = false;
                    Page.Response.BufferOutput = false;
                    if (Page.Response.IsClientConnected)
                    {
                        // TODO: a different solution, Response.TransmitFile has a 2 GB limit
                        Page.Response.TransmitFile(downloadPath);
                        SharedFile.IncrementDownloadCount(sharedFile.ItemId);
                    }
                    Page.Response.End();
                }
                catch (HttpException) { }
            }

        }

        private bool UserHasPermission()
        {
            bool result = false;

            if (
                (CurrentPage != null)
                &&(WebUser.IsInRoles(CurrentPage.AuthorizedRoles))
                )
            {
                bool moduleIsOnPage = false;
                
                foreach (Module m in CurrentPage.Modules)
                {
                    if (m.ModuleId == moduleID)
                    {
                        moduleIsOnPage = true;
                        // user has page viewpermission but not module view permission so his module is no visible on the page for this user.
                        if (!WebUser.IsInRoles(m.ViewRoles)) { return false; }

                    }

                }

                if (moduleIsOnPage)
                {
                    sharedFile = new SharedFile(moduleID, fileID);

                    if (sharedFile.ModuleId == moduleID) result = true;
                }

            }

            return result;

        }

        private bool LoadAndCheckParams()
        {
            bool result = true;

            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);
            fileID = WebUtils.ParseInt32FromQueryString("fileid", -1);
            if (pageID == -1) result = false;
            if (moduleID == -1) result = false;
            if (fileID == -1) result = false;

            return result;

        }

        
    }
}
