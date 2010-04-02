using System;
using Cynthia.Business;

namespace Cynthia.Web.Controls
{
    public partial class XSiteTitle :ViewBase
    {
        public string Url { get; set; }
        public string Title{get;set;}
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Title ?? CurSettings.SiteName;
            Url = Url ?? "";
            if (Title.Length > 0 && Url.Length > 0)
            {
                Url = (Url.StartsWith("~/") && CurSettings.SiteFolderName.Length > 0)
                               ? SiteUtils.GetNavigationSiteRoot() + Url.Replace("~/", "/")
                               : Url;
            }
            else
            {
                if (CurSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX)
                {
                    Url = SiteUtils.GetNavigationSiteRoot() + "/Default.aspx";
                }
                else
                {
                    Url = SiteUtils.GetNavigationSiteRoot();
                }
            }
        }
    }
}