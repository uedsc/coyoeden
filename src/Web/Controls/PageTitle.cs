/// Author:				    Joe Audette
/// Created:			    2006/09/01
///	Last Modified:		    2007/04/13
/// 
/// 2007/04/13   Alexander Yushchenko: code refactoring, made it WebControl instead of UserControl.
/// 
/// 2007/06/01 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// This is a deparacated legacy control
    /// </summary>
    public class PageTitle : WebControl
    {
        public Literal Title = new Literal();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (currentPage == null)return;

            

            if (currentPage.PageTitle.Length > 0)
            {
                //writer.Write(Context.Server.HtmlEncode(currentPage.PageTitle));
                Title.Text = Context.Server.HtmlEncode(currentPage.PageTitle);
            }
            else
            {
                //writer.Write(Context.Server.HtmlEncode(siteSettings.SiteName + " - " + pageName));
                String pageName = currentPage.PageName;
                if (Context.Request.RawUrl.Contains("SiteMap.aspx"))
                {
                    pageName = Resource.SiteMapLink;
                }

                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings == null) return;
                Title.Text = Context.Server.HtmlEncode(siteSettings.SiteName + " - " + pageName);
            }

        }

        

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                //writer.Write("[" + this.ID + "]");
            }
            else
            {
                Title.RenderControl(writer);
            }
        }

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
        //    PageSettings currentPage = CacheHelper.GetCurrentPage();
        //    if ((siteSettings == null) || (currentPage == null)) return;

        //    String pageName = currentPage.PageName;
        //    if (Context.Request.RawUrl.Contains("SiteMap.aspx"))
        //    {
        //        pageName = Resource.SiteMapLink;
        //    }

        //    if (currentPage.PageTitle.Length > 0)
        //    {
        //        writer.Write(Context.Server.HtmlEncode(currentPage.PageTitle));
        //    }
        //    else
        //    {
        //        writer.Write(Context.Server.HtmlEncode(siteSettings.SiteName + " - " + pageName));
        //    }
        //}
    }
}
