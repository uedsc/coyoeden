// Author:					Joe Audette
// Created:					2009-12-24
// Last Modified:			2009-12-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace Cynthia.Data
{

    public static class DBSavedQuery
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


        /// <summary>
        /// Inserts a row in the cy_SavedQuery table. Returns rows affected count.
        /// </summary>
        /// <param name="id"> id </param>
        /// <param name="name"> name </param>
        /// <param name="statement"> statement </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid id,
            string name,
            string statement,
            DateTime createdUtc,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_SavedQuery_Insert", 7);
            sph.DefineSqlParameter("@Id", SqlDbType.UniqueIdentifier, ParameterDirection.Input, id);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Statement", SqlDbType.NText, ParameterDirection.Input, statement);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the cy_SavedQuery table. Returns true if row updated.
        /// </summary>
        /// <param name="id"> id </param>
        /// <param name="statement"> statement </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid id,
            string statement,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_SavedQuery_Update", 4);
            sph.DefineSqlParameter("@Id", SqlDbType.UniqueIdentifier, ParameterDirection.Input, id);
            sph.DefineSqlParameter("@Statement", SqlDbType.NText, ParameterDirection.Input, statement);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the cy_SavedQuery table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "cy_SavedQuery_Delete", 1);
            sph.DefineSqlParameter("@Id", SqlDbType.UniqueIdentifier, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_SavedQuery table.
        /// </summary>
        /// <param name="id"> id </param>
        public static IDataReader GetOne(Guid id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_SavedQuery_SelectOne", 1);
            sph.DefineSqlParameter("@Id", SqlDbType.UniqueIdentifier, ParameterDirection.Input, id);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_SavedQuery table.
        /// </summary>
        /// <param name="name"> name </param>
        public static IDataReader GetOne(string name)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetReadConnectionString(), "cy_SavedQuery_SelectOneByName", 1);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, name);
            return sph.ExecuteReader();

        }

        

        /// <summary>
        /// Gets an IDataReader with all rows in the cy_SavedQuery table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                GetReadConnectionString(),
                CommandType.StoredProcedure,
                "cy_SavedQuery_SelectAll",
                null);

        }

        

    }

}
