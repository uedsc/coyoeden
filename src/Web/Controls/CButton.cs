//	Created:			    2010-02-20
//	Last Modified:		    2010-02-20
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
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// inherits button and adds extra markup for Artisteer if RenderArtisteer is true
    /// </summary>
    public class CButton : Button
    {
        private bool renderArtisteer = false;
        public bool RenderArtisteer
        {
            get { return renderArtisteer; }
            set { renderArtisteer = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (renderArtisteer)
            {
                writer.Write("<span class=\"art-button-wrapper\">");
                writer.Write("<span class=\"l\"> </span>\n");
                writer.Write("<span class=\"r\"> </span>\n");
            }

            base.Render(writer);

            if (renderArtisteer)
            {
                writer.Write("\n</span>");
            }
        }

    }
}
