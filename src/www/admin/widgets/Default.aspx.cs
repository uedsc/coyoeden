using System;
using System.Linq;
using CoyoEden.UI;
using CoyoEden.Core;
using Vivasky.Core.WebControls;

public partial class admin_widgets_Default :AdminBasePage
{
    /// <summary>
    /// Current selected Zone
    /// </summary>
    protected WidgetZone CurZone { get; set; }
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(QStrData.t0)) {
			CurZone= WidgetZone.Find(QStrData.t0);
            WidgetList1.Zone = CurZone;
		}
		yPager1.TotalItems = WidgetList1.ItemCount;
		WidgetList1.PageIndex = yPager1.CurrentPage;
		WidgetList1.PageSize = yPager1.ItemsPerPage;
	}
	protected void yPager1_OnPageChanged(object sender, EventArgs e)
	{
		WidgetList1.PageIndex = yPager1.CurrentPage;
	}
    protected void OnWidgetsLoad(object sender, EventArgs arg)
    {
        var ddl = sender as SiteDDSelect;
        if (ddl != null)
        {
            ddl.DataSource = Widget.WidgetModels.Cast<object>();
        }
    }
    protected void OnWidgetZonesLoad(object sender, EventArgs arg)
    {
        var ddl = sender as SiteDDSelect;
        if (ddl != null)
        {
            ddl.DataSource = WidgetZone.AllWidgetZones.Cast<object>();
        }
    }
}
