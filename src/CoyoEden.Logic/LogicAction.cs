using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Logic
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
