using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Core.DataContracts
{
	public class UserInfo
	{
		public string UserName { get; set; }
		public string Avatar { get; set; }
		public int MessageCount { get; set; }
		public int Points { get; set; } 
	}
}
