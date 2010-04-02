///	Created:			    2008-06-19
///	Last Modified:		    2009-01-29
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.UI
{
    public class CopyrightLabel : WebControl
    {
        private int beginYear = -1;
        private string copyrightBy = string.Empty;
        private bool showYear = true;

        public string CopyrightBy
        {
            get { return copyrightBy; }
            set { copyrightBy = value; }
        }

        public int BeginYear
        {
            get { return beginYear; }
            set { beginYear = value; }
        }

        public bool ShowYear
        {
            get { return showYear; }
            set { showYear = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (copyrightBy.Length == 0)
                {
                    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                    if (siteSettings == null)
                    {
                        this.Visible = false;
                        return;
                    }

                    copyrightBy = siteSettings.CompanyName;

                }

                writer.Write("&copy; ");
                if (showYear)
                {
                    if ((beginYear > -1) && (beginYear != DateTime.UtcNow.Year))
                    {
                        writer.Write(beginYear.ToString(CultureInfo.InvariantCulture));
                        writer.Write(" - ");
                        writer.Write(DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture));
                        writer.Write(" ");
                    }

                }

                writer.WriteEncodedText(copyrightBy);
                
            }
        }

    }
}
