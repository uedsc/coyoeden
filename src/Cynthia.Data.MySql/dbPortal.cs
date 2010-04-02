/// Author:					Joe Audette
/// Created:				2004-08-03
/// Last Modified:		    2009-12-24
/// 
/// This data Facade has all static methods and serves to abstract the underlying database
/// from the business layer. To port this application to another database platform, you only have to
/// replace this class. Make a copy of it and change the implementation to support your db of choice, 
/// then compile and replace the original Cynthia.Data.dll
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// GPL (http://www.gnu.org/licenses/gpl.html)
/// which can be found in the file GPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
///  
/// 2007/04/29 column names shortened
/// cy_Users rename columns
/// FailedPasswordAnswerAttemptCount > FailedPwdAnswerAttemptCount 27
/// FailedPasswordAnswerAttemptWindowStart > FailedPwdAnswerWindowStart 26
/// FailedPasswordAttemptWindowStart > FailedPwdAttemptWindowStart 27
/// 
/// cy_Sites rename columns
/// MinRequiredNonAlphanumericCharacters > MinReqNonAlphaChars 19
/// PasswordStrengthRegularExpression > PwdStrengthRegex 16
/// 
/// 2007-11-03 moved most methods to feature specific classes

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using log4net;


namespace Cynthia.Data
{
	
	public static class DBPortal
	{
		
