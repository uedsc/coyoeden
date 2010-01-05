
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using System.Collections.Generic;
using System.Linq;
using Vivasky.Core;
using CoyoEden.Core.Infrastructure;
using Vivasky.Core.Infrastructure;

namespace CoyoEden.Core
{
	/// <summary>
	/// extensive property which has multiple value options.
	/// </summary>
    public partial class XProperty
    {
		private static readonly object _synHelper = new object();
		private static List<XProperty> _XProperties;
		public static List<XProperty> XProperties
		{
			get
			{
				if (_XProperties == null)
				{
					lock (_synHelper)
					{
						if (_XProperties == null)
						{
							_XProperties = LoadAll();
						}
					}
				}
				return _XProperties;
			}
		}
		public static List<XProperty> LoadAll() { 
			var retVal=default(List<XProperty>);
			if (null != Querying) { 
				var args=new ActionEventArgs(LogicAction.Query,retVal);
				Querying(null, args);
				retVal = args.Target as List<XProperty>;
			}
			if (null != retVal) return retVal;//from cache.
			retVal = Broker.GetBusinessObjectCollection<XProperty>("Id is not null");
			if (null != Queried) {
				Queried(null, new ActionEventArgs(LogicAction.Query, retVal));
			}
			return retVal;
		}
		public static XProperty GetXProperty(string name)
		{
			return XProperties.SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		}
		/// <summary>
		/// TODO:Add 'Icon' as a column
		/// </summary>
		public string Icon {
			get
			{
				return String.Format("{0}themes/admin/img/icon_c100.jpg", Utils.AbsoluteWebRoot);
			}
		}
		public DateTime CreatedOn {
			get
			{
				return DateTime.Now;
			}
		}
		public DateTime ModifiedOn {
			get
			{
				return DateTime.Now;
			}
		}
		public string CreatedBy {
			get
			{
				return "CoyoEden";
			}
		}

		#region events
		public static event EventHandler<ActionEventArgs> Querying;
		public static event EventHandler<ActionEventArgs> Queried;
		public static event EventHandler<ActionEventArgs<Widget>> Showing;
		static void onQuerying<Tt>(string queryingKey, ref Tt retVal, out BOMessager msg)
		{
			msg = new BOMessager();
			if (Querying != null)
			{
				var args = new ActionEventArgs(LogicAction.Query, retVal, ref msg);
				Querying(null, args);
			}
		}
		static void onQueried<Tt>(string queryingKey, Tt result)
		{
			if (null != Queried)
			{
				var args = new ActionEventArgs(LogicAction.Query, result);
				Queried(null, args);
			}
		}
		static void onShowing(Widget obj, out BOMessager msg)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
