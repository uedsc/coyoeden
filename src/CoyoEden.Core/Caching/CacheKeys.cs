using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Core.Caching
{
	public struct CacheKeys
	{
		public const string All_WidgetZone = "WidgetZones";
		public const string List_Widgets_Zone = "WidgetList-{0}";//{0} is the placeholder of zone id.
	}
}
