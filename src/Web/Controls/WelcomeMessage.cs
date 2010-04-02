/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	
///
///		Author:				Joe Audette
///		Created:			2005-03-24
///		Last Modified:		2007-11-15
/// 
/// 03/13/2007   Alexander Yushchenko: moved all the control logic to Render() to simplify it.

using System.Web;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Resources;

namespace Cynthia.Web.UI
{
    public partial class WelcomeMessage : WebControl
    {
        
        private bool useRightSeparator = false;
        /// <summary>
        /// This property is deprecated. Instead to use Cynthia.Web.Controls.SeparatorControl
        /// </summary>
        public bool UseRightSeparator
        {
            get { return useRightSeparator; }
            set { useRightSeparator = value; }
        }

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = "topnavitem";
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }


            if (!HttpContext.Current.Request.IsAuthenticated) { return; }

            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if ((siteUser == null) || (siteUser.UserId <= -1)) { return; }

            if (CssClass.Length == 0) CssClass = "sitelink";

            if (renderAsListItem) writer.Write("<li class='" + listItemCSS + "'>");


            writer.Write("<span class='" + CssClass + "'>{0}&nbsp;{1}</span>",
                Resource.WelcomeLabel, HttpUtility.HtmlEncode(siteUser.Name));

            if (UseRightSeparator) writer.Write(" <span class='Accent'>|</span>");

            if (renderAsListItem) writer.Write("</li>");
            
        }

    }
}