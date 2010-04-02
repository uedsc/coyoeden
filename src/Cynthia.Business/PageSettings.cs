// Author:				Joe Audette
// Created:			    2004-09-06
// Last Modified:		2009-07-15
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
using System.Collections;
using Cynthia.Data;

namespace Cynthia.Business
{
	/// <summary>
	/// Represents a page in the content management system.
	/// </summary>
	public class PageSettings : IComparable 
	{
		#region Constructors

		public PageSettings()
		{
            
        }

        public PageSettings(Guid pageGuid)
        {
            if (pageGuid == null) { return; }
            if (pageGuid == Guid.Empty) { return; }

            GetPage(pageGuid);

        }

        public PageSettings(int siteId, int pageId)
        {
            if (siteId > -1)
            {
                GetPage(siteId, pageId);
            }

        }


		#endregion

		#region Private Properties

		private int pageID = -1;
		private int	parentID = -1;
		private int siteID = 0;
		private int pageIndex;
		private int pageOrder = 0;
        private string pageName = string.Empty;
        private string pageTitle = String.Empty;
		private string skin = string.Empty;
		private bool requireSSL = false;
		private bool showBreadcrumbs = false;
		private bool showChildPageBreadcrumbs = false;
        private string authorizedRoles = string.Empty;
		private string editRoles = string.Empty;
        private string draftEditOnlyRoles = string.Empty;
		private string createChildPageRoles = string.Empty;
        private string createChildDraftRoles = string.Empty;
		private string pageMetaKeyWords = string.Empty;
		private string pageMetaDescription = string.Empty;
		private string pageMetaEncoding = string.Empty;
		private string pageMetaAdditional = string.Empty;
		private bool useUrl = true;
		private string url = string.Empty;
		private bool openInNewWindow = false;
		private bool showChildPageMenu = false;
		private bool hideMainMenu = false;
		private ArrayList modules = new ArrayList();
		private string indexPath = string.Empty;
        private bool includeInMenu = true;
        private String menuImage = String.Empty;
        private bool allowBrowserCache = true;

        private PageChangeFrequency changeFrequency = PageChangeFrequency.Daily;
        private string siteMapPriority = "0.5";
        private DateTime lastModifiedUTC = DateTime.UtcNow;

        private Guid pageGuid = Guid.Empty;
        private Guid parentGuid = Guid.Empty;
        private string depthIndicator = string.Empty;
        private bool hideAfterLogin = false;

        private Guid siteGuid = Guid.Empty;
        private string compiledMeta = string.Empty;
        private DateTime compiledMetaUtc = DateTime.UtcNow;

        private bool includeInSiteMap = true;
        private bool isClickable = true;
        private bool showHomeCrumb = false;
        private bool urlHasBeenAdjustedForFolderSites = false;

        private bool isPending = false;
        private string canonicalOverride = string.Empty;
        private bool includeInSearchMap = true;
        private bool enableComments = false;

        
        
       
		#endregion

		#region Public Properties

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public string CompiledMeta
        {
            get { return compiledMeta; }
            set { compiledMeta = value; }
        }

        public DateTime CompiledMetaUtc
        {
            get { return compiledMetaUtc; }
            set { compiledMetaUtc = value; }
        } 
		
		public ArrayList Modules 
		{
			get {return modules;}
			set {modules = value;}
		}

        public Guid PageGuid
        {
            get { return pageGuid; }
            set { pageGuid = value; }
        } 

		public int PageId 
		{
			get {return pageID;}
			set {pageID = value;}
		}

        public Guid ParentGuid
        {
            get { return parentGuid; }
            set { parentGuid = value; }
        } 

		public int ParentId 
		{
			get {return parentID;}
			set {parentID = value;}
		} 

		public int SiteId 
		{
			get {return siteID;}
			set {siteID = value;}
		} 

		public int PageIndex 
		{
			get {return pageIndex;}
			set {pageIndex = value;}
		} 

		public int PageOrder 
		{
			get {return pageOrder;}
			set {pageOrder = value;}
		}    

		public string PageName 
		{
			get {return pageName;}
			set 
            {
                // don't allow | in page name as this is used
                // as separator character in valuePath
                // which allows finding noeds withing the menu or treeview
                //pageName = value.ToString().Replace("|", string.Empty);
                // 2009-03-21 the above is no longer needed because the
                // valuepath in treeview menus now uses the guid for value instead of page name
                pageName = value;
            }
		}

        public string PageTitle
        {
            get { return pageTitle; }
            set { pageTitle = value; }
        }

