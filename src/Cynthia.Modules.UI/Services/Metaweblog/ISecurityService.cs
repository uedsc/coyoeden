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
using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog
{
    public interface ISecurityService
    {
        bool IsValidUser(string loginName, string password);
        bool CanUserPostToBlog(string loginName, int moduleId);
        bool CanUserPostToBlog(string loginName, BlogInfo b);
        bool CanUserEditPost(string loginName, int postId);
        bool CanUserDeleteBlogPost(string loginName, int postId);
    }
}