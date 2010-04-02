/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2007-11-14
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

namespace WebStore.Data
{
   
    public static class DBOfferAvailability
    {
        
        private static string GetConnectionString()
        {
            if (ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"];
            }

            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

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

        public static String DBPlatform()
        {
            return "MSSQL";
        }

        

        public static int Add(
            Guid guid,
            Guid offerGuid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferAvailability_Insert", 13);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@BeginUTC", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@EndUTC", SqlDbType.DateTime, ParameterDirection.Input, endUtc);
            sph.DefineSqlParameter("@RequiresOfferCode", SqlDbType.Bit, ParameterDirection.Input, requiresOfferCode);
            sph.DefineSqlParameter("@OfferCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, offerCode);
            sph.DefineSqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int, ParameterDirection.Input, maxAllowedPerCustomer);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[13];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferAvailability_Insert");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = offerGuid;
            //    arParams[2].Value = beginUtc;
            //    arParams[3].Value = endUtc;
            //    arParams[4].Value = requiresOfferCode;
            //    arParams[5].Value = offerCode;
            //    arParams[6].Value = maxAllowedPerCustomer;
            //    arParams[7].Value = created;
            //    arParams[8].Value = createdBy;
            //    arParams[9].Value = createdFromIP;
            //    arParams[10].Value = lastModified;
            //    arParams[11].Value = lastModifedBy;
            //    arParams[12].Value = lastModifedFromIP;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = offerGuid;

            //    arParams[2] = new SqlParameter("@BeginUTC", SqlDbType.DateTime);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = beginUtc;

            //    arParams[3] = new SqlParameter("@EndUTC", SqlDbType.DateTime);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = endUtc;

            //    arParams[4] = new SqlParameter("@RequiresOfferCode", SqlDbType.Bit);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = requiresOfferCode;

            //    arParams[5] = new SqlParameter("@OfferCode", SqlDbType.NVarChar, 50);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = offerCode;

            //    arParams[6] = new SqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = maxAllowedPerCustomer;

            //    arParams[7] = new SqlParameter("@Created", SqlDbType.DateTime);
            //    arParams[7].Direction = ParameterDirection.Input;
            //    arParams[7].Value = created;

            //    arParams[8] = new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            //    arParams[8].Direction = ParameterDirection.Input;
            //    arParams[8].Value = createdBy;

            //    arParams[9] = new SqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[9].Direction = ParameterDirection.Input;
            //    arParams[9].Value = createdFromIP;

            //    arParams[10] = new SqlParameter("@LastModified", SqlDbType.DateTime);
            //    arParams[10].Direction = ParameterDirection.Input;
            //    arParams[10].Value = lastModified;

            //    arParams[11] = new SqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier);
            //    arParams[11].Direction = ParameterDirection.Input;
            //    arParams[11].Value = lastModifedBy;

            //    arParams[12] = new SqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[12].Direction = ParameterDirection.Input;
            //    arParams[12].Value = lastModifedFromIP;


            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferAvailability_Insert",
            //    arParams));

            //return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferAvailability_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@BeginUTC", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@EndUTC", SqlDbType.DateTime, ParameterDirection.Input, endUtc);
            sph.DefineSqlParameter("@RequiresOfferCode", SqlDbType.Bit, ParameterDirection.Input, requiresOfferCode);
            sph.DefineSqlParameter("@OfferCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, offerCode);
            sph.DefineSqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int, ParameterDirection.Input, maxAllowedPerCustomer);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[9];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferAvailability_Update");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = beginUtc;
            //    arParams[2].Value = endUtc;
            //    arParams[3].Value = requiresOfferCode;
            //    arParams[4].Value = offerCode;
            //    arParams[5].Value = maxAllowedPerCustomer;
            //    arParams[6].Value = lastModified;
            //    arParams[7].Value = lastModifedBy;
            //    arParams[8].Value = lastModifedFromIP;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@BeginUTC", SqlDbType.DateTime);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = beginUtc;

            //    arParams[2] = new SqlParameter("@EndUTC", SqlDbType.DateTime);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = endUtc;

            //    arParams[3] = new SqlParameter("@RequiresOfferCode", SqlDbType.Bit);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = requiresOfferCode;

            //    arParams[4] = new SqlParameter("@OfferCode", SqlDbType.NVarChar, 50);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = offerCode;

            //    arParams[5] = new SqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = maxAllowedPerCustomer;

            //    arParams[6] = new SqlParameter("@LastModified", SqlDbType.DateTime);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = lastModified;

            //    arParams[7] = new SqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier);
            //    arParams[7].Direction = ParameterDirection.Input;
            //    arParams[7].Value = lastModifedBy;

            //    arParams[8] = new SqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[8].Direction = ParameterDirection.Input;
            //    arParams[8].Value = lastModifedFromIP;

            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferAvailability_Update",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            DateTime deletedTime,
            string deletedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferAvailability_Update", 5);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, true);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //bool isDeleted = true;
            //SqlParameter[] arParams = new SqlParameter[5];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferAvailability_Delete");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = isDeleted;
            //    arParams[2].Value = deletedBy;
            //    arParams[3].Value = deletedTime;
            //    arParams[4].Value = deletedFromIP;


            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@IsDeleted", SqlDbType.Bit);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = isDeleted;

            //    arParams[2] = new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = deletedBy;

            //    arParams[3] = new SqlParameter("@DeletedTime", SqlDbType.DateTime);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = deletedTime;

            //    arParams[4] = new SqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = deletedFromIP;



            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferAvailability_Delete",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_OfferAvailability_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferAvailability_SelectOne");

            //    arParams[0].Value = guid;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferAvailability_SelectOne",
            //    arParams);

        }

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_OfferAvailability_SelectByOffer", 1);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferAvailability_SelectByOffer");

