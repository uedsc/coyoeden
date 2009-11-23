using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivasky.Core;
using System.Text.RegularExpressions;

namespace CoyoEden.Core.DataContracts
{
	/// <summary>
	/// query string data
	/// </summary>
	public class QStringData
	{
		const string STR_FIELDSPLIT = "$";
		const string STR_KEYVALUESPLIT = "-";
		/// <summary>
		/// action
		/// </summary>
		public QueryTypes a { get; set; }

		#region common query string keys
		/// <summary>
		/// isvalid
		/// </summary>
		public bool? v { get; set; }
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
		/// <param name="queryString">accept like:a-1$v-true$lb-2008$ub-2009</param>
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
			a = QueryTypes.Unkown;
			if (string.IsNullOrEmpty(rawRequestString)) return this;
			var parts = rawRequestString.Split(STR_FIELDSPLIT.ToCharArray());
			if (parts[0].Length != 3)//'a-1'.length==3
			{
				return this;
			};

			var jsonData = rawRequestString.Replace(STR_FIELDSPLIT, ",").Replace(STR_KEYVALUESPLIT, ":");
			jsonData = String.Format("{{{0}}}", jsonData);

			var tempObj = jsonData.DeserializeJSONStr<QStringData>();
			a = tempObj.a;
			v = tempObj.v;
			i = tempObj.i;
			ub = tempObj.ub;
			lb = tempObj.lb;
			t0 = tempObj.t0;
			t1 = tempObj.t1;
			t2 = tempObj.t2;
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
