/// Author:				Joe Audette
/// Created:			2005-04-02
/// Last Modified:		2007-11-12
/// 
/// Based on code sample by Joseph Hill
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
using Rss;
using Cynthia.Business;
using Cynthia.Web.Framework;

namespace Cynthia.Web.FeedUI
{
    public partial class FeedAggregatePage : System.Web.UI.Page
    {
        private int feedListCacheTimeout = 3660;
        private int entryCacheTimeout = 3620;
        private int maxDaysOld = 90;
        private int maxEntriesPerFeed = 90;
        protected bool EnableSelectivePublishing = false;

        private int moduleID = -1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // nothing should post here
            if (Page.IsPostBack) return;

            moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);

            if (moduleID > -1)
            {
                RenderRss(moduleID);

            }
            else
            {
                RenderError("Invalid Request");
            }

        }

        private void RenderRss(int moduleId)
        {
            Response.ContentType = "text/xml";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            //Cynthia.Business.RssFeed feed = new Cynthia.Business.RssFeed(moduleId);
            Module module = GetModule();
            Hashtable moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            feedListCacheTimeout = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedFeedListCacheTimeoutSetting", 3660);

            entryCacheTimeout = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedEntryCacheTimeoutSetting", 3620);

            maxDaysOld = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedMaxDayCountSetting", 90);

            maxEntriesPerFeed = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "RSSFeedMaxPostsPerFeedSetting", 90);

            EnableSelectivePublishing = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "EnableSelectivePublishing", EnableSelectivePublishing);


            DataView dv = FeedCache.GetRssFeedEntries(
                module.ModuleId,
                module.ModuleGuid, 
                entryCacheTimeout,
                maxDaysOld, 
                maxEntriesPerFeed, 
                EnableSelectivePublishing).DefaultView;

            dv.Sort = "PubDate DESC";

            //if (EnableSelectivePublishing)
            //{
            //    dv.RowFilter = "Confirmed = true";
            //}

            RssChannel channel = new RssChannel();
            object value = GetModule();
            Module m;

            if (value != null)
            {
                m = (Module)value;

                channel.Title = m.ModuleTitle;
                channel.Description = m.ModuleTitle;
                channel.LastBuildDate = channel.Items.LatestPubDate();
                channel.Link = new System.Uri(SiteUtils.GetCurrentPageUrl());

            }
            else
            {
                // this prevents an error: Can't close RssWriter without first writing a channel. 
                channel.Title = "Not Found";
                channel.Description = "Not Found";
                channel.LastBuildDate = DateTime.UtcNow;
                //channel.Link = new System.Uri(SiteUtils.GetCurrentPageUrl());

            }

            foreach (DataRowView row in dv)
            {
                bool confirmed = Convert.ToBoolean(row["Confirmed"]);
                if (!EnableSelectivePublishing)
                {
                    confirmed = true;
                }

                if (confirmed)
                {
                    RssItem item = new RssItem();


                    item.Title = row["Title"].ToString();
                    item.Description = row["Description"].ToString();
                    item.PubDate = Convert.ToDateTime(row["PubDate"]);
                    item.Link = new System.Uri(row["Link"].ToString());
                    Trace.Write(item.Link.ToString());
                    item.Author = row["Author"].ToString();

                    channel.Items.Add(item);
                }
            }

            


            Rss.RssFeed rss = new Rss.RssFeed();
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
            //SiteSettings siteSettings = (SiteSettings) HttpContext.Current.Items["SiteSettings"];
            //PageSettings currentPage = CacheHelper.GetCurrentPage();

            //foreach (Module module in currentPage.Modules)
            //{
            //    if (module.moduleID == moduleID)
            //        return module;
            //}
            //return null;

            Module module = new Module(moduleID);
            return module;
        }


        


        

    }
}
