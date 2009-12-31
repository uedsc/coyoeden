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
using CoyoEden.Logic;
using CoyoEden.UI.UILogic;

namespace CoyoEden.UI.Controllers
{
	[ScriptService]
	public class WidgetController:ServiceBase
	{
		private IEditLogic<Widget> logic;
		private IQueryLogic<Widget> logicQ;
		private IUILogic<Widget> logicUI;
		#region .ctor
		public WidgetController():this(new LWidget()){}
		public WidgetController(IEditLogic<Widget> _logic) {
			logic = _logic;
			logicQ = (IQueryLogic<Widget>)logic;
			logicUI = new ULWidget(logicQ);
		}
		#endregion
		[WebMethod(true)]
		public BOMessager Sort(WidgetSortingData data) {
			var retVal = new BOMessager();
			CoyoEden.Core.WidgetZone.Sort(data, out retVal);
			return retVal;
		}
		[WebMethod(true)]
		public BOMessager Edit(IdInputData<Guid> data)
		{
			var retVal = new BOMessager();
			logicUI.OnPrepareEditingUI(data.Id, out retVal);
			return retVal;
		}
	}
}
