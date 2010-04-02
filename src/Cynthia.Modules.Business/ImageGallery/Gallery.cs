// Author:				Joe Audette
// Created:			    2004-11-28
// Last Modified:		2009-06-22

using System.Data;
using Cynthia.Data;

namespace Cynthia.Business
{
    /// <summary>
    /// Represents and image gallery instance
    /// </summary>
    public class Gallery
    {

        public Gallery(int moduleId)
        {
            this.galleryID = moduleId;
        }

        private int galleryID = 0;

        public IDataReader GetAllImages()
        {
            return DBGallery.GetAllImages(this.galleryID);
        }

        public DataTable GetThumbsByPage(int pageNumber, int thumbsPerPage)
        {
            return DBGallery.GetThumbsByPage(this.galleryID, pageNumber, thumbsPerPage);

        }

        //public DataSet GetThumbsByPageDataset(int pageNumber, int thumbsPerPage)
        //{
        //    return dbGallery.GetThumbsByPageDataset(this.galleryID, pageNumber, thumbsPerPage);

        //}

        public DataTable GetWebImageByPage(int pageNumber)
        {
            return DBGallery.GetWebImageByPage(this.galleryID, pageNumber);
        }

        //public static bool DeleteGalleryImage(int itemID) 
        //{
        //    return dbGallery.GalleryImage_DeleteGalleryImage(itemID);

        //}

        public static IDataReader GetAllImages(int moduleId)
        {
            return DBGallery.GetAllImages(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBGallery.DeleteBySite(siteId);
        }

        public static bool DeleteByModule(int moduleId)
        {
            return DBGallery.DeleteByModule(moduleId);
        }

    }
}
