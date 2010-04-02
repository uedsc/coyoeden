/// Author:				Christian Fredh
/// Created:			2007-07-01
/// Last Modified:		2010-01-28
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
    
    public static class DBPollOption
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

        public static IDataReader GetPollOptions(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_PollOptions_Select", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];

            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_PollOptions_Select");

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
            //    "cy_PollOptions_Select",
            //    arParams);

        }


        public static IDataReader GetPollOption(Guid optionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_PollOptions_SelectOne", 1);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];

            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_PollOptions_SelectOne");

            //    arParams[0].Value = optionGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = optionGuid;
            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_PollOptions_SelectOne",
            //    arParams);

        }

        public static bool IncrementVotes(
            Guid pollGuid,
            Guid optionGuid,
            Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_PollOptions_IncrementVotes", 3);
            
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[3];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_PollOptions_IncrementVotes");

            //    arParams[0].Value = pollGuid;
            //    arParams[1].Value = optionGuid;
            //    arParams[2].Value = userGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = pollGuid;

            //    arParams[1] = new SqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = optionGuid;

            //    arParams[2] = new SqlParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = userGuid;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_PollOptions_IncrementVotes",
            //    arParams);

            //return (rowsAffected > -1);
        }


        public static int Add(
            Guid optionGuid,
            Guid pollGuid,
            string answer,
            int order)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_PollOptions_Insert", 4);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, 255, ParameterDirection.Input, answer);
            sph.DefineSqlParameter("@Order", SqlDbType.Int, ParameterDirection.Input, order);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[4];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_PollOptions_Insert");

            //    arParams[0].Value = optionGuid;
            //    arParams[1].Value = pollGuid;
            //    arParams[2].Value = answer;
            //    arParams[3].Value = order;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = optionGuid;

            //    arParams[1] = new SqlParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = pollGuid;

            //    arParams[2] = new SqlParameter("@Answer", SqlDbType.NVarChar, 255);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = answer;

            //    arParams[3] = new SqlParameter("@Order", SqlDbType.Int);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = order;
            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteScalar(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_PollOptions_Insert",
            //    arParams));

            //return rowsAffected;
        }


        public static bool Update(
            Guid optionGuid,
            string answer,
            int order)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_PollOptions_Update", 3);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, 255, ParameterDirection.Input, answer);
            sph.DefineSqlParameter("@Order", SqlDbType.Int, ParameterDirection.Input, order);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[3];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_PollOptions_Update");

            //    arParams[0].Value = optionGuid;
            //    arParams[1].Value = answer;
            //    arParams[2].Value = order;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = optionGuid;

            //    arParams[1] = new SqlParameter("@Answer", SqlDbType.NVarChar, 255);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = answer;

            //    arParams[2] = new SqlParameter("@Order", SqlDbType.Int);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = order;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_PollOptions_Update",
            //    arParams);

            //return (rowsAffected > -1);
        }

        public static bool Delete(Guid optionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_PollOptions_Delete", 1);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "cy_PollOptions_Delete");

            //    arParams[0].Value = optionGuid;
            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = optionGuid;
            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "cy_PollOptions_Delete",
            //    arParams);

            //return (rowsAffected > -1);
        }



    }
}
