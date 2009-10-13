using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Core.Infrastructure
{
	public interface IComment:IPublishable,IComparable<IComment>
	{
		
		Guid? ParentID { get; set; }

		String Email { get; set; }

		Uri Website { get; set; }

		String Country { get; set; }

		String Ip { get; set; }

		bool? IsApproved { get; set; }

		IPublishable Parent { get; set; }

		string ModeratedBy { get; set; }

		string Teaser { get;}

		List<IComment> Comments { get; set; }
	}
}
