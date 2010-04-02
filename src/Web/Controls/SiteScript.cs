/// Author:				    Joe Audette
/// Created:			    2004/09/18
///	Last Modified:		    2007/04/13
/// 
/// 13/04/2007   Alexander Yushchenko: code refactoring, made it WebControl instead of UserControl.
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
    
    public class SiteScript : WebControl
    {
        private string scriptRelativeToRoot = string.Empty;

        public string ScriptRelativeToRoot
        {
            get { return scriptRelativeToRoot; }
            set { scriptRelativeToRoot = value; }
        }

        public string Script
        {
            get { return "<script type='text/javascript' src='" + WebUtils.GetSiteRoot() + scriptRelativeToRoot + "'></script>"; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                //writer.Write("[" + this.ID + "]");
            }
            else
            {
                writer.Write(this.Script);
            }
        }
    }
}
