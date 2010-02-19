using System;
namespace CoyoEden.DataAccess
{
	public partial interface IPostCategoryDto
	{
		Guid Id{get;set;}
		Guid PostID{get;set;}
		Guid CategoryID{get;set;}
	}
}
