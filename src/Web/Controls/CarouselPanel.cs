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
    /// implemented this only to find it is not compatible with jquery 1.3.2
    /// http://interface.eyecon.ro/docs/carousel
    /// </summary>
    public class CarouselPanel : Panel
    {

        private string itemsSelector = "a";
        /// <summary>
        /// jquery selector for items to rotate, default is a
        /// </summary>
        public string ItemsSelector
        {
            get { return itemsSelector; }
            set { itemsSelector = value; }
        }

        private int itemWidth = 200;
        /// <summary>
        /// the max width for each item
        /// </summary>
        public int ItemWidth
        {
            get { return itemWidth; }
            set { itemWidth = value; }
        }

        private int itemHeight = 200;
        /// <summary>
        /// the max height for each item
        /// </summary>
        public int ItemHeight
        {
            get { return itemHeight; }
            set { itemHeight = value; }
        }

        private int itemMinWidth = 50;
        /// <summary>
        /// the minimum width for each item, the height is automaticaly calculated to keep proportions
        /// </summary>
        public int ItemMinWidth
        {
            get { return itemWidth; }
            set { itemWidth = value; }
        }

        private float rotationSpeed = 1.8f;
        /// <summary>
        /// the speed for rotation animation
        /// </summary>
        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        private float reflectionSize = .5f;
        /// <summary>
        /// the reflection size a fraction from items' height
        /// </summary>
        public float ReflectionSize
        {
            get { return reflectionSize; }
            set { reflectionSize = value; }
        }

        private bool slowOnHover = false;
        /// <summary>
        /// if true the rotation speed slows down when an item is hovered
        /// </summary>
        public bool SlowOnHover
        {
            get { return slowOnHover; }
            set { slowOnHover = value; }
        }

        private bool slowOnOut = false;
        /// <summary>
        /// if true the rotation speed slows down when an item is hovered
        /// </summary>
        public bool SlowOnOut
        {
            get { return slowOnOut; }
            set { slowOnOut = value; }
        }


        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$('#" + ClientID + "').Carousel(");
            script.Append("{");
            script.Append("itemWidth:" + itemWidth.ToInvariantString());
            script.Append(",itemHeight:" + itemHeight.ToInvariantString());
            script.Append(",itemMinWidth:" + itemMinWidth.ToInvariantString());
            script.Append(",items:'" + itemsSelector + "'");
            script.Append(",rotationSpeed:" + rotationSpeed.ToInvariantString());
            script.Append(",reflectionSize:" + reflectionSize.ToInvariantString());
            if (slowOnHover)
            {
                script.Append(",slowOnHover:true");
            }

            if (slowOnOut)
            {
                script.Append(",slowOnOut:true");
            }

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

    }
}
