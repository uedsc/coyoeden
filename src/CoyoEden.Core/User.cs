
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using SystemX.LunaAtom;
using System.Collections.Generic;
using System.Linq;
using CoyoEden.Core.DataContracts;    
namespace CoyoEden.Core
{

    
    public partial class User
    {
		private static readonly object _synHelper = new object();
		private static List<User> _Users;
		public static List<User> Users {
			get
			{
				if (_Users == null) {
					lock (_synHelper) {
						if (_Users == null) {
							_Users = Broker.GetBusinessObjectCollection<User>("Id is not null").ToList();
						}
					}
				}
				return _Users;
			}
		}

		public static User GetUser(string username){
			return Users.SingleOrDefault(x=>x.UserName.Equals(username,StringComparison.OrdinalIgnoreCase));
		}
		public static User GetUserByEmail(string email) {
			return Users.SingleOrDefault(x => x.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
		}

		public static UserInfo GetUserInfo(string userName) { 
			//TODO:
			return new UserInfo { 
				UserName=userName,
				Points=2000,
				MessageCount=12
			};
		}
    }
}
