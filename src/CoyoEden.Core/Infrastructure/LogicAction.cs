using System;

namespace CoyoEden.Core.Infrastructure
{
	/// <summary>
	/// logic actions
	/// </summary>
	public enum LogicAction
	{
		None,
		PrepareEditingUI,
		Editing,
		AddNew,
		Update,
		Delete,
		Query
	}
}
