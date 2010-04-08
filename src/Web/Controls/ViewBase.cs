using System;
using System.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
namespace Cynthia.Web.Controls
{
	/// <summary>
	/// UserControl base class
	/// </summary>
	public partial class ViewBase:UserControl
	{
		#region member variables
		/// <summary>
		/// current site settings
		/// </summary>
		protected SiteSettings CurSettings { get; set; }
		/// <summary>
		/// current page settings
		/// </summary>
		protected PageSettings CurPageSettings { get; set; }
		/// <summary>
		/// short hand for Page.Request.IsAuthenticated
		/// </summary>
        protected bool IsAuthenticated
        {
            get
            {
                return Page.Request.IsAuthenticated;
            }
        }
		/// <summary>
		/// shorthand for Context.User.Identity.AuthenticationType
		/// </summary>
		protected string AuthType {
			get {
				return Context.User.Identity.AuthenticationType;
			}
		}
		#endregion
		protected override void OnInit(EventArgs e)
		{
			CurSettings = CacheHelper.GetCurrentSiteSettings();
			CurPageSettings = CacheHelper.GetCurrentPage();
			base.OnInit(e);
		}

	}
}
