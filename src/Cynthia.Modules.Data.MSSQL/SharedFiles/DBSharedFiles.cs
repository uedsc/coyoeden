﻿/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2009-12-08
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 
/// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;


namespace Cynthia.Data
{
    
    public static class DBSharedFiles
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }


        public static int AddSharedFileFolder(
            Guid folderGuid,
            Guid moduleGuid,
            Guid parentGuid,
            int moduleId,
            string folderName,
            int parentId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFileFolders_Insert", 6);
            sph.DefineSqlParameter("@FolderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, folderGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateSharedFileFolder(
            int folderId,
            int moduleId,
            string folderName,
            int parentId,
            Guid parentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFileFolders_Update", 5);
            sph.DefineSqlParameter("@FolderID", SqlDbType.Int, ParameterDirection.Input, folderId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteSharedFileFolder(int folderId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFileFolders_Delete", 1);
            sph.DefineSqlParameter("@FolderID", SqlDbType.Int, ParameterDirection.Input, folderId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetSharedFileFolder(int folderId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFileFolders_SelectOne", 1);
            sph.DefineSqlParameter("@FolderID", SqlDbType.Int, ParameterDirection.Input, folderId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSharedModuleFolders(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFileFolders_SelectAllByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSharedFolders(int moduleId, int parentId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFileFolders_SelectByModule", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            return sph.ExecuteReader();
        }

        public static int AddSharedFile(
            Guid itemGuid,
            Guid moduleGuid,
            Guid userGuid,
            Guid folderGuid,
            int moduleId,
            int uploadUserId,
            string friendlyName,
            string originalFileName,
            string serverFileName,
            int sizeInKB,
            DateTime uploadDate,
            int folderId,
            string description)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_Insert", 13);

            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@FolderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, folderGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UploadUserID", SqlDbType.Int, ParameterDirection.Input, uploadUserId);
            sph.DefineSqlParameter("@FriendlyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyName);
            sph.DefineSqlParameter("@OriginalFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, originalFileName);
            sph.DefineSqlParameter("@ServerFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, serverFileName);
            sph.DefineSqlParameter("@SizeInKB", SqlDbType.Int, ParameterDirection.Input, sizeInKB);
            sph.DefineSqlParameter("@UploadDate", SqlDbType.DateTime, ParameterDirection.Input, uploadDate);
            sph.DefineSqlParameter("@FolderID", SqlDbType.Int, ParameterDirection.Input, folderId);
            sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateSharedFile(
            int itemId,
            int moduleId,
            int uploadUserId,
            string friendlyName,
            string originalFileName,
            string serverFileName,
            int sizeInKB,
            DateTime uploadDate,
            int folderId,
            Guid folderGuid,
            Guid userGuid,
            string description)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_Update", 12);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UploadUserID", SqlDbType.Int, ParameterDirection.Input, uploadUserId);
            sph.DefineSqlParameter("@FriendlyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyName);
            sph.DefineSqlParameter("@OriginalFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, originalFileName);
            sph.DefineSqlParameter("@ServerFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, serverFileName);
            sph.DefineSqlParameter("@SizeInKB", SqlDbType.Int, ParameterDirection.Input, sizeInKB);
            sph.DefineSqlParameter("@UploadDate", SqlDbType.DateTime, ParameterDirection.Input, uploadDate);
            sph.DefineSqlParameter("@FolderID", SqlDbType.Int, ParameterDirection.Input, folderId);
            sph.DefineSqlParameter("@FolderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, folderGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool IncrementDownloadCount(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_IncrementDownloadCount", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }



        public static bool DeleteSharedFile(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetSharedFile(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_SelectOne", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSharedFiles(int moduleId, int folderId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_SelectByModule", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@FolderID", SqlDbType.Int, ParameterDirection.Input, folderId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSharedFilesByPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFiles_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }


        public static bool AddHistory(
            Guid itemGuid,
            Guid moduleGuid,
            Guid userGuid,
            int itemId,
            int moduleId,
            string friendlyName,
            string originalFileName,
            string serverFileName,
            int sizeInKB,
            DateTime uploadDate,
            int uploadUserId,
            DateTime archiveDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFilesHistory_Insert", 12);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@FriendlyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyName);
            sph.DefineSqlParameter("@OriginalFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, originalFileName);
            sph.DefineSqlParameter("@ServerFileName", SqlDbType.NVarChar, 50, ParameterDirection.Input, serverFileName);
            sph.DefineSqlParameter("@SizeInKB", SqlDbType.Int, ParameterDirection.Input, sizeInKB);
            sph.DefineSqlParameter("@UploadDate", SqlDbType.DateTime, ParameterDirection.Input, uploadDate);
            sph.DefineSqlParameter("@UploadUserID", SqlDbType.Int, ParameterDirection.Input, uploadUserId);
            sph.DefineSqlParameter("@ArchiveDate", SqlDbType.DateTime, ParameterDirection.Input, archiveDate);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return (newID > 0);
        }

        public static bool DeleteHistory(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFilesHistory_Delete", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteHistoryByItemID(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFilesHistory_DeleteByItemID", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetHistory(int moduleId, int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFilesHistory_Select", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetHistoryFile(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SharedFilesHistory_SelectOne", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            return sph.ExecuteReader();
        }





    }
}
