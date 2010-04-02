// Author:					Joe Audette
// Created:				    2009-07-26
// Last Modified:			2009-07-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// wrapper for http://plugins.learningjquery.com/cluetip/
    /// </summary>
    public class ClueTipHyperlink : HyperLink
    {

        private bool auto = true;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!(Page is CBasePage)) { return; }

            CBasePage CPage = Page as CBasePage;
            CPage.ScriptConfig.IncludeClueTip = true;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (auto) 
            {
                if (CssClass.Length == 0)
                {
                    CssClass = "cluetiplink";
                }
                else
                {
                    CssClass += " cluetiplink";
                }
            }
        }
    }
}
