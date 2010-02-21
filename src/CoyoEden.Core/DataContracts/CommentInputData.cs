using System;
using SystemX.Web;

namespace CoyoEden.Core.DataContracts
{
	public class CommentInputData
	{
		public CommentInputData() {
			UserName = string.Empty;
			Email = string.Empty;
			Website = string.Empty;
			Country = string.Empty;
			Content = string.Empty;
		}
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Website { get; set; }
		public string Country { get; set; }
		public string Content { get; set; }
		public string FlagImageUrl {
			get
			{
				var retVal=String.Format("{0}pics/pixel.png", Utils.RelativeWebRoot);
				if (!string.IsNullOrEmpty(Country)) {
					retVal = String.Format("{0}pics/flags/{1}.png", Utils.RelativeWebRoot, Country);
				}
				return retVal;
			}
		}
	}
}
