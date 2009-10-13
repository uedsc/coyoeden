using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CoyoEden.Core;
 
namespace CoyoEden.Tests.Core
{
	[TestFixture] 
	public class WidgetZoneTests:TestBase
	{
		[Test]
		public void can_find_by_name()
		{
			//arrange & act
			var item = WidgetZone.Find("cy_WidgetZone0");
			//assert
			Assert.IsNotNull(item);
			Console.Write(item.Id);
		}
		[Test]
		public void can_get_widgets()
		{
			//arrange & act
			var item = WidgetZone.Find("cy_WidgetZone0");
			var items = item.Widgets;
			//assert
			Assert.IsTrue(items.Count>0);
			Console.Write(items.Count);
		} 
	}
}
