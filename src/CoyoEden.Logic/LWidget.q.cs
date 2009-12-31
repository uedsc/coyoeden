using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core;
using Vivasky.Core.Infrastructure;

namespace CoyoEden.Logic
{
	public partial class LWidget:IQueryLogic<Widget>
	{
		#region IQueryLogic<Widget,Guid> Members

		public event EventHandler<ActionEventArgs<Widget>> Querying;

		public event EventHandler<ActionEventArgs<Widget>> Queried;

		public Widget OnFind(Guid id, out BOMessager msg)
		{
			msg=new BOMessager();
			var retVal=default(Widget);
			var args=new ActionEventArgs<Widget>(LogicAction.Query, retVal, ref msg);
			if (null != Querying) { Querying(null, args); };
			retVal = args.Target;
			if (null != retVal) return retVal;
			retVal = Widget.Find(id);
			if (null != retVal)
			{
				if (null != Queried)
				{
					Queried(null, args);
				}
			}
			else {
				msg.Error<Widget>(BOMessager.NORECORD_X, id);
			}
			return retVal;
		}

		public List<Widget> OnFindAll(out BOMessager msg)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
