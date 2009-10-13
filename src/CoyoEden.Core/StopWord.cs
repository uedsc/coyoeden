
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
	using System.Collections.Generic;
using System.Collections.Specialized;
    
    
    public partial class StopWord
    {
		private static readonly object _SynHelper = new object();
		private static List<StopWord> _StopWords;
		public static List<StopWord> StopWords
		{
			get
			{
				if (_StopWords == null)
				{
					lock (_SynHelper)
					{
						if (_StopWords == null)
						{
							_StopWords = LoadAll();
						}
					}
				}
				return _StopWords;
			}
		}

		public static StringCollection Words {
			get
			{
				var retVal = new StringCollection();
				StopWords.ForEach(x => {
					retVal.Add(x.Word);
				});
				return retVal;
			}
		}

		public static List<StopWord> LoadAll()
		{
			return Broker.GetBusinessObjectCollection<StopWord>("Id is not null");
		} 
    }
}
