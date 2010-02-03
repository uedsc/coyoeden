using System;
using System.Collections.Generic;
using CoyoEden.Core;

namespace CoyoEden.UI.Views
{
	public class WidgetListViewM:CommonItemListViewM<Widget>
	{
		public override List<Widget> AllItems
		{
			get
			{
				if (Zone != null) {
					return Zone.WidgetList;
				}
				return Widget.AllWidgets;
			}
		}
		public WidgetZone Zone { get; set; }
	}
}
