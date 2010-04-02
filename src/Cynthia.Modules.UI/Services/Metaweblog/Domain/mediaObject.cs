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

namespace Cynthia.Web.Services.Metaweblog.Domain
{
    /// <summary>
    /// This structure represents some sort of binary object that would be used in a blog, 
    /// typically an image.
    /// </summary>
    public struct mediaObject
    {
        public string name;
        public string type;
        public Byte[] bits;
    }
}