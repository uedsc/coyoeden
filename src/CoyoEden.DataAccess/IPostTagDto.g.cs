using System;
namespace CoyoEden.DataAccess
{
	public partial interface IPostTagDto
	{
		Guid Id{get;set;}
		Guid PostID{get;set;}
		string Tag{get;set;}
	}
}
