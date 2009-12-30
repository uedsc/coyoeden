using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivasky.Core.Cache;

namespace CoyoEden.Core.Caching
{
	public class CacheManager
	{
		public static void Init() {
			WidgetZone.LoadingAll += (s, e) => {
				e.ItemList = Cacher<WidgetZone>.Get<List<WidgetZone>>(CacheKeys.All_WidgetZone);
			};
			WidgetZone.LoadedAll += (s, e) => {
				Cacher<WidgetZone>.Remove(CacheKeys.All_WidgetZone);
				Cacher<WidgetZone>.Add(CacheKeys.All_WidgetZone, e.ItemList);
			};

			WidgetZone.LoadingWidgets += (s, e) => {
				e.ItemList = Cacher<WidgetZone>.Get<List<Widget>>(string.Format(CacheKeys.List_Widgets_Zone, ((WidgetZone)s).Id));
			};
			WidgetZone.LoadedWidgets += (s, e) => {
				var key=string.Format(CacheKeys.List_Widgets_Zone, ((WidgetZone)s).Id);
				Cacher<WidgetZone>.Remove(key);
				Cacher<WidgetZone>.Add(key, e.ItemList);
			};
		}
	}
}
