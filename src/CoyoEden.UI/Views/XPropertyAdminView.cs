using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core;

namespace CoyoEden.UI.Views
{
	public class XPropertyAdminView:ViewBase
	{
		/// <summary>
		/// All extensive properties
		/// </summary>
		public List<XProperty> XPropertyList {
			get
			{
				return XProperty.XProperties;
			}
		}
	}
}
