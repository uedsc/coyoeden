// Author:					Joe Audette
// Created:					2009-02-23
// Last Modified:			2009-02-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;

namespace Cynthia.Data
{
    public static class DBEmailTemplate
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
        /// Inserts a row in the cy_EmailTemplate table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="specialGuid1"> specialGuid1 </param>
        /// <param name="specialGuid2"> specialGuid2 </param>
        /// <param name="name"> name </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="hasHtml"> hasHtml </param>
        /// <param name="isEditable"> isEditable </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid specialGuid1,
            Guid specialGuid2,
            string name,
            string subject,
            string textBody,
            string htmlBody,
            bool hasHtml,
            bool isEditable,
            DateTime createdUtc,
            Guid lastModBy)
        {
            #region Bit Conversion

            int intHasHtml = 0;
            if (hasHtml)
            {
                intHasHtml = 1;
            }

            int intIsEditable = 0;
            if (isEditable)
            {
                intIsEditable = 1;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_EmailTemplate (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("SpecialGuid1, ");
            sqlCommand.Append("SpecialGuid2, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("TextBody, ");
            sqlCommand.Append("HtmlBody, ");
            sqlCommand.Append("HasHtml, ");
            sqlCommand.Append("IsEditable, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":FeatureGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":SpecialGuid1, ");
            sqlCommand.Append(":SpecialGuid2, ");
            sqlCommand.Append(":Name, ");
            sqlCommand.Append(":Subject, ");
            sqlCommand.Append(":TextBody, ");
            sqlCommand.Append(":HtmlBody, ");
            sqlCommand.Append(":HasHtml, ");
            sqlCommand.Append(":IsEditable, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":LastModUtc, ");
            sqlCommand.Append(":LastModBy )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[15];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid.ToString();

            arParams[3] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new SqliteParameter(":SpecialGuid1", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid1.ToString();

            arParams[5] = new SqliteParameter(":SpecialGuid2", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = specialGuid2.ToString();

            arParams[6] = new SqliteParameter(":Name", DbType.String, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = name;

            arParams[7] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = subject;

            arParams[8] = new SqliteParameter(":TextBody", DbType.Object);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = textBody;

            arParams[9] = new SqliteParameter(":HtmlBody", DbType.Object);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = htmlBody;

            arParams[10] = new SqliteParameter(":HasHtml", DbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intHasHtml;

            arParams[11] = new SqliteParameter(":IsEditable", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intIsEditable;

            arParams[12] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = createdUtc;

            arParams[13] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdUtc;

            arParams[14] = new SqliteParameter(":LastModBy", DbType.String, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = lastModBy.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the cy_EmailTemplate table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="hasHtml"> hasHtml </param>
        /// <param name="isEditable"> isEditable </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string name,
            string subject,
            string textBody,
            string htmlBody,
            bool hasHtml,
            bool isEditable,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            #region Bit Conversion

            int intHasHtml = 0;
            if (hasHtml)
            {
                intHasHtml = 1;
            }

            int intIsEditable = 0;
            if (isEditable)
            {
                intIsEditable = 1;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_EmailTemplate ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Name = :Name, ");
            sqlCommand.Append("Subject = :Subject, ");
            sqlCommand.Append("TextBody = :TextBody, ");
            sqlCommand.Append("HtmlBody = :HtmlBody, ");
            sqlCommand.Append("HasHtml = :HasHtml, ");
            sqlCommand.Append("IsEditable = :IsEditable, ");
            sqlCommand.Append("LastModUtc = :LastModUtc, ");
            sqlCommand.Append("LastModBy = :LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[9];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":Name", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new SqliteParameter(":TextBody", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = textBody;

            arParams[4] = new SqliteParameter(":HtmlBody", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = htmlBody;

            arParams[5] = new SqliteParameter(":HasHtml", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intHasHtml;

            arParams[6] = new SqliteParameter(":IsEditable", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intIsEditable;

            arParams[7] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModUtc;

            arParams[8] = new SqliteParameter(":LastModBy", DbType.String, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModBy.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Deletes a row from the cy_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the cy_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the cy_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = :FeatureGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the cy_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the cy_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySpecial1(Guid specialGuid1)
        {
            if (specialGuid1 == Guid.Empty) { return false; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SpecialGuid1 = :SpecialGuid1 ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SpecialGuid1", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = specialGuid1.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the cy_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySpecial2(Guid specialGuid2)
        {
            if (specialGuid2 == Guid.Empty) { return false; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SpecialGuid2 = :SpecialGuid2 ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SpecialGuid2", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = specialGuid2.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_EmailTemplate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_EmailTemplate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append("ORDER BY Name ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_EmailTemplate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByModule(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append("AND SpecialGuid1 = :SpecialGuid1 ");
            sqlCommand.Append("AND SpecialGuid2 = :SpecialGuid2 ");
            sqlCommand.Append("ORDER BY Name ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new SqliteParameter(":SpecialGuid1", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = specialGuid1.ToString();

            arParams[2] = new SqliteParameter(":SpecialGuid2", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = specialGuid2.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the cy_EmailTemplate table.
        /// </summary>
        public static int GetCountByModuleAndName(Guid moduleGuid, string name)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append("AND Name = :Name ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new SqliteParameter(":Name", DbType.String, 2556);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the cy_EmailTemplate table.
        /// </summary>
        public static int GetCountByModuleSpecialAndName(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2, string name)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append("AND SpecialGuid1 = :SpecialGuid1 ");
            sqlCommand.Append("AND SpecialGuid2 = :SpecialGuid2 ");
            sqlCommand.Append("AND Name = :Name ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new SqliteParameter(":SpecialGuid1", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = specialGuid1.ToString();

            arParams[2] = new SqliteParameter(":SpecialGuid2", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = specialGuid2.ToString();

            arParams[3] = new SqliteParameter(":Name", DbType.String, 2556);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = name;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }



    }
}
