/// Author:				Christian Fredh
/// Created:			2007-07-01
/// Last Modified:		2008-11-08
/// 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.


using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using Cynthia.Data;


namespace PollFeature.Data
{
   
    public static class DBPoll
    {

        /// <summary>
        /// Gets the connection string for read.
        /// </summary>
        /// <returns></returns>
        private static string GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        /// <summary>
        /// Gets the connection string for write.
        /// </summary>
        /// <returns></returns>
        private static string GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["MSSQLWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["MSSQLWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        public static String dbPlatform()
        {
            return "MSSQL";
        }


        public static IDataReader GetPolls(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_Polls_Select", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();


            //SqlParameter[] arParams = new SqlParameter[1];

            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_Select");

            //    arParams[0].Value = siteGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = siteGuid;

            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_Select",
            //    arParams);

        }

        public static IDataReader GetActivePolls(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_Polls_SelectActive", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[2];

            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_SelectActive");

            //    arParams[0].Value = siteGuid;
            //    arParams[1].Value = DateTime.UtcNow;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = siteGuid;

            //    arParams[1] = new SqlParameter("@CurrentTime", SqlDbType.DateTime);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = DateTime.UtcNow;

            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_SelectActive",
            //    arParams);
        }

        public static IDataReader GetPollsByUserGuid(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_Polls_SelectByUserGuid", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];

            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_SelectByUserGuid");

            //    arParams[0].Value = userGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = userGuid;

            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_SelectByUserGuid",
            //    arParams);

        }

        public static int Add(
            Guid pollGuid,
            Guid siteGuid,
            String question,
            bool anonymousVoting,
            bool allowViewingResultsBeforeVoting,
            bool showOrderNumbers,
            bool showResultsWhenDeactivated,
            bool active,
            DateTime activeFrom,
            DateTime activeTo)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_Insert", 10);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Question", SqlDbType.NVarChar, 255, ParameterDirection.Input, question);
            sph.DefineSqlParameter("@AnonymousVoting", SqlDbType.Bit, ParameterDirection.Input, anonymousVoting);
            sph.DefineSqlParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit, ParameterDirection.Input, allowViewingResultsBeforeVoting);
            sph.DefineSqlParameter("@ShowOrderNumbers", SqlDbType.Bit, ParameterDirection.Input, showOrderNumbers);
            sph.DefineSqlParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit, ParameterDirection.Input, showResultsWhenDeactivated);
            sph.DefineSqlParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, active);
            sph.DefineSqlParameter("@ActiveFrom", SqlDbType.DateTime, ParameterDirection.Input, activeFrom);
            sph.DefineSqlParameter("@ActiveTo", SqlDbType.DateTime, ParameterDirection.Input, activeTo);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[10];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_Insert");

            //    arParams[0].Value = pollGuid;
            //    arParams[1].Value = siteGuid;
            //    arParams[2].Value = question;
            //    arParams[3].Value = anonymousVoting;
            //    arParams[4].Value = allowViewingResultsBeforeVoting;
            //    arParams[5].Value = showOrderNumbers;
            //    arParams[6].Value = showResultsWhenDeactivated;
            //    arParams[7].Value = active;
            //    arParams[8].Value = activeFrom;
            //    arParams[9].Value = activeTo;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;

            //    arParams[1] = new SqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = siteGuid;

            //    arParams[2] = new SqlParameter("@Question", SqlDbType.NVarChar, 255);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = question;

            //    arParams[3] = new SqlParameter("@AnonymousVoting", SqlDbType.Bit);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = anonymousVoting;

            //    arParams[4] = new SqlParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = allowViewingResultsBeforeVoting;

            //    arParams[5] = new SqlParameter("@ShowOrderNumbers", SqlDbType.Bit);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = showOrderNumbers;

            //    arParams[6] = new SqlParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = showResultsWhenDeactivated;

            //    arParams[7] = new SqlParameter("@Active", SqlDbType.Bit);
            //    arParams[7].Direction = ParameterDirection.Input;
            //    arParams[7].Value = active;

            //    arParams[8] = new SqlParameter("@ActiveFrom", SqlDbType.DateTime);
            //    arParams[8].Direction = ParameterDirection.Input;
            //    arParams[8].Value = activeFrom;

            //    arParams[9] = new SqlParameter("@ActiveTo", SqlDbType.DateTime);
            //    arParams[9].Direction = ParameterDirection.Input;
            //    arParams[9].Value = activeTo;
            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_Insert",
            //    arParams));

            //return rowsAffected;
        }

        public static bool Update(
            Guid pollGuid,
            String question,
            bool anonymousVoting,
            bool allowViewingResultsBeforeVoting,
            bool showOrderNumbers,
            bool showResultsWhenDeactivated,
            bool active,
            DateTime activeFrom,
            DateTime activeTo)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_Update", 9);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@Question", SqlDbType.NVarChar, 255, ParameterDirection.Input, question);
            sph.DefineSqlParameter("@AnonymousVoting", SqlDbType.Bit, ParameterDirection.Input, anonymousVoting);
            sph.DefineSqlParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit, ParameterDirection.Input, allowViewingResultsBeforeVoting);
            sph.DefineSqlParameter("@ShowOrderNumbers", SqlDbType.Bit, ParameterDirection.Input, showOrderNumbers);
            sph.DefineSqlParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit, ParameterDirection.Input, showResultsWhenDeactivated);
            sph.DefineSqlParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, active);
            sph.DefineSqlParameter("@ActiveFrom", SqlDbType.DateTime, ParameterDirection.Input, activeFrom);
            sph.DefineSqlParameter("@ActiveTo", SqlDbType.DateTime, ParameterDirection.Input, activeTo);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[9];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_Update");

            //    arParams[0].Value = pollGuid;
            //    arParams[1].Value = question;
            //    arParams[2].Value = anonymousVoting;
            //    arParams[3].Value = allowViewingResultsBeforeVoting;
            //    arParams[4].Value = showOrderNumbers;
            //    arParams[5].Value = showResultsWhenDeactivated;
            //    arParams[6].Value = active;
            //    arParams[7].Value = activeFrom;
            //    arParams[8].Value = activeTo;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;

            //    arParams[1] = new SqlParameter("@Question", SqlDbType.NVarChar, 255);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = question;

            //    arParams[2] = new SqlParameter("@AnonymousVoting", SqlDbType.Bit);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = anonymousVoting;

            //    arParams[3] = new SqlParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = allowViewingResultsBeforeVoting;

            //    arParams[4] = new SqlParameter("@ShowOrderNumbers", SqlDbType.Bit);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = showOrderNumbers;

            //    arParams[5] = new SqlParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = showResultsWhenDeactivated;

            //    arParams[6] = new SqlParameter("@Active", SqlDbType.Bit);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = active;

            //    arParams[7] = new SqlParameter("@ActiveFrom", SqlDbType.DateTime);
            //    arParams[7].Direction = ParameterDirection.Input;
            //    arParams[7].Value = activeFrom;

            //    arParams[8] = new SqlParameter("@ActiveTo", SqlDbType.DateTime);
            //    arParams[8].Direction = ParameterDirection.Input;
            //    arParams[8].Value = activeTo;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_Update",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static IDataReader GetPoll(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_Polls_SelectOne", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];

            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_SelectOne");

            //    arParams[0].Value = pollGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;
            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_SelectOne",
            //    arParams);

        }

        public static IDataReader GetPollByModuleID(int moduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_Polls_SelectOneByModuleID", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];

            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_SelectOneByModuleID");

            //    arParams[0].Value = moduleID;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@ModuleID", SqlDbType.Int);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = moduleID;
            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_SelectOneByModuleID",
            //    arParams);
        }

        public static bool ClearVotes(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_ClearVotes", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_ClearVotes");

            //    arParams[0].Value = pollGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_ClearVotes",
            //    arParams);

            //return (rowsAffected > -1);
        }

        public static bool Delete(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_Delete", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_Delete");

            //    arParams[0].Value = pollGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_Delete",
            //    arParams);

            //return (rowsAffected > -1);
        }

        

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool UserHasVoted(Guid pollGuid, Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_UserHasVoted", 2);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int userHasVoted = Convert.ToInt32(sph.ExecuteScalar());
            return (userHasVoted == 1);

            //SqlParameter[] arParams = new SqlParameter[2];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_UserHasVoted");

            //    arParams[0].Value = pollGuid;
            //    arParams[1].Value = userGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;

            //    arParams[1] = new SqlParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = userGuid;
            //}

            //int userHasVoted = (int)SqlHelper.ExecuteScalar(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_UserHasVoted",
            //    arParams);

            //return (userHasVoted == 1);
        }

        public static bool AddToModule(Guid pollGuid, int moduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_AddToModule", 2);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

            //SqlParameter[] arParams = new SqlParameter[2];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_AddToModule");

            //    arParams[0].Value = pollGuid;
            //    arParams[1].Value = moduleID;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;

            //    arParams[1] = new SqlParameter("@ModuleID", SqlDbType.Int);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = moduleID;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_AddToModule",
            //    arParams);

            //return (rowsAffected > -1);
        }

        public static bool RemoveFromModule(int moduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_Polls_RemoveFromModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_Polls_RemoveFromModule");

            //    arParams[0].Value = moduleID;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@ModuleID", SqlDbType.Int);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = moduleID;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_Polls_RemoveFromModule",
            //    arParams);

            //return (rowsAffected > -1);
        }
    }
}
