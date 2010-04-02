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
// Modified 2009-01-05 Joe Audette, added PageId as its needed for the friendly url mapping

using System;

namespace Cynthia.Web.Services.Metaweblog.Domain
{
    /// <summary>
    /// Thi structure holds some basic information about a given blog.
    /// </summary>
    public struct BlogInfo
    {
        public string blogid;
        public string url;
        public string blogName;
        public int pageId;
        public string editRoles;
        public string moduleEditRoles;
    }
}