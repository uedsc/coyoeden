using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.FeedUI
{
    public partial class FeedManagerModule : SiteModuleControl
    {
        #region Properties 

        private string previousPubDate;
        private int feedListCacheTimeout = 3660;
        private int entryCacheTimeout = 3620;
        private int maxDaysOld = 90;
        private int maxEntriesPerFeed = 90;
        private bool allowExternalImages = false;
        private bool ShowFeedListOnRight = true;
        private int RepeatColumns = 1;
        private bool ShowAggregateFeedLink = true;
        private bool showErrorMessageOnInvalidPosts = false;
        // added by Walter
        private int totalPages = 1;
        private int pageSize = 10;
        private DataTable dtFeedList = null;

        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;
        protected bool UseFeedListAsFilter = false;
        protected string RssImageFile = WebConfigSettings.RSSImageFileName;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected bool LinkToAuthorSite = true;
        protected bool RSSFeedSortAscending = false;
        protected bool RSSAggregatorShowAuthor = false;
        protected bool RSSAggregatorShowFeedName = false;
        protected bool RSSAggregatorShowFeedNameBeforeContent = false;
        protected bool RSSAggregatorUseExcerpt = false;
        protected bool EnableSelectivePublishing = false;
        protected bool EnableInPlacePublishing = false;
        protected int RSSAggregatorExcerptLength = 250;
        protected string RSSAggregatorExcerptSuffix = "...";
        protected bool ShowIndividualFeedLinks = true;
        protected bool ShowItemDetail = true;
        protected string ConfirmImage = string.Empty;
        protected bool RemoveMarkupFromInvalidPosts = true;
        protected string ErrorMessage = string.Empty;
        protected bool useNeatHtml = true;
        protected bool UseFillerOnEmptyDays = false;
        private bool useCalendar = false;

        protected int ItemID
        {
            get
            {
                if (ViewState["ItemID"] != null)
                {
                    return Convert.ToInt32(ViewState["ItemID"]);
                }
                return -1;
            }
            set { ViewState["ItemID"] = value; }

        }
        
        protected bool EnableInPlaceEditing
        {
            get { return (this.IsEditable && EnableInPlacePublishing && EnableSelectivePublishing); }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            SetupCss();
            PopulateLabels();
            PopulateControls();

            if (!IsPostBack)
            {
                if (useCalendar)
                {
                    
                    BindCalendar();
                }
                else
                {
                    pgrRptEntries.CurrentIndex = 1;
                    BindRepeater();
                }
            }


        }

        
        private void PopulateControls()
        {
            if (!this.ShowFeedListOnRight)
            {
                this.divNav.CssClass = "rssnavleft";
                this.divFeedEntries.CssClass = "rsscenter-leftnav";

            }
            
            if (dtFeedList == null) { dtFeedList = RssFeed.GetFeeds(ModuleId); }

            DataView members = dtFeedList.DefaultView;
            //members.Sort = "Author";
            
            if (UseFeedListAsFilter)
            {
                if (ItemID == -1)
                {
                    ItemID = Convert.ToInt32(members.Table.Rows[0]["ItemID"]);

                }
                BindSelectedFeed();

            }

			String rssFriendlyUrl = String.Format("{0}/aggregator{1}rss.aspx", SiteRoot, this.ModuleId);

            if (ShowAggregateFeedLink)
            {
                lnkAggregateRSS.HRef = rssFriendlyUrl;
				imgAggregateRSS.Src = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, RssImageFile);
            }
            else
            {
                lnkAggregateRSS.Visible = false;
            }

            if (RepeatColumns > 1)
            {
                this.dlstFeedList.RepeatDirection = RepeatDirection.Horizontal;
                this.dlstFeedList.RepeatColumns = RepeatColumns;
            }

            if (ShowIndividualFeedLinks)
            {
                dlstFeedList.DataSource = members;
                dlstFeedList.DataBind();
            }

            if (
                ((!this.ShowAggregateFeedLink)
                && (!this.ShowIndividualFeedLinks)
                )
                || (this.RenderInWebPartMode)
                )
            {
                this.divNav.Visible = false;
                this.divFeedEntries.CssClass = String.Empty;
            }

        }

        private void BindSelectedFeed()
        {
            RssFeed feed = new RssFeed(ModuleId, ItemID);
            this.lblFeedHeading.Visible = true;
			this.lblFeedHeading.Text = String.Format("<h2>{0}</h2>", feed.Author);
        }

        private DataTable GetEntriesTable()
        {
            DataTable entriesTable = FeedCache.GetRssFeedEntries(
                ModuleId,
                ModuleGuid,
                entryCacheTimeout,
                maxDaysOld,
                maxEntriesPerFeed,
                EnableSelectivePublishing);
           

            return entriesTable;
        }

        private void BindCalendar()
        {
            pgrRptEntries.Visible = false;
            dataCal1.Visible = true;
            DataTable entries = GetEntriesTable();
            LocalizeTimes(entries);
            dataCal1.DataSource = entries;
            dataCal1.DataBind();

        }

        private void LocalizeTimes(DataTable dt)
        {
            if (TimeZoneOffset == 0) { return; }

            foreach (DataRow RowNotInTableException in dt.Rows)
            {
                RowNotInTableException["PubDate"] = Convert.ToDateTime(RowNotInTableException["PubDate"]).AddHours(TimeZoneOffset);
            }

        }

        void dataCal1_SelectionChanged(object sender, EventArgs e)
        {
            BindCalendar();
        }

        void dataCal1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            BindCalendar();
        }


        private void BindRepeater()
        {
            dataCal1.Visible = false;
            rptEntries.Visible = true;
            DataView entries = GetEntriesTable().DefaultView;

            // filter just entries confirmed to end users
            if (!this.EnableInPlaceEditing)
            {
                entries.RowFilter = "Confirmed = true";
            }
            else
            {
                entries.RowFilter = string.Empty;
            }

            if (!EnableSelectivePublishing)
            {
                entries.RowFilter = string.Empty;
            }

            if ((UseFeedListAsFilter)&&(ItemID > -1))
            {
                if(entries.RowFilter == string.Empty)
                {
                    entries.RowFilter = "FeedId = " + ItemID.ToInvariantString();
                }
                else
                {
                    entries.RowFilter += " AND FeedId = " + ItemID.ToInvariantString();
                }
            }

            if (RSSFeedSortAscending)
            {
                entries.Sort = "PubDate ASC";
            }
            else
            {
                entries.Sort = "PubDate DESC";
            }

            PagedDataSource pagedDS = new PagedDataSource();

            pagedDS.DataSource = entries;
            pagedDS.AllowPaging = true;
            pagedDS.PageSize = pageSize;
            pagedDS.CurrentPageIndex = pgrRptEntries.CurrentIndex - 1;

            totalPages = 1;
            int totalRows = entries.Count;

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            if (this.totalPages > 1)
            {

                pgrRptEntries.ShowFirstLast = true;
                pgrRptEntries.PageSize = pageSize;
                pgrRptEntries.PageCount = totalPages;
            }
            else
            {
                pgrRptEntries.Visible = false;
            }

            rptEntries.DataSource = pagedDS;
            rptEntries.DataBind();

        }

        protected void pgrRptEntries_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            pgrRptEntries.CurrentIndex = currentPageIndex;
            BindRepeater();
            updPnlRSSA.Update();
        }


        protected void rptEntries_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Confirm")
            {
                string entryInfo = (string)e.CommandArgument;
                int sep = -1;
                sep = entryInfo.IndexOf('_');
                if (sep != -1)
                {
                    string[] entryHash = entryInfo.Split('_');
                    bool published = Convert.ToBoolean(entryHash[1]);
                    if (published)
                    {
                        RssFeed.UnPublish(ModuleGuid, Convert.ToInt32(entryHash[0]));
                    }
                    else
                    {
                        RssFeed.Publish(ModuleGuid, Convert.ToInt32(entryHash[0]));
                    }
                    BindRepeater();

                }

            }
        }

        void dlstFeedList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "filter")
            {
                ItemID = Convert.ToInt32(e.CommandArgument);
                pgrRptEntries.CurrentIndex = 1;
                BindSelectedFeed();
                BindRepeater();
            }
            
        }

        private void PopulateLabels()
        {
            Title1.EditUrl = SiteRoot + "/FeedManager/FeedEdit.aspx";
            Title1.EditText = FeedResources.AddButton;
            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if ((IsEditable)&&(EnableSelectivePublishing)&&(!EnableInPlaceEditing))
            {
				Title1.LiteralExtraMarkup = String.Format("&nbsp;<a href='{0}/FeedManager/FeedManager.aspx?pageid={1}&amp;mid={2}' class='ModuleEditLink' title='{3}'>{3}</a>", SiteRoot, PageId.ToInvariantString(), ModuleId.ToInvariantString(), FeedResources.ManagePublishingLink);
            }

        }

        private void LoadSettings()
        {
            Page.EnableViewState = true;
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            pnlContainer.ModuleId = ModuleId;            

            ConfirmImage = this.ImageSiteRoot + "/Data/SiteImages/confirmed";

            feedListCacheTimeout = WebUtils.ParseInt32FromHashtable(
                Settings, "RSSFeedFeedListCacheTimeoutSetting", feedListCacheTimeout);

            RepeatColumns = WebUtils.ParseInt32FromHashtable(
                Settings, "RSSFeedFeedListColumnsSetting", RepeatColumns);

            entryCacheTimeout = WebUtils.ParseInt32FromHashtable(
                Settings, "RSSFeedEntryCacheTimeoutSetting", entryCacheTimeout);

            maxDaysOld = WebUtils.ParseInt32FromHashtable(
                Settings, "RSSFeedMaxDayCountSetting", maxDaysOld);

            maxEntriesPerFeed = WebUtils.ParseInt32FromHashtable(
                Settings, "RSSFeedMaxPostsPerFeedSetting", maxEntriesPerFeed);

            ShowItemDetail = !WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedShowHeadingsOnlySetting", false);

            useCalendar = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedUseCalendarView", useCalendar);

            UseFillerOnEmptyDays = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedPadEmptyDaysInCalendarView", UseFillerOnEmptyDays);
               

            //showErrorMessageOnInvalidPosts = WebUtils.ParseBoolFromHashtable(
            //    Settings, "RSSFeedShowErrorMessageOnInvalidPosts", false);

            RSSFeedSortAscending = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedSortAscending", RSSFeedSortAscending);


            allowExternalImages = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedAllowExternalImages", allowExternalImages);

            EnableSelectivePublishing = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableSelectivePublishing", EnableSelectivePublishing);

            EnableInPlacePublishing = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableInPlacePublishing", EnableInPlacePublishing);

            if (allowExternalImages) allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;

            pageSize = WebUtils.ParseInt32FromHashtable(
                Settings, "RSSAggregatorPageSizeSetting", pageSize);

            RSSAggregatorShowAuthor = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSAggregatorShowAuthor", RSSAggregatorShowAuthor);

            RSSAggregatorShowFeedName = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSAggregatorShowFeedName", RSSAggregatorShowFeedName);

            RSSAggregatorShowFeedNameBeforeContent = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSAggregatorShowFeedNameBeforeContent", RSSAggregatorShowFeedNameBeforeContent);

            

            RSSAggregatorUseExcerpt = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSAggregatorExcerptSetting", RSSAggregatorUseExcerpt);

            RSSAggregatorExcerptLength = WebUtils.ParseInt32FromHashtable(
                Settings, "RSSAggregatorExcerptLengthSetting", RSSAggregatorExcerptLength);

            if (Settings.Contains("RSSAggregatorExcerptSuffixSetting"))
            {
                RSSAggregatorExcerptSuffix = Settings["RSSAggregatorExcerptSuffixSetting"].ToString();
            }

            if (RenderInWebPartMode)
            {
                ShowItemDetail = false;
            }

            if (showErrorMessageOnInvalidPosts)
            {
                ErrorMessage = FeedResources.MalformedMarkupWarning;
            }

            ShowFeedListOnRight = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedShowFeedListRightSetting", false);

            UseFeedListAsFilter = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedUseFeedListAsFilterSetting", false);

            LinkToAuthorSite = !UseFeedListAsFilter;

            ShowAggregateFeedLink = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedShowAggregateFeedLink", false);

            ShowIndividualFeedLinks = WebUtils.ParseBoolFromHashtable(
                Settings, "RSSFeedShowIndividualFeedLinks", false);

            if (Settings.Contains("RSSFeedListLabelSetting"))
            {
                this.lblFeedListName.Text = Settings["RSSFeedListLabelSetting"].ToString();

            }

            useNeatHtml = WebUtils.ParseBoolFromHashtable(
                Settings, "FilterInvalidMarkupAndPotentiallyInsecureContent", useNeatHtml);

           
        }

        private void SetupCss()
        {
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("rsscss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "rsscss";
					cssLink.Text = String.Format("\n<link href='{0}rssmodule.css' type='text/css' rel='stylesheet' media='screen' />", SiteUtils.GetSkinBaseUrl());

                    stylesheet.Controls.Add(cssLink);
                }
            }
            
        }

        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            rptEntries.ItemCommand += new RepeaterCommandEventHandler(rptEntries_ItemCommand);
            pgrRptEntries.Command += new CommandEventHandler(pgrRptEntries_Command);
            dlstFeedList.ItemCommand += new DataListCommandEventHandler(dlstFeedList_ItemCommand);

            dataCal1.VisibleMonthChanged += new MonthChangedEventHandler(dataCal1_VisibleMonthChanged);
            dataCal1.SelectionChanged += new EventHandler(dataCal1_SelectionChanged);
        }

        

    }
}