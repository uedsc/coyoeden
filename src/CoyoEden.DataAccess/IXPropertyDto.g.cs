using System;
namespace CoyoEden.DataAccess
{
	public partial interface IXPropertyDto
	{
		Guid Id{get;set;}
		string Name{get;set;}
		string Description{get;set;}
		string Icon{get;set;}
		DateTime CreatedOn{get;set;}
		string CreatedBy{get;set;}
		DateTime ModifiedOn{get;set;}
		string ModifiedBy{get;set;}
		bool IsDeleted{get;set;}
	}
}
