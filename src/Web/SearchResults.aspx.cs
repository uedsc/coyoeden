
using System;
using System.Linq;
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
using SystemX.Web;

namespace Cynthia.Web.UI.Pages
{
	
    public partial class SearchResults : CBasePage,IHtmlWriterControl
	{
       
		private static readonly ILog log = LogManager.GetLogger(typeof(SearchResults));
        private int pageNumber = 1;
        private int pageSize = WebConfigSettings.SearchResultsPageSize;
        private int totalHits = 0;
        private int totalPages = 1;
		private bool indexVerified = false;
        private bool showModuleTitleInResultLink = WebConfigSettings.ShowModuleTitleInSearchResultLink;
        private bool isSiteEditor = false;
        private List<Guid> featureGuids;
        private bool queryErrorOccurred = false;
		/// <summary>
		/// search term
		/// </summary>
		protected string SearchTerm { get; private set; }
  
        #region base overrides
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnRebuildSearchIndex.Click += new EventHandler(btnRebuildSearchIndex_Click);
           
            SuppressMenuSelection();
            SuppressPageMenu();
        }

		protected override void Render(HtmlTextWriter writer)
		{
			HtmlWriter = writer;
			base.Render(writer);
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

			SearchTerm= string.Empty;

            if (siteSettings == null)
            {
                siteSettings = CacheHelper.GetCurrentSiteSettings();
            }

			PopulateLabels();
            SetupScript();
            ShowNoResults();

			featureGuids = WebUtils.ParseGuidsFromQueryString("f", Guid.Empty);            
            
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
					WebUtils.SetupRedirect(this, String.Format("{0}/SearchResults.aspx?q={1}", SiteRoot, Server.UrlEncode(prevSearchTextBox.Text)));
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

            SearchTerm = Request.QueryString.Get("q");

			if (SearchTerm.Length == 0) { return; }
			SearchTerm = SecurityHelper.SanitizeHtml(SearchTerm);

            pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);

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
                SearchTerm,
                WebConfigSettings.EnableSearchResultsHighlighting,
                WebConfigSettings.SearchResultsFragmentSize,
                pageNumber,
                pageSize,
                out totalHits,
                out queryErrorOccurred,featureGuids.ToArray());

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
            this.lblQueryText.Text = Server.HtmlEncode(SearchTerm);
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

			string searchUrl = String.Format("{0}/SearchResults.aspx?q={1}&amp;p={{0}}&amp;f={2}", SiteRoot, Server.UrlEncode(SearchTerm), arrayToStr(",", featureGuids));

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
        }
	    private void ShowNoResults()
        {
            if (queryErrorOccurred)
            {
                lblNoResults.Text = Resource.SearchQueryInvalid;
            }
            divResults.Visible = false;
            pnlNoResults.Visible = (SearchTerm.Length > 0);
            
        }

        protected string FormatLinkText(string pageName, string moduleTtile, string itemTitle)
        {
            if (showModuleTitleInResultLink)
            {
                if (itemTitle.Length > 0)
                {
					return String.Format("{0} &gt; {1} &gt; {2}", pageName, moduleTtile, itemTitle);
                }

            }

            if (itemTitle.Length > 0)
            {
				return String.Format("{0} &gt; {1}", pageName, itemTitle);
            }


            return pageName;

  
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

            btnRebuildSearchIndex.Text = Resource.SearchRebuildIndexButton;
            UIHelper.AddConfirmationDialog(btnRebuildSearchIndex, Resource.SearchRebuildIndexWarning);

            divDelete.Visible = (WebConfigSettings.ShowRebuildSearchIndexButtonToAdmins && WebUser.IsAdmin);

            lblNoResults.Text = Resource.SearchResultsNotFound;
            
		}

		

        public string BuildUrl(IndexItem indexItem)
        {
            if (indexItem.UseQueryStringParams)
            {
				return String.Format("{0}/{1}?pageid={2}&mid={3}&ItemID={4}{5}", SiteRoot, indexItem.ViewPage, indexItem.PageId.ToString(CultureInfo.InvariantCulture), indexItem.ModuleId.ToString(CultureInfo.InvariantCulture), indexItem.ItemId.ToString(CultureInfo.InvariantCulture), indexItem.QueryStringAddendum);
                    
            }
            else
            {
				return String.Format("{0}/{1}", SiteRoot, indexItem.ViewPage);
            }

        }

		#region IHtmlWriterControl Members

		public Page CurPage
		{
			get { return this.Page; }
		}

		public HtmlTextWriter HtmlWriter
		{
			get;
			private set;
		}

		#endregion

		#region helper methods
		/// <summary>
		/// JOIN a collection of items into a string with specified separator.TODO:Remove this method to SystemX.Utils
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="separator"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		static string arrayToStr<T>(string separator, IEnumerable<T> items) {
			var sb = new StringBuilder();
			items.ToList().ForEach(x => {
				sb.AppendFormat("{0}{1}",x,separator??" ");
			});
			return sb.ToString();
		}
		#endregion
	}
}
