
using System;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web
{
    /// <summary>
    /// Purpose: extends SiteMapNode with MenuImage property to enable images in the menu
    /// </summary>
    public class CSiteMapNode : SiteMapNode
    {
        private String menuImage = String.Empty;
        private bool isRootNode = false;
        //private PageSettings pageSettings = null;

        //public PageSettings Settings
        //{
        //    get { return pageSettings; }
        //    set { pageSettings = value; }
        //}

        private int depth = 0;

        private bool includeInMenu = false;
        private bool includeInSiteMap = true;
        private bool includeInSearchMap = true;
        private Guid pageGuid = Guid.Empty;
        private int pageID = -1;
        private int parentID = -1;
        private string viewRoles = string.Empty;
        private string editRoles = string.Empty;
        private string createChildPageRoles = string.Empty;
        private string depthIndicator = string.Empty;
        private PageChangeFrequency changeFrequency = PageChangeFrequency.Daily;
        private string siteMapPriority = "0.5";
        private DateTime lastModifiedUTC = DateTime.MinValue;
        private bool openInNewWindow = false;
        private bool hideAfterLogin = false;
        private bool useSsl = false;
        private bool isPending = false;


        


        public PageChangeFrequency ChangeFrequency
        {
            get { return changeFrequency; }
            set { changeFrequency = value; }
        }

        public string SiteMapPriority
        {
            get { return siteMapPriority; }
            set { siteMapPriority = value; }
        }

        public DateTime LastModifiedUtc
        {
            get { return lastModifiedUTC; }
            set { lastModifiedUTC = value; }
        }

        public String DepthIndicator
        {
            get { return depthIndicator; }
            set { depthIndicator = value; }
        }

        public String ViewRoles
        {
            get { return viewRoles; }
            set { viewRoles = value; }
        }

        public String EditRoles
        {
            get { return editRoles; }
            set { editRoles = value; }
        }

        public String CreateChildPageRoles
        {
            get { return createChildPageRoles; }
            set { createChildPageRoles = value; }
        }

        public int PageId
        {
            get { return pageID; }
            set { pageID = value; }
        }

        public int ParentId
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public Guid PageGuid
        {
            get { return pageGuid; }
            set { pageGuid = value; }
        }

        public bool IsRootNode
        {
            get { return isRootNode; }
            set { isRootNode = value; }
        }

        public bool IncludeInMenu
        {
            get { return includeInMenu; }
            set { includeInMenu = value; }
        }

        public bool IncludeInSiteMap
        {
            get { return includeInSiteMap; }
            set { includeInSiteMap = value; }
        }

        public bool IncludeInSearchMap
        {
            get { return includeInSearchMap; }
            set { includeInSearchMap = value; }
        }

        public bool HideAfterLogin
        {
            get { return hideAfterLogin; }
            set { hideAfterLogin = value; }
        }

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        

        public String MenuImage
        {
            get { return menuImage; }
            set { menuImage = value; }
        }

        public bool OpenInNewWindow
        {
            get { return openInNewWindow; }
            set { openInNewWindow = value; }
        }

        public bool UseSsl
        {
            get { return useSsl; }
            set { useSsl = value; }
        }

        public bool IsPending
        {
            get { return isPending; }
            set { isPending = value; }
        }

        public bool HasVisibleChildren()
        {
            if (this.ChildNodes == null) { return false; }
            if (this.ChildNodes.Count == 0) { return false; }
            foreach (SiteMapNode child in ChildNodes)
            {
                if (child is CSiteMapNode)
                {
                    CSiteMapNode mchild = child as CSiteMapNode;
                    if ((mchild.IncludeInMenu)&& (WebUser.IsInRoles(mchild.ViewRoles))){ return true; }
                }

            }

            return false;

        }
        

        public CSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url,
            string title,
            string description,
            IList roles,
            NameValueCollection attributes,
            NameValueCollection explicitResourceKeys,
            string implicitResourceKey):base( provider, key,
                url,
                title,
                description,
                roles,
                attributes,
                explicitResourceKeys,
                implicitResourceKey)
        {

        }


        public CSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url,
            string title,
            string description):base(provider,key,url,title,description)
        {

        }

        public CSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url,
            string title)
            : base(provider, key, url, title)
        {

        }

        public CSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url)
            : base(provider, key, url)
        {
            
        }

        public CSiteMapNode(SiteMapProvider provider, string key)
            : base(provider, key)
        {

        }
        

        

    }
}
