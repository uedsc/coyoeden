using System;

namespace CoyoEden.Core.DataContracts
{
	/// <summary>
	/// Wrap around xml document
	/// </summary>
	[Serializable()]
	public class WidgetData
	{
		/// <summary>
		/// Defatul constructor
		/// </summary>
		public WidgetData() { }

		private string settings = string.Empty;
		/// <summary>
		/// Settings data
		/// </summary>
		public string Settings { get { return settings; } set { settings = value; } }
	}
}
