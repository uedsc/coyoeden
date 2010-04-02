/// Author:				Joe Audette
/// Created:			2007-11-15
/// Last Modified:		2007-11-15
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
    /// <summary>
    
    ///  
    /// </summary>
    public static class DBProductFile
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
            Guid productGuid,
            string fileName,
            byte[] fileImage,
            string serverFileName,
            int byteLength,
            DateTime created,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_ProductFile_Insert", 7);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fileName);
            sph.DefineSqlParameter("@FileImage", SqlDbType.Image, ParameterDirection.Input, fileImage);
            sph.DefineSqlParameter("@ServerFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, serverFileName);
            sph.DefineSqlParameter("@ByteLength", SqlDbType.Int, ParameterDirection.Input, byteLength);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            //SqlParameter[] arParams = new SqlParameter[6];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_ProductFile_Insert");

            //    arParams[0].Value = productGuid;
            //    arParams[1].Value = fileName;
            //    arParams[2].Value = fileImage;
            //    arParams[3].Value = serverFileName;
            //    arParams[4].Value = byteLength;
            //    arParams[5].Value = created;
            //    arParams[6].Value = createdBy;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = productGuid;

            //    arParams[1] = new SqlParameter("@FileName", SqlDbType.NVarChar, 255);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = fileName;

            //    arParams[2] = new SqlParameter("@FileImage", SqlDbType.Image);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = fileImage;

            //    arParams[3] = new SqlParameter("@ServerFileName", SqlDbType.NVarChar, 255);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = serverFileName;

            //    arParams[4] = new SqlParameter("@ByteLength", SqlDbType.Int);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = byteLength;

            //    arParams[5] = new SqlParameter("@Created", SqlDbType.DateTime);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = created;

            //    arParams[6] = new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = createdBy;


            //}

            //int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_ProductFile_Insert",
            //    arParams));

            //return rowsAffected;

        }


        public static bool Update(
            Guid productGuid,
            string fileName,
            byte[] fileImage,
            string serverFileName,
            int byteLength,
            DateTime created,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_ProductFile_Update", 7);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fileName);
            sph.DefineSqlParameter("@FileImage", SqlDbType.Image, ParameterDirection.Input, fileImage);
            sph.DefineSqlParameter("@ServerFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, serverFileName);
            sph.DefineSqlParameter("@ByteLength", SqlDbType.Int, ParameterDirection.Input, byteLength);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
            
            //SqlParameter[] arParams = new SqlParameter[7];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_ProductFile_Update");

            //    arParams[0].Value = productGuid;
            //    arParams[1].Value = fileName;
            //    arParams[2].Value = fileImage;
            //    arParams[3].Value = serverFileName;
            //    arParams[4].Value = byteLength;
            //    arParams[5].Value = created;
            //    arParams[6].Value = createdBy;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = productGuid;

            //    arParams[1] = new SqlParameter("@FileName", SqlDbType.NVarChar, 255);
            //    arParams[1].Direction = ParameterDirection.Input;
            //    arParams[1].Value = fileName;

            //    arParams[2] = new SqlParameter("@FileImage", SqlDbType.Image);
            //    arParams[2].Direction = ParameterDirection.Input;
            //    arParams[2].Value = fileImage;

            //    arParams[3] = new SqlParameter("@ServerFileName", SqlDbType.NVarChar, 255);
            //    arParams[3].Direction = ParameterDirection.Input;
            //    arParams[3].Value = serverFileName;

            //    arParams[4] = new SqlParameter("@ByteLength", SqlDbType.Int);
            //    arParams[4].Direction = ParameterDirection.Input;
            //    arParams[4].Value = byteLength;

            //    arParams[5] = new SqlParameter("@Created", SqlDbType.DateTime);
            //    arParams[5].Direction = ParameterDirection.Input;
            //    arParams[5].Value = created;

            //    arParams[6] = new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            //    arParams[6].Direction = ParameterDirection.Input;
            //    arParams[6].Value = createdBy;

            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_ProductFile_Update",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static bool Delete(Guid productGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "ws_ProductFile_Delete", 1);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
            
            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_ProductFile_Delete");

            //    arParams[0].Value = productGuid;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = productGuid;


            //}

            //int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_ProductFile_Delete",
            //    arParams);

            //return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid productGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "ws_ProductFile_SelectOne", 1);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            return sph.ExecuteReader();
            
            //SqlParameter[] arParams = new SqlParameter[1];
            //if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            //{
            //    arParams = SqlHelperParameterCache.GetSPParameterSet(GetConnectionString(),
            //        "ws_ProductFile_SelectOne");

            //    arParams[0].Value = productGuid;

            //}
            //else
            //{
            //    arParams[0] = new SqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier);
            //    arParams[0].Direction = ParameterDirection.Input;
            //    arParams[0].Value = productGuid;

            //}

            //return SqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "ws_ProductFile_SelectOne",
            //    arParams);

        }

        


    }
}
