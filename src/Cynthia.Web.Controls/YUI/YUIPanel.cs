using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web.Controls
{
    /// <summary>
    ///	Created:			    2008-09-10
    ///	Last Modified:		    2008-09-10
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.	
    /// 
    /// The purpose of this control is just to provide a convenient panel that adds the css class to the YUI skin
    /// </summary>
    public class YUIPanel : Panel
    {
        private string skinName = string.Empty; //"yui-skin-sam";

        public string SkinName
        {
            get { return skinName; }
            set { skinName = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            // if skin name is explicitly set on control don't override from eb.config
            if (skinName.Length == 0)
            {
                if (ConfigurationManager.AppSettings["YUIDefaultSkinClass"] != null)
                {
                    skinName = ConfigurationManager.AppSettings["YUIDefaultSkinClass"];
                }
            }

        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (this.CssClass.Length > 0)
            {
                this.CssClass += " " + skinName;
            }
            else
            {
                this.CssClass = skinName;
            }


        }
    }
}
