﻿///							DBTaxClass.cs
/// Author:					Joe Audette
/// Created:				2008-06-25
/// Last Modified:			2009-02-23
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cynthia.Data
{
    public static class DBTaxClass
    {
        
        public static String DBPlatform()
        {
            return "MySQL";
        }

        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["MySqlConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["MySqlWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["MySqlWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["MySqlConnectionString"];
        }



        /// <summary>
        /// Inserts a row in the cy_TaxClass table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="created"> created </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            string title,
            string description,
            DateTime lastModified,
            DateTime created)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_TaxClass (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("Created )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?Created )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastModified;

            arParams[5] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = created;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the cy_TaxClass table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="created"> created </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string title,
            string description,
            DateTime lastModified,
            DateTime created)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_TaxClass ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("Created = ?Created ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            
            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastModified;

            arParams[4] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = created;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the cy_TaxClass table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_TaxClass table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_TaxClass table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY Title ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the cy_TaxClass table.
        /// </summary>
        public static int GetCountBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the cy_TaxClass table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountBySite(siteGuid);

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	cy_TaxClass  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY Title ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
    }
}
