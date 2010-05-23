
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;

namespace Cynthia.Data
{
   
    public static class DBForums
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
            byte isMod = 1;
            if (!isModerated)
            {
                isMod = 0;
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
            sqlCommand.Append("INSERT INTO	cy_Groups ( ");
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

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :UserID  , ");
            sqlCommand.Append(" datetime('now','localtime'), ");
            sqlCommand.Append(" :Title , ");
            sqlCommand.Append(" :Description , ");
            sqlCommand.Append(" :IsModerated , ");
            sqlCommand.Append(" :IsActive , ");
            sqlCommand.Append(" :SortOrder , ");
            sqlCommand.Append(" :PostsPerPage , ");
            sqlCommand.Append(" :ThreadsPerPage , ");
            sqlCommand.Append(" :AllowAnonymousPosts  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Description", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqliteParameter(":IsModerated", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isMod;

            arParams[5] = new SqliteParameter(":IsActive", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = active;

            arParams[6] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sortOrder;

            arParams[7] = new SqliteParameter(":PostsPerPage", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = postsPerPage;

            arParams[8] = new SqliteParameter(":ThreadsPerPage", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = threadsPerPage;

            arParams[9] = new SqliteParameter(":AllowAnonymousPosts", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowAnonymous;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("UPDATE		cy_Groups ");
            sqlCommand.Append("SET	Title = :Title, ");
            sqlCommand.Append("Description = :Description, ");
            sqlCommand.Append("IsModerated = :IsModerated, ");
            sqlCommand.Append("IsActive = :IsActive, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("PostsPerPage = :PostsPerPage, ");
            sqlCommand.Append("ThreadsPerPage = :ThreadsPerPage, ");
            sqlCommand.Append("AllowAnonymousPosts = :AllowAnonymousPosts ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[9];


            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqliteParameter(":Description", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new SqliteParameter(":IsModerated", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moderated;

            arParams[4] = new SqliteParameter(":IsActive", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = active;

            arParams[5] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new SqliteParameter(":PostsPerPage", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postsPerPage;

            arParams[7] = new SqliteParameter(":ThreadsPerPage", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = threadsPerPage;

            arParams[8] = new SqliteParameter(":AllowAnonymousPosts", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowAnonymous;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Groups ");
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID);");
            sqlCommand.Append("DELETE FROM cy_GroupSubscriptions WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID = :ModuleID) ;");
            sqlCommand.Append("DELETE FROM cy_Groups WHERE ModuleID = :ModuleID;");

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

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID)) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID)) );");
            sqlCommand.Append("DELETE FROM cy_ForumThreads WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID));");
            sqlCommand.Append("DELETE FROM cy_GroupSubscriptions WHERE ForumID IN (SELECT ItemID FROM cy_Groups WHERE ModuleID IN  (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID)) ;");
            sqlCommand.Append("DELETE FROM cy_Groups WHERE ModuleID IN (SELECT ModuleID FROM cy_Modules WHERE SiteID = :SiteID);");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("(SELECT COUNT(*) FROM cy_GroupSubscriptions fs WHERE fs.ForumID = f.ItemID AND fs.UnSubscribeDate IS NULL) As SubscriberCount  ");

            sqlCommand.Append("FROM	cy_Groups f ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON f.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_GroupSubscriptions s ");
            sqlCommand.Append("ON f.ItemID = s.ForumID AND s.UserID = :UserID ");

            sqlCommand.Append("WHERE f.ModuleID	= :ModuleID ");
            sqlCommand.Append("AND f.IsActive = 1 ");
            sqlCommand.Append("ORDER BY		f.SortOrder, f.ItemID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetForum(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As CreatedByUser, ");
            sqlCommand.Append("up.Name As MostRecentPostUser ");
            sqlCommand.Append("FROM	cy_Groups f ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON f.CreatedBy = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users up ");
            sqlCommand.Append("ON f.MostRecentPostUserID = up.UserID ");
            sqlCommand.Append("WHERE f.ItemID	= :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool IncrementPostCount(
            int forumId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET MostRecentPostDate = :MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("PostCount = PostCount + 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

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
                sqlCommand.Append("WHERE UserID = :UserID ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool IncrementPostCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PostCount = PostCount + 1 ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementPostCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET PostCount = PostCount - 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID ");
            sqlCommand.Append("FROM cy_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = :ForumID ");
            sqlCommand.Append("ORDER BY MostRecentPostDate DESC ");
            sqlCommand.Append("LIMIT 1 ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE ForumID = :ForumID ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    postCount = Convert.ToInt32(reader["PostCount"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	cy_Groups ");
            sqlCommand.Append("SET	 ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate,	 ");
            sqlCommand.Append("MostRecentPostUserID = :MostRecentPostUserID,	 ");
            sqlCommand.Append("PostCount = :PostCount	 ");
            sqlCommand.Append("WHERE ItemID = :ForumID ;");

            arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostDate;

            arParams[2] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostUserID;

            arParams[3] = new SqliteParameter(":PostCount", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postCount;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool IncrementThreadCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	cy_Groups ");
            sqlCommand.Append("SET	ThreadCount = ThreadCount + 1 ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementThreadCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Groups ");
            sqlCommand.Append("SET ThreadCount = ThreadCount - 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static int GetUserThreadCount(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_ForumThreads ");
            sqlCommand.Append("WHERE ThreadID IN (Select DISTINCT ThreadID FROM cy_ForumPosts WHERE cy_ForumPosts.UserID = :UserID) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("(SELECT PageID FROM cy_PageModules WHERE cy_PageModules.ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > :CurrentDate) LIMIT 1) As PageID, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");

            sqlCommand.Append("FROM	cy_ForumThreads t ");

            sqlCommand.Append("JOIN	cy_Groups f ");
            sqlCommand.Append("ON t.ForumID = f.ItemID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");

            sqlCommand.Append("WHERE t.ThreadID IN (Select DISTINCT ThreadID FROM cy_ForumPosts WHERE cy_ForumPosts.UserID = :UserID) ");

            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", :PageSize ");
            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":CurrentDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



        public static IDataReader GetThreads(int forumId, int pageNumber)
        {
            int threadsPerPage = 1;
            int totalThreads = 0;
            using (IDataReader reader = GetForum(forumId))
            {
                if (reader.Read())
                {
                    threadsPerPage = Convert.ToInt32(reader["ThreadsPerPage"]);
                    totalThreads = Convert.ToInt32(reader["ThreadCount"]);
                }
            }

            int pageLowerBound = (threadsPerPage * pageNumber) - threadsPerPage;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");
            sqlCommand.Append("FROM	cy_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("WHERE	t.ForumID = :ForumID ");
            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC ");
            sqlCommand.Append("LIMIT		" + threadsPerPage + " ");
            sqlCommand.Append("OFFSET		" + pageLowerBound + " ");
            sqlCommand.Append(" ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int ForumThreadGetPostCount(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            sqlCommand.Append("SELECT COUNT(*) FROM cy_ForumPosts WHERE ThreadID = :ThreadID ");

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int GetSubscriberCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ForumID = :ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UnSubscribeDate IS NULL");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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

            sqlCommand.Append("FROM	cy_GroupSubscriptions fs ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("cy_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserID = fs.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fs.ForumID = :ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.Name  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(", :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static bool AddSubscriber(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM cy_GroupSubscriptions  ");
            sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
                sqlCommand.Append("UPDATE cy_GroupSubscriptions ");
                sqlCommand.Append("SET SubscribeDate = :SubscribeDate, ");
                sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID ;");

            }
            else
            {

                sqlCommand.Append("INSERT INTO	cy_GroupSubscriptions ( ");
                sqlCommand.Append("ForumID, ");
                sqlCommand.Append("UserID, ");
                sqlCommand.Append("SubscribeDate");
                sqlCommand.Append(") ");
                sqlCommand.Append("VALUES ( ");
                sqlCommand.Append(":ForumID, ");
                sqlCommand.Append(":UserID, ");
                sqlCommand.Append(":SubscribeDate");
                sqlCommand.Append(") ;");

            }

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":SubscribeDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SubscriptionID = :SubscriptionID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SubscriptionID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subscriptionId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool Unsubscribe(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_GroupSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
            sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_GroupSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
            sqlCommand.Append("WHERE UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumSubscriptionExists(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool ForumThreadSubscriptionExists(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("f.PostsPerPage As PostsPerPage ");
            sqlCommand.Append("FROM	cy_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("JOIN	cy_Groups f ");
            sqlCommand.Append("ON f.ItemID = t.ForumID ");
            sqlCommand.Append("WHERE t.ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader ForumThreadGetPost(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	fp.* ");
            sqlCommand.Append("FROM	cy_ForumPosts fp ");
            sqlCommand.Append("WHERE fp.PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT COALESCE(Max(ForumSequence) + 1,1) As ForumSequence FROM cy_ForumThreads WHERE ForumID = :ForumID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                }
            }


            sqlCommand = new StringBuilder();
            //sqlCommand.Append("SET @ForumSequence = " + ForumSequence.ToString() + " ; ");

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
            sqlCommand.Append(" :ForumID , ");
            sqlCommand.Append(" :ThreadSubject  , ");
            sqlCommand.Append(" :SortOrder, ");
            sqlCommand.Append(" :ForumSequence, ");
            sqlCommand.Append(" :IsLocked , ");
            sqlCommand.Append(" :StartedByUserID , ");
            sqlCommand.Append(" :ThreadDate , ");
            sqlCommand.Append(" :StartedByUserID , ");
            sqlCommand.Append(" :ThreadDate  ");
            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":ThreadSubject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSubject;

            arParams[2] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new SqliteParameter(":IsLocked", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = locked;

            arParams[4] = new SqliteParameter(":StartedByUserID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startedByUserId;

            arParams[5] = new SqliteParameter(":ForumSequence", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = forumSequence;

            arParams[6] = new SqliteParameter(":ThreadDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = threadDate;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_ForumThreadSubscriptions (ThreadID, UserID) ");
            sqlCommand.Append("    SELECT :ThreadID as ThreadID, UserID from cy_GroupSubscriptions fs ");
            sqlCommand.Append("        WHERE fs.ForumID = :ForumID AND fs.SubscribeDate IS NOT NULL AND fs.UnSubscribeDate IS NULL;");

            arParams = new SqliteParameter[2];
            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newID;

            arParams[1] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeletePosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeleteSubscriptions(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("SET	ForumID = :ForumID, ");
            sqlCommand.Append("ThreadSubject = :ThreadSubject, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("IsLocked = :IsLocked ");


            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            arParams[2] = new SqliteParameter(":ThreadSubject", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadSubject;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":IsLocked", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = locked;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("SET MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies + 1, ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDecrementReplyStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT UserID, PostDate ");
            sqlCommand.Append("FROM cy_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ");
            sqlCommand.Append("ORDER BY PostID DESC ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int userID = 0;
            DateTime postDate = DateTime.Now;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userID = Convert.ToInt32(reader["UserID"]);
                    postDate = Convert.ToDateTime(reader["PostDate"]);
                }
            }

            sqlCommand = new StringBuilder();


            sqlCommand.Append("UPDATE cy_ForumThreads ");
            sqlCommand.Append("SET MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies - 1, ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userID;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = postDate;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdateViewStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ForumThreads ");
            sqlCommand.Append("SET TotalViews = TotalViews + 1 ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int postsPerPage = 10;

            sqlCommand.Append("SELECT	f.PostsPerPage As PostsPerPage ");
            sqlCommand.Append("FROM		cy_ForumThreads ft ");
            sqlCommand.Append("JOIN		cy_Groups f ");
            sqlCommand.Append("ON		ft.ForumID = f.ItemID ");
            sqlCommand.Append("WHERE	ft.ThreadID = :ThreadID ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            int beginSequence = 0;
           
            if (currentPageMaxThreadSequence > postsPerPage)
            {
                beginSequence = currentPageMaxThreadSequence - postsPerPage;

            }


            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID As ForumID, ");
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
            sqlCommand.Append("WHERE ft.ThreadID = :ThreadID ");

            sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID ");
            sqlCommand.Append("LIMIT		" + postsPerPage + " ");
            sqlCommand.Append("OFFSET		" + beginSequence + " ; ");

            arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID As ForumID, ");
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
            sqlCommand.Append("WHERE ft.ThreadID = :ThreadID ");

            sqlCommand.Append("ORDER BY	p.PostID  ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID As ForumID, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
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
            sqlCommand.Append("WHERE ft.ThreadID = :ThreadID ");

            sqlCommand.Append("ORDER BY p.ThreadSequence DESC  ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  fp.*, ");
            sqlCommand.Append("f.ModuleID As ModuleID, ");
            sqlCommand.Append("f.ItemID As ItemID, ");
            sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles As ViewRoles, ");
            sqlCommand.Append("md.FeatureName As FeatureName, ");
            sqlCommand.Append("md.ResourceFile As ResourceFile ");
            sqlCommand.Append("FROM	cy_ForumPosts fp ");
            sqlCommand.Append("JOIN	cy_ForumThreads ft ");
            sqlCommand.Append("ON fp.ThreadID = ft.ThreadID ");
            sqlCommand.Append("JOIN	cy_Groups f ");
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
            sqlCommand.Append("p.SiteID = :SiteID ");
            sqlCommand.Append("AND pm.PageID = :PageID ");

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

        public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT		fp.*, ");
            sqlCommand.Append("ft.ThreadSubject As ThreadSubject, ");
            sqlCommand.Append("ft.ForumID As ForumID, ");
            sqlCommand.Append("COALESCE(s.[Name],'Guest') as StartedBy, ");
            sqlCommand.Append("COALESCE(up.[Name], 'Guest') as PostAuthor, ");
            sqlCommand.Append("up.TotalPosts as PostAuthorTotalPosts,");
            sqlCommand.Append("up.AvatarUrl as PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL as PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature as PostAuthorSignature ");

            sqlCommand.Append("FROM		cy_ForumPosts fp ");
            sqlCommand.Append("JOIN		cy_ForumThreads ft ");
            sqlCommand.Append("ON		fp.ThreadID = ft.ThreadID ");

            sqlCommand.Append("JOIN		cy_Groups f ");
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

            sqlCommand.Append("WHERE	p.SiteID = :SiteID ");
            sqlCommand.Append("AND	(:PageID = -1 OR p.PageID = :PageID) ");
            sqlCommand.Append("AND	(:ModuleID = -1 OR m.ModuleID = :ModuleID) ");
            sqlCommand.Append("AND	(:ItemID = -1 OR f.ItemID = :ItemID) ");
            sqlCommand.Append("AND	(:ThreadID = -1 OR ft.ThreadID = :ThreadID) ");
            sqlCommand.Append("AND	(:MaximumDays = -1 OR datetime(fp.PostDate) >= datetime('now', '-" + maximumDays + " days')) ");

            sqlCommand.Append("ORDER BY	fp.PostDate DESC ; ");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemId;

            arParams[4] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = threadId;

            arParams[5] = new SqliteParameter(":MaximumDays", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maximumDays;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT u.Email As Email ");
            sqlCommand.Append("FROM	cy_Users u ");

            sqlCommand.Append("WHERE u.UserID <> ?CurrentPostUserID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("(");

            sqlCommand.Append("(");
            sqlCommand.Append("u.UserID IN (SELECT UserID FROM cy_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ");
            sqlCommand.Append("AND UnSubscribeDate IS NULL) ");
            sqlCommand.Append(")");

            sqlCommand.Append("OR ");

            sqlCommand.Append("(");
            sqlCommand.Append("u.UserID IN (SELECT UserID FROM cy_GroupSubscriptions ");
            sqlCommand.Append("WHERE ForumID = :ForumID ");
            sqlCommand.Append("AND UnSubscribeDate IS NULL) ");
            sqlCommand.Append(")");

            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadId;

            arParams[2] = new SqliteParameter(":CurrentPostUserID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentPostUserId;

            return SqliteHelper.ExecuteDataset(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool ForumThreadAddSubscriber(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM cy_ForumThreadSubscriptions  ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
                sqlCommand.Append("SET SubscribeDate = datetime('now','localtime'), ");
                sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID ;");

            }
            else
            {

                sqlCommand.Append("INSERT INTO	cy_ForumThreadSubscriptions ( ");
                sqlCommand.Append("ThreadID, ");
                sqlCommand.Append("UserID ");
                sqlCommand.Append(") ");
                sqlCommand.Append("VALUES ( ");
                sqlCommand.Append(":ThreadID, ");
                sqlCommand.Append(":UserID ");
                sqlCommand.Append(") ;");

            }

            arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUNSubscribe(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = datetime('now','localtime') ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = datetime('now','localtime') ");
            sqlCommand.Append("WHERE UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            sqlCommand.Append("SELECT COALESCE(Max(ThreadSequence) + 1,1) As ThreadSequence FROM cy_ForumPosts WHERE ThreadID = :ThreadID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int threadSequence = 1;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
                }
            }

            sqlCommand = new StringBuilder();

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
            sqlCommand.Append(" :ThreadID , ");
            sqlCommand.Append(" :Subject  , ");
            sqlCommand.Append(" :Post, ");
            sqlCommand.Append(" :PostDate, ");
            sqlCommand.Append(" :Approved , ");
            sqlCommand.Append(" :UserID , ");
            sqlCommand.Append(" :ThreadSequence  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new SqliteParameter(":Post", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = approve;

            arParams[4] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userId;

            arParams[5] = new SqliteParameter(":ThreadSequence", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = threadSequence;

            arParams[6] = new SqliteParameter(":PostDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postDate;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("SET Subject = :Subject, ");
            sqlCommand.Append("Post = :Post, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("Approved = :Approved ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new SqliteParameter(":Post", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approve;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("SET ThreadSequence = :ThreadSequence ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqliteParameter(":ThreadSequence", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumPostDelete(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ForumPosts ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }






    }
}
