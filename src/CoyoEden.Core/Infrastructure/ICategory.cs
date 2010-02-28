using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Core.Infrastructure
{
	public interface ICategory:IComparable<ICategory>
	{
		Guid? Id { get; set; }
		string Name { get; set; }
		string FullName { get;}
		string Description { get; set; }
		Guid? ParentID { get; set; }
        string RelativeLink { get; }
	}
}
