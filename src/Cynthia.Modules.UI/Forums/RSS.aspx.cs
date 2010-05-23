/// Original Author:	Joseph Hill
/// Created:			2006-01-09
/// Last Modified:		2010-01-03
/// 
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
using System.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web.FeedUI
{
	public partial class RssGroupFeed : Page
	{
		private int maxDaysOld = 90;
        private int moduleId = -1;
        private int pageId = -1;
        private int itemId = -1;
        private int threadId = -1;
        private string navigationSiteRoot = string.Empty;
        private string imageSiteRoot = string.Empty;
        private string forumUrl = string.Empty;
        private string cssBaseUrl = string.Empty;

        private SiteSettings siteSettings;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            // nothing should post here
            if (Page.IsPostBack) return;

            siteSettings = CacheHelper.GetCurrentSiteSettings();
			RssGroup rssGroup = new RssGroup();
			rssGroup.ModuleId = -1;
			rssGroup.PageId = -1;
			rssGroup.ItemId = -1;
			rssGroup.ThreadId = -1;
			rssGroup.MaximumDays = maxDaysOld;

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            threadId = WebUtils.ParseInt32FromQueryString("thread", -1);

           
            if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                navigationSiteRoot = SiteUtils.GetNavigationSiteRoot();
                imageSiteRoot = WebUtils.GetSiteRoot();
                cssBaseUrl = imageSiteRoot;

            }
            else
            {
                navigationSiteRoot = WebUtils.GetHostRoot();
                imageSiteRoot = navigationSiteRoot;
                cssBaseUrl = WebUtils.GetSiteRoot();

            }

            forumUrl = SiteUtils.GetCurrentPageUrl();

            if(siteSettings != null)
            {
                rssGroup.SiteId = siteSettings.SiteId;
            }
                rssGroup.ModuleId = moduleId;
                rssGroup.PageId = pageId;
                rssGroup.ItemId = itemId;
                rssGroup.ThreadId = threadId;

                RenderRss(rssGroup);
			
		}

		private void RenderRss(RssGroup rssGroup)
		{

            Response.ContentType = "application/xml";
			Response.ContentEncoding = System.Text.Encoding.UTF8;
			
			Hashtable moduleSettings = ModuleSettings.GetModuleSettings(rssGroup.ModuleId);
            rssGroup.MaximumDays = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedMaxDaysOldSetting", 90);

            int entriesLimit = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedMaxPostsSetting", 90);

            int entryCount = 0;

            Rss.RssChannel channel = new Rss.RssChannel();
            string baseUrl = Request.Url.ToString().Replace("RSS.aspx", "Thread.aspx");

            using (IDataReader posts = rssGroup.GetPostsForRss())
            {
                while ((posts.Read())&&(entryCount <= entriesLimit))
                {
                    Rss.RssItem item = new Rss.RssItem();

                    item.Title = posts["Subject"].ToString();
                    item.Description = SiteUtils.ChangeRelativeUrlsToFullyQuailifiedUrls(navigationSiteRoot, imageSiteRoot, posts["Post"].ToString());
                    item.PubDate = Convert.ToDateTime(posts["PostDate"]);

                    string target = baseUrl;

                    if (target.IndexOf("&thread=") < 0 && target.IndexOf("?thread=") < 0)
                    {
                        if (target.IndexOf("?") < 0)
                        {
                            target += "?thread=" + posts["TopicID"].ToString() + "#post" + posts["PostID"].ToString();
                        }
                        else
                        {
                            target += "&thread=" + posts["TopicID"].ToString() + "#post" + posts["PostID"].ToString();
                        }
                    }
                    item.Link = new System.Uri(target);

                    item.Author = posts["StartedBy"].ToString();

                    channel.Items.Add(item);
                    entryCount += 1;
                }
            }

			object value = GetModule();
			Module m;

			channel.LastBuildDate = channel.Items.LatestPubDate();
            channel.Link = new System.Uri(forumUrl);

			if (value != null) 
			{
				m = (Module) value;

				channel.Title = m.ModuleTitle;
				channel.Description = m.ModuleTitle;

			} 
			else 
			{
				channel.Title = siteSettings.SiteName;
				channel.Description = siteSettings.SiteName;
			}

            if (channel.Items.Count == 0)
            {
                Rss.RssItem item = new Rss.RssItem();

                item.Title = "No Items Found";
                item.Description = "No items found";
                item.PubDate = DateTime.UtcNow;
                
                item.Link = new System.Uri(navigationSiteRoot);

                item.Author = "system";

                channel.Items.Add(item);


            }

			
			Rss.RssFeed rss = new Rss.RssFeed();
            rss.BaseUrl = cssBaseUrl;
            
			rss.Encoding = System.Text.Encoding.UTF8;
            rss.Channels.Add(channel);
            rss.Write(Response.OutputStream);
			//Response.End();

		}

        


		private void RenderError(string message)
		{

			Response.Write(message);
			//Response.End();
		}

		private Module GetModule() 
		{
			//SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (currentPage != null)
            {
                foreach (Module module in currentPage.Modules)
                {
                    if (module.ModuleId == moduleId)
                        return module;
                }
            }
			return null;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
