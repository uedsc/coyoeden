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
// Last Modified 2009-03-26 Joe Audette

using System;
using Cynthia.Business;
using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog.Transmorgifiers
{
    /// <summary>
    /// This class will covert a Metaweblog <see cref="Post" /> into a Cynthia <see cref="Blog" />.
    /// </summary>
    public class CreateBlog : TransmorgifierBase<Post, Blog>
    {
        private readonly Module _module;
        private readonly double _userTimeOffset = SiteUtils.GetUserTimeOffset();
        private readonly SiteUser _user;

        public CreateBlog(SiteSettings siteSettings, Module module, SiteUser user) : base(siteSettings)
        {
            _module = module;
            _user = user;
        }

        public override Blog Transmorgify(Post post)
        {
            Blog blog = new Blog();
            
            blog.Description = post.description;
            SetExcerpt(blog);
            blog.IncludeInFeed = true;
            blog.IsInNewsletter = true;
            blog.LastModUserGuid = _user.UserGuid;
            blog.ModuleId = _module.ModuleId;
            blog.ModuleGuid = _module.ModuleGuid;
            blog.Title = post.title;
            blog.UserGuid = _user.UserGuid;
            // TODO [TO080405@1854] Assuming e-mail == username for now, but that isn't always the case.
            blog.UserName = _user.Email;

            return blog;
        }


        private static void SetExcerpt(Blog blog)
        {
            //if (blog.Description.Length > 100)
            //{
            //    blog.Excerpt = blog.Description.Substring(0, 100);
            //}
            //else
            //{
            //    blog.Excerpt = blog.Description;
            //}
        }
    }
}