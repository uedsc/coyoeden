/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2009-05-23
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

namespace Cynthia.Data
{
    public static class DBModuleDefinition
    {

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

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
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_Insert", 14);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FeatureName", SqlDbType.NVarChar, 255, ParameterDirection.Input, featureName);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 255, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@DefaultCacheTime", SqlDbType.Int, ParameterDirection.Input, defaultCacheTime);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@IsCacheable", SqlDbType.Bit, ParameterDirection.Input, isCacheable);
            sph.DefineSqlParameter("@IsSearchable", SqlDbType.Bit, ParameterDirection.Input, isSearchable);
            sph.DefineSqlParameter("@SearchListName", SqlDbType.NVarChar, 255, ParameterDirection.Input, searchListName);
            sph.DefineSqlParameter("@SupportsPageReuse", SqlDbType.Bit, ParameterDirection.Input, supportsPageReuse);
            sph.DefineSqlParameter("@DeleteProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, deleteProvider);


            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateModuleDefinition(
            int moduleDefId,
            string featureName,
            string controlSrc,
            int sortOrder,
            int defaultCacheTime,
            string icon,
            bool isAdmin,
            string resourceFile,
            bool isCacheable,
            bool isSearchable,
            string searchListName,
            bool supportsPageReuse,
            string deleteProvider)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_Update", 13);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@FeatureName", SqlDbType.NVarChar, 255, ParameterDirection.Input, featureName);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 255, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@DefaultCacheTime", SqlDbType.Int, ParameterDirection.Input, defaultCacheTime);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@IsCacheable", SqlDbType.Bit, ParameterDirection.Input, isCacheable);
            sph.DefineSqlParameter("@IsSearchable", SqlDbType.Bit, ParameterDirection.Input, isSearchable);
            sph.DefineSqlParameter("@SearchListName", SqlDbType.NVarChar, 255, ParameterDirection.Input, searchListName);
            sph.DefineSqlParameter("@SupportsPageReuse", SqlDbType.Bit, ParameterDirection.Input, supportsPageReuse);
            sph.DefineSqlParameter("@DeleteProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, deleteProvider);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteModuleDefinition(int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_Delete", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SiteModuleDefinitions_DeleteByFeature", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteSettingById(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitionSettings_DeleteByID", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetModuleDefinition(
                int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_SelectOne", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetModuleDefinition(
            Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_SelectOneByGuid", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            return sph.ExecuteReader();
        }

        public static void EnsureInstallationInAdminSites()
        {
            new SqlParameterHelper(GetConnectionString(), "cy_SiteModuleDefinitions_EnsureForAdminSites", 0).ExecuteNonQuery();
        }

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_Select", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_Select", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            
            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleDefID", typeof(int));
            dt.Columns.Add("FeatureGuid", typeof(String));
            dt.Columns.Add("FeatureName", typeof(String));
            dt.Columns.Add("ControlSrc", typeof(String));

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ModuleDefID"] = reader["ModuleDefID"];
                    row["FeatureGuid"] = reader["Guid"].ToString();
                    row["FeatureName"] = reader["FeatureName"];
                    row["ControlSrc"] = reader["ControlSrc"];
                    dt.Rows.Add(row);
                }

            }
            return dt;
        }

        public static IDataReader GetUserModules(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_SelectUserModules", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSearchableModules(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitions_SelectSearchableModules", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static void SyncDefinitions()
        {
            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "cy_ModuleSettings_SyncDefinitions");
            
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
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitionSettings_Update", 10);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            sph.DefineSqlParameter("@SettingValue", SqlDbType.NVarChar, 255, ParameterDirection.Input, settingValue);
            sph.DefineSqlParameter("@ControlType", SqlDbType.NVarChar, 50, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@RegexValidationExpression", SqlDbType.NText, ParameterDirection.Input, regexValidationExpression);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@HelpKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
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
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitionSettings_UpdateByID", 10);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            sph.DefineSqlParameter("@SettingValue", SqlDbType.NVarChar, 255, ParameterDirection.Input, settingValue);
            sph.DefineSqlParameter("@ControlType", SqlDbType.NVarChar, 50, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@RegexValidationExpression", SqlDbType.NText, ParameterDirection.Input, regexValidationExpression);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@HelpKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static IDataReader ModuleDefinitionSettingsGetSetting(
            Guid featureGuid,
            string settingName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitionSettings_SelectOne", 2);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            return sph.ExecuteReader();
        }

        

    }
}
