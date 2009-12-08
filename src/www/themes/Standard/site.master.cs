using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Vivasky.Core;

public partial class StandardSite : CoyoEden.UI.SiteMaster
{
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!UserIsAdmin) {
			cssSiteTop.Visible = false;
		}
	}
}