            //    arParams[0].Value = offerGuid;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = offerGuid;

            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferAvailability_SelectByOffer",
            //    arParams);

        }


        public static int AddHistory(
            Guid guid,
            Guid availabilityGuid,
            Guid offerGuid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            bool isDeleted,
            Guid deletedBy,
            DateTime deletedTime,
            string deletedFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP,
            DateTime logTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferAvailabilityHistory_Insert", 19);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@AvailabilityGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, availabilityGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@BeginUTC", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@EndUTC", SqlDbType.DateTime, ParameterDirection.Input, endUtc);
            sph.DefineSqlParameter("@RequiresOfferCode", SqlDbType.Bit, ParameterDirection.Input, requiresOfferCode);
            sph.DefineSqlParameter("@OfferCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, offerCode);
            sph.DefineSqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int, ParameterDirection.Input, maxAllowedPerCustomer);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIP);
            sph.DefineSqlParameter("@LogTime", SqlDbType.DateTime, ParameterDirection.Input, logTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[18];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferAvailabilityHistory_Insert");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = availabilityGuid;
            //    arParams[2].Value = offerGuid;
            //    arParams[3].Value = beginUtc;
            //    arParams[4].Value = endUtc;
            //    arParams[5].Value = requiresOfferCode;
            //    arParams[6].Value = offerCode;
            //    arParams[7].Value = maxAllowedPerCustomer;
            //    arParams[8].Value = created;
            //    arParams[9].Value = createdBy;
            //    arParams[10].Value = createdFromIP;
            //    arParams[11].Value = isDeleted;
            //    arParams[12].Value = deletedBy;
            //    arParams[13].Value = deletedTime;
            //    arParams[14].Value = deletedFromIP;
            //    arParams[15].Value = lastModified;
            //    arParams[16].Value = lastModifedBy;
            //    arParams[17].Value = lastModifedFromIP;
            //    arParams[18].Value = logTime;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@AvailabilityGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = availabilityGuid;

            //    arParams[2] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = offerGuid;

            //    arParams[3] = new SqlParameter("@BeginUTC", SqlDbType.DateTime);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = beginUtc;

            //    arParams[4] = new SqlParameter("@EndUTC", SqlDbType.DateTime);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = endUtc;

            //    arParams[5] = new SqlParameter("@RequiresOfferCode", SqlDbType.Bit);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = requiresOfferCode;

            //    arParams[6] = new SqlParameter("@OfferCode", SqlDbType.NVarChar, 50);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = offerCode;

            //    arParams[7] = new SqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int);
            //    arParams[7].Direction = ParameterDirection.Input;
            //    arParams[7].Value = maxAllowedPerCustomer;

            //    arParams[8] = new SqlParameter("@Created", SqlDbType.DateTime);
            //    arParams[8].Direction = ParameterDirection.Input;
            //    arParams[8].Value = created;

            //    arParams[9] = new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            //    arParams[9].Direction = ParameterDirection.Input;
            //    arParams[9].Value = createdBy;

            //    arParams[10] = new SqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[10].Direction = ParameterDirection.Input;
            //    arParams[10].Value = createdFromIP;

            //    arParams[11] = new SqlParameter("@IsDeleted", SqlDbType.Bit);
            //    arParams[11].Direction = ParameterDirection.Input;
            //    arParams[11].Value = isDeleted;

            //    arParams[12] = new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier);
            //    arParams[12].Direction = ParameterDirection.Input;
            //    arParams[12].Value = deletedBy;

            //    arParams[13] = new SqlParameter("@DeletedTime", SqlDbType.DateTime);
            //    arParams[13].Direction = ParameterDirection.Input;
            //    arParams[13].Value = deletedTime;

            //    arParams[14] = new SqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[14].Direction = ParameterDirection.Input;
            //    arParams[14].Value = deletedFromIP;

            //    arParams[15] = new SqlParameter("@LastModified", SqlDbType.DateTime);
            //    arParams[15].Direction = ParameterDirection.Input;
            //    arParams[15].Value = lastModified;

            //    arParams[16] = new SqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier);
            //    arParams[16].Direction = ParameterDirection.Input;
            //    arParams[16].Value = lastModifedBy;

            //    arParams[17] = new SqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[17].Direction = ParameterDirection.Input;
            //    arParams[17].Value = lastModifedFromIP;

            //    arParams[18] = new SqlParameter("@LogTime", SqlDbType.DateTime);
            //    arParams[18].Direction = ParameterDirection.Input;
            //    arParams[18].Value = logTime;


            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferAvailabilityHistory_Insert",
            //    arParams));

            //return rowsAffected;

        }

        

    }
}
