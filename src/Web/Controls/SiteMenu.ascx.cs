
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{
	
	public partial class SiteMenu : UserControl
	{
	
		#region Private/Protected Properties

        //private Collection<PageSettings> menuPages;
        private SiteMapDataSource siteMapDataSource;
        private bool useTreeView = false;
        private int treeViewExpandDepth = 0;
        private bool treeViewPopulateOnDemand = true;
        private bool treeViewShowExpandCollapse = true;
        private bool treeviewPopulateNodesFromClient = true;
        private bool topLevelOnly = false;
		private string direction = "Horizontal";
        //private int pageIndex;
        private bool showPages = true;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private SiteSettings siteSettings;
        private PageSettings currentPage;
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteMenu));
        private int startingNodeOffset = 0;
        private bool suppressPageSelection = false;
        private bool useSpanInLinks = false;
        private bool use3SpansInLinks = false;
        private bool useArtisteer = false;
        private bool hideMenuOnSiteMap = true;
        private bool isSecureRequest = false;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private bool useSuperfish = false;
        private bool includeCornerRounders = false;
        private int dynamicDisplayLevels = 100;

        

		#endregion
		
		#region Public Properties

        public bool UseTreeView
        {
            get { return useTreeView; }
            set { useTreeView = value; }
        }

        public bool UseSuperfish
        {
            get { return useSuperfish; }
            set { useSuperfish = value; }
        }

        public bool UseArtisteer
        {
            get { return useArtisteer; }
            set { useArtisteer = value; }
        }

        public bool SuppressPageSelection
        {
            get { return suppressPageSelection; }
            set { suppressPageSelection = value; }
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

        public bool TreeViewShowExpandCollapse
        {
            get { return treeViewShowExpandCollapse; }
            set { treeViewShowExpandCollapse = value; }
        }

        public bool TreeviewPopulateNodesFromClient
        {
            get { return treeviewPopulateNodesFromClient; }
            set { treeviewPopulateNodesFromClient = value; }
        }


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

        public bool TopLevelOnly
        {
            get { return topLevelOnly; }
            set { topLevelOnly = value; }
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

        /// <summary>
        /// legacy property not used
        /// </summary>
        public bool HideMenuOnSiteMap
        {
            get { return hideMenuOnSiteMap; }
            set { hideMenuOnSiteMap = value; }
        }


		#endregion

		
		protected void Page_Load(object sender, EventArgs e)
		{
            String rawUrl = Request.RawUrl;
            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                insecureSiteRoot = secureSiteRoot.Replace("https", "http");
            }

            isSecureRequest = Request.IsSecureConnection;

            if ((Direction == "Horizontal") || (!includeCornerRounders))
            {
                topRounder.Visible = false;
                bottomRounder.Visible = false;
            }
            

            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            currentPage = CacheHelper.GetCurrentPage();

            if (siteSettings == null)
            {
                log.Error("tried to get siteSettings in Page_Load of SiteMenu.ascx but it came back null");
            }

            if (currentPage == null)
            {
                log.Error("tried to get currentPage in Page_Load of SiteMenu.ascx but it came back null");
            }

            if (
                (siteSettings != null)
                && (currentPage != null)
                )
            {
                PopulateControls();
            } 
           

        }

        private void PopulateControls()
        {
            bool hideMenu = siteSettings.AllowHideMenuOnPages && currentPage.HideMainMenu;
            if (showPages && !hideMenu)
            {
                siteMapDataSource = (SiteMapDataSource) this.Page.Master.FindControl("SiteMapData");
                if (siteMapDataSource == null) return;
                
                siteMapDataSource.SiteMapProvider
                    = String.Format("Csite{0}", siteSettings.SiteId.ToString(CultureInfo.InvariantCulture));

                if (this.useTreeView)
                {
                    RenderTreeView();
                }
                else
                {
                    RenderMenu();
                }
                
            }
        }

        
        #region ASP.NET Menu

        private void RenderMenu()
        {
            Menu menu = GetMenu();
            this.menuPlaceHolder.Controls.Add(menu);
            menu.MenuItemDataBound += new MenuEventHandler(pageMenu_MenuItemDataBound);
            
            if (direction == "Vertical")
            {
                menu.Orientation = Orientation.Vertical;
            }
            else
            {
                menu.Orientation = Orientation.Horizontal;
            }

           
            menu.EnableViewState = false;
            menu.PathSeparator = '|';

            if (topLevelOnly)
            {
                menu.MaximumDynamicDisplayLevels = 0;
            }
            else
            {
                menu.MaximumDynamicDisplayLevels = dynamicDisplayLevels;
            }

            if (startingNodeOffset > 0)
            {
                siteMapDataSource.StartingNodeOffset = startingNodeOffset;
            }

            menu.DataSourceID = siteMapDataSource.ID;
            try
            {
                menu.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }

            DoSelecetion(menu);

        }

        private void DoSelecetion(Menu menu)
        {
            if (suppressPageSelection) { return; }
            bool didSelect = false;

            
            String valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);

            if (valuePath.Length > 0)
            {
                MenuItem menuItem;
                menuItem = menu.FindItem(valuePath);

                if (
                    (topLevelOnly)
                    && (menuItem == null)
                    && (valuePath.IndexOf(menu.PathSeparator) > -1)
                    )
                {
                    valuePath = valuePath.Substring(0, (valuePath.IndexOf(menu.PathSeparator)));
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
                valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset);

                if (valuePath.Length > 0)
                {
                    MenuItem menuItem;
                    menuItem = menu.FindItem(valuePath);

                    if (
                        (topLevelOnly)
                        && (menuItem == null)
                        && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {
                        valuePath = valuePath.Substring(0, (valuePath.IndexOf(menu.PathSeparator)));
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

            }

        }

        private Menu GetMenu()
        {
            Menu menu;

            if (useSuperfish)
            {
                if (direction == "Vertical")
                {
                    menu = new CMenuSuperfishVertical();
                }
                else
                {
                    menu = new CMenuSuperfish();
                }
            }
            else if (useArtisteer)
            {
                menu = new CMenuArtisteer();
            }
            else if (use3SpansInLinks)
            {
                menu = new CMenuWith3SpansInLinks();
            }
            else if (UseSpanInLinks)
            {
                menu = new CMenuWithSpanInLinks();
            }
            else
            {
                menu = new CMenu();
            }

            return menu;
        }

        protected void pageMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
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
                    if ((mapNode.UseSsl)||(siteSettings.UseSslOnAllPages))
                    {
                        if (mapNode.Url.StartsWith("~/"))
                            e.Item.NavigateUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
            }

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
                        &&(WebUser.IsInRoles(mapNode.Roles))
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
            //menu1.Visible = false;

            CTreeView treeMenu1 = new CTreeView();
            this.menuPlaceHolder.Controls.Add(treeMenu1);

            treeMenu1.EnableViewState = true;
            //treeMenu1.EnableClientScript = true;
            treeMenu1.ShowExpandCollapse = treeViewShowExpandCollapse;
#if !MONO
            treeMenu1.PopulateNodesFromClient = treeviewPopulateNodesFromClient;
#endif
            treeMenu1.ExpandDepth = 0;

            
            //treeMenu1.TreeNodePopulate += new TreeNodeEventHandler(treeMenu1_TreeNodePopulate);
            treeMenu1.TreeNodeDataBound += new TreeNodeEventHandler(treeMenu1_TreeNodeDataBound);
            treeMenu1.TreeNodeExpanded += new TreeNodeEventHandler(treeMenu1_TreeNodeExpanded);

            
            //older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("treeviewcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "treeviewcss";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl(Page)
                    + "styletreeview.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                    log.Debug("added stylesheet for treeiew");
                }
            }
                
            if (treeViewExpandDepth > 0)
            {
                treeMenu1.ExpandDepth = treeViewExpandDepth;
                log.Debug("set ExpandDepth to " + treeViewExpandDepth.ToString(CultureInfo.InvariantCulture));

            }

#if !MONO
            treeMenu1.PopulateNodesFromClient = treeViewPopulateOnDemand;
#endif
            treeMenu1.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
            treeMenu1.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;
            
    
            if (Page.IsPostBack)
            {
                // return if menu already bound
                if(treeMenu1.Nodes.Count > 0) return;
            }
            treeMenu1.PathSeparator = '|';
            treeMenu1.DataSourceID = this.siteMapDataSource.ID;
            try
            {
                treeMenu1.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }
            
            if (
                (treeMenu1.SelectedNode != null)
                &&(!suppressPageSelection)
                )
            {
                CTreeView.ExpandToValuePath(treeMenu1, treeMenu1.SelectedNode.ValuePath);
                log.Debug("called CTreeview.ExpandToValuePath for selectednode value path: " + treeMenu1.SelectedNode.ValuePath);
            }
            else
            {
                bool didSelect = false;

                if (!suppressPageSelection)
                {
                    String valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);
                    CTreeView.ExpandToValuePath(treeMenu1, valuePath);
                    log.Debug("called CTreeview.ExpandToValuePath for value path: " + valuePath);


                    TreeNode nodeToSelect = treeMenu1.FindNode(valuePath);
                    if (nodeToSelect != null)
                    {
                        try
                        {
                            nodeToSelect.Selected = true;
                            didSelect = true;
                            log.Debug("selected node " + nodeToSelect.Text);
                        }
                        catch (InvalidOperationException)
                        {
                            //can happen if node disabled or unselectable
                        }
                    }

                    if (!didSelect)
                    {
                        valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset);
                        CTreeView.ExpandToValuePath(treeMenu1, valuePath);
                        log.Debug("called CTreeview.ExpandToValuePath for value path: " + valuePath);


                        nodeToSelect = treeMenu1.FindNode(valuePath);
                        if (nodeToSelect != null)
                        {
                            try
                            {
                                nodeToSelect.Selected = true;
                                didSelect = true;
                                log.Debug("selected node " + nodeToSelect.Text);
                            }
                            catch (InvalidOperationException)
                            {
                                //can happen if node disabled or unselectable
                            }
                        }

                    }

                }
            }

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
            if (e.Node.Parent != null)
            {
                CTreeView.ExpandToValuePath(treeView, e.Node.Parent.ValuePath);
            }
        }

        protected void treeMenu1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            TreeView menu = (TreeView)sender;
            CSiteMapNode mapNode = (CSiteMapNode)e.Node.DataItem;
            
            if (mapNode.MenuImage.Length > 0)
            {
                e.Node.ImageUrl = mapNode.MenuImage;
            }

            if (e.Node is CTreeNode)
            {
                CTreeNode tn = e.Node as CTreeNode;
                tn.HasVisibleChildren = mapNode.HasVisibleChildren();

            }


            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if ((!mapNode.UseSsl) && (!siteSettings.UseSslOnAllPages) && (mapNode.Url.StartsWith("~/")))
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
            //e.Node.Value = mapNode.Url;
            e.Node.Value = mapNode.PageGuid.ToString();

            bool remove = false;

            if (!
                    (
                        (isAdmin)
                        || 
                        (
                        (isContentAdmin)
                        && (mapNode.Roles != null)
                        && (!(mapNode.Roles.Count == 1)
                        && (mapNode.Roles[0].ToString() == "Admins")
                        )
                     )
                    || ((isContentAdmin) && (mapNode.Roles == null))
                    || 
                    (
                        (mapNode.Roles != null)
                        && (WebUser.IsInRoles(mapNode.Roles))
                    )
                    
                )
                 
                )
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
#if !MONO
                if (mapNode.HasChildNodes)
                {
                    e.Node.PopulateOnDemand = treeViewPopulateOnDemand;
                }
#endif
            }
            
        }

        #endregion


        
		
	}
}
