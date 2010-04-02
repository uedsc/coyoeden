// Author:					Joe Audette
// Created:				    2009-12-30
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Cynthia.Web;
using Cynthia.Web.Framework;


namespace Cynthia.FileSystem
{
    public class DiskFileSystem : IFileSystem
    {
        private DiskFileSystem(IFileSystemPermission permission, char displayPathSeparator) 
        {
            this.permission = permission;
            this.displayPathSeparator = displayPathSeparator;
        }

        private IFileSystemPermission permission = null;
        private char displayPathSeparator = '|';
        private long size = -1;
        private int folderCount = -1;
        private int fileCount = -1;

        public static DiskFileSystem GetFileSystem(IFileSystemPermission permission, char displayPathSeparator)
        {
            if (permission == null) { return null; }
            if(string.IsNullOrEmpty(permission.RootFolder)) { return null; }

            DiskFileSystem fs = new DiskFileSystem(permission, displayPathSeparator);
            return fs;
        }

       
        /// <summary>
        /// used when creating or renaming folders to enforce naming rules
        /// </summary>
        /// <param name="pipeSeparatedSegments"></param>
        /// <returns></returns>
        private string CleanLastSegment(string pipeSeparatedSegments)
        {
            List<string> segments = pipeSeparatedSegments.SplitOnChar(displayPathSeparator);

            if (segments.Count > 0)
            {
                segments[segments.Count - 1] = segments[segments.Count - 1].ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation);
                StringBuilder builder = new StringBuilder();
                string pipe = string.Empty;

                foreach (string seg in segments)
                {
                    builder.Append(pipe + seg);
                    pipe = displayPathSeparator.ToString();
                }

                return builder.ToString();
            }

            return pipeSeparatedSegments;

        }

        private static string GuessMime(string filePath)
        {
            var mime = IOHelper.GetMimeType(Path.GetExtension(filePath).ToLower());
            return mime ?? "application/x-unknown-content-type";
        }

        private void UpdateInfo()
        {
            size = 0;
            fileCount = 0;
            folderCount = 0;
            UpdateInfo(permission.RootFolder);
        }

        private void UpdateInfo(string path)
        {
            var files = Directory.GetFiles(path);
            fileCount += files.Length;
            foreach (var filePath in files)
            {
                size += new FileInfo(filePath).Length;
            }
            var subfolders = Directory.GetDirectories(path);
            folderCount += subfolders.Length;
            foreach (var folderPath in subfolders)
            {
                UpdateInfo(folderPath);
            }
        }

        private bool IsAllowed(string path)
        {
            if (path.Contains(".svn")) { return false; }

            return true;
        }

        private string ResolvePathResult(string path)
        {
            if (Path.IsPathRooted(path)) { path = path.Substring(permission.RootFolder.Length); }

            StringBuilder builder = new StringBuilder(path.Length);
            foreach (var c in path)
            {
                if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
                    builder.Append(displayPathSeparator);
                else
                    builder.Append(c);
            }
            return builder.ToString();
        }


        #region IFileSystem Members

        /// <summary>
        /// resolves the full file system path based on root path plus pipe separated segments like Data|logos
        /// basically it replaces the the pipe with Path.SeparatorChar to resolve the file system path
        /// so if the root is C:\, then it would resolve to C:\Data\logos (on a windows machine)
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public string GetPath(params string[] segments)
        {
            var path = permission.RootFolder;
            foreach (var segment in segments)
                path = Path.Combine(path, segment.Replace(displayPathSeparator, Path.DirectorySeparatorChar));
            return path;
        }

        public string RootFolder
        {
            get { return permission.RootFolder; }
        }

        public string RootFolderDisplayAlias
        {
            get { return permission.DisplayFolder; }
        }

        public OpResult SaveFile(string folderPath, HttpPostedFile file, bool overWrite)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (!permission.IsExtAllowed(Path.GetExtension(file.FileName))) { return OpResult.FileTypeNotAllowed; }

            if (file.ContentLength > permission.MaxSizePerFile) { return OpResult.FileSizeLimitExceed; }

            if (CountAllFiles() >= permission.MaxFiles) { return OpResult.FileLimitExceed; }

            if (GetTotalSize() + file.ContentLength >= permission.Quota) { return OpResult.QuotaExceed; }

            string fileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

            string fullPath = GetPath(folderPath, fileName);

            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                return OpResult.FolderNotFound;

            if (File.Exists(fullPath))
            {
                if (overWrite)
                    File.Delete(fullPath);
                else
                    return OpResult.AlreadyExist;
            }