		public string Skin 
		{
			get {return skin;}
			set {skin = value;}
		}

		public string AuthorizedRoles 
		{
			get {return authorizedRoles;}
			set {authorizedRoles = value;}
		}

		public string EditRoles 
		{
			get {return editRoles;}
			set {editRoles = value;}
		}

        public string DraftEditOnlyRoles
        {
            get { return draftEditOnlyRoles; }
            set { draftEditOnlyRoles = value; }
        }

		public string CreateChildPageRoles 
		{
			get {return createChildPageRoles;}
			set {createChildPageRoles = value;}
		}

        public string CreateChildDraftRoles
        {
            get { return createChildDraftRoles; }
            set { createChildDraftRoles = value; }
        }

       
		public bool RequireSsl 
		{
			get {return requireSSL;}
			set {requireSSL = value;}
		}

        public bool AllowBrowserCache
        {
            get { return allowBrowserCache; }
            set { allowBrowserCache = value; }
        } 

		public bool ShowBreadcrumbs 
		{
			get {return showBreadcrumbs;}
			set {showBreadcrumbs = value;}
		} 

		public bool ShowChildPageBreadcrumbs 
		{
			get {return showChildPageBreadcrumbs;}
			set {showChildPageBreadcrumbs = value;}
		} 

		public bool HideMainMenu 
		{
			get {return hideMainMenu;}
			set {hideMainMenu = value;}
		}

        public bool HideAfterLogin
        {
            get { return hideAfterLogin; }
            set { hideAfterLogin = value; }
        }

        public bool EnableComments
        {
            get { return enableComments; }
            set { enableComments = value; }
        } 

		public bool UseUrl 
		{
			get {return useUrl;}
			set {useUrl = value;}
		} 

		public string Url 
		{
			get {return url;}
			set {url = value;}
		}

		public bool OpenInNewWindow 
		{
			get {return openInNewWindow;}
			set {openInNewWindow = value;}
		} 

		public bool ShowChildPageMenu 
		{
			get {return showChildPageMenu;}
			set {showChildPageMenu = value;}
		} 


		public string PageMetaKeyWords 
		{
			get {return pageMetaKeyWords;}
			set {pageMetaKeyWords = value;}
		}

		public string PageMetaDescription 
		{
			get {return pageMetaDescription;}
			set {pageMetaDescription = value;}
		}

		public string PageMetaEncoding 
		{
			get {return pageMetaEncoding;}
			set {pageMetaEncoding = value;}
		}

		public string PageMetaAdditional 
		{
			get {return pageMetaAdditional;}
			set {pageMetaAdditional = value;}
		}

		public string IndexPath 
		{
			get { return indexPath; }
			set { indexPath = value; }
		}

        public bool IncludeInMenu
        {
            get { return includeInMenu; }
            set { includeInMenu = value; }
        }

        public string MenuImage
        {
            get { return menuImage; }
            set { menuImage = value; }
        }

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

        public string DepthIndicator
        {
            get { return depthIndicator; }
            set { depthIndicator = value; }
        }

        public bool IncludeInSiteMap
        {
            get { return includeInSiteMap; }
            set { includeInSiteMap = value; }
        }

        public bool IsClickable
        {
            get { return isClickable; }
            set { isClickable = value; }
        }

        public bool ShowHomeCrumb
        {
            get { return showHomeCrumb; }
            set { showHomeCrumb = value; }
        }

        public bool UrlHasBeenAdjustedForFolderSites
        {
            get { return urlHasBeenAdjustedForFolderSites; }
            set { urlHasBeenAdjustedForFolderSites = value; }
        }

        public bool IsPending
        {
            get { return isPending; }
            set { isPending = value; }
        }

        public string CanonicalOverride
        {
            get { return canonicalOverride; }
            set { canonicalOverride = value; }
        }

        public bool IncludeInSearchMap
        {
            get { return includeInSearchMap; }
            set { includeInSearchMap = value; }
        }

		#endregion

		#region IComparable

		public int CompareTo(object value) 
		{

			if (value == null) return 1;

			int compareOrder = ((PageSettings)value).PageOrder;
            
			if (this.pageOrder == compareOrder) return 0;
			if (this.pageOrder < compareOrder) return -1;
			if (this.pageOrder > compareOrder) return 1;
			return 0;
		}

		#endregion

		#region Private Methods

        

		private void GetPage(int siteId, int pageId)
		{
            using(IDataReader reader = DBPageSettings.GetPage(siteId, pageId))
            {
                LoadFromReader(reader);

                
            }
            
		}

