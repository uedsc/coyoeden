using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.BO;
using Vivasky.Core.Infrastructure;

namespace CoyoEden.Logic
{
	public interface IQueryLogic<T,TId>
		where T:BusinessObject
	{
		event EventHandler<ActionEventArgs<T>> Querying;
		event EventHandler<ActionEventArgs<T>> Queried;
		T OnFind(TId id, out BOMessager msg);
		List<T> OnFindAll(out BOMessager msg);
	}
	public interface IQueryLogic<T> : IQueryLogic<T, Guid>
		where T:BusinessObject
	{ 
	}
}
