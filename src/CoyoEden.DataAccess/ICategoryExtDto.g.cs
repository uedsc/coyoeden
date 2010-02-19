using System;
namespace CoyoEden.DataAccess
{
	public partial interface ICategoryExtDto
	{
		Guid Id{get;set;}
		Guid FKCategory{get;set;}
		Guid FKXProperty{get;set;}
	}
}
