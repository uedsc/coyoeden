/// Author:					Joe Audette
/// Created:				2007-09-23
/// Last Modified:			2009-10-27
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.ELetterUI
{
    public partial class DefaultPage : CBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();
            PopulateLabels();
            AddConnoicalUrl();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.NewslettersLink);
            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;

            litAnonymousHeader.Text = Resource.NewsletterPreferencesHeader;

            lnkThisPage.Text = Resource.NewslettersLink;

            MetaDescription = Resource.NewsletterSignUpPageMetaDescription;

        }

        private void AddConnoicalUrl()
        {
            if (Page.Header == null) { return; }

            Literal link = new Literal();
            link.ID = "threadurl";
            link.Text = "\n<link rel='canonical' href='"
                + SiteRoot + "/eletter/' />";

            Page.Header.Controls.Add(link);

        }

        private void LoadSettings()
        {
            newsLetterPrefs.Visible = Request.IsAuthenticated;
            pnlAnonymousSubscriber.Visible = !newsLetterPrefs.Visible;
            anonymousSubscribe.Visible = pnlAnonymousSubscriber.Visible;
            spnAdmin.Visible = WebUser.IsNewsletterAdmin;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/";
            
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();

            
        }

        #endregion
    }
}

