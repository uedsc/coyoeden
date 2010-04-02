// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2010-02-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Note moved into separate class file from dbPortal 2007-11-03
// 

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;


namespace Cynthia.Data
{
    public static class DBBlog
    {
        
        public static String DBPlatform()
        {
            return "MySQL";
        }

        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["MySqlConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["MySqlWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["MySqlWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["MySqlConnectionString"];
        }



        public static IDataReader GetBlogs(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM cy_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'BlogEntriesToShowSetting' ");
            sqlCommand.Append("AND ModuleID = ?ModuleID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = int.Parse(ConfigurationManager.AppSettings["DefaultBlogPageSize"]);

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    try
                    {
                        rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                    }
                    catch { }

                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	cy_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = ?ModuleID  ");
            sqlCommand.Append("AND ?BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= ?CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString() + " ;  ");

            arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static int GetCount(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_Blogs ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ");
            sqlCommand.Append("AND ?BeginDate >= StartDate  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= ?CurrentTime  ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                 GetReadConnectionString(),
                 sqlCommand.ToString(),
                 arParams));

        }

        public static IDataReader GetPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	cy_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = ?ModuleID  ");
            sqlCommand.Append("AND ?BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= ?CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }



        public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("b.ItemUrl, ");
            sqlCommand.Append("b.LastModUtc ");

            sqlCommand.Append("FROM	cy_Blogs b ");

            sqlCommand.Append("JOIN cy_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = m.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = ?SiteID ");
            sqlCommand.Append("AND b.IncludeInFeed = 1 ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= ?CurrentDateTime  ");
            sqlCommand.Append("AND b.ItemUrl <> ''  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?CurrentDateTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentUtcDateTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetDrafts(
            int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Blogs ");

            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ");
            sqlCommand.Append("AND ((StartDate > ?BeginDate) OR (IsPublished = 0))  ");

            sqlCommand.Append("ORDER BY  StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }





        public static IDataReader GetBlogsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ce.*, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	cy_Blogs ce ");

            sqlCommand.Append("JOIN	cy_Modules m ");
            sqlCommand.Append("ON ce.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	cy_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	cy_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	cy_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = ?SiteID ");
            sqlCommand.Append("AND pm.PageID = ?PageID ");

            
            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("EntryCount, ");
            sqlCommand.Append("CommentCount ");

            sqlCommand.Append("FROM	cy_BlogStats ");

            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("MONTH(StartDate) AS Month, ");
            sqlCommand.Append("DATE_FORMAT(StartDate, '%M') AS MonthName, ");
            //sqlCommand.Append("DATE_FORMAT(StartDate, '%Y') AS Year, ");
            sqlCommand.Append("YEAR(StartDate) AS Year, ");
            sqlCommand.Append("1 AS Day, ");
            sqlCommand.Append("count(*) AS Count ");

            sqlCommand.Append("FROM	cy_Blogs ");

            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= ?CurrentDate  ");
            sqlCommand.Append("GROUP BY DATE_FORMAT(StartDate, '%Y'),  ");
            sqlCommand.Append("MONTH(StartDate),  ");
            sqlCommand.Append("DATE_FORMAT(StartDate, '%M')  ");

            sqlCommand.Append("ORDER BY 	YEAR(StartDate) desc, MONTH(StartDate)  desc ;");



            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?CurrentDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");

            sqlCommand.Append("FROM	cy_Blogs ");

            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= ?CurrentTime ");

            sqlCommand.Append("AND DATE_FORMAT(StartDate, '%Y') = ?Year  ");
            sqlCommand.Append(" AND MONTH(StartDate)  = ?Month  ");

            sqlCommand.Append("ORDER BY StartDate DESC ;");


            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?Year", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new MySqlParameter("?Month", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month;

            arParams[3] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");

            sqlCommand.Append("(SELECT b2.ItemUrl FROM cy_Blogs b2 WHERE b2.IsPublished = 1 AND b2.StartDate <= ?CurrentTime AND (b2.StartDate > b.StartDate) AND b2.ModuleID = b.ModuleID AND b2.ItemUrl IS NOT NULL AND (b2.ItemUrl <> '') ORDER BY b2.StartDate LIMIT 1 ) AS NextPost, ");
            sqlCommand.Append("(SELECT b4.Title FROM cy_Blogs b4 WHERE b4.IsPublished = 1 AND b4.StartDate <= ?CurrentTime AND (b4.StartDate > b.StartDate) AND b4.ModuleID = b.ModuleID AND b4.ItemUrl IS NOT NULL AND (b4.ItemUrl <> '') ORDER BY b4.StartDate LIMIT 1 ) AS NextPostTitle, ");

            sqlCommand.Append(" (SELECT b3.ItemUrl FROM cy_Blogs b3 WHERE b3.IsPublished = 1 AND b3.StartDate <= ?CurrentTime AND (b3.StartDate < b.StartDate) AND b3.ModuleID = b.ModuleID AND b3.ItemUrl IS NOT NULL AND (b3.ItemUrl <> '') ORDER BY b3.StartDate DESC LIMIT 1 ) AS PreviousPost, ");
            sqlCommand.Append(" (SELECT b5.Title FROM cy_Blogs b5 WHERE b5.IsPublished = 1 AND b5.StartDate <= ?CurrentTime AND (b5.StartDate < b.StartDate) AND b5.ModuleID = b.ModuleID AND b5.ItemUrl IS NOT NULL AND (b5.ItemUrl <> '') ORDER BY b5.StartDate DESC LIMIT 1 ) AS PreviousPostTitle,  ");

            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	cy_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ItemID = ?ItemID ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



        public static bool DeleteBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Blogs ");
            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = ?ModuleID ) ");
            sqlCommand.Append(";");

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = ?ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = ?ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = ?ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogStats ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogComments ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Blogs ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ) ");
            sqlCommand.Append(";");

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT ModuleGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogStats ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogComments ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Blogs ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

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

            string inNews;
            if (isInNewsletter)
            {
                inNews = "1";
            }
            else
            {
                inNews = "0";
            }

            string inFeed;
            if (includeInFeed)
            {
                inFeed = "1";
            }
            else
            {
                inFeed = "0";
            }

            string isPub;
            if (isPublished)
            {
                isPub = "1";
            }
            else
            {
                isPub = "0";
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_Blogs (  ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedByUser, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("Heading, ");
            sqlCommand.Append("Abstract, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("StartDate, ");
            sqlCommand.Append("AllowCommentsForDays, ");

            sqlCommand.Append("BlogGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("ItemUrl, ");
            sqlCommand.Append("MetaKeywords, ");
            sqlCommand.Append("MetaDescription, ");
            sqlCommand.Append("CompiledMeta, ");
            sqlCommand.Append("IsInNewsletter, ");
            sqlCommand.Append("IsPublished, ");
            sqlCommand.Append("IncludeInFeed ");
            sqlCommand.Append(" )");


            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" ?ModuleID , ");
            sqlCommand.Append(" ?UserName  , ");
            sqlCommand.Append(" ?CreatedDate, ");
            sqlCommand.Append(" ?Heading , ");
            sqlCommand.Append(" ?Abstract , ");
            sqlCommand.Append(" ?Description  , ");
            sqlCommand.Append(" ?StartDate , ");
            sqlCommand.Append(" ?AllowCommentsForDays , ");
            sqlCommand.Append(" ?BlogGuid, ");
            sqlCommand.Append(" ?ModuleGuid, ");
            sqlCommand.Append(" ?Location, ");
            sqlCommand.Append(" ?UserGuid, ");
            sqlCommand.Append(" ?UserGuid, ");
            sqlCommand.Append(" ?CreatedDate, ");
            sqlCommand.Append(" ?ItemUrl, ");
            sqlCommand.Append(" ?MetaKeywords, ");
            sqlCommand.Append(" ?CompiledMeta, ");
            sqlCommand.Append(" ?MetaDescription, ");

            sqlCommand.Append(" " + inNews + ",  ");
            sqlCommand.Append(" " + isPub + ",  ");
            sqlCommand.Append(" " + inFeed + "  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[16];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?UserName", MySqlDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userName;

            arParams[2] = new MySqlParameter("?Heading", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Abstract", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = excerpt;

            arParams[4] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new MySqlParameter("?StartDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startDate;

            arParams[6] = new MySqlParameter("?AllowCommentsForDays", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowCommentsForDays;

            arParams[7] = new MySqlParameter("?BlogGuid", MySqlDbType.VarChar, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = blogGuid.ToString();

            arParams[8] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = moduleGuid.ToString();

            arParams[9] = new MySqlParameter("?Location", MySqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = location;

            arParams[10] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid.ToString();

            arParams[11] = new MySqlParameter("?CreatedDate", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = createdDate;

            arParams[12] = new MySqlParameter("?ItemUrl", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = itemUrl;

            arParams[13] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = metaKeywords;

            arParams[14] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = metaDescription;

            arParams[15] = new MySqlParameter("?CompiledMeta", MySqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = compiledMeta;

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) FROM cy_BlogStats WHERE ModuleID = ?ModuleID ;");

            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowCount = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            if (rowCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE cy_BlogStats ");
                sqlCommand.Append("SET EntryCount = EntryCount + 1 ");
                sqlCommand.Append("WHERE ModuleID = ?ModuleID ;");

                arParams = new MySqlParameter[1];

                arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                MySqlHelper.ExecuteNonQuery(
                    GetWriteConnectionString(), 
                    sqlCommand.ToString(), 
                    arParams);


            }
            else
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO cy_BlogStats(ModuleGuid, ModuleID, EntryCount, CommentCount, TrackBackCount) ");
                sqlCommand.Append("VALUES (?ModuleGuid, ?ModuleID, 1, 0, 0); ");

                arParams = new MySqlParameter[2];

                arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = moduleGuid.ToString();

                MySqlHelper.ExecuteNonQuery(
                    GetWriteConnectionString(), 
                    sqlCommand.ToString(), 
                    arParams);


            }

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
            string inNews;
            if (isInNewsletter)
            {
                inNews = "1";
            }
            else
            {
                inNews = "0";
            }

            string inFeed;
            if (includeInFeed)
            {
                inFeed = "1";
            }
            else
            {
                inFeed = "0";
            }

            string isPub;
            if (isPublished)
            {
                isPub = "1";
            }
            else
            {
                isPub = "0";
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Blogs ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("Heading = ?Heading  , ");
            sqlCommand.Append("Abstract = ?Abstract  , ");
            sqlCommand.Append("Description = ?Description , ");
            sqlCommand.Append("IsInNewsletter = " + inNews + " , ");
            sqlCommand.Append("IncludeInFeed = " + inFeed + " , ");
            sqlCommand.Append("IsPublished = " + isPub + " , ");
            sqlCommand.Append("Description = ?Description  , ");
            sqlCommand.Append("AllowCommentsForDays = ?AllowCommentsForDays, ");
            sqlCommand.Append("Location = ?Location, ");
            sqlCommand.Append("MetaKeywords = ?MetaKeywords, ");
            sqlCommand.Append("MetaDescription = ?MetaDescription, ");
            sqlCommand.Append("CompiledMeta = ?CompiledMeta, ");

            sqlCommand.Append("ItemUrl = ?ItemUrl, ");
            sqlCommand.Append("LastModUserGuid = ?LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc, ");

            sqlCommand.Append("StartDate = ?StartDate   ");


            sqlCommand.Append("WHERE ItemID = " + itemId.ToString() + " ;");

            MySqlParameter[] arParams = new MySqlParameter[14];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?UserName", MySqlDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userName;

            arParams[2] = new MySqlParameter("?Heading", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Abstract", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = excerpt;

            arParams[4] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new MySqlParameter("?StartDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startDate;

            arParams[6] = new MySqlParameter("?AllowCommentsForDays", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowCommentsForDays;

            arParams[7] = new MySqlParameter("?Location", MySqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = location;

            arParams[8] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUserGuid.ToString();

            arParams[9] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUtc;

            arParams[10] = new MySqlParameter("?ItemUrl", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemUrl;

            arParams[11] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = metaKeywords;

            arParams[12] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = metaDescription;

            arParams[13] = new MySqlParameter("?CompiledMeta", MySqlDbType.Text);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = compiledMeta;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_BlogComments (ModuleID, ItemID, Name, Title, URL, Comment, DateCreated)");
            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" ?ModuleID , ");
            sqlCommand.Append(" ?ItemID  , ");
            sqlCommand.Append(" ?Name , ");
            sqlCommand.Append(" ?Title , ");
            sqlCommand.Append(" ?URL , ");
            sqlCommand.Append(" ?Comment  , ");
            sqlCommand.Append(" ?DateCreated ");

            sqlCommand.Append(");    ");

            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            arParams[2] = new MySqlParameter("?Name", MySqlDbType.VarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = name;

            arParams[3] = new MySqlParameter("?Title", MySqlDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new MySqlParameter("?URL", MySqlDbType.VarChar, 200);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new MySqlParameter("?Comment", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = comment;

            arParams[6] = new MySqlParameter("?DateCreated", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update cy_Blogs ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID AND ItemID = ?ItemID ;");

            arParams = new MySqlParameter[2];
            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update cy_BlogStats ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ;");

            arParams = new MySqlParameter[1];
            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteAllCommentsForBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM	cy_BlogComments ");

            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }

        public static bool UpdateCommentStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_BlogStats ");
            sqlCommand.Append("SET 	CommentCount = (SELECT COUNT(*) FROM cy_BlogComments WHERE ModuleID = ?ModuleID) ");

            sqlCommand.Append("WHERE ModuleID = ?ModuleID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }

        public static bool UpdateEntryStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_BlogStats ");
            sqlCommand.Append("SET 	EntryCount = (SELECT COUNT(*) FROM cy_Blogs WHERE ModuleID = ?ModuleID) ");

            sqlCommand.Append("WHERE ModuleID = ?ModuleID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }


        public static bool DeleteBlogComment(int blogCommentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ModuleID, ItemID ");
            sqlCommand.Append("FROM	cy_BlogComments ");

            sqlCommand.Append("WHERE BlogCommentID = ?BlogCommentID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?BlogCommentID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int moduleId = 0;
            int itemId = 0;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    moduleId = (int)reader["ModuleID"];
                    itemId = (int)reader["ItemID"];
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogComments ");
            sqlCommand.Append("WHERE BlogCommentID = ?BlogCommentID ;");

            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?BlogCommentID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            if (moduleId > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE cy_Blogs ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = ?ModuleID AND ItemID = ?ItemID ;");

                sqlCommand.Append("UPDATE cy_BlogStats ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = ?ModuleID  ;");

                arParams = new MySqlParameter[2];

                arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = itemId;

                MySqlHelper.ExecuteNonQuery(
                    GetWriteConnectionString(),
                    sqlCommand.ToString(),
                    arParams);


                return (rowsAffected > 0);


            }

            return (rowsAffected > 0);

        }


        public static IDataReader GetBlogComments(int moduleId, int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_BlogComments ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID AND ItemID = ?ItemID  ");
            sqlCommand.Append("ORDER BY BlogCommentID,  DateCreated DESC  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        public static int AddBlogCategory(
            int moduleId,
            string category)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_BlogCategories (ModuleID, Category)");
            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" ?ModuleID , ");
            sqlCommand.Append(" ?Category   ");

            sqlCommand.Append(");    ");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");


            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?Category", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }

        public static bool UpdateBlogCategory(
            int categoryId,
            string category)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_BlogCategories ");
            sqlCommand.Append(" SET  ");
            sqlCommand.Append("Category =  ?Category   ");

            sqlCommand.Append("WHERE CategoryID = ?CategoryID ;    ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            arParams[1] = new MySqlParameter("?Category", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE CategoryID = ?CategoryID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;



            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogCategories ");
            sqlCommand.Append("WHERE CategoryID = ?CategoryID ;");

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        public static IDataReader GetCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_BlogCategories ");
            sqlCommand.Append("WHERE CategoryID = ?CategoryID ;  ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategories(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bc.CategoryID, bc.Category, COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM	cy_BlogCategories bc ");
            sqlCommand.Append("JOIN	cy_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("JOIN	cy_Blogs b ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = ?ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= ?CurrentDate ");
            sqlCommand.Append("GROUP BY bc.CategoryID, bc.Category; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?CurrentDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bc.CategoryID, bc.Category, COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM	cy_BlogCategories bc ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bc.ModuleID = ?ModuleID  ");
            sqlCommand.Append("GROUP BY bc.CategoryID, bc.Category; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static int AddBlogItemCategory(
            int itemId,
            int categoryId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_BlogItemCategories (ItemID, CategoryID)");
            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" ?ItemID , ");
            sqlCommand.Append(" ?CategoryID   ");

            sqlCommand.Append(");    ");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");


            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public static bool DeleteItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetBlogItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bic.ItemID, ");
            sqlCommand.Append("bic.CategoryID, ");
            sqlCommand.Append("bc.Category ");
            sqlCommand.Append("FROM	cy_BlogItemCategories bic ");
            sqlCommand.Append("JOIN	cy_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bic.ItemID = ?ItemID   ");
            sqlCommand.Append("ORDER BY bc.Category ;  ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetEntriesByCategory(int moduleId, int categoryId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.* ");

            sqlCommand.Append("FROM	cy_Blogs b ");
            sqlCommand.Append("JOIN	cy_BlogItemCategories bic ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");
            sqlCommand.Append("WHERE b.ModuleID = ?ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= ?CurrentTime ");
            sqlCommand.Append("AND  bic.CategoryID = ?CategoryID   ");
            sqlCommand.Append("ORDER BY b.StartDate DESC ;  ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }




    }
}
