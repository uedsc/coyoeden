/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2009-12-12
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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cynthia.Data
{
    public static class DBForums
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



        public static int Create(
            int moduleId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int threadsPerPage,
            bool allowAnonymousPosts)
        {
            byte moderated = 1;
            if (!isModerated)
            {
                moderated = 0;
            }

            byte active = 1;
            if (!isActive)
            {
                active = 0;
            }

            byte allowAnonymous = 1;
            if (!allowAnonymousPosts)
            {
                allowAnonymous = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO	cy_Forums ( ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description , ");
            sqlCommand.Append("IsModerated , ");
            sqlCommand.Append("IsActive , ");
            sqlCommand.Append("SortOrder , ");
            sqlCommand.Append("PostsPerPage , ");
            sqlCommand.Append("ThreadsPerPage , ");
            sqlCommand.Append("AllowAnonymousPosts  ");
            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");

            sqlCommand.Append(" ?ModuleID , ");
            sqlCommand.Append(" ?UserID  , ");
            sqlCommand.Append(" now(), ");
            sqlCommand.Append(" ?Title , ");
            sqlCommand.Append(" ?Description , ");
            sqlCommand.Append(" ?IsModerated , ");
            sqlCommand.Append(" ?IsActive , ");
            sqlCommand.Append(" ?SortOrder , ");
            sqlCommand.Append(" ?PostsPerPage , ");
            sqlCommand.Append(" ?ThreadsPerPage , ");
            sqlCommand.Append(" ?AllowAnonymousPosts  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[10];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?IsModerated", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moderated;

            arParams[5] = new MySqlParameter("?IsActive", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = active;

            arParams[6] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sortOrder;

            arParams[7] = new MySqlParameter("?PostsPerPage", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = postsPerPage;

            arParams[8] = new MySqlParameter("?ThreadsPerPage", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = threadsPerPage;

            arParams[9] = new MySqlParameter("?AllowAnonymousPosts", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowAnonymous;

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }



        public static bool Update(
            int itemId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int threadsPerPage,
            bool allowAnonymousPosts)
        {
            byte moderated = 1;
            if (!isModerated)
            {
                moderated = 0;
            }

            byte active = 1;
            if (!isActive)
            {
                active = 0;
            }

            byte allowAnonymous = 1;
            if (!allowAnonymousPosts)
            {
                allowAnonymous = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE		cy_Forums ");
            sqlCommand.Append("SET	Title = ?Title, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("IsModerated = ?IsModerated, ");
            sqlCommand.Append("IsActive = ?IsActive, ");
            sqlCommand.Append("SortOrder = ?SortOrder, ");
            sqlCommand.Append("PostsPerPage = ?PostsPerPage, ");
            sqlCommand.Append("ThreadsPerPage = ?ThreadsPerPage, ");
            sqlCommand.Append("AllowAnonymousPosts = ?AllowAnonymousPosts ");

            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[9];


            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?IsModerated", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moderated;

            arParams[4] = new MySqlParameter("?IsActive", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = active;

            arParams[5] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new MySqlParameter("?PostsPerPage", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postsPerPage;

            arParams[7] = new MySqlParameter("?ThreadsPerPage", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = threadsPerPage;

            arParams[8] = new MySqlParameter("?AllowAnonymousPosts", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowAnonymous;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Forums ");
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID = ?ModuleID) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID = ?ModuleID) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID = ?ModuleID);");
            sqlCommand.Append("DELETE FROM cy_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID = ?ModuleID) ;");
            sqlCommand.Append("DELETE FROM cy_Forums WHERE ModuleID = ?ModuleID;");

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

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID)) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID)) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID));");
            sqlCommand.Append("DELETE FROM cy_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM cy_Forums WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID)) ;");
            sqlCommand.Append("DELETE FROM cy_Forums WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = ?SiteID);");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        public static IDataReader GetForums(int moduleId, int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As MostRecentPostUser, ");
            sqlCommand.Append("s.SubscribeDate IS NOT NULL AND s.UnSubscribeDate IS NULL As Subscribed, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM cy_ForumSubscriptions fs WHERE fs.ForumID = f.ItemID AND fs.UnSubscribeDate IS NULL) As SubscriberCount  ");

            sqlCommand.Append("FROM	cy_Forums f ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON f.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_ForumSubscriptions s ");
            sqlCommand.Append("ON f.ItemID = s.ForumID AND s.UserID = ?UserID ");

            sqlCommand.Append("WHERE f.ModuleID	= ?ModuleID ");
            sqlCommand.Append("AND f.IsActive = 1 ");

            sqlCommand.Append("ORDER BY		f.SortOrder, f.ItemID ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetForum(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As CreatedByUser, ");
            sqlCommand.Append("up.Name As MostRecentPostUser ");
            sqlCommand.Append("FROM	cy_Forums f ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON f.CreatedBy = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON f.MostRecentPostUserID = up.UserID ");
            sqlCommand.Append("WHERE f.ItemID	= ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool IncrementPostCount(
            int forumId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Forums ");
            sqlCommand.Append("SET MostRecentPostDate = ?MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID = ?MostRecentPostUserID, ");
            sqlCommand.Append("PostCount = PostCount + 1 ");

            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateUserStats(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TotalPosts = (SELECT COUNT(*) FROM cy_ForumPosts WHERE cy_ForumPosts.UserID = cy_Users.UserID) ");
            if (userId > -1)
            {
                sqlCommand.Append("WHERE UserID = ?UserID ");
            }
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool IncrementPostCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Forums ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PostCount = PostCount + 1 ");
            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementPostCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Forums ");
            sqlCommand.Append("SET PostCount = PostCount - 1 ");

            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool RecalculatePostStats(int forumId)
        {
            DateTime mostRecentPostDate = DateTime.Now;
            int mostRecentPostUserID = 0;
            int postCount = 0;

            StringBuilder sqlCommand = new StringBuilder();
            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID ");
            sqlCommand.Append("FROM cy_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = ?ForumID ");
            sqlCommand.Append("ORDER BY MostRecentPostDate DESC ");
            sqlCommand.Append("LIMIT 1 ;");

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
                    mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("SUM(TotalReplies) + COUNT(*) As PostCount ");
            sqlCommand.Append("FROM cy_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = ?ForumID ;");

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    postCount = Convert.ToInt32(reader["PostCount"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	cy_Forums ");
            sqlCommand.Append("SET	 ");
            sqlCommand.Append("MostRecentPostDate = ?MostRecentPostDate,	 ");
            sqlCommand.Append("MostRecentPostUserID = ?MostRecentPostUserID,	 ");
            sqlCommand.Append("PostCount = ?PostCount	 ");
            sqlCommand.Append("WHERE ItemID = ?ForumID ;");

            arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostDate;

            arParams[2] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostUserID;

            arParams[3] = new MySqlParameter("?PostCount", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postCount;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool IncrementThreadCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	cy_Forums ");
            sqlCommand.Append("SET	ThreadCount = ThreadCount + 1 ");
            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementThreadCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Forums ");
            sqlCommand.Append("SET ThreadCount = ThreadCount - 1 ");

            sqlCommand.Append("WHERE ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static int GetUserThreadCount(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_ForumThreads ");
            sqlCommand.Append("WHERE ThreadID IN (Select DISTINCT ThreadID FROM cy_ForumPosts WHERE cy_ForumPosts.UserID = ?UserID) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }


        public static IDataReader GetThreadPageByUser(
            int userId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetUserThreadCount(userId);

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
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("f.Title As Forum, ");
            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("(SELECT PageID FROM cy_PageModules WHERE cy_PageModules.ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > ?CurrentDate) LIMIT 1) As PageID, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");

            sqlCommand.Append("FROM	cy_ForumThreads t ");

            sqlCommand.Append("JOIN	cy_Forums f ");
            sqlCommand.Append("ON t.ForumID = f.ItemID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");

            sqlCommand.Append("WHERE t.ThreadID IN (Select DISTINCT ThreadID FROM cy_ForumPosts WHERE cy_ForumPosts.UserID = ?UserID) ");

            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize ");
            sqlCommand.Append(";");


            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?CurrentDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        //public static IDataReader GetThreadPageByUser(
        //    int userId,
        //    int forumId,
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{

        //    //int pageSize = 1;
        //    int totalRows = 0;
        //    //IDataReader reader = GetForum(forumId);
        //    //if (reader.Read())
        //    //{
        //    //    pageSize = Convert.ToInt32(reader["ThreadsPerPage"]);
        //    //    totalRows = Convert.ToInt32(reader["ThreadCount"]);
        //    //}
        //    //reader.Close();

        //    int totalPages = totalRows / pageSize;
        //    if (totalRows <= pageSize)
        //    {
        //        totalPages = 1;
        //    }
        //    else
        //    {
        //        int remainder = 0;
        //        Math.DivRem(totalRows, pageSize, out remainder);
        //        if (remainder > 0)
        //        {
        //            totalPages += 1;
        //        }
        //    }

        //    int offset = pageSize * (pageNumber - 1);


        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT	 ");
        //    sqlCommand.Append(" t.*, ");
        //    sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
        //    sqlCommand.Append("s.Name As StartedBy ");
        //    sqlCommand.Append("FROM	cy_ForumThreads t ");
        //    sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
        //    sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
        //    sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
        //    sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
        //    sqlCommand.Append("WHERE t.ForumID = ?ForumID ");

        //    sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

        //    if (pageNumber > 1)
        //    {
        //        sqlCommand.Append("LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
        //            + ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
        //    }
        //    else
        //    {
        //        sqlCommand.Append("LIMIT "
        //            + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
        //    }

        //    sqlCommand.Append(" ; ");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = forumId;

        //    return MySqlHelper.ExecuteReader(
        //        GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}



        public static IDataReader GetThreads(int forumId, int pageNumber)
        {

            int pageSize = 1;
            int totalRows = 0;
            using (IDataReader reader = GetForum(forumId))
            {
                if (reader.Read())
                {
                    pageSize = Convert.ToInt32(reader["ThreadsPerPage"]);
                    totalRows = Convert.ToInt32(reader["ThreadCount"]);
                }
            }

            int totalPages = totalRows / pageSize;
            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder = 0;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            int offset = pageSize * (pageNumber - 1);


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");
            sqlCommand.Append("FROM	cy_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("WHERE t.ForumID = ?ForumID ");

            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
                    + ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            }
            else
            {
                sqlCommand.Append("LIMIT "
                    + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            }

            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int ForumThreadGetPostCount(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            sqlCommand.Append("SELECT COUNT(*) FROM cy_ForumPosts WHERE ThreadID = ?ThreadID ");

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int GetSubscriberCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_ForumSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ForumID = ?ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UnSubscribeDate IS NULL");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }


        public static IDataReader GetSubscriberPage(
            int forumId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetSubscriberCount(forumId);

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
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("fs.SubscriptionID, ");
            sqlCommand.Append("fs.SubscribeDate, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	cy_ForumSubscriptions fs  ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("cy_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserID = fs.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fs.ForumID = ?ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.Name  ");
            

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static bool AddSubscriber(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM cy_ForumSubscriptions  ");
            sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }

            sqlCommand = new StringBuilder();


            if (subscriptionCount > 0)
            {
                sqlCommand.Append("UPDATE cy_ForumSubscriptions ");
                sqlCommand.Append("SET SubscribeDate = ?SubscribeDate, ");
                sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID ;");

            }
            else
            {

                sqlCommand.Append("INSERT INTO	cy_ForumSubscriptions ( ");
                sqlCommand.Append("ForumID, ");
                sqlCommand.Append("UserID, ");
                sqlCommand.Append("SubscribeDate");
                sqlCommand.Append(") ");
                sqlCommand.Append("VALUES ( ");
                sqlCommand.Append("?ForumID, ");
                sqlCommand.Append("?UserID, ");
                sqlCommand.Append("?SubscribeDate");
                sqlCommand.Append(") ;");

            }

            arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new MySqlParameter("?SubscribeDate", MySqlDbType.Datetime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SubscriptionID = ?SubscriptionID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SubscriptionID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subscriptionId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool Unsubscribe(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = ?UnSubscribeDate ");
            sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID ;");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new MySqlParameter("?UnSubscribeDate", MySqlDbType.Datetime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = ?UnSubscribeDate ");
            sqlCommand.Append("WHERE UserID = ?UserID ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?UnSubscribeDate", MySqlDbType.Datetime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumSubscriptionExists(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_ForumSubscriptions ");
            sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID AND UnSubscribeDate IS NULL ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool ForumThreadSubscriptionExists(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID AND UnSubscribeDate IS NULL ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader ForumThreadGetThread(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("f.PostsPerPage ");
            sqlCommand.Append("FROM	cy_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("JOIN	cy_Forums f ");
            sqlCommand.Append("ON f.ItemID = t.ForumID ");
            sqlCommand.Append("WHERE t.ThreadID = ?ThreadID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader ForumThreadGetPost(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	fp.* ");
            sqlCommand.Append("FROM	cy_ForumPosts fp ");
            sqlCommand.Append("WHERE fp.PostID = ?PostID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int ForumThreadCreate(
            int forumId,
            string threadSubject,
            int sortOrder,
            bool isLocked,
            int startedByUserId,
            DateTime threadDate)
        {

            byte locked = 1;
            if (!isLocked)
            {
                locked = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            int forumSequence = 1;
            sqlCommand.Append("SELECT COALESCE(Max(ForumSequence) + 1,1) As ForumSequence FROM cy_ForumThreads WHERE ForumID = ?ForumID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                }
            }


            sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO cy_ForumThreads ( ");
            sqlCommand.Append("ForumID, ");
            sqlCommand.Append("ThreadSubject, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("ForumSequence, ");
            sqlCommand.Append("IsLocked, ");
            sqlCommand.Append("StartedByUserID, ");
            sqlCommand.Append("ThreadDate, ");
            sqlCommand.Append("MostRecentPostUserID, ");
            sqlCommand.Append("MostRecentPostDate ");
            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" ?ForumID , ");
            sqlCommand.Append(" ?ThreadSubject  , ");
            sqlCommand.Append(" ?SortOrder, ");
            sqlCommand.Append(" ?ForumSequence, ");
            sqlCommand.Append(" ?IsLocked , ");
            sqlCommand.Append(" ?StartedByUserID , ");
            sqlCommand.Append(" ?ThreadDate , ");
            sqlCommand.Append(" ?StartedByUserID , ");
            sqlCommand.Append(" ?ThreadDate  ");
            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?ThreadSubject", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSubject;

            arParams[2] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new MySqlParameter("?IsLocked", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = locked;

            arParams[4] = new MySqlParameter("?StartedByUserID", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startedByUserId;

            arParams[5] = new MySqlParameter("?ThreadDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = threadDate;

            arParams[6] = new MySqlParameter("?ForumSequence", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = forumSequence;


            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_ForumThreadSubscriptions (ThreadID, UserID) ");
            sqlCommand.Append("    SELECT ?ThreadID as ThreadID, UserID from cy_ForumSubscriptions fs ");
            sqlCommand.Append("        WHERE fs.ForumID = ?ForumID AND fs.SubscribeDate IS NOT NULL AND fs.UnSubscribeDate IS NULL;");

            arParams = new MySqlParameter[2];
            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newID;

            arParams[1] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return newID;

        }

        public static bool ForumThreadDelete(int threadId)
        {
            ForumThreadDeletePosts(threadId);
            ForumThreadDeleteSubscriptions(threadId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumThreads ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeletePosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeleteSubscriptions(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdate(
            int threadId,
            int forumId,
            string threadSubject,
            int sortOrder,
            bool isLocked)
        {
            byte locked = 1;
            if (!isLocked)
            {
                locked = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	 cy_ForumThreads ");
            sqlCommand.Append("SET	ForumID = ?ForumID, ");
            sqlCommand.Append("ThreadSubject = ?ThreadSubject, ");
            sqlCommand.Append("SortOrder = ?SortOrder, ");
            sqlCommand.Append("IsLocked = ?IsLocked ");


            sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            arParams[2] = new MySqlParameter("?ThreadSubject", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadSubject;

            arParams[3] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new MySqlParameter("?IsLocked", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = locked;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadIncrementReplyStats(
            int threadId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ForumThreads ");
            sqlCommand.Append("SET MostRecentPostUserID = ?MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies + 1, ");
            sqlCommand.Append("MostRecentPostDate = ?MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDecrementReplyStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT UserID, PostDate ");
            sqlCommand.Append("FROM cy_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ");
            sqlCommand.Append("ORDER BY PostID DESC ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int userId = 0;
            DateTime postDate = DateTime.Now;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["UserID"]);
                    postDate = Convert.ToDateTime(reader["PostDate"]);
                }
            }

            sqlCommand = new StringBuilder();


            sqlCommand.Append("UPDATE cy_ForumThreads ");
            sqlCommand.Append("SET MostRecentPostUserID = ?MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies - 1, ");
            sqlCommand.Append("MostRecentPostDate = ?MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

            arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = postDate;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdateViewStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ForumThreads ");
            sqlCommand.Append("SET TotalViews = TotalViews + 1 ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
        {

            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int postsPerPage = 10;

            sqlCommand.Append("SELECT	f.PostsPerPage ");
            sqlCommand.Append("FROM		cy_ForumThreads ft ");
            sqlCommand.Append("JOIN		cy_Forums f ");
            sqlCommand.Append("ON		ft.ForumID = f.ItemID ");
            sqlCommand.Append("WHERE	ft.ThreadID = ?ThreadID ;");

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                }
            }
            sqlCommand = new StringBuilder();
            int currentPageMaxThreadSequence = postsPerPage * pageNumber;
            int beginSequence = 1;
            int endSequence;
            if (currentPageMaxThreadSequence > postsPerPage)
            {
                beginSequence = currentPageMaxThreadSequence - postsPerPage + 1;

            }

            //sqlCommand.Append("DECLARE @BeginSequence int; ");
            // sqlCommand.Append("DECLARE @EndSequence int; ");
            // sqlCommand.Append("SET @BeginSequence = " + beginSequence.ToString(CultureInfo.InvariantCulture) + " ;");
            //EndSequence = BeginSequence + PostsPerPage - 1;
            endSequence = beginSequence + postsPerPage;
            // sqlCommand.Append("SET @EndSequence = " + endSequence.ToString(CultureInfo.InvariantCulture) + " ;");

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
            sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");

            sqlCommand.Append("FROM	cy_ForumPosts p ");

            sqlCommand.Append("JOIN	cy_ForumThreads ft ");
            sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN cy_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");

            sqlCommand.Append("WHERE ft.ThreadID = ?ThreadID ");
            sqlCommand.Append("AND p.ThreadSequence >= ?BeginSequence ");
            sqlCommand.Append("AND p.ThreadSequence <= ?EndSequence ");
            //sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID ;");
            sqlCommand.Append("ORDER BY	p.ThreadSequence ;");

            arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?PageNumber", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new MySqlParameter("?BeginSequence", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = beginSequence;

            arParams[3] = new MySqlParameter("?EndSequence", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = endSequence;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("up.TotalPosts As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");
            sqlCommand.Append("FROM	cy_ForumPosts p ");
            sqlCommand.Append("JOIN	cy_ForumThreads ft ");
            sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN cy_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");
            sqlCommand.Append("WHERE ft.ThreadID = ?ThreadID ");

            //sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID DESC ;");
            sqlCommand.Append("ORDER BY p.PostID  ;");

            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;



            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            MySqlParameter[] arParams;

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("p.PostID, ");
            sqlCommand.Append("p.ThreadID, ");
            sqlCommand.Append("p.ThreadSequence, ");
            sqlCommand.Append("p.Subject, ");
            sqlCommand.Append("p.PostDate, ");
            sqlCommand.Append("p.Approved, ");
            sqlCommand.Append("p.UserID, ");
            sqlCommand.Append("p.SortOrder, ");
            sqlCommand.Append("p.Post, ");

            sqlCommand.Append("ft.ForumID, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");

            sqlCommand.Append("FROM	cy_ForumPosts p ");

            sqlCommand.Append("JOIN	cy_ForumThreads ft ");
            sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN cy_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");

            sqlCommand.Append("WHERE ft.ThreadID = ?ThreadID ");

            //sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID DESC ;");
            sqlCommand.Append("ORDER BY p.ThreadSequence DESC  ;");

            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  fp.*, ");

            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("f.ItemID, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	cy_ForumPosts fp ");

            sqlCommand.Append("JOIN	cy_ForumThreads ft ");
            sqlCommand.Append("ON fp.ThreadID = ft.ThreadID ");

            sqlCommand.Append("JOIN	cy_Forums f ");
            sqlCommand.Append("ON f.ItemID = ft.ForumID ");

            sqlCommand.Append("JOIN	cy_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

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

        public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT		fp.*, ");
            sqlCommand.Append("ft.ThreadSubject, ");
            sqlCommand.Append("ft.ForumID, ");
            sqlCommand.Append("COALESCE(s.Name,'Guest') as StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') as PostAuthor, ");
            sqlCommand.Append("up.TotalPosts as PostAuthorTotalPosts, ");
            sqlCommand.Append("up.AvatarUrl as PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL as PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature as PostAuthorSignature ");


            sqlCommand.Append("FROM		cy_ForumPosts fp ");

            sqlCommand.Append("JOIN		cy_ForumThreads ft ");
            sqlCommand.Append("ON		fp.ThreadID = ft.ThreadID ");

            sqlCommand.Append("JOIN		cy_Forums f ");
            sqlCommand.Append("ON		ft.ForumID = f.ItemID ");

            sqlCommand.Append("JOIN		cy_Modules m ");
            sqlCommand.Append("ON		f.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN		cy_PageModules pm ");
            sqlCommand.Append("ON		pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN		cy_Pages p ");
            sqlCommand.Append("ON		pm.PageID = p.PageID ");

            sqlCommand.Append("LEFT OUTER JOIN		cy_Users u ");
            sqlCommand.Append("ON		ft.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN		cy_Users s ");
            sqlCommand.Append("ON		ft.StartedByUserID = s.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN		cy_Users up ");
            sqlCommand.Append("ON		up.UserID = fp.UserID ");

            sqlCommand.Append("WHERE	p.SiteID = ?SiteID ");
            sqlCommand.Append("AND	(?PageID = -1 OR p.PageID = ?PageID) ");
            sqlCommand.Append("AND	(?ModuleID = -1 OR m.ModuleID = ?ModuleID) ");
            sqlCommand.Append("AND	(?ItemID = -1 OR f.ItemID = ?ItemID) ");
            sqlCommand.Append("AND	(?ThreadID = -1 OR ft.ThreadID = ?ThreadID) ");
            //sqlCommand.Append("AND	(?MaximumDays = -1 OR datediff('dd', now(), fp.PostDate) <= ?MaximumDays) ");

            sqlCommand.Append("AND	( (?MaximumDays = -1) OR  ((now() - ?MaximumDays) >= fp.PostDate )) ");


            sqlCommand.Append("ORDER BY	fp.PostDate DESC ; ");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new MySqlParameter("?ModuleID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new MySqlParameter("?ItemID", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemId;

            arParams[4] = new MySqlParameter("?ThreadID", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = threadId;

            arParams[5] = new MySqlParameter("?MaximumDays", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maximumDays;

            //String test = sqlCommand.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT u.Email ");
            sqlCommand.Append("FROM	cy_Users u ");

            sqlCommand.Append("WHERE u.UserID <> ?CurrentPostUserID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("(");

            sqlCommand.Append("(");
            sqlCommand.Append("u.UserID IN (SELECT UserID FROM cy_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID ");
            sqlCommand.Append("AND UnSubscribeDate IS NULL) ");
            sqlCommand.Append(")");

            sqlCommand.Append("OR ");

            sqlCommand.Append("(");
            sqlCommand.Append("u.UserID IN (SELECT UserID FROM cy_ForumSubscriptions ");
            sqlCommand.Append("WHERE ForumID = ?ForumID ");
            sqlCommand.Append("AND UnSubscribeDate IS NULL) ");
            sqlCommand.Append(")");

            sqlCommand.Append(")");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadId;

            arParams[2] = new MySqlParameter("?CurrentPostUserID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentPostUserId;

            return MySqlHelper.ExecuteDataset(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool ForumThreadAddSubscriber(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM cy_ForumThreadSubscriptions  ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }

            sqlCommand = new StringBuilder();


            if (subscriptionCount > 0)
            {
                sqlCommand.Append("UPDATE cy_ForumThreadSubscriptions ");
                sqlCommand.Append("SET SubscribeDate = now(), ");
                sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID ;");

            }
            else
            {

                sqlCommand.Append("INSERT INTO	cy_ForumThreadSubscriptions ( ");
                sqlCommand.Append("ThreadID, ");
                sqlCommand.Append("UserID ");
                sqlCommand.Append(") ");
                sqlCommand.Append("VALUES ( ");
                sqlCommand.Append("?ThreadID, ");
                sqlCommand.Append("?UserID ");
                sqlCommand.Append(") ;");

            }

            arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUNSubscribe(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = now() ");
            sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = now() ");
            sqlCommand.Append("WHERE UserID = ?UserID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static int ForumPostCreate(
            int threadId,
            string subject,
            string post,
            bool approved,
            int userId,
            DateTime postDate)
        {

            byte approve = 1;
            if (!approved)
            {
                approve = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COALESCE(Max(ThreadSequence) + 1,1) As ThreadSequence FROM cy_ForumPosts WHERE ThreadID = ?ThreadID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int threadSequence = 1;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
                }
            }

            sqlCommand = new StringBuilder();
            //sqlCommand.Append("SET @ThreadSequence = " + threadSequence.ToString() + " ; ");

            sqlCommand.Append("INSERT INTO cy_ForumPosts ( ");
            sqlCommand.Append("ThreadID, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("Post, ");
            sqlCommand.Append("PostDate, ");
            sqlCommand.Append("Approved, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("ThreadSequence ");

            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" ?ThreadID , ");
            sqlCommand.Append(" ?Subject  , ");
            sqlCommand.Append(" ?Post, ");
            sqlCommand.Append(" ?PostDate, ");
            sqlCommand.Append(" ?Approved , ");
            sqlCommand.Append(" ?UserID , ");
            sqlCommand.Append(" ?ThreadSequence  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new MySqlParameter("?Subject", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new MySqlParameter("?Post", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new MySqlParameter("?Approved", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = approve;

            arParams[4] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userId;

            arParams[5] = new MySqlParameter("?PostDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = postDate;

            arParams[6] = new MySqlParameter("?ThreadSequence", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = threadSequence;


            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            return newID;


        }

        public static bool ForumPostUpdate(
            int postId,
            string subject,
            string post,
            int sortOrder,
            bool approved)
        {
            byte approve = 1;
            if (!approved)
            {
                approve = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ForumPosts ");
            sqlCommand.Append("SET Subject = ?Subject, ");
            sqlCommand.Append("Post = ?Post, ");
            sqlCommand.Append("SortOrder = ?SortOrder, ");
            sqlCommand.Append("Approved = ?Approved ");
            sqlCommand.Append("WHERE PostID = ?PostID ;");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new MySqlParameter("?Subject", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new MySqlParameter("?Post", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new MySqlParameter("?Approved", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approve;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumPostUpdateThreadSequence(
            int postId,
            int threadSequence)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ForumPosts ");
            sqlCommand.Append("SET ThreadSequence = ?ThreadSequence ");
            sqlCommand.Append("WHERE PostID = ?PostID ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new MySqlParameter("?ThreadSequence", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumPostDelete(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts ");
            sqlCommand.Append("WHERE PostID = ?PostID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }






    }
}
