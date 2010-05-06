

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
			imgRSS.Src = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, RssImageFile);


			lnkAddThisRss.HRef = String.Format("http://www.addthis.com/feed.php?pub={0}&amp;h1={1}&amp;t1=", addThisAccountId, Server.UrlEncode(GetRssUrl()));

            imgAddThisRss.Src = addThisRssButtonImageUrl;

			lnkAddMSN.HRef = String.Format("http://my.msn.com/addtomymsn.armx?id=rss&amp;ut={0}", GetRssUrl());

			imgMSNRSS.Src = String.Format("{0}/Data/SiteImages/rss_mymsn.gif", ImageSiteRoot);

			lnkAddToLive.HRef = String.Format("http://www.live.com/?add={0}", Server.UrlEncode(GetRssUrl()));

			imgAddToLive.Src = String.Format("{0}/Data/SiteImages/addtolive.gif", ImageSiteRoot);

			lnkAddYahoo.HRef = String.Format("http://e.my.yahoo.com/config/promo_content?.module=ycontent&amp;.url={0}", GetRssUrl());

			imgYahooRSS.Src = String.Format("{0}/Data/SiteImages/addtomyyahoo2.gif", ImageSiteRoot);

			lnkAddGoogle.HRef = String.Format("http://fusion.google.com/add?feedurl={0}", GetRssUrl());

			imgGoogleRSS.Src = String.Format("{0}/Data/SiteImages/googleaddrss.gif", ImageSiteRoot);

            liOdiogoPodcast.Visible = (OdiogoPodcastUrlSetting.Length > 0);
            lnkOdiogoPodcast.HRef = OdiogoPodcastUrlSetting;
            lnkOdiogoPodcastTextLink.NavigateUrl = OdiogoPodcastUrlSetting;
			imgOdiogoPodcast.Src = String.Format("{0}/Data/SiteImages/podcast.png", ImageSiteRoot);

            


        }

        private string GetRssUrl()
        {
            if (feedburnerFeedUrl.Length > 0) return feedburnerFeedUrl;

			return String.Format("{0}/blog{1}rss.aspx", SiteRoot, ModuleId.ToString(CultureInfo.InvariantCulture));

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