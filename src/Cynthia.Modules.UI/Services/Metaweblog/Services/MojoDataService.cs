// Author:				Tom Opgenorth	
// Created:				May 03, 2008
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Modified 2009-01-05 Joe Audette, add support for friendly urls and indexing to the search index

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Services.Metaweblog.Domain;
using Cynthia.Web.Services.Metaweblog.Transmorgifiers;

namespace Cynthia.Web.Services.Metaweblog.Services
{
    public class CynDataService : ICynDataService
    {
        private readonly ITransmorgifier<Blog, Post> _blogToPost;
        private readonly ITransmorgifier<IDataReader, BlogInfo> _dataReaderToBlogInfo;
//        private readonly ITransmorgifier<IDataReader, CategoryInfo> _dataReaderToCategoryInfo;
        private readonly ITransmorgifier<IDataReader, Post> _dataReaderToPost;
//        private readonly ITransmorgifier<Post, Blog> _postToBlog;
        private readonly SiteSettings _siteSettings;


        public CynDataService(SiteSettings siteSettings)
        {
            _siteSettings = siteSettings;
            _blogToPost = new CreatePost(_siteSettings);
            _dataReaderToPost = (ITransmorgifier<IDataReader, Post>) _blogToPost;
            _dataReaderToBlogInfo = new CreateBlogInfo();
        }


        public CynDataService(ITransmorgifier<Blog, Post> blogToPost,
                               ITransmorgifier<IDataReader, BlogInfo> dataReaderToBlogInfo,
                               ITransmorgifier<IDataReader, CategoryInfo> dataReaderToCategoryInfo,
                               ITransmorgifier<IDataReader, Post> dataReaderToPost,
                               ITransmorgifier<Post, Blog> postToBlog,
                               SiteSettings siteSettings)
        {
            _blogToPost = blogToPost;
            _dataReaderToBlogInfo = dataReaderToBlogInfo;
//            _dataReaderToCategoryInfo = dataReaderToCategoryInfo;
            _dataReaderToPost = dataReaderToPost;
//            _postToBlog = postToBlog;
            _siteSettings = siteSettings;
        }

        public Post GetBlogPost(int postId)
        {
            Blog blog = new Blog(postId);
            Post post = _blogToPost.Transmorgify(blog);
            post.categories = GetCategoriesForPost(Convert.ToInt32(post.postid));
            return post;
        }

        public Post GetBlogPost(string postId)
        {
            return GetBlogPost(Convert.ToInt32(postId));
        }

        public string AddPostToBlog(int moduleId, string username, Post post, bool publish)
        {
            Module module = new Module(moduleId);
            SiteUser user = new SiteUser(_siteSettings, username);
            ITransmorgifier<Post, Blog> postToBlog = new CreateBlog(_siteSettings, module, user);

            Blog blog = postToBlog.Transmorgify(post);

            blog.UserGuid = user.UserGuid;
            blog.LastModUserGuid = user.UserGuid;
            blog.ModuleId = moduleId;
            blog.ModuleGuid = module.ModuleGuid;
            blog.IncludeInFeed = true;
            blog.AllowCommentsForDays = 90; // TODO [TO080506@2125] Doesn't look like there is a default value for this.

            //added 2009-08-30 by Joe Audette
            if ((post.dateCreated != null) &&(post.dateCreated > DateTime.MinValue) &&(post.dateCreated < DateTime.MaxValue))
            {
                if (!WebConfigSettings.DisableUseOfPassedInDateForMetaWeblogApi) { blog.StartDate = post.dateCreated; }
            }
            

            // [TO080506@2135] Maybe this should be wrapped up in a transaction?

            // need to get the page id to use for real url mapping
            DataTable modulePages = Module.GetPageModulesTable(moduleId);
            int pageId = GetPageIdForModule(moduleId);
            
            string newUrl = FriendlyUrl.SuggestFriendlyUrl(blog.Title, _siteSettings);
            
            blog.ItemUrl = "~/" + newUrl;
            blog.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);

            blog.Save();

            FriendlyUrl newFriendlyUrl = new FriendlyUrl();
            newFriendlyUrl.SiteId = _siteSettings.SiteId;
            newFriendlyUrl.SiteGuid = _siteSettings.SiteGuid;
            newFriendlyUrl.PageGuid = blog.BlogGuid;
            newFriendlyUrl.Url = newUrl;
            newFriendlyUrl.RealUrl = "~/Blog/ViewPost.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + blog.ModuleId.ToString(CultureInfo.InvariantCulture)
                + "&ItemID=" + blog.ItemId.ToString(CultureInfo.InvariantCulture);

            if (pageId > -1)
            {
                newFriendlyUrl.Save();
            }

            AddCategoriesToBlog(blog, post.categories);

            SiteUtils.QueueIndexing();

            return blog.ItemId.ToString();
        }

