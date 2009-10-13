#region Using

using System;
#endregion
namespace CoyoEden.UI.Controls
{
	/// <summary>
	/// Summary description for WidgetBase
	/// </summary>
	public abstract class WidgetEditBase : WidgetAncestor
	{
		public WidgetEditBase()
		{
		}

		/// <summary>
		/// Saves this the basic widget settings such as the Title.
		/// </summary>
		public abstract void Save();

		public static event EventHandler<EventArgs> Saved;
		/// <summary>
		/// Occurs when the class is Saved
		/// </summary>
		public static void OnSaved()
		{
			if (Saved != null)
			{
				Saved(null, new EventArgs());
			}
		}

	}
}
