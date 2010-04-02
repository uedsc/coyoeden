using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace Cynthia.Web.UI
{
    public class InsecurePageWarning : Label
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            
            if (Page.Request.IsSecureConnection)
            {
                this.Visible = false;
                return;
            }

            this.CssClass = "nosslwarning txterror";
            this.Text = Resource.NoSSLWarning;
            //this.Style.Add(HtmlTextWriterStyle.FontSize, "large");
            this.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            this.Style.Add(HtmlTextWriterStyle.Color, "red");

        }
    }
}
