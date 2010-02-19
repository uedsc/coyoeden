using System;
namespace CoyoEden.DataAccess
{
	public partial interface IXFolderDto
	{
		Guid Id{get;set;}
		string Name{get;set;}
		string Description{get;set;}
		int DisplayOrder{get;set;}
		DateTime CreatedOn{get;set;}
		string CreatedBy{get;set;}
		DateTime ModifiedOn{get;set;}
		string ModifiedBy{get;set;}
		bool IsDeleted{get;set;}
		int Hits{get;set;}
		int XType{get;set;}
	}
}
