using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace CoyoEden.UI.Views
{
	public class CommonItemListViewM<T>:ViewBase
	{
		public virtual List<T> AllItems { get; set; }
		public virtual List<T> ItemsPaged {
			get
			{
				var retVal= AllItems.Skip(PageIndex * PageSize).Take(PageSize).ToList();
                if (!string.IsNullOrEmpty(SortName)) {
                    if (Ascending)
                    {
                        retVal = retVal.AsQueryable().OrderBy("{0} {1}", SortName, "asc").ToList();
                    }
                    else {
                        retVal = retVal.AsQueryable().OrderBy("{0} {1}",SortName,"desc").ToList();
                    }
                }
                return retVal;
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
        /// <summary>
        /// sorting property
        /// </summary>
        public virtual string SortName { get; set; }
        /// <summary>
        /// sorting direction.DESC OR ASC
        /// </summary>
        public virtual bool Ascending { get; set; }
	}
}
