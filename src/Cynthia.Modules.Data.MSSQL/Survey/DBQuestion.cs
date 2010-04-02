/// Author:				Rob Henry
/// Created:			2007-08-31
/// Last Modified:		2009-10-24
/// 
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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using Cynthia.Data;

namespace SurveyFeature.Data
{
    
    public static class DBQuestion
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBQuestion));


        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        public static String DBPlatform()
        {
            return "MSSQL";
        }

        public static int Add(
            Guid questionGuid,
            Guid surveyPageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            string validationMessage)
        {

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyQuestions_Insert", 6);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyPageGuid);
            sph.DefineSqlParameter("@QuestionText", SqlDbType.NText, ParameterDirection.Input, questionText);
            sph.DefineSqlParameter("@QuestionTypeId", SqlDbType.Int, ParameterDirection.Input, questionTypeId);
            sph.DefineSqlParameter("@AnswerIsRequired", SqlDbType.Bit, ParameterDirection.Input, answerIsRequired);
            sph.DefineSqlParameter("@ValidationMessage", SqlDbType.NVarChar, 255, ParameterDirection.Input, validationMessage);
            int rowsAffected = sph.ExecuteNonQuery();

            return rowsAffected;

        }


        public static bool Update(
            Guid questionGuid,
            Guid surveyPageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            int questionOrder,
            string validationMessage)
        {

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyQuestions_Update", 7);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyPageGuid);
            sph.DefineSqlParameter("@QuestionText", SqlDbType.NText, ParameterDirection.Input, questionText);
            sph.DefineSqlParameter("@QuestionTypeId", SqlDbType.Int, ParameterDirection.Input, questionTypeId);
            sph.DefineSqlParameter("@AnswerIsRequired", SqlDbType.Bit, ParameterDirection.Input, answerIsRequired);
            sph.DefineSqlParameter("@QuestionOrder", SqlDbType.Int, ParameterDirection.Input, questionOrder);
            sph.DefineSqlParameter("@ValidationMessage", SqlDbType.NVarChar, 255, ParameterDirection.Input, validationMessage);
            int rowsAffected = sph.ExecuteNonQuery();

            return (rowsAffected > -1);

        }

        public static bool Delete(
            Guid questionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyQuestions_Delete", 1);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);

            int rowsAffected = sph.ExecuteNonQuery();

            return (rowsAffected > -1);

        }

        public static IDataReader GetOne(
            Guid questionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyQuestions_SelectOne", 1);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);

            return sph.ExecuteReader();
        }

        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "cy_SurveyQuestions_GetCount",
                null));

        }

        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "cy_SurveyQuestions_SelectAll",
                null);

        }

        public static IDataReader GetAllByPage(Guid pageQuestionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SurveyQuestions_SelectAllByPage", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageQuestionGuid);

            return sph.ExecuteReader();
        }	

    }
}
