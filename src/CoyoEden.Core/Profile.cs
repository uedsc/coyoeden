
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using System.Linq;
using SystemX.LunaAtom;
using System.Collections.Generic;
namespace CoyoEden.Core
{
    
    public partial class Profile
    {
		public static List<Profile> LoadAll() {
			return Broker.GetBusinessObjectCollection<Profile>("Id is not null").ToList();
		}
		public static BusinessObjectCollection<Profile> LoadAll(string userName) {
			return Broker.GetBusinessObjectCollection<Profile>(string.Format("UserName='{0}'", userName));
		}
    }
}
