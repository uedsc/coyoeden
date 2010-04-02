///	Author:					Joe Audette
///	Created:				2005-08-27
///	Last Modified:		    2007-07-08
/// 
/// 01/19/2007   Alexander Yushchenko: moved all the control logic to Render() to simplify it.
/// 13/04/2007   Alexander Yushchenko: made it WebControl instead of UserControl.
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
    
    public class Favicon : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings != null)
                {
                    writer.Write("\n<link rel='shortcut icon' href='{0}/Data/Sites/{1}/skins/{2}/favicon.ico' />",
                        WebUtils.GetSiteRoot(), siteSettings.SiteId, siteSettings.Skin);
                }
            }
        }
    }
}

