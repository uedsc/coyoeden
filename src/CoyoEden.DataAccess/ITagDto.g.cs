using System;
namespace CoyoEden.DataAccess
{
	public partial interface ITagDto
	{
		Guid Id{get;set;}
		string Name{get;set;}
		int Hits{get;set;}
	}
}
