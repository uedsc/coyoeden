
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using System.Collections.Generic;
using CoyoEden.Core.Providers;
namespace CoyoEden.Core
{
    
    public partial class Profile
    {
		public static List<Profile> LoadAll() {
			return Broker.GetBusinessObjectCollection<Profile>("Id is not null");
		}
		public static BusinessObjectCollection<Profile> LoadAll(string userName) {
			return Broker.GetBusinessObjectCollection<Profile>(string.Format("UserName='{0}'", userName));
		}
    }
}
