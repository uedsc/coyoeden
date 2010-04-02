// Author:					Joe Audette
// Created:				    2006-08-28
// Last Modified:			2010-03-20
//		
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{
    public partial class PageMenuControl : UserControl
    {
        #region Private/Protected Properties

        private SiteMapDataSource pageMapDataSource;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private SiteSettings siteSettings;
        private PageSettings currentPage;
        private static readonly ILog log = LogManager.GetLogger(typeof(PageMenuControl));

        private int dynamicDisplayLevels = 100;
        private bool useTreeView = false;
        private int treeViewExpandDepth = 0;
        private bool treeViewPopulateOnDemand = true;
        private bool treeViewShowExpandCollapse = true;
        //private bool treeviewPopulateNodesFromClient = true;
        private int startingNodeOffset = 0;
        private string siteMapDataSource = "PageMapDataSource";

        private bool useSpanInLinks = false;
        private bool use3SpansInLinks = false;
        private bool useSuperfish = false;

        private string direction = "Vertical";

        private int currentPageDepth = 0;
        private bool isSecureRequest = false;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private bool includeCornerRounders = true;

        private bool useArtisteer = false;
        private bool isSubMenu = true;

        

        #endregion

        #region Public Properties

        /// <summary>
        /// the server side control id of the SiteMapDataSourceControl to use
        /// </summary>
        public string SiteMapDataSource
        {
            get { return siteMapDataSource; }
            set { siteMapDataSource = value; }
        }

        public bool UseTreeView
        {
            get { return useTreeView; }
            set { useTreeView = value; }
        }

        public bool UseArtisteer
        {
            get { return useArtisteer; }
            set { useArtisteer = value; }
        }

        public bool IsSubMenu
        {
            get { return isSubMenu; }
            set { isSubMenu = value; }
        }

        public bool TreeViewShowExpandCollapse
        {
            get { return treeViewShowExpandCollapse; }
            set { treeViewShowExpandCollapse = value; }
        }

        public bool TreeViewPopulateOnDemand
        {
            get { return treeViewPopulateOnDemand; }
            set { treeViewPopulateOnDemand = value; }
        }

        public int TreeViewExpandDepth
        {
            get { return treeViewExpandDepth; }
            set { treeViewExpandDepth = value; }
        }


        //public bool TreeviewPopulateNodesFromClient
        //{
        //    get { return treeviewPopulateNodesFromClient; }
        //    set { treeviewPopulateNodesFromClient = value; }
        //}


        public int StartingNodeOffset
        {
            get { return startingNodeOffset; }
            set { startingNodeOffset = value; }
        }

        public int DynamicDisplayLevels
        {
            get { return dynamicDisplayLevels; }
            set { dynamicDisplayLevels = value; }
        }


        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public bool IncludeCornerRounders
        {
            get { return includeCornerRounders; }
            set { includeCornerRounders = value; }
        }

        public bool UseSpanInLinks
        {
            get { return useSpanInLinks; }
            set { useSpanInLinks = value; }
        }

        public bool Use3SpansInLinks
        {
            get { return use3SpansInLinks; }
            set { use3SpansInLinks = value; }
        }

        public bool UseSuperfish
        {
            get { return useSuperfish; }
            set { useSuperfish = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                insecureSiteRoot = secureSiteRoot.Replace("https", "http");
            }

            isSecureRequest = Request.IsSecureConnection;

            pageMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl(siteMapDataSource);

            if (pageMapDataSource == null) { return; }

            pageMapDataSource.SiteMapProvider = "Csite" + siteSettings.SiteId.ToInvariantString();

            if ((Direction == "Horizontal")||(!includeCornerRounders))
            {
                topRounder.Visible = false;
                bottomRounder.Visible = false;
            }

            if (startingNodeOffset > 0)
            {
                currentPageDepth = SiteUtils.GetCurrentPageDepth(pageMapDataSource.Provider.RootNode);

                if (currentPageDepth >= startingNodeOffset)
                {
                    startingNodeOffset -= 1;
                }

            }

            if (SiteUtils.TopPageHasChildren(pageMapDataSource.Provider.RootNode, startingNodeOffset))
            {
                
                
                currentPage = CacheHelper.GetCurrentPage();
                bool showMenu = true;

                if (siteSettings == null)
                {
                    showMenu = false;
                    log.Error("tried to get siteSettings in Page_Load of PageeMenu.ascx but it came back null");
                }

                if (currentPage == null)
                {
                    showMenu = false;
                    log.Error("tried to get currentPage in Page_Load of PageeMenu.ascx but it came back null");
                }

                if (
                    (siteSettings != null)
                    &&(currentPage != null)
                    &&(siteSettings.AllowHideMenuOnPages)
                    )
                {
                    if (currentPage.HideMainMenu)
                    {
                        showMenu = false;
                    }
                }
                if (showMenu)
                {
                    isAdmin = WebUser.IsAdmin;
                    isContentAdmin = WebUser.IsContentAdmin;
                    
                    if (useTreeView)
                    {
                        RenderTreeView();
                    }
                    else
                    {
                        RenderMenu();
                    }
                }
                else
                {
                    this.Visible = false;
                }
            }
            else if (!isSubMenu)
            {
                isAdmin = WebUser.IsAdmin;
                isContentAdmin = WebUser.IsContentAdmin;
                if (useTreeView)
                {
                    RenderTreeView();
                }
                else
                {
                    RenderMenu();
                }
            }
            else
            {
                this.Visible = false;
            }
        }


        #region ASP.NET Menu

        private void RenderMenu()
        {
            Menu pageMenu = GetMenu();
            this.menuPlaceHolder.Controls.Add(pageMenu);
            pageMenu.MenuItemDataBound += new MenuEventHandler(pageMenu_MenuItemDataBound);

            if (direction == "Vertical")
            {
                pageMenu.Orientation = Orientation.Vertical;
            }
            else
            {
                pageMenu.Orientation = Orientation.Horizontal;
            }

            pageMenu.EnableViewState = false;
            pageMenu.PathSeparator = '|';

            if (currentPage == null) { currentPage = CacheHelper.GetCurrentPage(); }
            
            if (isSubMenu)
            {
                if (
                    (currentPage != null)
                    && (currentPage.ParentId == -1)
                    )
                {  // this is a root level page

                    if (
                        (currentPage.UseUrl)
                        && (currentPage.Url.Length > 0)
                        )
                    {
                        pageMapDataSource.StartingNodeUrl = currentPage.Url;
                    }
                    else
                    {
                        String pageUrl = "~/Default.aspx?pageid=" + currentPage.PageId.ToString();
                        pageMapDataSource.StartingNodeUrl = pageUrl;
                    }


                }
                else
                {
                    // not a root level page

                    //pageMapDataSource.StartingNodeUrl
                    //    = SiteUtils.GetTopParentUrlForPageMenu(pageMapDataSource.Provider.RootNode);

                    pageMapDataSource.StartingNodeUrl = SiteUtils.GetStartUrlForPageMenu(pageMapDataSource.Provider.RootNode, startingNodeOffset);
                }
            }

            

            //if (startingNodeOffset > 0)
            //{
            //    pageMapDataSource.StartingNodeOffset = startingNodeOffset;
            //}


          

            pageMenu.MaximumDynamicDisplayLevels = dynamicDisplayLevels;
            
            pageMenu.DataSourceID = pageMapDataSource.ID;
            try
            {
                pageMenu.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }

            DoMenuSelection(pageMenu);

            
            if (pageMenu.Items.Count == 0) this.Visible = false;

        }

        private void DoMenuSelection(Menu menu)
        {
            // TODO: clean up this hairy mess without breaking anything

            MenuItem menuItem = null;
            bool didSelect = false;
            string valuePath;

            if (isSubMenu)
            {
                valuePath = SiteUtils.GetPageMenuActivePageValuePath(pageMapDataSource.Provider.RootNode);
            }
            else
            {
                valuePath = SiteUtils.GetActivePageValuePath(pageMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);
            }

            if (valuePath.Length > 0)
            {
                menuItem = menu.FindItem(valuePath);

                if (menuItem == null)
                {
                    if (startingNodeOffset > 0)
                    {
                        for (int i = 1; i <= startingNodeOffset; i++)
                        {
                            if (valuePath.IndexOf("|") > -1)
                            {
                                valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
                            }

                        }
                    }
                }



                if (menuItem == null)
                {
                    valuePath = SiteUtils.GetPageMenuActivePageValuePath(pageMapDataSource.Provider.RootNode);
                    menuItem = menu.FindItem(valuePath);
                }

                if (menuItem != null)
                {
                    try
                    {
                        menuItem.Selected = true;
                        didSelect = true;
                    }
                    catch (InvalidOperationException)
                    {
                        //can happen if node disabled or unselectable
                    }
                }
            }

            if (!didSelect)
            {
                valuePath = SiteUtils.GetActivePageValuePath(pageMapDataSource.Provider.RootNode, startingNodeOffset);


                if (valuePath.Length > 0)
                {

                    menuItem = menu.FindItem(valuePath);

                    if (
                         (menuItem == null)
                         && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {
                        valuePath = valuePath.Substring(0, (valuePath.IndexOf(menu.PathSeparator)));
                        menuItem = menu.FindItem(valuePath);
                    }

                    if (
                        (dynamicDisplayLevels == 0)
                        && (menuItem == null)
                        && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {

                        foreach (MenuItem m in menu.Items)
                        {
                            if (valuePath.Contains(m.ValuePath))
                            {
                                try
                                {
                                    m.Selected = true;
                                    didSelect = true;
                                }
                                catch (InvalidOperationException)
                                {
                                    //can happen if node disabled or unselectable
                                }
                                return;
                            }

                        }
                    }


                    if (menuItem != null)
                    {
                        try
                        {
                            menuItem.Selected = true;
                            didSelect = true;
                        }
                        catch (InvalidOperationException)
                        {
                            //can happen if node disabled or unselectable
                        }
                    }

                }
            }

            if (!didSelect)
            {
                valuePath = SiteUtils.GetActivePageValuePath(pageMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);

                if (valuePath.Length > 0)
                {

                    menuItem = menu.FindItem(valuePath);

                    if (menuItem == null)
                    {
                        if (currentPage == null) { currentPage = CacheHelper.GetCurrentPage(); }
                        if (currentPage != null)
                        {
                            menuItem = menu.FindItem(currentPage.PageGuid.ToString());
                        }
                    }


                    if (
                        (dynamicDisplayLevels == 0)
                        && (menuItem == null)
                        && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {

                        foreach (MenuItem m in menu.Items)
                        {
                            if (valuePath.Contains(m.ValuePath))
                            {
                                try
                                {
                                    m.Selected = true;
                                }
                                catch (InvalidOperationException)
                                {
                                    //can happen if node disabled or unselectable
                                }
                                return;
                            }

                        }
                    }

                    if (menuItem != null)
                    {
                        try
                        {
                            menuItem.Selected = true;
                            didSelect = true;
                        }
                        catch (InvalidOperationException)
                        {
                            //can happen if node disabled or unselectable
                        }
                    }

                }
            }


        }

        private Menu GetMenu()
        {
            Menu pageMenu;

            if (useSuperfish)
            {
                if (direction == "Vertical")
                {
                    pageMenu = new CMenuSuperfishVertical();
                }
                else
                {
                    pageMenu = new CMenuSuperfish();
                }
            }
            else if (useArtisteer)
            {
                pageMenu = new CMenuArtisteerVertical();
            }
            else if (use3SpansInLinks)
            {
                pageMenu = new CMenuWith3SpansInLinks();
            }
            else if (UseSpanInLinks)
            {
                pageMenu = new CMenuWithSpanInLinks();
            }
            else
            {
                pageMenu = new CMenu();
            }

            return pageMenu;


        }

        void pageMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            Menu menu = (Menu)sender;
            CSiteMapNode mapNode = (CSiteMapNode)e.Item.DataItem;
            if (mapNode.MenuImage.Length > 0)
            {
                e.Item.ImageUrl = mapNode.MenuImage;
            }

            if (mapNode.OpenInNewWindow)
            {
                e.Item.Target = "_blank";
            }

            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if ((!mapNode.UseSsl) && (!siteSettings.UseSslOnAllPages) && (mapNode.Url.StartsWith("~/")))
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

            // added this 2007-09-07
            // to solve treeview expand issue when page name is the same
            // as Page Name was used for value if not set explicitly
            e.Item.Value = mapNode.PageGuid.ToString();

            bool remove = false;

            if (!(
                    (isAdmin)
                    || (
                        (isContentAdmin)
                        && (mapNode.Roles != null)
                        && (!(mapNode.Roles.Count == 1)
                        && (mapNode.Roles[0].ToString() == "Admins")
                           )
                        )
                    || ((isContentAdmin)&&(mapNode.Roles == null))
                    || (
                        (mapNode.Roles != null)
                        && (WebUser.IsInRoles(mapNode.Roles))
                        )
                ))
            {
                remove = true;
            }

            if (!mapNode.IncludeInMenu) remove = true;
            if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) remove = true;
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


        #endregion

        #region TreeView

        private void RenderTreeView()
        {
            Page.EnableViewState = true;
            //pageMenu.Visible = false;
            PageSettings currentPage = CacheHelper.GetCurrentPage();

            CTreeView treeMenu1;

            if (useArtisteer)
            {
                treeMenu1 = new ArtisteerTreeView();
            }
            else
            {
                treeMenu1 = new CTreeView();
            }
            this.menuPlaceHolder.Controls.Add(treeMenu1);

            treeMenu1.ShowExpandCollapse = treeViewShowExpandCollapse;

            if (treeViewShowExpandCollapse)
            {
                treeMenu1.EnableViewState = true;
                treeMenu1.TreeNodeExpanded += new TreeNodeEventHandler(treeMenu1_TreeNodeExpanded);
            }
            else
            {
                treeMenu1.EnableViewState = false;
                //treeMenu1.EnableClientScript = true;
            }

            //treeMenu1.EnableClientScript = false;
            //treeMenu1.ShowExpandCollapse = treeViewShowExpandCollapse;
            treeMenu1.PopulateNodesFromClient = treeViewPopulateOnDemand;
            
            treeMenu1.ExpandDepth = treeViewExpandDepth;

            

            
            //treeMenu1.TreeNodePopulate += new TreeNodeEventHandler(treeMenu1_TreeNodePopulate);
            treeMenu1.TreeNodeDataBound += new TreeNodeEventHandler(treeMenu1_TreeNodeDataBound);

            treeMenu1.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
            treeMenu1.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;

            
            //// older skins have this
            //StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            //if (stylesheet != null)
            //{
            //    if (stylesheet.FindControl("treeviewcss") == null)
            //    {
            //        Literal cssLink = new Literal();
            //        cssLink.ID = "treeviewcss";
            //        cssLink.Text = "\n<link href='"
            //        + SiteUtils.GetSkinBaseUrl(Page)
            //        + "styletreeview.css' type='text/css' rel='stylesheet' media='screen' />";

            //        stylesheet.Controls.Add(cssLink);
            //        log.Debug("added stylesheet for treeiew");
            //    }
            //}
               

            //bool useFolderForSiteDetection
            //    = WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites;
            //string virtualFolder = VirtualFolderEvaluator.VirtualFolderName();

            if (isSubMenu)
            {
                if (
                    (currentPage != null)
                    && (currentPage.ParentId == -1)
                    )
                {
                    if (
                        (currentPage.UseUrl)
                        && (currentPage.Url.Length > 0)
                        )
                    {
                        pageMapDataSource.StartingNodeUrl = currentPage.Url;
                    }
                    else
                    {
                        String pageUrl = "~/Default.aspx?pageid=" + currentPage.PageId.ToString(CultureInfo.InvariantCulture);
                        pageMapDataSource.StartingNodeUrl = pageUrl;
                    }

                }
                else
                {
                    //pageMapDataSource.StartingNodeUrl
                    //    = SiteUtils.GetTopParentUrlForPageMenu(pageMapDataSource.Provider.RootNode);
                    pageMapDataSource.StartingNodeUrl = SiteUtils.GetStartUrlForPageMenu(pageMapDataSource.Provider.RootNode, startingNodeOffset);
                }
            }

            if (Page.IsPostBack)
            {
                // return if menu already bound
                if (treeMenu1.Nodes.Count > 0) return;
            }

            
            if (startingNodeOffset > (currentPageDepth + 1))
            {
                this.Visible = false;
                return;
            }

            //if (startingNodeOffset > 0)
            //{
            //    pageMapDataSource.StartingNodeOffset = startingNodeOffset;
            //}

            //if (treeViewExpandDepth > 0)
            //{
            //    treeMenu1.ExpandDepth = treeViewExpandDepth;
            //}

            //treeViewPopulateOnDemand = false;
            //treeMenu1.PopulateNodesFromClient = treeViewPopulateOnDemand;
            
            treeMenu1.PathSeparator = '|';
            treeMenu1.DataSourceID = pageMapDataSource.ID;
            try
            {
                treeMenu1.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }

            

            if (treeMenu1.SelectedNode != null)
            {
                CTreeView.ExpandToValuePath(treeMenu1, treeMenu1.SelectedNode.ValuePath);
            }
            else
            {
                
                //bool didSelect = false;

                String valuePath = SiteUtils.GetPageMenuActivePageValuePath(pageMapDataSource.Provider.RootNode);
                if (startingNodeOffset > 0)
                {
                    for (int i = 1; i <= startingNodeOffset; i++)
                    {
                        if (valuePath.IndexOf("|") > -1)
                        {
                            valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
                        }

                    }
                }

                CTreeView.ExpandToValuePath(treeMenu1, valuePath);
                
                TreeNode nodeToSelect = treeMenu1.FindNode(valuePath);
                if (nodeToSelect == null)
                {
                    nodeToSelect = treeMenu1.FindNode(currentPage.PageName);
                }

                if (nodeToSelect != null)
                {
                    try
                    {
                        nodeToSelect.Selected = true;
                        //didSelect = true;
                    }
                    catch (InvalidOperationException)
                    {
                        //can happen if node disabled or unselectable
                    }
                }

                

            }

            if (treeMenu1.Nodes.Count == 0) this.Visible = false;

        }

        protected void treeMenu1_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;
            //this never seems to fire

        }

        protected void treeMenu1_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            TreeView treeView = sender as TreeView;
            CTreeView.ExpandToValuePath(treeView, e.Node.ValuePath);
            
        }

        protected void treeMenu1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) { return; }
            if (e == null) { return; }

            

            TreeView menu = (TreeView)sender;
            CSiteMapNode mapNode = (CSiteMapNode)e.Node.DataItem;
            if (mapNode.MenuImage.Length > 0)
            {
                e.Node.ImageUrl = mapNode.MenuImage;
            }

            if (treeViewShowExpandCollapse)
            {
                if (e.Node is CTreeNode)
                {
                    CTreeNode tn = e.Node as CTreeNode;
                    tn.HasVisibleChildren = mapNode.HasVisibleChildren();

                }
            }

            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if ((!mapNode.UseSsl) &&(!siteSettings.UseSslOnAllPages) && (mapNode.Url.StartsWith("~/")))
                    {
                        e.Node.NavigateUrl = insecureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
                else
                {
                    if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
                    {
                        if (mapNode.Url.StartsWith("~/"))
                            e.Node.NavigateUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
            }

            if (mapNode.OpenInNewWindow)
            {
                e.Node.Target = "_blank";
            }

            

            // added this 2007-09-07
            // to solve treeview expand issue when page name is the same
            // as Page Name was used for value if not set explicitly
            e.Node.Value = mapNode.PageGuid.ToString();

            //log.Info("databound tree node with value path " + e.Node.ValuePath);

            bool remove = false;

            if (!(
                    (isAdmin)
                    || (
                        (isContentAdmin)
                        && (mapNode.Roles != null)
                        && (!(mapNode.Roles.Count == 1)
                        && (mapNode.Roles[0].ToString() == "Admins")
                           )
                        )
                    || ((isContentAdmin) && (mapNode.Roles == null))
                    || (
                        (mapNode.Roles != null)
                        && (WebUser.IsInRoles(mapNode.Roles))
                        )
                ))
            {
                remove = true;
            }

            if (!mapNode.IncludeInMenu) remove = true;
            if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) remove = true;
            if ((mapNode.HideAfterLogin) && (Request.IsAuthenticated)) remove = true;

            if (remove)
            {
                if (e.Node.Depth == 0)
                {
                    menu.Nodes.Remove(e.Node);
                }
                else
                {
                    TreeNode parent = e.Node.Parent;
                    if (parent != null)
                    {
                        parent.ChildNodes.Remove(e.Node);
                    }
                }
            }
            else
            {
                //e.Node.PopulateOnDemand = treeViewPopulateOnDemand;

                if (mapNode.HasVisibleChildren())
                {
                    e.Node.PopulateOnDemand = treeViewPopulateOnDemand;
                }
                else
                {
                    e.Node.PopulateOnDemand = false;
                }
            }

        }

        

        #endregion

    }
}
