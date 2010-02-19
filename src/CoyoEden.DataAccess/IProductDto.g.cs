using System;
namespace CoyoEden.DataAccess
{
	public partial interface IProductDto
	{
		Guid Id{get;set;}
		string Name{get;set;}
		string Description{get;set;}
		decimal Price{get;set;}
		decimal CostPrice{get;set;}
		int Stock{get;set;}
		string ExternalLink{get;set;}
		DateTime CreatedOn{get;set;}
		string CreatedBy{get;set;}
		DateTime ModifiedOn{get;set;}
		string ModifiedBy{get;set;}
		bool IsDeleted{get;set;}
		Guid FKFolder{get;set;}
	}
}
