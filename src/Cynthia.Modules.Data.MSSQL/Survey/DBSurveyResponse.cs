using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using Cynthia.Data;

namespace SurveyFeature.Data
{
    /// <summary>
    /// Author:				Rob Henry
    /// Created:			2007-08-31
    /// Last Modified:		2008-01-21
    /// 
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public static class DBSurveyResponse
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBSurveyResponse));


        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        public static String DBPlatform()
        {
            return "MSSQL";
        }

        /// <summary>
        /// Inserts a row in the cy_SurveyResponses table. Returns rows affected count.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="userId"> userId </param>
        /// <param name="submissionDate"> submissionDate </param>
        /// <param name="annonymous"> annonymous </param>
        /// <param name="complete"> complete </param>
        /// <returns>int</returns>
        public static int Add(
            Guid responseGuid,
            Guid surveyGuid,
            Guid userGuid,
            bool annonymous,
            bool complete)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_Insert", 5);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Annonymous", SqlDbType.Bit, ParameterDirection.Input, annonymous);
            sph.DefineSqlParameter("@Complete", SqlDbType.Bit, ParameterDirection.Input, complete);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        /// <summary>
        /// Updates the status of a response. Returns true if row updated.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="complete"> complete </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid responseGuid,
            DateTime submissionDate,
            bool complete)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_Update", 3);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            sph.DefineSqlParameter("@SubmissionDate", SqlDbType.DateTime, ParameterDirection.Input, submissionDate);
            sph.DefineSqlParameter("@Complete", SqlDbType.Bit, ParameterDirection.Input, complete);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the cy_SurveyResponses table. Returns true if row deleted.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid responseGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_Delete", 1);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the cy_SurveyResponses table.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        public static IDataReader GetOne(
            Guid responseGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_SelectOne", 1);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the cy_SurveyResponses table.
        /// </summary>
        public static IDataReader GetAll(Guid surveyGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_SelectAll", 1);
            sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets an IDataReader with the first response to a survey
        /// </summary>
        public static IDataReader GetFirst(Guid surveyGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_GetFirst", 1);
            sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);
            return sph.ExecuteReader();
        }


        /// <summary>
        /// Gets an IDataReader with the next response to a survey
        /// </summary>
        public static IDataReader GetNext(Guid responseGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_GetNext", 1);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets an IDataReader with the next response to a survey
        /// </summary>
        public static IDataReader GetPrevious(Guid responseGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyResponses_GetPrevious", 1);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            return sph.ExecuteReader();
        }


    }
}
