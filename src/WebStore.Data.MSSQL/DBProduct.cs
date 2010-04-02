/// Author:				Joe Audette
/// Created:			2007-11-15
/// Last Modified:		2009-12-07
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
   
    public static class DBProduct
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

        public static String DBPlatform()
        {
            return "MSSQL";
        }

       

        #region Product Main

        public static int Add(
            Guid guid,
            Guid storeGuid,
            Guid taxClassGuid,
            string modelNumber,
            byte status,
            byte fullfillmentType,
            decimal weight,
            int quantityOnHand,
            string imageFileName,
            byte[] imageFileBytes,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid lastModifedBy,
            string url,
            string name,
            string description,
            string teaser,
            bool showInProductList,
            bool enableRating,
            string metaDescription,
            string metaKeywords,
            int sortRank1,
            int sortRank2,
            string teaserFile,
            string teaserFileLink,
            string compiledMeta)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Product_Insert", 27);
			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
			sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
			sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
			sph.DefineSqlParameter("@ModelNumber", SqlDbType.NVarChar, 255, ParameterDirection.Input, modelNumber);
			sph.DefineSqlParameter("@Status", SqlDbType.TinyInt, ParameterDirection.Input, status);
			sph.DefineSqlParameter("@FullfillmentType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillmentType);
			sph.DefineSqlParameter("@Weight", SqlDbType.Decimal, ParameterDirection.Input, weight);
			sph.DefineSqlParameter("@QuantityOnHand", SqlDbType.Int, ParameterDirection.Input, quantityOnHand);
			sph.DefineSqlParameter("@ImageFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageFileName);
			sph.DefineSqlParameter("@ImageFileBytes", SqlDbType.Image, ParameterDirection.Input, imageFileBytes);
			sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
			sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
			sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
			sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
			sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
			sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
			sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);
			sph.DefineSqlParameter("@Abstract", SqlDbType.NText, ParameterDirection.Input, teaser);
            sph.DefineSqlParameter("@ShowInProductList", SqlDbType.Bit, ParameterDirection.Input, showInProductList);
            sph.DefineSqlParameter("@EnableRating", SqlDbType.Bit, ParameterDirection.Input, enableRating);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@SortRank1", SqlDbType.Int, ParameterDirection.Input, sortRank1);
            sph.DefineSqlParameter("@SortRank2", SqlDbType.Int, ParameterDirection.Input, sortRank2);
            sph.DefineSqlParameter("@TeaserFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, teaserFile);
            sph.DefineSqlParameter("@TeaserFileLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, teaserFileLink);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NText, ParameterDirection.Input, compiledMeta);
			int rowsAffected = sph.ExecuteNonQuery();
			return rowsAffected;

            

        }

        public static bool Update(
            Guid guid,
            Guid taxClassGuid,
            string modelNumber,
            byte status,
            byte fullfillmentType,
            decimal weight,
            int quantityOnHand,
            string imageFileName,
            byte[] imageFileBytes,
            DateTime lastModified,
            Guid lastModifedBy,
            string url,
            string name,
            string description,
            string teaser,
            bool showInProductList,
            bool enableRating,
            string metaDescription,
            string metaKeywords,
            int sortRank1,
            int sortRank2,
            string teaserFile,
            string teaserFileLink,
            string compiledMeta)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Product_Update", 24);
			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
			sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
			sph.DefineSqlParameter("@ModelNumber", SqlDbType.NVarChar, 255, ParameterDirection.Input, modelNumber);
			sph.DefineSqlParameter("@Status", SqlDbType.TinyInt, ParameterDirection.Input, status);
			sph.DefineSqlParameter("@FullfillmentType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillmentType);
			sph.DefineSqlParameter("@Weight", SqlDbType.Decimal, ParameterDirection.Input, weight);
			sph.DefineSqlParameter("@QuantityOnHand", SqlDbType.Int, ParameterDirection.Input, quantityOnHand);
			sph.DefineSqlParameter("@ImageFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageFileName);
			sph.DefineSqlParameter("@ImageFileBytes", SqlDbType.Image, ParameterDirection.Input, imageFileBytes);
			sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
			sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
			sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
			sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
			sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);
			sph.DefineSqlParameter("@Abstract", SqlDbType.NText, ParameterDirection.Input, teaser);
            sph.DefineSqlParameter("@ShowInProductList", SqlDbType.Bit, ParameterDirection.Input, showInProductList);
            sph.DefineSqlParameter("@EnableRating", SqlDbType.Bit, ParameterDirection.Input, enableRating);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@SortRank1", SqlDbType.Int, ParameterDirection.Input, sortRank1);
            sph.DefineSqlParameter("@SortRank2", SqlDbType.Int, ParameterDirection.Input, sortRank2);
            sph.DefineSqlParameter("@TeaserFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, teaserFile);
            sph.DefineSqlParameter("@TeaserFileLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, teaserFileLink);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NText, ParameterDirection.Input, compiledMeta);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

            
           

        }

        public static bool Delete(
            Guid guid,
            DateTime deletedTime,
            Guid deletedBy,
            string deletedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_Product_Delete", 4);
			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
			sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
			sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
			
			
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

            //SqlParameter[] arParams = new SqlParameter[4];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_Product_Delete");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = deletedTime;
            //    arParams[2].Value = deletedBy;
            //    arParams[3].Value = deletedFromIP;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@DeletedTime", SqlDbType.DateTime);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = deletedTime;

            //    arParams[2] = new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = deletedBy;

            //    arParams[3] = new SqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = deletedFromIP;


            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_Product_Delete",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetAll(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_SelectAll", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetForSiteMap(Guid siteGuid, Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_SelectForSiteMap", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            return sph.ExecuteReader();
        }

        public static int GetCount(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_GetCount", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a count of rows in the ws_Product table.
        /// </summary>
        public static int GetCountForAdminList(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_GetCountForAdminList", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static DataTable GetPageForAdminList(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            totalPages = 1;
            int totalRows
                = GetCountForAdminList(storeGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_SelectPageForAdminList", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            IDataReader reader = sph.ExecuteReader();

            DataTable dataTable = GetProductEmptyTable();

            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();
                row["Guid"] = new Guid(reader["Guid"].ToString());
                //row["StoreGuid"] = new Guid(reader["StoreGuid"].ToString());
                //row["TaxClassGuid"] = new Guid(reader["TaxClassGuid"].ToString());
                row["ModelNumber"] = reader["ModelNumber"];

                // doh! actual field is mis-spelled as FullFillmentType
                row["FulFillmentType"] = Convert.ToInt32(reader["FullFillmentType"]);

                row["Weight"] = Convert.ToDecimal(reader["Weight"]);
                //row["RetailPrice"] = Convert.ToDecimal(reader["RetailPrice"]);
                row["Url"] = reader["Url"];
                row["Name"] = reader["Name"];
                row["Abstract"] = reader["Abstract"];
                row["Description"] = reader["Description"];

                dataTable.Rows.Add(row);

            }
            reader.Close();


            return dataTable;

        }

        public static DataTable GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(storeGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            
            DataTable dataTable = GetProductEmptyTable();

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    //row["StoreGuid"] = new Guid(reader["StoreGuid"].ToString());
                    //row["TaxClassGuid"] = new Guid(reader["TaxClassGuid"].ToString());
                    row["ModelNumber"] = reader["ModelNumber"];

                    // doh! actual field is mis-spelled as FullFillmentType
                    row["FulFillmentType"] = Convert.ToInt32(reader["FullFillmentType"]);

                    row["Weight"] = Convert.ToDecimal(reader["Weight"]);
                    //row["RetailPrice"] = Convert.ToDecimal(reader["RetailPrice"]);
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["EnableRating"] = Convert.ToBoolean(reader["EnableRating"]);
                    row["TeaserFile"] = reader["TeaserFile"];
                    row["TeaserFileLink"] = reader["TeaserFileLink"];

                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;
           

        }

        private static DataTable GetProductEmptyTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            
            dataTable.Columns.Add("ModelNumber", typeof(string));
            // doh! actual field is mis-spelled as FullFillmentType
            dataTable.Columns.Add("FulFillmentType", typeof(int));
            dataTable.Columns.Add("Weight", typeof(decimal));
            
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("EnableRating", typeof(bool));
            dataTable.Columns.Add("TeaserFile", typeof(string));
            dataTable.Columns.Add("TeaserFileLink", typeof(string));


            return dataTable;

        }

        private static DataTable GetOfferProductEmptyTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));

            dataTable.Columns.Add("OfferGuid", typeof(Guid));
            dataTable.Columns.Add("ModelNumber", typeof(string));
            // doh! actual field is mis-spelled as FullFillmentType
            dataTable.Columns.Add("FulFillmentType", typeof(int));
            dataTable.Columns.Add("Weight", typeof(decimal));

            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("EnableRating", typeof(bool));
            dataTable.Columns.Add("TeaserFile", typeof(string));
            dataTable.Columns.Add("TeaserFileLink", typeof(string));


            return dataTable;

        }

        public static DataTable GetListForPageOfOffers(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {

            DataTable dataTable = GetOfferProductEmptyTable();

            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_GetListForPageOfOffers", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["OfferGuid"] = new Guid(reader["OfferGuid"].ToString());
                    
                    row["ModelNumber"] = reader["ModelNumber"];

                    // doh! actual field is mis-spelled as FullFillmentType
                    row["FulFillmentType"] = Convert.ToInt32(reader["FullFillmentType"]);

                    row["Weight"] = Convert.ToDecimal(reader["Weight"]);
                   
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["EnableRating"] = Convert.ToBoolean(reader["EnableRating"]);
                    row["TeaserFile"] = reader["TeaserFile"];
                    row["TeaserFileLink"] = reader["TeaserFileLink"];

                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;

        }


        #endregion

        public static IDataReader GetProductByPage(int siteId, int pageId)
        {

            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_Product_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();

        }

        

        #region Product History

        public static int AddHistory(
            Guid guid,
            Guid productGuid,
            Guid storeGuid,
            Guid taxClassGuid,
            string sku,
            byte status,
            byte fullfillmentType,
            decimal weight,
            int quantityOnHand,
            string imageFileName,
            byte[] imageFileBytes,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid lastModifedBy,
            DateTime logTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_ProductHistory_Insert", 16);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@Sku", SqlDbType.NVarChar, 255, ParameterDirection.Input, sku);
            sph.DefineSqlParameter("@Status", SqlDbType.TinyInt, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@FullfillmentType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillmentType);
            sph.DefineSqlParameter("@Weight", SqlDbType.Decimal, ParameterDirection.Input, weight);
            sph.DefineSqlParameter("@QuantityOnHand", SqlDbType.Int, ParameterDirection.Input, quantityOnHand);
            sph.DefineSqlParameter("@ImageFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageFileName);
            sph.DefineSqlParameter("@ImageFileBytes", SqlDbType.Image, ParameterDirection.Input, imageFileBytes);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LogTime", SqlDbType.DateTime, ParameterDirection.Input, logTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[16];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_ProductHistory_Insert");

            //    arParams[0].Value = guid;
            //    arParams[1].Value = productGuid;
            //    arParams[2].Value = storeGuid;
            //    arParams[3].Value = taxClassGuid;
            //    arParams[4].Value = sku;
            //    arParams[5].Value = status;
            //    arParams[6].Value = fullfillmentType;
            //    arParams[7].Value = weight;
            //    arParams[8].Value = quantityOnHand;
            //    arParams[9].Value = imageFileName;
            //    arParams[10].Value = imageFileBytes;
            //    arParams[11].Value = created;
            //    arParams[12].Value = createdBy;
            //    arParams[13].Value = lastModified;
            //    arParams[14].Value = lastModifedBy;
            //    arParams[15].Value = logTime;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = guid;

            //    arParams[1] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = productGuid;

            //    arParams[2] = new SqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = storeGuid;

            //    arParams[3] = new SqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = taxClassGuid;

            //    arParams[4] = new SqlParameter("@Sku", SqlDbType.NVarChar, 255);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = sku;

            //    arParams[5] = new SqlParameter("@Status", SqlDbType.TinyInt);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = status;

            //    arParams[6] = new SqlParameter("@FullfillmentType", SqlDbType.TinyInt);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = fullfillmentType;

            //    arParams[7] = new SqlParameter("@Weight", SqlDbType.Decimal);
            //    arParams[7].Direction = ParameterDirection.Input;
            //    arParams[7].Value = weight;

            //    arParams[8] = new SqlParameter("@QuantityOnHand", SqlDbType.Int);
            //    arParams[8].Direction = ParameterDirection.Input;
            //    arParams[8].Value = quantityOnHand;

            //    arParams[9] = new SqlParameter("@ImageFileName", SqlDbType.NVarChar, 255);
            //    arParams[9].Direction = ParameterDirection.Input;
            //    arParams[9].Value = imageFileName;

            //    arParams[10] = new SqlParameter("@ImageFileBytes", SqlDbType.Image);
            //    arParams[10].Direction = ParameterDirection.Input;
            //    arParams[10].Value = imageFileBytes;

            //    arParams[11] = new SqlParameter("@Created", SqlDbType.DateTime);
            //    arParams[11].Direction = ParameterDirection.Input;
            //    arParams[11].Value = created;

            //    arParams[12] = new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            //    arParams[12].Direction = ParameterDirection.Input;
            //    arParams[12].Value = createdBy;

            //    arParams[13] = new SqlParameter("@LastModified", SqlDbType.DateTime);
            //    arParams[13].Direction = ParameterDirection.Input;
            //    arParams[13].Value = lastModified;

            //    arParams[14] = new SqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier);
            //    arParams[14].Direction = ParameterDirection.Input;
            //    arParams[14].Value = lastModifedBy;

            //    arParams[15] = new SqlParameter("@LogTime", SqlDbType.DateTime);
            //    arParams[15].Direction = ParameterDirection.Input;
            //    arParams[15].Value = logTime;


            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_ProductHistory_Insert",
            //    arParams));

            //return rowsAffected;

        }

        #endregion

        


    }
}
