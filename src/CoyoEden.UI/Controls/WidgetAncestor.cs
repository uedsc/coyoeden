using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CoyoEden.Core;
using SystemX.Infrastructure;

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
		public string Title {
            get
            {
                return (BOWidget!=null)?BOWidget.Title:null;
            }
            set
            {
            	BOWidget.Title = value;
            }
        }
		/// <summary>
		/// Gets or sets a value indicating whether [show title].
		/// </summary>
		/// <value><c>true</c> if [show title]; otherwise, <c>false</c>.</value>
		public bool ShowTitle {
            get
            {
                return BOWidget.ShowTitle.Value;
            }
            set
            {
            	BOWidget.ShowTitle = value;
            }
        }
		/// <summary>
		/// Gets the widget ID.
		/// </summary>
		/// <value>The widget ID.</value>
		public Guid WidgetID {
            get
            {
                return BOWidget!=null?BOWidget.Id.Value:Guid.Empty;
            }
        }

		public SerializableStringDictionary CurrentSettings {
			get
			{
				return GetSettings();
			}
		}
        /// <summary>
        /// add an extra setting for the widget
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public WidgetAncestor AddSetting(string name, string value) {
            BOWidget.AddExtConfig(name, value);
            return this;
        }
		#endregion


		/// <summary>
		/// Get settings from data store
		/// </summary>
		/// <returns>Settings</returns>
		public SerializableStringDictionary GetSettings()
		{
			return BOWidget.ExtConfigs;
		}
		/// <summary>
		/// update the related business object.
		/// </summary>
		public virtual void Update()
		{
			BOWidget.Save();
		}
	}
}
