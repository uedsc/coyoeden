

using System;
using System.Data;
using Cynthia.Data;
using Cynthia.Business.DataContracts;
using System.Collections.Generic;

namespace Cynthia.Business
{
	/// <summary>
	/// Represents a feature that can plug into the content management system.
	/// </summary>
	public class ModuleDefinition : IModuleInfo
	{

		#region Constructors

		public ModuleDefinition()
		{}
	    
	
		public ModuleDefinition(int moduleDefId) 
		{
			GetModuleDefinition(moduleDefId); 
		}

        public ModuleDefinition(Guid featureGuid)
        {
            providedFeatureGuid = featureGuid;
            GetModuleDefinition(featureGuid);
        }

		#endregion

		#region Private Properties

		private int moduleDefID = -1;
        private Guid featureGuid = Guid.Empty;
        private Guid providedFeatureGuid = Guid.Empty;
		private int siteID = -1;
        private string resourceFile = "Resource";
		private string featureName; 
		private string controlSrc; 
		private int sortOrder = 500;
        private bool isCacheable = false;
        private int defaultCacheTime = 0;
		private bool isAdmin = false;
        private String icon = String.Empty;
        private bool isSearchable = false;
        private string searchListName = string.Empty;
        private bool supportsPageReuse = true;
        private string deleteProvider = string.Empty;

        
	
		#endregion

		#region Public Properties

		public int ModuleDefId 
		{
			get { return moduleDefID; }
			set { moduleDefID = value; }
		}

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public string ResourceFile
        {
            get { return resourceFile; }
            set { resourceFile = value; }
        }

		public string FeatureName 
		{
			get { return featureName; }
			set { featureName = value; }
		}

        public bool IsCacheable
        {
            get { return isCacheable; }
            set { isCacheable = value; }
        }

        public bool IsSearchable
        {
            get { return isSearchable; }
            set { isSearchable = value; }
        }

        public string SearchListName
        {
            get { return searchListName; }
            set { searchListName = value; }
        }

        public bool SupportsPageReuse
        {
            get { return supportsPageReuse; }
            set { supportsPageReuse = value; }
        }

        public string DeleteProvider
        {
            get { return deleteProvider; }
            set { deleteProvider = value; }
        }

		public string ControlSrc 
		{
			get { return controlSrc; }
			set { controlSrc = value; }
		}
		public int SortOrder 
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}
        public int DefaultCacheTime
        {
            get { return defaultCacheTime; }
            set { defaultCacheTime = value; }
        }
		public bool IsAdmin 
		{
			get { return isAdmin; }
			set { isAdmin = value; }
		}

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

		#endregion

		#region Private Methods

        private void GetModuleDefinition(Guid featureGuid)
        {
            using (IDataReader reader = DBModuleDefinition.GetModuleDefinition(featureGuid))
            {
                GetModuleDefinition(reader);
            }
        }

		private void GetModuleDefinition(int moduleDefId) 
		{
            using (IDataReader reader = DBModuleDefinition.GetModuleDefinition(moduleDefId))
            {
                GetModuleDefinition(reader);
            }
			
		}

        private void GetModuleDefinition(IDataReader reader)
        {
			if (reader.Read())
			{
				loadFromDataReader(this, reader);
			}
        }

		static void loadFromDataReader(ModuleDefinition m,IDataReader reader) {
			m.moduleDefID = Convert.ToInt32(reader["ModuleDefID"]);
			//this.siteID = Convert.ToInt32(reader["SiteID"]);

			m.featureName = reader["FeatureName"].ToString();
			m.controlSrc = reader["ControlSrc"].ToString();
			m.sortOrder = Convert.ToInt32(reader["SortOrder"]);
			m.defaultCacheTime = Convert.ToInt32(reader["DefaultCacheTime"]);
			m.isAdmin = Convert.ToBoolean(reader["IsAdmin"]);
			m.icon = reader["Icon"].ToString();
			m.featureGuid = new Guid(reader["Guid"].ToString());
			m.resourceFile = reader["ResourceFile"].ToString();

			m.searchListName = reader["SearchListName"].ToString();
			if (reader["IsCacheable"] != DBNull.Value)
			{
				m.isCacheable = Convert.ToBoolean(reader["IsCacheable"]);
			}

			if (reader["IsSearchable"] != DBNull.Value)
			{
				m.isSearchable = Convert.ToBoolean(reader["IsSearchable"]);
			}

			if (reader["SupportsPageReuse"] != DBNull.Value)
			{
				m.supportsPageReuse = Convert.ToBoolean(reader["SupportsPageReuse"]);
			}

			m.deleteProvider = reader["DeleteProvider"].ToString();
		}

