

using System;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.GroupUI
{
    public partial class GroupSearchBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((WebConfigSettings.DisableSearchFeatureFilters)||(WebConfigSettings.DisableSearchIndex))
            {
                this.Visible = false;
                return;
            }

            btnSearch.Text = GroupResources.Search;
            reqSearchText.ErrorMessage = GroupResources.SearchTermRequiredWarning;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string redirectUrl = Request.RawUrl;
            if (txtSearch.Text.Length > 0)
            {
                redirectUrl = SiteUtils.GetNavigationSiteRoot()
                    + "/SearchResults.aspx?f=E75BAF8C-7079-4d10-A122-1AA3624E26F2&q=" + Server.UrlEncode(txtSearch.Text);
            }

            WebUtils.SetupRedirect(this, redirectUrl);
            
        }

        //SearchResults.aspx?q=foo&f=E75BAF8C-7079-4d10-A122-1AA3624E26F2
        //btnSearch

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        
    }
}