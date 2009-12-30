
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
using CoyoEden.Core.Infrastructure;
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
							onLoadingAll(ref _allWidgetZones);
							if (_allWidgetZones == null)
							{
								_allWidgetZones = LoadAll();
								onLoadedAll(_allWidgetZones);
							}
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
				if (zone.WidgetList.Count > 0) {
					var item = zone.WidgetList.Find(x => x.Id.Value.Equals(sortData.Id));
					zone.WidgetList.Remove(item);
					zone.WidgetList.Insert(sortData.NewIndex, item);
					for (int i = 0; i < zone.WidgetList.Count; i++)
					{
						zone.WidgetList[i].DisplayIndex = i;
					}
				}
				//persistence
				zone.Widgets.Clear();
				zone.Widgets.AddRange(zone.WidgetList);
				zone.Widgets.SaveAll();
				onSorted(zone, zone.WidgetList);
	
			} else { 
				//TODO:Handle the sort between different zones
			}
		}
		#endregion

		#region biz Methods
		public List<Widget> WidgetList {
			get
			{
				var items=default(List<Widget>);
				onLoadingWidgets(this,ref items);
				if (items == null) {
					items = Widgets.OrderBy(x=>x.DisplayIndex).ToList();
					onLoadedWidgets(this,items);
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

		#region events
		public static event EventHandler<ListingEventArgs<WidgetZone>> LoadingAll;
		public static event EventHandler<ListingEventArgs<WidgetZone>> LoadedAll;
		static void onLoadingAll(ref List<WidgetZone> zones) {
			if (null != LoadingAll) {
				var args=new ListingEventArgs<WidgetZone>(zones);
				LoadingAll(null, args);
				zones = args.ItemList;
			}
		}
		static void onLoadedAll(List<WidgetZone> zones)
		{
			if (null != LoadingAll)
			{
				LoadedAll(null, new ListingEventArgs<WidgetZone>(zones));
			}
		}
		public static event EventHandler<ListingEventArgs<Widget>> LoadingWidgets;
		public static event EventHandler<ListingEventArgs<Widget>> LoadedWidgets;
		static void onLoadingWidgets(WidgetZone target,ref List<Widget> widgets)
		{
			if (null != LoadingWidgets)
			{
				var args= new ListingEventArgs<Widget>(widgets);
				LoadingWidgets(target,args);
				widgets = args.ItemList;
			}
		}
		static void onLoadedWidgets(WidgetZone target, List<Widget> widgets)
		{
			if (null != LoadedWidgets)
			{
				var args=new ListingEventArgs<Widget>(widgets);
				LoadedWidgets(target, args);
			}
		}
		public static event EventHandler<ListingEventArgs<Widget>> Sorted;
		static void onSorted(WidgetZone zone, List<Widget> items) {
			if (null != Sorted) {
				Sorted(zone, new ListingEventArgs<Widget>(items));
			}
		}
		#endregion
	}
}
