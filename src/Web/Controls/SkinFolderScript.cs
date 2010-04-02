//	Created:			    2008-08-18
//	Last Modified:		    2010-02-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// allows you to inclde a javascript file form the skin folder by just its name, the path to the skin folder will be resolved.
    /// </summary>
    public class SkinFolderScript : WebControl
    {
        private string scriptFileName = string.Empty;

        public string ScriptFileName
        {
            get { return scriptFileName; }
            set { scriptFileName = value; }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            SetupScript();
        }

        private void SetupScript()
        {
            if (scriptFileName.Length == 0) { return; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            scriptFileName, "\n<script src=\""
            + SiteUtils.GetSkinBaseUrl(Page) + scriptFileName +"\" type=\"text/javascript\" ></script>");


        }


        protected override void Render(HtmlTextWriter writer)
        {
            //base.RenderContents(writer);
            //no need to render anything just setup script
        }
    }
}
