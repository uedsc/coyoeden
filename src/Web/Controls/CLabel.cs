using System.Web;
using System.Web.UI.WebControls;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// this extends ASP.NET Label and changes the behavior so that if the text property is empty the control does not render.
    /// The ASP.NET Label will render an empty span but we don't always want that
    /// </summary>
    public class CLabel : Label
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if(Text.Length > 0) { base.Render(writer); }
        }

    }
}
