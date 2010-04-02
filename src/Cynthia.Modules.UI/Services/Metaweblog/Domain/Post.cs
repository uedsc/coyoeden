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

namespace Cynthia.Web.Services.Metaweblog.Domain
{
    /// <summary>
    /// This structure is an actual blog post.
    /// </summary>
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)] [XmlRpcMember(Description = "Required when posting.")] public DateTime dateCreated;
        [XmlRpcMissingMapping(MappingAction.Error)] [XmlRpcMember(Description = "Required when posting.")] public string description;
        [XmlRpcMissingMapping(MappingAction.Error)] [XmlRpcMember(Description = "Required when posting.")] public string title;
        [XmlRpcMember("categories", Description = "Contains categories for the post.")] public string[] categories;
        public Enclosure enclosure;
        public string link;
        public string permalink;

        [XmlRpcMember(
            Description = "Not required when posting. Depending on server may "
                          + "be either string or integer. "
                          + "Use Convert.ToInt32(postid) to treat as integer or "
                          + "Convert.ToString(postid) to treat as string")] public object postid;

        public Source source;
        public string userid;
    }
}