//	Author:					Joe Audette
//	Created:				2007-05-13
//	Last Modified:		    2009-11-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
    public class IEStyleIncludes : WebControl
    {
        //http://msdn.microsoft.com/en-us/library/ms537512(VS.85).aspx

        private bool includeHtml5Script = false;

        public bool IncludeHtml5Script
        {
            get { return includeHtml5Script; }
            set { includeHtml5Script = value; }
        }

        private bool includeIE8 = false;

        /// <summary>
        /// if true will include IE8Specific.css from the skin folder
        /// </summary>
        public bool IncludeIE8
        {
            get { return includeIE8; }
            set { includeIE8 = value; }
        }

        private bool includeIE7Script = false;
        public bool IncludeIE7Script
        {
            get { return includeIE7Script; }
            set { includeIE7Script = value; }
        }


        private bool includeIE8Script = false;
        public bool IncludeIE8Script
        {
            get { return includeIE8Script; }
            set { includeIE8Script = value; }
        }

        private string ie6CssFile = "IESpecific.css";

        public string IE6CssFile
        {
            get { return ie6CssFile; }
            set { ie6CssFile = value; }
        }

        private string ie7CssFile = "IE7Specific.css";

        public string IE7CssFile
        {
            get { return ie7CssFile; }
            set { ie7CssFile = value; }
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
                bool allowPageOverride = true;
                if (Page is CBasePage)
                {
                    CBasePage basePage = (CBasePage)Page;
                    allowPageOverride = basePage.AllowSkinOverride;
                }
                string skinBaseUrl = SiteUtils.GetSkinBaseUrl(true, Page);
                string scriptBaseUrl = WebUtils.GetSiteRoot() + "/ClientScript/";

                if (includeHtml5Script)
                {
                    writer.Write("\n<!--[if IE]>\n");

                    writer.Write("<script defer=\"defer\" src=\"" + scriptBaseUrl + "html5.js\" type=\"text/javascript\"></script>\n");
                    // the above script does what the below script used to do

                    //writer.Write("<script type=\"text/javascript\">");
                    //writer.Write("\n document.createElement(\"header\");");
                    //writer.Write("\n document.createElement(\"footer\");");
                    //writer.Write("\n document.createElement(\"nav\");");
                    //writer.Write("\n document.createElement(\"article\");");
                    //writer.Write("\n document.createElement(\"section\");");
                    //writer.Write("\n</script>");

                    writer.Write("\n<![endif]-->");

                }


                writer.Write("\n<!--[if lt IE 7]>\n");
                if (includeIE7Script)
                {
                    writer.Write("<script defer=\"defer\" src=\"" + scriptBaseUrl + "IE7.js\" type=\"text/javascript\"></script>\n");
                }

                writer.Write("<link rel=\"stylesheet\" href=\"" + skinBaseUrl + ie6CssFile + "\" type=\"text/css\" id=\"IEMenuCSS\" />\n");

                writer.Write("<![endif]-->\n");

                writer.Write("<!--[if IE 7]>\n");
                if (includeIE8Script)
                {
                    writer.Write("<script defer=\"defer\" src=\"" + scriptBaseUrl + "IE8.js\" type=\"text/javascript\"></script>\n");
                }

                writer.Write("<link rel=\"stylesheet\" href=\"" + skinBaseUrl + ie7CssFile + "\" type=\"text/css\" id=\"IE7MenuCSS\" />\n");
                writer.Write("<![endif]-->\n");

                if (includeIE8)
                {
                    writer.Write("<!--[if gt IE 7]>\n");
                    writer.Write("<link rel=\"stylesheet\" href=\"" + skinBaseUrl + "IE8Specific.css\" type=\"text/css\" id=\"IE8MenuCSS\" />\n");
                    writer.Write("<![endif]-->\n");

                }
               

                

            }

        }
    }
}
