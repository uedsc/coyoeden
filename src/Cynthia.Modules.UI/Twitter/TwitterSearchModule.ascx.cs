// Author:					Joe Audette
// Created:					2009-09-18
// Last Modified:			2009-09-18
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
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using log4net;
using Resources;

namespace Cynthia.Modules.UI
{

    public partial class TwitterSearchModule : SiteModuleControl
    {
        // FeatureGuid 58F5CE04-C0A2-445d-86B5-A2B3D275F6CA

        private string searchTerms = string.Empty;
        private string title = string.Empty;
        private string subject = string.Empty;
        private bool loopTweets = true;
        private int widgetWidth = 250;
        private int widgetHeight = 300;
        private string shellBackColor = "#30ad71";
        private string shellForeColor = "#ffffff";
        private string tweetsBackColor = "#ffffff";
        private string tweetsForeColor = "#444444";
        private string tweetsLinkColor = "";

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            //TitleControl.EditUrl = SiteRoot + "/TwitterSearch/TwitterSearchEdit.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (searchTerms.Length == 0)
            {
                lblNoSearch.Visible = true;
                twitter.Visible = false;
                return;
            }

            twitter.SearchTerms = searchTerms;
            twitter.Title = title;
            twitter.Subject = subject;
            twitter.Profile = false;
            twitter.Loop = loopTweets;
            twitter.WidgetWidth = widgetWidth;
            twitter.WidgetHeight = widgetHeight;
            twitter.ShellBackColor = shellBackColor;
            twitter.ShellForeColor = shellForeColor;
            twitter.TweetsBackColor = tweetsBackColor;
            twitter.TweetsForeColor = tweetsForeColor;
            twitter.TweetsLinkColor = tweetsLinkColor;

        }


        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {

            if (Settings.Contains("SearchTerms"))
            {
                searchTerms = SecurityHelper.RemoveMarkup(Settings["SearchTerms"].ToString()).HtmlEscapeQuotes();
            }

            if (Settings.Contains("Title"))
            {
                title = SecurityHelper.RemoveMarkup(Settings["Title"].ToString()).HtmlEscapeQuotes();
            }
            if (Settings.Contains("Subject"))
            {
                subject = SecurityHelper.RemoveMarkup(Settings["Subject"].ToString()).HtmlEscapeQuotes();
            }

            loopTweets = WebUtils.ParseBoolFromHashtable(Settings, "LoopTweets", loopTweets);

            widgetWidth = WebUtils.ParseInt32FromHashtable(Settings, "WidgetWidth", widgetWidth);

            widgetHeight = WebUtils.ParseInt32FromHashtable(Settings, "WidgetHeight", widgetHeight);

            if (Settings.Contains("ShellBackColor"))
            {
                shellBackColor = Settings["ShellBackColor"].ToString();
            }

            if (Settings.Contains("ShellForeColor"))
            {
                shellForeColor = Settings["ShellForeColor"].ToString();
            }

            if (Settings.Contains("TweetsBackColor"))
            {
                tweetsBackColor = Settings["TweetsBackColor"].ToString();
            }
            if (Settings.Contains("TweetsForeColor"))
            {
                tweetsForeColor = Settings["TweetsForeColor"].ToString();
            }

            if (Settings.Contains("TweetsLinkColor"))
            {
                tweetsLinkColor = Settings["TweetsLinkColor"].ToString();
            }

        }


    }
}
