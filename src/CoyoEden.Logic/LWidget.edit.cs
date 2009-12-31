using System;
using CoyoEden.Core;
using Vivasky.Core.Infrastructure;
using Vivasky.Core;

namespace CoyoEden.Logic
{
	/// <summary>
	/// Logic for Widget
	/// </summary>
	public partial class LWidget:IEditLogic<Widget>
	{
		#region IEditLogic<Widget,Guid> Members
		public event EventHandler<ActionEventArgs<Widget>> Editing;

		public void OnEditing(Widget obj)
		{
			throw new NotImplementedException();
		}

		public event EventHandler<ActionEventArgs<Widget>> Edited;

		public void OnEdited(Widget obj)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
