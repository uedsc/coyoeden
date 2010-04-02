// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2010-01-05
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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;


namespace Cynthia.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class DBBlog
    {
       
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }



        public static IDataReader GetBlogs(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_Select", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static int GetCount(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blogs_GetCount", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return Convert.ToInt32(sph.ExecuteScalar());
          
        }

        public static IDataReader GetPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(moduleId, beginDate, currentTime);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blogs_SelectPage", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


        public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectForSiteMap", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@CurrentUtcDateTime", SqlDbType.DateTime, ParameterDirection.Input, currentUtcDateTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetDrafts(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectDrafts", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBlogsByPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }


        public static IDataReader GetBlogStats(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogStats_Select", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }


        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {
            if (CultureInfo.CurrentCulture.Name == "fa-IR")
            {
                return GetBlogMonthArchiveForPerisian(moduleId, currentTime);
            }

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectArchiveByMonth", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// By A.Samarian 2009-09-11
        /// special handling to support Persian Calendar 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>IDataReader</returns>
        public static IDataReader GetBlogMonthArchiveForPerisian(int moduleId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectArchiveByMonth_Persian", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }


        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
        {
            if (CultureInfo.CurrentCulture.Name == "fa-IR")
            {
                return GetBlogEntriesByMonthPersian(month, year, moduleId, currentTime);
            }

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectByMonth", 4);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// By A.Samarian 2009-09-11
        /// special handling to support Persian Calendar 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>IDataReader</returns>
        public static IDataReader GetBlogEntriesByMonthPersian(int month, int year, int moduleId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectByMonth_Persian", 4);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetEntriesByCategory(int moduleId, int categoryId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectByCategory", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }


        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_SelectOne", 2);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static bool DeleteBlog(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static int AddBlog(
            Guid blogGuid,
            Guid moduleGuid,
            int moduleId,
            string userName,
            string title,
            string excerpt,
            string description,
            DateTime startDate,
            bool isInNewsletter,
            bool includeInFeed,
            int allowCommentsForDays,
            string location,
            Guid userGuid,
            DateTime createdDate,
            string itemUrl,
            string metaKeywords,
            string metaDescription,
            string compiledMeta,
            bool isPublished)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_Insert", 20);

            sph.DefineSqlParameter("@BlogGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, blogGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.Input, userName);
            sph.DefineSqlParameter("@Heading", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Abstract", SqlDbType.NText, ParameterDirection.Input, excerpt);
            sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Location", SqlDbType.NText, ParameterDirection.Input, location);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@IsInNewsletter", SqlDbType.Bit, ParameterDirection.Input, isInNewsletter);
            sph.DefineSqlParameter("@IncludeInFeed", SqlDbType.Bit, ParameterDirection.Input, includeInFeed);
            sph.DefineSqlParameter("@AllowCommentsForDays", SqlDbType.Int, ParameterDirection.Input, allowCommentsForDays);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@ItemUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, itemUrl);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NText, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.InputOutput, null);


            sph.ExecuteNonQuery();
            int newID = Convert.ToInt32(sph.Parameters[19].Value);
            return newID;
        }

        public static bool UpdateBlog(
            int moduleId,
            int itemId,
            string userName,
            string title,
            string excerpt,
            string description,
            DateTime startDate,
            bool isInNewsletter,
            bool includeInFeed,
            int allowCommentsForDays,
            string location,
            Guid lastModUserGuid,
            DateTime lastModUtc,
            string itemUrl,
            string metaKeywords,
            string metaDescription,
            string compiledMeta,
            bool isPublished)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Blog_Update", 18);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.Input, userName);
            sph.DefineSqlParameter("@Heading", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Abstract", SqlDbType.NText, ParameterDirection.Input, excerpt);
            sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@IsInNewsletter", SqlDbType.Bit, ParameterDirection.Input, isInNewsletter);
            sph.DefineSqlParameter("@IncludeInFeed", SqlDbType.Bit, ParameterDirection.Input, includeInFeed);
            sph.DefineSqlParameter("@AllowCommentsForDays", SqlDbType.Int, ParameterDirection.Input, allowCommentsForDays);
            sph.DefineSqlParameter("@Location", SqlDbType.NText, ParameterDirection.Input, location);
            sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@ItemUrl", SqlDbType.NVarChar, 512, ParameterDirection.Input, itemUrl);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NText, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool AddBlogComment(
          int moduleId,
          int itemId,
          string name,
          string title,
          string url,
          string comment,
                DateTime dateCreated)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogComment_Insert", 7);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 100, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@URL", SqlDbType.NVarChar, 200, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Comment", SqlDbType.NText, ParameterDirection.Input, comment);
            sph.DefineSqlParameter("@DateCreated", SqlDbType.DateTime, ParameterDirection.Input, dateCreated);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


        public static bool DeleteAllCommentsForBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            // TODO: use stored proc

            sqlCommand.Append("DELETE FROM cy_BlogComments  ");
            sqlCommand.Append("WHERE ItemID = @ItemID  ");

            SqlParameterHelper sph = new SqlParameterHelper(
                GetConnectionString(),
                sqlCommand.ToString(),
                CommandType.Text, 1);

            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool UpdateCommentStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            // TODO: use stored proc

            sqlCommand.Append("UPDATE cy_BlogStats  ");
            sqlCommand.Append("SET 	CommentCount = (SELECT COUNT(*) FROM cy_BlogComments WHERE ModuleID = @ModuleID)   ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");

            SqlParameterHelper sph = new SqlParameterHelper(
                GetConnectionString(),
                sqlCommand.ToString(),
                CommandType.Text, 1);

            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool UpdateEntryStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            // TODO: use stored proc

            sqlCommand.Append("UPDATE cy_BlogStats  ");
            sqlCommand.Append("SET 	EntryCount = (SELECT COUNT(*) FROM cy_Blogs WHERE ModuleID = @ModuleID)   ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");

            SqlParameterHelper sph = new SqlParameterHelper(
                GetConnectionString(),
                sqlCommand.ToString(),
                CommandType.Text, 1);

            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteBlogComment(int commentId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogComment_Delete", 1);
            sph.DefineSqlParameter("@BlogCommentID", SqlDbType.Int, ParameterDirection.Input, commentId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static IDataReader GetBlogComments(int moduleId, int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogComments_Select", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        public static int AddBlogCategory(
          int moduleId,
          string category)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogCategories_Insert", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@Category", SqlDbType.NVarChar, 255, ParameterDirection.Input, category);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateBlogCategory(
          int categoryId,
          string category)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogCategories_Update", 2);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@Category", SqlDbType.NVarChar, 255, ParameterDirection.Input, category);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteCategory(int categoryId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogCategories_Delete", 1);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetCategory(int categoryId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogCategories_SelectOne", 1);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetCategories(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogCategories_SelectByModule", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();
        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogCategories_SelectListByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        public static int AddBlogItemCategory(
          int itemId,
          int categoryId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogItemCategories_Insert", 2);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool DeleteItemCategories(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogItemCategories_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetBlogItemCategories(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_BlogItemCategories_SelectByItem", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }






    }
}
