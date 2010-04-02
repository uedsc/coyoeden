/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2010-03-18
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
    
    public static class DBSiteUser
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

        public static IDataReader GetUserCountByYearMonth(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(DateCreated) As Y,  ");
            sqlCommand.Append("MONTH(DateCreated) As M, ");
            sqlCommand.Append("CONCAT(YEAR(DateCreated), '-', MONTH(DateCreated)) As Label, ");
            sqlCommand.Append("COUNT(*) As Users ");
            
            sqlCommand.Append("FROM ");
            sqlCommand.Append("cy_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = ?SiteID ");
            sqlCommand.Append("GROUP BY YEAR(DateCreated), MONTH(DateCreated) ");
            sqlCommand.Append("ORDER BY YEAR(DateCreated), MONTH(DateCreated) ");
            sqlCommand.Append("; ");
            

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
       

        public static IDataReader GetUserList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Pwd, ");
            sqlCommand.Append("Email ");
            sqlCommand.Append("FROM cy_Users ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Email");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, Name As SiteUser ");
            sqlCommand.Append("FROM cy_Users ");
            sqlCommand.Append("WHERE SiteID = ?SiteID AND Name LIKE ?Query  ");

            sqlCommand.Append("UNION ");
            sqlCommand.Append("SELECT UserID, Email As SiteUser ");
            sqlCommand.Append("FROM cy_Users ");
            sqlCommand.Append("WHERE SiteID = ?SiteID AND Email LIKE ?Query   ");

            sqlCommand.Append("ORDER BY SiteUser; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?Query", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = query + "%";


            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static int UserCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM cy_Users WHERE SiteID = ?SiteID;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            return count;

        }

        public static int UserCount(int siteId, String userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM cy_Users ");
            sqlCommand.Append("WHERE SiteID = ?SiteID ");
            if (userNameBeginsWith.Length == 1)
            {
                sqlCommand.Append("AND LEFT(Name, 1) = ?UserNameBeginsWith ");
            }

            sqlCommand.Append("; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 1);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userNameBeginsWith;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            return count;

        }

        public static int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM cy_Users WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND DateCreated >= ?BeginDate ");
            sqlCommand.Append("AND DateCreated < ?EndDate; ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            return count;

        }

        public static int CountOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM cy_Users WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND LastActivityDate > ?SinceTime ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SinceTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM cy_Users WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND LastActivityDate >= ?SinceTime ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SinceTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM cy_Users WHERE SiteID = ?SiteID ");
            sqlCommand.Append("AND LastActivityDate >= ?SinceTime   ");
            sqlCommand.Append("ORDER BY LastActivityDate desc   ");
            sqlCommand.Append("LIMIT 50 ;   ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SinceTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sinceTime;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static int GetNewestUserId(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT MAX(UserID) FROM cy_Users WHERE SiteID = ?SiteID;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            return count;
        }



        public static int Count(int siteId, string userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM cy_Users WHERE SiteID = ?SiteID ");
            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND Name  LIKE ?UserNameBeginsWith ");
            }
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userNameBeginsWith + "%";

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static IDataReader GetUserListPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string userNameBeginsWith)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            int totalRows
                = Count(siteId, userNameBeginsWith);

            int totalPages = 1;
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

            
            sqlCommand.Append("SELECT	u.*,  ");
            
            sqlCommand.Append(" " + totalPages.ToString() + " As TotalPages  ");
            sqlCommand.Append("FROM	cy_Users u  ");

            sqlCommand.Append("WHERE u.ProfileApproved = 1   ");
            sqlCommand.Append("AND u.SiteID = ?SiteID   ");

            if (userNameBeginsWith.Length > 0)
            {
                sqlCommand.Append(" AND u.Name LIKE ?UserNameBeginsWith ");
            }

            sqlCommand.Append(" ORDER BY u.Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[3];


            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userNameBeginsWith + "%";

            arParams[2] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        private static int CountForSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM cy_Users WHERE SiteID = ?SiteID ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (Email LIKE ?SearchInput) ");

                sqlCommand.Append(")");
            }
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchInput + "%";

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static IDataReader GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            out int totalPages)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            int totalRows
                = CountForSearch(siteId, searchInput);

            totalPages = 1;
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


            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	cy_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                //sqlCommand.Append(" OR ");
                //sqlCommand.Append(" (Email LIKE ?SearchInput) ");

                sqlCommand.Append(")");
            }

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[3];


            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        private static int CountForAdminSearch(int siteId, string searchInput)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM cy_Users WHERE SiteID = ?SiteID ");
            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (Email LIKE ?SearchInput) ");

                sqlCommand.Append(")");
            }
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchInput + "%";

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static IDataReader GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            out int totalPages)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            int totalRows = CountForAdminSearch(siteId, searchInput);

            totalPages = 1;
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


            sqlCommand.Append("SELECT *  ");
            sqlCommand.Append("FROM	cy_Users  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID    ");

            if (searchInput.Length > 0)
            {
                sqlCommand.Append(" AND ");
                sqlCommand.Append("(");

                sqlCommand.Append(" (Name LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (LoginName LIKE ?SearchInput) ");
                sqlCommand.Append(" OR ");
                sqlCommand.Append(" (Email LIKE ?SearchInput) ");

                sqlCommand.Append(")");
            }

            sqlCommand.Append(" ORDER BY Name ");
            sqlCommand.Append("LIMIT "
                + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[3];


            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            arParams[1] = new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchInput + "%";

            arParams[2] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        //public static DataTable GetUserListPageTable(
        //    int siteId,
        //    int pageNumber,
        //    int pageSize,
        //    string userNameBeginsWith)
        //{

        //    StringBuilder sqlCommand = new StringBuilder();

        //    int pageLowerBound = (pageSize * pageNumber) - pageSize;
        //    int totalRows
        //        = CountUsersByRegistrationDateRange(
        //        siteId,
        //        DateTime.MinValue,
        //        DateTime.MaxValue);

        //    sqlCommand.Append("SET @TotalRows = " + totalRows.ToString(CultureInfo.InvariantCulture) + ";");
        //    sqlCommand.Append("SET @TotalPages = CEILING(@TotalRows / ?PageSize) ;");

        //    sqlCommand.Append("SELECT		u.*,  ");
        //    sqlCommand.Append("@TotalPages As TotalPages  ");
        //    sqlCommand.Append("FROM	cy_Users u  ");

        //    sqlCommand.Append("WHERE u.ProfileApproved = 1   ");
        //    sqlCommand.Append("AND u.SiteID = ?SiteID = 1   ");

        //    if (userNameBeginsWith.Length > 0)
        //    {
        //        sqlCommand.Append(" AND LEFT(u.Name, 1) = ?UserNameBeginsWith ");
        //    }


        //    sqlCommand.Append(" ORDER BY 	u.Name ");

        //    sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize  ; ");

        //    MySqlParameter[] arParams = new MySqlParameter[4];

        //    arParams[0] = new MySqlParameter("?PageNumber", MySqlDbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = pageNumber;

        //    arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = pageSize;

        //    arParams[2] = new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 1);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = userNameBeginsWith;

        //    arParams[3] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = siteId;

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("UserID", typeof(int));
        //    dataTable.Columns.Add("Name", typeof(String));
        //    dataTable.Columns.Add("DateCreated", typeof(DateTime));
        //    dataTable.Columns.Add("WebSiteUrl", typeof(String));
        //    dataTable.Columns.Add("TotalPosts", typeof(int));

        //    IDataReader reader = MySqlHelper.ExecuteReader(
        //        GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    while (reader.Read())
        //    {
        //        DataRow row = dataTable.NewRow();
        //        row["UserID"] = Convert.ToInt32(reader["UserID"]);
        //        row["Name"] = reader["Name"].ToString();
        //        row["DateCreated"] = Convert.ToDateTime(reader["DateCreated"]);
        //        row["WebSiteUrl"] = reader["WebSiteUrl"].ToString();
        //        row["TotalPosts"] = Convert.ToInt32(reader["TotalPosts"]);
        //        dataTable.Rows.Add(row);

        //    }

        //    reader.Close();

        //    return dataTable;


        //}




        public static int AddUser(
            Guid siteGuid,
            int siteId,
            string fullName,
            String loginName,
            string email,
            string password,
            Guid userGuid,
            DateTime dateCreated,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId)
        {
            int intmustChangePwd = 0;
            if (mustChangePwd) { intmustChangePwd = 1; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_Users (");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("LoginName, ");
            sqlCommand.Append("Email, ");

            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("TimeZoneId, ");
            sqlCommand.Append("EmailChangeGuid, ");


            sqlCommand.Append("Pwd, ");
            sqlCommand.Append("MustChangePwd, ");
            sqlCommand.Append("DateCreated, ");
            sqlCommand.Append("TotalPosts, ");
            sqlCommand.Append("TotalRevenue, ");
            sqlCommand.Append("UserGuid");
            sqlCommand.Append(")");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" ?SiteGuid , ");
            sqlCommand.Append(" ?SiteID , ");
            sqlCommand.Append(" ?FullName , ");
            sqlCommand.Append(" ?LoginName , ");
            sqlCommand.Append(" ?Email , ");

            sqlCommand.Append("?FirstName, ");
            sqlCommand.Append("?LastName, ");
            sqlCommand.Append("?TimeZoneId, ");
            sqlCommand.Append("?EmailChangeGuid, ");

            sqlCommand.Append(" ?Password, ");
            sqlCommand.Append("?MustChangePwd, ");
            sqlCommand.Append(" ?DateCreated, ");
            sqlCommand.Append(" 0, ");
            sqlCommand.Append(" 0, ");
            sqlCommand.Append(" ?UserGuid ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?FullName", MySqlDbType.VarChar, 100);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fullName;

            arParams[1] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = loginName;

            arParams[2] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = email;

            arParams[3] = new MySqlParameter("?Password", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = password;

            arParams[4] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = siteId;

            arParams[5] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid.ToString();

            arParams[6] = new MySqlParameter("?DateCreated", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;

            arParams[7] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = siteGuid.ToString();

            arParams[8] = new MySqlParameter("?MustChangePwd", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intmustChangePwd;


            arParams[9] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = firstName;

            arParams[10] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastName;

            arParams[11] = new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 100);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = timeZoneId;

            arParams[12] = new MySqlParameter("?EmailChangeGuid", MySqlDbType.VarChar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = Guid.Empty.ToString();


            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

            return newID;

        }

        public static bool UpdateUser(
            int userId,
            String fullName,
            String loginName,
            String email,
            String password,
            String gender,
            bool profileApproved,
            bool approvedForForums,
            bool trusted,
            bool displayInMemberList,
            String webSiteUrl,
            String country,
            String state,
            String occupation,
            String interests,
            String msn,
            String yahoo,
            String aim,
            String icq,
            String avatarUrl,
            String signature,
            String skin,
            String loweredEmail,
            String passwordQuestion,
            String passwordAnswer,
            String comment,
            int timeOffsetHours,
            string openIdUri,
            string windowsLiveId,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            string editorPreference,
            string newEmail,
            Guid emailChangeGuid)
        {
            byte approved = 1;
            if (!profileApproved)
            {
                approved = 0;
            }

            byte canPost = 1;
            if (!approvedForForums)
            {
                canPost = 0;
            }

            byte trust = 1;
            if (!trusted)
            {
                trust = 0;
            }

            byte displayInList = 1;
            if (!displayInMemberList)
            {
                displayInList = 0;
            }
            int intmustChangePwd = 0;
            if (mustChangePwd) { intmustChangePwd = 1; }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET Email = ?Email ,   ");
            sqlCommand.Append("Name = ?FullName,    ");
            sqlCommand.Append("LoginName = ?LoginName,    ");

            sqlCommand.Append("FirstName = ?FirstName,    ");
            sqlCommand.Append("LastName = ?LastName,    ");
            sqlCommand.Append("TimeZoneId = ?TimeZoneId,    ");
            sqlCommand.Append("EditorPreference = ?EditorPreference,    ");
            sqlCommand.Append("NewEmail = ?NewEmail,    ");
            sqlCommand.Append("EmailChangeGuid = ?EmailChangeGuid,    ");

            sqlCommand.Append("Pwd = ?Password,    ");
            sqlCommand.Append("MustChangePwd = ?MustChangePwd,    ");
            sqlCommand.Append("Gender = ?Gender,    ");
            sqlCommand.Append("ProfileApproved = ?ProfileApproved,    ");
            sqlCommand.Append("ApprovedForForums = ?ApprovedForForums,    ");
            sqlCommand.Append("Trusted = ?Trusted,    ");
            sqlCommand.Append("DisplayInMemberList = ?DisplayInMemberList,    ");
            sqlCommand.Append("WebSiteURL = ?WebSiteURL,    ");
            sqlCommand.Append("Country = ?Country,    ");
            sqlCommand.Append("State = ?State,    ");
            sqlCommand.Append("Occupation = ?Occupation,    ");
            sqlCommand.Append("Interests = ?Interests,    ");
            sqlCommand.Append("MSN = ?MSN,    ");
            sqlCommand.Append("Yahoo = ?Yahoo,   ");
            sqlCommand.Append("AIM = ?AIM,   ");
            sqlCommand.Append("ICQ = ?ICQ,    ");
            sqlCommand.Append("AvatarUrl = ?AvatarUrl,    ");
            sqlCommand.Append("Signature = ?Signature,    ");
            sqlCommand.Append("Skin = ?Skin,    ");

            sqlCommand.Append("LoweredEmail = ?LoweredEmail,    ");
            sqlCommand.Append("PasswordQuestion = ?PasswordQuestion,    ");
            sqlCommand.Append("PasswordAnswer = ?PasswordAnswer,    ");
            sqlCommand.Append("Comment = ?Comment,    ");
            sqlCommand.Append("OpenIDURI = ?OpenIDURI,    ");
            sqlCommand.Append("WindowsLiveID = ?WindowsLiveID,    ");

            sqlCommand.Append("TimeOffsetHours = ?TimeOffsetHours    ");

            sqlCommand.Append("WHERE UserID = ?UserID ;");

            MySqlParameter[] arParams = new MySqlParameter[36];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            arParams[2] = new MySqlParameter("?Password", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            arParams[3] = new MySqlParameter("?Gender", MySqlDbType.VarChar, 1);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = gender;

            arParams[4] = new MySqlParameter("?ProfileApproved", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approved;

            arParams[5] = new MySqlParameter("?ApprovedForForums", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = canPost;

            arParams[6] = new MySqlParameter("?Trusted", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = trust;

            arParams[7] = new MySqlParameter("?DisplayInMemberList", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = displayInList;

            arParams[8] = new MySqlParameter("?WebSiteURL", MySqlDbType.VarChar, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = webSiteUrl;

            arParams[9] = new MySqlParameter("?Country", MySqlDbType.VarChar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = country;

            arParams[10] = new MySqlParameter("?State", MySqlDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = state;

            arParams[11] = new MySqlParameter("?Occupation", MySqlDbType.VarChar, 100);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = occupation;

            arParams[12] = new MySqlParameter("?Interests", MySqlDbType.VarChar, 100);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = interests;

            arParams[13] = new MySqlParameter("?MSN", MySqlDbType.VarChar, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = msn;

            arParams[14] = new MySqlParameter("?Yahoo", MySqlDbType.VarChar, 100);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = yahoo;

            arParams[15] = new MySqlParameter("?AIM", MySqlDbType.VarChar, 100);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = aim;

            arParams[16] = new MySqlParameter("?ICQ", MySqlDbType.VarChar, 100);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = icq;

            arParams[17] = new MySqlParameter("?AvatarUrl", MySqlDbType.VarChar, 100);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = avatarUrl;

            arParams[18] = new MySqlParameter("?Signature", MySqlDbType.VarChar, 100);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = signature;

            arParams[19] = new MySqlParameter("?Skin", MySqlDbType.VarChar, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = skin;

            arParams[20] = new MySqlParameter("?FullName", MySqlDbType.VarChar, 100);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = fullName;

            arParams[21] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = loginName;

            arParams[22] = new MySqlParameter("?LoweredEmail", MySqlDbType.VarChar, 100);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = loweredEmail;

            arParams[23] = new MySqlParameter("?PasswordQuestion", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = passwordQuestion;

            arParams[24] = new MySqlParameter("?PasswordAnswer", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = passwordAnswer;

            arParams[25] = new MySqlParameter("?Comment", MySqlDbType.Text);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = comment;

            arParams[26] = new MySqlParameter("?TimeOffsetHours", MySqlDbType.Int32);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = timeOffsetHours;

            arParams[27] = new MySqlParameter("?OpenIDURI", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = openIdUri;

            arParams[28] = new MySqlParameter("?WindowsLiveID", MySqlDbType.VarChar, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = windowsLiveId;

            arParams[29] = new MySqlParameter("?MustChangePwd", MySqlDbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intmustChangePwd;

            arParams[30] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = firstName;

            arParams[31] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = lastName;

            arParams[32] = new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 100);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = timeZoneId;

            arParams[33] = new MySqlParameter("?EditorPreference", MySqlDbType.VarChar, 100);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = editorPreference;

            arParams[34] = new MySqlParameter("?NewEmail", MySqlDbType.VarChar, 100);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = newEmail;

            arParams[35] = new MySqlParameter("?EmailChangeGuid", MySqlDbType.VarChar, 36);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = emailChangeGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static bool DeleteUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Users ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);


            return (rowsAffected > 0);
        }

        public static bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET LastActivityDate = ?LastUpdate  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?LastUpdate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastUpdate;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET LastLoginDate = ?LastLoginTime,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?LastLoginTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastLoginTime;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool AccountLockout(Guid userGuid, DateTime lockoutTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET IsLockedOut = 1,  ");
            sqlCommand.Append("LastLockoutDate = ?LockoutTime  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?LockoutTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lockoutTime;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("LastPasswordChangedDate = ?LastPasswordChangedDate  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?LastPasswordChangedDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastPasswordChangeTime;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAttemptWindowStart = ?FailedPasswordAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?FailedPasswordAttemptWindowStart", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPasswordAttemptCount = ?AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?AttemptCount", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = attemptCount;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAnswerAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAnswerWindowStart = ?FailedPasswordAnswerAttemptWindowStart  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?FailedPasswordAnswerAttemptWindowStart", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowStartTime;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateFailedPasswordAnswerAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = ?AttemptCount  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?AttemptCount", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = attemptCount;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 1,  ");
            sqlCommand.Append("RegisterConfirmGuid = ?RegisterConfirmGuid  ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET   ");
            sqlCommand.Append("IsLockedOut = 0,  ");
            sqlCommand.Append("RegisterConfirmGuid = ?EmptyGuid  ");
            sqlCommand.Append("WHERE RegisterConfirmGuid = ?RegisterConfirmGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?EmptyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = emptyGuid.ToString();

            arParams[1] = new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = registrationConfirmationGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static bool AccountClearLockout(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET IsLockedOut = 0,  ");
            sqlCommand.Append("FailedPasswordAttemptCount = 0, ");
            sqlCommand.Append("FailedPwdAnswerAttemptCount = 0 ");

            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static bool UpdatePasswordQuestionAndAnswer(
            Guid userGuid,
            String passwordQuestion,
            String passwordAnswer)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET PasswordQuestion = ?PasswordQuestion,  ");
            sqlCommand.Append("PasswordAnswer = ?PasswordAnswer  ");

            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PasswordQuestion", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = passwordQuestion;

            arParams[2] = new MySqlParameter("?PasswordAnswer", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = passwordAnswer;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static void UpdateTotalRevenue(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
            sqlCommand.Append("SELECT SUM(SubTotal) FROM cy_CommerceReport WHERE UserGuid = ?UserGuid)  ");
            sqlCommand.Append(", 0) ");

            sqlCommand.Append("WHERE UserGuid = ?UserGuid  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

        }

        public static void UpdateTotalRevenue()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET TotalRevenue = COALESCE((  ");
            sqlCommand.Append("SELECT SUM(SubTotal) FROM cy_CommerceReport WHERE UserGuid = cy_Users.UserGuid)  ");
            sqlCommand.Append(", 0) ");

            sqlCommand.Append("  ;");

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                null);
        }



        public static bool FlagAsDeleted(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET IsDeleted = 1 ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static bool IncrementTotalPosts(int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET	TotalPosts = TotalPosts + 1 ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);


            return (rowsAffected > 0);
        }

        public static bool DecrementTotalPosts(int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Users ");
            sqlCommand.Append("SET	TotalPosts = TotalPosts - 1 ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = 0;

            rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader GetRolesByUser(int siteId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("cy_Roles.RoleID, ");
            sqlCommand.Append("cy_Roles.DisplayName, ");
            sqlCommand.Append("cy_Roles.RoleName ");

            sqlCommand.Append("FROM	 cy_UserRoles ");

            sqlCommand.Append("INNER JOIN cy_Users ");
            sqlCommand.Append("ON cy_UserRoles.UserID = cy_Users.UserID ");

            sqlCommand.Append("INNER JOIN cy_Roles ");
            sqlCommand.Append("ON  cy_UserRoles.RoleID = cy_Roles.RoleID ");

            sqlCommand.Append("WHERE cy_Users.SiteID = ?SiteID AND cy_Users.UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID AND RegisterConfirmGuid = ?RegisterConfirmGuid  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = registerConfirmGuid;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetSingleUser(int siteId, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID AND Email = ?Email  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSingleUserByLoginName(int siteId, string loginName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Users ");

            sqlCommand.Append("WHERE SiteID = ?SiteID AND LoginName = ?LoginName  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = loginName;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSingleUser(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Users ");

            sqlCommand.Append("WHERE UserID = ?UserID ;  ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSingleUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	cy_Users ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid ;  ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetUserGuidFromOpenId(
            int siteId,
            string openIdUri)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserGuid ");
            sqlCommand.Append("FROM	cy_Users ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID  ");
            sqlCommand.Append("AND OpenIDURI = ?OpenIDURI   ");
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?OpenIDURI", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = openIdUri;

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    userGuid = new Guid(reader["UserGuid"].ToString());
                }
            }

            return userGuid;

        }

        public static Guid GetUserGuidFromWindowsLiveId(
            int siteId,
            string windowsLiveId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserGuid ");
            sqlCommand.Append("FROM	cy_Users ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("SiteID = ?SiteID  ");
            sqlCommand.Append("AND WindowsLiveID = ?WindowsLiveID   ");
            sqlCommand.Append(" ;  ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?WindowsLiveID", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = windowsLiveId;

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    userGuid = new Guid(reader["UserGuid"].ToString());
                }
            }

            return userGuid;

        }

        public static string LoginByEmail(int siteId, string email, string password)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Name ");

            sqlCommand.Append("FROM  cy_Users ");

            sqlCommand.Append("WHERE Email = ?Email  ");
            sqlCommand.Append("AND SiteID = ?SiteID  ");
            sqlCommand.Append("AND Pwd = ?Password ;  ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            arParams[2] = new MySqlParameter("?Password", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            string userName = string.Empty;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userName = reader["Name"].ToString();
                }

            }

            return userName;
        }

        public static string Login(int siteId, string loginName, string password)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Name ");

            sqlCommand.Append("FROM  cy_Users ");

            sqlCommand.Append("WHERE LoginName = ?LoginName  ");
            sqlCommand.Append("AND SiteID = ?SiteID  ");
            sqlCommand.Append("AND Pwd = ?Password ;  ");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = loginName;

            arParams[2] = new MySqlParameter("?Password", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = password;

            string userName = string.Empty;

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userName = reader["Name"].ToString();
                }

            }
            return userName;
        }

        public static DataTable GetNonLazyLoadedPropertiesForUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserGuid", typeof(String));
            dataTable.Columns.Add("PropertyName", typeof(String));
            dataTable.Columns.Add("PropertyValueString", typeof(String));
            dataTable.Columns.Add("PropertyValueBinary", typeof(object));

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["UserGuid"] = reader["UserGuid"].ToString();
                    row["PropertyName"] = reader["PropertyName"].ToString();
                    row["PropertyValueString"] = reader["PropertyValueString"].ToString();
                    row["PropertyValueBinary"] = reader["PropertyValueBinary"];
                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;
        }

        public static IDataReader GetLazyLoadedProperty(Guid userGuid, String propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_UserProperties ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid AND PropertyName = ?PropertyName  ");
            sqlCommand.Append("LIMIT 1 ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool PropertyExists(Guid userGuid, string propertyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_UserProperties ");
            sqlCommand.Append("WHERE UserGuid = ?UserGuid AND PropertyName = ?PropertyName ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static void CreateProperty(
            Guid propertyId,
            Guid userGuid,
            String propertyName,
            String propertyValues,
            byte[] propertyValueb,
            DateTime lastUpdatedDate,
            bool isLazyLoaded)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO cy_UserProperties (");
            sqlCommand.Append("PropertyID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("PropertyName, ");
            sqlCommand.Append("PropertyValueString, ");
            sqlCommand.Append("PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?PropertyID, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?PropertyName, ");
            sqlCommand.Append("?PropertyValueString, ");
            sqlCommand.Append("?PropertyValueBinary, ");
            sqlCommand.Append("?LastUpdatedDate, ");
            sqlCommand.Append("?IsLazyLoaded );");


            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?PropertyID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = propertyId.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = propertyName;

            arParams[3] = new MySqlParameter("?PropertyValueString", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = propertyValues;

            arParams[4] = new MySqlParameter("?PropertyValueBinary", MySqlDbType.LongBlob);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = propertyValueb;

            arParams[5] = new MySqlParameter("?LastUpdatedDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastUpdatedDate;

            arParams[6] = new MySqlParameter("?IsLazyLoaded", MySqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isLazyLoaded;

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

        }

        public static void UpdateProperty(
            Guid userGuid,
            String propertyName,
            String propertyValues,
            byte[] propertyValueb,
            DateTime lastUpdatedDate,
            bool isLazyLoaded)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_UserProperties ");
            sqlCommand.Append("SET  ");
            //sqlCommand.Append("UserGuid = ?UserGuid, ");
            //sqlCommand.Append("PropertyName = ?PropertyName, ");
            sqlCommand.Append("PropertyValueString = ?PropertyValueString, ");
            sqlCommand.Append("PropertyValueBinary = ?PropertyValueBinary, ");
            sqlCommand.Append("LastUpdatedDate = ?LastUpdatedDate, ");
            sqlCommand.Append("IsLazyLoaded = ?IsLazyLoaded ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserGuid = ?UserGuid AND PropertyName = ?PropertyName ;");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = propertyName;

            arParams[2] = new MySqlParameter("?PropertyValueString", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = propertyValues;

            arParams[3] = new MySqlParameter("?PropertyValueBinary", MySqlDbType.LongBlob);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = propertyValueb;

            arParams[4] = new MySqlParameter("?LastUpdatedDate", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastUpdatedDate;

            arParams[5] = new MySqlParameter("?IsLazyLoaded", MySqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isLazyLoaded;

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);


        }


        

    }
}
