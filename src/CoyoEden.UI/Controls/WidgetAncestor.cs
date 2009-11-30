using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CoyoEden.Core;
using Vivasky.Core.Infrastructure;

namespace CoyoEden.UI.Controls
{
	public abstract class WidgetAncestor:UserControl
	{
		#region Properties
		public Widget BOWidget { get; set; }
		/// <summary>
		/// Gets or sets the title of the widget. It is mandatory for all widgets to set the Title.
		/// </summary>
		/// <value>The title of the widget.</value>
		public string Title { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [show title].
		/// </summary>
		/// <value><c>true</c> if [show title]; otherwise, <c>false</c>.</value>
		public bool ShowTitle { get; set; }
		/// <summary>
		/// Gets the widget ID.
		/// </summary>
		/// <value>The widget ID.</value>
		public Guid WidgetID { get; set; }
		public string CacheKey
		{
			get
			{
				return Widget.PREFIX_CACHEID + WidgetID;
			}
		}
		public SerializableStringDictionary CurrentSettings {
			get
			{
				return GetSettings();
			}
		}
		#endregion


		/// <summary>
		/// Get settings from data store
		/// </summary>
		/// <returns>Settings</returns>
		public SerializableStringDictionary GetSettings()
		{
			if (Cache[CacheKey] != null)
			{
				BOWidget = Cache[CacheKey] as Widget;
			}
			else
			{
				Cache[CacheKey] = BOWidget;
			}
			return BOWidget.ExtConfigs;
		}
		/// <summary>
		/// Saves settings to data store
		/// </summary>
		/// <param name="settings">Settings</param>
		protected virtual void SaveSettings(SerializableStringDictionary settings)
		{

			foreach (string key in settings.Keys)
			{
				if (BOWidget.ExtConfigs.ContainsKey(key))
				{
					BOWidget.ExtConfigs.Remove(key);
				};
				BOWidget.ExtConfigs.Add(key, settings[key]);
			}
			BOWidget.Save();
			Cache[CacheKey] = BOWidget;
		}
	}
}