        private void GetPage(Guid pageGuid)
        {
            using (IDataReader reader = DBPageSettings.GetPage(pageGuid))
            {
                LoadFromReader(reader);

            }
        }

        private void LoadFromReader(IDataReader reader)
        {
           
            if (reader.Read())
            {
                this.pageID = int.Parse(reader["PageID"].ToString());
                this.parentID = int.Parse(reader["ParentID"].ToString());
                this.siteID = int.Parse(reader["SiteID"].ToString());
                this.pageOrder = int.Parse(reader["PageOrder"].ToString());
                this.pageName = reader["PageName"].ToString();
                this.pageTitle = reader["PageTitle"].ToString();
                this.menuImage = reader["MenuImage"].ToString();
                this.skin = reader["Skin"].ToString();
                this.authorizedRoles = reader["AuthorizedRoles"].ToString();
                this.editRoles = reader["EditRoles"].ToString();
                this.draftEditOnlyRoles = reader["DraftEditRoles"].ToString();
                this.createChildPageRoles = reader["CreateChildPageRoles"].ToString();
                this.createChildDraftRoles = reader["CreateChildDraftRoles"].ToString();

                this.requireSSL = Convert.ToBoolean(reader["RequireSSL"]);
                this.allowBrowserCache = Convert.ToBoolean(reader["AllowBrowserCache"]);
                this.showBreadcrumbs = Convert.ToBoolean(reader["ShowBreadcrumbs"]);
                this.showChildPageBreadcrumbs = Convert.ToBoolean(reader["ShowChildBreadCrumbs"]);
                this.useUrl = Convert.ToBoolean(reader["UseUrl"]);
                this.url = reader["Url"].ToString();
                this.openInNewWindow = Convert.ToBoolean(reader["OpenInNewWindow"]);
                this.showChildPageMenu = Convert.ToBoolean(reader["ShowChildPageMenu"]);
                this.hideMainMenu = Convert.ToBoolean(reader["HideMainMenu"]);

                this.pageMetaKeyWords = reader["PageKeyWords"].ToString();
                this.pageMetaDescription = reader["PageDescription"].ToString();
                this.pageMetaEncoding = reader["PageEncoding"].ToString();
                this.pageMetaAdditional = reader["AdditionalMetaTags"].ToString();
                this.includeInMenu = Convert.ToBoolean(reader["IncludeInMenu"]);

                string cf = reader["ChangeFrequency"].ToString();
                switch (cf)
                {
                    case "Always":
                        this.changeFrequency = PageChangeFrequency.Always;
                        break;

                    case "Hourly":
                        this.changeFrequency = PageChangeFrequency.Hourly;
                        break;

                    case "Daily":
                        this.changeFrequency = PageChangeFrequency.Daily;
                        break;

                    case "Monthly":
                        this.changeFrequency = PageChangeFrequency.Monthly;
                        break;

                    case "Yearly":
                        this.changeFrequency = PageChangeFrequency.Yearly;
                        break;

                    case "Never":
                        this.changeFrequency = PageChangeFrequency.Never;
                        break;

                    case "Weekly":
                    default:
                        this.changeFrequency = PageChangeFrequency.Weekly;
                        break;


                }

                string smp = reader["SiteMapPriority"].ToString().Trim();
                if (smp.Length > 0) this.siteMapPriority = smp;

                if (reader["LastModifiedUTC"] != DBNull.Value)
                {
                    this.lastModifiedUTC = Convert.ToDateTime(reader["LastModifiedUTC"]);
                }

                this.pageGuid = new Guid(reader["PageGuid"].ToString());
                this.parentGuid = new Guid(reader["ParentGuid"].ToString());
                this.hideAfterLogin = Convert.ToBoolean(reader["HideAfterLogin"]);
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.compiledMeta = reader["CompiledMeta"].ToString();
                if (reader["CompiledMetaUtc"] != DBNull.Value)
                {
                    this.compiledMetaUtc = Convert.ToDateTime(reader["CompiledMetaUtc"]);
                }

                this.includeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]);
                this.isClickable = Convert.ToBoolean(reader["IsClickable"]);
                this.showHomeCrumb = Convert.ToBoolean(reader["ShowHomeCrumb"]);
                this.isPending = Convert.ToBoolean(reader["IsPending"]);

