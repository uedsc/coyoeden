using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core;
using CoyoEden.Core.Infrastructure;
using CoyoEden.Core.DataContracts;

namespace CoyoEden.UI.Views
{
	public class XPropertyListViewM:ViewBase
	{
		/// <summary>
		/// All extensive properties
		/// </summary>
		public List<XProperty> XPropertyList {
			get
			{
				var allItems=XProperty.XProperties;

				return allItems.Skip(PageIndex * PageSize).Take(PageSize).ToList();
			}
		}
		public int ItemCount {
			get
			{
				return XProperty.XProperties.Count;
			}
		}
		public int PageIndex { get; set; }
		public int PageSize { get; set; }

	}
}