		private bool Create()
		{ 
			int newID = -1;

            if (this.featureGuid == Guid.Empty)
            {
                if (this.providedFeatureGuid != Guid.Empty)
                {
                    this.featureGuid = this.providedFeatureGuid;
                }
                else
                {
                    this.featureGuid = Guid.NewGuid();
                }
            }
			
			newID = DBModuleDefinition.AddModuleDefinition(
                this.featureGuid,
				this.siteID, 
				this.featureName, 
				this.controlSrc, 
				this.sortOrder,
                this.defaultCacheTime,
                this.icon,
				this.isAdmin,
                this.resourceFile,
                this.isCacheable,
                this.isSearchable,
                this.searchListName,
                this.supportsPageReuse,
                this.deleteProvider); 
			
			this.moduleDefID = newID;
					
			return (newID > -1);

		}

		private bool Update()
		{

			return DBModuleDefinition.UpdateModuleDefinition(
				this.moduleDefID, 
				this.featureName, 
				this.controlSrc, 
				this.sortOrder, 
                this.defaultCacheTime,
                this.icon,
				this.isAdmin,
                this.resourceFile,
                this.isCacheable,
                this.isSearchable,
                this.searchListName,
                this.supportsPageReuse,
                this.deleteProvider); 
				
		}


		#endregion

		#region Public Methods

	
		public bool Save()
		{
			if(this.moduleDefID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}
		
		#endregion

		#region Static Methods

		public static bool DeleteModuleDefinition(int moduleDefId)
		{
            DBModuleDefinition.DeleteModuleDefinitionFromSites(moduleDefId);
			return DBModuleDefinition.DeleteModuleDefinition(moduleDefId);
		}

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
		{
            return DBModuleDefinition.GetModuleDefinitions(siteGuid);
		}

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            return DBModuleDefinition.GetModuleDefinitionsBySite(siteGuid);
        }

		public static IDataReader GetUserModules(int siteId)
		{
			return DBModuleDefinition.GetUserModules(siteId);
		}

        public static IDataReader GetSearchableModules(int siteId)
        {
            return DBModuleDefinition.GetSearchableModules(siteId);
        }

		public static List<ModuleDefinition> GetModules(int siteID,string excludeModuleIDs) {
			var retVal = new List<ModuleDefinition>();
			using (var reader = ModuleDefinition.GetSearchableModules(siteID))
			{
				while (reader.Read())
				{
					if (!excludeModuleIDs.Contains(reader["Guid"].ToString())) {
						var tempObj=new ModuleDefinition();
						loadFromDataReader(tempObj,reader);
						retVal.Add(tempObj);
					}
				}

			}
			return retVal;
		}

        public static bool UpdateModuleDefinitionSetting(
            Guid featureGuid,
            int moduleDefId, 
            string resourceFile,
            string settingName, 
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            return DBModuleDefinition.UpdateModuleDefinitionSetting(
                featureGuid,
                moduleDefId, 
                resourceFile,
                settingName, 
                settingValue,
                controlType,
                regexValidationExpression,
                controlSrc,
                helpKey,
                sortOrder);

        }

        public static bool UpdateModuleDefinitionSettingById(
            int id,
            int moduleDefId,
            string resourceFile,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            return DBModuleDefinition.UpdateModuleDefinitionSettingById(
                id,
                moduleDefId,
                resourceFile,
                settingName,
                settingValue,
                controlType,
                regexValidationExpression,
                controlSrc,
                helpKey,
                sortOrder);

        }

        /// <summary>
        /// update instance setting properties to match definition settings
        /// for controltype, controlsrc, regexvalidationexpression ans sort
        /// this is called at the end of an upgrade
        /// </summary>
        public static void SyncDefinitions()
        {
            DBModuleDefinition.SyncDefinitions();
        }

        //public static void SyncDefinitions(object o)
        //{
        //    DBModuleDefinition.SyncDefinitions();
        //}

        public static bool DeleteSettingById(int id)
        {
            return DBModuleDefinition.DeleteSettingById(id);

        }

        public static bool SettingExists(Guid featureGuid, string settingName)
        {
            bool result = false;

            using (IDataReader reader = DBModuleDefinition.ModuleDefinitionSettingsGetSetting(
                featureGuid,
                settingName))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        public static void EnsureInstallationInAdminSites()
        {
            DBModuleDefinition.EnsureInstallationInAdminSites();
        }


		#endregion


	}
	
}
