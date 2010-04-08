// Author:					Joe Audette
// Created:					2009-04-16
// Last Modified:			2009-04-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.UI;

namespace Cynthia.Modules.UI.LiveMessenger
{
    public partial class MessengerThemeSetting : UserControl, ISettingControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region ISettingControl

        public string GetValue()
        {
            return ddTheme.SelectedValue;
        }

        public void SetValue(string val)
        {
            ListItem item = ddTheme.Items.FindByValue(val);
            if (item != null)
            {
                ddTheme.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion

    }
}