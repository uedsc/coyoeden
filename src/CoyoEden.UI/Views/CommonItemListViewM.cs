using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.UI.Views
{
	public class CommonItemListViewM<T>:ViewBase
	{
		public virtual List<T> AllItems { get; set; }
		public virtual List<T> ItemsPaged {
			get
			{
				return AllItems.Skip(PageIndex * PageSize).Take(PageSize).ToList();
			}
		}
		public virtual int ItemCount
		{
			get
			{
				return AllItems.Count;
			}
		}
		public virtual int PageIndex { get; set; }
		public virtual int PageSize { get; set; }
	}
}
