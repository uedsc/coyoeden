using System;
using SystemX.Infrastructure;

namespace CoyoEden.Core.Infrastructure
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
	public class ActionEventArgs : ActionEventArgs<object>
	{
		public ActionEventArgs()
		{
			
		}
		public ActionEventArgs(LogicAction action, object target)
			: base(action, target)
		{
			
		}
		public ActionEventArgs(LogicAction action, object target, ref BOMessager msg)
			: base(action, target, ref msg)
		{
			
		}
                                                             }
}
