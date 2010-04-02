///		Author:				Joe Audette
///		Created:			2005-05-21
///		Last Modified:		2008-11-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
	
	public partial class ChildPageMenu : System.Web.UI.UserControl
	{
		private string cssClass = "txtnormal";
        private PageSettings currentPage;
        private SiteMapDataSource siteMapDataSource;
        private bool usePageImages = false;
        private bool hidePagesNotInSiteMap = false;
        private int maximumDynamicDisplayLevels = 20;
        private bool treatChildPageIndexAsSiteMap = false;

        public int MaximumDynamicDisplayLevels
        {
            get { return maximumDynamicDisplayLevels; }
            set { maximumDynamicDisplayLevels = value; }
        }

        public bool UsePageImages
        {
            get { return usePageImages; }
            set { usePageImages = value; }
        }

        public bool HidePagesNotInSiteMap
        {
            get { return hidePagesNotInSiteMap; }
            set { hidePagesNotInSiteMap = value; }
        }


		public string CssClass
		{	
			get {return cssClass;}
			set {cssClass = value;}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
            currentPage = CacheHelper.GetCurrentPage();
            treatChildPageIndexAsSiteMap = WebConfigSettings.TreatChildPageIndexAsSiteMap;

            if (WebConfigSettings.UsePageImagesInSiteMap && treatChildPageIndexAsSiteMap)
            {
                usePageImages = true;
            }
            

            if (
                (currentPage != null)
                && (currentPage.ShowChildPageMenu)
                && (Request.Url.ToString().IndexOf("Default.aspx") > -1)
                )
            {
                // moved and commented out 2007-08-07
                //PreviousImplementation();
                ShowChildPageMap();


            }
            else
            {
                this.Visible = false;
            }

		}

        private void ShowChildPageMap()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return;

            siteMapDataSource
                = (SiteMapDataSource)this.Page.Master.FindControl("ChildPageSiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "Csite" 
                    + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            if (siteMapDataSource == null) return;

            //SiteMapNode node
            //    = siteMapDataSource.Provider.FindSiteMapNode(Request.RawUrl);
            //if (node != null)
            //{
            //    siteMapDataSource.StartingNodeUrl = Request.RawUrl;
            //}

            SiteMapNode node
                = siteMapDataSource.Provider.FindSiteMapNode(currentPage.Url);
            if (node != null)
            {
                siteMapDataSource.StartingNodeUrl = node.Url;
            }
            else
            {
                node = siteMapDataSource.Provider.FindSiteMapNode(Request.RawUrl);
                if (node != null)
                {
                    siteMapDataSource.StartingNodeUrl = Request.RawUrl;
                }
            }

            SiteMap1.MenuItemDataBound += new MenuEventHandler(SiteMap_MenuItemDataBound);
            SiteMap1.Orientation = Orientation.Vertical;

            SiteMap1.PathSeparator = '|';
            SiteMap1.MaximumDynamicDisplayLevels = maximumDynamicDisplayLevels;

            SiteMap1.DataSourceID = siteMapDataSource.ID;
            SiteMap1.DataBind();

           


        }

        void SiteMap_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            Menu menu = (Menu)sender;
            CSiteMapNode mapNode = (CSiteMapNode)e.Item.DataItem;
            if ((usePageImages) && (mapNode.MenuImage.Length > 0))
            {
                e.Item.ImageUrl = mapNode.MenuImage;
            }

            bool remove = false;

            if (
                (
                WebUser.IsAdmin
                || (WebUser.IsContentAdmin && (!(mapNode.Roles.Count == 1) && (mapNode.Roles[0].ToString() == "Admins")))
                || (WebUser.IsInRoles(mapNode.Roles))
                )
                )
            {
                remove = false;
            }
            else
            {
                remove = true;
            }

            if ((treatChildPageIndexAsSiteMap)||(hidePagesNotInSiteMap))
            {
                if (!mapNode.IncludeInSiteMap) { remove = true; }
            }
            else
            {
                if (!mapNode.IncludeInMenu) { remove = true; }
            }

            if ((mapNode.HideAfterLogin) && (Request.IsAuthenticated)) remove = true;


            if (remove)
            {
                if (e.Item.Depth == 0)
                {
                    menu.Items.Remove(e.Item);
                }
                else
                {
                    MenuItem parent = e.Item.Parent;
                    if (parent != null)
                    {
                        parent.ChildItems.Remove(e.Item);
                    }
                }
            }
        }


       

		
	}
}
