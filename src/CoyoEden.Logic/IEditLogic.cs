using System;
using Habanero.BO;
using Vivasky.Core.Infrastructure;
namespace CoyoEden.Logic
{
	public interface IEditLogic<T> where T:BusinessObject
	{
		event EventHandler<ActionEventArgs<T>> Editing;
		void OnEditing(T obj);
		event EventHandler<ActionEventArgs<T>> Edited;
		void OnEdited(T obj);
	}
}
