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
using System.Collections.Generic;

using CookComputing.XmlRpc;

using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Services.Metaweblog.Domain;
using Cynthia.Web.Services.Metaweblog.Services;

namespace Cynthia.Web.Services.Metaweblog
{
    /// <summary>
    /// This class is our implementation of the MetaWeblog protocol, which will allow Cynthia to support
    /// Windows Live Writer for authoring blog posts.
    /// </summary>
    public class MetaWeblogger : XmlRpcService, IMetaWeblog
    {
        private readonly IMediaObjectPersistor _mediaObjectPersistor;
        private readonly ICynDataService _CDataService;
        private readonly ISecurityService _securityService;

        public MetaWeblogger() : this(CacheHelper.GetCurrentSiteSettings()) {}

        public MetaWeblogger(SiteSettings siteSettings)
        {
            _securityService = new SecurityService(siteSettings);
            _CDataService = new CynDataService(siteSettings);
            _mediaObjectPersistor = new SaveMediaObjectToFileSystem(siteSettings.DataFolder);
        }


        public MetaWeblogger(IMediaObjectPersistor mediaObjectPersistor, ICynDataService CDataService, ISecurityService securityService)
        {
            _mediaObjectPersistor = mediaObjectPersistor;
            _CDataService = CDataService;
            _securityService = securityService;
        }

        public bool editPost(string postid, string username, string password, Post post, bool publish)
        {
            int blogid = Convert.ToInt32(postid);
            ValidateUser(username, password);
            CheckThatUserCanEditBlogPost(username, blogid);

            try
            {
                SiteUser user = _CDataService.GetSiteUser(username);
                Blog blog = new Blog(blogid);
                blog.Description = post.description;
                blog.Excerpt = blog.Description.Length > 100 ? blog.Description.Substring(0, 100) : blog.Description;
                blog.LastModUserGuid = user.UserGuid;
                blog.Title = post.title;
                _CDataService.UpdateBlog(blog, post.categories);
                return true;
            }
            catch (Exception)
            {
                throw new XmlRpcFaultException(0, "Could not update post.");
            }
        }

        public CategoryInfo[] getCategories(string blogid, string username, string password)
        {
            int moduleId = Convert.ToInt32(blogid);
            ValidateUser(username, password);
            CheckThatUserCanPostToBlog(username, moduleId);
            try
            {
                List<CategoryInfo> categoryInfos = _CDataService.GetCategoriesForBlog(moduleId);
                return categoryInfos.ToArray();
            }
            catch (Exception ex)
            {
                throw NewXmlRpcFaultWithMessage("Could not get categories.", ex);
            }
        }

        public Post getPost(string postid, string username, string password)
        {
            ValidateUser(username, password);
            int postId = Convert.ToInt32(postid);
            CheckThatUserCanEditBlogPost(username, postId);
            try
            {
                Post post = _CDataService.GetBlogPost(postid);
                return post;
            }
            catch (Exception ex)
            {
                throw NewXmlRpcFaultWithMessage("Could not get the post.", ex);
            }
        }

        public Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            // TODO [TO080503@1001] Write test.
            // TODO [TO080518@2222] This is to big.
            ValidateUser(username, password);
            int moduleId = Convert.ToInt32(blogid);
            CheckThatUserCanPostToBlog(username, moduleId);
            try
            {
                // the number of posts returned here is already limited by a module setting
                List<Post> posts = _CDataService.GetPostsFor(moduleId);
                posts.Sort(delegate(Post a, Post b)
                               {
                                   int compare = a.dateCreated.CompareTo(b.dateCreated);
                                   if (compare == 0)
                                   {
                                       compare = a.title.CompareTo(b.title);
                                   }
                                   return compare;
                               });

                if (numberOfPosts > posts.Count)
                    numberOfPosts = posts.Count;
                Post[] postsToReturn = new Post[numberOfPosts];
                posts.CopyTo(0, postsToReturn, 0, numberOfPosts);
                return postsToReturn;
            }
            catch (Exception ex)
            {
                throw NewXmlRpcFaultWithMessage("Could not get a list of recent posts.", ex);
            }
        }

        public string newPost(string blogid, string username, string password, Post post, bool publish)
        {
            ValidateUser(username, password);
            int moduleId = Convert.ToInt32(blogid);
            CheckThatUserCanPostToBlog(username, moduleId);
            try
            {
                return _CDataService.AddPostToBlog(moduleId, username, post, publish);
            }
            catch (Exception ex)
            {
                throw NewXmlRpcFaultWithMessage("Could not save post to blog.", ex);
            }
        }

        public mediaObjectInfo newMediaObject(object blogid, string username, string password, mediaObject mediaobject)
        {
            ValidateUser(username, password);
            int moduleId = Convert.ToInt32(blogid);
            CheckThatUserCanPostToBlog(username, moduleId);
            try
            {
                mediaObjectInfo mediaInfo = _mediaObjectPersistor.Save(mediaobject);
                return mediaInfo;
            }
            catch (Exception ex)
            {
                throw NewXmlRpcFaultWithMessage("Could not save the media object.", ex);
            }
        }

        public bool deletePost(string appKey, string postid, string username, string password,
                               [XmlRpcParameter(Description = "Where applicable, this specifies whether the blog should be republished after the post has been deleted.")] bool publish)
        {
            // TODO [TO080503@1000] need test
            // [TO080405@2300] Oddly, it doesn't seem like Windows Live Writer allows you to delete.
            ValidateUser(username, password);
            try
            {
                int id = Convert.ToInt32(postid);
                if (_securityService.CanUserDeleteBlogPost(username, id))
                {
                    _CDataService.DeleteBlogPost(id);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw NewXmlRpcFaultWithMessage("Could not delete post.", ex);
            }
        }

        public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
        {
            // [TO080426@1841] I don't know what appKey is.
            ValidateUser(username, password);
            try
            {
                List<BlogInfo> blogs = _CDataService.GetAllBlogsForCurrentSite();
                List<BlogInfo> blogsForUser = new List<BlogInfo>(blogs.Count);
                foreach (BlogInfo blogInfo in blogs)
                {
                    if (_securityService.CanUserPostToBlog(username, blogInfo))
                    {
                        blogsForUser.Add(blogInfo);
                    }
                }
                return blogsForUser.ToArray();
            }
            catch (Exception ex)
            {
                throw NewXmlRpcFaultWithMessage("Could not get blogs for user.", ex);
            }
        }

        private static XmlRpcFaultException NewXmlRpcFaultWithMessage(string msg, Exception ex)
        {
            XmlRpcFaultException xmlEx = new XmlRpcFaultException(0, String.Format("{0}.  Message: {1}.", msg, ex.Message));
            return xmlEx;
        }

        private void ValidateUser(string username, string password)
        {
            bool isValid = _securityService.IsValidUser(username, password);

            if (!isValid)
            {
                throw new XmlRpcFaultException(0, "Username and/or password denied.");
            }
        }

        private void CheckThatUserCanEditBlogPost(string username, int postId)
        {
            if (!_securityService.CanUserEditPost(username, postId))
            {
                throw new XmlRpcFaultException(0, "User no allow to edit post.");
            }
        }

        private void CheckThatUserCanPostToBlog(string username, int moduleId)
        {
            if (!_securityService.CanUserPostToBlog(username, moduleId))
            {
                throw new XmlRpcFaultException(0, "User not allow access to this blog.");
            }
        }
    }
}