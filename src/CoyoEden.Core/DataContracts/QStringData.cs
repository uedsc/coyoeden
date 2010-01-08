using System;
using System.Linq;
using Vivasky.Core;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CoyoEden.Core.DataContracts
{
	/// <summary>
	/// query string data
	/// </summary>
	public class QStringData
	{
		public static string STR_FIELDSPLIT = "$";
		public static string STR_KEYVALUESPLIT = "^";
		/// <summary>
		/// action
		/// </summary>
		public ActionTypes a { get; set; }

		#region common query string keys
		/// <summary>
		/// isvalid
		/// </summary>
		public YesNoOptions v { get; set; }
		/// <summary>
		/// id
		/// </summary>
		public string i { get; set; }
		/// <summary>
		/// up-bound
		/// </summary>
		public string ub { get; set; }
		/// <summary>
		/// lower-bound
		/// </summary>
		public string lb { get; set; }
		/// <summary>
		/// tag 0
		/// </summary>
		public string t0 { get; set; }
		/// <summary>
		/// tag 1
		/// </summary>
		public string t1 { get; set; }
		/// <summary>
		/// tag 2
		/// </summary>
		public string t2 { get; set; }
		#endregion

		#region .ctor
		public QStringData() { }
		#endregion

		#region factory methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="queryString">accept like:a^1$v^0$lb^2008$ub^2009</param>
		/// <returns></returns>
		public static QStringData New(string queryString) {
			return new QStringData().ParseData(queryString);
		}
		#endregion

		#region methods
		/// <summary>
		/// Parse request string
		/// </summary>
		/// <param name="rawRequestString"></param>
		/// <returns></returns>
		public QStringData ParseData(string rawRequestString)
		{
			a = ActionTypes.Unkown;
			if (string.IsNullOrEmpty(rawRequestString)) return this;
			var parts = rawRequestString.Split(STR_FIELDSPLIT.ToCharArray());
			if (parts[0].Length != 3)//'a^1'.length==3
			{
				return this;
			};
			var parts0=default(string[]);
			parts.ToList().ForEach(x => {
				parts0 = x.Split(STR_KEYVALUESPLIT.ToCharArray());
				if (parts0[0] == "a" || parts0[0] == "v")
				{
					var i=0;
					int.TryParse(parts0[1],out i);
					Set(parts0[0], i);
				}
				else
				{
					Set(parts0[0], parts0[1]);
				}
			});
			return this;
		}
		/// <summary>
		/// serialize to query string data
		/// </summary>
		/// <returns></returns>
		private string ToQString() {
			var data = this.ToJSONStr();
			//remove null values
			data = Regex.Replace(data, ",\"[^\"]+?\":null", "", RegexOptions.Compiled);
			//encode process
			data=data
				.Replace("{", "")
				.Replace("}", "")
				.Replace("\"", "")
				.Replace(":", STR_KEYVALUESPLIT)
				.Replace(",", STR_FIELDSPLIT);
			return string.Format("d={0}",data);
		}
		/// <summary>
		/// update a property,and serialize to query string data
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public QStringData Set(string key, object val) {
			this.SetPropertyValue(key, val, true);
			return this;
		}
		#endregion

		#region base overrides
		public override string ToString()
		{
			return ToQString();
		}
		#endregion
	}
}
