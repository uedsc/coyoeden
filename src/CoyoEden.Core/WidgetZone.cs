
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using Vivasky.Core.Services;
using System.Collections.Generic;
using System.Linq;
using Vivasky.Core.Infrastructure;
using CoyoEden.Core.DataContracts;
using Vivasky.Core;
namespace CoyoEden.Core
{   
    public partial class WidgetZone:ICacheable
	{

		#region .ctor
		public WidgetZone() {
			Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
		}
		public WidgetZone(string name, string tag):this() {
			Name = name;
			Tag = tag;
		}
		#endregion

		#region factory methods
		private static readonly object _SynHelper = new object();
		private static List<WidgetZone> _allWidgetZones;
		public static List<WidgetZone> AllWidgetZones
		{
			get
			{
				if (_allWidgetZones == null)
				{
					lock (_SynHelper)
					{
						if (_allWidgetZones == null)
						{
							_allWidgetZones = LoadAll();
						}
					}
				}
				return _allWidgetZones;
			}
		}
		public static List<WidgetZone> LoadAll()
		{
			return Broker.GetBusinessObjectCollection<WidgetZone>("Id is not null");
		}
		/// <summary>
		/// Find by name.If not exists,return null
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static WidgetZone Find(string name) {
			return AllWidgetZones.SingleOrDefault(x=>x.Name==name);
		}
		/// <summary>
		/// load by name.if not exists,create a new one.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static WidgetZone Load(string name) {
			var data = Find(name);
			if (null == data) {
				data = new WidgetZone(name, "Normal");
				data.Save();
			}
			return data;
		}
		public static void Sort(WidgetSortingData sortData, out BOMessager msg) {
			msg = new BOMessager();
			if (sortData.Zone0 == sortData.Zone1) { 
				//sort in the same zone
				var zone = Find(sortData.Zone0);
				for (int i = 0; i < zone.WidgetList.Count; i++)
				{
					if (i < sortData.NewIndex) {
						continue;
					}
					zone.Widgets[i].DisplayIndex++;
				}
				//TODO:persistence
	
			} else { 
				//TODO:Handle the sort between different zones
			}
		}
		#endregion

		#region biz Methods
		public List<Widget> WidgetList {
			get
			{
				var cachedKey = Utils.GetCaller();
				var items = System.Web.HttpContext.Current.Cache[cachedKey] as List<Widget>;
				if (items == null) {
					items = Widgets;
					System.Web.HttpContext.Current.Cache[cachedKey] = items;
				}
				return items;
			}
		}
		#endregion

		#region ICacheable Members

		public void ClearCache()
		{
			_allWidgetZones = null;
		}

		#endregion
	}
}
