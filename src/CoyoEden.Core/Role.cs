
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using Vivasky.Core.Infrastructure;
using System.Collections.Generic;
using System.Linq;
    
namespace CoyoEden.Core
{
    public partial class Role
	{
		#region biz methods
		#endregion

		#region factory methods
		public Role() {
			Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
		}
		public Role(string name):this() {
			Name = name;
		}
		private static readonly object _synHelper = new object();
		private static List<Role> _Roles;
		public static List<Role> Roles
		{
			get
			{
				if (_Roles == null)
				{
					lock (_synHelper)
					{
						if (_Roles == null)
						{
							_Roles = Broker.GetBusinessObjectCollection<Role>("Id is not null");
						}
					}
				}
				return _Roles;
			}
		}

		public static Role GetRole(string Rolename)
		{
			return Roles.SingleOrDefault(x => x.Name.Equals(Rolename, StringComparison.OrdinalIgnoreCase));
		}
		#endregion
	}
}
