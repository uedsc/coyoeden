/// Author:					An Ngo
/// Created:				2009-09-10
/// Last Modified:			2009-09-17 Joe Audette
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace Cynthia.Data
{
   
    public static class DBSiteSettingsEx
    {
       
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];
        }

        //public static string GetSiteSettingsEx(int siteID, string keyName)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SiteSettingsEx_SelectOne", 2);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
        //    sph.DefineSqlParameter("@KeyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, keyName);
        //    string keyValue = Convert.ToString(sph.ExecuteScalar());
        //    return keyValue;
        //}

        public static IDataReader GetSiteSettingsExList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SiteSettingsEx_SelectAll", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

      
        public static void EnsureSettings()
        {
            new SqlParameterHelper(GetConnectionString(), "cy_SiteSettingsEx_EnsureDefinitions", 0).ExecuteNonQuery();
        }

        public static bool SaveExpandoProperty(
            int siteId,
            Guid siteGuid,
            string groupName,
            string keyName,
            string keyValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SiteSettingsEx_Save", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@KeyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, keyName);
            sph.DefineSqlParameter("@KeyValue", SqlDbType.NText, ParameterDirection.Input, keyValue);
            sph.DefineSqlParameter("@GroupName", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyName);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool UpdateRelatedSitesProperty(
            int siteId,
            string keyName,
            string keyValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SiteSettingsEx_UpdateRelated", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@KeyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, keyName);
            sph.DefineSqlParameter("@KeyValue", SqlDbType.NText, ParameterDirection.Input, keyValue);
            

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

       
    }
}
