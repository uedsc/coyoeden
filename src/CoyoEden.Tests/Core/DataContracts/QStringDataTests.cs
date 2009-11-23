using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CoyoEden.Core.DataContracts; 
using Vivasky.Core;
namespace CoyoEden.Tests.Core.DataContracts
{
	[TestFixture]
	public class QStringDataTests
	{
		[Test]
		public void can_parse_str()
		{
			//arrange
			var data = "a-1$v-true$lb-2008$ub-2009";
			//act
			var obj = QStringData.New(data);
			//assert
			Assert.IsTrue(obj.a == QueryTypes.Filter);
			Console.WriteLine(obj.ToJSONStr());
			Console.WriteLine(obj.ToString());
		}
		[Test]
		public void can_set_property() {
			//arrange
			var data = "a-1$v-true$lb-2008$ub-2009";
			//act
			var obj = QStringData.New(data);
			obj.Set("a", 2);
			obj.Set("v", true);
			//assert
			Console.WriteLine(obj.ToString());
		}
	}
}