        void blog_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["BlogIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        public int GetPageIdForModule(int moduleId)
        {
            DataTable modulePages = Module.GetPageModulesTable(moduleId);
            int pageId = -1;
            if (modulePages.Rows.Count > 0)
            {
                pageId = Convert.ToInt32(modulePages.Rows[0]["PageId"]);
            }

            return pageId;
        }


        public SiteUser GetSiteUser(string loginName)
        {
            // [TO080505@2034] Maybe we should factor out the SiteSettings stuff?
            SiteUser user = new SiteUser(_siteSettings, loginName);
            return user;
        }

        public void UpdateBlog(Blog blog, string[] categories)
        {
            // [TO080517@2256] How does one start a transaction?
            if (!Blog.DeleteItemCategories(blog.ItemId))
            {
                throw new ApplicationException("Could not update post - failed to delete categories");
            }

            blog.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);

            if (!blog.Save())
            {
                throw new ApplicationException("Could not update post - failed to save.");
            }

            List<CategoryInfo> allBlogCategories = GetCategoriesForBlog(blog.ModuleId);
            foreach (string category in categories)
            {
                CategoryInfo categoryInfo = allBlogCategories.Find(delegate(CategoryInfo ci) { return ci.title.Equals(category, StringComparison.OrdinalIgnoreCase); });
                Blog.AddItemCategory(blog.ItemId, Convert.ToInt32(categoryInfo.categoryid));
            }

            SiteUtils.QueueIndexing();
        }

        public void DeleteBlogPost(int postId)
        {
            Blog blog = new Blog(postId);
            blog.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);
            blog.Delete();
            FriendlyUrl.DeleteByPageGuid(blog.BlogGuid);
    
            SiteUtils.QueueIndexing();

            
        }

        public List<Post> GetPostsFor(int moduleId)
        {
            // TODO [TO080503@0955] Need a test
            List<Post> posts = new List<Post>();

            using (IDataReader rdr = Blog.GetBlogs(moduleId, DateTime.MaxValue))
            {
                while (rdr.Read())
                {
                    Post post = _dataReaderToPost.Transmorgify(rdr);
                    post.categories = GetCategoriesForPost(Convert.ToInt32(post.postid));
                    posts.Add(post);
                }
            }
            posts.Sort(delegate(Post a, Post b) { return a.dateCreated.CompareTo(b.dateCreated); });
            return posts;
        }

        public List<CategoryInfo> GetCategoriesForBlog(int moduleId)
        {
            List<CategoryInfo> categories = new List<CategoryInfo>();
            ITransmorgifier<IDataReader, CategoryInfo> dataReaderToCategoryInfo = new CreateCategoryInfo(_siteSettings.SiteRoot, moduleId);
            using (IDataReader rdr = Blog.GetCategoriesList(moduleId))
            {
                while (rdr.Read())
                {
                    CategoryInfo categoryInfo = dataReaderToCategoryInfo.Transmorgify(rdr);
                    categories.Add(categoryInfo);
                }
            }
            categories.Sort((a, b) => a.title.CompareTo(b.title));
            return categories;
        }

        public List<BlogInfo> GetAllBlogsForCurrentSite()
        {
            List<BlogInfo> blogs = new List<BlogInfo>();

            Guid blogFeatureGuid = new Guid("D1D86E30-7864-40fe-BCBF-46E77AF80342");

            using (IDataReader rdr = Module.GetModulesForSite(_siteSettings.SiteId, blogFeatureGuid))
            {
                while (rdr.Read())
                {
                    BlogInfo blogInfo = _dataReaderToBlogInfo.Transmorgify(rdr);
                    blogs.Add(blogInfo);
                }
            }
            blogs.Sort(delegate(BlogInfo a, BlogInfo b) { return a.blogName.CompareTo(b.blogName); });

            return blogs;
        }



        private static string[] GetCategoriesForPost(int postId)
        {
            List<string> categories = new List<string>();
            using (IDataReader rdr = Blog.GetItemCategories(postId))
            {
                while (rdr.Read())
                {
                    categories.Add(rdr["Category"].ToString());
                }
            }
            categories.Sort();
            return categories.ToArray();
        }

        private void AddCategoriesToBlog(Blog blog, IEnumerable<string> categories)
        {
            Blog.DeleteItemCategories(blog.ItemId);
            List<CategoryInfo> categoryInfos = GetCategoriesForBlog(blog.ModuleId);
            foreach (string category in categories)
            {
                CategoryInfo categoryInfoToAdd = categoryInfos.Find(delegate(CategoryInfo categoryInfo) { return categoryInfo.title.Equals(category, StringComparison.OrdinalIgnoreCase); });
                Blog.AddItemCategory(blog.ItemId, Convert.ToInt32(categoryInfoToAdd.categoryid));
            }
        }
    }
}