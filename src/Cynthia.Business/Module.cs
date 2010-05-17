

using System;
using System.Data;
using Cynthia.Data;

namespace Cynthia.Business
{
	/// <summary>
	/// Represents an instance of a feature
	/// </summary>
	public class Module : IComparable 
	{

		#region Constructors

		public Module()
		{}

        public Module(Guid moduleGuid)
        {
            GetModule(moduleGuid);
        }


        public Module(int moduleId)
        {
            GetModule(moduleId);
        }

        public Module(int moduleId, int pageId)
        {
            this._pageId = pageId;
            GetModule(moduleId, pageId);
        }

		#endregion

		#region Private Properties

		private int _moduleId = -1;
        private Guid _guid = Guid.Empty;
        private Guid _featureGuid = Guid.Empty;
        private Guid _siteGuid = Guid.Empty;
        private int _siteId = 0;
		private int _pageId = -1; 
		private int _moduleDefId; 
		private int _moduleOrder = 999; 
		private string _paneName = String.Empty; 
		private string _moduleTitle = String.Empty;
        private string _viewRoles = "All Users;";
		private string _authorizedEditRoles = String.Empty;
        private string _draftEditRoles = String.Empty;
		private int _cacheTime = 0; 
		private bool _showTitle = true; 
		private string _controlSource = string.Empty;
		private int _editUserId = -1;
        private Guid _editUserGuid = Guid.Empty;
        private bool _availableForMyPage = false;
        private bool _allowMultipleInstancesOnMyPage = true;
        private String _icon = String.Empty;
        private int _createdByUserId = -1;
        private DateTime _createdDate = DateTime.MinValue;
        private String _featureName = String.Empty;
        private bool _hideFromAuthenticated = false;
        private bool _hideFromUnauthenticated = false;

        
        
      
		#endregion

		#region Public Properties

        public Guid ModuleGuid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        public Guid SiteGuid
        {
            get { return _siteGuid; }
            set { _siteGuid = value; }
        }

        public Guid FeatureGuid
        {
            get { return _featureGuid; }
            set { _featureGuid = value; }
        }

		public int ModuleId 
		{
			get { return _moduleId; }
			set { _moduleId = value; }
		}

        public int SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

		public int EditUserId 
		{
			get { return _editUserId; }
			set { _editUserId = value; }
		}

        public Guid EditUserGuid
        {
            get { return _editUserGuid; }
            set { _editUserGuid = value; }
        }

		public int PageId 
		{
			get { return _pageId; }
			set { _pageId = value; }
		}
		public int ModuleDefId 
		{
			get { return _moduleDefId; }
			set { _moduleDefId = value; }
		}
		public int ModuleOrder 
		{
			get { return _moduleOrder; }
			set { _moduleOrder = value; }
		}
		public string PaneName 
		{
			get { return _paneName; }
			set { _paneName = value; }
		}
		public string ModuleTitle 
		{
			get { return _moduleTitle; }
			set { _moduleTitle = value; }
		}

        public string ViewRoles
        {
            get { return _viewRoles; }
            set { _viewRoles = value; }
        }

		public string AuthorizedEditRoles 
		{
			get { return _authorizedEditRoles; }
			set { _authorizedEditRoles = value; }
		}

        public string DraftEditRoles
        {
            get { return _draftEditRoles; }
            set { _draftEditRoles = value; }
        }

		public int CacheTime 
		{
			get { return _cacheTime; }
			set { _cacheTime = value; }
		}
		public bool ShowTitle 
		{
			get { return _showTitle; }
			set { _showTitle = value; }
		}

		public string ControlSource 
		{
			get {return _controlSource;}
			set {_controlSource = value;}
		}

        public bool AvailableForMyPage
        {
            get { return _availableForMyPage; }
            set { _availableForMyPage = value; }
        }

        public bool AllowMultipleInstancesOnMyPage
        {
            get { return _allowMultipleInstancesOnMyPage; }
            set { _allowMultipleInstancesOnMyPage = value; }
        }

        public int CreatedByUserId
        {
            get { return _createdByUserId; }
            set { _createdByUserId = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
           
        }

        public string FeatureName
        {
            get { return this._featureName; }
           
        }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public bool HideFromAuthenticated
        {
            get { return _hideFromAuthenticated; }
            set { _hideFromAuthenticated = value; }
        }

        public bool HideFromUnauthenticated
        {
            get { return _hideFromUnauthenticated; }
            set { _hideFromUnauthenticated = value; }
        }

		#endregion

		#region Private Methods

        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this._moduleId = Convert.ToInt32(reader["ModuleID"]);
                this._siteId = Convert.ToInt32(reader["SiteID"]);
                //this.pageID = Convert.ToInt32(reader["PageID"]);
                this._moduleDefId = Convert.ToInt32(reader["ModuleDefID"]);
                //this.moduleOrder = Convert.ToInt32(reader["ModuleOrder"]);
                //this.paneName = reader["PaneName"].ToString();
                this._moduleTitle = reader["ModuleTitle"].ToString();
                this._authorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
                this._draftEditRoles = reader["DraftEditRoles"].ToString();
                this._cacheTime = Convert.ToInt32(reader["CacheTime"]);
                this._showTitle = Convert.ToBoolean(reader["ShowTitle"]);
                if (reader["EditUserID"] != DBNull.Value)
                {
                    this._editUserId = Convert.ToInt32(reader["EditUserID"]);
                }
                this._availableForMyPage = Convert.ToBoolean(reader["AvailableForMyPage"]);
                this._allowMultipleInstancesOnMyPage = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
                if (reader["CreatedByUserID"] != DBNull.Value)
                {
                    this._createdByUserId = Convert.ToInt32(reader["CreatedByUserID"]);
                }
                if (reader["CreatedDate"] != DBNull.Value)
                {
                    this._createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                }

                this._icon = reader["Icon"].ToString();


                this._guid = new Guid(reader["Guid"].ToString());
                this._featureGuid = new Guid(reader["FeatureGuid"].ToString());
                this._siteGuid = new Guid(reader["SiteGuid"].ToString());

                string edUserGuid = reader["EditUserGuid"].ToString();
                if (edUserGuid.Length == 36) this._editUserGuid = new Guid(edUserGuid);

                this._hideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
                this._hideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);

