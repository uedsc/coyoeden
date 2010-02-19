using System;
namespace CoyoEden.DataAccess
{
	public partial interface IUserDto
	{
		Guid Id{get;set;}
		string UserName{get;set;}
		string Password{get;set;}
		DateTime? LastLoginTime{get;set;}
		string EmailAddress{get;set;}
	}
}
