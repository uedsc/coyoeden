/// Author:				        Joe Audette
/// Created:			        2004-10-03
/// Last Modified:		        2010-02-03
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Configuration;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI.Pages
{
	
    public partial class MemberList : CBasePage
	{
        private int totalPages = 1;
		private int pageNumber = 1;
        private int pageSize = 20;
        protected bool IsAdmin = false;
        protected bool canManageUsers = false;
        private string userNameBeginsWith = string.Empty;
        private string searchText = string.Empty;
        
        protected bool ShowWebSiteColumn = false;
        protected bool ShowGroupPostColumn = WebConfigSettings.ShowGroupPostsInMemberList;
        
        private bool allowView = false;
		
        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSearchUser.Click += new EventHandler(btnSearchUser_Click);
            btnIPLookup.Click += new EventHandler(btnIPLookup_Click);

            SuppressMenuSelection();
            if (WebConfigSettings.HidePageMenuOnMemberListPage) { SuppressPageMenu(); }
        }

        

        
        #endregion

		private void Page_Load(object sender, EventArgs e)
		{
            if ((SiteUtils.SslIsAvailable()) 
                && ((WebConfigSettings.UseSslForMemberList)||(siteSettings.UseSslOnAllPages))
                )
            {
                SiteUtils.ForceSsl();
            }

            LoadSettings();
            PopulateLabels();
            
            if (!allowView)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                PopulateControls();
            }
			
		}

        private void PopulateControls()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MemberListLink);

            if ((Page.Header != null) && (CurrentPage.Url.Length > 0))
            {
                Literal link = new Literal();
                link.ID = "pageurl";
                link.Text = "\n<link rel='canonical' href='"
                    + SiteRoot + "/MemberList.aspx' />";
                Page.Header.Controls.Add(link);
            }

            if (searchText.Length > 0)
            {
                BindForSearch();
               
            }
            else
            {
                BindAlphaList();
            }

            
            

           
        }

        private void BindAlphaList()
        {
            List<SiteUser> siteUserPage = SiteUser.GetPage(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                userNameBeginsWith,
                out totalPages);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;
                siteUserPage = SiteUser.GetPage(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                userNameBeginsWith,
                out totalPages);
            }

            if (userNameBeginsWith.Length > 1)
            {
                txtSearchUser.Text = Server.HtmlEncode(userNameBeginsWith);
            }

            Literal topPageLinks = new Literal();
            string pageUrl = SiteRoot
                + "/MemberList.aspx?"
                + "pagenumber=";

            string alphaChars;

            if (WebConfigSettings.GetAlphaPagerCharsFromResourceFile)
            {
                alphaChars = Resource.AlphaPagerChars;
            }
            else
            {
                alphaChars = WebConfigSettings.AlphaPagerChars;
            }

            topPageLinks.Text = UIHelper.GetAlphaPagerLinks(
                pageUrl,
                pageNumber,
                alphaChars,
                userNameBeginsWith);

            this.spnTopPager.Controls.Add(topPageLinks);

            pageUrl = SiteRoot
                + "/MemberList.aspx?"
                + "pagenumber={0}"
                + "&amp;letter=" + Server.UrlEncode(Server.HtmlEncode(userNameBeginsWith));

            pgrMembers.PageURLFormat = pageUrl;
            pgrMembers.ShowFirstLast = true;
            pgrMembers.CurrentIndex = pageNumber;
            pgrMembers.PageSize = pageSize;
            pgrMembers.PageCount = totalPages;
            pgrMembers.Visible = (totalPages > 1);


            rptUsers.DataSource = siteUserPage;

            rptUsers.DataBind();


        }

        private void BindForSearch()
        {
            List<SiteUser> siteUserPage;

            if (WebUser.IsAdmin)
            {
                // admins can also search against email address
                siteUserPage = SiteUser.GetUserAdminSearchPage(
                        siteSettings.SiteId,
                        pageNumber,
                        pageSize,
                        searchText,
                        out totalPages);
            }
            else
            {
                siteUserPage = SiteUser.GetUserSearchPage(
                        siteSettings.SiteId,
                        pageNumber,
                        pageSize,
                        searchText,
                        out totalPages);
            }
            
            if (pageNumber > totalPages)
            {
                pageNumber = 1;
                
            }

            if (searchText.Length > 0)
            {
                txtSearchUser.Text = Server.HtmlEncode(searchText);
            }

            Literal topPageLinks = new Literal();
            string pageUrl = SiteRoot + "/MemberList.aspx?pagenumber=";

            string alphaChars;

            if (WebConfigSettings.GetAlphaPagerCharsFromResourceFile)
            {
                alphaChars = Resource.AlphaPagerChars;
            }
            else
            {
                alphaChars = WebConfigSettings.AlphaPagerChars;
            }

            topPageLinks.Text = UIHelper.GetAlphaPagerLinks(
                pageUrl,
                pageNumber,
                alphaChars,
                userNameBeginsWith);

            this.spnTopPager.Controls.Add(topPageLinks);

            pageUrl = SiteRoot
                + "/MemberList.aspx?"
                + "search=" + Server.UrlEncode(Server.HtmlEncode(searchText))
                + "&amp;pagenumber={0}";

            pgrMembers.PageURLFormat = pageUrl;
            pgrMembers.ShowFirstLast = true;
            pgrMembers.CurrentIndex = pageNumber;
            pgrMembers.PageSize = pageSize;
            pgrMembers.PageCount = totalPages;
            pgrMembers.Visible = (totalPages > 1);


            rptUsers.DataSource = siteUserPage;
            rptUsers.DataBind();


        }

        void btnSearchUser_Click(object sender, EventArgs e)
        {
            string pageUrl = SiteRoot + "/MemberList.aspx?search=" + Server.UrlEncode(Server.HtmlEncode(txtSearchUser.Text)) + "&pagenumber=";

            WebUtils.SetupRedirect(this, pageUrl);

        }

        void btnIPLookup_Click(object sender, EventArgs e)
        {
            pgrMembers.Visible = false;
            List<SiteUser> users = SiteUser.GetByIPAddress(siteSettings.SiteGuid, txtIPAddress.Text);
            rptUsers.DataSource = users;
            rptUsers.DataBind();


        }

        private void PopulateLabels()
        {
           
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MemberListLink);

            MetaDescription = string.Format(CultureInfo.InvariantCulture, 
                Resource.MetaDescriptionMemberListFormat, siteSettings.SiteName);

            btnIPLookup.Text = Resource.LookupUserByIPAddressButton;

            
            lnkAllUsers.Text = Resource.MemberListAllUsersLink;
            btnSearchUser.Text = Resource.MemberListSearchButton;
        }

        private void LoadSettings()
        {
            lnkAllUsers.NavigateUrl = SiteRoot + "/MemberList.aspx";

            allowView = WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList);

            if (WebUser.IsAdmin)
            {
                IsAdmin = true;
                canManageUsers = true;
                spnIPLookup.Visible = true;
            }
            else
            {
                canManageUsers = WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers);
            }

            if (IsAdmin || canManageUsers)
            {
                lnkNewUser.Visible = true;
                lnkNewUser.Text = Resource.MemberListAddUserLabel;
                lnkNewUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userId=-1";
            }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            
            if (Request.Params["letter"] != null)
            {
                userNameBeginsWith = Request.Params["letter"].Trim();
            }

            if (Request.Params["search"] != null)
            {
                searchText = Request.Params["search"].Trim();
            }

            
            pageSize = WebConfigSettings.MemberListPageSize;

            CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                if (profileConfig.Contains("WebSiteUrl"))
                {
                    CProfilePropertyDefinition webSiteUrlProperty = profileConfig.GetPropertyDefinition("WebSiteUrl");
                    if(
                        (webSiteUrlProperty.OnlyVisibleForRoles.Length == 0)
                        || (WebUser.IsInRoles(webSiteUrlProperty.OnlyVisibleForRoles))
                        )
                    {
                        ShowWebSiteColumn = true;
                    }

                }
            }

            if (!ShowWebSiteColumn)
            {
                //thTitle.ColSpan = 4;
                //divNewUser.ColSpan = 4;
                //tdModulePager.ColSpan = 4;
                thWebLink.Visible = false;

            }

            if (WebConfigSettings.UseRelatedSiteMode)
            {
                // this can't be used in related site mode
                // because we can't assume forum posts were in this site.
                ShowGroupPostColumn = false;
            }

        }

		
		
		
	}
}