            file.SaveAs(fullPath);
            return OpResult.Succeed;
        }

        public UserFile RetrieveFile(string path)
        {
            string fullPath = GetPath(path);
            FileInfo info = new FileInfo(fullPath);
            if (info.Exists)
                return new UserFile()
                {
                    Path = fullPath,
                    Name = info.Name,
                    Size = info.Length,
                    ContentType = GuessMime(info.Name),
                    Modified = info.LastWriteTime
                };

            // file not found
            return null;
        }

        public OpResult MoveFile(string srcPath, string destPath, bool overWrite)
        {
            srcPath = GetPath(srcPath);
            destPath = GetPath(destPath);

            if (!File.Exists(srcPath))
                return OpResult.NotFound;

            if (!Directory.Exists(Path.GetDirectoryName(destPath)))
                return OpResult.FolderNotFound;

            if (File.Exists(destPath))
            {
                if (overWrite)
                    File.Delete(destPath);
                else
                    return OpResult.AlreadyExist;
            }

            File.Move(srcPath, destPath);
            return OpResult.Succeed;
        }

        public OpResult DeleteFile(string path)
        {
            string fullPath = GetPath(path);
            if (!File.Exists(fullPath))
                return OpResult.NotFound;

            File.Delete(fullPath);
            return OpResult.Succeed;
        }

        public IEnumerable<UserFileInfo> GetFileList(string path)
        {
            string fullPath = String.IsNullOrEmpty(path) ? permission.RootFolder : GetPath(path);
            if (!Directory.Exists(fullPath)) { return null; }
            int fullPathLen = fullPath.Length + 1;

            var filePaths = Directory.GetFiles(fullPath);
            var files = new List<UserFileInfo>(filePaths.Length);
            foreach (var filePath in filePaths)
            {
                FileInfo file = new FileInfo(filePath);
                files.Add(new UserFileInfo()
                {
                    Name = file.Name,
                    Size = file.Length,
                    ContentType = GuessMime(file.Name),
                    Modified = file.LastWriteTimeUtc
                });

            }
            return files;
        }

        public int CountAllFiles()
        {
            if (fileCount < 0)
                UpdateInfo();
            return fileCount;
        }

        public long GetTotalSize()
        {
            if (size < 0)
                UpdateInfo();
            return size;
        }

        public OpResult CreateFolder(string path)
        {
            if (CountFolders() >= permission.MaxFolders) { return OpResult.FolderLimitExceed; }

            string segments = CleanLastSegment(path);

            string fullPath = GetPath(segments);
            if (Directory.Exists(fullPath))
                return OpResult.AlreadyExist;
            Directory.CreateDirectory(fullPath);
            return OpResult.Succeed;
        }

        public OpResult MoveFolder(string srcPath, string destPath)
        {
            srcPath = GetPath(srcPath);
            destPath = GetPath(destPath);

            if (!Directory.Exists(srcPath))
                return OpResult.NotFound;

            if (!Directory.Exists(Path.GetDirectoryName(destPath)))
                return OpResult.FolderNotFound;

            if (Directory.Exists(destPath))
                return OpResult.AlreadyExist;

            Directory.Move(srcPath, destPath);
            return OpResult.Succeed;
        }

        public OpResult DeleteFolder(string path)
        {
            string fullPath = GetPath(path);
            if (!Directory.Exists(fullPath))
                return OpResult.NotFound;

            Directory.Delete(fullPath, true);
            return OpResult.Succeed;
        }

        public IEnumerable<UserFolder> GetAllFolders()
        {
            DirectoryInfo di = new DirectoryInfo(permission.RootFolder);

            //var folders = from folder in Directory.GetDirectories(permission.RootFolder, "*", SearchOption.AllDirectories)
            //              where IsAllowed(folder)
            //              select folder;

            var folders = from folder in di.GetDirectories("*", SearchOption.AllDirectories)
                          where IsAllowed(folder.Name)
                          select folder;

            return folders.Select(p => new UserFolder() { Path = ResolvePathResult(p.FullName), Name = p.Name });

        }

        public IEnumerable<UserFolder> GetFolderList(string path)
        {
            string fullPath = String.IsNullOrEmpty(path) ? permission.RootFolder : GetPath(path);
            if (!Directory.Exists(fullPath)) { return null; }

            DirectoryInfo di = new DirectoryInfo(fullPath);
            

            //var folders = from folder in Directory.GetDirectories(fullPath, "*", SearchOption.TopDirectoryOnly)
            //              where IsAllowed(folder)
            //              select folder;

            var folders = from folder in di.GetDirectories("*", SearchOption.TopDirectoryOnly)
                          where IsAllowed(folder.Name)
                          select folder;

            return folders.Select(p => new UserFolder() { Path = ResolvePathResult(p.FullName), Name = p.Name });

        }

        public int CountFolders()
        {
            if (folderCount < 0)
                UpdateInfo();
            return folderCount;
        }

        #endregion
    }
}
