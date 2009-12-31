using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core;
using CoyoEden.Logic;
using Vivasky.Core.Infrastructure;
using Vivasky.Core;
using System.IO;
using System.Web;

namespace CoyoEden.UI.UILogic
{
	public partial class ULWidget:IUILogic<Widget>
	{
		#region .ctor
		public ULWidget():this(new LWidget()) { }
		public ULWidget(IQueryLogic<Widget> _logicQ) {
			logicQ = _logicQ;
		}
		#endregion
		private IQueryLogic<Widget> logicQ;
		public const string WidgetPathFormatStr_EDIT = "{0}widgets/{1}/edit.ascx"; 
		#region IUILogic<Widget,Guid> Members

		public event EventHandler<ActionEventArgs<Widget>> PrepareEditingUI;

		public void OnPrepareEditingUI(Guid id, out BOMessager msg)
		{
			var obj = logicQ.OnFind(id, out msg);
			if (msg.IsError) return;

			OnPrepareEditingUI(obj, out msg);
		}

		public void OnPrepareEditingUI(Widget obj, out BOMessager msg)
		{
			msg = new BOMessager();
			var ucPath = string.Format(WidgetPathFormatStr_EDIT, Utils.RelativeWebRoot, obj.Name);
			if (File.Exists(HttpContext.Current.Server.MapPath(ucPath))) { 
				//TODO:Use View manager to load the widget 
			}
		}

		#endregion
	}
}
