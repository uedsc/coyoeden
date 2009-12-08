using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Core.DataContracts
{
	public class WidgetSortingData
	{
		/// <summary>
		/// widget id
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// New index of the widget 
		/// </summary>
		public int NewIndex { get; set; }
		/// <summary>
		/// old zone
		/// </summary>
		public string Zone0 { get; set; }
		/// <summary>
		/// new zone
		/// </summary>
		public string Zone1 { get; set; }
	}
}
