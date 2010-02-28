using System;
using SystemX.WebControls;
using CoyoEden.Core;

namespace CoyoEden.UI.Controls
{
    /// <summary>
    /// support reference css files in the current theme!
    /// </summary>
    public class SiteCssX:SiteCSS
    {
        public string CssPathInTheme { get; set; }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(CssPathInTheme)) {
                CSSRelativeToRoot = string.Format("{0}themes/{1}/{2}",SystemX.Web.Utils.AbsoluteWebRoot, BlogSettings.Instance.Theme, CssPathInTheme);
            }
            base.Render(writer);
        }
    }
}
