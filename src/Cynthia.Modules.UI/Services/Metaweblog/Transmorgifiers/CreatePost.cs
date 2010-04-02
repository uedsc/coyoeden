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

using Cynthia.Business;
using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog.Transmorgifiers
{
    /// <summary>
    /// This class will convert a either a Cynthia <see cref="Blog" /> object or the 
    /// current record of an <see cref="IDataReader" /> into a  Metaweblog <see cref="Post" />.
    /// </summary>
    public class CreatePost : TransmorgifierBase<Blog, Post>, ITransmorgifier<IDataReader, Post>
    {
        public CreatePost(SiteSettings siteSettings) : base(siteSettings) {}

        /// <summary>
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public override Post Transmorgify(Blog blog)
        {
            Post post = new Post();
            post.dateCreated = blog.CreatedUtc;
            post.description = blog.Description;
            post.title = blog.Title;

            // TODO [TO080405@2143] We need to append the module id (mid), and the page id (pageid).
            post.link = SiteSettings.SiteRoot + String.Format("/BlogView.aspx?ItemID={0}", blog.ItemId);
            post.permalink = post.link;
            post.postid = blog.ItemId;
            post.userid = blog.UserName;

            return post;
        }


        /// <summary>
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public Post Transmorgify(IDataReader rdr)
        {
            Post post = new Post();
            post.dateCreated = Convert.ToDateTime(rdr["CreatedDate"]);
            post.description = rdr["Description"].ToString();
            post.title = rdr["Title"].ToString();
            // TODO [TO080411@2156] Get the categories
            // TODO [TO080411@2156] Get the enclosures
            post.link = SiteSettings.SiteRoot + String.Format("/BlogView.aspx?ItemID={0}&ModuleID={1}", rdr["ItemID"], rdr["ModuleID"]);
            post.permalink = post.link;
            post.postid = Convert.ToInt32(rdr["ItemID"].ToString());
            // TODO [TO080411@2158] Get the Source, whatever that is.
            post.userid = rdr["CreatedByUser"].ToString();
            return post;
        }
    }
}