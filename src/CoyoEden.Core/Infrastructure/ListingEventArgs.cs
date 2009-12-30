using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Core.Infrastructure
{
	public class ListingEventArgs<T>:EventArgs
	{
		public List<T> ItemList { get; set; }
		public ListingEventArgs(List<T> items) {
			ItemList = items;
		}
	}
}
