//	Author:                 Joe Audette
//  Created:			    2009-09-20
//	Last Modified:		    2009-09-26
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// 
    /// http://interface.eyecon.ro/docs/fisheye
    /// implemented this only to find it is not compatible with jquery 1.3.2
    /// </summary>
    public class FishEyePanel : Panel
    {
        private string container = "fisheyeContainter";
        ///// <summary>
        ///// container element selector
        ///// </summary>
        //public string Container
        //{
        //    get { return container; }
        //    set { container = value; }
        //}

        private string itemsSelector = "a";
        /// <summary>
        /// jquery selector for items to rotate, default is a
        /// </summary>
        public string ItemsSelector
        {
            get { return itemsSelector; }
            set { itemsSelector = value; }
        }

        private string itemsText = "span";
        /// <summary>
        /// jquery selector for item text
        /// </summary>
        public string ItemsText
        {
            get { return itemsText; }
            set { itemsText = value; }
        }

        private int itemWidth = 40;
        /// <summary>
        /// the minimum width for each item
        /// </summary>
        public int ItemWidth
        {
            get { return itemWidth; }
            set { itemWidth = value; }
        }

        private int maxWidth = 50;
        /// <summary>
        /// the maximum width for each item
        /// </summary>
        public int MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = value; }
        }

        private int proximity = 90;
        /// <summary>
        /// the distance from element that makes item to interact
        /// </summary>
        public int Proximity
        {
            get { return proximity; }
            set { proximity = value; }
        }

        private string valign = "bottom";
        /// <summary>
        /// vertical alignment, default is bottom
        /// </summary>
        public string VerticalAlignment
        {
            get { return valign; }
            set { valign = value; }
        }

        private string halign = "center";
        /// <summary>
        /// vertical alignment, default is center
        /// </summary>
        public string HorizontalAlignment
        {
            get { return halign; }
            set { halign = value; }
        }
        

        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$('#" + ClientID + "').Fisheye(");
            script.Append("{");

            script.Append("items:'" + itemsSelector + "'");
            script.Append(",itemsText:'" + itemsText + "'");
            script.Append(",container:'." + container + "'");
            script.Append(",itemWidth:" + itemWidth.ToInvariantString());
            script.Append(",maxWidth:" + maxWidth.ToInvariantString());
            script.Append(",proximity:" + proximity.ToInvariantString());
            script.Append(",halign:'" + halign + "'");
            script.Append(",valign:'" + valign + "'");

            script.Append("}");
            script.Append(");");

            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                //include the main script
                basePage.ScriptConfig.IncludeInterface = true;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupInstanceScript();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);

            writer.Write("<div class='" + container + "'>");

            base.RenderContents(writer);

            writer.Write("</div>");

            base.RenderEndTag(writer);
        }

    }
}