        private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));

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


        #region Versioning and Upgrade Helpers

       
       
        #region Schema Table Methods


        public static bool SchemaVersionAddSchemaVersion(
            Guid applicationId,
            string applicationName,
            int major,
            int minor,
            int build,
            int revision)
        {

            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_SchemaVersion (");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ApplicationName, ");
            sqlCommand.Append("Major, ");
            sqlCommand.Append("Minor, ");
            sqlCommand.Append("Build, ");
            sqlCommand.Append("Revision )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ApplicationID, ");
            sqlCommand.Append("?ApplicationName, ");
            sqlCommand.Append("?Major, ");
            sqlCommand.Append("?Minor, ");
            sqlCommand.Append("?Build, ");
            sqlCommand.Append("?Revision );");


            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new MySqlParameter("?ApplicationName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new MySqlParameter("?Major", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new MySqlParameter("?Minor", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new MySqlParameter("?Build", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new MySqlParameter("?Revision", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }


        public static bool SchemaVersionUpdateSchemaVersion(
            Guid applicationId,
            string applicationName,
            int major,
            int minor,
            int build,
            int revision)
        {
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_SchemaVersion ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ApplicationName = ?ApplicationName, ");
            sqlCommand.Append("Major = ?Major, ");
            sqlCommand.Append("Minor = ?Minor, ");
            sqlCommand.Append("Build = ?Build, ");
            sqlCommand.Append("Revision = ?Revision ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ;");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new MySqlParameter("?ApplicationName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new MySqlParameter("?Major", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new MySqlParameter("?Minor", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new MySqlParameter("?Build", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new MySqlParameter("?Revision", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > -1);

        }


        public static bool SchemaVersionDeleteSchemaVersion(
            Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool SchemaVersionExists(Guid applicationId)
        {
            bool result = false;

            using (IDataReader reader = SchemaVersionGetSchemaVersion(applicationId))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        public static IDataReader SchemaVersionGetSchemaVersion(
            Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader SchemaVersionGetNonCore()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID <> '077E4857-F583-488E-836E-34A4B04BE855' ");
            sqlCommand.Append("ORDER BY ApplicationName ");
            sqlCommand.Append(";");

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static int SchemaScriptHistoryAddSchemaScriptHistory(
            Guid applicationId,
            string scriptFile,
            DateTime runTime,
            bool errorOccurred,
            string errorMessage,
            string scriptBody)
        {

            #region Bit Conversion
            int intErrorOccurred;
            if (errorOccurred)
            {
                intErrorOccurred = 1;
            }
            else
            {
                intErrorOccurred = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_SchemaScriptHistory (");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ScriptFile, ");
            sqlCommand.Append("RunTime, ");
            sqlCommand.Append("ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage, ");
            sqlCommand.Append("ScriptBody )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ApplicationID, ");
            sqlCommand.Append("?ScriptFile, ");
            sqlCommand.Append("?RunTime, ");
            sqlCommand.Append("?ErrorOccurred, ");
            sqlCommand.Append("?ErrorMessage, ");
            sqlCommand.Append("?ScriptBody );");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new MySqlParameter("?ScriptFile", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            arParams[2] = new MySqlParameter("?RunTime", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = runTime;

            arParams[3] = new MySqlParameter("?ErrorOccurred", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intErrorOccurred;

            arParams[4] = new MySqlParameter("?ErrorMessage", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = errorMessage;

            arParams[5] = new MySqlParameter("?ScriptBody", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = scriptBody;


            int newID = 0;
            newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());
            return newID;

        }

        public static bool SchemaScriptHistoryDeleteSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = ?ID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = ?ID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ");
            //sqlCommand.Append("AND ErrorOccurred = 0 ");

            sqlCommand.Append(" ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptErrorHistory(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ");
            sqlCommand.Append("AND ErrorOccurred = 1 ");

            sqlCommand.Append(" ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	cy_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = ?ApplicationID ");
            sqlCommand.Append("AND ScriptFile = ?ScriptFile ");

            sqlCommand.Append(" ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new MySqlParameter("?ScriptFile", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }




        #endregion

        #endregion

       
        #region DatabaseHelper

        public static DataTable GetTableFromDataReader(IDataReader reader)
        {
            DataTable dataTable = new DataTable();

            using (reader)
            {
                DataTable schemaTable = reader.GetSchemaTable();
                DataColumn column;
                DataRow row;
                ArrayList arrayList = new ArrayList();

                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {

                    column = new DataColumn();

                    if (!dataTable.Columns.Contains(schemaTable.Rows[i]["ColumnName"].ToString()))
                    {

                        column.ColumnName = schemaTable.Rows[i]["ColumnName"].ToString();
                        column.Unique = Convert.ToBoolean(schemaTable.Rows[i]["IsUnique"]);
                        column.AllowDBNull = Convert.ToBoolean(schemaTable.Rows[i]["AllowDBNull"]);
                        column.ReadOnly = Convert.ToBoolean(schemaTable.Rows[i]["IsReadOnly"]);
                        arrayList.Add(column.ColumnName);
                        dataTable.Columns.Add(column);

                    }

                }

                while (reader.Read())
                {

                    row = dataTable.NewRow();

                    for (int i = 0; i < arrayList.Count; i++)
                    {

                        row[((System.String)arrayList[i])] = reader[(System.String)arrayList[i]];

                    }

                    dataTable.Rows.Add(row);

                }
                
            }

            return dataTable;

        }

        public static DbException DatabaseHelperGetConnectionError(String overrideConnectionInfo)
        {
            DbException exception = null;

            MySqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new MySqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new MySqlConnection(GetWriteConnectionString());
            }

            try
            {
                connection.Open();


            }
            catch (DbException ex)
            {
                exception = ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }


            return exception;

        }

        public static bool DatabaseHelperCanAccessDatabase(String overrideConnectionInfo)
        {
            bool result = false;

            MySqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new MySqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new MySqlConnection(GetWriteConnectionString());
            }

            try
            {
                connection.Open();
                result = (connection.State == ConnectionState.Open);

            }
            catch { }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }


            return result;

        }

        public static bool DatabaseHelperCanAccessDatabase()
        {
            return DatabaseHelperCanAccessDatabase(null);
        }

        public static bool DatabaseHelperCanAlterSchema(String overrideConnectionInfo)
        {
            
            bool result = true;
            // Make sure we can create, alter and drop tables

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(@"
                CREATE TABLE `cy_Testdb` (
                  `FooID` int(11) NOT NULL auto_increment,
                  `Foo` varchar(255) NOT NULL default '',
                  PRIMARY KEY  (`FooID`)
                ) ENGINE=MyISAM  ;
                ");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }


            sqlCommand = new StringBuilder();
            sqlCommand.Append("ALTER TABLE cy_Testdb ADD COLUMN `MoreFoo` varchar(255) NULL;");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DROP TABLE cy_Testdb;");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            return result;
        }

        public static bool DatabaseHelperCanAlterInnoDbSchema(String overrideConnectionInfo)
        {

            bool result = true;
            // Make sure we can create, alter and drop tables

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(@"
                CREATE TABLE `cy_Testdb` (
                  `FooID` int(11) NOT NULL auto_increment,
                  `Foo` varchar(255) NOT NULL default '',
                  PRIMARY KEY  (`FooID`)
                ) ENGINE=InnoDB  ;
                ");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }


            sqlCommand = new StringBuilder();
            sqlCommand.Append("ALTER TABLE cy_Testdb ADD COLUMN `MoreFoo` varchar(255) NULL;");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DROP TABLE cy_Testdb;");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            return result;
        }

        public static bool DatabaseHelperCanCreateTemporaryTables()
        {
            bool result = true;
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(" CREATE TEMPORARY TABLE IF NOT EXISTS Temptest ");
            sqlCommand.Append("(IndexID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,");
            sqlCommand.Append(" foo VARCHAR (100) NOT NULL);");
            sqlCommand.Append(" DROP TABLE Temptest;");
            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), GetWriteConnectionString());
            }
            catch (Exception)
            {
                result = false;
            }


            return result;
        }

        public static bool DatabaseHelperRunScript(
            FileInfo scriptFile,
            String overrideConnectionInfo)
        {
            if (scriptFile == null) return false;

            string script = File.ReadAllText(scriptFile.FullName);

            if ((script == null) || (script.Length == 0)) return true;

            return DatabaseHelperRunScript(script, overrideConnectionInfo);

        }

        public static bool DatabaseHelperRunScript(string script, String overrideConnectionInfo)
        {
            if ((script == null) || (script.Trim().Length == 0)) return true;

            bool result = false;
            MySqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new MySqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new MySqlConnection(GetWriteConnectionString());
            }

            connection.Open();

            MySqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // this fixed the problems with mysql 5.1
                MySqlScript mySqlScript = new MySqlScript(connection, script);
                mySqlScript.Execute();

                //this worked in all versions of mysql prior to 5.1
                //MySqlHelper.ExecuteNonQuery(
                //    connection,
                //    script, 
                //    null);

                transaction.Commit();
                result = true;
                
            }
            catch (MySqlException ex)
            {
                transaction.Rollback();
                log.Error("dbPortal.RunScript failed", ex);
                throw;
            }
            finally
            {
                connection.Close();

            }

            return result;
        }

        public static bool DatabaseHelperUpdateTableField(
            String connectionString,
            String tableName,
            String keyFieldName,
            String keyFieldValue,
            String dataFieldName,
            String dataFieldValue,
            String additionalWhere)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = ?fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?fieldValue", MySqlDbType.Blob);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                connectionString, 
                sqlCommand.ToString(), 
                arParams);
                
            
            return (rowsAffected > 0);

        }

        public static bool DatabaseHelperUpdateTableField(
            String tableName,
            String keyFieldName,
            String keyFieldValue,
            String dataFieldName,
            String dataFieldValue,
            String additionalWhere)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = ?fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?fieldValue", MySqlDbType.Blob);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams);
                
            return (rowsAffected > 0);

        }

        public static IDataReader DatabaseHelperGetReader(
            String connectionString,
            String tableName,
            String whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append(" ; ");

            return MySqlHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString());

        }

        public static IDataReader DatabaseHelperGetReader(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = GetReadConnectionString(); }

            return MySqlHelper.ExecuteReader(
                connectionString,
                query);

        }

        public static int DatabaseHelperExecteNonQuery(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = GetWriteConnectionString(); }

            int rowsAffected =  MySqlHelper.ExecuteNonQuery(
                connectionString,
                query);

            return rowsAffected;

        }



        public static DataTable DatabaseHelperGetTable(
            String connectionString,
            String tableName,
            String whereClause)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append(" ; ");

            DataSet ds = MySqlHelper.ExecuteDataset(
                connectionString,
                sqlCommand.ToString());

            return ds.Tables[0];

        }

        public static void DatabaseHelperDoVersion2320PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            string connectionString;
            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connectionString = overrideConnectionInfo;
            }
            else
            {
                connectionString = GetReadConnectionString();
            }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("u.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("ls.BeginUTC, ");
            sqlCommand.Append("ls.UseHtml ");


            sqlCommand.Append("FROM ");
            sqlCommand.Append("cy_LetterSubscriber ls ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("cy_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");
            sqlCommand.Append(" ; ");

            DataSet ds = MySqlHelper.ExecuteDataset(
                connectionString,
                sqlCommand.ToString());

            DataTable dataTable = ds.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {

                DBLetterSubscription.Create(
                    Guid.NewGuid(),
                    new Guid(row["SiteGuid"].ToString()),
                    new Guid(row["LetterInfoGuid"].ToString()),
                    new Guid(row["UserGuid"].ToString()),
                    row["Email"].ToString().ToLower(),
                    true,
                    new Guid("00000000-0000-0000-0000-000000000000"),
                    Convert.ToDateTime(row["BeginUTC"]),
                    Convert.ToBoolean(row["UseHtml"]));

            }

        }

        public static void DatabaseHelperDoVersion2230PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            string connectionString;
            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connectionString = overrideConnectionInfo;
            }
            else
            {
                connectionString = GetWriteConnectionString();
            }

            DataTable dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_ModuleDefinitions",
                " where Guid is null ");

            // UPDATE cy_ModuleDefinitions SET [Guid] = newid() 
            // WHERE [Guid] IS NULL
            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_ModuleDefinitions",
                    "ModuleDefID",
                    row["ModuleDefID"].ToString(),
                    "guid",
                    Guid.NewGuid().ToString(),
                    " AND Guid is null ");

            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ModuleDefinitionSettings ");
            sqlCommand.Append("SET FeatureGuid = (SELECT Guid ");
            sqlCommand.Append("FROM cy_ModuleDefinitions ");
            sqlCommand.Append("WHERE cy_ModuleDefinitions.ModuleDefID ");
            sqlCommand.Append(" = cy_ModuleDefinitionSettings.ModuleDefID LIMIT 1)");
            sqlCommand.Append(";");

            DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);

            DatabaseHelperRunScript(
                "ALTER TABLE `cy_ModuleDefinitions` CHANGE `Guid` `Guid` CHAR(36)  NOT NULL;",
                overrideConnectionInfo);

            DatabaseHelperRunScript(
                "ALTER TABLE `cy_ModuleDefinitionSettings` CHANGE `FeatureGuid` `FeatureGuid` CHAR(36)  NOT NULL;",
                overrideConnectionInfo);


        }

        public static void DatabaseHelperDoVersion2234PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            string connectionString;
            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connectionString = overrideConnectionInfo;
            }
            else
            {
                connectionString = GetWriteConnectionString();
            }

            DataTable dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_Pages",
                " where PageGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_Pages",
                    "PageID",
                    row["PageID"].ToString(),
                    "PageGuid",
                    Guid.NewGuid().ToString(),
                    " and PageGuid is null ");

            }


        }

        public static void DatabaseHelperDoVersion2247PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            string connectionString;
            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connectionString = overrideConnectionInfo;
            }
            else
            {
                connectionString = GetWriteConnectionString();
            }

            DataTable dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_FriendlyUrls",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_FriendlyUrls",
                    "UrlID",
                    row["UrlID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_Modules",
                " where Guid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_Modules",
                    "ModuleID",
                    row["ModuleID"].ToString(),
                    "Guid",
                    Guid.NewGuid().ToString(),
                    " and Guid is null ");

            }


            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_Roles",
                " where RoleGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_Roles",
                    "RoleID",
                    row["RoleID"].ToString(),
                    "RoleGuid",
                    Guid.NewGuid().ToString(),
                    " and RoleGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_ModuleSettings",
                " where SettingGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_ModuleSettings",
                    "ID",
                    row["ID"].ToString(),
                    "SettingGuid",
                    Guid.NewGuid().ToString(),
                    " and SettingGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_Blogs",
                " where BlogGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_Blogs",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "BlogGuid",
                    Guid.NewGuid().ToString(),
                    " and BlogGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_CalendarEvents",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_CalendarEvents",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }


            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_GalleryImages",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_GalleryImages",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_HtmlContent",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_HtmlContent",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_Links",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_Links",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }


            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_SharedFileFolders",
                " where FolderGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_SharedFileFolders",
                    "FolderID",
                    row["FolderID"].ToString(),
                    "FolderGuid",
                    Guid.NewGuid().ToString(),
                    " and FolderGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_SharedFiles",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_SharedFiles",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }


        }

        public static void DatabaseHelperDoVersion2253PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            string connectionString;
            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connectionString = overrideConnectionInfo;
            }
            else
            {
                connectionString = GetWriteConnectionString();
            }

            DataTable dataTable = DatabaseHelperGetTable(
                connectionString,
                "cy_RssFeeds",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "cy_RssFeeds",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }

            


        }

        public static bool DatabaseHelperSitesTableExists()
        {
            bool result = false;
            //return DatabaseHelper_TableExists("`cy_Sites`");
            try
            {
                using (IDataReader reader = DBSiteSettings.GetSiteList())
                {
                    if (reader.Read())
                    {
                        reader.Close();
                    }
                }
                // no error yet it must exist
                result = true;
            }
            catch { }

            return result;
        }

        public static bool DatabaseHelperTableExists(string tableName)
        {
            using (MySqlConnection connection = new MySqlConnection(GetWriteConnectionString()))
            {
                string[] restrictions = new string[4];
                restrictions[2] = tableName;
                connection.Open();
                DataTable table = connection.GetSchema("Tables", restrictions);
                connection.Close();
                if (table != null)
                {
                    return (table.Rows.Count > 0);
                }
            }

            return false;
        }

        public static IDataReader DatabaseHelperGetApplicationId(string applicationName)
        {
            return DatabaseHelperGetReader(
                GetReadConnectionString(),
                "cy_SchemaVersion",
                " WHERE LCASE(ApplicationName) = '" + applicationName.ToLower() + "'");

        }


        #endregion

        

    }
}
