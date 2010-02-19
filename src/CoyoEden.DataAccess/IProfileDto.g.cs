using System;
namespace CoyoEden.DataAccess
{
	public partial interface IProfileDto
	{
		Guid Id{get;set;}
		string UserName{get;set;}
		string SettingName{get;set;}
		string SettingValue{get;set;}
	}
}
