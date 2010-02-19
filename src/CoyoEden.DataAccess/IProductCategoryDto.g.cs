using System;
namespace CoyoEden.DataAccess
{
	public partial interface IProductCategoryDto
	{
		Guid Id{get;set;}
		Guid FKProduct{get;set;}
		Guid FKCategory{get;set;}
	}
}
