using System;
namespace CoyoEden.DataAccess
{
	public partial interface IUserTagDto
	{
		Guid Id{get;set;}
		Guid FKUser{get;set;}
		Guid FKTag{get;set;}
		DateTime CreatedOn{get;set;}
	}
}
