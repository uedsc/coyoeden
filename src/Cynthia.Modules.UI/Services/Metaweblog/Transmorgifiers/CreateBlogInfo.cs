// Author:				Tom Opgenorth	
// Created:				2008-04-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
using System;
using System.Data;
using System.Web;

using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog.Transmorgifiers
{
    /// <summary>
    /// This class is used to convert the current record in an <see cref="IDataReader" into a 
    /// Metaweblog <see cref="BlogInfo" /> .
    /// </summary>
    public class CreateBlogInfo : ITransmorgifier<IDataReader, BlogInfo>
    {
        public BlogInfo Transmorgify(IDataReader rdr)
        {
            BlogInfo bloginfo = new BlogInfo();
            bloginfo.blogid = rdr["ModuleId"].ToString();
            bloginfo.blogName = rdr["PageName"] + " - " + rdr["ModuleTitle"];
            bloginfo.url = ReplaceWithAppPath(rdr["url"].ToString());
            bloginfo.pageId = Convert.ToInt32(rdr["PageID"]);
            bloginfo.editRoles = rdr["EditRoles"].ToString();
            bloginfo.moduleEditRoles = rdr["AuthorizedEditRoles"].ToString();
            return bloginfo;
        }


        private static string ReplaceWithAppPath(string str)
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;

            //Ensure the app path ends w/ a slash
            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }

            return str.Replace("~/", appPath);
        }
    }
}