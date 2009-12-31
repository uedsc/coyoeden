using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivasky.Core.Infrastructure;

namespace CoyoEden.Logic
{
	public class ActionEventArgs<T>:EventArgs
	{
		public LogicAction Action { get; set; }
		public T Target { get; set; }
		public BOMessager Msg { get; set; }
		public ActionEventArgs() { }
		public ActionEventArgs(LogicAction action, T target) {
			Action = action;
			Target = target;
		}
		public ActionEventArgs(LogicAction action, T target,ref BOMessager msg)
		{
			Action = action;
			Target = target;
			Msg = msg;
		}

	}
}
