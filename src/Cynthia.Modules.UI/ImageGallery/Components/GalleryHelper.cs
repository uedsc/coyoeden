// Author:					Joe Audette
// Created:				    2010-02-12
// Last Modified:			2010-02-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Drawing;
using System.Xml;
using System.IO;
using Cynthia.Business;
using Cynthia.Web.Framework;

namespace Cynthia.Modules.UI.ImageGallery
{
    public static class GalleryHelper
    {
        public static void ProcessImage(GalleryImage galleryImage, string filePath)
        {
            string originalPath = galleryImage.StorageFolderPath + "FullSizeImages" + Path.DirectorySeparatorChar + galleryImage.ImageFile;
            string webSizeImagePath = galleryImage.StorageFolderPath + "WebImages" + Path.DirectorySeparatorChar + galleryImage.WebImageFile;
            string thumbnailPath = galleryImage.StorageFolderPath + "Thumbnails" + Path.DirectorySeparatorChar + galleryImage.ThumbnailFile;

            using (Bitmap originalImage = new Bitmap(originalPath))
            {
                SetExifData(galleryImage, originalImage, filePath);
            }
            galleryImage.Save();

            File.Copy(originalPath, webSizeImagePath, true);
            ImageHelper.ResizeImage(webSizeImagePath, IOHelper.GetMimeType(Path.GetExtension(webSizeImagePath)), galleryImage.WebImageWidth, galleryImage.WebImageHeight);

            File.Copy(originalPath, thumbnailPath, true);
            ImageHelper.ResizeImage(thumbnailPath, IOHelper.GetMimeType(Path.GetExtension(thumbnailPath)), galleryImage.ThumbNailWidth, galleryImage.ThumbNailHeight);

           


        }

        private static void SetExifData(GalleryImage galleryImage, Bitmap originalImage, string filePath)
        {
            XmlDocument metaData = new XmlDocument();
            if (metaData.DocumentElement == null)
            {
                metaData.AppendChild(metaData.CreateElement("MetaData"));
            }


            ImageHelper.SetMetadata("ImageFile", galleryImage.ImageFile, metaData);
            ImageHelper.SetMetadata("WebImageFile", galleryImage.WebImageFile, metaData);
            ImageHelper.SetMetadata("ThumbnailFile", galleryImage.ThumbnailFile, metaData);
            ImageHelper.SetMetadata("Caption", galleryImage.Caption, metaData);
            ImageHelper.SetMetadata("Description", galleryImage.Description, metaData);

            ImageHelper.SetMetadata("OriginalFilename", filePath, metaData);
            ImageHelper.SetMetadata("WebImageWidth", galleryImage.WebImageWidth.ToInvariantString(), metaData);
            ImageHelper.SetMetadata("WebImageHeight", galleryImage.WebImageHeight.ToInvariantString(), metaData);
            ImageHelper.SetMetadata("ThumbNailWidth", galleryImage.ThumbNailWidth.ToInvariantString(), metaData);
            ImageHelper.SetMetadata("ThumbNailHeight", galleryImage.ThumbNailHeight.ToInvariantString(), metaData);

            try
            {
                ImageHelper.SetExifInformation(originalImage, metaData);
            }
            catch { } //TODO log, this feature doesn't work in mono 1.0

            galleryImage.MetaDataXml = metaData.OuterXml;

        }


    }
}
