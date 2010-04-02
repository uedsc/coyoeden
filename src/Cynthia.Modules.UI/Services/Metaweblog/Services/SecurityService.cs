// Author:				Tom Opgenorth	
// Created:				2008-04-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Modified 2009-01-05 Joe Audette - add check for page edit roles, validate user using membership provider

using System;
using System.Data;
using System.Web.Security;
using Cynthia.Business;
using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly SiteSettings _siteSettings;

        public SecurityService(SiteSettings siteSettings)
        {
            _siteSettings = siteSettings;
        }

        public bool CanUserDeleteBlogPost(string loginName, int postId)
        {
            // [TO080511@2129] For now, we appy the same rules for deleting as posting. 
            return CanUserEditPost(loginName, postId);
        }

        /// <summary>
        /// This is basically the same as the logic in CBasePage.UserCanEditModule().
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="postId">This is the same as itemid in Cynthia parlance.</param>
        /// <returns></returns>
        public bool CanUserEditPost(string loginName, int postId)
        {
            SiteUser user = new SiteUser(_siteSettings, loginName);
            if (user.IsInRoles("Admins;Content Administrators"))
            {
                return true;
            }

            Blog blog = new Blog(postId);
            Module module = new Module(blog.ModuleId);

            if (module.EditUserId.Equals(user.UserId))
            {
                return true;
            }

            if (user.IsInRoles(module.AuthorizedEditRoles))
            {
                return true;
            }

            int pageId = GetPageIdForModule(blog.ModuleId);
            if (pageId > -1)
            {
                PageSettings blogPage = new PageSettings(_siteSettings.SiteId, pageId);
                if (user.IsInRoles(blogPage.EditRoles)) { return true; }

            }

            return false;
        }

        public bool CanUserPostToBlog(string loginName, BlogInfo b)
        {
            SiteUser user = new SiteUser(_siteSettings, loginName);
            if (user.IsInRoles("Admins;Content Administrators"))
            {
                return true;
            }

            if (user.IsInRoles(b.editRoles))
            {
                return true;
            }

            if (user.IsInRoles(b.moduleEditRoles))
            {
                return true;
            }

            

            return false;
        }

        

        public bool CanUserPostToBlog(string loginName, int moduleId)
        {
            SiteUser user = new SiteUser(_siteSettings, loginName);
            if (user.IsInRoles("Admins;Content Administrators"))
            {
                return true;
            }

            Module module = new Module(moduleId);

            if (module.EditUserId.Equals(user.UserId))
            {
                return true;
            }

            if (user.IsInRoles(module.AuthorizedEditRoles))
            {
                return true;
            }

            int pageId = GetPageIdForModule(moduleId);
            if (pageId > -1)
            {
                PageSettings blogPage = new PageSettings(_siteSettings.SiteId, pageId);
                if (user.IsInRoles(blogPage.EditRoles)) { return true; }

            }

            return false;
        }

        public bool IsValidUser(string loginName, string password)
        {
            return Membership.ValidateUser(loginName, password);

            //bool isValid;
            //SiteUser siteUser = new SiteUser(_siteSettings, loginName);
            //if ((siteUser.IsLockedOut) || (siteUser.IsDeleted) || (siteUser.UserId < 0))
            //{
            //    isValid = false;
            //}
            //else
            //{
            //    isValid = siteUser.Password.Equals(password, StringComparison.Ordinal);
            //}
            //return isValid;
        }

        private int GetPageIdForModule(int moduleId)
        {
            DataTable modulePages = Module.GetPageModulesTable(moduleId);
            int pageId = -1;
            if (modulePages.Rows.Count > 0)
            {
                pageId = Convert.ToInt32(modulePages.Rows[0]["PageId"]);
            }

            return pageId;
        }
    }
}