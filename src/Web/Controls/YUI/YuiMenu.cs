//	Created:			    2009-04-21
//	Last Modified:		    2009-04-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web.UI
{
    public class YuiMenu : WebControl
    {

        private string controlId = string.Empty;


        /// <summary>
        /// This should be the server id of a Panel, inside the panel should be a ul or nested uls
        /// </summary>
        public string ControlId
        {
            get { return controlId; }
            set { controlId = value; }
        }


        private void SetupScripts()
        {
            if (controlId.Length == 0) { return; }

            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");

            script.Append("\nvar mnu" + controlId + " = new YAHOO.widget.Menu('" + controlId + "' ");

            //script.Append("{");


            //script.Append("}");

            script.Append(");");

            script.Append("\nmnu" + controlId + ".render();");
            script.Append("\nmnu" + controlId + ".show();");

            script.Append("\n</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "yui-tree" + this.ClientID, script.ToString());

        }


        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            SetupScripts();
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
                // don't render anything, just setup the scripts
            }

        }
    }
}
