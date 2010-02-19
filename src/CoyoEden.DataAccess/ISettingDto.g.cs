using System;
namespace CoyoEden.DataAccess
{
	public partial interface ISettingDto
	{
		Guid Id{get;set;}
		string SettingType{get;set;}
		string SettingName{get;set;}
		string SettingValue{get;set;}
	}
}
