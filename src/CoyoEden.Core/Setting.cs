
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using System.Collections.Generic;
using System.Linq;
using SystemX.Infrastructure;
using CoyoEden.Core.DataContracts;
using CoyoEden.Core.Infrastructure;
using SystemX.Utility;
using System.Xml;
using SystemX.Services;

namespace CoyoEden.Core
{

    
    
    public partial class Setting:ISettingsBehavior,ICacheable
	{
		#region properties

		#endregion

		#region biz methods
		public SettingTypes GetSelfType() {
			return ParseSettingType(SettingType);
		}
		#endregion

		#region factory methods
		public Setting() {
			Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
			this.Saved += (s, e) => { ClearCache(); };
		}
		public Setting(string type,string name,string value):this() {
			SettingType = type;
			SettingName = name;
			SettingValue = value;
		}

		private static readonly object _SynHelper=new object();
		private static List<Setting> _allSettings;
		public static List<Setting> AllSettings{
			get
            {
            	if (_allSettings==null){
					lock(_SynHelper){
						if (_allSettings==null){
							_allSettings=LoadAll();
						}
					}
				}
				return _allSettings;
            }
		}


		public static SettingTypes ParseSettingType(string settingType)
		{
			var obj = Enum.Parse(typeof(SettingTypes), settingType);
			return (SettingTypes)obj;
		}

		public static List<Setting> LoadAll(){
			return Broker.GetBusinessObjectCollection<Setting>("Id is not null");
		}
		public static List<Setting> LoadAll(SettingTypes type) {
			var items = Broker.GetBusinessObjectCollection<Setting>(string.Format("SettingType='{0}'",type));
			return items.ToList();
		}
		public static void SaveAll(List<Setting> items) {
			var tempItems = new BusinessObjectCollection<Setting>();
			tempItems.AddRange(items);
			tempItems.SaveAll();
		}
		public static void Save(SettingTypes settingType, string settingName, string settingValue)
		{
			var obj = Find(settingType, settingName);
			obj = obj ?? new Setting();
			obj.SettingValue = settingValue;
			obj.SettingType = settingType.ToString();
			obj.SettingName = settingName;
			obj.Save();
		}
		public static Setting Find(SettingTypes settingType, string settingName)
		{
			return AllSettings.SingleOrDefault(x => x.SettingType.Equals(settingType.ToString()) && x.SettingName.Equals(settingName));
		}
		public static void Delete(SettingTypes settingType,string settingName) {
			var obj = Find(settingType, settingName);
			if (obj != null) {
				obj.MarkForDelete();
				obj.Save();
			}
		}
		#endregion

		#region ISettingsBehavior Members
		/// <summary>
		/// Save the Settings object instance.This will serialize the instance using XmlSerializer before saving. 
		/// </summary>
		/// <typeparam name="T">Type of settings object</typeparam>
		/// <param name="exType"></param>
		/// <param name="name"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		public bool SaveSettings<T>(SettingTypes exType, string name, T settings)
		{
			Save(exType, name, settings.ToXml());
			return true;
		}
		/// <summary>
		/// get the settings from database and convert it a stronge type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="exType"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public T GetSettings<T>(SettingTypes exType, string name) where T:class
		{
			var obj= Find(exType, name);
			if (obj == null) {
				return default(T);
			}
			//T is string,return the SettingValue
			if (typeof(T)==typeof(string)) {
				return obj.SettingValue as T;
			};
			//T is XmlDocument
			if (typeof(T)==typeof(XmlDocument)){
				return obj.SettingValue.ToXMLDoc() as T;
			};
			return XmlUtil.Deserialize<T>(obj.SettingValue);
		}

		#endregion

		#region ICacheable Members

		public void ClearCache()
		{
			_allSettings = null;
		}

		#endregion
	}
}
