using System;
using NUnit.Framework; 
using SystemX;
using CoyoEden.Core.DataContracts;
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
	}
}
