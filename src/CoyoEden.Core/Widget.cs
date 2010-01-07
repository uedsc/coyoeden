
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
using CoyoEden.Core.Infrastructure;
using System.Web;
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
			AddExtConfig(EXTCFG_COLOR, GetRandomColor());
		}
		#endregion

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
		/// <summary>
		/// TODO:fetch from config
		/// </summary>
		public string Icon
		{
			get
			{
				return String.Format("{0}themes/admin/img/icon_c100.jpg", Utils.AbsoluteWebRoot);
			}
		}
		public const string PREFIX_CACHEID = "cy_widget_";
		public const string EXTCFG_COLOR = "WidgetColor";
		public const string WidgetPathFormatStr_EDIT = "{0}widgets/{1}/edit.ascx"; 
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
		public string Color {
			get
			{
				var retVal = "null";
				if (ExtConfigs.ContainsKey(EXTCFG_COLOR))
				{
					retVal = ExtConfigs[EXTCFG_COLOR];
				}
				else {
					retVal = GetRandomColor();
					ExtConfigs.Add(EXTCFG_COLOR, retVal);
				}
				return retVal;
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
		public static Widget Find(Guid id,out BOMessager msg) {
			msg = new BOMessager();
			var obj=default(Widget);
			onQuerying(id.ToString(), ref obj, out msg);
			if (obj != null) return obj;
			obj = AllWidgets.Find(x=>x.Id.Value.Equals(id));
			if (obj == null)
			{
				msg.Error<Widget>(BOMessager.NORECORD_X, id);
			}
			else {
				onQueried(id.ToString(), obj);
			}
			return obj;
		}

		private static XProperty _XColor;
		/// <summary>
		/// get the WidgetColor XProperty
		/// </summary>
		public static XProperty XColor {
			get
			{
				if (_XColor == null) {
					_XColor = XProperty.GetXProperty(EXTCFG_COLOR);
				}
				return _XColor;
			}
		}
		/// <summary>
		/// Get a random color for the widget
		/// </summary>
		/// <returns></returns>
		public static string GetRandomColor() {
			var retVal = "null";
			if (XColor != null && XColor.XPropertySettings.Count > 0) {
				var colorIndex = new Random().Next(XColor.XPropertySettings.Count);
				retVal = XColor.XPropertySettings[colorIndex].SettingValue;
			}
			return retVal;
		}
		/// <summary>
		/// Loading a editing view
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static string LoadEditView(Guid id, out BOMessager msg) {
			var obj = Find(id, out msg);
			var retVal = default(string);
			if (obj != null) {//dispatching the showing logic to the CoyoEden.UI component.
				onShowing(obj, out msg);
				if (!msg.IsError) {
					retVal = msg.Body;
				}
			}
			return retVal;
		}
		#endregion


		#region events
		public static event EventHandler<ActionEventArgs> Querying;
		public static event EventHandler<ActionEventArgs> Queried;
		public static event EventHandler<ActionEventArgs<Widget>> Showing;
		static void onQuerying<Tt>(string queryingKey,ref Tt retVal, out BOMessager msg) {
			msg=new BOMessager();
			if (Querying != null) { 
				var args=new ActionEventArgs(LogicAction.Query,retVal,ref msg);
				Querying(null, args);
			}
		}
		static void onQueried<Tt>(string queryingKey, Tt result) {
			if (null != Queried) {
				var args = new ActionEventArgs(LogicAction.Query, result);
				Queried(null, args);
			}
		}
		static void onShowing(Widget obj,out BOMessager msg) {
			msg = new BOMessager();
			var ucPath = string.Format(WidgetPathFormatStr_EDIT, Utils.RelativeWebRoot, obj.Name);
			if (File.Exists(HttpContext.Current.Server.MapPath(ucPath)))
			{
				if (Showing != null)
				{
					var args = new ActionEventArgs<Widget>(LogicAction.EditingView, obj, ref msg);
					Showing(ucPath, args);
				}
			}
			else { 
				//not editable
				msg.Error<Widget>("Not Editable!");
			}
		}
		#endregion
	}
}
