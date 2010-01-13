using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivasky.Core.Web;
using System.Web.Script.Services;
using Vivasky.Core.Infrastructure;
using System.Web.Services;
using CoyoEden.Core;
using CoyoEden.Core.DataContracts;
using CoyoEden.Core.Infrastructure;
using CoyoEden.UI.Views;

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
        [WebMethod(true)]
        public BOMessager Delete(IdInputData<Guid> data)
        {
            var retVal = new BOMessager();
            Widget.Remove(data.Id, out retVal);
            return retVal;
        }
        [WebMethod(true)]
        public BOMessager Add(WidgetAddingData data)
        {
            var retVal = new BOMessager();
            var zone = WidgetZone.Find(data.Zone);
            if (zone == null)
            {
                retVal.Error<WidgetZone>(BOMessager.NORECORD_X, data.Zone);
            }
            else {
                zone.AddWidget(data.Name, out retVal);
            }
            return retVal;
        }
	}
}
