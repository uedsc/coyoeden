// Author:				Tom Opgenorth	
// Created:				2008-04-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
using System;
using System.IO;
using System.Web;

using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog.Services
{
    /// <summary>
    /// This class will save a <see cref="mediaObject" /> to the filesystem.
    /// </summary>
    /// <remarks>
    /// <see cref="mediaObject" />s will be saved to the directory specified by <c>_sourceDirectory</c>, in the folder specified by the
    /// <see cref="mediaObject" />'s <c>.name</c> property.
    /// </remarks>
    public class SaveMediaObjectToFileSystem : IMediaObjectPersistor
    {
        private readonly string _sourceDirectory;

        private static string FixupUrl(string Url)
        {
            if (Url.StartsWith("~"))
            {
                return (HttpContext.Current.Request.ApplicationPath + Url.Substring(1)).Replace("//", "/");
            }

            return Url;
        }

        public SaveMediaObjectToFileSystem(string sourceDirectory)
        {
            _sourceDirectory = sourceDirectory;
        }

        private static string GetPathForMediaObject(string url)
        {
            // TODO [TO080410@2140] Maybe stick the pictures in the images folder, everything else in the files folder.
            FileInfo fileInfo = new FileInfo(HttpContext.Current.Request.MapPath(url));
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            return fileInfo.ToString();
        }

        public mediaObjectInfo Save(mediaObject mediaobject)
        {
            string url = Path.Combine(_sourceDirectory, mediaobject.name);
            string path = GetPathForMediaObject(url);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(mediaobject.bits);
                }
            }
            mediaObjectInfo info = new mediaObjectInfo();
            info.url = FixupUrl(url);
            return info;
        }
    }
}