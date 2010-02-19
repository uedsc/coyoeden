using System;
namespace CoyoEden.DataAccess
{
	public partial interface IXFolderTagDto
	{
		Guid Id{get;set;}
		Guid FKXFolder{get;set;}
		Guid FKTag{get;set;}
		DateTime CreatedOn{get;set;}
	}
}
