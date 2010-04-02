// Author:				        Joe Audette
// Created:			            2009-05-02
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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Cynthia.Business;
using Resources;

namespace Cynthia.Web.BlogUI
{
    public partial class BlogCategories : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string siteRoot = string.Empty;
        private bool renderAsTagCloud = true;
        private bool canEdit = false;
        //protected string EditContentImage = ConfigurationManager.AppSettings["EditContentImage"];
        //protected string ulClass = "blognav";
       

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

        public bool RenderAsTagCloud
        {
            get { return renderAsTagCloud; }
            set { renderAsTagCloud = value; }
        }

        public bool CanEdit
        {
            get { return canEdit; }
            set { canEdit = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Visible)
            {
                BindList();
            }


            base.OnPreRender(e);

        }

        private void BindList()
        {
            if (pageId == -1) { return; }
            if (moduleId == -1) { return; }

            //lnkEditCategories.ToolTip = BlogResources.BlogEditCategoriesLabel;
            //lnkEditCategories.Visible = canEdit;
            //lnkEditCategories.NavigateUrl = SiteRoot
            //    + "/Blog/EditCategory.aspx?pageid=" + PageId.ToString(CultureInfo.InvariantCulture)
            //    + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture);

            //this.lnkEditCategories.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + EditContentImage);
            //this.lnkEditCategories.Text = BlogResources.EditImageAltText;

            using (IDataReader reader = Blog.GetCategories(ModuleId))
            {
                if (renderAsTagCloud)
                {
                    dlCategories.Visible = false;
                    cloud.DataHrefFormatString = SiteRoot + "/Blog/ViewCategory.aspx?cat={0}&amp;mid=" 
                        + ModuleId.ToString(CultureInfo.InvariantCulture) 
                        + "&amp;pageid=" + PageId.ToString(CultureInfo.InvariantCulture);

                    cloud.UseWeightInTextFormat = true;
                    cloud.DataHrefField = "CategoryID";
                    cloud.DataTextField = "Category";
                    cloud.DataWeightField = "PostCount";
                    cloud.DataSource = reader;
                    cloud.DataBind();

                    if (cloud.Items.Count == 0) { Visible = false; }
                    

                }
                else
                {
                    cloud.Visible = false;
                    dlCategories.DataSource = reader;
                    dlCategories.DataBind();
                    dlCategories.Visible = (dlCategories.Items.Count > 0);
                    if (dlCategories.Items.Count == 0) { Visible = false; }
                }
                
            }

            
            

        }

        protected string GetCssClass(double catagoryCount)
        {

            return string.Empty;
        }
    }
}