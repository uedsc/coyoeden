/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2009-12-09
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

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace Cynthia.Data
{
   
    public static class DBPageSettings
    {
       
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        


        public static int Create(
            int siteId,
            int parentId,
            string pageName,
            string pageTitle,
            string skin,
            int pageOrder,
            string authorizedRoles,
            string editRoles,
            string draftEditRoles,
            string createChildPageRoles,
            string createChildDraftRoles,
            bool requireSsl,
            bool allowBrowserCache,
            bool showBreadcrumbs,
            bool showChildPageBreadcrumbs,
            string pageKeyWords,
            string pageDescription,
            string pageEncoding,
            string additionalMetaTags,
            bool useUrl,
            string url,
            bool openInNewWindow,
            bool showChildPageMenu,
            bool hideMainMenu,
            bool includeInMenu,
            String menuImage,
            string changeFrequency,
            string siteMapPriority,
            Guid pageGuid,
            Guid parentGuid,
            bool hideAfterLogin,
            Guid siteGuid,
            string compiledMeta,
            DateTime compiledMetaUtc,
            bool includeInSiteMap,
            bool isClickable,
            bool showHomeCrumb,
            bool isPending,
            string canonicalOverride,
            bool includeInSearchMap,
            bool enableComments)
        {

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_Insert", 42);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@PageName", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageName);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            sph.DefineSqlParameter("@AuthorizedRoles", SqlDbType.NText, ParameterDirection.Input, authorizedRoles);
            sph.DefineSqlParameter("@EditRoles", SqlDbType.NText, ParameterDirection.Input, editRoles);
            sph.DefineSqlParameter("@DraftEditRoles", SqlDbType.NText, ParameterDirection.Input, draftEditRoles);
            sph.DefineSqlParameter("@CreateChildPageRoles", SqlDbType.NText, ParameterDirection.Input, createChildPageRoles);
            sph.DefineSqlParameter("@CreateChildDraftRoles", SqlDbType.NText, ParameterDirection.Input, createChildDraftRoles);
            sph.DefineSqlParameter("@RequireSSL", SqlDbType.Bit, ParameterDirection.Input, requireSsl);
            sph.DefineSqlParameter("@ShowBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showBreadcrumbs);
            sph.DefineSqlParameter("@ShowChildPageBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showChildPageBreadcrumbs);
            sph.DefineSqlParameter("@PageKeyWords", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageKeyWords);
            sph.DefineSqlParameter("@PageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageDescription);
            sph.DefineSqlParameter("@PageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageEncoding);
            sph.DefineSqlParameter("@AdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, additionalMetaTags);
            sph.DefineSqlParameter("@UseUrl", SqlDbType.Bit, ParameterDirection.Input, useUrl);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@OpenInNewWindow", SqlDbType.Bit, ParameterDirection.Input, openInNewWindow);
            sph.DefineSqlParameter("@ShowChildPageMenu", SqlDbType.Bit, ParameterDirection.Input, showChildPageMenu);
            sph.DefineSqlParameter("@HideMainMenu", SqlDbType.Bit, ParameterDirection.Input, hideMainMenu);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@IncludeInMenu", SqlDbType.Bit, ParameterDirection.Input, includeInMenu);
            sph.DefineSqlParameter("@MenuImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, menuImage);
            sph.DefineSqlParameter("@PageTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitle);
            sph.DefineSqlParameter("@AllowBrowserCache", SqlDbType.Bit, ParameterDirection.Input, allowBrowserCache);
            sph.DefineSqlParameter("@ChangeFrequency", SqlDbType.NVarChar, 20, ParameterDirection.Input, changeFrequency);
            sph.DefineSqlParameter("@SiteMapPriority", SqlDbType.NVarChar, 10, ParameterDirection.Input, siteMapPriority);
            sph.DefineSqlParameter("@LastModifiedUTC", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            sph.DefineSqlParameter("@HideAfterLogin", SqlDbType.Bit, ParameterDirection.Input, hideAfterLogin);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NText, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@CompiledMetaUtc", SqlDbType.DateTime, ParameterDirection.Input, compiledMetaUtc);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@IsClickable", SqlDbType.Bit, ParameterDirection.Input, isClickable);
            sph.DefineSqlParameter("@ShowHomeCrumb", SqlDbType.Bit, ParameterDirection.Input, showHomeCrumb);
            sph.DefineSqlParameter("@IsPending", SqlDbType.Bit, ParameterDirection.Input, isPending);
            sph.DefineSqlParameter("@CanonicalOverride", SqlDbType.NVarChar, 255, ParameterDirection.Input, canonicalOverride);
            sph.DefineSqlParameter("@IncludeInSearchMap", SqlDbType.Bit, ParameterDirection.Input, includeInSearchMap);
            sph.DefineSqlParameter("@EnableComments", SqlDbType.Bit, ParameterDirection.Input, enableComments);

            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdatePage(
            int siteId,
            int pageId,
            int parentId,
            string pageName,
            string pageTitle,
            string skin,
            int pageOrder,
            string authorizedRoles,
            string editRoles,
            string draftEditRoles,
            string createChildPageRoles,
            string createChildDraftRoles,
            bool requireSsl,
            bool allowBrowserCache,
            bool showBreadcrumbs,
            bool showChildPageBreadcrumbs,
            string pageKeyWords,
            string pageDescription,
            string pageEncoding,
            string additionalMetaTags,
            bool useUrl,
            string url,
            bool openInNewWindow,
            bool showChildPageMenu,
            bool hideMainMenu,
            bool includeInMenu,
            String menuImage,
            string changeFrequency,
            string siteMapPriority,
            Guid parentGuid,
            bool hideAfterLogin,
            string compiledMeta,
            DateTime compiledMetaUtc,
            bool includeInSiteMap,
            bool isClickable,
            bool showHomeCrumb,
            bool isPending,
            string canonicalOverride,
            bool includeInSearchMap,
            bool enableComments)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_Update", 41);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            sph.DefineSqlParameter("@PageName", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageName);
            sph.DefineSqlParameter("@AuthorizedRoles", SqlDbType.NText, ParameterDirection.Input, authorizedRoles);
            sph.DefineSqlParameter("@EditRoles", SqlDbType.NText, ParameterDirection.Input, editRoles);
            sph.DefineSqlParameter("@DraftEditRoles", SqlDbType.NText, ParameterDirection.Input, draftEditRoles);
            sph.DefineSqlParameter("@CreateChildPageRoles", SqlDbType.NText, ParameterDirection.Input, createChildPageRoles);
            sph.DefineSqlParameter("@CreateChildDraftRoles", SqlDbType.NText, ParameterDirection.Input, createChildDraftRoles);
            sph.DefineSqlParameter("@RequireSSL", SqlDbType.Bit, ParameterDirection.Input, requireSsl);
            sph.DefineSqlParameter("@ShowBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showBreadcrumbs);
            sph.DefineSqlParameter("@ShowChildPageBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showChildPageBreadcrumbs);
            sph.DefineSqlParameter("@PageKeyWords", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageKeyWords);
            sph.DefineSqlParameter("@PageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageDescription);
            sph.DefineSqlParameter("@PageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageEncoding);
            sph.DefineSqlParameter("@AdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, additionalMetaTags);
            sph.DefineSqlParameter("@UseUrl", SqlDbType.Bit, ParameterDirection.Input, useUrl);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@OpenInNewWindow", SqlDbType.Bit, ParameterDirection.Input, openInNewWindow);
            sph.DefineSqlParameter("@ShowChildPageMenu", SqlDbType.Bit, ParameterDirection.Input, showChildPageMenu);
            sph.DefineSqlParameter("@HideMainMenu", SqlDbType.Bit, ParameterDirection.Input, hideMainMenu);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@IncludeInMenu", SqlDbType.Bit, ParameterDirection.Input, includeInMenu);
            sph.DefineSqlParameter("@MenuImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, menuImage);
            sph.DefineSqlParameter("@PageTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitle);
            sph.DefineSqlParameter("@AllowBrowserCache", SqlDbType.Bit, ParameterDirection.Input, allowBrowserCache);
            sph.DefineSqlParameter("@ChangeFrequency", SqlDbType.NVarChar, 20, ParameterDirection.Input, changeFrequency);
            sph.DefineSqlParameter("@SiteMapPriority", SqlDbType.NVarChar, 10, ParameterDirection.Input, siteMapPriority);
            sph.DefineSqlParameter("@LastModifiedUTC", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            sph.DefineSqlParameter("@HideAfterLogin", SqlDbType.Bit, ParameterDirection.Input, hideAfterLogin);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NText, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@CompiledMetaUtc", SqlDbType.DateTime, ParameterDirection.Input, compiledMetaUtc);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@IsClickable", SqlDbType.Bit, ParameterDirection.Input, isClickable);
            sph.DefineSqlParameter("@ShowHomeCrumb", SqlDbType.Bit, ParameterDirection.Input, showHomeCrumb);
            sph.DefineSqlParameter("@IsPending", SqlDbType.Bit, ParameterDirection.Input, isPending);
            sph.DefineSqlParameter("@CanonicalOverride", SqlDbType.NVarChar, 255, ParameterDirection.Input, canonicalOverride);
            sph.DefineSqlParameter("@IncludeInSearchMap", SqlDbType.Bit, ParameterDirection.Input, includeInSearchMap);
            sph.DefineSqlParameter("@EnableComments", SqlDbType.Bit, ParameterDirection.Input, enableComments);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateTimestamp(
            int pageId,
            DateTime lastModifiedUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_UpdateTimeStamp", 2);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@LastModifiedUTC", SqlDbType.DateTime, ParameterDirection.Input, lastModifiedUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdatePageOrder(int pageId, int pageOrder)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_UpdatePageOrder", 2);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeletePage(int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_Delete", 1);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool CleanupOrphans()
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_CleanupOrphans", 0);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }



        public static IDataReader GetPage(Guid pageGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_SelectOneByGuid", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_SelectOne", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPageList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_SiteSettings_GetPageList", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static int GetPendingCount(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_SelectPendingPageCount", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPendingPageListPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetPendingCount(siteGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_SelectPendingPageListPage", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetChildPages(int siteId, int parentPageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_SelectChildPages", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentPageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBreadcrumbs(int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_GetBreadcrumbs", 1);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static int GetNextPageOrder(
                int siteId,
                int parentId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "cy_Pages_GetNextPageOrder", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            int pageOrder = Convert.ToInt32(sph.ExecuteScalar());
            return pageOrder;
        }

        


    }
}
