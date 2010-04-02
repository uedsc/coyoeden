/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2008-10-16
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
    
    public static class DBOfferProduct
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
            Guid productGuid,
            byte fullfillType,
            Guid fullFillTermsGuid,
            int quantity,
            int sortOrder,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferProduct_Insert", 13);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullFillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullFillTermsGuid);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifiedBy);
            sph.DefineSqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifiedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[13];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferProduct_Insert");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = offerGuid;
            //    arParams[2].Value = productGuid;
            //    arParams[3].Value = fullfillType;
            //    arParams[4].Value = fullFillTermsGuid;
            //    arParams[5].Value = quantity;
            //    arParams[6].Value = sortOrder;
            //    arParams[7].Value = created;
            //    arParams[8].Value = createdBy;
            //    arParams[9].Value = createdFromIP;
            //    arParams[10].Value = lastModified;
            //    arParams[11].Value = lastModifiedBy;
            //    arParams[12].Value = lastModifiedFromIP;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = offerGuid;

            //    arParams[2] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = productGuid;

            //    arParams[3] = new SqlParameter("@FullfillType", SqlDbType.TinyInt);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = fullfillType;

            //    arParams[4] = new SqlParameter("@FullFillTermsGuid", SqlDbType.UniqueIdentifier);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = fullFillTermsGuid;

            //    arParams[5] = new SqlParameter("@Quantity", SqlDbType.Int);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = quantity;

            //    arParams[6] = new SqlParameter("@SortOrder", SqlDbType.Int);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = sortOrder;

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

            //    arParams[11] = new SqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier);
            //    arParams[11].Direction = ParameterDirection.Input;
            //    arParams[11].Value = lastModifiedBy;

            //    arParams[12] = new SqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[12].Direction = ParameterDirection.Input;
            //    arParams[12].Value = lastModifiedFromIP;


            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferProduct_Insert",
            //    arParams));

            //return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            byte fullfillType,
            Guid fullFillTermsGuid,
            int quantity,
            int sortOrder,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferProduct_Update", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullFillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullFillTermsGuid);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifiedBy);
            sph.DefineSqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifiedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[8];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferProduct_Update");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = fullfillType;
            //    arParams[2].Value = fullFillTermsGuid;
            //    arParams[3].Value = quantity;
            //    arParams[4].Value = sortOrder;
            //    arParams[5].Value = lastModified;
            //    arParams[6].Value = lastModifiedBy;
            //    arParams[7].Value = lastModifiedFromIP;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@FullfillType", SqlDbType.TinyInt);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = fullfillType;

            //    arParams[2] = new SqlParameter("@FullFillTermsGuid", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = fullFillTermsGuid;

            //    arParams[3] = new SqlParameter("@Quantity", SqlDbType.Int);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = quantity;

            //    arParams[4] = new SqlParameter("@SortOrder", SqlDbType.Int);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = sortOrder;

            //    arParams[5] = new SqlParameter("@LastModified", SqlDbType.DateTime);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = lastModified;

            //    arParams[6] = new SqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = lastModifiedBy;

            //    arParams[7] = new SqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[7].Direction = ParameterDirection.Input;
            //    arParams[7].Value = lastModifiedFromIP;

            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferProduct_Update",
            //    arParams);

            //return (rowsAffected > -1);

        }


        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            string deletedFromIP,
            DateTime deletedTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferProduct_Delete", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[4];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferProduct_Delete");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = deletedBy;
            //    arParams[2].Value = deletedFromIP;
            //    arParams[3].Value = deletedTime;


            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = deletedBy;

            //    arParams[2] = new SqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = deletedFromIP;

            //    arParams[3] = new SqlParameter("@DeletedTime", SqlDbType.DateTime);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = deletedTime;

            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferProduct_Delete",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static bool DeleteByProduct(
            Guid productGuid,
            Guid deletedBy,
            string deletedFromIP,
            DateTime deletedTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_OfferProduct_DeleteByProduct", 4);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[4];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_OfferProduct_DeleteByProduct");

            //    arParams[0].Value = productGuid;
            //    arParams[1].Value = deletedBy;
            //    arParams[2].Value = deletedFromIP;
            //    arParams[3].Value = deletedTime;


            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = productGuid;

            //    arParams[1] = new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = deletedBy;

            //    arParams[2] = new SqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = deletedFromIP;

            //    arParams[3] = new SqlParameter("@DeletedTime", SqlDbType.DateTime);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = deletedTime;

            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_OfferProduct_DeleteByProduct",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OfferProduct_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        
        }

        /// <summary>
        /// Gets a count of rows in the ws_Offer table.
        /// </summary>
        public static int GetCountByOffer(Guid offerGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OfferProduct_CountByOffer", 1);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);

            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OfferProduct_SelectByOffer", 1);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            
            return sph.ExecuteReader();

           
        }



        
    }
}
