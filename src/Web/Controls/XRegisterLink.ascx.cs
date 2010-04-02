using System;
using Cynthia.Web.Framework;

namespace Cynthia.Web.Controls
{
	public partial class XRegisterLink :ViewBase
	{
		protected string Href { get; set; }
		protected void Page_Load(object sender, EventArgs e)
		{
			setVisibility();
			setHref();
		}

		private void setVisibility()
		{
			Visible = !((CurSettings == null) || (!CurSettings.AllowNewRegistration) || (CurSettings.UseLdapAuth)||(IsAuthenticated));
			if (CurSettings.DisableDbAuth)
			{
				if ((!CurSettings.AllowOpenIdAuth) && (!CurSettings.AllowWindowsLiveAuth)) {
					Visible = false;
				}
			}
		}

		private void setHref()
		{
			if (!Visible) return;
			if (CurSettings.DisableDbAuth)
			{
				if (!CurSettings.AllowOpenIdAuth)
				{
					Href = "/Secure/RegisterWithWindowsLiveID.aspx";
				}

				if ((!CurSettings.AllowWindowsLiveAuth) && (CurSettings.RpxNowApiKey.Length == 0))
				{
					Href = "/Secure/RegisterWithOpenID.aspx";
				}
			}
			else {
				Href = "/Secure/Register.aspx";
			}
			Href = string.Format("{0}{1}", SiteUtils.GetNavigationSiteRoot(),Href);
			if (SiteUtils.SslIsAvailable())
			{
				Href = Href.Replace("http:", "https:");
			}

			string returnUrlParam = Page.Request.Params.Get("returnurl");
			if (!String.IsNullOrEmpty(returnUrlParam))
			{
				Href += "?returnurl=" + SecurityHelper.RemoveAngleBrackets(returnUrlParam);
			}
		}
	}
}