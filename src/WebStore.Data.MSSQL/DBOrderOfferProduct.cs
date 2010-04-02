/// Author:				Joe Audette
/// Created:			2007-11-15
/// Last Modified:		2008-07-18
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
    
    public static class DBOrderOfferProduct
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
            Guid orderGuid,
            Guid offerGuid,
            Guid productGuid,
            byte fullfillType,
            Guid fullfillTermsGuid,
            DateTime created)
        {

            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OrderOfferProduct_Insert", 7);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullfillTermsGuid);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[6];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OrderOfferProduct_Insert");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = orderGuid;
            //    arParams[2].Value = offerGuid;
            //    arParams[3].Value = productGuid;
            //    arParams[4].Value = fullfillType;
            //    arParams[5].Value = fullfillTermsGuid;
            //    arParams[6].Value = created;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = orderGuid;

            //    arParams[2] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = offerGuid;

            //    arParams[3] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = productGuid;

            //    arParams[4] = new SqlParameter("@FullfillType", SqlDbType.TinyInt);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = fullfillType;

            //    arParams[5] = new SqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = fullfillTermsGuid;

            //    arParams[6] = new SqlParameter("@Created", SqlDbType.DateTime);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = created;


            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OrderOfferProduct_Insert",
            //    arParams));

            //return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            Guid orderGuid,
            Guid offerGuid,
            Guid productGuid,
            byte fullfillType,
            Guid fullfillTermsGuid,
            DateTime created)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OrderOfferProduct_Update", 7);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullfillTermsGuid);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[7];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OrderOfferProduct_Update");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = orderGuid;
            //    arParams[2].Value = offerGuid;
            //    arParams[3].Value = productGuid;
            //    arParams[4].Value = fullfillType;
            //    arParams[5].Value = fullfillTermsGuid;
            //    arParams[6].Value = created;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = orderGuid;

            //    arParams[2] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = offerGuid;

            //    arParams[3] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = productGuid;

            //    arParams[4] = new SqlParameter("@FullfillType", SqlDbType.TinyInt);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = fullfillType;

            //    arParams[5] = new SqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = fullfillTermsGuid;

            //    arParams[6] = new SqlParameter("@Created", SqlDbType.DateTime);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = created;

            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OrderOfferProduct_Update",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OrderOfferProduct_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OrderOfferProduct_Delete");

            //    arParams[0].Value = guid;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;


            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OrderOfferProduct_Delete",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_OrderOfferProduct_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OrderOfferProduct_SelectOne");

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
            //    "ws_OrderOfferProduct_SelectOne",
            //    arParams);

        }

        /// <summary>
        /// Deletes a row from the ws_OrderOfferProduct table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OrderOfferProduct_DeleteByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }
        


    }
}
