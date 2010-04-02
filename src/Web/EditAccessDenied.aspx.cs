using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Cynthia.Business;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.UI.Pages 
{

	/// <summary>
	/// 
	/// The use and distribution terms for this software are covered by the 
	/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
	/// which can be found in the file CPL.TXT at the root of this distribution.
	/// By using this software in any fashion, you are agreeing to be bound by 
	/// the terms of this license.
	///
	/// You must not remove this notice, or any other, from this software.
	///  
	/// </summary>
    public partial class EditAccessDenied : CBasePage
	{
        

        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }

		private void Page_Load(object sender, System.EventArgs e)
		{

            lnkHome.Text = Resource.ReturnHomeLabel;
            lnkHome.ToolTip = Resource.ReturnHomeLabel;
            lnkHome.NavigateUrl = SiteRoot + "/Default.aspx";
			//lblEditAccessDeniedMessage.Text = ResourceHelper.GetMessageTemplate("EditAccessDeniedMessage.config");

		}

    }
}
