// Author:					Joe Audette
// Created:				    2009-12-30
// Last Modified:			2010-01-02
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
using System.Web.Hosting;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;
using Cynthia.Web.Framework;


namespace Cynthia.FileSystem
{
    public class DiskFileSystemProvider : FileSystemProvider
    {
        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private const long bytesPerMegabyte = 1048576;

        
        public override IFileSystem GetFileSystem(char displayPathSeparator)
        {
            IFileSystemPermission p = GetFileSystemPermission();
            if (p == null) { return null; }
            if(string.IsNullOrEmpty(p.RootFolder)) { return null; }

            return DiskFileSystem.GetFileSystem(p, displayPathSeparator);
        }

        private IFileSystemPermission GetFileSystemPermission()
        {
           
            return new FileSystemPermission()
            {
                RootFolder = GetRootPath(),
                DisplayFolder = GetDisplayPath(),
                Quota = GetQuota(),
                MaxSizePerFile = GetMaxSizePerFile(),
                MaxFiles = GetMaxFiles(),
                MaxFolders = GetMaxFolders(),
                AllowedExtensions = GetAllowedExtensions()
            };
        }

        

        private string GetDisplayPath()
        {
            return WebUtils.GetApplicationRoot() + GetVirtualPath().Replace("~/", "/").TrimEnd('/');
            
        }

        private string GetRootPath()
        {
            string virtualPath = GetVirtualPath();
            if(string.IsNullOrEmpty(virtualPath)) { return virtualPath; }
            string rootPath = HostingEnvironment.MapPath(virtualPath);
            
            if (!Directory.Exists(rootPath)) { Directory.CreateDirectory(rootPath); }
            return rootPath;

        }

       
        private string GetVirtualPath()
        {
            string virtualPath = string.Empty;
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                throw new ArgumentNullException("could not load SiteSettings");
            }

            if (WebUser.IsAdminOrContentAdmin)
            {
                if (WebConfigSettings.ForceAdminsToUseMediaFolder)
                {
                    virtualPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/";

                }
                else
                {
                    if ((siteSettings.IsServerAdminSite)&&(WebConfigSettings.AllowAdminsToUseDataFolder) && (WebUser.IsAdmin))
                    {
                        virtualPath = "~/Data/";
                    }
                    else
                    {
                        virtualPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/";

                    }
                }
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                virtualPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/";

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null)
                {
                    throw new ArgumentNullException("could not load current SiteUser");
                }

                virtualPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/userfiles/" + currentUser.UserId.ToInvariantString() + "/";

            }

            return virtualPath;

        }

        private int GetMaxFiles()
        {
            if (WebUser.IsAdminOrContentAdmin)
            {
                return WebConfigSettings.AdminMaxNumberOfFiles;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderMaxNumberOfFiles;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderMaxNumberOfFiles;
            }

            return 0;
        }

        private int GetMaxFolders()
        {
            if (WebUser.IsAdminOrContentAdmin)
            {
                return WebConfigSettings.AdminMaxNumberOfFolders;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderMaxNumberOfFolders;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderMaxNumberOfFolders;
            }

            return 0;
        }

        private long GetMaxSizePerFile()
        {
            if (WebUser.IsAdminOrContentAdmin)
            {
                return WebConfigSettings.AdminMaxSizePerFileInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderMaxSizePerFileInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderMaxSizePerFileInMegaBytes * bytesPerMegabyte;
            }

            return 0;
        }

        private long GetQuota()
        {
            if (WebUser.IsAdminOrContentAdmin)
            {
                return WebConfigSettings.AdminDiskQuotaInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderDiskQuotaInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderDiskQuotaInMegaBytes * bytesPerMegabyte;
            }

            return 0;
        }

        private IEnumerable<string> GetAllowedExtensions()
        {

            if (WebUser.IsAdminOrContentAdmin)
            {
                return WebConfigSettings.AllowedUploadFileExtensions.SplitOnChar('|');
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.AllowedUploadFileExtensions.SplitOnChar('|');

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions.SplitOnChar('|');

            }

            return new string[0];
        }

    }
}
