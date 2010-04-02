/// Author:				    Joe Audette
/// Created:			    2005-06-05
/// Last Modified:		    2009-12-18
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
using Cynthia.Business;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.BlogUI
{
	
    public partial class BlogCategoryView : CBasePage
    {
        #region Private Properties

        private string blogDateTimeFormat;
        private int countOfDrafts = 0;
        private string feedBackLabel;
        private int pageId = 0;
        private int moduleId = 0;
        private int categoryId = 0;
        private string category = string.Empty;
        private Hashtable moduleSettings;
        private bool showCategories = false;
        private bool BlogUseTagCloudForCategoriesSetting = false;
        private bool showArchives = false;
        private bool navigationOnRight = false;
        private bool showStatistics = true;
        private bool showFeedLinks = true;
        private bool showAddFeedLinks = true;
        private bool allowComments = true;
        private Double timeOffset = 0;
        private Module blogModule = null;
        private string DisqusSiteShortName = string.Empty;

        private bool showLeftContent = false;
        private bool showRightContent = false;

        #endregion

        #region Protected Properties

        protected string BlogDateTimeFormat
        {
            get{return blogDateTimeFormat;}  
        }

        protected string FeedBackLabel
        {
            get{return feedBackLabel;} 
        }
         
        protected int PageId
        {
            get{return pageId;} 
        }

        protected int ModuleId
        {
            get{return moduleId;} 
        }

        protected int CategoryId
        {
            get{return categoryId;}
        }

        protected string Category
        {
            get{return category;} 
        }

        protected bool ShowCategories
        {
            get{return showCategories;} 
        }

        protected bool ShowArchives
        {
            get{return showArchives;}
        }

        protected bool NavigationOnRight
        {
            get{return navigationOnRight;}
        }

        protected bool ShowStatistics
        {
            get{return showStatistics;}
        }

        protected bool ShowFeedLinks
        {
            get{return showFeedLinks;} 
        }

        protected bool ShowAddFeedLinks
        {
            get{return showAddFeedLinks;}  
        }

        protected bool AllowComments
        {
            get{return allowComments;}  
        }

        protected Double TimeOffset
        {
            get{return timeOffset;}
        }

        //protected string OdiogoFeedIDSetting = string.Empty;
        //protected string OdiogoPodcastUrlSetting = string.Empty;

        #endregion

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            LoadParams();

            if (!UserCanViewPage())
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            SetupCss();
            AddConnoicalUrl();
            PopulateLabels();
			GetModuleSettings();

            if (!this.NavigationOnRight)
            {
                this.divNav.CssClass = "blognavleft";
                this.divblog.CssClass = "blogcenter-leftnav";

            }

			if(!IsPostBack)
			{
				if((moduleId > 0)&&(categoryId > 0))
				{
                    using (IDataReader reader = Blog.GetCategory(categoryId))
                    {
                        if (reader.Read())
                        {
                            this.category = reader["Category"].ToString();
                        }
                    }

                    this.litHeader.Text = Page.Server.HtmlEncode(BlogResources.BlogCategoriesPrefixLabel + this.Category);

                    if (blogModule != null)
                    {
                        Title = SiteUtils.FormatPageTitle(siteSettings, 
                            blogModule.ModuleTitle + " - " + BlogResources.BlogCategoriesPrefixLabel + this.Category);

                        MetaDescription = string.Format(CultureInfo.InvariantCulture,
                            BlogResources.CategoryMetaDescriptionFormat,
                            blogModule.ModuleTitle, category);
                    }

                    using (IDataReader reader = Blog.GetEntriesByCategory(moduleId, categoryId))
                    {
                        dlArchives.DataSource = reader;
                        dlArchives.DataBind();
                    }

                    PopulateNavigation();

				}
			}

            LoadSideContent(showLeftContent, showRightContent);

		}

        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (itemUrl.Length > 0)
                return SiteRoot + itemUrl.Replace("~", string.Empty);

            return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + PageId.ToString(CultureInfo.InvariantCulture)
                + "&ItemID=" + itemId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture);

        }


        private void PopulateLabels()
        {
            
            blogDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

            feedBackLabel = BlogResources.BlogFeedbackLabel;
            
        }


		private void PopulateNavigation()
		{
            Feeds.ModuleSettings = moduleSettings;
            Feeds.PageId = PageId;
            Feeds.ModuleId = ModuleId;
            Feeds.Visible = ShowFeedLinks;

			if(this.ShowCategories)
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

		private void GetModuleSettings()
		{
			moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            showCategories = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowCategoriesSetting", false);

            BlogUseTagCloudForCategoriesSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogUseTagCloudForCategoriesSetting", BlogUseTagCloudForCategoriesSetting);

            showArchives = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowArchiveSetting", false);

            navigationOnRight = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogNavigationOnRightSetting", false);

            showStatistics = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowStatisticsSetting", true);

            showFeedLinks = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowFeedLinksSetting", true);

            showAddFeedLinks = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogShowAddFeedLinksSetting", true);

            allowComments = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogAllowComments", true);

            if (moduleSettings.Contains("DisqusSiteShortName"))
            {
                DisqusSiteShortName = moduleSettings["DisqusSiteShortName"].ToString();
            }

            if (DisqusSiteShortName.Length > 0) { stats.ShowCommentCount = false; }

            showLeftContent = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "ShowPageLeftContentSetting", showLeftContent);

            showRightContent = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "ShowPageRightContentSetting", showRightContent);
           
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
            link.ID = "blogcaturl";
            link.Text = "\n<link rel='canonical' href='"
                + SiteRoot
                + "/Blog/ViewCategory.aspx?cat="
                + categoryId.ToInvariantString()
                + "&amp;mid=" + ModuleId.ToInvariantString()
                + "&amp;pageid=" + PageId.ToInvariantString()
                + "' />";

            Page.Header.Controls.Add(link);

        }

        private void LoadParams()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            categoryId = WebUtils.ParseInt32FromQueryString("cat", -1);
            pnlContainer.ModuleId = moduleId;
            blogModule = GetModule(moduleId);
        }

	}
}
