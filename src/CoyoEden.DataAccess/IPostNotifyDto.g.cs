using System;
namespace CoyoEden.DataAccess
{
	public partial interface IPostNotifyDto
	{
		Guid Id{get;set;}
		Guid PostID{get;set;}
		string NotifyAddress{get;set;}
	}
}
