using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivasky.Core.WebControls;
using CoyoEden.Core;

namespace CoyoEden.UI.Views
{
	public class SiteTop:ViewBase
	{
		public string UserName {
			get
			{
				var retVal= Page.User.Identity.Name;
				retVal = string.IsNullOrEmpty(retVal) ? null : retVal;
				return retVal;
			}
		}
		public IList<string> WidgetModels
		{
			get
			{
				return Widget.WidgetModels;
			}
		}
		public IList<WidgetZone> Zones
		{
			get
			{
				var retVal = WidgetZone.AllWidgetZones;
				if (!string.IsNullOrEmpty(Tag))
				{
					retVal = retVal.Where(x => x.Tag == Tag).ToList();
				};
				return retVal;
			}
		}

		protected void OnWidgetsLoad(object sender, EventArgs arg)
		{
			var ddl = sender as SiteDDSelect;
			if (ddl != null)
			{
				ddl.DataSource = WidgetModels.Cast<object>();
			}
		}
		protected void OnWidgetZonesLoad(object sender, EventArgs arg)
		{
			var ddl = sender as SiteDDSelect;
			if (ddl != null)
			{
				ddl.DataSource = Zones.Cast<object>();
			}
		}
	}
}