                this._viewRoles = reader["ViewRoles"].ToString();

                ModuleDefinition moduleDef = new ModuleDefinition(this._moduleDefId);
                this._controlSource = moduleDef.ControlSrc;
                this._featureName = moduleDef.FeatureName;

            }

        }

        private void GetModule(Guid moduleGuid)
        {
            using (IDataReader reader = DBModule.GetModule(moduleGuid))
            {
                PopulateFromReader(reader);
            }
        }

		private void GetModule(int moduleId) 
		{
            using (IDataReader reader = DBModule.GetModule(moduleId))
            {
                PopulateFromReader(reader);
            }
		
		}

        private void GetModule(int moduleId, int pageId)
        {
            using (IDataReader reader = DBModule.GetModule(moduleId, pageId))
            {
                PopulateFromReader(reader);

            }
        }


		#endregion

		#region Public Methods

		public int CompareTo(object value) 
		{

			if (value == null) return 1;

			int compareOrder = ((Module)value).ModuleOrder;
            
			if (this.ModuleOrder == compareOrder) return 0;
			if (this.ModuleOrder < compareOrder) return -1;
			if (this.ModuleOrder > compareOrder) return 1;
			return 0;
		}

		public bool Save()
		{
			if(this._moduleId > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}

		private bool Create()
		{ 
			bool created = false;
			int newID = -1;
            this._guid = Guid.NewGuid();

			newID = DBModule.AddModule(
				this._pageId, 
                this._siteId,
                this._siteGuid,
				this._moduleDefId, 
				this._moduleOrder, 
				this._paneName, 
				this._moduleTitle, 
                this._viewRoles,
				this._authorizedEditRoles,
                this._draftEditRoles,
				this._cacheTime, 
				this._showTitle,
                this._availableForMyPage,
                this._allowMultipleInstancesOnMyPage,
                this._icon,
                this._createdByUserId,
                DateTime.UtcNow,
                this._guid,
                this._featureGuid,
                this._hideFromAuthenticated,
                this._hideFromUnauthenticated); 
			
			this._moduleId = newID;
			created = (newID > -1);
			if(created)
			{
				ModuleSettings.CreateDefaultModuleSettings(this._moduleId);
			}
					
			return created;

		}

		private bool Update()
		{

			return DBModule.UpdateModule(
				this._moduleId, 
				this._moduleDefId, 
				this._moduleTitle, 
                this._viewRoles,
				this._authorizedEditRoles, 
                this._draftEditRoles,
				this._cacheTime, 
				this._showTitle,
				this._editUserId,
                this._availableForMyPage,
                this._allowMultipleInstancesOnMyPage,
                this._icon,
                this._hideFromAuthenticated,
                this._hideFromUnauthenticated); 
				
		}
		
		#endregion


		#region Static Methods

        /// <summary>
        /// Returns a DataReader of published pagemodules
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns></returns>
		public static IDataReader GetPageModules(int pageId) 
		{
			return DBModule.GetPageModules(pageId);
		}

        public static DataTable GetPageModulesTable(int moduleId)
        {
            return DBModule.PageModuleGetByModule(moduleId);
        }

        public static void DeletePageModules(int pageId)
        {
            DBModule.PageModuleDeleteByPage(pageId);
        }

        public static bool UpdateModuleOrder(int pageId, int moduleId, int moduleOrder, string paneName)
        {
            return DBModule.UpdateModuleOrder(pageId, moduleId, moduleOrder, paneName);
        }

		public static bool DeleteModule(int moduleId) 
		{
            
			ModuleSettings.DeleteModuleSettings(moduleId);
			return DBModule.DeleteModule(moduleId);
			
		}

        public static bool DeleteModuleInstance(int moduleId, int pageId)
        {
            return DBModule.DeleteModuleInstance(moduleId, pageId);
        }

        public static bool Publish(
            Guid pageGuid,
            Guid moduleGuid,
            int moduleId,
            int pageId,
            String paneName,
            int moduleOrder,
            DateTime publishBeginDate,
            DateTime publishEndDate)
        {
            return DBModule.Publish(
                pageGuid,
                moduleGuid,
                moduleId, 
                pageId, 
                paneName, 
                moduleOrder, 
                publishBeginDate, 
                publishEndDate);

        }

        public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
        {
            return DBModule.UpdatePage(oldPageId, newPageId, moduleId);
        }

        public static DataTable SelectPage(
            int siteId,
            int moduleDefId,
            string title,
            int pageNumber,
            int pageSize,
            bool sortByModuleType,
            bool sortByAuthor,
            out int totalPages)
        {
            return DBModule.SelectPage(
                siteId, 
                moduleDefId,
                title,
                pageNumber, 
                pageSize, 
                sortByModuleType,
                sortByAuthor,
                out totalPages);
        }


        public static IDataReader GetMyPageModules(int siteId)
        {
            return DBModule.GetMyPageModules(siteId);

        }

        public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
        {
            return DBModule.UpdateCountOfUseOnMyPage(moduleId, increment);
        }

        public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
        {
            return DBModule.GetModulesForSite(siteId, featureGuid);
        }


		#endregion


	}
	
}
