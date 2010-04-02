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

using CookComputing.XmlRpc;

using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog
{
    public interface IMetaWeblog
    {
        [XmlRpcMethod("metaWeblog.editPost", Description = "Updates and existing post to a designated blog using the metaWeblog API. Returns true if completed.")]
        bool editPost(string postid, string username, string password, Post post, bool publish);

        [XmlRpcMethod("metaWeblog.getCategories",
            Description = "Retrieves a list of valid categories for a post " + "using the metaWeblog API. Returns the metaWeblog categories " + "struct collection.")]
        CategoryInfo[] getCategories(string blogid, string username, string password);

        [XmlRpcMethod("metaWeblog.getPost",
            Description = "Retrieves an existing post using the metaWeblog " + "API. Returns the metaWeblog struct.")]
        Post getPost(string postid, string username, string password);

        [XmlRpcMethod("metaWeblog.getRecentPosts",
            Description = "Retrieves a list of the most recent existing post " + "using the metaWeblog API. Returns the metaWeblog struct collection.")]
        Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts);

        [XmlRpcMethod("metaWeblog.newPost",
            Description = "Makes a new post to a designated blog using the " + "metaWeblog API. Returns postid as a string.")]
        string newPost(string blogid, string username, string password, Post post, bool publish);

        [XmlRpcMethod("metaWeblog.newMediaObject",
            Description = "Uploads an image, movie, song, or other media " + "using the metaWeblog API. Returns the metaObject struct.")]
        mediaObjectInfo newMediaObject(object blogid, string username, string password, mediaObject mediaobject);

        [XmlRpcMethod("blogger.deletePost", Description = "Deletes a post.")]
        [return : XmlRpcReturnValue(Description = "Always returns true.")]
        bool deletePost(string appKey, string postid, string username, string password,
                        [XmlRpcParameter(Description = "Where applicable, this specifies whether the blog " + "should be republished after the post has been deleted.")] bool publish);

        [XmlRpcMethod("blogger.getUsersBlogs", Description = "Returns information on all the blogs a given user " + "is a member.")]
        BlogInfo[] getUsersBlogs(string appKey, string username, string password);
    }
}