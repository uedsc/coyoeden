
using System;
using Cynthia.Web.Controls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// automatically sets the addthis accountid from site settings
    /// </summary>
    public class CAddThisButton : AddThisButton
    {
        private SiteSettings siteSettings = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            AccountId = siteSettings.AddThisDotComUsername;

        }
    }
}
