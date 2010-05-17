
using System;
using System.Configuration;
using System.Data;
using Cynthia.Data;

namespace Cynthia.Business
{
	/// <summary>
	///
	/// </summary>
	public class FriendlyUrl
	{

		#region Constructors

		public FriendlyUrl()
		{}
	    
	
		public FriendlyUrl(int urlId) 
		{
			GetFriendlyUrl(urlId); 
		}

		public FriendlyUrl(string hostName, string friendlyUrl) 
		{
			GetFriendlyUrl(hostName, friendlyUrl); 
		}

        public FriendlyUrl(int siteId, string friendlyUrl)
        {
            if (string.IsNullOrEmpty(friendlyUrl)) { return; }
            GetFriendlyUrl(siteId, friendlyUrl);
        }

		#endregion

		#region Private Properties

        private Guid _itemGuid = Guid.Empty;
        private Guid _siteGuid = Guid.Empty;
        private Guid _pageGuid = Guid.Empty;
		private int _urlId = -1; 
		private int _siteId = -1; 
		private string _friendlyUrl = string.Empty; 
		private string _realUrl = string.Empty; 
		private bool _isPattern; 
		private bool _foundFriendlyUrl = false;

        
		
		#endregion

		#region Public Properties

        public Guid ItemGuid
        {
            get { return _itemGuid; }

        }

        public Guid SiteGuid
        {
            get { return _siteGuid; }
            set { _siteGuid = value; }
        }

        public Guid PageGuid
        {
            get { return _pageGuid; }
            set { _pageGuid = value; }
        }

		public bool FoundFriendlyUrl 
		{
			get { return _foundFriendlyUrl; }
			
		}

		public int UrlId 
		{
			get { return _urlId; }
			set { _urlId = value; }
		}
		public int SiteId 
		{
			get { return _siteId; }
			set { _siteId = value; }
		}
		public string Url 
		{
			get { return _friendlyUrl; }
			set { _friendlyUrl = value; }
		}
		public string RealUrl 
		{
			get { return _realUrl; }
			set 
			{ 
				if(!value.StartsWith("~/"))
				{
					value = "~/" + value;
				}
				_realUrl = value; 
			}
		}
		public bool IsPattern 
		{
			get { return _isPattern; }
			set { _isPattern = value; }
		}

		#endregion

		#region Private Methods

		private void GetFriendlyUrl(int urlId) 
		{
            using (IDataReader reader = DBFriendlyUrl.GetFriendlyUrl(urlId))
            {
                GetFriendlyUrl(reader);
            }
			
		}

        private void GetFriendlyUrl(int siteId, string friendlyUrl)
        {
            using (IDataReader reader = DBFriendlyUrl.GetFriendlyUrl(siteId, friendlyUrl))
            {
                GetFriendlyUrl(reader);
            }

        }

		private void GetFriendlyUrl(string hostName, string friendlyUrl) 
		{
            using (IDataReader reader = DBFriendlyUrl.GetByUrl(hostName, friendlyUrl))
            {
                GetFriendlyUrl(reader);
            }
			
		}

        private void GetFriendlyUrl(IDataReader reader)
        {
            if (reader.Read())
            {
                this._foundFriendlyUrl = true;
                this._urlId = Convert.ToInt32(reader["UrlID"]);
                this._siteId = Convert.ToInt32(reader["SiteID"]);
                this._friendlyUrl = reader["FriendlyUrl"].ToString();
                this._realUrl = reader["RealUrl"].ToString();
                this._isPattern = Convert.ToBoolean(reader["IsPattern"]);
                string pg = reader["PageGuid"].ToString();
                if (pg.Length == 36) this._pageGuid = new Guid(pg);
                this._siteGuid = new Guid(reader["SiteGuid"].ToString());
                this._itemGuid = new Guid(reader["ItemGuid"].ToString());

            }
        }
		
		private bool Create()
		{ 
			int newID = 0;
            this._itemGuid = Guid.NewGuid();

			newID = DBFriendlyUrl.AddFriendlyUrl(
                this._itemGuid,
                this._siteGuid,
                this._pageGuid,
				this._siteId, 
				this._friendlyUrl, 
				this._realUrl, 
				this._isPattern); 
			
			this._urlId = newID;
	
			return (newID > 0);

		}

		private bool Update()
		{

			return DBFriendlyUrl.UpdateFriendlyUrl(
				this._urlId, 
				this._siteId, 
                this.PageGuid,
				this._friendlyUrl, 
				this._realUrl, 
				this._isPattern); 
				
		}


		#endregion

		#region Public Methods

