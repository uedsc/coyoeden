/// Author:				    Joe Audette
/// Created:			    2006-10-01
/// Last Modified:		    2010-02-08
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI.Pages
{
    
    public partial class SiteMapPage : CBasePage
    {
        protected SiteMapDataSource siteMapDataSource;

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SiteMap1.MenuItemDataBound += new MenuEventHandler(SiteMap1_MenuItemDataBound);

            if (WebConfigSettings.HideMenusOnSiteMap)
            {
                SuppressAllMenus();
            }
            else
            {
                SuppressMenuSelection();
                if (WebConfigSettings.HidePageMenusOnSiteMap) { SuppressPageMenu(); }
            }
        }
        #endregion

        private bool useImagesInSiteMap = false;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private bool isSecureRequest = false;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteMapLink);

            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                insecureSiteRoot = secureSiteRoot.Replace("https", "http");
            }

            isSecureRequest = Request.IsSecureConnection;

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionSiteMapFormat, siteSettings.SiteName);

            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "Csite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            if (Request.Params["startnode"] != null)
            {
                string startNode = Server.UrlDecode(Request.Params["startnode"]);
                SiteMapNode node
                    = siteMapDataSource.Provider.FindSiteMapNode(startNode);
                if (node != null)
                {
                    siteMapDataSource.StartingNodeUrl = startNode;
                }
            }

            useImagesInSiteMap = WebConfigSettings.UsePageImagesInSiteMap;

            RenderSiteMap();
        }

        private void RenderSiteMap()
        {

            SiteMap1.Orientation = Orientation.Vertical;

            SiteMap1.PathSeparator = ',';
            SiteMap1.MaximumDynamicDisplayLevels = 20;

            SiteMap1.DataSourceID = this.siteMapDataSource.ID;
            SiteMap1.DataBind();

            //System.Web.UI.WebControls.MenuItem menuItem;
            //menuItem = SiteMap1.FindItem(SiteUtils.GetActivePageValuePath());

            //if (menuItem != null)
            //{
            //    menuItem.Selected = true;
            //}

        }

        void SiteMap1_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            Menu menu = (Menu)sender;
            CSiteMapNode mapNode = (CSiteMapNode)e.Item.DataItem;
            if ((useImagesInSiteMap)&&(mapNode.MenuImage.Length > 0))
            {
                e.Item.ImageUrl = mapNode.MenuImage;
            }

            if (mapNode.OpenInNewWindow)
            {
                e.Item.Target = "_blank";
            }

            // added this 2007-09-07
            // to solve treeview expand issue when page name is the same
            // as Page Name was used for value if not set explicitly
            e.Item.Value = mapNode.PageGuid.ToString();

            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if (
                        (!mapNode.UseSsl)
                        && (!siteSettings.UseSslOnAllPages)
                        && (mapNode.Url.StartsWith("~/"))
                        )
                    {
                        e.Item.NavigateUrl = insecureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
                else
                {
                    if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
                    {
                        if (mapNode.Url.StartsWith("~/"))
                            e.Item.NavigateUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
            }

            if (
                (
                WebUser.IsAdmin
                || (WebUser.IsContentAdmin && (!(mapNode.Roles.Count == 1) && (mapNode.Roles[0].ToString() == "Admins")))
                || (WebUser.IsInRoles(mapNode.Roles))
                )
                &&(mapNode.IncludeInSiteMap)
                &&(!mapNode.IsPending)
                )
               return;

            

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

