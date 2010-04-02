using System;
using System.Web.UI;
using Cynthia.Business;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI.Pages
{
    public partial class ConfirmRegistration : CBasePage
    {
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
            
        }


        private Guid registrationConfirmationGuid = Guid.Empty;

        private void Page_Load(object sender, System.EventArgs e)
        {
            GetGuidFromQueryString();
            if (this.registrationConfirmationGuid == Guid.Empty)
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            SiteUser siteUser = SiteUser.GetByConfirmationGuid(siteSettings, registrationConfirmationGuid);

            if (SiteUser.ConfirmRegistration(this.registrationConfirmationGuid))
            {
                this.lblMessage.Text = Resources.Resource.RegisterConfirmMessage;
                if (siteUser != null) { NewsletterHelper.ClaimExistingSubscriptions(siteUser); }

                CGoogleAnalyticsScript analytics = Page.Master.FindControl("CGoogleAnalyticsScript1") as CGoogleAnalyticsScript;
                if (analytics == null) { return; }
                
                analytics.PageToTrack = "/RegistrationConfirmed.aspx";


            }
            else
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

        }

        private void GetGuidFromQueryString()
        {
            this.registrationConfirmationGuid = WebUtils.ParseGuidFromQueryString("ticket", Guid.Empty);

            

        }


    }
}
