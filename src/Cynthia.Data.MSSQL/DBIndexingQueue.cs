namespace Cynthia.Data
{
    using System;
    using System.IO;
    using System.Text;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Configuration;

    /// <summary>
    ///							DBIndexingQueue.cs
    /// Author:					Joe Audette
    /// Created:				2008-06-18
    /// Last Modified:			2008-12-13
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    public static class DBIndexingQueue
    {
        
        public static String DBPlatform()
        {
            return "MSSQL";
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }


        /// <summary>
        /// Inserts a row in the cy_IndexingQueue table. Returns new integer id.
        /// </summary>
        /// <param name="indexPath"> indexPath </param>
        /// <param name="serializedItem"> serializedItem </param>
        /// <param name="itemKey"> itemKey </param>
        /// <param name="removeOnly"> removeOnly </param>
        /// <returns>int</returns>
        public static Int64 Create(
            string indexPath,
            string serializedItem,
            string itemKey,
            bool removeOnly)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_IndexingQueue_Insert", 4);
            sph.DefineSqlParameter("@IndexPath", SqlDbType.NVarChar, 255, ParameterDirection.Input, indexPath);
            sph.DefineSqlParameter("@SerializedItem", SqlDbType.NText, ParameterDirection.Input, serializedItem);
            sph.DefineSqlParameter("@ItemKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, itemKey);
            sph.DefineSqlParameter("@RemoveOnly", SqlDbType.Bit, ParameterDirection.Input, removeOnly);
            Int64 newID = Convert.ToInt64(sph.ExecuteScalar());
            return newID;
        }


        ///// <summary>
        ///// Updates a row in the cy_IndexingQueue table. Returns true if row updated.
        ///// </summary>
        ///// <param name="rowId"> rowId </param>
        ///// <param name="indexPath"> indexPath </param>
        ///// <param name="serializedItem"> serializedItem </param>
        ///// <param name="itemKey"> itemKey </param>
        ///// <param name="removeOnly"> removeOnly </param>
        ///// <returns>bool</returns>
        //public static bool Update(
        //    Int64 rowId,
        //    string indexPath,
        //    string serializedItem,
        //    string itemKey,
        //    bool removeOnly)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_IndexingQueue_Update", 5);
        //    sph.DefineSqlParameter("@RowId", SqlDbType.BigInt, ParameterDirection.Input, rowId);
        //    sph.DefineSqlParameter("@IndexPath", SqlDbType.NVarChar, 255, ParameterDirection.Input, indexPath);
        //    sph.DefineSqlParameter("@SerializedItem", SqlDbType.NText, ParameterDirection.Input, serializedItem);
        //    sph.DefineSqlParameter("@ItemKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, itemKey);
        //    sph.DefineSqlParameter("@RemoveOnly", SqlDbType.Bit, ParameterDirection.Input, removeOnly);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);

        //}

        /// <summary>
        /// Deletes a row from the cy_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(Int64 rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_IndexingQueue_Delete", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.BigInt, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes all rows from the cy_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool DeleteAll()
        {
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "cy_IndexingQueue_DeleteAll",
                null);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the cy_IndexingQueue table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "cy_IndexingQueue_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the cy_IndexingQueue table.
        /// </summary>
        public static DataTable GetIndexPaths()
        {

            IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "cy_IndexingQueue_SelectDistinctPaths",
                null);

            return DBPortal.GetTableFromDataReader(reader);

        }

        /// <summary>
        /// Gets an DataTable with rows from the cy_IndexingQueue table with the passed path.
        /// </summary>
        public static DataTable GetByPath(string indexPath)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_IndexingQueue_SelectByPath", 1);
            sph.DefineSqlParameter("@IndexPath", SqlDbType.NVarChar, 255, ParameterDirection.Input, indexPath);
            IDataReader reader = sph.ExecuteReader();

            return DBPortal.GetTableFromDataReader(reader);

        }

        

        

        

    }

}
