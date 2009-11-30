
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using Vivasky.Core.Infrastructure;
using Vivasky.Core.Utility;
using System.Collections.Generic;
using System.IO;
using Vivasky.Core;
using System.Linq;
using System.Xml.Linq;
namespace CoyoEden.Core
{

    public partial class Widget
	{
		#region .ctor
		public Widget() {
			Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
			this.Saved+=(s,e)=>{
				AllWidgets.RemoveAll(x=>x.Id.Equals(this.Id));
				if (!this.Status.IsDeleted) {
					AllWidgets.Add(this);
				}
			};
			//default color xproperty
			var colorProperty = XProperty.GetXProperty("WidgetColor");
			if (colorProperty != null&&colorProperty.XPropertySettings.Count>0)
			{
				var colorIndex = new Random().Next(colorProperty.XPropertySettings.Count);
				AddExtConfig("WidgetColor", colorProperty.XPropertySettings[colorIndex].SettingValue);
			}
		}
		#endregion

		public const string PREFIX_CACHEID = "cy_widget_";

		#region biz methods
		/// <summary>
		/// Use this method to add extension config
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public void AddExtConfig(string key, string val)
		{
			ExtConfigs.AddStringDicEntry(key, val, (dic) =>
			{
				ExtConfig = ((SerializableStringDictionary)dic).ToXML();
			});
		}
		#endregion

		#region properties
		public SerializableStringDictionary ExtConfigs
		{
			get
			{
				if (string.IsNullOrEmpty(ExtConfig))
				{
					return new SerializableStringDictionary();
				}
				return XmlUtil.Deserialize<SerializableStringDictionary>(ExtConfig);
			}
		}

		#endregion


		#region factory methods
		private static readonly object _SynHelper = new object();
		private static List<Widget> _allWidgets;
		public static List<Widget> AllWidgets
		{
			get
			{
				if (_allWidgets == null)
				{
					lock (_SynHelper)
					{
						if (_allWidgets == null)
						{
							_allWidgets = LoadAll();
						}
					}
				}
				return _allWidgets;
			}
		}
		public static List<Widget> LoadAll()
		{
			return Broker.GetBusinessObjectCollection<Widget>("Id is not null");
		}
		public static List<string> WidgetModels
		{
			get
			{
				var retVal = new List<string>();
				var dir = new DirectoryInfo(Utils.ConvertToPhysicalPath("widgets", true));
				foreach (DirectoryInfo obj in dir.GetDirectories())
				{
					if (obj.Name.StartsWith("_") || obj.Name.StartsWith(".")) continue;
					if (File.Exists(Path.Combine(obj.FullName, "widget.ascx")))
					{
						retVal.Add(obj.Name);
					}
				};
				return retVal;
			}
		}
		/// <summary>
		/// find by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Widget Find(Guid id) {
			return AllWidgets.SingleOrDefault(x => x.Id.Value.Equals(id));
		}
		#endregion
	}
}