                this.includeInSearchMap = Convert.ToBoolean(reader["IncludeInSearchMap"]);
                this.canonicalOverride = reader["CanonicalOverride"].ToString();
                this.enableComments = Convert.ToBoolean(reader["EnableComments"]);
            }
           

        }

		private IDataReader GetChildPages() 
		{
			return DBPageSettings.GetChildPages(this.siteID, this.parentID);
		}

        private DataTable GetChildPageIds()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageID", typeof(int));
            using (IDataReader reader = GetChildPages())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["PageID"] = reader["PageID"];

                    dt.Rows.Add(row);
                }
            }
            
            return dt;

        }

		private bool Create()
		{ 
			int newID = -1;

            if (this.pageGuid == Guid.Empty) this.pageGuid = Guid.NewGuid();
			
			newID = DBPageSettings.Create(
				this.siteID,
				this.parentID,
				this.pageName,
                this.pageTitle,
				this.skin,
				this.pageOrder,
				this.authorizedRoles,
				this.editRoles,
                this.draftEditOnlyRoles,
				this.createChildPageRoles,
                this.createChildDraftRoles,
				this.requireSSL,
                this.allowBrowserCache,
				this.showBreadcrumbs,
				this.showChildPageBreadcrumbs,
				this.pageMetaKeyWords,
				this.pageMetaDescription,
				this.pageMetaEncoding,
				this.pageMetaAdditional,
				this.useUrl,
				this.url,
				this.openInNewWindow,
				this.showChildPageMenu,
				this.hideMainMenu,
                this.includeInMenu,
                this.menuImage,
                this.changeFrequency.ToString(),
                this.siteMapPriority,
                this.pageGuid,
                this.parentGuid,
                this.hideAfterLogin,
                this.siteGuid,
                this.compiledMeta,
                this.compiledMetaUtc,
                this.includeInSiteMap,
                this.isClickable,
                this.showHomeCrumb,
                this.isPending,
                this.canonicalOverride,
                this.includeInSearchMap,
                this.enableComments);

			this.pageID = newID;

            bool result = (newID > -1);

            
            if (result)
            {
                PageCreatedEventArgs e = new PageCreatedEventArgs();
                OnPageCreated(e);
            }
					
			return result;
		}

		private bool Update()
		{

			return DBPageSettings.UpdatePage(
				this.siteID,
				this.pageID,
				this.parentID,
				this.pageName,
                this.pageTitle,
				this.skin,
				this.pageOrder,
				this.authorizedRoles,
				this.editRoles,
                this.draftEditOnlyRoles,
				this.createChildPageRoles,
                this.createChildDraftRoles,
				this.requireSSL,
                this.allowBrowserCache,
				this.showBreadcrumbs,
				this.showChildPageBreadcrumbs,
				this.pageMetaKeyWords,
				this.pageMetaDescription,
				this.pageMetaEncoding,
				this.pageMetaAdditional,
				this.useUrl,
				this.url,
				this.openInNewWindow,
				this.showChildPageMenu,
				this.hideMainMenu,
                this.includeInMenu,
                this.menuImage,
                this.changeFrequency.ToString(),
                this.siteMapPriority,
                this.parentGuid,
                this.hideAfterLogin,
                this.compiledMeta,
                this.compiledMetaUtc,
                this.includeInSiteMap,
                this.isClickable,
                this.showHomeCrumb,
                this.isPending,
                this.canonicalOverride,
                this.includeInSearchMap,
                this.enableComments);
		}


        public void RefreshModules()
        {
            this.modules.Clear();
            using (IDataReader reader = Module.GetPageModules(this.pageID))
            {
                while (reader.Read())
                {
                    Module m = new Module();
                    m.ModuleId = Convert.ToInt32(reader["ModuleID"]);
                    m.ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]);
                    m.PageId = Convert.ToInt32(reader["PageID"]);
                    m.PaneName = reader["PaneName"].ToString();
                    m.ModuleTitle = reader["ModuleTitle"].ToString();
                    m.ViewRoles = reader["ViewRoles"].ToString();
                    m.AuthorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
                    m.CacheTime = Convert.ToInt32(reader["CacheTime"]);
                    m.ModuleOrder = Convert.ToInt32(reader["ModuleOrder"]);
                    if (reader["EditUserID"] != DBNull.Value)
                    {
                        m.EditUserId = Convert.ToInt32(reader["EditUserID"]);
                    }

                    string showTitle = reader["ShowTitle"].ToString();
                    m.ShowTitle = (showTitle == "True" || showTitle == "1");
                    m.ControlSource = reader["ControlSrc"].ToString();

                    m.HideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
                    m.HideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);


                    this.modules.Add(m);
                }
            }


        }

		
		#endregion

		#region Public Methods

		

		public bool Save()
		{
			if(this.pageID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
			
		}



		public void MoveUp()
		{
			//if there are pages with the same parentID and a lower sort than me
			//subtract 3 from my sort and save it, then reset sort order on
			//all children of my parent by 2 starting from 1

			//no need to look up whther pages above me, assume yes if my sort is > 1
			if(this.pageOrder > 1)
			{
				//pages exist above me so update my page order
				this.pageOrder -= 3;
				PageSettings.UpdatePageOrder(this.pageID, this.pageOrder);
				//now get all children of my parent id and reset sort using current order
				ResortPages();
				
			}
			else
			{
				if(this.pageOrder < 1)
				{
					this.pageOrder = 1;
					PageSettings.UpdatePageOrder(this.pageID, this.pageOrder);
					ResortPages();
					
				}
				//else must be equal to 1 no need to do anything
				
			}

		}

		public void MoveDown()
		{
			this.pageOrder += 3;
			PageSettings.UpdatePageOrder(this.pageID, this.pageOrder);
			//now get all children of my parent id and reset sort using current order
			ResortPages();
				
		}

		public void ResortPages()
		{
			int i = 1;
            DataTable table = GetChildPageIds();

			foreach(DataRow row in table.Rows)
			{
				int pageID = Convert.ToInt32(row["PageID"]);
				PageSettings.UpdatePageOrder(pageID, i);
				i += 2;
			}	
		}

        public bool ContainsModule(int moduleId)
        {
            bool result = false;
            
            foreach (Module m in modules)
            {
                if (m.ModuleId == moduleId) result = true;
            }

            return result;
        }

        public bool ContainsModuleInProgress()
        {
            int count = ContentWorkflow.GetWorkInProgressCountByPage(this.pageGuid);
            return (count > 0);
        }

        public string ResolveUrl(SiteSettings siteSettings)
        {
            if (siteSettings == null) return url;
            string resolvedUrl;
            if (this.useUrl)
            {
                if((this.url.StartsWith("~/"))&&(this.url.EndsWith(".aspx")))
                {
                    if (urlHasBeenAdjustedForFolderSites)
                    {
                        resolvedUrl = url.Replace("~/", "/");
                    }
                    else
                    {
                        resolvedUrl = siteSettings.SiteRoot
                            + url.Replace("~/", "/");
                    }

                }
                else
                {
                    resolvedUrl = url;
                }

            }
            else
            {
                resolvedUrl = siteSettings.SiteRoot 
                    + "/Default.aspx?pageid=" 
                    + this.pageID.ToString();
            }

            return resolvedUrl;

        }

        public bool UpdateLastModifiedTime()
        {
            if (this.pageID == -1) return false;
            return UpdateTimestamp(this.pageID, DateTime.UtcNow);

        }

		#endregion

		#region Static Methods

        public static IDataReader GetPageList(int siteId)
        {
            return DBPageSettings.GetPageList(siteId);
        }

        public static IDataReader GetPendingPageListPage(Guid siteGuid, int pageNumber, int pageSize, out int totalPages)
        {
            return DBPageSettings.GetPendingPageListPage(siteGuid, pageNumber, pageSize, out totalPages);
        }

		public static bool UpdatePageOrder(int pageId, int pageOrder)
		{
			return DBPageSettings.UpdatePageOrder(pageId,pageOrder);
		}

        public static bool UpdateTimestamp(
            int pageId,
            DateTime lastModifiedUtc)
        {
            return DBPageSettings.UpdateTimestamp(pageId, lastModifiedUtc);
        }

		public static bool DeletePage(int pageId)
		{
			bool result =  DBPageSettings.DeletePage(pageId);
            DBPageSettings.CleanupOrphans();
            return result;
		}

        public static int GetCountOfPages(int siteId)
        {
            int count = 0;
            using (IDataReader reader = GetPageList(siteId))
            {
                while (reader.Read())
                {
                    count += 1;
                }
            }

            return count;
        }

        
        
		public static int GetNextPageOrder(int siteId, int parentId) 
		{
			return DBPageSettings.GetNextPageOrder(siteId, parentId);
		}

        //public static IDataReader GetChildPagesByPageId(int pageId) 
        //{
        //    return DBPageSettings.GetChildPages(pageId);
        //}

        public static bool Exists(Guid pageGuid)
        {
            bool result = false;
            using (IDataReader reader = DBPageSettings.GetPage(pageGuid))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;

        }

        

		#endregion

        #region Events

        public event PageCreatedEventHandler PageCreated;

        protected void OnPageCreated(PageCreatedEventArgs e)
        {
            if (PageCreated != null)
            {
                PageCreated(this, e);
            }
        }

        #endregion

    }
}
