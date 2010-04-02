// Author:             Joe Audette
// Created:            2006-01-21
// Last Modified:      2010-01-30

using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Web.Caching;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using log4net;

namespace Cynthia.Web
{
    
    public class CSiteMapProvider : StaticSiteMapProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CSiteMapProvider));

        private const string CacheDependencyName = "siteMapCacheDependency";
        private readonly object objLock = new object();
        private SiteMapNode rootNode;
        private Dictionary<int, SiteMapNode> nodes = new Dictionary<int, SiteMapNode>(16);
        private String iconBaseUrl = "~/Data/SiteImages/FeatureIcons/";
        private Collection<PageSettings> menuPages;
        private bool useUrlRewriter = true;
        private bool sslIsAvailable = false;
        
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "CSiteMapProvider";
            }

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Cynthia site map provider");
            }

            useUrlRewriter = WebConfigSettings.UseUrlReWriting;
            sslIsAvailable = SiteUtils.SslIsAvailable();

            base.Initialize(name, config);
            
        }

        public override SiteMapNode BuildSiteMap()
        {
            lock (this)
            {
                if (rootNode != null)
                {
                    return rootNode;
                }

                menuPages = CacheHelper.GetMenuPages();
                int depth = 0;
                if (menuPages != null)
                {
                    rootNode = CreateRootNode();
                    //int i = 0;
                    foreach (PageSettings page in menuPages)
                    {
                        if (page.ParentId <= -1)
                        {
                            SiteMapNode node = CreateSiteMapNode(page, depth);
                            
                            try
                            {
                                AddNode(node, GetParentNode(page));
                            }
                            catch (InvalidOperationException ex)
                            {
                                log.Error("failed to add node to sitemap", ex);

                            }
                            catch (HttpException ex)
                            {
                                log.Error("failed to add node to sitemap", ex);

                            }

                            if ((page.UseUrl) && (page.Url.StartsWith("http")))
                            {
                                node.Url = page.Url;
                            }
                        }

                       // i += 1;
                    }
                }
            }

            CacheDependency cacheDependency = new CacheDependency(
                CacheHelper.GetPathToSiteMapCacheDependencyFile());

            HttpRuntime.Cache.Insert(
                CacheDependencyName,
                new object(),
                cacheDependency,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
               new CacheItemRemovedCallback(OnSiteMapChanged));

            return rootNode;
        }

        private void CreateChildNodes(
            CSiteMapNode node, 
            PageSettings page,
            int depth)
        {
            
            foreach (PageSettings p in menuPages)
            {
                if (p.ParentId == page.PageId)
                {
                    SiteMapNode childNode = CreateSiteMapNode(p, depth);
                    try
                    {
                        AddNode(childNode, node);
                    }
                    catch (InvalidOperationException ex)
                    {
                        log.Error("failed to add node to sitemap", ex);

                    }
                    catch (HttpException ex)
                    {
                        log.Error("failed to add node to sitemap", ex);

                    }

                    if ((p.UseUrl) && (p.Url.StartsWith("http")))
                    {
                        childNode.Url = p.Url;
                    }
                }
            }
        }


        private SiteMapNode CreateSiteMapNode(
            PageSettings page,
            int depth)
        {
            string[] rolelist = null;
            if (!String.IsNullOrEmpty(page.AuthorizedRoles))
            {
                rolelist = page.AuthorizedRoles.Split(new char[] { ',', ';' }, 512);
            }

            string pageUrl;
            if (
                (page.UseUrl)
                &&(!page.Url.StartsWith("http"))
                &&(page.Url.Length > 0)
                &&(useUrlRewriter)
                )
            {

                pageUrl = page.Url ;
            }
            else
            {
               pageUrl = "~/Default.aspx?pageid=" + page.PageId.ToString();
            }

            // this was making a title (tooltip) on the link with the same text as the link text
            // not a good idea, adds no value and actually can make the page obnoxious for a screen reader user as it
            // would read the link text and the title
            //CSiteMapNode node = new CSiteMapNode(
            //    this,
            //    page.PageId.ToString(),
            //    pageUrl,
            //    HttpContext.Current.Server.HtmlEncode(page.PageName),
            //    HttpContext.Current.Server.HtmlEncode(page.PageName),
            //    rolelist, 
            //    null, 
            //    null, 
            //    null);

            CSiteMapNode node = new CSiteMapNode(
                this,
                page.PageId.ToString(),
                pageUrl,
                HttpContext.Current.Server.HtmlEncode(page.PageName),
                string.Empty,
                rolelist,
                null,
                null,
                null);

            if((page.MenuImage.Length > 0)&&(page.MenuImage.ToLower().IndexOf("blank") == -1))
            {
                node.MenuImage = this.iconBaseUrl + page.MenuImage;
            }

            //node.Settings = page;

            node.PageGuid = page.PageGuid;
            node.PageId = page.PageId;
            node.ParentId = page.ParentId;
            node.Depth = depth;
            node.ViewRoles = page.AuthorizedRoles;
            node.EditRoles = page.EditRoles;
            node.CreateChildPageRoles = page.CreateChildPageRoles;
            node.IncludeInMenu = page.IncludeInMenu;
            node.IncludeInSiteMap = page.IncludeInSiteMap;
            node.IncludeInSearchMap = page.IncludeInSearchMap;
            node.LastModifiedUtc = page.LastModifiedUtc;
            node.ChangeFrequency = page.ChangeFrequency;
            node.SiteMapPriority = page.SiteMapPriority;
            node.OpenInNewWindow = page.OpenInNewWindow;
            node.HideAfterLogin = page.HideAfterLogin;
            node.UseSsl = (page.RequireSsl && sslIsAvailable);
            node.IsPending = page.IsPending;

            //node.PageID = page.PageID;
            //node.ParentID = page.ParentID;
            //node.ViewRoles = page.AuthorizedRoles;
            //node.EditRoles = page.EditRoles;
            //node.CreateChildPageRoles = page.CreateChildPageRoles;

            if(!this.nodes.ContainsKey(page.PageId))
            {
                nodes.Add(page.PageId, node);
            }

            //this.CreateChildNodes(node, page, pageIndex);
            this.CreateChildNodes(node, page, depth + 1);
   
            return node;
        }

        private SiteMapNode GetParentNode(PageSettings page)
        {
            if (page.ParentId <= -1)
            {
                return this.rootNode;

            }

            if (nodes.ContainsKey(page.ParentId))
            {
                return nodes[page.ParentId];
            }

            return this.rootNode;

        }

        private CSiteMapNode CreateRootNode()
        {
            string[] rolelist = new string[0];

            CSiteMapNode node = new CSiteMapNode(
                this,
                "-1");
            node.IsRootNode = true;

            nodes.Add(-1, node);

            return node;

        }


        protected override SiteMapNode GetRootNodeCore()
        {
             BuildSiteMap();
             return rootNode;  
        }

        protected override void Clear()
        {
            lock (objLock)
            {
                this.rootNode = null;
                this.nodes.Clear();
                base.Clear();
            }
        }


        public void OnSiteMapChanged(string key, object item, CacheItemRemovedReason reason)
        {
            Clear();

        }

        public static void PopulateArrayList(
            ArrayList arrayList,
            SiteMapNode siteMapNode)
        {
            CSiteMapNode CNode = (CSiteMapNode)siteMapNode;

            if (!CNode.IsRootNode)
            {
                CNode.DepthIndicator = GetDepthIndicatorString(CNode.Depth);
                arrayList.Add(CNode);
                
            }

            foreach (SiteMapNode childNode in CNode.ChildNodes)
            {
                //recurse to populate children
                PopulateArrayList(arrayList, childNode);

            }


        }

        private static string GetDepthIndicatorString(int depth)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                stringBuilder.Append("-");
            }
            return stringBuilder.ToString();
        }

    }
}
