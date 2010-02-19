using System;
namespace CoyoEden.DataAccess
{
	public partial interface IPageDto
	{
		Guid Id{get;set;}
		string Title{get;set;}
		string Description{get;set;}
		string Content{get;set;}
		string Keywords{get;set;}
		DateTime DateCreated{get;set;}
		DateTime DateModified{get;set;}
		bool IsPublished{get;set;}
		bool IsFrontPage{get;set;}
		Guid? ParentID{get;set;}
		bool ShowInList{get;set;}
		string Slug{get;set;}
	}
}
