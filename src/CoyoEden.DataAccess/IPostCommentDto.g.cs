using System;
namespace CoyoEden.DataAccess
{
	public partial interface IPostCommentDto
	{
		Guid Id{get;set;}
		Guid PostID{get;set;}
		Guid ParentID{get;set;}
		DateTime DateCreated{get;set;}
		string Author{get;set;}
		string Email{get;set;}
		string Website{get;set;}
		string Content{get;set;}
		string Country{get;set;}
		string Ip{get;set;}
		bool IsApproved{get;set;}
	}
}
