using System;
namespace CoyoEden.DataAccess
{
	public partial interface ICategoryDto
	{
		Guid Id{get;set;}
		string Name{get;set;}
		string Description{get;set;}
		string Tag{get;set;}
		Guid? ParentID{get;set;}
	}
}
