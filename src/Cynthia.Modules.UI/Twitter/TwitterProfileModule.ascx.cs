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
using Cynthia.Web;
using Cynthia.Web.Framework;


namespace Cynthia.Modules.UI
{

    public partial class TwitterProfileModule : SiteModuleControl
    {
        // FeatureGuid F36D66D7-C459-4ff9-A18B-93EB401A3843

        private string username = string.Empty;
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
            //TitleControl.EditUrl = SiteRoot + "/TwitterProfile/TwitterProfileEdit.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (username.Length == 0)
            {
                lblNoUser.Visible = true;
                twitter.Visible = false;
                return;
            }

            twitter.ProfileName = username;
            twitter.Profile = true;
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
            if (Settings.Contains("Username"))
            {
                username = Settings["Username"].ToString();
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
