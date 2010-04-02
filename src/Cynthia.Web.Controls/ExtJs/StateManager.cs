using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace Cynthia.Web.Controls.ExtJs
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-11-04
    /// Last Modified:			2007-11-04
    ///		
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.	 
    /// </summary>
    public static class StateManager
    {
        //TODO: implement configuration and choice of providers?
        // for now just use cookieprovider, this maintains the state of the ui
        // for panel dragging sizing collapsing etc.

        public static void Require(Page currentPage)
        {

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n<!-- \n");

            script.Append("var ExtStateManager = {   ");
            script.Append("\n init : function(){  ");

            script.Append("Ext.state.Manager.setProvider(new Ext.state.CookieProvider());");

            script.Append(" } ");
            script.Append("} ");

            script.Append("\n Ext.EventManager.onDocumentReady(ExtStateManager.init, ExtStateManager, true);  ");


            script.Append("\n//--> ");
            script.Append(" </script>");


            currentPage.ClientScript.RegisterClientScriptBlock(
                typeof(StateManager),
                "ExtStateManager",
                script.ToString());

        }

    }
}
