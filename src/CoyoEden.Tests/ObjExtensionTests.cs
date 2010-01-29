using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework; 
using Vivasky.Core;
using CoyoEden.Core.DataContracts;
using Vivasky.Core.Infrastructure;
using System.Collections.Specialized;
namespace CoyoEden.Tests
{
	[TestFixture]
	public class ObjExtensionTests
	{
		[Test]
		public void as_T_test0()
		{
			//arrange
			var x1 ="0";
			var x2 = 1;
			var x3 = "Filter";
			//act
			var y1 = x1.As<ActionTypes>();
			var y2 = x2.As<ActionTypes>();
			var y3 = x3.As<ActionTypes>();
			//assert
			Assert.IsTrue(y1==ActionTypes.Unkown);
			Assert.IsTrue(y2 == ActionTypes.Filter);
			Assert.IsTrue(y3 == ActionTypes.Filter);
		}
		[Test]
		public void as_T_test1()
		{
			//arrange
			var x = "2010-01-01";
			//act
			var y = x.As<DateTime>();
			//assert
			Assert.IsTrue(y.Year == 2010);
		}

		[Test]
		public void str_dic_to_json()
		{
			//arrange
			var dic = new NameValueCollection();
			dic.Add("a", "1");
			dic.Add("t", "2009-09-09");

			var dic1 = dic.ToStrDic();

			var dic2 = new Dictionary<string, string>();
			dic2.Add("a", "1");
			dic2.Add("t", "2009-09-09");
			//act
			var json = dic.ToJSONStr();
			var json1 = dic1.ToJSONStr();
			var json1_1 = dic1.ToJSONStr1();
			var json2 = dic2.ToJSONStr();
			//assert
			Console.WriteLine(json);
			Console.WriteLine(json1);
			Console.WriteLine(json1_1);
			Console.WriteLine(json2);

		}  
	}
}
