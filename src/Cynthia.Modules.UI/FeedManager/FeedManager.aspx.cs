/// Author:					Joe Audette and Walter Ferrari
/// Created:				2008-09-27
/// Last Modified:			2008-10-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Cynthia.Business;
using Resources;

namespace Cynthia.Web.FeedUI
{
    public partial class FeedManagerPage : CBasePage
    {
       
        private Module module = null;
        private Hashtable moduleSettings = null;
        private bool canEdit = false;
        private int totalPages = 1;
        private int pageSize = 10;
        private string dateFormat;
        private int maxDaysOld = 90;
        private int maxEntriesPerFeed = 90;
        private string previousPubDate;
        private int feedListCacheTimeout = 3660;
        private int entryCacheTimeout = 3620;
        private bool allowExternalImages = false;

        protected int PageId = -1;
        protected int ModuleId = -1;
        protected int ItemId = -1;
        protected bool EnableSelectivePublishing = false;
        protected bool RSSAggregatorUseExcerpt = false;
        protected bool RSSFeedSortAscending = false;
        protected string ConfirmImage = string.Empty;
        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();

            if (!EnableSelectivePublishing)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            SetupCss();
            PopulateLabels();
            PopulateControls();

            

        }

        private void PopulateControls()
        {
            if (!IsPostBack)
            {
                pgrRptEntries.CurrentIndex = 1;
                BindRepeater();
            }

        }

        private DataView GetEntriesTable()
        {
            DataTable entriesTable = null;

            entriesTable = FeedCache.GetRssFeedEntries(
                ModuleId,
                module.ModuleGuid,
                entryCacheTimeout,
                maxDaysOld,
                maxEntriesPerFeed,
                EnableSelectivePublishing);


            return entriesTable.DefaultView;
        }


        private void BindRepeater()
        {
            DataView entries = GetEntriesTable();

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
                        RssFeed.UnPublish(module.ModuleGuid, Convert.ToInt32(entryHash[0]));
                    }
                    else
                    {
                        RssFeed.Publish(module.ModuleGuid, Convert.ToInt32(entryHash[0]));
                    }
                    BindRepeater();

                }

            }
        }

        protected string GetDateHeader(DateTime pubDate)
        {
            string retVal = string.Empty;
            if (previousPubDate != pubDate.ToString(dateFormat))
            {
                previousPubDate = pubDate.ToString(dateFormat);
                retVal = previousPubDate;
            }
            return retVal;
        }


        private void PopulateLabels()
        {

        }

        private void LoadSettings()
        {
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            if (ModuleId == -1) { return; }

            module = new Module(ModuleId, CurrentPage.PageId);
            moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);
            if (moduleSettings == null) { return; }

            dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            ConfirmImage = this.ImageSiteRoot + "/Data/SiteImages/confirmed";

            feedListCacheTimeout = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedFeedListCacheTimeoutSetting", feedListCacheTimeout);

            maxDaysOld = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedMaxDayCountSetting", maxDaysOld);

            maxEntriesPerFeed = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedMaxPostsPerFeedSetting", maxEntriesPerFeed);

            EnableSelectivePublishing = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "EnableSelectivePublishing", EnableSelectivePublishing);

            entryCacheTimeout = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedEntryCacheTimeoutSetting", entryCacheTimeout);

            pageSize = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSAggregatorPageSizeSetting", pageSize);

            allowExternalImages = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "RSSFeedAllowExternalImages", allowExternalImages);

            if (allowExternalImages) allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;

            litHeading.Text = string.Format(CultureInfo.InvariantCulture,
                FeedResources.PublishingHeaderFormat, module.ModuleTitle);

            lnkBackToPage.Text = string.Format(CultureInfo.InvariantCulture,
                FeedResources.BackToPageLinkFormat, CurrentPage.PageName);

            lnkBackToPage.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkEditFeeds.Text = FeedResources.AddButton;
            lnkEditFeeds.NavigateUrl = SiteRoot + "/FeedManager/FeedEdit.aspx?pageid="
                + PageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture);


            

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
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl()
                    + "rssmodule.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }
            
        }

        private void LoadParams()
        {
            PageId = WebUtils.ParseInt32FromQueryString("pageid", PageId);
            ModuleId = WebUtils.ParseInt32FromQueryString("mid", ModuleId);
            canEdit = UserCanEditModule(ModuleId);

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            rptEntries.ItemCommand += new RepeaterCommandEventHandler(rptEntries_ItemCommand);
            pgrRptEntries.Command += new CommandEventHandler(pgrRptEntries_Command);


        }

        #endregion
    }
}
