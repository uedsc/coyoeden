using System;
using CoyoEden.Core;
using SystemX.Infrastructure;
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

        private BOMessager _appMsg=new BOMessager();
        /// <summary>
        /// messager
        /// </summary>
        public virtual BOMessager AppMsg{
            get
            {
                return _appMsg;
            }
            set
            {
            	_appMsg = value;
            }
        }
	}
}
