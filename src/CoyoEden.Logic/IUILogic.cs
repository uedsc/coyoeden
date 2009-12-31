using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivasky.Core.Infrastructure;
using Habanero.BO;

namespace CoyoEden.Logic
{
	/// <summary>
	/// ui logic
	/// </summary>
	public interface IUILogic<T,TId>
		where T:BusinessObject
	{
		event EventHandler<ActionEventArgs<T>> PrepareEditingUI;
		void OnPrepareEditingUI(TId id, out BOMessager msg);
		void OnPrepareEditingUI(T obj, out BOMessager msg);
	}
	public interface IUILogic<T> : IUILogic<T, Guid> 
		where T:BusinessObject
	{ }
}
