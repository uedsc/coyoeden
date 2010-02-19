using System;
namespace CoyoEden.DataAccess
{
	public partial interface IPostDto
	{
		Guid Id{get;set;}
		string Title{get;set;}
		string Description{get;set;}
		string Content{get;set;}
		DateTime DateCreated{get;set;}
		DateTime DateModified{get;set;}
		string Author{get;set;}
		bool IsPublished{get;set;}
		bool IsCommentEnabled{get;set;}
		bool IsHeadline{get;set;}
		int Raters{get;set;}
		decimal Rating{get;set;}
		string Slug{get;set;}
	}
}
