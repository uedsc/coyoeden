// Author:				Joe Audette
// Created:			    2004-08-14
// Last Modified:	    2009-06-04
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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.BlogUI
{
   
    public partial class ArchiveViewControl : UserControl
    {
        #region Private Properties

        private int countOfDrafts = 0;
        private Hashtable moduleSettings;
        private bool BlogUseTagCloudForCategoriesSetting = false;
        private CBasePage basePage;
        private Module blogModule = null;

        #endregion

        #region Protected Properties

        protected string SiteRoot = string.Empty;
        protected string ImageSiteRoot = string.Empty;
        protected string BlogDateTimeFormat;
        protected string FeedBackLabel = ConfigurationManager.AppSettings["BlogFeedbackLabel"];
        protected int PageId = -1;
        protected int ModuleId = 0;
        protected int ItemId = 0;
        protected int Month = DateTime.UtcNow.Month;
        protected int Year = DateTime.UtcNow.Year;
        protected bool ShowCategories = false;
        protected bool ShowArchives = false;
        protected bool NavigationOnRight = false;
        protected bool ShowFeedLinks = true;
        protected bool ShowStatistics = true;
        protected bool ShowAddFeedLinks = true;
        protected bool AllowComments = true;
        protected Double TimeOffset = 0;
        private string DisqusSiteShortName = string.Empty;
        private bool showLeftContent = false;
        private bool showRightContent = false;
        

        #endregion


        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);

            basePage = Page as CBasePage;
            SiteRoot = basePage.SiteRoot;
            ImageSiteRoot = basePage.ImageSiteRoot;
        }
        #endregion

        private void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (
                (basePage == null)
                || (!basePage.UserCanViewPage())
            )
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            SetupCss();
            AddConnoicalUrl();
            GetModuleSettings();
            if (!this.NavigationOnRight)
            {
                this.divNav.CssClass = "blognavleft";
                this.divblog.CssClass = "blogcenter-leftnav";

            }
            PopulateLabels();

            if (!IsPostBack)
            {
                //lblCopyright.Text = moduleSettings["Copyright"].ToString();

                IDataReader reader;
                if ((Month > -1) && (Year > -1))
                {
                    //DateTime selectedMonth = DateTime.Now;
                    DateTime selectedMonth = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
                    try
                    {
                       // selectedMonth = new DateTime(year, month, 1);
                        selectedMonth = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
                    }
                    catch (Exception)
                    { }

                    this.litHeader.Text = Page.Server.HtmlEncode(BlogResources.BlogArchivesPrefixLabel
                        + selectedMonth.ToString("MMMM, yyyy"));

                    if (blogModule != null)
                    {
                        basePage.Title = SiteUtils.FormatPageTitle(basePage.SiteInfo, blogModule.ModuleTitle + " - " + BlogResources.BlogArchivesPrefixLabel
                        + selectedMonth.ToString("MMMM, yyyy"));

                        basePage.MetaDescription = string.Format(CultureInfo.InvariantCulture, 
                            BlogResources.ArchiveMetaDescriptionFormat, 
                            blogModule.ModuleTitle, 
                            selectedMonth.ToString("MMMM, yyyy"));

                        
                    }

                  
                    reader = Blog.GetBlogEntriesByMonth(Month, Year, ModuleId);
                }
                else
                {
                    reader = Blog.GetBlogs(ModuleId, DateTime.UtcNow);
                }
                try
                {
                    dlArchives.DataSource = reader;
                    dlArchives.DataBind();
                }
                finally
                {
                    reader.Close();
                }
                PopulateNavigation();

               
            }

            basePage.LoadSideContent(showLeftContent, showRightContent);

        }


        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (itemUrl.Length > 0)
                return SiteRoot + itemUrl.Replace("~", string.Empty);

            return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + PageId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

        }

        private void PopulateNavigation()
        {
            Feeds.ModuleSettings = moduleSettings;
            Feeds.PageId = PageId;
            Feeds.ModuleId = ModuleId;
            Feeds.Visible = ShowFeedLinks;

            if (this.ShowCategories)
            {
                //tags.CanEdit = IsEditable;
                tags.PageId = PageId;
                tags.ModuleId = ModuleId;
                tags.SiteRoot = SiteRoot;
                tags.RenderAsTagCloud = BlogUseTagCloudForCategoriesSetting;
            }
            else
            {
                tags.Visible = false;
                this.pnlCategories.Visible = false;
            }

            if (this.ShowArchives)
            {
                archive.PageId = PageId;
                archive.ModuleId = ModuleId;
                archive.SiteRoot = SiteRoot;

            }
            else
            {
                archive.Visible = false;
                this.pnlArchives.Visible = false;
            }

            stats.PageId = PageId;
            stats.ModuleId = ModuleId;
            stats.CountOfDrafts = countOfDrafts;
            stats.Visible = ShowStatistics;


        }

        private void GetModuleSettings()
        {
            moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);

            ShowCategories = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowCategoriesSetting", false);

            BlogUseTagCloudForCategoriesSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogUseTagCloudForCategoriesSetting", BlogUseTagCloudForCategoriesSetting);

            ShowArchives = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowArchiveSetting", false);

            NavigationOnRight = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogNavigationOnRightSetting", false);

            ShowStatistics = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowStatisticsSetting", true);

            ShowFeedLinks = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowFeedLinksSetting", true);

            ShowAddFeedLinks = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowAddFeedLinksSetting", true);

            AllowComments = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogAllowComments", true);

            showLeftContent = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "ShowPageLeftContentSetting", showLeftContent);

            showRightContent = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "ShowPageRightContentSetting", showRightContent);

            if (moduleSettings.Contains("DisqusSiteShortName"))
            {
                DisqusSiteShortName = moduleSettings["DisqusSiteShortName"].ToString();
            }

            if (DisqusSiteShortName.Length > 0) { stats.ShowCommentCount = false; }

            pnlStatistics.Visible = ShowStatistics;

            divNav.Visible = false;
            if (ShowArchives
                || ShowAddFeedLinks
                || ShowCategories
                || ShowFeedLinks
                || ShowStatistics)
            {
                divNav.Visible = true;
            }

            if (!divNav.Visible)
            {
                divblog.CssClass = "blogcenter-nonav";
            }

            countOfDrafts = Blog.CountOfDrafts(ModuleId);




        }


        private void PopulateLabels()
        {
            BlogDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

            FeedBackLabel = BlogResources.BlogFeedbackLabel;
            
        }


        private void SetupCss()
        {

            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("blogcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "blogcss";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl()
                    + "blogmodule.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }

        }

        private void AddConnoicalUrl()
        {
            if (Page.Header == null) { return; }

            Literal link = new Literal();
            link.ID = "blogarchiveurl";
            link.Text = "\n<link rel='canonical' href='"
                + SiteRoot
                + "/Blog/ViewArchive.aspx?month="
                + Month.ToInvariantString()
                + "&amp;year=" + Year.ToInvariantString()
                + "&amp;mid=" + ModuleId.ToInvariantString()
                + "&amp;pageid=" + PageId.ToInvariantString()
                + "' />";

            Page.Header.Controls.Add(link);

        }

        private void LoadParams()
        {
            TimeOffset = SiteUtils.GetUserTimeOffset();
            PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            ModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            ItemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            Month = WebUtils.ParseInt32FromQueryString("month", Month);
            Year = WebUtils.ParseInt32FromQueryString("year", Year);

            // don't let the archive be used to see unpublished future posts
            try
            {
                //This line commentted by Asad Samarian 2009-01-08
                //DateTime d = new DateTime(year, month, 1,ResourceHelper.GetDefaultCulture().Calendar);
                //This line added by Asad Samarian 2009-01-08
                DateTime d = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
             
                if (d > DateTime.UtcNow)
                {
                    Month = DateTime.UtcNow.Month;
                    Year = DateTime.UtcNow.Year;

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Month = DateTime.UtcNow.Month;
                Year = DateTime.UtcNow.Year;
            }

            blogModule = basePage.GetModule(ModuleId);

        }

    }
}
