// Author:					Joe Audette
// Created:				    2009-09-17
// Last Modified:			2009-09-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
    public class TwitterWidget : Panel
    {

        private bool profile = true;
        public bool Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        private string profileName = string.Empty;
        public string ProfileName
        {
            get { return profileName; }
            set { profileName = value; }
        }

        private bool loop = true;
        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        private int widgetWidth = 250;
        public int WidgetWidth
        {
            get { return widgetWidth; }
            set { widgetWidth = value; }
        }

        private int widgetHeight = 300;
        public int WidgetHeight
        {
            get { return widgetHeight; }
            set { widgetHeight = value; }
        }

        private string searchTerms = string.Empty;
        public string SearchTerms
        {
            get { return searchTerms; }
            set { searchTerms = value; }
        }

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string subject = string.Empty;
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string shellBackColor = "#30ad71";
        public string ShellBackColor
        {
            get { return shellBackColor; }
            set { shellBackColor = value; }
        }

        private string shellForeColor = "#ffffff";
        public string ShellForeColor
        {
            get { return shellForeColor; }
            set { shellForeColor = value; }
        }

        private string tweetsBackColor = "#ffffff";
        public string TweetsBackColor
        {
            get { return tweetsBackColor; }
            set { tweetsBackColor = value; }
        }

        private string tweetsForeColor = "#444444";
        public string TweetsForeColor
        {
            get { return tweetsForeColor; }
            set { tweetsForeColor = value; }
        }

        private string tweetsLinkColor = "#1985b5";
        public string TweetsLinkColor
        {
            get { return tweetsLinkColor; }
            set { tweetsLinkColor = value; }
        }

        private bool isValid = true;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Visible) { return; }

            ValidateSettings();
            if (!isValid) { return; }

            
            SetupMainScript();
            SetupInstanceScript();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetupCss();
        }

        private void ValidateSettings()
        {
            //TODO:
        }

        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">\n");
            script.Append("new TWTR.Widget({");

            if (profile)
            {
                script.Append("profile: true");
            }
            else
            {
                script.Append("search: \"" + searchTerms + "\"");
                script.Append(",title: \"" + title + "\"");
                script.Append(",subject: \"" + subject + "\"");
            }

            script.Append(",id: '" + ClientID + "'");
            if (loop)
            {
                script.Append(",loop: true");
            }
            else
            {
                script.Append(",loop: false");
            }

            script.Append(",width:" + widgetWidth.ToInvariantString());
            script.Append(",height:" + widgetHeight.ToInvariantString());
            script.Append(",theme: {shell: {");
            script.Append("background: '" + shellBackColor + "'");
            script.Append(",color:'" + shellForeColor + "'");
            script.Append("}");
            script.Append(",tweets: {");
            script.Append("background: '" + tweetsBackColor + "'");
            script.Append(",color: '" + tweetsForeColor + "'");
            script.Append(",links: '" + tweetsLinkColor + "'");
            script.Append("}");
            script.Append("}");

            if (profile)
            {
                script.Append("}).render().setProfile('" + profileName + "').start();");
            }
            else
            {
                script.Append("}).render().start();");
            }

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());

        }


        private void SetupMainScript()
        {
            
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "twitterwidget", "\n<script  src=\"http://widgets.twimg.com/j/1/widget.js\" type=\"text/javascript\" ></script>");


        }

        private void SetupCss()
        {
            if (Page.Master == null) { return; }

            StyleSheetCombiner style = Page.Master.FindControl("StyleSheetCombiner") as StyleSheetCombiner;
            if (style != null) { style.IncludeTwitterCss = true; }

            //try
            //{
            //    Control head = Page.Master.FindControl("Head1");
            //    if (head == null) { return; }

            //    if (head.FindControl("twittercss") == null)
            //    {
            //        Literal cssLink = new Literal();
            //        cssLink.ID = "twittercsss";
            //        cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='http://widgets.twimg.com/j/1/widget.css' />";
                    
            //            head.Controls.Add(cssLink);
                    
            //    }
            //}
            //catch (HttpException) { }
        }

    }
}
