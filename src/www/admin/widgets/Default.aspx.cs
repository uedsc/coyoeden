using System;
using CoyoEden.UI;

public partial class admin_widgets_Default :AdminBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		yPager1.TotalItems = WidgetList1.ItemCount;
		WidgetList1.PageIndex = yPager1.CurrentPage;
		WidgetList1.PageSize = yPager1.ItemsPerPage;
	}
	protected void yPager1_OnPageChanged(object sender, EventArgs e)
	{
		WidgetList1.PageIndex = yPager1.CurrentPage;
	}
}
