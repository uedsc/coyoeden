//  Author:                     Joe Audette
//  Created:                    2009-08-16
//	Last Modified:              2010-03-15
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Services.Protocols;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using log4net;

namespace Cynthia.Web.Services
{
    /// <summary>
    /// Returns html fragments representing folders and files.
    /// Used for populating the jQueryFileTree using the MCEFileDialog.aspx page which is used in TinyMCE editor
    /// http://plugins.jquery.com/project/filetree
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class jqueryFileTreeMediaBrowser : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(jqueryFileTreeMediaBrowser));
        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private string rootDir = string.Empty;
        private string currentDir = string.Empty;
        private string type = "image";
        private string serverName = string.Empty;
        private bool canView = false;
        private string allowedExtensions = string.Empty;
        private Page page = new Page();

        public void ProcessRequest(HttpContext context)
        {
            LoadSettings(context);

            if (!canView)
            {
                context.Response.Write("<span class='txterror'>" + context.Server.HtmlEncode(Resource.AccessDeniedLabel) + "</span>");
                return;
            }
            
            RenderFileTree(context);

        }

        private void LoadSettings(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            //if (!(WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))) { return; }

            //this is only used to resolve the paths since httphandler does not have it built in
            
            page.AppRelativeVirtualPath = context.Request.AppRelativeCurrentExecutionFilePath; 


            if (WebUser.IsAdminOrContentAdmin)
            {
                if (WebConfigSettings.ForceAdminsToUseMediaFolder)
                {
                    rootDir = page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/");
                }
                else
                {
                    if ((WebConfigSettings.AllowAdminsToUseDataFolder)&&(WebUser.IsAdmin))
                    {
                        rootDir = page.ResolveUrl("~/Data/");
                    }
                    else
                    {
                        rootDir = page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/");
                    }
                }
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                canView = true;
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                rootDir = page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/");
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                canView = true;

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null) { return; }

                rootDir = page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/userfiles/" + currentUser.UserId.ToInvariantString() + "/");
                allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
                canView = true;
            }

            if (!canView) { return; }

            if (!Directory.Exists(context.Server.MapPath(rootDir)))
            {
                try
                {
                    Directory.CreateDirectory(context.Server.MapPath(rootDir));

                }
                catch (IOException ex)
                {
                    log.Error("failed to create the directory " + rootDir, ex);
                    canView = false;
                    return;
                }

            }

            currentDir = rootDir;

            if (context.Request.Params.Get("dir") != null)
            {
                string requestedDir = context.Server.UrlDecode(context.Request.Params.Get("dir"));
                
                if (requestedDir == "/Pages/")
                {
                    currentDir = requestedDir;
                }
                else
                {
                    if (IsChildDirectory(context, requestedDir)) { currentDir = requestedDir; }
                }
            }

            ResolveType(context);

            

        }

        private void RenderFileTree(HttpContext context)
        {
            
            context.Response.Write("<ul class=\"jqueryFileTree\" style=\"display: none;\">\n");

            if (type == "file")
            {
                //this allows browsingpages in the links browser
                if (currentDir == "/Pages/")
                {
                    //context.Response.Write("\t<li class=\"directory expanded\"><a href=\"#\" rel=\"" + "Pages" + "/\">" + context.Server.HtmlEncode(Resource.PageBrowseNode) + "</a></li>\n");
                    RenderPages(context);
                }
                else if(currentDir == rootDir)
                {
                    context.Response.Write("\t<li class=\"directory collapsed\"><a href=\"#\" rel=\"" + "/Pages" + "/\">" + context.Server.HtmlEncode(Resource.PageBrowseNode) + "</a></li>\n");
                }
            }

            if (!(currentDir == "/Pages/"))
            {
                DirectoryInfo currentDirectory = new DirectoryInfo(context.Server.MapPath(currentDir));

                foreach (DirectoryInfo subDirectory in currentDirectory.GetDirectories())
                {
                    if (subDirectory.Name == ".svn") { continue; }

                    context.Response.Write("\t<li class=\"directory collapsed\"><a href=\"#\" rel=\"" + currentDir + subDirectory.Name + "/\">" + subDirectory.Name + "</a></li>\n");
                }

            
                foreach (FileInfo fileInfo in currentDirectory.GetFiles())
                {
                    if ((type == "image") && (!fileInfo.IsWebImageFile())) { continue; }

                    if ((type == "media") && (!fileInfo.IsAllowedMediaFile())) { continue; }

                    if ((type == "file") && (!fileInfo.IsAllowedUploadBrowseFile(allowedExtensions))) { continue; }

                    string ext = "";
                    if (fileInfo.Extension.Length > 1) { ext = fileInfo.Extension.Substring(1).ToLower(); }
                    context.Response.Write("\t<li class=\"file ext_" + ext + "\"><a href=\"#\" rel=\"" + currentDir + fileInfo.Name + "\">" + fileInfo.Name + "</a></li>\n");
                }
            }
            
            context.Response.Write("</ul>");
            //context.Response.Write("<input type='hidden' name='hdnCurrentServerDir' id='hdnCurrentServerDir' value='" + currentDir + "' /> ");

        }

        private void RenderPages(HttpContext context)
        {
            serverName = WebUtils.GetHostName();
            string serverPort = context.Request.ServerVariables["SERVER_PORT"];
            if ((serverPort != "80") && (serverPort != "443"))
            {
                serverName += ":" + serverPort;
            }

            SiteMapDataSource siteMapDataSource = new SiteMapDataSource();

            siteMapDataSource.SiteMapProvider = "Csite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            RenderSiteMapNodes(context, siteMapNode, string.Empty);

        }

        private void RenderSiteMapNodes(
            HttpContext context,
            SiteMapNode siteMapNode,
            string pagePrefix)
        {
            CSiteMapNode CNode = (CSiteMapNode)siteMapNode;

            if (!CNode.IsRootNode)
            {
                if (WebUser.IsInRoles(CNode.ViewRoles))
                {
                    if (CNode.ParentId > -1) pagePrefix += "-";

                    //context.Response.Write("\t<li class=\"file ext_txt\"><a href=\"#\" rel=\"" + CNode.Url.Replace("~/", "/") + "\">" + pagePrefix + CNode.Title + "</a></li>\n");
                    context.Response.Write("\t<li class=\"file ext_txt\"><a href=\"#\" rel=\"" + page.ResolveUrl(CNode.Url) + "\">" + pagePrefix + CNode.Title + "</a></li>\n");

                    //XmlNode oFileNode = XmlUtil.AppendElement(listNode, "File");
                    //XmlUtil.SetAttribute(oFileNode, "name", pagePrefix + CNode.Title);
                    //XmlUtil.SetAttribute(oFileNode, "size", 1.ToString(CultureInfo.InvariantCulture));

                    //XmlUtil.SetAttribute(oFileNode, "url", Page.ResolveUrl(CNode.Url));


                }
            }


            foreach (SiteMapNode childNode in CNode.ChildNodes)
            {
                //recurse to populate children
                RenderSiteMapNodes(context, childNode, pagePrefix);

            }


        }

        private bool IsChildDirectory(HttpContext context, string requestedDirectory)
        {
            if(string.IsNullOrEmpty(requestedDirectory)){ return false; }
            if (!Directory.Exists(context.Server.MapPath(requestedDirectory))) { return false; }

            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(context.Server.MapPath(rootDir));
            DirectoryInfo requestedDirectoryInfo = new DirectoryInfo(context.Server.MapPath(requestedDirectory));
            return IOHelper.IsDecendentDirectory(rootDirectoryInfo, requestedDirectoryInfo);
        }

        private void ResolveType(HttpContext context)
        {
            string requestedType = "image";
            if (context.Request.QueryString["type"] != null)
            {
                requestedType = context.Request.QueryString["type"];
            }

            switch (requestedType)
            {
                case "media":
                    type = "media";
                    break;

                case "file":
                    type = "file";
                    break;

                case "image":
                default:
                    type = "image";
                    break;

            }

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
