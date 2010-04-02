/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2010-03-18
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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace Cynthia.Data
{
   
    public static class DBPageSettings
    {
       
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

            #region byte conversion

            int intIncludeInSearchMap = 0;
            if (includeInSearchMap) { intIncludeInSearchMap = 1; }

            int intenableComments = 0;
            if (enableComments) { intenableComments = 1; }

            int intincludeInSiteMap = 0;
            if (includeInSiteMap)
            {
                intincludeInSiteMap = 1;
            }

            int intisClickable = 0;
            if (isClickable)
            {
                intisClickable = 1;
            }

            int intshowHomeCrumb = 0;
            if (showHomeCrumb)
            {
                intshowHomeCrumb = 1;
            }

            byte hideauth = 0;
            if (hideAfterLogin)
            {
                hideauth = 1;
            }
            

            byte ssl = 0;
            if (requireSsl)
            {
                ssl = 1;
            }
            

            byte show = 0;
            if (showBreadcrumbs)
            {
                show = 1;
            }
            

            byte u = 0;
            if (useUrl)
            {
                u = 1;
            }
            

            byte nw = 0;
            if (openInNewWindow)
            {
                nw = 1;
            }
            

            byte cm = 0;
            if (showChildPageMenu)
            {
                cm = 1;
            }
            

            byte cb = 0;
            if (showChildPageBreadcrumbs)
            {
                cb = 1;
            }
            

            byte hm = 0;
            if (hideMainMenu)
            {
                hm = 1;
            }
            

            byte inMenu = 0;
            if (includeInMenu)
            {
                inMenu = 1;
            }

            int intIsPending = 0;
            if (isPending)
            {
                intIsPending = 1;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO cy_Pages ( ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("ParentID, ");
            sqlCommand.Append("PageName, ");
            sqlCommand.Append("PageTitle, ");
            sqlCommand.Append("PageOrder, ");
            sqlCommand.Append("AuthorizedRoles, ");
            sqlCommand.Append("EditRoles, ");
            sqlCommand.Append("DraftEditRoles, ");
            sqlCommand.Append("CreateChildPageRoles, ");
            sqlCommand.Append("CreateChildDraftRoles, ");
            sqlCommand.Append("RequireSSL, ");
            sqlCommand.Append("AllowBrowserCache, ");
            sqlCommand.Append("ShowBreadcrumbs, ");
            sqlCommand.Append("PageKeyWords, ");
            sqlCommand.Append("PageDescription, ");
            sqlCommand.Append("PageEncoding, ");
            sqlCommand.Append("AdditionalMetaTags, ");
            sqlCommand.Append("UseUrl, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("OpenInNewWindow, ");
            sqlCommand.Append("ShowChildPageMenu, ");
            sqlCommand.Append("ShowChildBreadcrumbs, ");
            sqlCommand.Append("HideMainMenu, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("MenuImage, ");
            sqlCommand.Append("IncludeInMenu, ");
            sqlCommand.Append("ChangeFrequency, ");
            sqlCommand.Append("SiteMapPriority, ");
            sqlCommand.Append("LastModifiedUTC, ");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("ParentGuid, ");
            sqlCommand.Append("HideAfterLogin, ");

            sqlCommand.Append("CanonicalOverride, ");
            sqlCommand.Append("IncludeInSearchMap, ");
            sqlCommand.Append("EnableComments, ");

            sqlCommand.Append("IncludeInSiteMap, ");
            sqlCommand.Append("IsClickable, ");
            sqlCommand.Append("ShowHomeCrumb, ");
            sqlCommand.Append("IsPending, ");

            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("CompiledMeta, ");
            sqlCommand.Append("CompiledMetaUtc )"); 

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" ?SiteID , ");
            sqlCommand.Append(" ?ParentID , ");
            sqlCommand.Append(" ?PageName , ");
            sqlCommand.Append(" ?PageTitle , ");
            sqlCommand.Append(" ?PageOrder , ");
            sqlCommand.Append(" ?AuthorizedRoles , ");
            sqlCommand.Append(" ?EditRoles , ");
            sqlCommand.Append(" ?DraftEditRoles, ");
            sqlCommand.Append(" ?CreateChildPageRoles , ");
            sqlCommand.Append("?CreateChildDraftRoles, ");
            sqlCommand.Append(" ?RequireSSL , ");
            sqlCommand.Append(" ?AllowBrowserCache, ");
            sqlCommand.Append(" ?ShowBreadcrumbs , ");
            sqlCommand.Append(" ?PageKeyWords , ");
            sqlCommand.Append(" ?PageDescription , ");
            sqlCommand.Append(" ?PageEncoding , ");
            sqlCommand.Append(" ?AdditionalMetaTags,  ");
            sqlCommand.Append(" ?UseUrl,  ");
            sqlCommand.Append(" ?Url,  ");
            sqlCommand.Append(" ?OpenInNewWindow,  ");
            sqlCommand.Append(" ?ShowChildPageMenu,  ");
            sqlCommand.Append(" ?ShowChildPageBreadcrumbs,  ");
            sqlCommand.Append(" ?HideMainMenu,  ");
            sqlCommand.Append(" ?Skin,  ");
            sqlCommand.Append(" ?MenuImage,  ");
            sqlCommand.Append(" ?IncludeInMenu,  ");
            sqlCommand.Append(" ?ChangeFrequency,  ");
            sqlCommand.Append(" ?SiteMapPriority,  ");
            sqlCommand.Append(" ?LastModifiedUTC,  ");
            sqlCommand.Append(" ?PageGuid,  ");
            sqlCommand.Append(" ?ParentGuid,  ");
            sqlCommand.Append("?HideAfterLogin, ");

            sqlCommand.Append("?CanonicalOverride, ");
            sqlCommand.Append("?IncludeInSearchMap, ");
            sqlCommand.Append("?EnableComments, ");

            sqlCommand.Append("?IncludeInSiteMap, ");
            sqlCommand.Append("?IsClickable, ");
            sqlCommand.Append("?ShowHomeCrumb, ");
            sqlCommand.Append("?IsPending, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?CompiledMeta, ");
            sqlCommand.Append("?CompiledMetaUtc )");
            sqlCommand.Append(";");


            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[42];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?ParentID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            arParams[2] = new MySqlParameter("?PageName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageName;

            arParams[3] = new MySqlParameter("?PageOrder", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new MySqlParameter("?AuthorizedRoles", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = authorizedRoles;

            arParams[5] = new MySqlParameter("?RequireSSL", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = ssl;


            arParams[6] = new MySqlParameter("?ShowBreadcrumbs", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = show;

            arParams[7] = new MySqlParameter("?PageKeyWords", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = pageKeyWords;

            arParams[8] = new MySqlParameter("?PageDescription", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageDescription;

            arParams[9] = new MySqlParameter("?PageEncoding", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = pageEncoding;

            arParams[10] = new MySqlParameter("?AdditionalMetaTags", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = additionalMetaTags;

            arParams[11] = new MySqlParameter("?UseUrl", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = u;

            arParams[12] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = url;

            arParams[13] = new MySqlParameter("?OpenInNewWindow", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = nw;

            arParams[14] = new MySqlParameter("?ShowChildPageMenu", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = cm;

            arParams[15] = new MySqlParameter("?EditRoles", MySqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = editRoles;

            arParams[16] = new MySqlParameter("?CreateChildPageRoles", MySqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createChildPageRoles;

            arParams[17] = new MySqlParameter("?ShowChildPageBreadcrumbs", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = cb;

            arParams[18] = new MySqlParameter("?HideMainMenu", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = hm;

            arParams[19] = new MySqlParameter("?Skin", MySqlDbType.VarChar, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = skin;

            arParams[20] = new MySqlParameter("?IncludeInMenu", MySqlDbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = inMenu;

            arParams[21] = new MySqlParameter("?MenuImage", MySqlDbType.VarChar, 50);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = menuImage;

            arParams[22] = new MySqlParameter("?PageTitle", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = pageTitle;

            arParams[23] = new MySqlParameter("?AllowBrowserCache", MySqlDbType.Bit);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = allowBrowserCache;

            arParams[24] = new MySqlParameter("?ChangeFrequency", MySqlDbType.VarChar, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new MySqlParameter("?SiteMapPriority", MySqlDbType.VarChar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new MySqlParameter("?LastModifiedUTC", MySqlDbType.DateTime);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = pageGuid.ToString();

            arParams[28] = new MySqlParameter("?ParentGuid", MySqlDbType.VarChar, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = parentGuid.ToString();

            arParams[29] = new MySqlParameter("?HideAfterLogin", MySqlDbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = hideauth;

            arParams[30] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = siteGuid.ToString();

            arParams[31] = new MySqlParameter("?CompiledMeta", MySqlDbType.Blob);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = compiledMeta;

            arParams[32] = new MySqlParameter("?CompiledMetaUtc", MySqlDbType.DateTime);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = compiledMetaUtc;

            arParams[33] = new MySqlParameter("?IncludeInSiteMap", MySqlDbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intincludeInSiteMap;

            arParams[34] = new MySqlParameter("?IsClickable", MySqlDbType.Int32);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intisClickable;

            arParams[35] = new MySqlParameter("?ShowHomeCrumb", MySqlDbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intshowHomeCrumb;

            arParams[36] = new MySqlParameter("?DraftEditRoles", MySqlDbType.Text);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = draftEditRoles;

            arParams[37] = new MySqlParameter("?IsPending", MySqlDbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intIsPending;

            arParams[38] = new MySqlParameter("?CanonicalOverride", MySqlDbType.VarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = canonicalOverride;

            arParams[39] = new MySqlParameter("?IncludeInSearchMap", MySqlDbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = intIncludeInSearchMap;

            arParams[40] = new MySqlParameter("?EnableComments", MySqlDbType.Int32);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intenableComments;

            arParams[41] = new MySqlParameter("?CreateChildDraftRoles", MySqlDbType.Text);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = createChildDraftRoles;

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

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

            #region byte conversion

            int intIncludeInSearchMap = 0;
            if (includeInSearchMap) { intIncludeInSearchMap = 1; }

            int intenableComments = 0;
            if (enableComments) { intenableComments = 1; }

            int intincludeInSiteMap = 0;
            if (includeInSiteMap)
            {
                intincludeInSiteMap = 1;
            }

            int intisClickable = 0;
            if (isClickable)
            {
                intisClickable = 1;
            }

            int intshowHomeCrumb = 0;
            if (showHomeCrumb)
            {
                intshowHomeCrumb = 1;
            }

            byte hideauth;
            if (hideAfterLogin)
            {
                hideauth = 1;
            }
            else
            {
                hideauth = 0;
            }

            byte ssl;
            if (requireSsl)
            {
                ssl = 1;
            }
            else
            {
                ssl = 0;
            }

            byte show;
            if (showBreadcrumbs)
            {
                show = 1;
            }
            else
            {
                show = 0;
            }

            byte u;
            if (useUrl)
            {
                u = 1;
            }
            else
            {
                u = 0;
            }

            byte nw;
            if (openInNewWindow)
            {
                nw = 1;
            }
            else
            {
                nw = 0;
            }

            byte cm;
            if (showChildPageMenu)
            {
                cm = 1;
            }
            else
            {
                cm = 0;
            }

            byte cb;
            if (showChildPageBreadcrumbs)
            {
                cb = 1;
            }
            else
            {
                cb = 0;
            }

            byte hm;
            if (hideMainMenu)
            {
                hm = 1;
            }
            else
            {
                hm = 0;
            }

            byte inMenu;
            if (includeInMenu)
            {
                inMenu = 1;
            }
            else
            {
                inMenu = 0;
            }

            int intIsPending = 0;
            if (isPending)
            {
                intIsPending = 1;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE cy_Pages ");
            sqlCommand.Append("SET PageOrder = ?PageOrder , ");
            sqlCommand.Append("ParentID = ?ParentID,  ");
            sqlCommand.Append("PageName = ?PageName  , ");
            sqlCommand.Append("PageTitle = ?PageTitle  , ");
            sqlCommand.Append("AuthorizedRoles = ?AuthorizedRoles  , ");

            sqlCommand.Append("EditRoles = ?EditRoles  , ");
            sqlCommand.Append("DraftEditRoles = ?DraftEditRoles  , ");
            sqlCommand.Append("CreateChildPageRoles = ?CreateChildPageRoles  , ");
            sqlCommand.Append("CreateChildDraftRoles = ?CreateChildDraftRoles  , ");

            sqlCommand.Append("RequireSSL = ?RequireSSL , ");
            sqlCommand.Append("AllowBrowserCache = ?AllowBrowserCache , ");
            sqlCommand.Append("ShowBreadcrumbs = ?ShowBreadcrumbs, ");
            sqlCommand.Append("PageKeyWords = ?PageKeyWords , ");
            sqlCommand.Append("PageDescription = ?PageDescription , ");
            sqlCommand.Append("PageEncoding = ?PageEncoding , ");
            sqlCommand.Append("AdditionalMetaTags = ?AdditionalMetaTags,  ");
            sqlCommand.Append("UseUrl = ?UseUrl,  ");
            sqlCommand.Append("Url = ?Url,  ");
            sqlCommand.Append("OpenInNewWindow = ?OpenInNewWindow,  ");
            sqlCommand.Append("ShowChildPageMenu = ?ShowChildPageMenu,  ");
            sqlCommand.Append("ShowChildBreadcrumbs = ?ShowChildPageBreadcrumbs,  ");
            sqlCommand.Append("HideMainMenu = ?HideMainMenu,  ");
            sqlCommand.Append("Skin = ?Skin,  ");
            sqlCommand.Append("MenuImage = ?MenuImage,  ");
            sqlCommand.Append("IncludeInMenu = ?IncludeInMenu,  ");
            sqlCommand.Append("ChangeFrequency = ?ChangeFrequency,  ");
            sqlCommand.Append("SiteMapPriority = ?SiteMapPriority,  ");
            sqlCommand.Append("LastModifiedUTC = ?LastModifiedUTC,  ");
            sqlCommand.Append("ParentGuid = ?ParentGuid,  ");
            sqlCommand.Append("HideAfterLogin = ?HideAfterLogin, ");

            sqlCommand.Append("CanonicalOverride = ?CanonicalOverride, ");
            sqlCommand.Append("IncludeInSearchMap = ?IncludeInSearchMap, ");

            sqlCommand.Append("IncludeInSiteMap = ?IncludeInSiteMap, ");
            sqlCommand.Append("EnableComments = ?EnableComments, ");
            sqlCommand.Append("IsClickable = ?IsClickable, ");
            sqlCommand.Append("ShowHomeCrumb = ?ShowHomeCrumb, ");
            sqlCommand.Append("IsPending = ?IsPending  , ");

            sqlCommand.Append("CompiledMeta = ?CompiledMeta, ");
            sqlCommand.Append("CompiledMetaUtc = ?CompiledMetaUtc "); 

            sqlCommand.Append("WHERE PageID = ?PageID ;");

            MySqlParameter[] arParams = new MySqlParameter[40];

            arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new MySqlParameter("?ParentID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            arParams[2] = new MySqlParameter("?PageName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageName;

            arParams[3] = new MySqlParameter("?PageOrder", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new MySqlParameter("?AuthorizedRoles", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = authorizedRoles;

            arParams[5] = new MySqlParameter("?PageKeyWords", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageKeyWords;

            arParams[6] = new MySqlParameter("?PageDescription", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = pageDescription;

            arParams[7] = new MySqlParameter("?PageEncoding", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = pageEncoding;

            arParams[8] = new MySqlParameter("?AdditionalMetaTags", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = additionalMetaTags;

            arParams[9] = new MySqlParameter("?RequireSSL", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ssl;

            arParams[10] = new MySqlParameter("?ShowBreadcrumbs", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = show;

            arParams[11] = new MySqlParameter("?UseUrl", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = u;

            arParams[12] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = url;

            arParams[13] = new MySqlParameter("?OpenInNewWindow", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = nw;

            arParams[14] = new MySqlParameter("?ShowChildPageMenu", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = cm;

            arParams[15] = new MySqlParameter("?EditRoles", MySqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = editRoles;

            arParams[16] = new MySqlParameter("?CreateChildPageRoles", MySqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createChildPageRoles;

            arParams[17] = new MySqlParameter("?ShowChildPageBreadcrumbs", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = cb;

            arParams[18] = new MySqlParameter("?HideMainMenu", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = hm;

            arParams[19] = new MySqlParameter("?Skin", MySqlDbType.VarChar, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = skin;

            arParams[20] = new MySqlParameter("?IncludeInMenu", MySqlDbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = inMenu;

            arParams[21] = new MySqlParameter("?MenuImage", MySqlDbType.VarChar, 50);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = menuImage;

            arParams[22] = new MySqlParameter("?PageTitle", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = pageTitle;

            arParams[23] = new MySqlParameter("?AllowBrowserCache", MySqlDbType.Bit);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = allowBrowserCache;

            arParams[24] = new MySqlParameter("?ChangeFrequency", MySqlDbType.VarChar, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new MySqlParameter("?SiteMapPriority", MySqlDbType.VarChar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new MySqlParameter("?LastModifiedUTC", MySqlDbType.DateTime);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new MySqlParameter("?ParentGuid", MySqlDbType.VarChar, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = parentGuid.ToString();

            arParams[28] = new MySqlParameter("?HideAfterLogin", MySqlDbType.Int32);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = hideauth;

            arParams[29] = new MySqlParameter("?CompiledMeta", MySqlDbType.Blob);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = compiledMeta;

            arParams[30] = new MySqlParameter("?CompiledMetaUtc", MySqlDbType.DateTime);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = compiledMetaUtc;

            arParams[31] = new MySqlParameter("?IncludeInSiteMap", MySqlDbType.Int32);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = intincludeInSiteMap;

            arParams[32] = new MySqlParameter("?IsClickable", MySqlDbType.Int32);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = intisClickable;

            arParams[33] = new MySqlParameter("?ShowHomeCrumb", MySqlDbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intshowHomeCrumb;

            arParams[34] = new MySqlParameter("?DraftEditRoles", MySqlDbType.Text);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = draftEditRoles;

            arParams[35] = new MySqlParameter("?IsPending", MySqlDbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intIsPending;

            arParams[36] = new MySqlParameter("?CanonicalOverride", MySqlDbType.VarChar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = canonicalOverride;

            arParams[37] = new MySqlParameter("?IncludeInSearchMap", MySqlDbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intIncludeInSearchMap;

            arParams[38] = new MySqlParameter("?EnableComments", MySqlDbType.Int32);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = intenableComments;

            arParams[39] = new MySqlParameter("?CreateChildDraftRoles", MySqlDbType.Text);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = createChildDraftRoles;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);



        }

        public static int GetNextPageOrder(
            int siteId,
            int parentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(MAX(PageOrder),-1) ");
            sqlCommand.Append("FROM	cy_Pages ");

            sqlCommand.Append("WHERE SiteID = ?SiteID AND ParentID = ?ParentID ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?ParentID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            int nextPageOrder = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams)) + 2;
            if (nextPageOrder == 1)
            {
                nextPageOrder = 3;
            }

            return nextPageOrder;

        }

        public static IDataReader GetPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	cy_Pages ");
            sqlCommand.Append("WHERE PageGuid = ?PageGuid  ");
            sqlCommand.Append("LIMIT 1  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetPage(int siteId, int pageId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Pages ");

            sqlCommand.Append("WHERE (PageID = ?PageID OR ?PageID = -1)  ");
            sqlCommand.Append("AND SiteID = ?SiteID  ");

            sqlCommand.Append("ORDER BY ParentID, PageOrder  ");
            sqlCommand.Append("LIMIT 1  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetChildPages(int siteId, int parentPageId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Pages ");

            sqlCommand.Append("WHERE ParentID = ?ParentPageID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY PageOrder; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?ParentPageID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentPageId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetBreadcrumbs(int pageId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("p.PageID,  ");
            sqlCommand.Append("p.PageName,  ");
            sqlCommand.Append("COALESCE(p.ParentID,0) As Parent1ID,  ");
            sqlCommand.Append("p1.PageName As Parent1Name,  ");
            sqlCommand.Append("COALESCE(p1.ParentID,0) As Parent2ID,  ");
            sqlCommand.Append("p2.PageName As Parent2Name,  ");
            sqlCommand.Append("COALESCE(p2.ParentID,0) As Parent3ID,  ");
            sqlCommand.Append("p3.PageName As Parent3Name,  ");
            sqlCommand.Append("COALESCE(p3.ParentID,0) As Parent4ID,  ");
            sqlCommand.Append("p4.PageName As Parent4Name,  ");
            sqlCommand.Append("COALESCE(p4.ParentID,0) As Parent5ID,  ");
            sqlCommand.Append("p5.PageName As Parent5Name,  ");
            sqlCommand.Append("COALESCE(p5.ParentID,0) As Parent6ID,  ");
            sqlCommand.Append("p6.PageName As Parent6Name,  ");
            sqlCommand.Append("COALESCE(p6.ParentID,0) As Parent7ID,  ");
            sqlCommand.Append("p7.PageName As Parent7Name,  ");
            sqlCommand.Append("COALESCE(p7.ParentID,0) As Parent8ID,  ");
            sqlCommand.Append("p8.PageName As Parent8Name,  ");
            sqlCommand.Append("COALESCE(p8.ParentID,0) As Parent9ID,  ");
            sqlCommand.Append("p9.PageName As Parent9Name,  ");
            sqlCommand.Append("COALESCE(p9.ParentID,0) As Parent10ID,  ");
            sqlCommand.Append("p10.PageName As Parent10Name,  ");
            sqlCommand.Append("COALESCE(p10.ParentID,0) As Parent11ID,  ");
            sqlCommand.Append("p11.PageName As Parent11Name,  ");
            sqlCommand.Append("COALESCE(p11.ParentID,0) As Parent12ID,  ");
            sqlCommand.Append("p12.PageName As Parent12Name  ");

            sqlCommand.Append("FROM	cy_Pages p ");

            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p1 ");
            sqlCommand.Append("ON p1.PageID = p.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p2 ");
            sqlCommand.Append("ON p2.PageID = p1.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p3 ");
            sqlCommand.Append("ON p3.PageID = p2.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p4 ");
            sqlCommand.Append("ON p4.PageID = p3.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p5 ");
            sqlCommand.Append("ON p5.PageID = p4.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p6 ");
            sqlCommand.Append("ON p6.PageID = p5.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p7 ");
            sqlCommand.Append("ON p7.PageID = p6.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p8 ");
            sqlCommand.Append("ON p8.PageID = p7.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p9 ");
            sqlCommand.Append("ON p9.PageID = p8.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p10 ");
            sqlCommand.Append("ON p10.PageID = p9.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p11 ");
            sqlCommand.Append("ON p11.PageID = p10.ParentID ");
            sqlCommand.Append("LEFT OUTER JOIN	cy_Pages p12 ");
            sqlCommand.Append("ON p12.PageID = p11.ParentID ");

            sqlCommand.Append("WHERE p.PageID = ?PageID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }



        public static bool UpdateTimestamp(
            int pageId,
            DateTime lastModifiedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Pages ");
            sqlCommand.Append("SET LastModifiedUTC = ?LastModifiedUTC  ");
            sqlCommand.Append("WHERE PageID = ?PageID ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new MySqlParameter("?LastModifiedUTC", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModifiedUtc;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdatePageOrder(int pageId, int pageOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE cy_Pages ");
            sqlCommand.Append("SET PageOrder = ?PageOrder  ");
            sqlCommand.Append("WHERE PageID = ?PageID ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new MySqlParameter("?PageOrder", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeletePage(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM cy_Pages ");
            sqlCommand.Append("WHERE PageID = ?PageID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool CleanupOrphans()
        {
            StringBuilder sqlCommand = new StringBuilder();
            //sqlCommand.Append("UPDATE cy_Pages ");
            //sqlCommand.Append("SET ParentID = -1, ParentGuid = '00000000-0000-0000-0000-000000000000' ");
            //sqlCommand.Append("WHERE ParentID <> -1 AND ParentID NOT IN (SELECT p.PageID FROM (SELECT * FROM cy_Pages) p ) ");
            //sqlCommand.Append(";");

            // the abocve wa snot compatible with MySql 4
            sqlCommand.Append("UPDATE cy_Pages AS p1 ");
            sqlCommand.Append("LEFT JOIN ( SELECT * FROM cy_Pages ) AS p2 ");
            sqlCommand.Append("ON p1.ParentID = p2.PageID ");
            sqlCommand.Append("SET p1.ParentID = -1, p1.ParentGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("p1.ParentID <> -1 ");
            sqlCommand.Append("AND p2.PageID IS NULL ");
            sqlCommand.Append(";");

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                null);

            return (rowsAffected > 0);

        }


        public static IDataReader GetPageList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	cy_Pages ");

            sqlCommand.Append("WHERE SiteID = ?SiteID ");

            sqlCommand.Append("ORDER BY	ParentID, PageOrder, PageName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static int GetPendingCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	cy_Pages ");
            sqlCommand.Append("WHERE SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsPending = 1 ");
            //sqlCommand.Append("PageID IN ");
            //sqlCommand.Append("( ");
            //sqlCommand.Append("SELECT pm.PageID FROM cy_PageModules pm ");
            //sqlCommand.Append("JOIN cy_ContentWorkflow cw ");
            //sqlCommand.Append("ON cw.ModuleGuid = pm.ModuleGuid ");
            //sqlCommand.Append("WHERE Status Not In ('Cancelled','Approved') ");
            //sqlCommand.Append(")");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPendingPageListPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("	p.*, ");
            sqlCommand.Append("COALESCE(wip.Total,0) as WipCount ");

            sqlCommand.Append("FROM	cy_Pages p  ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("(");

            sqlCommand.Append("SELECT Count(*) as Total, ");
            sqlCommand.Append("pm.PageGuid ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("cy_PageModules pm ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("cy_ContentWorkflow cw ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = pm.ModuleGuid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.Status Not In ('Cancelled','Approved') ");
            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("pm.PageGuid ");

            sqlCommand.Append(") wip ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("wip.PageGuid = p.PageGuid ");

            sqlCommand.Append("WHERE p.SiteGuid = ?SiteGuid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("p.IsPending = 1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("p.PageName ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
        


    }
}
