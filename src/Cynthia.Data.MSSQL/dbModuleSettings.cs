using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace Cynthia.Data
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-11-03
    /// Last Modified:			2008-06-04
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// Note moved into separate class file from dbPortal 2007-11-03
    /// 
    /// </summary>
    public static class DBModuleSettings
    {
       
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        

        public static IDataReader GetModuleSettings(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleSettings_Select", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetDefaultModuleSettings(int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleDefinitionSettings_Select", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            return sph.ExecuteReader();
        }



        public static bool CreateModuleSetting(
            Guid settingGuid,
            Guid moduleGuid,
            int moduleId,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleSettings_Insert", 10);

            sph.DefineSqlParameter("@SettingGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, settingGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            sph.DefineSqlParameter("@SettingValue", SqlDbType.NVarChar, 255, ParameterDirection.Input, settingValue);
            sph.DefineSqlParameter("@ControlType", SqlDbType.NVarChar, 50, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@RegexValidationExpression", SqlDbType.NText, ParameterDirection.Input, regexValidationExpression);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@HelpKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateModuleSetting(
            Guid moduleGuid,
            int moduleId, 
            string settingName, 
            string settingValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleSettings_Update", 4);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            sph.DefineSqlParameter("@SettingValue", SqlDbType.NVarChar, 255, ParameterDirection.Input, settingValue);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool CreateDefaultModuleSettings(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleSettings_CreateDefaultSettings", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteModuleSettings(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_ModuleSettings_Delete", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        


    }
}
