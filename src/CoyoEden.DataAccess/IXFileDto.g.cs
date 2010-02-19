using System;
namespace CoyoEden.DataAccess
{
	public partial interface IXFileDto
	{
		Guid Id{get;set;}
		string Name{get;set;}
		string Description{get;set;}
		string Url{get;set;}
		DateTime CreatedOn{get;set;}
		string CreatedBy{get;set;}
		string ExtName{get;set;}
		int Hits{get;set;}
		Guid FKFolder{get;set;}
	}
}
