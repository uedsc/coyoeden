using System;
using CoyoEden.Core;
namespace CoyoEden.UI
{
	public class SiteMaster:SystemX.Web.SiteMaster
	{
		public string CssClass { get; set; }
		protected virtual bool UserIsAdmin
		{
			get
			{
				return Page.User.IsInRole(BlogSettings.Instance.AdministratorRole);
			}
		}
	}
}
