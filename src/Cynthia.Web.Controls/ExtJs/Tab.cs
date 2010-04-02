using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web.Controls.ExtJs
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-10-27
    /// Last Modified:			2007-11-02
    ///		
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.	 
    /// </summary>
    public class Tab : ExtPanel
    {
        
        private bool closable = false;

        public bool Closable
        {
            get { return closable; }
            set { closable = value; }
        }

        public override void RenderControlToScript(StringBuilder script)
        {
           
            if (DidRenderScript) return;

            script.Append("{");
            if (UnMinify) script.Append("\n ");

            script.Append("contentEl:'" + this.ClientID + "'");

            if (UnMinify) script.Append("\n ");
            script.Append(",id:'tab" + this.ClientID + "'");

            if (UnMinify) script.Append("\n ");
            script.Append(",title:'" + this.Title + "'");

            if (this.closable)
            {
                if (UnMinify) script.Append("\n ");
                script.Append(",closable:true");
            }

            if ((AutoHeight) && (FixedPixelHeight == 0))
            {
                if (UnMinify) script.Append("\n ");
                script.Append(",autoHeight:true");
            }

            if (FixedPixelHeight > 0)
            {
                if (UnMinify) script.Append("\n ");
                script.Append(",height:'" + FixedPixelHeight.ToString() + "'");
            }

            if ((AutoWidth) && (FixedPixelWidth == 0))
            {
                if (UnMinify) script.Append("\n ");
                script.Append(",autoWidth:true");
            }

            if (FixedPixelWidth > 0)
            {
                if (UnMinify) script.Append("\n ");
                script.Append(",width:" + FixedPixelWidth.ToString());
            }

            if (AutoScroll)
            {
                if (UnMinify) script.Append("\n ");
                script.Append(",autoScroll:true");
            }

            if (UnMinify) script.Append("\n ");
            script.Append("}");

            DidRenderScript = true;


        }

    }
}
