// Author:				Joe Audette
// Created:			    2007-11-14
// Last Modified:		2009-04-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//

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
    /// <summary>
    ///  
    /// </summary>
    public static class DBCart
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

        

        public static int AddCart(
            Guid cartGuid,
            Guid storeGuid,
            Guid userGuid,
            decimal subTotal,
            decimal shippingTotal,
            decimal taxTotal,
            decimal orderTotal,
            DateTime created,
            string createdFromIP,
            DateTime lastModified,
            DateTime lastUserActivity,
            decimal discount,
            string discountCodesCsv,
            string customData,
            Guid clerkGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Cart_Insert", 15);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@SubTotal", SqlDbType.Decimal, ParameterDirection.Input, subTotal);
            sph.DefineSqlParameter("@ShippingTotal", SqlDbType.Decimal, ParameterDirection.Input, shippingTotal);
            sph.DefineSqlParameter("@TaxTotal", SqlDbType.Decimal, ParameterDirection.Input, taxTotal);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastUserActivity", SqlDbType.DateTime, ParameterDirection.Input, lastUserActivity);
            sph.DefineSqlParameter("@CustomData", SqlDbType.NText, ParameterDirection.Input, customData);
            sph.DefineSqlParameter("@DiscountCodesCsv", SqlDbType.NText, ParameterDirection.Input, discountCodesCsv);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@ClerkGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, clerkGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        public static bool UpdateCart(
            Guid cartGuid,
            Guid userGuid,
            decimal subTotal,
            decimal shippingTotal,
            decimal taxTotal,
            decimal orderTotal,
            DateTime lastModified,
            DateTime lastUserActivity,
            decimal discount,
            string discountCodesCsv,
            string customData,
            Guid clerkGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Cart_Update", 12);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@SubTotal", SqlDbType.Decimal, ParameterDirection.Input, subTotal);
            sph.DefineSqlParameter("@ShippingTotal", SqlDbType.Decimal, ParameterDirection.Input, shippingTotal);
            sph.DefineSqlParameter("@TaxTotal", SqlDbType.Decimal, ParameterDirection.Input, taxTotal);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastUserActivity", SqlDbType.DateTime, ParameterDirection.Input, lastUserActivity);
            sph.DefineSqlParameter("@CustomData", SqlDbType.NText, ParameterDirection.Input, customData);
            sph.DefineSqlParameter("@DiscountCodesCsv", SqlDbType.NText, ParameterDirection.Input, discountCodesCsv);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@ClerkGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, clerkGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
            
           
        }

        public static bool DeleteCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Cart_Delete", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteAnonymousByStore(Guid storeGuid, DateTime olderThan)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Cart_DeleteAnonymousByStore", 2);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@OlderThan", SqlDbType.DateTime, ParameterDirection.Input, olderThan);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByStore(Guid storeGuid, DateTime olderThan)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Cart_DeleteByStore", 2);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@OlderThan", SqlDbType.DateTime, ParameterDirection.Input, olderThan);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static IDataReader GetCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Cart_SelectOne", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_Cart_SelectOne");

            //    arParams[0].Value = cartGuid;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = cartGuid;

            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_Cart_SelectOne",
            //    arParams);

        }

        public static IDataReader GetByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Cart_SelectOneByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();

            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_Cart_SelectOneByUser");

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
            //    "ws_Cart_SelectOneByUser",
            //    arParams);

        }

        /// <summary>
        /// Gets a count of rows in the ws_Cart table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Cart_GetCountByStore", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

      
        public static int GetItemCountByFulfillmentType(Guid cartGuid, byte fulFillmentType)
        {

            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Cart_GetItemCountByFulfillmentType", 2);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@FulFillmentType", SqlDbType.TinyInt, ParameterDirection.Input, fulFillmentType);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a page of data from the ws_Cart table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount(storeGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Cart_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


        


    }
}