		public bool Save()
		{
            if (_friendlyUrl.Length == 0) { return false; }
            if (_realUrl.Length == 0) { return false; }

			if( this._urlId > 0)
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


		public static DataTable GetByHostName(string  hostName)  
		{
			return DBFriendlyUrl.GetByHostName(hostName);
		
		}

        public static DataTable GetBySite(int siteId)
        {
            return DBFriendlyUrl.GetBySite(siteId);

        }

        /// <summary>
        /// Gets a page of data from the cy_FriendlyUrls table.
        /// </summary>
        /// <param name="siteId">The siteId.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBFriendlyUrl.GetPage(
                siteId,
                pageNumber,
                pageSize,
                out totalPages);

        }

        public static IDataReader GetPage(
            int siteId,
            string searchTerm,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBFriendlyUrl.GetPage(
                siteId,
                searchTerm,
                pageNumber,
                pageSize,
                out totalPages);

        }


		public static bool DeleteUrl(int urlId) 
		{
			return DBFriendlyUrl.DeleteFriendlyUrl(urlId);
		}

        public static bool DeleteUrlByPageId(int pageId)
        {
            return DBFriendlyUrl.DeleteByPageId(pageId); ;
        }

        public static bool DeleteByPageGuid(Guid pageGuid)
        {
            return DBFriendlyUrl.DeleteByPageGuid(pageGuid);
        }

        public static bool Exists(int siteId, String friendlyUrl)
        {
            bool result = false;

            using (IDataReader reader = DBFriendlyUrl.GetFriendlyUrl(siteId, friendlyUrl))
            {
                while (reader.Read())
                {
                    result = true;
                }
            }

            return result;

        }

        [Obsolete("This method is obsolete. You should use SiteUtils.SuggestFriendlyUrl")]
        public static String SuggestFriendlyUrl(
            String pageName, 
            SiteSettings siteSettings)
        {
            String friendlyUrl = CleanStringForUrl(pageName);

            switch (siteSettings.DefaultFriendlyUrlPattern)
            {
                case SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX:
                    friendlyUrl += ".aspx";
                    break;

            }

            int i = 1;
            while (FriendlyUrl.Exists(siteSettings.SiteId, friendlyUrl))
            {
                friendlyUrl = i.ToString() + friendlyUrl;
            }

            bool forceToLowerCase = GetBoolPropertyFromConfig("ForceFriendlyUrlsToLowerCase", true);

            if (forceToLowerCase) return friendlyUrl.ToLower();

            return friendlyUrl;
        }


        
        public static String CleanStringForUrl(String input)
        {
            String ouputString = RemovePunctuation(input).Replace(" - ", "-").Replace(" ", "-").Replace("/", String.Empty).Replace("\"", String.Empty).Replace("'", String.Empty).Replace("#", String.Empty).Replace("~", String.Empty).Replace("`", String.Empty).Replace("@", String.Empty).Replace("$", String.Empty).Replace("*", String.Empty).Replace("^", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty).Replace("+", String.Empty).Replace("=", String.Empty).Replace("%", String.Empty).Replace(">", String.Empty).Replace("<", String.Empty);
            
            return ouputString;

        }

        public static String RemovePunctuation(String input)
        {
            String outputString = String.Empty;
            if (input != null)
            {
                outputString = input.Replace(".", String.Empty).Replace(",", String.Empty).Replace(":", String.Empty).Replace("?", String.Empty).Replace("!", String.Empty).Replace(";", String.Empty).Replace("&", String.Empty).Replace("{", String.Empty).Replace("}", String.Empty).Replace("[", String.Empty).Replace("]", String.Empty);
            }
            return outputString;
        }


        private static bool GetBoolPropertyFromConfig(string key, bool defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] == null) return defaultValue;

            if (string.Equals(ConfigurationManager.AppSettings[key], "true", StringComparison.InvariantCultureIgnoreCase)) 
                return true;
            
            if (string.Equals(ConfigurationManager.AppSettings[key], "false", StringComparison.InvariantCultureIgnoreCase))
                return false;
            
            return defaultValue;


        }

        public static  bool AddNew(int siteId,Guid siteGuid,Guid pageGuid,string url,string realUrl,bool isPattern)
        {
            var boUrl = new FriendlyUrl()
                          {
                              
                              SiteId = siteId,
                              SiteGuid = siteGuid,
                              PageGuid = pageGuid,
                              Url = url,
                              RealUrl = realUrl,
                              IsPattern = isPattern
                          };
            return boUrl.Save();
        }

	    #endregion


	}
	
}