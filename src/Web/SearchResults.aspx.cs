// Author:					    Joe Audette
// Created:				        2005-06-26
//	Last Modified:              2010-01-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI.Pages
{
	
    public partial class SearchResults : CBasePage
	{
       
		private static readonly ILog log = LogManager.GetLogger(typeof(SearchResults));

        private string query = string.Empty;
        private int pageNumber = 1;
        private int pageSize = WebConfigSettings.SearchResultsPageSize;
        private int totalHits = 0;
        private int totalPages = 1;
		private bool indexVerified = false;
        private bool showModuleTitleInResultLink = WebConfigSettings.ShowModuleTitleInSearchResultLink;
        private bool isSiteEditor = false;
        private Guid featureGuid = Guid.Empty;
        private bool queryErrorOccurred = false;

  
        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnDoSearch.Click += new EventHandler(btnDoSearch_Click);
            btnRebuildSearchIndex.Click += new EventHandler(btnRebuildSearchIndex_Click);
           
            SuppressMenuSelection();
            SuppressPageMenu();
        }

        

        
        #endregion

		private void Page_Load(object sender, EventArgs e)
		{
            if (WebConfigSettings.DisableSearchIndex)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot());
                return;
            }

            isSiteEditor = WebUser.IsAdminOrContentAdmin || (SiteUtils.UserIsSiteEditor());

			this.query = string.Empty;

            if (siteSettings == null)
            {
                siteSettings = CacheHelper.GetCurrentSiteSettings();
            }

			PopulateLabels();
            SetupScript();
            ShowNoResults();

            if (!WebConfigSettings.DisableSearchFeatureFilters)
            {
                featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);
                
                if (!Page.IsPostBack)
                {
                    BindFeatureList();
                    ddFeatureList.Items.Insert(0, new ListItem(Resource.SearchAllContentItem, Guid.Empty.ToString()));
                    if (ddFeatureList.Items.Count > 0)
                    {
                        ListItem item = ddFeatureList.Items.FindByValue(featureGuid.ToString());
                        if (item != null)
                        {
                            ddFeatureList.ClearSelection();
                            item.Selected = true;

                        }
                    }
                    else
                    {
                        ddFeatureList.Visible = false;
                    }
                }

            }
            else
            {
                ddFeatureList.Visible = false;
            }

            
            
            //got here by a cross page postback from another page if Page.PreviousPage is not null
            // this occurs when the seach input is used in the skin rather than the search link
            if (Page.PreviousPage != null)
            {
                HandleCrossPagePost();
            }
            else
            {
                DoSearch();
            }


		}

        private void HandleCrossPagePost()
        {
            
            SearchInput previousPageSearchInput = (SearchInput)PreviousPage.Master.FindControl("SearchInput1");
            // try in page if not found in masterpage
            if (previousPageSearchInput == null) { previousPageSearchInput = (SearchInput)PreviousPage.FindControl("SearchInput1"); }

            if (previousPageSearchInput != null)
            {
                TextBox prevSearchTextBox = (TextBox)previousPageSearchInput.FindControl("txtSearch");
                if ((prevSearchTextBox != null)&&(prevSearchTextBox.Text.Length > 0))
                {
                    //this.txtSearchInput.Text = prevSearchTextBox.Text;
                    WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx?q=" + Server.UrlEncode(prevSearchTextBox.Text));
                    return;
                }
            }

            DoSearch();

           

        }

        private List<string> GetUserRoles()
        {
            List<string> userRoles = new List<string>();

            userRoles.Add("All Users");
            if (Request.IsAuthenticated)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    using (IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, currentUser.UserId))
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(reader["RoleName"].ToString());
                        }

                    }

                }


            }

            return userRoles;
        }


        private void DoSearch()
        {
            if (Page.IsPostBack) { return; }

            if (Request.QueryString.Get("q") == null) { return; }

            query = Request.QueryString.Get("q");

            if (this.query.Length == 0) { return; }

            pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);

            //txtSearchInput.Text = Server.HtmlEncode(query).Replace("&quot;", "\"") ;
            txtSearchInput.Text = SecurityHelper.SanitizeHtml(query);

            // this is only to make sure its initialized
            // before indexing is queued on a thread
            // because there is no HttpContext on
            // external threads and httpcontext is needed to initilaize
            // once initialized its cached
            IndexBuilderProviderCollection
                indexProviders = IndexBuilderManager.Providers;

            queryErrorOccurred = false;
          
            IndexItemCollection searchResults = IndexHelper.Search(
                siteSettings.SiteId,
                isSiteEditor,
                GetUserRoles(),
                featureGuid,
                query,
                WebConfigSettings.EnableSearchResultsHighlighting,
                WebConfigSettings.SearchResultsFragmentSize,
                pageNumber,
                pageSize,
                out totalHits,
                out queryErrorOccurred);

            if (searchResults.Count == 0)
            {
                
                ShowNoResults();
                InitIndexIfNeeded();
                return;
            }

            int start = 1;
            if (pageNumber > 1) 
            { 
                start = ((pageNumber -1) * pageSize) + 1; 
            }

            int end = pageSize;
            if (start > 1) { end += start; }

            if (end > totalHits)
            {
                end = totalHits;
            }

            this.pnlSearchResults.Visible = true;
            this.pnlNoResults.Visible = false;
            this.lblDuration.Visible = true;
            this.lblSeconds.Visible = true;

            this.lblFrom.Text = (start).ToString();
            this.lblTo.Text = end.ToString(CultureInfo.InvariantCulture);
            this.lblTotal.Text = totalHits.ToString(CultureInfo.InvariantCulture);
            this.lblQueryText.Text = Server.HtmlEncode(query);
            float duration = searchResults.ExecutionTime*0.0000001F;
            this.lblDuration.Text = duration.ToString();
            divResults.Visible = true;

            totalPages = 1;
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            if (pageSize > 0) { totalPages = totalHits / pageSize; }

            if (totalHits <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalHits, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            //totalPages always seems 1 more than it should be not sure why
            //if (totalPages > 1) { totalPages -= 1; }

            string searchUrl = SiteRoot
                + "/SearchResults.aspx?q=" + Server.UrlEncode(query)
                + "&amp;p={0}&amp;f=" + featureGuid.ToString();

            pgrTop.PageURLFormat = searchUrl;
            pgrTop.ShowFirstLast = true;
            pgrTop.CurrentIndex = pageNumber;
            pgrTop.PageSize = pageSize;
            pgrTop.PageCount = totalPages;
            pgrTop.Visible = (totalPages > 1);

            pgrBottom.PageURLFormat = searchUrl;
            pgrBottom.ShowFirstLast = true;
            pgrBottom.CurrentIndex = pageNumber;
            pgrBottom.PageSize = pageSize;
            pgrBottom.PageCount = totalPages;
            pgrBottom.Visible = (totalPages > 1);

            

            this.rptResults.DataSource = searchResults;
            this.rptResults.DataBind();

            
            
        }

        private void BindFeatureList()
        {
            using (IDataReader reader = ModuleDefinition.GetSearchableModules(siteSettings.SiteId))
            {
                ListItem listItem;

                // this flag tells it to look first for a web config setting for the resource string
                // corresponding to SearchListName value
                // it allows you to customize searchlist names wheeas by default they are just localized
                bool useConfigOverrides = true;

                while (reader.Read())
                {
                    string featureid = reader["Guid"].ToString();

                    if (!WebConfigSettings.SearchableFeatureGuidsToExclude.Contains(featureid))
                    {
                        listItem = new ListItem(
                            ResourceHelper.GetResourceString(
                            reader["ResourceFile"].ToString(),
                            reader["SearchListName"].ToString(),
                            useConfigOverrides),
                            featureid);

                        ddFeatureList.Items.Add(listItem);
                    }
                    
                }

            }

        }

        

        private void InitIndexIfNeeded()
        {
            if (indexVerified) { return; }

            indexVerified = true;
            if (!IndexHelper.VerifySearchIndex(siteSettings))
            {
                this.lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;
                Thread.Sleep(5000); //wait 5 seconds
                SiteUtils.QueueIndexing();
            }
            
            //lblDuration.Visible = false;
            //lblSeconds.Visible = false;
            //pnlSearchResults.Visible = false;
            //pnlNoResults.Visible = true;
           

        }

        


	    private void ShowNoResults()
        {
            if (queryErrorOccurred)
            {
                lblNoResults.Text = Resource.SearchQueryInvalid;
            }
            divResults.Visible = false;
            pnlNoResults.Visible = (txtSearchInput.Text.Length > 0);
            

            //this.lblFrom.Text = "0";
            //this.lblTo.Text = "0";
            //this.lblTotal.Text = "0";
            //this.lblQueryText.Text = Server.HtmlEncode(query);
            

            //divResults.Visible = (txtSearchInput.Text.Length > 0);

            
            
        }

        protected string FormatLinkText(string pageName, string moduleTtile, string itemTitle)
        {
            if (showModuleTitleInResultLink)
            {
                if (itemTitle.Length > 0)
                {
                    return pageName + " &gt; " + moduleTtile + " &gt; " + itemTitle;
                }

            }

            if (itemTitle.Length > 0)
            {
                return pageName +  " &gt; " + itemTitle;
            }


            return pageName;

  
        }

        private void btnDoSearch_Click(object sender, EventArgs e)
        {
            
            if (ddFeatureList.SelectedValue.Length == 36)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx?q=" 
                    + Server.UrlEncode(this.txtSearchInput.Text)
                    + "&f=" + ddFeatureList.SelectedValue
                    );

                return;
            }

            WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx?q=" + Server.UrlEncode(this.txtSearchInput.Text));

        }

        void btnRebuildSearchIndex_Click(object sender, EventArgs e)
        {
            IndexingQueue.DeleteAll();
            IndexHelper.DeleteSearchIndex(siteSettings);
            IndexHelper.VerifySearchIndex(siteSettings);
            
            this.lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;
            Thread.Sleep(5000); //wait 5 seconds
            SiteUtils.QueueIndexing();
           
            
        }

        private void SetupScript()
        {
            if (WebConfigSettings.DisablejQuery) { return; }
            if (!WebConfigSettings.OpenSearchDownloadLinksInNewWindow) { return; }

            // make shared files download links open in a new window
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$('a[href*=Download.aspx]')");
            //script.Append(".bind('click', function(){return confirm('sure you want to download ?')});");
            script.Append(".bind('click', function(){window.open(this.href,'_blank');return false;}); ");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "searchpage",
                script.ToString());

        }


		private void PopulateLabels()
		{
            if (siteSettings == null) return;

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SearchPageTitle);

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionSearchFormat, siteSettings.SiteName);

			lblMessage.Text = string.Empty;
            divResults.Visible = true;

            btnDoSearch.Text = Resource.SearchButtonText;
            SiteUtils.SetButtonAccessKey(btnDoSearch, AccessKeys.SearchButtonTextAccessKey);

            btnRebuildSearchIndex.Text = Resource.SearchRebuildIndexButton;
            UIHelper.AddConfirmationDialog(btnRebuildSearchIndex, Resource.SearchRebuildIndexWarning);

            divDelete.Visible = (WebConfigSettings.ShowRebuildSearchIndexButtonToAdmins && WebUser.IsAdmin);

            lblNoResults.Text = Resource.SearchResultsNotFound;
            
		}

		

        public string BuildUrl(IndexItem indexItem)
        {
            if (indexItem.UseQueryStringParams)
            {
                return SiteRoot + "/" + indexItem.ViewPage
                    + "?pageid="
                    + indexItem.PageId.ToString(CultureInfo.InvariantCulture)
                    + "&mid="
                    + indexItem.ModuleId.ToString(CultureInfo.InvariantCulture)
                    + "&ItemID="
                    + indexItem.ItemId.ToString(CultureInfo.InvariantCulture)
                    + indexItem.QueryStringAddendum;
                    
            }
            else
            {
                return SiteRoot + "/" + indexItem.ViewPage;
            }

        }
	}
}
