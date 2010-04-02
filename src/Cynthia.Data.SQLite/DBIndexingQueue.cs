
namespace Cynthia.Data
{
    using System;
    using System.Text;
    using System.Data;
    using System.Data.Common;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using Mono.Data.Sqlite;

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
            return "SQLite";
        }

        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {
                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/Cynthia.db.config");

            }
            return connectionString;
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
            #region Bit Conversion

            int intRemoveOnly;
            if (removeOnly)
            {
                intRemoveOnly = 1;
            }
            else
            {
                intRemoveOnly = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_IndexingQueue (");
            sqlCommand.Append("IndexPath, ");
            sqlCommand.Append("SerializedItem, ");
            sqlCommand.Append("ItemKey, ");
            sqlCommand.Append("RemoveOnly )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":IndexPath, ");
            sqlCommand.Append(":SerializedItem, ");
            sqlCommand.Append(":ItemKey, ");
            sqlCommand.Append(":RemoveOnly )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":IndexPath", DbType.String, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = indexPath;

            arParams[1] = new SqliteParameter(":SerializedItem", DbType.Object);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = serializedItem;

            arParams[2] = new SqliteParameter(":ItemKey", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = itemKey;

            arParams[3] = new SqliteParameter(":RemoveOnly", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intRemoveOnly;


            Int64 newID = 0;
            newID = Convert.ToInt64(SqliteHelper.ExecuteScalar(GetConnectionString(), sqlCommand.ToString(), arParams).ToString());
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
        //    long rowId,
        //    string indexPath,
        //    string serializedItem,
        //    string itemKey,
        //    bool removeOnly)
        //{
        //    #region Bit Conversion

        //    int intRemoveOnly;
        //    if (removeOnly)
        //    {
        //        intRemoveOnly = 1;
        //    }
        //    else
        //    {
        //        intRemoveOnly = 0;
        //    }


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();

        //    sqlCommand.Append("UPDATE cy_IndexingQueue ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("IndexPath = :IndexPath, ");
        //    sqlCommand.Append("SerializedItem = :SerializedItem, ");
        //    sqlCommand.Append("ItemKey = :ItemKey, ");
        //    sqlCommand.Append("RemoveOnly = :RemoveOnly ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("RowId = :RowId ");
        //    sqlCommand.Append(";");

        //    SqliteParameter[] arParams = new SqliteParameter[5];

        //    arParams[0] = new SqliteParameter(":RowId", DbType.Int64);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = rowId;

        //    arParams[1] = new SqliteParameter(":IndexPath", DbType.String, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = indexPath;

        //    arParams[2] = new SqliteParameter(":SerializedItem", DbType.Object);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = serializedItem;

        //    arParams[3] = new SqliteParameter(":ItemKey", DbType.String, 255);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = itemKey;

        //    arParams[4] = new SqliteParameter(":RemoveOnly", DbType.Int32);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = intRemoveOnly;


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > -1);

        //}

        /// <summary>
        /// Deletes a row from the cy_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(Int64 rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_IndexingQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowId = :RowId ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowId", DbType.Int64);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes all rows from the cy_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool DeleteAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_IndexingQueue ");
            
            sqlCommand.Append(";");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                null);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the cy_IndexingQueue table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_IndexingQueue ");
            sqlCommand.Append(";");

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets an DataTable with rows from the cy_IndexingQueue table with the passed path.
        /// </summary>
        public static DataTable GetByPath(string indexPath)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_IndexingQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("IndexPath = :IndexPath ");
            sqlCommand.Append("ORDER BY RowId ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":IndexPath", DbType.String, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = indexPath;

            DataTable dt = new DataTable();
            dt.Columns.Add("RowId", typeof(int));
            dt.Columns.Add("IndexPath", typeof(String));
            dt.Columns.Add("SerializedItem", typeof(String));
            dt.Columns.Add("ItemKey", typeof(String));
            dt.Columns.Add("RemoveOnly", typeof(bool));

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["RowId"] = reader["RowId"];
                    row["IndexPath"] = reader["IndexPath"];
                    row["SerializedItem"] = reader["SerializedItem"];
                    row["ItemKey"] = reader["ItemKey"];
                    row["RemoveOnly"] = Convert.ToBoolean(reader["RemoveOnly"]);

                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the cy_IndexingQueue table.
        /// </summary>
        public static DataTable GetIndexPaths()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DISTINCT IndexPath ");
            sqlCommand.Append("FROM	cy_IndexingQueue ");
            sqlCommand.Append("ORDER BY IndexPath ");
            sqlCommand.Append(";");

            IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

            return DBPortal.GetTableFromDataReader(reader);

        }

        

        
    }
}
