using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CoyoEden.Core;
using CoyoEden.Core.DataContracts;
using SystemX.Infrastructure;
using CoyoEden.Core.Infrastructure;
using System.Xml;
 
namespace CoyoEden.Tests.Core
{
	[TestFixture]
	public class SettingTests:TestBase
	{
		[Test]
		public void can_get_all_settings()
		{
			//arrange & act
			var items = Setting.AllSettings;
			//assert
			Assert.IsTrue(items.Count>0);
			Console.Write(items.Count);
		}
		[Test]
		public void can_find_setting() { 
			//arrange & act
			var item = Setting.Find(SettingTypes.Widget, "cy_WidgetZone0");
			//assert
			Assert.IsTrue(item != null);
			Console.Write(item.SettingValue);
		}
		[Test]
		public void can_get_setting_as_xmldocument()
		{
			//arrange & act
			var settingSrv = ServiceLocatorX<ISettingsBehavior>.GetService();
			var xdoc = settingSrv.GetSettings<XmlDocument>(SettingTypes.Widget, "cy_WidgetZone0");
			var xdoc1 = new XmlDocument();
			xdoc1.InnerXml = settingSrv.GetSettings<string>(SettingTypes.Widget, "cy_WidgetZone0");
			var widgets = xdoc1.SelectNodes("//widget");
			//assert
			Assert.IsNotNull(xdoc);
			Console.WriteLine(xdoc.OuterXml);

			//Assert.IsTrue(widgets.Count>0);
			Console.WriteLine(xdoc1.OuterXml);


		} 
	}
}
