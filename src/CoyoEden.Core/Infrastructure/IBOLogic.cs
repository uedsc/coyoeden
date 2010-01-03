using System;
using System.Collections.Generic;
using Habanero.BO;
using Vivasky.Core.Infrastructure;

namespace CoyoEden.Core.Infrastructure
{
	/// <summary>
	/// Business object common logic interface
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TId"></typeparam>
	public interface IBOLogic<T,TId> where T : BusinessObject
	{
		#region edit logic interfaces
		event EventHandler<ActionEventArgs<T>> Editing;
		void OnEditing(T obj);
		event EventHandler<ActionEventArgs<T>> Edited;
		void OnEdited(T obj);
		#endregion

		#region query logic interfaces
		event EventHandler<ActionEventArgs<T>> Querying;
		event EventHandler<ActionEventArgs<T>> Queried;
		T OnFind(TId id, out BOMessager msg);
		List<T> OnFindAll(out BOMessager msg);
		#endregion

		#region ui logic interfaces
		event EventHandler<ActionEventArgs<T>> PrepareEditingUI;
		void OnPrepareEditingUI(TId id, out BOMessager msg);
		void OnPrepareEditingUI(T obj, out BOMessager msg);
		#endregion
	}
	public interface IBOLogic<T> : IBOLogic<T, Guid>
		where T : BusinessObject
	{ }

}
