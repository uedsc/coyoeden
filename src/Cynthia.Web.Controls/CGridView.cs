using System;
using System.Web.UI.WebControls;

namespace Cynthia.Web.Controls
{
    /// <summary>
    /// The only purposeof this control is because we use the CSS Control Adapter to extend it
    /// rather thasn extend the standard GridView directly. This allows the standard GridView to work
    /// as expected for those who don't want to use the Adapter.
    /// http://www.asp.net/cssadapters/gridview.aspx
    /// http://www.vivasky.com/Forums/Thread.aspx?thread=2276&mid=34&pageid=5&ItemID=4
    /// 
    /// </summary>
    public class CGridView : GridView
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //if (Page.IsPostBack) { return; }
            //if (this.Rows.Count == 0) { this.Visible = false; }
        }
    }
}
