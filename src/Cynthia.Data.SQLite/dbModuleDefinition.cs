/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2010-03-17
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
using System.Web;
using Mono.Data.Sqlite;

namespace Cynthia.Data
{
    public static class DBModuleDefinition
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


        


        public static int AddModuleDefinition(
            Guid featureGuid,
            int siteId,
            string featureName,
            string controlSrc,
            int sortOrder,
            int defaultCacheTime,
            String icon,
            bool isAdmin,
            string resourceFile,
            bool isCacheable,
            bool isSearchable,
            string searchListName,
            bool supportsPageReuse,
            string deleteProvider)
        {

            int intIsAdmin = 0;
            if (isAdmin) { intIsAdmin = 1; }

            int intIsCacheable = 0;
            if (isCacheable) { intIsCacheable = 1; }

            int intIsSearchable = 0;
            if (isSearchable) { intIsSearchable = 1; }

            int intSupportsPageReuse = 0;
            if (supportsPageReuse) { intSupportsPageReuse = 1; }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_ModuleDefinitions (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("FeatureName, ");
            sqlCommand.Append("ControlSrc, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("DefaultCacheTime, ");
            sqlCommand.Append("Icon, ");
            sqlCommand.Append("IsAdmin, ");
            sqlCommand.Append("IsCacheable, ");
            sqlCommand.Append("IsSearchable, ");
            sqlCommand.Append("SearchListName, ");
            sqlCommand.Append("SupportsPageReuse, ");
            sqlCommand.Append("DeleteProvider, ");
            sqlCommand.Append("ResourceFile ");
            sqlCommand.Append(" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":FeatureGuid, ");
            sqlCommand.Append(":FeatureName, ");
            sqlCommand.Append(":ControlSrc, ");
            sqlCommand.Append(":SortOrder, ");
            sqlCommand.Append(":DefaultCacheTime, ");
            sqlCommand.Append(":Icon, ");
            sqlCommand.Append(":IsAdmin, ");
            sqlCommand.Append(":IsCacheable, ");
            sqlCommand.Append(":IsSearchable, ");
            sqlCommand.Append(":SearchListName, ");
            sqlCommand.Append(":SupportsPageReuse, ");
            sqlCommand.Append(":DeleteProvider, ");
            sqlCommand.Append(":ResourceFile ");
            sqlCommand.Append(" );");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[14];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":FeatureName", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureName;

            arParams[2] = new SqliteParameter(":ControlSrc", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = controlSrc;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":IsAdmin", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intIsAdmin;

            arParams[5] = new SqliteParameter(":Icon", DbType.String, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = icon;

            arParams[6] = new SqliteParameter(":DefaultCacheTime", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = defaultCacheTime;

            arParams[7] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = featureGuid;

            arParams[8] = new SqliteParameter(":ResourceFile", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = resourceFile;

            arParams[9] = new SqliteParameter(":IsCacheable", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsCacheable;

            arParams[10] = new SqliteParameter(":IsSearchable", DbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intIsSearchable;

            arParams[11] = new SqliteParameter(":SearchListName", DbType.String, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = searchListName;

            arParams[12] = new SqliteParameter(":SupportsPageReuse", DbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intSupportsPageReuse;

            arParams[13] = new SqliteParameter(":DeleteProvider", DbType.String, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = deleteProvider;


            int newID = Convert.ToInt32(
                SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            if (siteId > -1)
            {
                // now add to  cy_SiteModuleDefinitions
                sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO cy_SiteModuleDefinitions (");
                sqlCommand.Append("SiteID, ");
                sqlCommand.Append("SiteGuid, ");
                sqlCommand.Append("FeatureGuid, ");
                sqlCommand.Append("ModuleDefID ) ");

                sqlCommand.Append(" VALUES (");
                sqlCommand.Append(":SiteID, ");
                sqlCommand.Append("(SELECT SiteGuid FROM cy_Sites WHERE SiteID = :SiteID LIMIT 1), ");
                sqlCommand.Append("(SELECT Guid FROM cy_ModuleDefinitions WHERE ModuleDefID = :ModuleDefID LIMIT 1), ");
                sqlCommand.Append(":ModuleDefID ) ; ");

                arParams = new SqliteParameter[2];

                arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = siteId;

                arParams[1] = new SqliteParameter(":ModuleDefID", DbType.Int32);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = newID;

                SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

            }

            return newID;

        }


        public static bool UpdateModuleDefinition(
            int moduleDefId,
            string featureName,
            string controlSrc,
            int sortOrder,
            int defaultCacheTime,
            String icon,
            bool isAdmin,
            string resourceFile, 
            bool isCacheable,
            bool isSearchable,
            string searchListName,
            bool supportsPageReuse,
            string deleteProvider)
        {

            int intIsAdmin = 0;
            if (isAdmin) { intIsAdmin = 1; }

            int intIsCacheable = 0;
            if (isCacheable) { intIsCacheable = 1; }

            int intIsSearchable = 0;
            if (isSearchable) { intIsSearchable = 1; }

            int intSupportsPageReuse = 0;
            if (supportsPageReuse) { intSupportsPageReuse = 1; }


            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ModuleDefinitions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FeatureName = :FeatureName, ");
            sqlCommand.Append("ControlSrc = :ControlSrc, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("DefaultCacheTime = :DefaultCacheTime, ");
            sqlCommand.Append("Icon = :Icon, ");
            sqlCommand.Append("IsAdmin = :IsAdmin, ");
            sqlCommand.Append("IsCacheable = :IsCacheable, ");
            sqlCommand.Append("IsSearchable = :IsSearchable, ");
            sqlCommand.Append("SearchListName = :SearchListName, ");
            sqlCommand.Append("SupportsPageReuse = :SupportsPageReuse, ");
            sqlCommand.Append("DeleteProvider = :DeleteProvider, ");
            sqlCommand.Append("ResourceFile = :ResourceFile ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleDefID = :ModuleDefID ;");

            SqliteParameter[] arParams = new SqliteParameter[13];

            arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            arParams[1] = new SqliteParameter(":FeatureName", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureName;

            arParams[2] = new SqliteParameter(":ControlSrc", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = controlSrc;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":IsAdmin", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intIsAdmin;

            arParams[5] = new SqliteParameter(":Icon", DbType.String, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = icon;

            arParams[6] = new SqliteParameter(":DefaultCacheTime", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = defaultCacheTime;

            arParams[7] = new SqliteParameter(":ResourceFile", DbType.String, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = resourceFile;

            arParams[8] = new SqliteParameter(":IsCacheable", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intIsCacheable;

            arParams[9] = new SqliteParameter(":IsSearchable", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsSearchable;

            arParams[10] = new SqliteParameter(":SearchListName", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = searchListName;

            arParams[11] = new SqliteParameter(":SupportsPageReuse", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intSupportsPageReuse;

            arParams[12] = new SqliteParameter(":DeleteProvider", DbType.String, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deleteProvider;

            int rowsAffected = -1;

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteModuleDefinition(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = :ModuleDefID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_SiteModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = :ModuleDefID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetModuleDefinition(
            int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = :ModuleDefID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetModuleDefinition(
            Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :FeatureGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static void EnsureInstallationInAdminSites()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_SiteModuleDefinitions ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleDefID ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("md.Guid, ");
            sqlCommand.Append("md.ModuleDefID ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("cy_Sites s, ");
            sqlCommand.Append("cy_ModuleDefinitions md ");
            sqlCommand.Append("WHERE s.IsServerAdminSite = 1 ");
            sqlCommand.Append("AND md.ModuleDefID NOT IN ");
            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT sd.ModuleDefID ");
            sqlCommand.Append("FROM cy_SiteModuleDefinitions sd ");
            sqlCommand.Append("WHERE sd.SiteID = s.SiteID ");
            sqlCommand.Append(") ");
            sqlCommand.Append(" ;");

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	cy_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteGuid = :SiteGuid ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetModuleDefinitions(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	cy_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteID = :SiteID ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	cy_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteGuid = :SiteGuid ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleDefID", typeof(int));
            dt.Columns.Add("FeatureName", typeof(String));
            dt.Columns.Add("ControlSrc", typeof(String));

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ModuleDefID"] = reader["ModuleDefID"];
                    row["FeatureName"] = reader["FeatureName"];
                    row["ControlSrc"] = reader["ControlSrc"];
                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        public static DataTable GetModuleDefinitionsBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	cy_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteID = :SiteID ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleDefID", typeof(int));
            dt.Columns.Add("FeatureName", typeof(String));
            dt.Columns.Add("ControlSrc", typeof(String));

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ModuleDefID"] = reader["ModuleDefID"];
                    row["FeatureName"] = reader["FeatureName"];
                    row["ControlSrc"] = reader["ControlSrc"];
                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        public static IDataReader GetUserModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	cy_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteID = :SiteID AND md.IsAdmin = 0 ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSearchableModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	cy_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	cy_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteID = :SiteID AND md.IsAdmin = 0 AND md.IsSearchable = 1 ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.SearchListName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        public static void SyncDefinitions()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ModuleSettings ");
            sqlCommand.Append("SET ControlSrc = (SELECT mds.ControlSrc ");
            sqlCommand.Append("FROM cy_ModuleDefinitionSettings mds  ");
            sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
            sqlCommand.Append("FROM cy_Modules m ");
            sqlCommand.Append("WHERE m.ModuleID = cy_ModuleSettings.ModuleID) ");
            sqlCommand.Append("AND mds.SettingName = cy_ModuleSettings.SettingName LIMIT 1 ) ");
            //sqlCommand.Append(" ");
            sqlCommand.Append("; ");

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ModuleSettings ");
            sqlCommand.Append("SET ControlType = (SELECT  mds.ControlType ");
            sqlCommand.Append("FROM cy_ModuleDefinitionSettings mds  ");
            sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
            sqlCommand.Append("FROM cy_Modules m ");
            sqlCommand.Append("WHERE m.ModuleID = cy_ModuleSettings.ModuleID) ");
            sqlCommand.Append("AND mds.SettingName = cy_ModuleSettings.SettingName LIMIT 1 ) ");

            sqlCommand.Append("; ");

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ModuleSettings ");
            sqlCommand.Append("SET SortOrder = (SELECT mds.SortOrder ");
            sqlCommand.Append("FROM cy_ModuleDefinitionSettings mds  ");
            sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
            sqlCommand.Append("FROM cy_Modules m ");
            sqlCommand.Append("WHERE m.ModuleID = cy_ModuleSettings.ModuleID) ");
            sqlCommand.Append("AND mds.SettingName = cy_ModuleSettings.SettingName LIMIT 1 ); ");
            sqlCommand.Append(" ");

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ModuleSettings ");
            sqlCommand.Append("SET HelpKey = (SELECT mds.HelpKey ");
            sqlCommand.Append("FROM cy_ModuleDefinitionSettings mds  ");
            sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
            sqlCommand.Append("FROM cy_Modules m ");
            sqlCommand.Append("WHERE m.ModuleID = cy_ModuleSettings.ModuleID) ");
            sqlCommand.Append("AND mds.SettingName = cy_ModuleSettings.SettingName LIMIT 1 ); ");
            sqlCommand.Append(" ");

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_ModuleSettings ");
            sqlCommand.Append("SET RegexValidationExpression = (SELECT mds.RegexValidationExpression ");
            sqlCommand.Append("FROM cy_ModuleDefinitionSettings mds  ");
            sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
            sqlCommand.Append("FROM cy_Modules m ");
            sqlCommand.Append("WHERE m.ModuleID = cy_ModuleSettings.ModuleID) ");
            sqlCommand.Append("AND mds.SettingName = cy_ModuleSettings.SettingName LIMIT 1 ); ");
            sqlCommand.Append(" ");

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);



        }

        public static bool UpdateModuleDefinitionSetting(
            Guid featureGuid,
            int moduleDefId,
            string resourceFile,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*)");
            sqlCommand.Append("FROM	cy_ModuleDefinitionSettings ");

            sqlCommand.Append("WHERE (ModuleDefID = :ModuleDefID OR FeatureGuid = :FeatureGuid)  ");
            sqlCommand.Append("AND SettingName = :SettingName  ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            arParams[1] = new SqliteParameter(":SettingName", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid;


            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();

            int rowsAffected = 0;

            if (count > 0)
            {
                sqlCommand.Append("UPDATE cy_ModuleDefinitionSettings ");
                sqlCommand.Append("SET SettingValue = :SettingValue  ,");
                sqlCommand.Append("FeatureGuid = :FeatureGuid,  ");
                sqlCommand.Append("ResourceFile = :ResourceFile,  ");
                sqlCommand.Append("ControlType = :ControlType  ,");
                sqlCommand.Append("ControlSrc = :ControlSrc  ,");
                sqlCommand.Append("HelpKey = :HelpKey  ,");
                sqlCommand.Append("SortOrder = :SortOrder  ,");
                sqlCommand.Append("RegexValidationExpression = :RegexValidationExpression  ");

                sqlCommand.Append("WHERE ModuleDefID = :ModuleDefID  ");
                sqlCommand.Append("AND SettingName = :SettingName  ; ");

                arParams = new SqliteParameter[10];

                arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleDefId;

                arParams[1] = new SqliteParameter(":SettingName", DbType.String, 50);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = settingName;

                arParams[2] = new SqliteParameter(":SettingValue", DbType.String, 255);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = settingValue;

                arParams[3] = new SqliteParameter(":ControlType", DbType.String, 50);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = controlType;

                arParams[4] = new SqliteParameter(":RegexValidationExpression", DbType.Object);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = regexValidationExpression;

                arParams[5] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = featureGuid;

                arParams[6] = new SqliteParameter(":ResourceFile", DbType.String, 255);
                arParams[6].Direction = ParameterDirection.Input;
                arParams[6].Value = resourceFile;

                arParams[7] = new SqliteParameter(":ControlSrc", DbType.String, 255);
                arParams[7].Direction = ParameterDirection.Input;
                arParams[7].Value = controlSrc;

                arParams[8] = new SqliteParameter(":HelpKey", DbType.String, 255);
                arParams[8].Direction = ParameterDirection.Input;
                arParams[8].Value = helpKey;

                arParams[9] = new SqliteParameter(":SortOrder", DbType.Int32);
                arParams[9].Direction = ParameterDirection.Input;
                arParams[9].Value = sortOrder;

                rowsAffected = SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

                return (rowsAffected > 0);

            }
            else
            {
                sqlCommand.Append("INSERT INTO cy_ModuleDefinitionSettings ");
                sqlCommand.Append("( ");
                sqlCommand.Append("FeatureGuid, ");
                sqlCommand.Append("ModuleDefID, ");
                sqlCommand.Append("ResourceFile, ");
                sqlCommand.Append("SettingName, ");
                sqlCommand.Append("SettingValue, ");
                sqlCommand.Append("ControlType, ");
                sqlCommand.Append("ControlSrc, ");
                sqlCommand.Append("HelpKey, ");
                sqlCommand.Append("SortOrder, ");
                sqlCommand.Append("RegexValidationExpression");
                sqlCommand.Append(")");

                sqlCommand.Append("VALUES (  ");
                sqlCommand.Append(" :FeatureGuid  , ");
                sqlCommand.Append(" :ModuleDefID , ");
                sqlCommand.Append(" :ResourceFile  , ");
                sqlCommand.Append(" :SettingName  , ");
                sqlCommand.Append(" :SettingValue  ,");
                sqlCommand.Append(" :ControlType  ,");
                sqlCommand.Append(" :ControlSrc, ");
                sqlCommand.Append(" :HelpKey, ");
                sqlCommand.Append(" :SortOrder, ");
                sqlCommand.Append(" :RegexValidationExpression  ");
                sqlCommand.Append(");");

                arParams = new SqliteParameter[10];

                arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleDefId;

                arParams[1] = new SqliteParameter(":SettingName", DbType.String, 50);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = settingName;

                arParams[2] = new SqliteParameter(":SettingValue", DbType.String, 255);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = settingValue;

                arParams[3] = new SqliteParameter(":ControlType", DbType.String, 50);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = controlType;

                arParams[4] = new SqliteParameter(":RegexValidationExpression", DbType.Object);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = regexValidationExpression;

                arParams[5] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = featureGuid;

                arParams[6] = new SqliteParameter(":ResourceFile", DbType.String, 255);
                arParams[6].Direction = ParameterDirection.Input;
                arParams[6].Value = resourceFile;

                arParams[7] = new SqliteParameter(":ControlSrc", DbType.String, 255);
                arParams[7].Direction = ParameterDirection.Input;
                arParams[7].Value = controlSrc;

                arParams[8] = new SqliteParameter(":HelpKey", DbType.String, 255);
                arParams[8].Direction = ParameterDirection.Input;
                arParams[8].Value = helpKey;

                arParams[9] = new SqliteParameter(":SortOrder", DbType.Int32);
                arParams[9].Direction = ParameterDirection.Input;
                arParams[9].Value = sortOrder;

                rowsAffected = SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

                return (rowsAffected > 0);

            }

        }

        public static bool UpdateModuleDefinitionSettingById(
            int id,
            int moduleDefId,
            string resourceFile,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_ModuleDefinitionSettings ");
            sqlCommand.Append("SET SettingName = :SettingName,  ");
            sqlCommand.Append("ResourceFile = :ResourceFile,  ");
            sqlCommand.Append("SettingValue = :SettingValue,  ");
            sqlCommand.Append("ControlType = :ControlType,  ");
            sqlCommand.Append("ControlSrc = :ControlSrc  ,");
            sqlCommand.Append("HelpKey = :HelpKey  ,");
            sqlCommand.Append("SortOrder = :SortOrder  ,");
            sqlCommand.Append("RegexValidationExpression = :RegexValidationExpression  ");

            sqlCommand.Append("WHERE ID = :ID  ");
            sqlCommand.Append("AND ModuleDefID = :ModuleDefID  ; ");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":ID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            arParams[1] = new SqliteParameter(":ModuleDefID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqliteParameter(":SettingName", DbType.String, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingName;

            arParams[3] = new SqliteParameter(":SettingValue", DbType.String, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = settingValue;

            arParams[4] = new SqliteParameter(":ControlType", DbType.String, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = controlType;

            arParams[5] = new SqliteParameter(":RegexValidationExpression", DbType.Object);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = regexValidationExpression;

            arParams[6] = new SqliteParameter(":ResourceFile", DbType.String, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = resourceFile;

            arParams[7] = new SqliteParameter(":ControlSrc", DbType.String, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new SqliteParameter(":HelpKey", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = helpKey;

            arParams[9] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = sortOrder;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteSettingById(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_ModuleDefinitionSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = :ID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader ModuleDefinitionSettingsGetSetting(
            Guid featureGuid,
            string settingName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_ModuleDefinitionSettings ");

            sqlCommand.Append("WHERE FeatureGuid = :FeatureGuid  ");
            sqlCommand.Append("AND SettingName = :SettingName ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            arParams[1] = new SqliteParameter(":SettingName", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        

    }
}
