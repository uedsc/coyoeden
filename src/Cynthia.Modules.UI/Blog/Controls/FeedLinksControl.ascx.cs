// Author:				        Joe Audette
// Created:			            2009-05-04
//	Last Modified:              2010-01-05
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
using System.Globalization;
using System.Data;
using System.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.BlogUI
{
    public partial class FeedLinksControl : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string siteRoot = string.Empty;
        private Hashtable moduleSettings = null;
        private string imageSiteRoot = string.Empty;
        private SiteSettings siteSettings = null;

        protected string addThisAccountId = string.Empty;
        protected string feedburnerFeedUrl = string.Empty;
        protected string OdiogoFeedIDSetting = string.Empty;
        protected string OdiogoPodcastUrlSetting = string.Empty;
        //protected string addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";
        protected string addThisRssButtonImageUrl = "~/Data/SiteImages/addthisrss.gif";
        protected string RssImageFile = WebConfigSettings.RSSImageFileName;

        protected bool ShowFeedLinks = true;
        protected bool ShowAddFeedLinks = true;

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        public Hashtable ModuleSettings
        {
            get { return moduleSettings; }
            set { moduleSettings = value; }
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.Visible) { return; }
            //if (pageId == -1) { return; }
            //if (moduleId == -1) { return; }
            //if (moduleSettings == null) { return; }

            //LoadSettings();
            //PopulateLabels();
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Visible)
            {
                if (pageId == -1) { return; }
                if (moduleId == -1) { return; }
                if (moduleSettings == null) { return; }

                LoadSettings();
                PopulateLabels();
                SetupLinks();
            }


            base.OnPreRender(e);

        }

        private void SetupLinks()
        {
            if (siteSettings == null) { return; }

            lnkRSS.HRef = GetRssUrl();
            imgRSS.Src = ImageSiteRoot + "/Data/SiteImages/" + RssImageFile;


            lnkAddThisRss.HRef = "http://www.addthis.com/feed.php?pub="
                + addThisAccountId + "&amp;h1=" + Server.UrlEncode(GetRssUrl())
                + "&amp;t1=";

            imgAddThisRss.Src = addThisRssButtonImageUrl;

            lnkAddMSN.HRef = "http://my.msn.com/addtomymsn.armx?id=rss&amp;ut=" + GetRssUrl();

            imgMSNRSS.Src = ImageSiteRoot + "/Data/SiteImages/rss_mymsn.gif";

            lnkAddToLive.HRef = "http://www.live.com/?add=" + Server.UrlEncode(GetRssUrl());

            imgAddToLive.Src = ImageSiteRoot + "/Data/SiteImages/addtolive.gif";

            lnkAddYahoo.HRef = "http://e.my.yahoo.com/config/promo_content?.module=ycontent&amp;.url="
                + GetRssUrl();

            imgYahooRSS.Src = ImageSiteRoot + "/Data/SiteImages/addtomyyahoo2.gif";

            lnkAddGoogle.HRef = "http://fusion.google.com/add?feedurl="
                + GetRssUrl();

            imgGoogleRSS.Src = ImageSiteRoot + "/Data/SiteImages/googleaddrss.gif";

            liOdiogoPodcast.Visible = (OdiogoPodcastUrlSetting.Length > 0);
            lnkOdiogoPodcast.HRef = OdiogoPodcastUrlSetting;
            lnkOdiogoPodcastTextLink.NavigateUrl = OdiogoPodcastUrlSetting;
            imgOdiogoPodcast.Src = ImageSiteRoot + "/Data/SiteImages/podcast.png";

            


        }

        private string GetRssUrl()
        {
            if (feedburnerFeedUrl.Length > 0) return feedburnerFeedUrl;

            return SiteRoot + "/blog" + ModuleId.ToString(CultureInfo.InvariantCulture) + "rss.aspx";

        }

        private void PopulateLabels()
        {
            lnkRSS.Title = BlogResources.BlogRSSLinkTitle;
            lnkAddThisRss.Title = BlogResources.BlogAddThisSubscribeAltText;
            lnkAddMSN.Title = BlogResources.BlogModuleAddToMyMSNLink;
            lnkAddYahoo.Title = BlogResources.BlogModuleAddToMyYahooLink;
            lnkAddGoogle.Title = BlogResources.BlogModuleAddToGoogleLink;
            lnkAddToLive.Title = BlogResources.BlogModuleAddToWindowsLiveLink;
            lnkOdiogoPodcast.Title = BlogResources.PodcastLink;
            lnkOdiogoPodcastTextLink.Text = BlogResources.PodcastLink;

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            siteRoot = siteSettings.SiteRoot;
            addThisAccountId = siteSettings.AddThisDotComUsername;

            string altAddThisAccount = string.Empty;

            if (moduleSettings.Contains("BlogAddThisDotComUsernameSetting"))
                altAddThisAccount = moduleSettings["BlogAddThisDotComUsernameSetting"].ToString().Trim();

            if (altAddThisAccount.Length > 0)
                addThisAccountId = altAddThisAccount;

            if (moduleSettings.Contains("OdiogoFeedIDSetting"))
                OdiogoFeedIDSetting = moduleSettings["OdiogoFeedIDSetting"].ToString();


            if (moduleSettings.Contains("OdiogoPodcastUrlSetting"))
                OdiogoPodcastUrlSetting = moduleSettings["OdiogoPodcastUrlSetting"].ToString();

            if (moduleSettings.Contains("BlogFeedburnerFeedUrl"))
                feedburnerFeedUrl = moduleSettings["BlogFeedburnerFeedUrl"].ToString().Trim();

            if (moduleSettings.Contains("BlogAddThisRssButtonImageUrlSetting"))
                addThisRssButtonImageUrl = moduleSettings["BlogAddThisRssButtonImageUrlSetting"].ToString().Trim();

            ShowFeedLinks = WebUtils.ParseBoolFromHashtable(moduleSettings, "BlogShowFeedLinksSetting", ShowFeedLinks);

            ShowAddFeedLinks = WebUtils.ParseBoolFromHashtable(moduleSettings, "BlogShowAddFeedLinksSetting", ShowAddFeedLinks);

            liAddThisRss.Visible = (addThisAccountId.Length > 0);

            liAddThisRss.Visible = (ShowAddFeedLinks && (addThisAccountId.Length > 0));
            liAddGoogle.Visible = ShowAddFeedLinks;
            liAddMSN.Visible = ShowAddFeedLinks;
            liAddYahoo.Visible = ShowAddFeedLinks;
            liAddToLive.Visible = ShowAddFeedLinks;

            if (liAddThisRss.Visible)
            {
                liAddGoogle.Visible = false;
                liAddMSN.Visible = false;
                liAddYahoo.Visible = false;
                liAddToLive.Visible = false;

            }

            //if (moduleSettings.Contains("BlogAddThisButtonImageUrlSetting"))
            //    addThisButtonImageUrl = moduleSettings["BlogAddThisButtonImageUrlSetting"].ToString().Trim();

            //if (addThisButtonImageUrl.Length == 0)
            //    addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";

            if (imageSiteRoot.Length == 0) { imageSiteRoot = WebUtils.GetSiteRoot(); }

           

        }


    }
}