using System;
namespace CoyoEden.DataAccess
{
	public partial interface IUserRoleDto
	{
		Guid Id{get;set;}
		Guid UserID{get;set;}
		Guid RoleID{get;set;}
	}
}
