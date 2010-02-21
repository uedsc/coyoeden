using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CoyoEden.Core.BootStrappers;
using CoyoEden.Core;

namespace CoyoEden.Tests
{
	[TestFixture]
	public class TestBase
	{

		[SetUp]
		public virtual void setup() {
			//Setup SystemX
			var app = new ApplicationBoot("CoyoEden", BlogSettings.AppVersion())
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
