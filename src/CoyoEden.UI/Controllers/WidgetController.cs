using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivasky.Core.Web;
using System.Web.Script.Services;
using Vivasky.Core.Infrastructure;
using System.Web.Services;
using CoyoEden.Core.DataContracts;

namespace CoyoEden.UI.Controllers
{
	[ScriptService]
	public class WidgetController:ServiceBase
	{
		[WebMethod(true)]
		public BOMessager Sort(WidgetSortingData data) {
			var retVal = new BOMessager();
			CoyoEden.Core.WidgetZone.Sort(data, out retVal);
			return retVal;
		}
	}
}
