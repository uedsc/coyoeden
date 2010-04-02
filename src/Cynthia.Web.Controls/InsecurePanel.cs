/// Author:					Joe Audette
/// Created:				3/24/2006
/// Last Modified:			3/24/2007


using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Cynthia.Web.Controls
{
    /// <summary>
    /// I use this to wrap things so they are not displayed 
    /// when using https/ssl, otherwise the user gets browser warnings about
    /// the insecure items in the request
    /// </summary>
    public class InsecurePanel : Panel
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            if (Page.Request.IsSecureConnection) this.Visible = false;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            base.Render(writer);
        }
    }
}
