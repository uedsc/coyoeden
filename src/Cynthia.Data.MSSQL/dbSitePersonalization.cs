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
    /// Last Modified:			2008-01-28
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
    public static class DBSitePersonalization
    {

       

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        
        public static void SavePersonalizationBlob(
            int siteId,
            String path,
            Guid userGuid,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SitePersonalizationPerUser_SetPageSettings", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Path", SqlDbType.NVarChar, 255, ParameterDirection.Input, path);
            sph.DefineSqlParameter("@UserId", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PageSettings", SqlDbType.Image, ParameterDirection.Input, dataBlob);
            sph.DefineSqlParameter("@LastUpdate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdateTime);
            sph.ExecuteNonQuery();
        }

        public static byte[] GetPersonalizationBlob(
            int siteId,
            String path,
            Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SitePersonalizationPerUser_GetPageSettings", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Path", SqlDbType.NVarChar, 255, ParameterDirection.Input, path);
            sph.DefineSqlParameter("@UserId", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            byte[] result = (byte[])sph.ExecuteScalar();
            return result;
        }

        public static void ResetPersonalizationBlob(
            int siteId,
            String path,
            Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SitePersonalizationPerUser_ResetPageSettings", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Path", SqlDbType.NVarChar, 255, ParameterDirection.Input, path);
            sph.DefineSqlParameter("@UserId", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.ExecuteNonQuery();
        }

        public static void ResetPersonalizationBlob(
            int siteId,
            String path)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE  ");
            sqlCommand.Append("FROM  cy_SitePersonalizationAllUsers ");
            sqlCommand.Append("WHERE PathID IN (   ");
            sqlCommand.Append("SELECT PathID    ");
            sqlCommand.Append("FROM cy_SitePaths    ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ");
            sqlCommand.Append(" )  ");

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), sqlCommand.ToString(), CommandType.Text, 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);

            sph.ExecuteNonQuery();
        }

        public static byte[] GetPersonalizationBlobAllUsers(
            int siteId,
            String path)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SitePersonalizationAllUsers_GetPageSettings", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Path", SqlDbType.NVarChar, 255, ParameterDirection.Input, path);

            byte[] result = (byte[])sph.ExecuteScalar();
            return result;
        }

        public static void SavePersonalizationBlobAllUsers(
            int siteId,
            String path,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SitePersonalizationAllUsers_SetPageSettings", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Path", SqlDbType.NVarChar, 255, ParameterDirection.Input, path);
            sph.DefineSqlParameter("@PageSettings", SqlDbType.Image, ParameterDirection.Input, dataBlob);
            sph.DefineSqlParameter("@LastUpdate", SqlDbType.DateTime, ParameterDirection.Input, lastUpdateTime);
            sph.ExecuteNonQuery();


        }

        public static int GetCountOfState(
            int siteId,
            String path,
            bool allUserScope,
            Guid userGuid,
            DateTime inactiveSince)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SitePersonalizationAdministration_GetCountOfState", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Path", SqlDbType.NVarChar, 255, ParameterDirection.Input, path);
            sph.DefineSqlParameter("@AllUsersScope", SqlDbType.Bit, ParameterDirection.Input, allUserScope);
            sph.DefineSqlParameter("@UserId", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@InactiveSinceDate", SqlDbType.DateTime, ParameterDirection.Input, inactiveSince);
            int result = Convert.ToInt32(sph.ExecuteScalar());
            return result;
        }


        //public static IDataReader FindState(
        //    int siteID,
        //    String path,
        //    bool allUserScope,
        //    Guid userGuid,
        //    DateTime inactiveSince,
        //    int pageIndex,
        //    int pageSize)
        //{
        //    SqlConnection Connection = GetConnection();
        //    SqlParameter[] arParams = new SxqlParameter[7];
        //    
        //            

        //        arParams[0].Value = siteID;
        //        arParams[1].Value = path;
        //        arParams[2].Value = allUserScope;
        //        arParams[3].Value = userGuid;
        //        arParams[4].Value = inactiveSince;
        //        arParams[5].Value = pageIndex;
        //        arParams[6].Value = pageSize;
        //    }
        //    {

        //        arParams[0] = new SxqlParameter("@SiteID", SqlDbType.Int);
        //        arParams[0].Direction = ParameterDirection.Input;
        //        arParams[0].Value = siteID;

        //        arParams[1] = new SxqlParameter("@Path", SqlDbType.NVarChar, 255);
        //        arParams[1].Direction = ParameterDirection.Input;
        //        arParams[1].Value = path;

        //        arParams[2] = new SxqlParameter("@AllUsersScope", SqlDbType.Bit);
        //        arParams[2].Direction = ParameterDirection.Input;
        //        arParams[2].Value = allUserScope;

        //        arParams[3] = new SxqlParameter("@UserId", SqlDbType.UniqueIdentifier);
        //        arParams[3].Direction = ParameterDirection.Input;
        //        arParams[3].Value = userGuid;

        //        arParams[4] = new SxqlParameter("@InactiveSinceDate", SqlDbType.DateTime);
        //        arParams[4].Direction = ParameterDirection.Input;
        //        arParams[4].Value = inactiveSince;

        //        arParams[5] = new SxqlParameter("@pageIndex", SqlDbType.Int);
        //        arParams[5].Direction = ParameterDirection.Input;
        //        arParams[5].Value = pageIndex;

        //        arParams[6] = new SxqlParameter("@pageSize", SqlDbType.Int);
        //        arParams[6].Direction = ParameterDirection.Input;
        //        arParams[6].Value = pageSize;

        //    }

        //    return sph.ExecuteReader();



        //}



        



    }
}
