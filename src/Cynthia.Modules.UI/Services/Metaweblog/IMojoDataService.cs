// Author:				Tom Opgenorth	
// Created:				April 27, 2008
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;

using Cynthia.Business;
using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog
{
    public interface ICynDataService
    {
        Post GetBlogPost(string postId);
        Post GetBlogPost(int postId);
        string AddPostToBlog(int moduleId, string username, Post post, bool publish);
        void UpdateBlog(Blog blog, string[] categories);
        SiteUser GetSiteUser(string loginName);
        List<CategoryInfo> GetCategoriesForBlog(int moduleId);
        void DeleteBlogPost(int postId);
        List<Post> GetPostsFor(int moduleId);
        List<BlogInfo> GetAllBlogsForCurrentSite();
    }
}