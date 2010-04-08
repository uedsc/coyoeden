

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
				writer.Write(String.Format("[{0}]", this.ID));
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
