using System;
namespace CoyoEden.DataAccess
{
	public partial interface IWidgetDto
	{
		Guid Id{get;set;}
		string Title{get;set;}
		string Name{get;set;}
		bool ShowTitle{get;set;}
		string Template{get;set;}
		string Tag{get;set;}
		string ExtConfig{get;set;}
		int DisplayIndex{get;set;}
		DateTime ModifiedOn{get;set;}
		Guid FKZone{get;set;}
	}
}
