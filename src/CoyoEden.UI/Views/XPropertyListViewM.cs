using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core;
using CoyoEden.Core.Infrastructure;
using CoyoEden.Core.DataContracts;

namespace CoyoEden.UI.Views
{
	public class XPropertyListViewM:CommonItemListViewM<XProperty>
	{
		public override List<XProperty> AllItems
		{
			get
			{
				return XProperty.XProperties;
			}
		}

	}
}
