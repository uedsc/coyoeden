using System;
using System.Globalization;
using System.Web.UI.Design;

namespace mojoPortal.Web.Editor
{
    /// <summary>
    /// Author:		        Joe Audette
    /// Created:            2007/05/26
    /// Last Modified:      2007/05/28
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class TinyMCEDesigner : ControlDesigner
    {
        public TinyMCEDesigner()
        {
        }

        public override string GetDesignTimeHtml()
        {
            TinyMCETextBox control = (TinyMCETextBox)Component;
            return String.Format(CultureInfo.InvariantCulture,
                "<div><table width=\"{0}\" height=\"{1}\" bgcolor=\"#f5f5f5\" bordercolor=\"#c7c7c7\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\"><tr><td valign=\"middle\" align=\"center\">TinyMCE<b>{2}</b></td></tr></table></div>",
                    control.Width,
                    control.Height,
                    control.ID);
        }
    }
}
