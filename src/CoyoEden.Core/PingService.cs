
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------
using System;
using Habanero.BO;
using System.Collections.Generic;
using System.Collections.Specialized;
using Vivasky.Core.Infrastructure;
using System.Linq;

namespace CoyoEden.Core
{
    public partial class PingService
    {

		public PingService() {
			Id = GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential);
			this.Saved += (s, e) => { ClearCache(); };
		}

		public PingService(string link):this() {
			Link = link;
		}

		private static readonly object _SynHelper = new object();
		private static List<PingService> _PingServices ;
		public static List<PingService> PingServices {
			get
			{
				if (_PingServices == null) {
					lock (_SynHelper) {
						if (_PingServices == null) {
							_PingServices = LoadAll();
						}
					}
				}
				return _PingServices;
			}
		}
		public static StringCollection PingServiceLinks {
			get
			{
				var retVal = new StringCollection();
				PingServices.ForEach(x => {
					retVal.Add(x.Link);
				});
				return retVal;
			}
		} 

		public static List<PingService> LoadAll()
		{
			return Broker.GetBusinessObjectCollection<PingService>("Id is not null");
		}

		public static void DeleteByLink(string link) {
			var item = Find(link);
			if (item != null) {
				item.MarkForDelete();
				item.Save();
			}
		}
		public static PingService Find(string link) {
			return PingServices.SingleOrDefault(x => x.Link == link);
		}
		public void ClearCache() {
			_PingServices = null;
		}
    }
}
