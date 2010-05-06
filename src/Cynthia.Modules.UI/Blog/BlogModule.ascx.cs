

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Cynthia.Web.Controls;
using Cynthia.Web.Controls.google;
using Resources;
using Calendar = System.Web.UI.WebControls.Calendar;

namespace Cynthia.Web.BlogUI
{
	public partial class BlogModule : SiteModuleControl
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(BlogModule));
        
        private int countOfDrafts = 0;
        private int pageNumber = 1;
        private int totalPages = 1;
        private int pageSize = 10;
        private bool showPager = true;
        protected string addThisAccountId = string.Empty;
        protected bool useAddThisMouseOverWidget = true;
        protected string addThisCustomBrand = string.Empty;
        protected string addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";
        protected string addThisCustomOptions = string.Empty;
        protected string addThisCustomLogoUrl = string.Empty;
        protected string addThisCustomLogoBackColor = string.Empty;
        protected string addThisCustomLogoForeColor = string.Empty;
        protected string feedburnerFeedUrl = string.Empty;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string EditBlogAltText = "Edit";
        protected string BlogDateTimeFormat;
        protected string FeedBackLabel;
        protected bool ShowCalendar = false;
        protected DateTime CalendarDate;
        protected bool ShowCategories = false;
        protected bool ShowArchives = false;
        protected bool AllowComments = true;
        protected bool NavigationOnRight = false;
        protected bool ShowStatistics = true;
        protected bool ShowFeedLinks = true;
        protected bool ShowAddFeedLinks = true;
        protected bool BlogUseLinkForHeading = true;
        protected Double TimeOffset = 0; 
        protected string GmapApiKey = string.Empty;
        protected int GoogleMapHeightSetting = 300;
        protected int GoogleMapWidthSetting = 500;
        protected bool GoogleMapEnableMapTypeSetting = false;
        protected bool GoogleMapEnableZoomSetting = false;
        protected bool GoogleMapShowInfoWindowSetting = false;
        protected bool GoogleMapEnableLocalSearchSetting = false;
        protected bool GoogleMapEnableDirectionsSetting = false;
        protected int GoogleMapInitialZoomSetting = 13;
        protected MapType mapType = MapType.G_SATELLITE_MAP;
        protected string OdiogoFeedIDSetting = string.Empty;
        protected bool UseExcerpt = false;
        protected bool TitleOnly = false;
        protected bool HideAddThisButton = false;
        protected int ExcerptLength = 250;
        protected string ExcerptSuffix = "...";
        protected string MoreLinkText = "read more";
        protected bool EnableContentRatingSetting = false;
        protected bool EnableRatingCommentsSetting = false;
        protected bool ShowPostAuthorSetting = false;
        protected bool GoogleMapIncludeWithExcerptSetting = false;
        protected bool ShowGoogleMap = true;
        protected bool BlogUseTagCloudForCategoriesSetting = false;
        protected string blogAuthor = string.Empty;
        private string CommentSystem = "internal";
        private string DisqusSiteShortName = string.Empty;
        protected string disqusFlag = string.Empty;
        protected string IntenseDebateAccountId = string.Empty;
        protected bool ShowCommentCounts = true;
        protected string EditLinkText = BlogResources.BlogEditEntryLink;
        protected string EditLinkTooltip = BlogResources.BlogEditEntryLink;
        protected string EditLinkImageUrl = string.Empty;
       
        
        
        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.calBlogNav.SelectionChanged += new EventHandler(calBlogNav_SelectionChanged);
            this.calBlogNav.VisibleMonthChanged += new MonthChangedEventHandler(CalBlogNavVisibleMonthChanged);
            pgr.Command += new CommandEventHandler(pgr_Command);
            this.EnableViewState = false;
            
        }

        

        

		protected virtual void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();

            SetupCss();
            if (!RenderInWebPartMode)
            {
                SetupRssLink();
            }
            
            PopulateLabels();
            if (!Page.IsPostBack)
            {
                PopulateControls();
            }

		}

        private void PopulateControls()
        {
            BindBlogs();
            PopulateNavigation();

        }

        private void BindBlogs()
        {
            using (IDataReader reader = Blog.GetPage(ModuleId, CalendarDate.Date.AddDays(1), pageNumber, pageSize, out totalPages))
            {
                rptBlogs.DataSource = reader;
                rptBlogs.DataBind();

                pgr.ShowFirstLast = true;
                pgr.PageSize = pageSize;
                pgr.PageCount = totalPages;
                pgr.Visible = (totalPages > 1) && showPager;
            }

        }

        void pgr_Command(object sender, CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgr.CurrentIndex = pageNumber;
            PopulateControls();
            updBlog.Update();
        }

	    protected virtual void PopulateNavigation()
		{

            Feeds.ModuleSettings = Settings;
            Feeds.PageId = PageId;
            Feeds.ModuleId = ModuleId;
            Feeds.Visible = ShowFeedLinks;
            
			if(this.ShowCategories)
			{
                tags.CanEdit = IsEditable;
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

			if(this.ShowArchives)
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

		private void calBlogNav_SelectionChanged(object sender, EventArgs e)
		{
            System.Web.UI.WebControls.Calendar cal = (System.Web.UI.WebControls.Calendar)sender;

            CalendarDate = cal.SelectedDate;
            calBlogNav.VisibleDate = CalendarDate;
            calBlogNav.SelectedDate = CalendarDate;
            PopulateControls();

		}

		private void CalBlogNavVisibleMonthChanged(object sender, MonthChangedEventArgs e)
		{
            CalendarDate = e.NewDate;
            calBlogNav.VisibleDate = CalendarDate;
            calBlogNav.SelectedDate = CalendarDate;
            PopulateControls();

		}

        


        protected virtual void PopulateLabels()
        {
            Title1.EditUrl = SiteRoot + "/Blog/EditPost.aspx";
            Title1.EditText = BlogResources.BlogAddPostLabel;

            if ((IsEditable) && (countOfDrafts > 0))
            {
                //BlogEditCategoriesLabel
                Title1.LiteralExtraMarkup = 
                    "&nbsp;<a href='"
                    + SiteRoot
                    + "/Blog/EditCategory.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;mid=" + ModuleId.ToInvariantString()
                    + "' class='ModuleEditLink' title='" + BlogResources.BlogEditCategoriesLabel + "'>" + BlogResources.BlogEditCategoriesLabel + "</a>"
                    + "&nbsp;<a href='"
                    + SiteRoot
                    + "/Blog/Drafts.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;mid=" + ModuleId.ToInvariantString()
                    + "' class='ModuleEditLink' title='" + BlogResources.BlogDraftsLink + "'>" + BlogResources.BlogDraftsLink + "</a>";
            }
            else if (IsEditable)
            {
                Title1.LiteralExtraMarkup =
                    "&nbsp;<a href='"
                    + SiteRoot
                    + "/Blog/EditCategory.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;mid=" + ModuleId.ToInvariantString()
                    + "' class='ModuleEditLink' title='" + BlogResources.BlogEditCategoriesLabel + "'>" + BlogResources.BlogEditCategoriesLabel + "</a>";
            }

            

            calBlogNav.UseAccessibleHeader = true;

            EditBlogAltText = BlogResources.EditImageAltText;
            FeedBackLabel = BlogResources.BlogFeedbackLabel;

            CBasePage basePage = Page as CBasePage;
            if (basePage != null)
            {
                if (!basePage.UseTextLinksForFeatureSettings)
                {
					EditLinkImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, EditContentImage);
                }
                
            }
           
        }

        protected string FormatPostAuthor(string authorName)
        {
            if (ShowPostAuthorSetting)
            {
                if (blogAuthor.Length > 0)
                {
                    return string.Format(CultureInfo.InvariantCulture,
                    BlogResources.PostAuthorFormat, blogAuthor);
                }

                return string.Format(CultureInfo.InvariantCulture,
                    BlogResources.PostAuthorFormat, authorName);
            }

            return string.Empty;

        }

        protected string FormatBlogEntry(string blogHtml, string excerpt, string url, int itemId)
        {
            if (UseExcerpt)
            {
                if ((excerpt.Length > 0)&&(excerpt != "<p>&#160;</p>"))
                {
					return String.Format("{0}{1} <a href='{2}'>{3}</a><div>&nbsp;</div>", excerpt, ExcerptSuffix, FormatBlogUrl(url, itemId), MoreLinkText);
                }
                
                string result = string.Empty;
                if ((blogHtml.Length > ExcerptLength)&&(MoreLinkText.Length > 0))
                {

                    result = UIHelper.CreateExcerpt(blogHtml, ExcerptLength, ExcerptSuffix);
					result += String.Format(" <a href='{0}'>{1}</a><div>&nbsp;</div>", FormatBlogTitleUrl(url, itemId), MoreLinkText);
                    return result;
                }
                
            }

            return blogHtml;
        }

        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (itemUrl.Length > 0)
                return SiteRoot + itemUrl.Replace("~", string.Empty) + disqusFlag;

			return String.Format("{0}/Blog/ViewPost.aspx?pageid={1}&ItemID={2}&mid={3}{4}", SiteRoot, PageId.ToInvariantString(), itemId.ToInvariantString(), ModuleId.ToInvariantString(), disqusFlag);

        }

        protected string FormatBlogTitleUrl(string itemUrl, int itemId)
        {
            if (itemUrl.Length > 0)
                return SiteRoot + itemUrl.Replace("~", string.Empty);

			return String.Format("{0}/Blog/ViewPost.aspx?pageid={1}&ItemID={2}&mid={3}", SiteRoot, PageId.ToInvariantString(), itemId.ToInvariantString(), ModuleId.ToInvariantString());

        }

        private string GetRssUrl()
        {
            if (feedburnerFeedUrl.Length > 0) return feedburnerFeedUrl;

			return String.Format("{0}/blog{1}rss.aspx", SiteRoot, ModuleId.ToInvariantString());

        }

        protected virtual void SetupRssLink()
        {
            if (this.ModuleConfiguration != null)
            {
                if (Page.Master != null)
                {
                    Control head = Page.Master.FindControl("Head1");
                    if (head != null)
                    {
                        
                        Literal rssLink = new Literal();
						rssLink.Text = String.Format("<link rel=\"alternate\" type=\"application/rss+xml\" title=\"{0}\" href=\"{1}\" />", this.ModuleConfiguration.ModuleTitle, GetRssUrl());

                        head.Controls.Add(rssLink);

                    }

                }
            }

        }

        protected virtual void SetupCss()
        {
            
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("blogcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "blogcss";
					cssLink.Text = String.Format("\n<link href='{0}blogmodule.css' type='text/css' rel='stylesheet' media='screen' />", SiteUtils.GetSkinBaseUrl());

                    stylesheet.Controls.Add(cssLink);
                }

                if (stylesheet.FindControl("aspcalendar") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "aspcalendar";
					cssLink.Text = String.Format("\n<link href='{0}aspcalendar.css' type='text/css' rel='stylesheet' media='screen' />", SiteUtils.GetSkinBaseUrl());

                    stylesheet.Controls.Add(cssLink);
                }

            }
           
        }

       
        protected virtual void LoadSettings()
        {
            TimeOffset = SiteUtils.GetUserTimeOffset();
            GmapApiKey = SiteUtils.GetGmapApiKey();
            addThisAccountId = siteSettings.AddThisDotComUsername;

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            pnlContainer.ModuleId = this.ModuleId;

            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (Page.Request.Params.Get("blogdate") != null)
            {

                //calendarDate = DateTime.Parse(Page.Request.Params.Get("blogdate"));
                DateTimeFormatInfo dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
                if (!DateTime.TryParse(Page.Request.Params.Get("blogdate"), dtfi, DateTimeStyles.AdjustToUniversal, out CalendarDate))
                {
                    CalendarDate = DateTime.UtcNow.Date;
                }

            }
            else
            {
                CalendarDate = DateTime.UtcNow.Date;
            }

            if (CalendarDate > DateTime.UtcNow.Date)
            {
                CalendarDate = DateTime.UtcNow.Date;
            }


            UseExcerpt = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogUseExcerptSetting", UseExcerpt);

            TitleOnly = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowTitleOnlySetting", TitleOnly);

            showPager = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowPagerInListSetting", showPager);
            

            GoogleMapIncludeWithExcerptSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapIncludeWithExcerptSetting", GoogleMapIncludeWithExcerptSetting);

            if ((UseExcerpt) && (!GoogleMapIncludeWithExcerptSetting)) { ShowGoogleMap = false; }


            EnableContentRatingSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableContentRatingSetting", EnableContentRatingSetting);

            EnableRatingCommentsSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableRatingCommentsSetting", EnableRatingCommentsSetting);

            if (UseExcerpt) { EnableContentRatingSetting = false; }

            HideAddThisButton = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogHideAddThisButtonSetting", HideAddThisButton);

            ExcerptLength = WebUtils.ParseInt32FromHashtable(
                Settings, "BlogExcerptLengthSetting", ExcerptLength);

            if (Settings.Contains("BlogExcerptSuffixSetting"))
            {
                ExcerptSuffix = Settings["BlogExcerptSuffixSetting"].ToString();
            }

            if (Settings.Contains("BlogMoreLinkText"))
            {
                MoreLinkText = Settings["BlogMoreLinkText"].ToString();
            }

            if (Settings.Contains("BlogAuthorSetting"))
            {
                blogAuthor = Settings["BlogAuthorSetting"].ToString();
            }

            BlogDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            if (Settings.Contains("BlogDateTimeFormat"))
            {
                BlogDateTimeFormat = Settings["BlogDateTimeFormat"].ToString().Trim();
                if (BlogDateTimeFormat.Length > 0)
                {
                    try
                    {
                        string d = DateTime.Now.ToString(BlogDateTimeFormat, CultureInfo.CurrentCulture);
                    }
                    catch (FormatException)
                    {
                        BlogDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
                    }
                }
                else
                {
                    BlogDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
                }
            }

            

            ShowCalendar = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowCalendarSetting", false);

            ShowCategories = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowCategoriesSetting", false);

            BlogUseTagCloudForCategoriesSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogUseTagCloudForCategoriesSetting", false);

            ShowArchives = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowArchiveSetting", false);

            NavigationOnRight = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogNavigationOnRightSetting", false);

            ShowStatistics = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowStatisticsSetting", true);

            ShowFeedLinks = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowFeedLinksSetting", true);

            ShowAddFeedLinks = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogShowAddFeedLinksSetting", true);

            AllowComments = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogAllowComments", true);

            BlogUseLinkForHeading = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogUseLinkForHeading", true);

            ShowPostAuthorSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowPostAuthorSetting", ShowPostAuthorSetting);

            if (Settings.Contains("GoogleMapInitialMapTypeSetting"))
            {
                string gmType = Settings["GoogleMapInitialMapTypeSetting"].ToString();
                try
                {
                    mapType = (MapType)Enum.Parse(typeof(MapType), gmType);
                }
                catch (ArgumentException) { }

            }

            GoogleMapHeightSetting = WebUtils.ParseInt32FromHashtable(
                Settings, "GoogleMapHeightSetting", GoogleMapHeightSetting);

            GoogleMapWidthSetting = WebUtils.ParseInt32FromHashtable(
                Settings, "GoogleMapWidthSetting", GoogleMapWidthSetting);

           
            GoogleMapEnableMapTypeSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableMapTypeSetting", false);

            GoogleMapEnableZoomSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableZoomSetting", false);

            GoogleMapShowInfoWindowSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapShowInfoWindowSetting", false);

            GoogleMapEnableLocalSearchSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableLocalSearchSetting", false);

            GoogleMapEnableDirectionsSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableDirectionsSetting", false);

            GoogleMapInitialZoomSetting = WebUtils.ParseInt32FromHashtable(
                Settings, "GoogleMapInitialZoomSetting", GoogleMapInitialZoomSetting);

            pageSize = WebUtils.ParseInt32FromHashtable(
                Settings, "BlogEntriesToShowSetting", pageSize);


            if (Settings.Contains("OdiogoFeedIDSetting"))
                OdiogoFeedIDSetting = Settings["OdiogoFeedIDSetting"].ToString();

            string altAddThisAccount = string.Empty;

            if (Settings.Contains("BlogAddThisDotComUsernameSetting"))
                altAddThisAccount = Settings["BlogAddThisDotComUsernameSetting"].ToString().Trim();

            if (altAddThisAccount.Length > 0)
                addThisAccountId = altAddThisAccount;

            useAddThisMouseOverWidget = WebUtils.ParseBoolFromHashtable(
                Settings, "BlogAddThisDotComUseMouseOverWidgetSetting", useAddThisMouseOverWidget);


            if (Settings.Contains("BlogAddThisButtonImageUrlSetting"))
                addThisButtonImageUrl = Settings["BlogAddThisButtonImageUrlSetting"].ToString().Trim();

            if (addThisButtonImageUrl.Length == 0)
                addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";

            if (Settings.Contains("BlogAddThisCustomBrandSetting"))
                addThisCustomBrand = Settings["BlogAddThisCustomBrandSetting"].ToString().Trim();

            if (addThisCustomBrand.Length == 0)
                addThisCustomBrand = siteSettings.SiteName;

            if (Settings.Contains("BlogAddThisCustomOptionsSetting"))
                addThisCustomOptions = Settings["BlogAddThisCustomOptionsSetting"].ToString().Trim();

            if (Settings.Contains("BlogAddThisCustomLogoUrlSetting"))
                addThisCustomLogoUrl = Settings["BlogAddThisCustomLogoUrlSetting"].ToString().Trim();

            if (Settings.Contains("BlogAddThisCustomLogoBackColorSetting"))
                addThisCustomLogoBackColor = Settings["BlogAddThisCustomLogoBackColorSetting"].ToString().Trim();

            if (Settings.Contains("BlogAddThisCustomLogoForeColorSetting"))
                addThisCustomLogoForeColor = Settings["BlogAddThisCustomLogoForeColorSetting"].ToString().Trim();

            if (Settings.Contains("BlogFeedburnerFeedUrl"))
                feedburnerFeedUrl = Settings["BlogFeedburnerFeedUrl"].ToString().Trim();

            if (Settings.Contains("DisqusSiteShortName"))
            {
                DisqusSiteShortName = Settings["DisqusSiteShortName"].ToString();
            }

            if (Settings.Contains("CommentSystemSetting"))
            {
                CommentSystem = Settings["CommentSystemSetting"].ToString();
            }

            if (Settings.Contains("IntenseDebateAccountId"))
            {
                IntenseDebateAccountId = Settings["IntenseDebateAccountId"].ToString();
            }

            if (AllowComments)
            {
                if ((DisqusSiteShortName.Length > 0) && (CommentSystem == "disqus"))
                {
                    disqusFlag = "#disqus_thread";
                    disqus.SiteShortName = DisqusSiteShortName;
                    disqus.RenderCommentCountScript = true;
                    stats.ShowCommentCount = false;

                }

                if ((IntenseDebateAccountId.Length > 0) && (CommentSystem == "intensedebate"))
                {
                    ShowCommentCounts = false;
                    stats.ShowCommentCount = false;
                }
            }

            
            if (!this.NavigationOnRight)
            {
                this.divNav.CssClass = "blognavleft";
                this.divblog.CssClass = "blogcenter-leftnav";

            }
            if (ShowCalendar)
            {
                this.calBlogNav.Visible = true;
                try
                {
                    calBlogNav.FirstDayOfWeek = (FirstDayOfWeek)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                }
                catch (ArgumentNullException) { }
                catch (ArgumentOutOfRangeException) { }
                catch (InvalidOperationException) { }
                catch (InvalidCastException) { }

                if (!Page.IsPostBack)
                {
                    this.calBlogNav.SelectedDate = CalendarDate;
                    this.calBlogNav.VisibleDate = CalendarDate;
                    
                }
            }
            else
            {
                this.calBlogNav.Visible = false;
            }

            if (Settings.Contains("BlogCopyrightSetting"))
            {
                lblCopyright.Text = Settings["BlogCopyrightSetting"].ToString();
            }

            pnlStatistics.Visible = ShowStatistics;
           
            divNav.Visible = false;

            if (ShowCalendar 
                || ShowArchives 
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
            if (IsEditable)
            {
                countOfDrafts = Blog.CountOfDrafts(ModuleId);
            }


        }
	
		

	}
}
