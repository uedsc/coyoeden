/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2010-02-03
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
    public static class DBSiteUser
    {
       
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

       

        public static IDataReader GetUserList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_Select", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetUserCountByYearMonth(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_GetCountByMonthYear", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SmartDropDown", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Query", SqlDbType.NVarChar, 50, ParameterDirection.Input, query);
            sph.DefineSqlParameter("@RowsToGet", SqlDbType.Int, ParameterDirection.Input, rowsToGet);
            return sph.ExecuteReader();
        }

        public static int UserCount(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_Count", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public static int UserCount(int siteId, String userNameBeginsWith)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_CountByFirstLetter", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 1, ParameterDirection.Input, userNameBeginsWith);

            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public static int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_CountByRegistrationDateRange", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public static int CountOnlineSince(int siteId, DateTime sinceTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_CountOnlineSinceTime", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SinceTime", SqlDbType.DateTime, ParameterDirection.Input, sinceTime);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectUsersOnlineSinceTime", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SinceTime", SqlDbType.DateTime, ParameterDirection.Input, sinceTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectTop50UsersOnlineSinceTime", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SinceTime", SqlDbType.DateTime, ParameterDirection.Input, sinceTime);
            return sph.ExecuteReader();
        }


        public static int GetNewestUserId(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_GetNewestUserID", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public static IDataReader GetUserListPage(
                int siteId,
                int pageNumber,
                int pageSize,
                string userNameBeginsWith)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectPage", 4);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            sph.DefineSqlParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 50, ParameterDirection.Input, userNameBeginsWith);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        private static int CountForSearch(int siteId, string searchInput)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_CountForSearch", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public static IDataReader GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = CountForSearch(siteId, searchInput);

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

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectSearchPage", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        private static int CountForAdminSearch(int siteId, string searchInput)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_CountForAdminSearch", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }

        public static IDataReader GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = CountForAdminSearch(siteId, searchInput);

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

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectAdminSearchPage", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchInput);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static DataTable GetUserListPageTable(int siteId, int pageNumber, int pageSize, string userNameBeginsWith)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectPage", 4);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            sph.DefineSqlParameter("@UserNameBeginsWith", SqlDbType.NVarChar, 1, ParameterDirection.Input, userNameBeginsWith);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("Name", typeof(String));
            dataTable.Columns.Add("DateCreated", typeof(DateTime));
            dataTable.Columns.Add("WebSiteUrl", typeof(String));
            dataTable.Columns.Add("TotalPosts", typeof(int));

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["UserID"] = Convert.ToInt32(reader["UserID"]);
                    row["Name"] = reader["Name"].ToString();
                    row["DateCreated"] = Convert.ToDateTime(reader["DateCreated"]);
                    row["WebSiteUrl"] = reader["WebSiteUrl"].ToString();
                    row["TotalPosts"] = Convert.ToInt32(reader["TotalPosts"]);
                    dataTable.Rows.Add(row);

                }

            }

            return dataTable;

        }

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

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_Insert", 13);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, fullName);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Password", SqlDbType.NVarChar, 1000, ParameterDirection.Input, password);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@DateCreated", SqlDbType.DateTime, ParameterDirection.Input, dateCreated);
            sph.DefineSqlParameter("@MustChangePwd", SqlDbType.Bit, ParameterDirection.Input, mustChangePwd);

            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@TimeZoneId", SqlDbType.NVarChar, 32, ParameterDirection.Input, timeZoneId);
            sph.DefineSqlParameter("@EmailChangeGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, Guid.Empty);


            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateUser(
            int userId,
            string name,
            string loginName,
            string email,
            string password,
            string gender,
            bool profileApproved,
            bool approvedForGroups,
            bool trusted,
            bool displayInMemberList,
            string webSiteUrl,
            string country,
            string state,
            string occupation,
            string interests,
            string msn,
            string yahoo,
            string aim,
            string icq,
            string avatarUrl,
            string signature,
            string skin,
            string loweredEmail,
            string passwordQuestion,
            string passwordAnswer,
            string comment,
            int timeOffsetHours,
            string openIdUri,
            string windowsLiveId,
            bool mustChangePwd,
            string firstName,
            string lastName,
            string timeZoneId,
            string editorPreference,
            string newEmail,
            Guid emailChangeGuid
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_Update", 36);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Password", SqlDbType.NVarChar, 1000, ParameterDirection.Input, password);
            sph.DefineSqlParameter("@Gender", SqlDbType.NChar, 1, ParameterDirection.Input, gender);
            sph.DefineSqlParameter("@ProfileApproved", SqlDbType.Bit, ParameterDirection.Input, profileApproved);
            sph.DefineSqlParameter("@ApprovedForGroups", SqlDbType.Bit, ParameterDirection.Input, approvedForGroups);
            sph.DefineSqlParameter("@Trusted", SqlDbType.Bit, ParameterDirection.Input, trusted);
            sph.DefineSqlParameter("@DisplayInMemberList", SqlDbType.Bit, ParameterDirection.Input, displayInMemberList);
            sph.DefineSqlParameter("@WebSiteUrl", SqlDbType.NVarChar, 100, ParameterDirection.Input, webSiteUrl);
            sph.DefineSqlParameter("@Country", SqlDbType.NVarChar, 100, ParameterDirection.Input, country);
            sph.DefineSqlParameter("@State", SqlDbType.NVarChar, 100, ParameterDirection.Input, state);
            sph.DefineSqlParameter("@Occupation", SqlDbType.NVarChar, 100, ParameterDirection.Input, occupation);
            sph.DefineSqlParameter("@Interests", SqlDbType.NVarChar, 100, ParameterDirection.Input, interests);
            sph.DefineSqlParameter("@MSN", SqlDbType.NVarChar, 50, ParameterDirection.Input, msn);
            sph.DefineSqlParameter("@Yahoo", SqlDbType.NVarChar, 50, ParameterDirection.Input, yahoo);
            sph.DefineSqlParameter("@AIM", SqlDbType.NVarChar, 50, ParameterDirection.Input, aim);
            sph.DefineSqlParameter("@ICQ", SqlDbType.NVarChar, 50, ParameterDirection.Input, icq);
            sph.DefineSqlParameter("@AvatarUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, avatarUrl);
            sph.DefineSqlParameter("@Signature", SqlDbType.NVarChar, 255, ParameterDirection.Input, signature);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);

            sph.DefineSqlParameter("@LoweredEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, loweredEmail);
            sph.DefineSqlParameter("@PasswordQuestion", SqlDbType.NVarChar, 255, ParameterDirection.Input, passwordQuestion);
            sph.DefineSqlParameter("@PasswordAnswer", SqlDbType.NVarChar, 255, ParameterDirection.Input, passwordAnswer);
            sph.DefineSqlParameter("@Comment", SqlDbType.NText, ParameterDirection.Input, comment);
            sph.DefineSqlParameter("@TimeOffsetHours", SqlDbType.Int, ParameterDirection.Input, timeOffsetHours);
            sph.DefineSqlParameter("@OpenIDURI", SqlDbType.NVarChar, 255, ParameterDirection.Input, openIdUri);
            sph.DefineSqlParameter("@WindowsLiveID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveId);
            sph.DefineSqlParameter("@MustChangePwd", SqlDbType.Bit, ParameterDirection.Input, mustChangePwd);

            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@TimeZoneId", SqlDbType.NVarChar, 32, ParameterDirection.Input, timeZoneId);
            sph.DefineSqlParameter("@EditorPreference", SqlDbType.NVarChar, 100, ParameterDirection.Input, editorPreference);
            sph.DefineSqlParameter("@NewEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, newEmail);
            sph.DefineSqlParameter("@EmailChangeGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, emailChangeGuid);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static bool DeleteUser(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_Delete", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_UpdateLastActivityTime", 2);
            sph.DefineSqlParameter("@UserID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@LastUpdate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_UpdateLastLoginTime", 2);
            sph.DefineSqlParameter("@UserID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@LastLoginTime", SqlDbType.DateTime, ParameterDirection.Input, lastLoginTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_UpdateLastPasswordChangeTime", 2);
            sph.DefineSqlParameter("@UserID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PasswordChangeTime", SqlDbType.DateTime, ParameterDirection.Input, lastPasswordChangeTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateFailedPasswordAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SetFailedPasswordAttemptStartWindow", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@WindowStartTime", SqlDbType.DateTime, ParameterDirection.Input, windowStartTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateFailedPasswordAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SetFailedPasswordAttemptCount", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@AttemptCount", SqlDbType.Int, ParameterDirection.Input, attemptCount);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateFailedPasswordAnswerAttemptStartWindow(
            Guid userGuid,
            DateTime windowStartTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SetFailedPasswordAnswerAttemptStartWindow", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@WindowStartTime", SqlDbType.DateTime, ParameterDirection.Input, windowStartTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateFailedPasswordAnswerAttemptCount(
            Guid userGuid,
            int attemptCount)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SetFailedPasswordAnswerAttemptCount", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@AttemptCount", SqlDbType.Int, ParameterDirection.Input, attemptCount);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool AccountLockout(Guid userGuid, DateTime lockoutTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_AccountLockout", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@LockoutTime", SqlDbType.DateTime, ParameterDirection.Input, lockoutTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool AccountClearLockout(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_AccountClearLockout", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SetRegistrationConfirmationGuid", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, registrationConfirmationGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_ConfirmRegistration", 2);
            sph.DefineSqlParameter("@EmptyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, emptyGuid);
            sph.DefineSqlParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, registrationConfirmationGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdatePasswordQuestionAndAnswer(
            Guid userGuid,
            String passwordQuestion,
            String passwordAnswer)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_UpdatePasswordQuestionAndAnswer", 3);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PasswordQuestion", SqlDbType.NVarChar, 255, ParameterDirection.Input, passwordQuestion);
            sph.DefineSqlParameter("@PasswordAnswer", SqlDbType.NVarChar, 255, ParameterDirection.Input, passwordAnswer);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static void UpdateTotalRevenue(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_UpdateTotalRevenueByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.ExecuteNonQuery();

        }

        public static void UpdateTotalRevenue()
        {
            SqlHelper.ExecuteNonQuery(
				GetConnectionString(),
				CommandType.StoredProcedure,
				"cy_Users_UpdateTotalRevenue",
				null);
        }


        public static bool FlagAsDeleted(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_FlagAsDeleted", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool IncrementTotalPosts(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_IncrementTotalPosts", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DecrementTotalPosts(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_DecrementTotalPosts", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetRolesByUser(int siteId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_GetUserRoles", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }


        public static IDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectByRegisterGuid", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@RegisterConfirmGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, registerConfirmGuid);
            return sph.ExecuteReader();
        }


        public static IDataReader GetSingleUser(int siteId, string email)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectByEmail", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSingleUserByLoginName(int siteId, string loginName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectByLoginName", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSingleUser(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectOne", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSingleUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectByGuid", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();
        }

        public static Guid GetUserGuidFromOpenId(
            int siteId,
            string openIduri)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectGuidByOpenIDURI", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@OpenIDURI", SqlDbType.NVarChar, 100, ParameterDirection.Input, openIduri);

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = sph.ExecuteReader())
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
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_SelectGuidByWindowsLiveID", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@WindowsLiveID", SqlDbType.NVarChar, 100, ParameterDirection.Input, windowsLiveId);

            Guid userGuid = Guid.Empty;

            using (IDataReader reader = sph.ExecuteReader())
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
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_LoginByEmail", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Password", SqlDbType.NVarChar, 1000, ParameterDirection.Input, password);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.InputOutput, null);
            sph.ExecuteNonQuery();


            if (sph.Parameters[3] != null)
            {
                return sph.Parameters[3].Value.ToString();
            }
            else
            {
                return string.Empty;
            }

        }

        public static string Login(int siteId, string loginName, string password)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Users_Login", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Password", SqlDbType.NVarChar, 1000, ParameterDirection.Input, password);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.InputOutput, null);
            sph.ExecuteNonQuery();


            if (sph.Parameters[3] != null)
            {
                return sph.Parameters[3].Value.ToString();
            }
            else
            {
                return string.Empty;
            }

        }



        public static DataTable GetNonLazyLoadedPropertiesForUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_UserProperties_SelectByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserGuid", typeof(String));
            dataTable.Columns.Add("PropertyName", typeof(String));
            dataTable.Columns.Add("PropertyValueString", typeof(String));
            dataTable.Columns.Add("PropertyValueBinary", typeof(object));

            using (IDataReader reader = sph.ExecuteReader())
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
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_UserProperties_SelectOne", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            return sph.ExecuteReader();
        }

        public static bool PropertyExists(Guid userGuid, string propertyName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_UserProperties_PropertyExists", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        public static void CreateProperty(
            Guid propertyId,
            Guid userGuid,
            String propertyName,
            String propertyValue,
            byte[] propertyValueBinary,
            DateTime lastUpdateDate,
            bool isLazyLoaded)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_UserProperties_Insert", 7);
            sph.DefineSqlParameter("@PropertyID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, propertyId);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            sph.DefineSqlParameter("@PropertyValueString", SqlDbType.NText, ParameterDirection.Input, propertyValue);
            sph.DefineSqlParameter("@PropertyValueBinary", SqlDbType.Image, ParameterDirection.Input, propertyValueBinary);
            sph.DefineSqlParameter("@LastUpdatedDate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdateDate);
            sph.DefineSqlParameter("@isLazyLoaded", SqlDbType.Bit, ParameterDirection.Input, isLazyLoaded);
            sph.ExecuteNonQuery();
        }

        public static void UpdateProperty(
            Guid userGuid,
            String propertyName,
            String propertyValue,
            byte[] propertyValueBinary,
            DateTime lastUpdateDate,
            bool isLazyLoaded)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_UserProperties_Update", 6);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PropertyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, propertyName);
            sph.DefineSqlParameter("@PropertyValueString", SqlDbType.NText, ParameterDirection.Input, propertyValue);
            sph.DefineSqlParameter("@PropertyValueBinary", SqlDbType.Image, ParameterDirection.Input, propertyValueBinary);
            sph.DefineSqlParameter("@LastUpdatedDate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdateDate);
            sph.DefineSqlParameter("@isLazyLoaded", SqlDbType.Bit, ParameterDirection.Input, isLazyLoaded);
            sph.ExecuteNonQuery();
        }



        


    }
}
