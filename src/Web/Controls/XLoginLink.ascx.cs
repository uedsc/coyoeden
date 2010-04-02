using System;

namespace Cynthia.Web.Controls
{
	public partial class XLoginLink :ViewBase
	{
		protected string Href { get; set; }
		protected void Page_Load(object sender, EventArgs e)
		{
			setHref();
		}

		private void setHref()
		{
			if (IsAuthenticated)
			{
				if (AuthType != "Forms") return;
                Href = String.Format("{0}/Logoff.aspx", SiteUtils.GetNavigationSiteRoot());
				if ((!WebConfigSettings.SslIsRequiredByWebServer) && (Page.Request.IsSecureConnection)) { Href = Href.Replace("https", "http"); }
			}
			else {
                Href = String.Format("{0}{1}", SiteUtils.GetNavigationSiteRoot(), SiteUtils.GetLoginRelativeUrl());
				if (SiteUtils.SslIsAvailable())
				{
					Href = Href.Replace("http:", "https:");
				}

				if (CurPageSettings.HideAfterLogin)
				{
					Href += "?r=h";
				}
			}
		}
	}
}