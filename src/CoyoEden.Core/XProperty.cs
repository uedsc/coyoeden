
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using System.Collections.Generic;
using System.Linq;
using Vivasky.Core;

namespace CoyoEden.Core
{
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
							_XProperties = Broker.GetBusinessObjectCollection<XProperty>("Id is not null");
						}
					}
				}
				return _XProperties;
			}
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
    }
}
