using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using CoyoEden.Core;
namespace CoyoEden.UI
{
	public class SiteMaster:Vivasky.Core.Web.SiteMaster
	{
		public string CssClass { get; set; }
		protected bool UserIsAdmin
		{
			get
			{
				return Page.User.IsInRole(BlogSettings.Instance.AdministratorRole);
			}
		}
	}
}
