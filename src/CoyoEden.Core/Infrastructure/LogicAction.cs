using System;

namespace CoyoEden.Core.Infrastructure
{
	/// <summary>
	/// logic actions
	/// </summary>
	public enum LogicAction
	{
		None,
		/// <summary>
		/// Showing a editing view
		/// </summary>
		EditingView,
		/// <summary>
		/// showing a displaying view
		/// </summary>
		DisplayingView,
		Editing,
		AddNew,
		Update,
		Delete,
		Query
	}
}
