using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CoyoEden.Core.BootStrappers;
using Habanero.UI.Win;
using CoyoEden.Core;

namespace CoyoEden.Tests
{
	[TestFixture]
	public class TestBase
	{

		[SetUp]
		public virtual void setup() {
			//Setup Habanero
			var app = new HabaneroAppWin("CoyoEden", BlogSettings.AppVersion())
			{
				//ClassDefsFileName = Server.MapPath("~/app_data/ClassDefs.xml") 
				ClassDefsXml = BOBroker.GetClassDefsXml()
			};
			app.Startup();

			//setup ioc/di
			ServicesRegister.Boot();

		}
	}
}
