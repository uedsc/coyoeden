using System;
namespace CoyoEden.DataAccess
{
	public partial interface IProductExtDto
	{
		Guid Id{get;set;}
		Guid FKProduct{get;set;}
		Guid FKXProperty{get;set;}
		Guid FKXPropertySetting{get;set;}
	}
}
