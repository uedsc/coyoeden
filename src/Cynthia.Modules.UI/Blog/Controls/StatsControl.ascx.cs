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
    public partial class StatsControl : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        private int countOfDrafts = 0;
        private bool showCommentCount = true;

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

        public int CountOfDrafts
        {
            get { return countOfDrafts; }
            set { countOfDrafts = value; }
        }

        public bool ShowCommentCount
        {
            get { return showCommentCount; }
            set { showCommentCount = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Visible)
            {
                PopulateControls();
            }


            base.OnPreRender(e);

        }

        private void PopulateControls()
        {
            if (pageId == -1) { return; }
            if (moduleId == -1) { return; }

            using (IDataReader reader = Blog.GetBlogStats(ModuleId))
            {
                if (reader.Read())
                {
                    int entryCount = Convert.ToInt32(reader["EntryCount"]);
                    int commentCount = Convert.ToInt32(reader["CommentCount"]);
                    litEntryCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogEntryCountLabel, (entryCount - countOfDrafts));
                    litCommentCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogCommentCountLabel, commentCount);

                }
                else
                {
                    litEntryCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogEntryCountLabel, 0);
                    litCommentCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogCommentCountLabel, 0);
                }

            }

            liComments.Visible = showCommentCount;

        }
    }
}