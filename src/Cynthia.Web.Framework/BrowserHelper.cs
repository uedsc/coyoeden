// Author:					Joe Audette
// Created:				    2009-06-29
// Last Modified:			2010-03-07
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
using System.Web;


namespace Cynthia.Web.Framework
{
    public static class BrowserHelper
    {
        private const string IE = "IE";
        private const string Version6 = "6";

        public static bool IsIE6()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.Browser != null))
            {
                HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                if ((browser.Browser == IE) && (browser.Version.StartsWith(Version6)))
                {
                    result = true;
                }
                
            }

            return result;

        }

        public static bool IsSafari()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("safari"));
            }

            return result;

        }

        public static bool IsOpera()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("opera"));
            }

            return result;

        }

        public static bool IsIphone()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone"));
            }

            return result;

        }

    }
}
