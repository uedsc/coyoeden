using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoyoEden.UI;

public partial class admin_Pages_XProperties :AdminBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		yPager1.TotalItems = Xprolist.ItemCount;
		Xprolist.PageIndex = yPager1.CurrentPage;
		Xprolist.PageSize = yPager1.ItemsPerPage;
	}
	protected void yPager1_OnPageChanged(object sender, EventArgs e) {
		Xprolist.PageIndex = yPager1.CurrentPage;
	}
}
