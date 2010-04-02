/// Author:					Joe Audette
/// Created:				2008-08-26
/// Last Modified:			2008-08-26
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Business.WebHelpers;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{
    public partial class AdminSalesReportPage : CBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        protected Store store;
        protected Double timeOffset = 0;
        protected string currencyFormat = "{0:C}";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;

            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            if (CurrentPage.ContainsModule(moduleId))
                store = StoreHelper.GetStore();


        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);


        }

        #endregion
    }
}
