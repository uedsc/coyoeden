/// Author:				    Joe Audette
/// Created:			    2005-12-03
///	Last Modified:		    2007-07-08
/// 
/// 13/04/2007   Alexander Yushchenko: code refactoring, made it WebControl instead of UserControl.
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.UI
{
    
    public class SkinPreview : WebControl
    {
        private string skinFileName = "printerfriendly";
        public string SkinFileName
        {
            get { return skinFileName; }
            set { skinFileName = value; }
        }

        private string imageUrl = string.Empty;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }


            string url = WebUtils.GetUrlWithoutQueryString(Context.Request.RawUrl)
                + "?skin="
                + skinFileName
                + WebUtils.BuildQueryString(WebUtils.GetQueryString(Context.Request.RawUrl), "skin");

            if (imageUrl.Length > 0)
            {
                writer.Write("<a href='{0}' title='{1}' rel='nofollow'><img alt='{1}' src='{2}' /></a>",
                    Context.Server.HtmlEncode(url),
                    Resource.PrinterFriendlyLink,
                    Page.ResolveUrl(imageUrl));
            }
            else
            {
                writer.Write("<a href='{0}' rel='nofollow'>{1}</a>",
                    Context.Server.HtmlEncode(url),
                    Resource.PrinterFriendlyLink);
            }
           

        }

    }
}
