/// Author:					Joe Audette
/// Created:				2008-07-28
/// Last Modified:		    2008-07-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using Resources;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.UI
{
    
    public partial class SignInOrRegisterPrompt : UserControl
    {
        private string instructions = string.Empty;

        public string Instructions
        {
            get { return instructions; }
            set { instructions = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                this.Visible = false;
                return;
            }

            PopulateControls();
        }

        private void PopulateControls()
        {
            if (instructions.Length > 0)
            {
                litLoginInstructions.Text = instructions;
            }
            else
            {
                litLoginInstructions.Text = Resource.DefaultSignInPrompt;
            }

            litLoginPrompt.Text = Resource.AlreadyRegistered;
            litRegisterPrompt.Text = Resource.NotYetRegistered;
            lnkLogin.Text = Resource.LoginLink;
            lnkRegister.Text = Resource.RegisterLink;

            string siteRoot = SiteUtils.GetNavigationSiteRoot();

            lnkLogin.NavigateUrl = siteRoot + "/Secure/Login.aspx";
            lnkRegister.NavigateUrl = siteRoot + "/Secure/Register.aspx?returnurl=" + Server.UrlEncode(Request.RawUrl);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            lnkRegisterWithOpenId.Text = Resource.RegisterWithOpenIDLink;
            lnkRegisterWithOpenId.NavigateUrl = siteRoot + "?Secure/RegisterWithOpenID.aspx";

            lnkRegisterWithWindowsLive.Text = Resource.RegisterWithWindowsLiveLink;
            lnkRegisterWithWindowsLive.NavigateUrl = siteRoot + "/Secure/RegisterWithWindowsLiveID.aspx";

            if((siteSettings.AllowOpenIdAuth)&&(siteSettings.AllowWindowsLiveAuth)&&(siteSettings.WindowsLiveAppId.Length > 0))
            {
                litAdditionalRegisterOptions.Text = Resource.RegisterWithOpenIdOrWindowsLiveInstructions;
                divAdditionalRegisterOptions.Visible = true;
                
            }
            else if(siteSettings.AllowOpenIdAuth)
            {
                litAdditionalRegisterOptions.Text = Resource.RegisterWithOpenIDInstructions;
                divAdditionalRegisterOptions.Visible = true;
                divWindowsLive.Visible = false;
            }
            else if((siteSettings.AllowWindowsLiveAuth)&&(siteSettings.WindowsLiveAppId.Length > 0))
            {
                litAdditionalRegisterOptions.Text = Resource.RegisterWithWindowsLiveInstructions;
                divAdditionalRegisterOptions.Visible = true;
                divOpenId.Visible = false;
            }


        }



    }
}