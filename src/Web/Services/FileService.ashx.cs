// Author:					Joe Audette
// Created:				    2009-12-28
// Last Modified:			2009-12-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using log4net;
using Cynthia.Web.Framework;
using Cynthia.FileSystem;


namespace Cynthia.Web.Services
{
    /// <summary>
    /// Service endpoint for file operations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FileService : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileService));

        IFileSystem fileSystem = null;
        private char displayPathSeparator = '|';
        protected bool overwriteExistingFiles = false;
        private const string DefaultFilePath = "~/Data";
        private string folder = string.Empty;
        private string siteRoot = string.Empty;

        private string cmd = string.Empty;
        private string path = string.Empty;
        private string srcPath = string.Empty;
        private string destPath = string.Empty;
        HttpPostedFile fileUploaded = null;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

       
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                LoadSettings(context);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                RenderJsonResult(context, OpResult.Error);
                return;
            }

            //will be bull if user has not permissions
            if (fileSystem == null)
            {
                log.Info("Could not load file system or user not allowed so blocking access.");
                RenderJsonResult(context, OpResult.Denied);
                return;
            }

            switch (cmd)
            {
                case "download":
                    Download(context);
                    break;

                case "movefile":
                    MoveFile(context);
                    break;

                case "deletefile":
                    DeleteFile(context);
                    break;

                case "listfiles":
                    ListFiles(context);
                    break;

                case "upload":
                    Upload(context);
                    break;

                case "createfolder":
                    CreateFolder(context);
                    break;

                case "movefolder":
                    MoveFolder(context);
                    break;

                case "deletefolder":
                    DeleteFolder(context);
                    break;

                case "listfolders":
                    ListFolders(context);
                    break;

                default:
                    RenderJsonResult(context, OpResult.Error);
                    break;
            }

        }

        #region File Methods

        private void Download(HttpContext context)
        {
            try
            {
                if (OnFileDownloading(path))
                {
                    var file = fileSystem.RetrieveFile(path);

                    if (file != null)
                    {   
                        //TODO: may need to support stream for other file system implementations
                        //if (file.Stream != null)
                        //{
                        //    //return File(file.Stream, file.ContentType, context.Server.UrlEncode(file.Name));
                        //    //context.Response.OutputStream.Write(
                        //}
                        //else if (file.Path != null)
                        if (file.Path != null)
                        {
       
                            context.Response.AppendHeader("Content-Length", file.Size.ToString(CultureInfo.InvariantCulture));
                            context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name.Trim() + "\"");
                            
                            try
                            {
                                context.Response.Buffer = false;
                                context.Response.BufferOutput = false;
                                context.Response.TransmitFile(file.Path);
                                context.Response.End();
                                return;
                            }
                            catch (HttpException ex) 
                            {
                                log.Error(ex);
                            }

                           
                        }
                        //else if (file.Data != null)
                        //{
                        //    //return File(file.Data, file.ContentType, context.Server.UrlEncode(file.Name));
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                context.Response.StatusCode = 500;
            }

            // Requested file not found
            context.Response.StatusCode = 404;

        }

        private void Upload(HttpContext context)
        {
            var result = OpResult.Denied;
           
            string jsonResult = serializer.Serialize(result);

            if (context.Request.Files.Count > 0)
            {
                fileUploaded = context.Request.Files[0];

                try
                {
                    if (OnFileUploading(path, fileUploaded, ref result))
                    {
                        result = fileSystem.SaveFile(path, fileUploaded, overwriteExistingFiles);
                        if (result == OpResult.Succeed)
                        {
                            jsonResult = serializer.Serialize(
                                UserFileInfo.FromPostedFile(fileUploaded,
                                Path.GetFileName(fileUploaded.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles)).ToJson());

                        }
                        else
                        {
                            jsonResult = serializer.Serialize(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = OpResult.Error;
                    jsonResult = serializer.Serialize(result);
                }
            }
            else
            {
                // no file posted
                jsonResult = serializer.Serialize(OpResult.Error);
            }


             //this is supposed to be an html response not a json response

            context.Response.ContentType = "text/html";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;
            context.Response.Write(BuildHtmlWrapper(context, jsonResult));
                       

            //RenderJsonResult(context, result);
            
        }

        private string BuildHtmlWrapper(HttpContext context, string json)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head>\n");
            sb.Append("<title>Upload Result</title>");
            sb.Append("\n</head>");

            sb.Append("<body><p>");
            sb.Append(context.Server.HtmlEncode(json));

            sb.Append("</p></body>");
            sb.Append("</html>\r\n");

            return sb.ToString();
        }

        private void ListFiles(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFileListing(path, ref result))
                {
                    var files = fileSystem.GetFileList(path);

                    context.Response.ContentType = "text/javascript";
                    Encoding encoding = new UTF8Encoding();
                    context.Response.ContentEncoding = encoding;
                    context.Response.Write(serializer.Serialize(files.Select(f => f.ToJson())));
                }
                else
                {
                    RenderJsonResult(context, result);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                RenderJsonResult(context, OpResult.Error);
            }

        }

        private void DeleteFile(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFileDeleting(path, ref result))
                {
                    result = fileSystem.DeleteFile(path);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

            RenderJsonResult(context, result);

        }

        private void MoveFile(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFileMoving(srcPath, destPath, ref result))
                {
                    result = fileSystem.MoveFile(srcPath, destPath, false);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

            RenderJsonResult(context, result);

        }

        #endregion

        #region Folder Methods

        private void CreateFolder(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFolderCreating(path, ref result))
                {
                    result = fileSystem.CreateFolder(path);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

            RenderJsonResult(context, result);
        }

        private void ListFolders(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFolderListing(ref result))
                {
                    var folders = fileSystem.GetAllFolders();
                    context.Response.ContentType = "text/javascript";
                    Encoding encoding = new UTF8Encoding();
                    context.Response.ContentEncoding = encoding;
                    context.Response.Write(serializer.Serialize(folders.Select(f => f.ToJson())));

                }
                else
                {
                    RenderJsonResult(context, result);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

        }

        private void DeleteFolder(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFolderDeleting(path, ref result))
                {
                    result = fileSystem.DeleteFolder(path);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

            RenderJsonResult(context, result);

        }

        private void MoveFolder(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFolderMoving(srcPath, destPath, ref result))
                {
                    result = fileSystem.MoveFolder(srcPath, destPath);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

            RenderJsonResult(context, result);

        }

        #endregion


        private void RenderJsonResult(HttpContext context, OpResult result)
        {
            context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            context.Response.Write("{");
            if (result == OpResult.Succeed)
            {
                context.Response.Write("\"succeed\":true");
            }
            else
            {
                context.Response.Write("\"succeed\":false");
            }

            context.Response.Write(",\"status\" : \"" + result.ToString() + "\"");


            context.Response.Write("}");
        }

        private void LoadSettings(HttpContext context)
        {

            if (context.Request.QueryString["cmd"] != null) { cmd = context.Request.QueryString["cmd"]; }
            if (context.Request.Params.Get("path") != null) { path = context.Request.Params.Get("path"); }
            if (context.Request.QueryString["srcPath"] != null) { srcPath = context.Request.QueryString["srcPath"]; }
            if (context.Request.QueryString["destPath"] != null) { destPath = context.Request.QueryString["destPath"]; }

            siteRoot = SiteUtils.GetNavigationSiteRoot();

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null)
            {
                log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
                return;
            }

            fileSystem = p.GetFileSystem(displayPathSeparator);
            if (fileSystem == null)
            {
                log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
                return;
            }

        }

        protected virtual bool OnFileDownloading(string path)
        {
            return true;
        }

        protected virtual bool OnFolderCreating(string path, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFolderMoving(string srcPath, string destPath, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFolderDeleting(string path, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFolderListing(ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileListing(string path, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileUploading(string path, HttpPostedFile fileUploaded, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileMoving(string srcPath, string destPath, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileDeleting(string path, ref OpResult result)
        {
            return true;
        }


        

        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
