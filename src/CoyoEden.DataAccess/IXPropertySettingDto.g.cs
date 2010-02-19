using System;
namespace CoyoEden.DataAccess
{
	public partial interface IXPropertySettingDto
	{
		Guid Id{get;set;}
		string SettingName{get;set;}
		string SettingValue{get;set;}
		bool IsDeleted{get;set;}
		Guid FKXProperty{get;set;}
	}
}
