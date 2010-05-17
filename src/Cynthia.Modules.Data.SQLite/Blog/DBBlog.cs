

using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;
using System.Collections.Generic;

namespace Cynthia.Data
{
    // <summary>
    /// 
    /// </summary>
    public static class DBBlog
    {
        
        public static String DBPlatform()
        {
            return "SQLite";
        }

        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/Cynthia.db.config");

            }
            return connectionString;
        }

		public static IDataReader GetBlogs(int moduleId,DateTime beginDate,DateTime currentTime, params int[] categoryIds)
		{
			var categorySpecified = categoryIds != null && categoryIds.Length > 0;
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT SettingValue ");
			sqlCommand.Append("FROM cy_ModuleSettings ");
			sqlCommand.Append("WHERE SettingName = 'BlogEntriesToShowSetting' ");
			sqlCommand.Append("AND ModuleID = :ModuleID ;");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			int rowsToShow = int.Parse(ConfigurationManager.AppSettings["DefaultBlogPageSize"]);

			using (IDataReader reader = SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{
				if (reader.Read())
				{
					if (reader["SettingValue"] != DBNull.Value)
					{
						try
						{
							rowsToShow = Convert.ToInt32(reader["SettingValue"]);
						}
						catch (ArgumentException) { }
						catch (FormatException) { }
						catch (OverflowException) { }

					}

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

			if (categorySpecified) {
				sqlCommand.Append("LEFT OUTER JOIN cy_BlogItemCategories c ");
				sqlCommand.Append("ON b.ItemID=c.ItemID ");
			}

			sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
			sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
			sqlCommand.Append("AND b.IsPublished = 1 ");
			sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");

			if (categorySpecified) {
				sqlCommand.Append("AND c.CategoryID IN(:CategoryIDs) ");
			}

			sqlCommand.Append("ORDER BY b.StartDate DESC  ");
			sqlCommand.Append(String.Format("LIMIT {0};", rowsToShow));

			arParams =categorySpecified? new SqliteParameter[4]:new SqliteParameter[3];

			arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = beginDate;

			arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = currentTime;

			if (categorySpecified) {
				arParams[3] = new SqliteParameter(":CategoryIDs");
				var cIds=new List<string>();
				categoryIds.ToList().ForEach(x=>{
					cIds.Add(x.ToString());
				});
				arParams[3].Value = string.Join(",",cIds.ToArray() );
			}

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= StartDate  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= :CurrentTime  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
               GetConnectionString(),
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

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(", :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("b.ItemUrl AS ItemUrl, ");
            sqlCommand.Append("b.LastModUtc AS LastModUtc ");

            sqlCommand.Append("FROM	cy_Blogs b ");

            sqlCommand.Append("JOIN cy_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = m.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = :SiteID ");
            sqlCommand.Append("AND b.IncludeInFeed = 1 ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentDateTime  ");
            sqlCommand.Append("AND b.ItemUrl <> ''  ");
            sqlCommand.Append("ORDER BY b.StartDate DESC  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CurrentDateTime", DbType.DateTime);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = currentUtcDateTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetDrafts(
            int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	cy_Blogs ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND ((StartDate > :BeginDate) OR (IsPublished = 0))  ");

            sqlCommand.Append("ORDER BY StartDate DESC  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("p.SiteID = :SiteID ");
            sqlCommand.Append("AND pm.PageID = :PageID ");
            //sqlCommand.Append("AND pm.PublishBeginDate < datetime('now','localtime') ");
            //sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > datetime('now','localtime'))  ;"); 
            sqlCommand.Append(" ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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

            sqlCommand.Append("WHERE ModuleID = :ModuleID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("strftime('%m', StartDate) AS Month, ");
            sqlCommand.Append("strftime('%m', StartDate) AS MonthName, ");
            sqlCommand.Append("strftime('%Y', StartDate) AS Year, ");
            sqlCommand.Append("1 AS Day, ");
            sqlCommand.Append("count(*) AS Count ");

            sqlCommand.Append("FROM	cy_Blogs ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= :CurrentDate  ");
            sqlCommand.Append("GROUP BY strftime('%Y', StartDate),  ");
            sqlCommand.Append("strftime('%m', StartDate),  ");
            sqlCommand.Append("strftime('%m', StartDate)  ");

            sqlCommand.Append("ORDER BY strftime('%Y', StartDate) desc, strftime('%m', StartDate)  desc ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");

            sqlCommand.Append("FROM	cy_Blogs ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= :CurrentTime ");

            sqlCommand.Append("AND strftime('%Y', StartDate) = :Year  ");
            sqlCommand.Append(" AND strftime('%m', StartDate)  = :Month  ");

            sqlCommand.Append("ORDER BY StartDate DESC ;");


            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Year", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year.ToString(CultureInfo.InvariantCulture);

            arParams[2] = new SqliteParameter(":Month", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month.ToString("00",CultureInfo.InvariantCulture);

            arParams[3] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");

            sqlCommand.Append("(SELECT b2.ItemUrl FROM cy_Blogs b2 WHERE b2.IsPublished = 1 AND b2.StartDate <= :CurrentTime AND b2.StartDate > b.StartDate AND b2.ModuleID = b.ModuleID AND b2.ItemUrl IS NOT NULL AND b2.ItemUrl <> '' ORDER BY b2.StartDate LIMIT 1 ) AS NextPost, ");
            sqlCommand.Append("(SELECT b4.Title FROM cy_Blogs b4 WHERE b4.IsPublished = 1 AND b4.StartDate <= :CurrentTime AND b4.StartDate > b.StartDate AND b4.ModuleID = b.ModuleID AND b4.ItemUrl IS NOT NULL AND b4.ItemUrl <> '' ORDER BY b4.StartDate LIMIT 1 ) AS NextPostTitle, ");

            sqlCommand.Append(" (SELECT b3.ItemUrl FROM cy_Blogs b3 WHERE b3.IsPublished = 1 AND b3.StartDate <= :CurrentTime AND b3.StartDate < b.StartDate AND b3.ModuleID = b.ModuleID AND b3.ItemUrl IS NOT NULL AND b3.ItemUrl <> '' ORDER BY b3.StartDate DESC LIMIT 1 ) AS PreviousPost, ");
            sqlCommand.Append(" (SELECT b5.Title FROM cy_Blogs b5 WHERE b5.IsPublished = 1 AND b5.StartDate <= :CurrentTime AND b5.StartDate < b.StartDate AND b5.ModuleID = b.ModuleID AND b5.ItemUrl IS NOT NULL AND b5.ItemUrl <> '' ORDER BY b5.StartDate DESC LIMIT 1 ) AS PreviousPostTitle,  ");
            
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	cy_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ItemID = :ItemID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



        public static bool DeleteBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Blogs ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
          
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID  = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogStats ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogComments ");
            sqlCommand.Append("WHERE ModuleID  = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Blogs ");
            sqlCommand.Append("WHERE ModuleID  = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT ModuleGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM cy_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogStats ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogComments ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Blogs ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            #region Bit Conversion

            int intIsInNewsletter;
            if (isInNewsletter)
            {
                intIsInNewsletter = 1;
            }
            else
            {
                intIsInNewsletter = 0;
            }

            int intIncludeInFeed;
            if (includeInFeed)
            {
                intIncludeInFeed = 1;
            }
            else
            {
                intIncludeInFeed = 0;
            }

            int intIsPublished = 0;
            if (isPublished) { intIsPublished = 1; }


            #endregion


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_Blogs (");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("Heading, ");
            sqlCommand.Append("Abstract, ");
            sqlCommand.Append("Excerpt, ");
            sqlCommand.Append("StartDate, ");
            sqlCommand.Append("IsInNewsletter, ");
            sqlCommand.Append("IsPublished, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("CommentCount, ");
            sqlCommand.Append("TrackBackCount, ");
            sqlCommand.Append("IncludeInFeed, ");
            sqlCommand.Append("AllowCommentsForDays, ");
            sqlCommand.Append("BlogGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("MetaKeywords, ");
            sqlCommand.Append("MetaDescription, ");
            sqlCommand.Append("CompiledMeta, ");
            sqlCommand.Append("ItemUrl, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":CreatedDate, ");
            sqlCommand.Append(":Heading, ");
            sqlCommand.Append(":Abstract, ");
            sqlCommand.Append("'', ");
            sqlCommand.Append(":StartDate, ");
            sqlCommand.Append(":IsInNewsletter, ");
            sqlCommand.Append(":IsPublished, ");
            sqlCommand.Append(":Description, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append(":IncludeInFeed, ");
            sqlCommand.Append(":AllowCommentsForDays, ");
            sqlCommand.Append(":BlogGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":Location, ");
            sqlCommand.Append(":MetaKeywords, ");
            sqlCommand.Append(":MetaDescription, ");
            sqlCommand.Append(":CompiledMeta, ");
            sqlCommand.Append(":ItemUrl, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":CreatedDate )");
            sqlCommand.Append(";");


            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[18];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CreatedDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdDate;

            arParams[2] = new SqliteParameter(":Heading", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Abstract", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = excerpt;

            arParams[4] = new SqliteParameter(":StartDate", DbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startDate;

            arParams[5] = new SqliteParameter(":IsInNewsletter", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsInNewsletter;

            arParams[6] = new SqliteParameter(":Description", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new SqliteParameter(":IncludeInFeed", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intIncludeInFeed;

            arParams[8] = new SqliteParameter(":AllowCommentsForDays", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowCommentsForDays;

            arParams[9] = new SqliteParameter(":BlogGuid", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = blogGuid.ToString();

            arParams[10] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid.ToString();

            arParams[11] = new SqliteParameter(":Location", DbType.Object);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = location;

            arParams[12] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid.ToString();

            arParams[13] = new SqliteParameter(":ItemUrl", DbType.String, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = itemUrl;

            arParams[14] = new SqliteParameter(":MetaKeywords", DbType.String, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = metaKeywords;

            arParams[15] = new SqliteParameter(":MetaDescription", DbType.String, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = metaDescription;

            arParams[16] = new SqliteParameter(":CompiledMeta", DbType.Object);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = compiledMeta;

            arParams[17] = new SqliteParameter(":IsPublished", DbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intIsPublished;


            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) FROM cy_BlogStats WHERE ModuleID = :ModuleID ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowCount = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            if (rowCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE cy_BlogStats ");
                sqlCommand.Append("SET EntryCount = EntryCount + 1 ");
                sqlCommand.Append("WHERE ModuleID = :ModuleID ;");

                arParams = new SqliteParameter[1];

                arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);


            }
            else
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO cy_BlogStats(ModuleGuid, ModuleID, EntryCount, CommentCount, TrackBackCount) ");
                sqlCommand.Append("VALUES (:ModuleGuid, :ModuleID, 1, 0, 0); ");


                arParams = new SqliteParameter[2];

                arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = moduleGuid.ToString();

                SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
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
            sqlCommand.Append("SET CreatedByUser = :UserName  , ");
            sqlCommand.Append("CreatedDate = datetime('now','localtime') , ");
            sqlCommand.Append("Heading = :Heading  , ");
            sqlCommand.Append("Abstract = :Abstract  , ");
            sqlCommand.Append("ItemUrl = :ItemUrl  , ");
            sqlCommand.Append("Description = :Description , ");
            sqlCommand.Append("IsInNewsletter = " + inNews + " , ");
            sqlCommand.Append("IncludeInFeed = " + inFeed + " , ");
            sqlCommand.Append("IsPublished = " + isPub + " , ");
            sqlCommand.Append("Description = :Description  , ");
            sqlCommand.Append("AllowCommentsForDays = :AllowCommentsForDays  , ");
            sqlCommand.Append("StartDate = :StartDate,   ");
            sqlCommand.Append("Location = :Location, ");
            sqlCommand.Append("MetaKeywords = :MetaKeywords, ");
            sqlCommand.Append("MetaDescription = :MetaDescription, ");
            sqlCommand.Append("CompiledMeta = :CompiledMeta, ");
            sqlCommand.Append("LastModUserGuid = :LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = :LastModUtc ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[14];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":UserName", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userName;

            arParams[2] = new SqliteParameter(":Heading", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Abstract", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = excerpt;

            arParams[4] = new SqliteParameter(":Description", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new SqliteParameter(":StartDate", DbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startDate;

            arParams[6] = new SqliteParameter(":AllowCommentsForDays", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowCommentsForDays;

            arParams[7] = new SqliteParameter(":Location", DbType.Object);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = location;

            arParams[8] = new SqliteParameter(":LastModUserGuid", DbType.String, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUserGuid.ToString();

            arParams[9] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUtc;

            arParams[10] = new SqliteParameter(":ItemUrl", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemUrl;

            arParams[11] = new SqliteParameter(":MetaKeywords", DbType.String, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = metaKeywords;

            arParams[12] = new SqliteParameter(":MetaDescription", DbType.String, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = metaDescription;

            arParams[13] = new SqliteParameter(":CompiledMeta", DbType.Object);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = compiledMeta;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :ItemID  , ");
            sqlCommand.Append(" :Name , ");
            sqlCommand.Append(" :Title , ");
            sqlCommand.Append(" :URL , ");
            sqlCommand.Append(" :Comment  , ");
            sqlCommand.Append(" :DateCreated ");

            sqlCommand.Append(");    ");

            SqliteParameter[] arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            arParams[2] = new SqliteParameter(":Name", DbType.String, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = name;

            arParams[3] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new SqliteParameter(":URL", DbType.String, 200);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new SqliteParameter(":Comment", DbType.Object);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = comment;

            arParams[6] = new SqliteParameter(":DateCreated", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update cy_Blogs ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID AND ItemID = :ItemID ;");

            arParams = new SqliteParameter[2];
            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update cy_BlogStats ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ;");

            arParams = new SqliteParameter[1];
            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static bool DeleteAllCommentsForBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM	cy_BlogComments ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateCommentStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_BlogStats ");
            sqlCommand.Append("SET 	CommentCount = (SELECT COUNT(*) FROM cy_BlogComments WHERE ModuleID = :ModuleID) ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateEntryStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_BlogStats ");
            sqlCommand.Append("SET 	EntryCount = (SELECT COUNT(*) FROM cy_Blogs WHERE ModuleID = :ModuleID) ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static bool DeleteBlogComment(int blogCommentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ModuleID, ItemID ");
            sqlCommand.Append("FROM	cy_BlogComments ");

            sqlCommand.Append("WHERE BlogCommentID = :BlogCommentID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":BlogCommentID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int moduleId = 0;
            int itemId = 0;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE BlogCommentID = :BlogCommentID ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":BlogCommentID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int rowsAffected = 0;

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            if (moduleId > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE cy_Blogs ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = :ModuleID AND ItemID = :ItemID ;");

                sqlCommand.Append("UPDATE cy_BlogStats ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = :ModuleID  ;");

                arParams = new SqliteParameter[2];

                arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = itemId;

                SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID AND ItemID = :ItemID  ");
            sqlCommand.Append("ORDER BY BlogCommentID,  DateCreated DESC  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :Category   ");

            sqlCommand.Append(");    ");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Category", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }
        public static int AddBlogCategory(int moduleId,string category,int siteId,Guid siteGuid,Guid pageGuid,int pageId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_BlogCategories (ModuleID, Category)");
            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :Category   ");

            sqlCommand.Append(");    ");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Category", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int newID = 0;
            
            using (var connection=new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                using(var trans=connection.BeginTransaction())
                {
                    try
                    {
                        //add to cy_BlogCategories table
                        newID = Convert.ToInt32(connection.ExecuteScalar(sqlCommand.ToString(), arParams).ToString());
                        //add blog category rss url
                        var friendlyUrl = string.Format("blog{0}rss{1}.aspx", moduleId,newID);
                        var realUrl = string.Format("~/Blog/RSS.aspx?pageid={0}&mid={1}&cid={2}", pageId,
                                                    moduleId,newID);
                        DBFriendlyUrl.AddFriendlyUrl(Guid.NewGuid(), siteGuid, pageGuid, siteId, friendlyUrl, realUrl,
                                                     false, connection);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return newID;

        }

        public static bool UpdateBlogCategory(
            int categoryId,
            string category)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_BlogCategories ");
            sqlCommand.Append(" SET  ");
            sqlCommand.Append("Category =  :Category   ");

            sqlCommand.Append("WHERE CategoryID = :CategoryID;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            arParams[1] = new SqliteParameter(":Category", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int rowsAffected = 0;

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE CategoryID = :CategoryID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            int rowsAffected = 0;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogCategories ");
            sqlCommand.Append("WHERE CategoryID = :CategoryID ;");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        public static IDataReader GetCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_BlogCategories ");
            sqlCommand.Append("WHERE CategoryID = :CategoryID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategories(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bc.CategoryID As CategoryID, ");
            sqlCommand.Append("bc.Category As Category, ");
            sqlCommand.Append("COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM cy_BlogCategories bc ");
            sqlCommand.Append("JOIN	cy_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("JOIN	cy_Blogs b ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");

            sqlCommand.Append("WHERE bc.ModuleID = :ModuleID ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentDate ");
            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append(" bc.CategoryID, bc.Category ");
            sqlCommand.Append(" ; ");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("bc.CategoryID As CategoryID, ");
            sqlCommand.Append("bc.Category As Category, ");
            sqlCommand.Append("COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM cy_BlogCategories bc ");
            sqlCommand.Append("LEFT OUTER JOIN cy_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bc.ModuleID = :ModuleID  ");
            sqlCommand.Append("GROUP BY bc.CategoryID, bc.Category;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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

            sqlCommand.Append(" :ItemID , ");
            sqlCommand.Append(" :CategoryID   ");

            sqlCommand.Append(");    ");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public static bool DeleteItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = 0;

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetBlogItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bic.ItemID As ItemID, ");
            sqlCommand.Append("bic.CategoryID As CategoryID, ");
            sqlCommand.Append("bc.Category As Category ");
            sqlCommand.Append("FROM	cy_BlogItemCategories bic ");
            sqlCommand.Append("JOIN	cy_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bic.ItemID = :ItemID   ");
            sqlCommand.Append("ORDER BY bc.Category;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE b.ModuleID = :ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime ");
            sqlCommand.Append("AND  bic.CategoryID = :CategoryID   ");
            sqlCommand.Append("ORDER BY b.StartDate DESC;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }




    }
}
