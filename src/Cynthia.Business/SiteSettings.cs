
using System;
using System.Data;
using System.Globalization;
using log4net;
using Cynthia.Data;

namespace Cynthia.Business
{
	/// <summary>
    /// The preferred way to obtasin a reference to SiteSettings object is using Cynthia.Business.WebHelpers.CacheHelper.GetCurrentSiteSettings();
	/// </summary>
	public class SiteSettings
	{
		#region Constructors

        public SiteSettings(Guid siteGuid)
        {
            GetSiteSettings(siteGuid);
        }

		public SiteSettings(string hostName) 
		{
			GetSiteSettings(hostName);
		}

		public SiteSettings(int siteId) 
		{
			if(siteId > 0)
			{
				GetSiteSettings(siteId);
			}
			
		}

        public SiteSettings()
        {

        }

		#endregion

		#region Enums

        public enum ContentEditorSkin
        {
            normal,
            office2003,
            silver

        }

        public enum FriendlyUrlPattern
        {
            PageName, // this won't normally work on Windows but can work on mono if mono is the handler for the whole directory
            PageNameWithDotASPX


        }

		
		
		#endregion

		#region Private Properties

		private static readonly ILog log = LogManager.GetLogger(typeof(SiteSettings));
		private int siteID = -1;
        private Guid siteGuid = Guid.Empty;
		private string siteName = string.Empty;
        private string skin = string.Empty;
        private string logo = string.Empty;
        private string icon = string.Empty; 
		private bool allowUserSkins; 
		private bool allowPageSkins;
		private bool allowHideMenuOnPages;
		private bool allowNewRegistration = true; 
		private bool useSecureRegistration; 
		private bool useSSLOnAllPages;

        //these are legacy fields may be removed some day
        private string metaKeyWords = string.Empty;
        private string metaDescription = string.Empty;
        private string metaEncoding = string.Empty;
        private string metaAdditional = string.Empty; 


		private bool isServerAdminSite; 
		private bool allowUserFullNameChange;
		private bool useEmailForLogin = true;
		private bool reallyDeleteUsers = true;
        private string editorProviderName = "CKEditorProvider";// CKEditorProvider FCKeditorProvider TinyMCEProvider
		private ContentEditorSkin editorSkin = SiteSettings.ContentEditorSkin.normal;
		private FriendlyUrlPattern defaultFriendlyUrlPattern = SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX;

		// LDAP Settings
		private bool useLdapAuth = false;
		private bool autoCreateLDAPUserOnFirstLogin = true;
		private LdapSettings ldapSettings = new LdapSettings();

        private bool enableMyPageFeature = true;

        // extended properties
        private bool extendedPropertiesLoaded;
        private bool extendedPropertiesAreDirty = false;

        private bool allowPasswordRetrieval;
        private bool allowPasswordReset;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;
        private int maxInvalidPasswordAttempts = 10;
        private int passwordAttemptWindowMinutes = 5;
        private int passwordFormat = 1;
        private int minRequiredPasswordLength = 7;
        private int minRequiredNonAlphanumericCharacters = 0;
        private string passwordStrengthRegularExpression = string.Empty;
        private string defaultEmailFromAddress = string.Empty;
        private string siteRoot = string.Empty;
        private string skinBaseUrl = string.Empty;
        private string siteFolderName = string.Empty;
        private string virtualPageRoot = string.Empty;
        
        // SubkismetCaptchaProvider
        //"RecaptchaCaptchaProvider"; 
        //"SimpleMathCaptchaProvider";
        //SubkismetInvisibleCaptchaProvider
        private string datePickerProvider = "CDatePicker";
        private string captchaProvider = "SubkismetCaptchaProvider";
        private string recaptchaPrivateKey = string.Empty;
        private string recaptchaPublicKey = string.Empty;
        private string wordpressAPIKey = string.Empty;
        private string windowsLiveAppID = string.Empty;
        private string windowsLiveKey = string.Empty;
        private bool allowOpenIDAuth = false;
        private bool allowWindowsLiveAuth = false;
        private string gmapApiKey = string.Empty;

        
        // AddThisDotComUsername maps to apiKeyExtra1
        private string apiKeyExtra1 = string.Empty;

        //GoogleAnalyticsAccountCode
        private string apiKeyExtra2 = string.Empty;

        //OpenIdSelectorId
        private string apiKeyExtra3 = string.Empty;

        // for future use
        private string apiKeyExtra4 = string.Empty;

        // maps To PreferredHostName as of 2008-05-22
        private string apiKeyExtra5 = string.Empty;

        private Currency currency = null;

        private bool disableDbAuth = false;

        private string timeZoneId = "Eastern Standard Time"; //default


        


		#endregion

		#region Public Properties

		
		public int SiteId 
		{
			get { return siteID; }
			set { siteID = value; }
		}

        public Guid SiteGuid
        {
            get { return siteGuid; }
            
        }
		
		public string SiteName 
		{
			get { return siteName; }
			set { siteName = value; }
		}

        /// <summary>
        /// In case multiple host names map to your site and you want to force a particular one.
        /// For example I want to force urls with hostname vivasky.com to www.vivasky.com,
        /// because my SSL certificate matches www.vivasky.com but not vivasky.com
        /// </summary>
        public string PreferredHostName
        {
            get { return apiKeyExtra5; }
            set { apiKeyExtra5 = value; }
        }

        public String DefaultEmailFromAddress
        {
            get
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return defaultEmailFromAddress;
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                defaultEmailFromAddress = value; 
            }
        }

		public string Skin 
		{
			get { return skin; }
			set { skin = value; }
		}

        public string EditorProviderName
        {
            get { return editorProviderName; }
            set { editorProviderName = value; }
        }

		public ContentEditorSkin EditorSkin 
		{
			get { return editorSkin; }
			set { editorSkin = value; }
		}

		public string Logo 
		{
			get { return logo; }
			set { logo = value; }
		}
		public string Icon 
		{
			get { return icon; }
			set { icon = value; }
		}

        public bool EnableMyPageFeature
        {
            get { return enableMyPageFeature; }
            set { enableMyPageFeature = value; }
        }

		public bool AllowUserSkins 
		{
			get { return allowUserSkins; }
			set { allowUserSkins = value; }
		}

		public bool AllowPageSkins 
		{
			get { return allowPageSkins; }
			set { allowPageSkins = value; }
		}

		public bool AllowHideMenuOnPages 
		{
			get { return allowHideMenuOnPages; }
			set { allowHideMenuOnPages = value; }
		}

		public bool AllowNewRegistration 
		{
			get { return allowNewRegistration; }
			set { allowNewRegistration = value; }
		}
		public bool UseSecureRegistration 
		{
			get { return useSecureRegistration; }
			set { useSecureRegistration = value; }
		}

		public bool UseSslOnAllPages 
		{
			get { return useSSLOnAllPages; }
			set { useSSLOnAllPages = value; }
		}

       
		public bool IsServerAdminSite 
		{
			get { return isServerAdminSite; }
			set { isServerAdminSite = value; }
		}

		public bool UseLdapAuth
		{
			get { return useLdapAuth; }
			set { useLdapAuth = value; }
		}

		public bool AutoCreateLdapUserOnFirstLogin
		{
			get { return autoCreateLDAPUserOnFirstLogin; }
			set { autoCreateLDAPUserOnFirstLogin = value; }
		}

		public LdapSettings SiteLdapSettings
		{
			get { return ldapSettings; }
			set { ldapSettings = value; }
		}

		public bool AllowUserFullNameChange 
		{
			get { return allowUserFullNameChange; }
			set { allowUserFullNameChange = value; }
		}

		public bool ReallyDeleteUsers 
		{
			get { return reallyDeleteUsers; }
			set { reallyDeleteUsers = value; }
		}

		public bool UseEmailForLogin 
		{
			get { return useEmailForLogin; }
			set { useEmailForLogin = value; }
		}

		public FriendlyUrlPattern DefaultFriendlyUrlPattern 
		{
			get { return defaultFriendlyUrlPattern; }
			set { defaultFriendlyUrlPattern = value; }
		}

        public bool DisableDbAuth
        {
            get { return disableDbAuth; }
            set { disableDbAuth = value; }
        }

        public bool AllowPasswordRetrieval
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                // 1 = hashed, can't be retrieved
                //2009-01-25 commented this out because we can generate a new random password
                // and send it
                //if (PasswordFormat == 1) { return false; }
                return allowPasswordRetrieval; 
            }
            set
            {
                extendedPropertiesAreDirty = true;
                allowPasswordRetrieval = value; 
            }
        }

        public bool AllowPasswordReset
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return allowPasswordReset; 
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                allowPasswordReset = value; 
            }
        }

        public bool RequiresQuestionAndAnswer
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return requiresQuestionAndAnswer; 
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                requiresQuestionAndAnswer = value; 
            }
        }

        public bool RequiresUniqueEmail
        {
            // I'm not exposing this in the UI because it really needs to
            // always be true with the current design if email is used for login
            // we could expose this in scenario is loginname for login
            // but if someone starts that way and changes it things could get inconsistent
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return requiresUniqueEmail;
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                requiresUniqueEmail = value; 
            }
        }

        public int MaxInvalidPasswordAttempts
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return maxInvalidPasswordAttempts; 
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                maxInvalidPasswordAttempts = value; 
            }
        }

        public int PasswordAttemptWindowMinutes
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return passwordAttemptWindowMinutes;
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                passwordAttemptWindowMinutes = value; 
            }
        }

        /// <summary>
        /// Clear = 0, Hashed = 1, Encrypted = 2, corresponding to MembershipPasswordFormat Enum
        /// </summary>
        public int PasswordFormat
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return passwordFormat; 
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                
                passwordFormat = value; 
            }
        }

        public int MinRequiredPasswordLength
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return minRequiredPasswordLength; 
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                minRequiredPasswordLength = value; 
            }
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return minRequiredNonAlphanumericCharacters; 
            }
            set
            {
                extendedPropertiesAreDirty = true;
                minRequiredNonAlphanumericCharacters = value; 
            }
        }

        public String PasswordStrengthRegularExpression
        {
            get 
            {
                if (!this.extendedPropertiesLoaded)
                {
                    LoadExtendedProperties();
                }
                return passwordStrengthRegularExpression; 
            }
            set 
            {
                extendedPropertiesAreDirty = true;
                passwordStrengthRegularExpression = value; 
            }
        }

        public String SiteRoot
        {
            get 
            {
                if (siteFolderName.Length > 0)
                {
                    return siteRoot.EndsWith("/")
                               ? siteRoot + siteFolderName
                               : String.Format("{0}/{1}", siteRoot, siteFolderName);
                }
                return siteRoot ; 
            }
            set { siteRoot = value; }
        }

        public String SkinBaseUrl
        {
            get { return skinBaseUrl; }
            set { skinBaseUrl = value; }
        }

        public string SiteFolderName
        {
            get { return siteFolderName; }
            set { siteFolderName = value; }
        }

        /// <summary>
        /// application relative Data folder of current site.Such as '~/data/sites/1/'
        /// </summary>
	    public string DataFolder
	    {
	        get
	        {
                return GetDataFolder(this.SiteId);
	        }
	    }
        public string DataFolderUrl {
            get
            {
                return string.Format("{0}/{1}",SiteRoot,DataFolder.Replace("~/",""));
            }
        }

        public string DatePickerProvider
        {
            get { return datePickerProvider; }
            set { datePickerProvider = value; }
        }

        public string CaptchaProvider
        {
            get { return captchaProvider; }
            set { captchaProvider = value; }
        }

        public string RecaptchaPrivateKey
        {
            get { return recaptchaPrivateKey; }
            set { recaptchaPrivateKey = value; }
        }

        public string RecaptchaPublicKey
        {
            get { return recaptchaPublicKey; }
            set { recaptchaPublicKey = value; }
        }

        public string WordpressApiKey
        {
            get { return wordpressAPIKey; }
            set { wordpressAPIKey = value; }
        }

        public string WindowsLiveAppId
        {
            get { return windowsLiveAppID; }
            set { windowsLiveAppID = value; }
        }

        public string WindowsLiveKey
        {
            get { return windowsLiveKey; }
            set { windowsLiveKey = value; }
        }

        public bool AllowOpenIdAuth
        {
            get { return allowOpenIDAuth; }
            set { allowOpenIDAuth = value; }
        }

        public bool AllowWindowsLiveAuth
        {
            get { return allowWindowsLiveAuth; }
            set { allowWindowsLiveAuth = value; }
        }

        public string GmapApiKey
        {
            get { return gmapApiKey; }
            set { gmapApiKey = value; }
        }

        public string AddThisDotComUsername
        {
            get { return apiKeyExtra1; }
            set { apiKeyExtra1 = value; }
        }

        public string GoogleAnalyticsAccountCode
        {
            get { return apiKeyExtra2; }
            set { apiKeyExtra2 = value; }
        }

        /// <summary>
        /// https://www.idselector.com/
        /// </summary>
        public string OpenIdSelectorId
        {
            get { return apiKeyExtra3; }
            set { apiKeyExtra3 = value; }
        }


        

        public string MyPageSkin
        {
            get
            {
                string result = GetExpandoProperty("MyPageSkin");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("MyPageSkin", value); }
        }

        public string SiteMapSkin
        {
            get
            {
                string result = GetExpandoProperty("SiteMapSkin");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("SiteMapSkin", value); }
        }

        public string AppLogoForWindowsLive
        {
            get
            {
                string result = GetExpandoProperty("AppLogoForWindowsLive");
                if (result != null) { return result; }
                return "/Data/logos/Cmoonprint.jpg";
            }
            set { SetExpandoProperty("AppLogoForWindowsLive", value); }
        }

        public bool AllowWindowsLiveMessengerForMembers
        {
            get
            {
                string sBool = GetExpandoProperty("AllowWindowsLiveMessengerForMembers");

                if ((sBool != null) && (sBool.Length > 0))
                {
                    return Convert.ToBoolean(sBool);
                }

                return false;
            }
            set { SetExpandoProperty("AllowWindowsLiveMessengerForMembers", value.ToString()); }
        }

        public string RpxNowApiKey
        {
            get
            {
                string result = GetExpandoProperty("RpxNowApiKey");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RpxNowApiKey", value); }
        }

        public string RpxNowApplicationName
        {
            get
            {
                string result = GetExpandoProperty("RpxNowApplicationName");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RpxNowApplicationName", value); }
        }

        public string RpxNowAdminUrl
        {
            get
            {
                string result = GetExpandoProperty("RpxNowAdminUrl");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RpxNowAdminUrl", value); }
        }

        public string WebSnaprKey
        {
            get
            {
                string result = GetExpandoProperty("WebSnaprKey");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("WebSnaprKey", value); }
        }

        public string OpenSearchName
        {
            get
            {
                string result = GetExpandoProperty("OpenSearchName");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("OpenSearchName", value); }
        }

        public string Slogan
        {
            get
            {
                string result = GetExpandoProperty("Slogan");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("Slogan", value); }
        }

        public string CompanyName
        {
            get
            {
                string result = GetExpandoProperty("CompanyName");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("CompanyName", value); }
        }

        public bool EnableWoopra
        {
            get
            {
                string sBool = GetExpandoProperty("EnableWoopra");

                if ((sBool != null) && (sBool.Length > 0))
                {
                    return Convert.ToBoolean(sBool);
                }

                return false;
            }
            set { SetExpandoProperty("EnableWoopra", value.ToString()); }
        }

        public string PrivacyPolicyUrl
        {
            get
            {
                string result = GetExpandoProperty("PrivacyPolicyUrl");
                if (result != null) { return result; }
                return "/privacy.aspx";
            }
            set { SetExpandoProperty("PrivacyPolicyUrl", value); }
        }

        public string SMTPUser
        {
            get { return GetExpandoProperty("SMTPUser"); }
            set { SetExpandoProperty("SMTPUser", value); }
        }

        public string SMTPPassword
        {
            get { return GetExpandoProperty("SMTPPassword"); }
            set { SetExpandoProperty("SMTPPassword", value); }
        }

        public int SMTPPort
        {
            get 
            { 
                string sPort = GetExpandoProperty("SMTPPort");
                if ((sPort != null)&&(sPort.Length > 0))
                {
                    return Convert.ToInt32(sPort, CultureInfo.InvariantCulture);
                }
                return 25;
            }
            set 
            { 
                SetExpandoProperty("SMTPPort", value.ToString(CultureInfo.InvariantCulture)); 
            }
        }

        public string SMTPPreferredEncoding
        {
            get { return GetExpandoProperty("SMTPPreferredEncoding"); }
            set { SetExpandoProperty("SMTPPreferredEncoding", value); }
        }

        public string SMTPServer
        {
            get { return GetExpandoProperty("SMTPServer"); }
            set { SetExpandoProperty("SMTPServer", value); }
        }



        public bool SMTPRequiresAuthentication
        {
            get
            {

                string sUseSsl = GetExpandoProperty("SMTPRequiresAuthentication");

                if ((sUseSsl != null) && (sUseSsl.Length > 0))
                {
                    return Convert.ToBoolean(sUseSsl);
                }

                return false;
            }
            set { SetExpandoProperty("SMTPRequiresAuthentication", value.ToString()); }
        }

        
        public bool SMTPUseSsl
        {
            get {

                string sUseSsl = GetExpandoProperty("SMTPUseSsl");

                if ((sUseSsl != null)&&(sUseSsl.Length > 0))
                {
                    return Convert.ToBoolean(sUseSsl);
                }

                return false; 
            }
            set { SetExpandoProperty("SMTPUseSsl", value.ToString()); }
        }

        public bool AllowUserEditorPreference
        {
            get
            {

                string s = GetExpandoProperty("AllowUserEditorPreference");

                if ((s != null) && (s.Length > 0))
                {
                    return Convert.ToBoolean(s);
                }

                return false;
            }
            set { SetExpandoProperty("AllowUserEditorPreference", value.ToString()); }
        }

        public string SiteRootEditRoles
        {
            get
            {
                string result = GetExpandoProperty("SiteRootEditRoles");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("SiteRootEditRoles", value); }
        }

        public string SiteRootDraftEditRoles
        {
            get
            {
                string result = GetExpandoProperty("SiteRootDraftEditRoles");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("SiteRootDraftEditRoles", value); }
        }

        public string CommerceReportViewRoles
        {
            get
            {
                string result = GetExpandoProperty("CommerceReportViewRoles");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("CommerceReportViewRoles", value); }
        }

        public string RolesThatCanCreateRootPages
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanCreateRootPages");
                if (result != null) { return result; }
                return "Content Administrators;Content Publishers;";
            }
            set { SetExpandoProperty("RolesThatCanCreateRootPages", value); }
        }

        public string RolesThatCanViewMemberList
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanViewMemberList");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesThatCanViewMemberList", value); }
        }

        public string RolesThatCanManageUsers
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanManageUsers");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesThatCanManageUsers", value); }
        }

        public string RolesThatCanViewMyPage
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanViewMyPage");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesThatCanViewMyPage", value); }
        }

        public string RolesThatCanLookupUsers
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanLookupUsers");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesThatCanLookupUsers", value); }
        }

        public string RolesNotAllowedToEditModuleSettings
        {
            get
            {
                string result = GetExpandoProperty("RolesNotAllowedToEditModuleSettings");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesNotAllowedToEditModuleSettings", value); }
        }

        public string RolesThatCanEditContentTemplates
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanEditContentTemplates");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesThatCanEditContentTemplates", value); }
        }

        
        public string GeneralBrowseAndUploadRoles
        {
            get
            {
                string result = GetExpandoProperty("GeneralBrowseAndUploadRoles");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("GeneralBrowseAndUploadRoles", value); }
        }

        public string UserFilesBrowseAndUploadRoles
        {
            get
            {
                string result = GetExpandoProperty("UserFilesBrowseAndUploadRoles");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("UserFilesBrowseAndUploadRoles", value); }
        }

        public string RolesThatCanDeleteFilesInEditor
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanDeleteFilesInEditor");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesThatCanDeleteFilesInEditor", value); }
        }

        public string RolesThatCanEditSkins
        {
            get
            {
                string result = GetExpandoProperty("RolesThatCanEditSkins");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("RolesThatCanEditSkins", value); }
        }

        public string AvatarSystem
        {
            get
            {
                string result = GetExpandoProperty("AvatarSystem");
                if (result != null) { return result; }
                return "gravatar";
            }
            set { SetExpandoProperty("AvatarSystem", value); }
        }

        public string CommentProvider
        {
            get
            {
                string result = GetExpandoProperty("CommentProvider");
                if (result != null) { return result; }
                return "intensedebate";
            }
            set { SetExpandoProperty("CommentProvider", value); }
        }

        public string IntenseDebateAccountId
        {
            get
            {
                string result = GetExpandoProperty("IntenseDebateAccountId");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("IntenseDebateAccountId", value); }
        }

        public string DisqusSiteShortName
        {
            get
            {
                string result = GetExpandoProperty("DisqusSiteShortName");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("DisqusSiteShortName", value); }
        }

        /// <summary>
        /// if you are using vocabularies such as Dublin Core in your page meta data, you can specify the profile which will be added to the head element
        /// http://dublincore.org/documents/dcq-html/
        /// ie for Dublin Core you would put http://dublincore.org/documents/dcq-html/ 
        /// if using multiple vocabularies you can separe the urls by white space
        /// </summary>
        public string MetaProfile
        {
            get
            {
                string result = GetExpandoProperty("MetaProfile");
                if (result != null) { return result; }
                return string.Empty;
            }
            set { SetExpandoProperty("MetaProfile", value); }
        }

        public string NewsletterEditor
        {
            get
            {
                string result = GetExpandoProperty("NewsletterEditor");
                if (result != null) { return result; }
                return "TinyMCEProvider";
            }
            set { SetExpandoProperty("NewsletterEditor", value); }
        }

        
        public Guid DefaultCountryGuid
        {
            get
            {
                string result = GetExpandoProperty("DefaultCountryGuid");
                if ((result != null) && (result.Length == 36)) { return new Guid(result); }
                return new Guid("a71d6727-61e7-4282-9fcb-526d1e7bc24f"); //US
            }
            set { SetExpandoProperty("DefaultCountryGuid", value.ToString()); }
        }

        public Guid DefaultStateGuid
        {
            get
            {
                string result = GetExpandoProperty("DefaultStateGuid");
                if ((result != null) && (result.Length == 36)) { return new Guid(result); }
                return Guid.Empty;
            }
            set { SetExpandoProperty("DefaultStateGuid", value.ToString()); }
        }

        public Guid CurrencyGuid
        {
            get
            {
                string result = GetExpandoProperty("CurrencyGuid");
                if ((result != null)&&(result.Length == 36)) { return new Guid(result); }
                return new Guid("ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31"); //USD
            }
            set { SetExpandoProperty("CurrencyGuid", value.ToString()); }
        }

        
        public Currency GetCurrency()
        {
            if (currency == null) { currency = new Currency(CurrencyGuid); }
            return currency;
        }

        public bool ForceContentVersioning
        {
            get
            {

                string b = GetExpandoProperty("ForceContentVersioning");

                if ((b != null) && (b.Length > 0))
                {
                    return Convert.ToBoolean(b);
                }

                return false;
            }
            set { SetExpandoProperty("ForceContentVersioning", value.ToString()); }
        }

        public bool EnableContentWorkflow
        {
            get
            {

                string b = GetExpandoProperty("EnableContentWorkflow");

                if ((b != null) && (b.Length > 0))
                {
                    return Convert.ToBoolean(b);
                }

                return false;
            }
            set { SetExpandoProperty("EnableContentWorkflow", value.ToString()); }
        }

		#endregion

		#region Private Methods

        private void GetSiteSettings(Guid siteGuid)
        {
            using (IDataReader result = DBSiteSettings.GetSite(siteGuid))
            {
                LoadSiteSettings(result);
            }
        }

		private void GetSiteSettings(string hostName)
		{
            using (IDataReader result = DBSiteSettings.GetSite(hostName))
            {
                LoadSiteSettings(result);
            }
		}

		private void GetSiteSettings(int siteId)
		{
            using (IDataReader result = DBSiteSettings.GetSite(siteId))
            {
                LoadSiteSettings(result);
            }
		}


        private void LoadSiteSettings(IDataReader reader)
        {
            if (reader == null) return;

            if (reader.Read())
            {
                this.siteID = Convert.ToInt32(reader["SiteID"]);
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.siteName = reader["SiteName"].ToString();
                this.skin = reader["Skin"].ToString();
                this.logo = reader["Logo"].ToString();
                this.icon = reader["Icon"].ToString();
                string editorProvider = reader["EditorProvider"].ToString().Trim();
                if (editorProvider.Length > 0)
                {
                    this.editorProviderName = editorProvider;
                }

                if (reader["EditorSkin"] != DBNull.Value)
                {
                    try
                    {

                        this.editorSkin =
                            (ContentEditorSkin)Enum.Parse(typeof(ContentEditorSkin),
                                                          reader["EditorSkin"].ToString());

                    }
                    catch (ArgumentException)
                    { }
                }

               
                this.enableMyPageFeature = Convert.ToBoolean(reader["EnableMyPageFeature"]);
                this.allowUserSkins = Convert.ToBoolean(reader["AllowUserSkins"]);
                this.allowPageSkins = Convert.ToBoolean(reader["AllowPageSkins"]);
                this.allowHideMenuOnPages = Convert.ToBoolean(reader["AllowHideMenuOnPages"]);
                this.allowNewRegistration = Convert.ToBoolean(reader["AllowNewRegistration"]);
                this.useSecureRegistration = Convert.ToBoolean(reader["UseSecureRegistration"]);
                this.useEmailForLogin = Convert.ToBoolean(reader["UseEmailForLogin"]);
                this.reallyDeleteUsers = Convert.ToBoolean(reader["ReallyDeleteUsers"]);
                this.useSSLOnAllPages = Convert.ToBoolean(reader["UseSSLOnAllPages"]);
                this.isServerAdminSite = Convert.ToBoolean(reader["IsServerAdminSite"]);

                
                //this.metaKeyWords = reader["DefaultPageKeyWords"].ToString();
                //this.metaDescription = reader["DefaultPageDescription"].ToString();
                //this.metaEncoding = reader["DefaultPageEncoding"].ToString();
                //this.metaAdditional = reader["DefaultAdditionalMetaTags"].ToString();

                //string useLdap = reader["UseLdapAuth"].ToString();
                this.useLdapAuth = Convert.ToBoolean(reader["UseLdapAuth"]);

                this.autoCreateLDAPUserOnFirstLogin = Convert.ToBoolean(reader["AutoCreateLDAPUserOnFirstLogin"]);

                this.ldapSettings.Server = reader["LdapServer"].ToString();
                if (reader["LdapPort"] != DBNull.Value)
                {
                    this.ldapSettings.Port = Convert.ToInt32(reader["LdapPort"]);
                }
                this.ldapSettings.Domain = reader["LdapDomain"].ToString();
                this.ldapSettings.RootDN = reader["LdapRootDN"].ToString();
                this.ldapSettings.UserDNKey = reader["LdapUserDNKey"].ToString();

                //this.allowPasswordRetrieval = (
                //                                  (string.Equals(reader["AllowPasswordRetrieval"].ToString(),"true", StringComparison.InvariantCultureIgnoreCase)) ||
                //                                  (reader["AllowPasswordRetrieval"].ToString() == "1"));

                //this.allowPasswordReset = (
                //                              (string.Equals(reader["AllowPasswordReset"].ToString(),"true", StringComparison.InvariantCultureIgnoreCase)) ||
                //                              (reader["AllowPasswordReset"].ToString() == "1"));

                //this.requiresQuestionAndAnswer = (
                //                                     (string.Equals(reader["RequiresQuestionAndAnswer"].ToString(), "true", StringComparison.InvariantCultureIgnoreCase)) ||
                //                                     (reader["RequiresQuestionAndAnswer"].ToString() == "1"));

                //this.requiresUniqueEmail = (
                //                               (string.Equals(reader["RequiresUniqueEmail"].ToString(),"true", StringComparison.InvariantCultureIgnoreCase)) ||
                //                               (reader["RequiresUniqueEmail"].ToString() == "1"));

                this.allowPasswordRetrieval = Convert.ToBoolean(reader["AllowPasswordRetrieval"]);
                this.allowPasswordReset = Convert.ToBoolean(reader["AllowPasswordReset"]);
                this.requiresQuestionAndAnswer = Convert.ToBoolean(reader["RequiresQuestionAndAnswer"]);
                this.requiresUniqueEmail = Convert.ToBoolean(reader["RequiresUniqueEmail"]);
                
                
                this.maxInvalidPasswordAttempts = Convert.ToInt32(reader["MaxInvalidPasswordAttempts"]);
                this.passwordAttemptWindowMinutes = Convert.ToInt32(reader["PasswordAttemptWindowMinutes"]);
                this.passwordFormat = Convert.ToInt32(reader["PasswordFormat"]);

                this.minRequiredPasswordLength = Convert.ToInt32(reader["MinRequiredPasswordLength"]);
                this.minRequiredNonAlphanumericCharacters = Convert.ToInt32(reader["MinReqNonAlphaChars"]);
                this.passwordStrengthRegularExpression = reader["PwdStrengthRegex"].ToString();

                this.defaultEmailFromAddress = reader["DefaultEmailFromAddress"].ToString();
               

                string dp = reader["DatePickerProvider"].ToString();
                if (dp.Length > 0)
                {
                    this.datePickerProvider = dp;
                }

                string cp = reader["CaptchaProvider"].ToString();
                if (cp.Length > 0)
                {
                    this.captchaProvider = cp;
                }

                this.recaptchaPrivateKey = reader["RecaptchaPrivateKey"].ToString();
                this.recaptchaPublicKey = reader["RecaptchaPublicKey"].ToString();
                this.wordpressAPIKey = reader["WordpressAPIKey"].ToString();
                this.windowsLiveAppID = reader["WindowsLiveAppID"].ToString();
                this.windowsLiveKey = reader["WindowsLiveKey"].ToString();
                
               

                this.allowOpenIDAuth = Convert.ToBoolean(reader["AllowOpenIDAuth"]);
                this.allowWindowsLiveAuth = Convert.ToBoolean(reader["AllowWindowsLiveAuth"]);
                this.allowUserFullNameChange = Convert.ToBoolean(reader["AllowUserFullNameChange"]);


                this.gmapApiKey = reader["GmapApiKey"].ToString();
                this.apiKeyExtra1 = reader["ApiKeyExtra1"].ToString();
                this.apiKeyExtra2 = reader["ApiKeyExtra2"].ToString();
                this.apiKeyExtra3 = reader["ApiKeyExtra3"].ToString();
                this.apiKeyExtra4 = reader["ApiKeyExtra4"].ToString();
                this.apiKeyExtra5 = reader["ApiKeyExtra5"].ToString();
                if (reader["DefaultFriendlyUrlPatternEnum"] != DBNull.Value)
                {
                    this.defaultFriendlyUrlPattern = (SiteSettings.FriendlyUrlPattern)Enum.Parse(typeof(SiteSettings.FriendlyUrlPattern), reader["DefaultFriendlyUrlPatternEnum"].ToString());
                }

                this.disableDbAuth = Convert.ToBoolean(reader["DisableDbAuth"]);

                this.extendedPropertiesLoaded = true;

            }

            
        }


        private void LoadExtendedProperties()
        {
            if (this.siteID > 0)
            {
                this.GetSiteSettings(this.siteID);
            }

            this.extendedPropertiesLoaded = true;
        }

        private bool Create()
        {
            this.siteGuid = Guid.NewGuid();

            int newID = DBSiteSettings.Create(
                this.siteGuid,
                this.siteName,
                this.skin,
                this.logo,
                this.icon,
                this.allowNewRegistration,
                this.allowUserSkins,
                this.allowPageSkins,
                this.allowHideMenuOnPages,
                this.useSecureRegistration,
                this.useSSLOnAllPages,
                this.metaKeyWords,
                this.metaDescription,
                this.metaEncoding,
                this.metaAdditional,
                this.isServerAdminSite,
                this.useLdapAuth,
                this.autoCreateLDAPUserOnFirstLogin,
                this.ldapSettings.Server,
                this.ldapSettings.Port,
                this.ldapSettings.Domain,
                this.ldapSettings.RootDN,
                this.ldapSettings.UserDNKey,
                this.allowUserFullNameChange,
                this.useEmailForLogin,
                this.reallyDeleteUsers,
                this.editorSkin.ToString(),
                this.defaultFriendlyUrlPattern.ToString(),
                this.enableMyPageFeature,
                this.editorProviderName,
                this.datePickerProvider,
                this.captchaProvider,
                this.recaptchaPrivateKey,
                this.recaptchaPublicKey,
                this.wordpressAPIKey,
                this.windowsLiveAppID,
                this.windowsLiveKey,
                this.allowOpenIDAuth,
                this.allowWindowsLiveAuth,
                this.gmapApiKey,
                this.apiKeyExtra1,
                this.apiKeyExtra2,
                this.apiKeyExtra3,
                this.apiKeyExtra4,
                this.apiKeyExtra5,
                this.disableDbAuth);

            bool result = (newID > 0);

            if (result)
            {
                this.siteID = newID;

                if (this.extendedPropertiesAreDirty)
                {
                    bool updatedExtended = UpdateExtendedProperties();
                }

                EnsureExpandoSettings();
                SaveExpandoProperties();
                
            }

            return result;
        }


	    private  bool Update()
		{
		    bool success = DBSiteSettings.Update(
		        this.siteID,
		        this.siteName,
		        this.skin,
		        this.logo,
		        this.icon,
		        this.allowNewRegistration,
		        this.allowUserSkins,
		        this.allowPageSkins,
		        this.allowHideMenuOnPages,
		        this.useSecureRegistration,
		        this.useSSLOnAllPages,
		        this.metaKeyWords,
		        this.metaDescription,
		        this.metaEncoding,
		        this.metaAdditional,
		        this.isServerAdminSite,
		        this.useLdapAuth,
		        this.autoCreateLDAPUserOnFirstLogin,
		        this.ldapSettings.Server,
		        this.ldapSettings.Port,
		        this.ldapSettings.Domain,
		        this.ldapSettings.RootDN,
		        this.ldapSettings.UserDNKey,
		        this.allowUserFullNameChange,
		        this.useEmailForLogin,
		        this.reallyDeleteUsers,
		        this.editorSkin.ToString(),
		        this.defaultFriendlyUrlPattern.ToString(),
		        this.enableMyPageFeature,
		        this.editorProviderName,
                this.datePickerProvider,
                this.captchaProvider,
                this.recaptchaPrivateKey,
                this.recaptchaPublicKey,
                this.wordpressAPIKey,
                this.windowsLiveAppID,
                this.windowsLiveKey,
                this.allowOpenIDAuth,
                this.allowWindowsLiveAuth,
                this.gmapApiKey,
                this.apiKeyExtra1,
                this.apiKeyExtra2,
                this.apiKeyExtra3,
                this.apiKeyExtra4,
                this.apiKeyExtra5,
                this.disableDbAuth);

            if (success && this.extendedPropertiesAreDirty)
            {
                success = UpdateExtendedProperties();
            }

            if (success)
            {
                SaveExpandoProperties();
            }

            return success;
		}

        private bool UpdateExtendedProperties()
        {
            bool success = DBSiteSettings.UpdateExtendedProperties(
                this.siteID,
                this.allowPasswordRetrieval,
                this.allowPasswordReset,
                this.requiresQuestionAndAnswer,
                this.maxInvalidPasswordAttempts,
                this.passwordAttemptWindowMinutes,
                this.requiresUniqueEmail,
                this.passwordFormat,
                this.minRequiredPasswordLength,
                this.minRequiredNonAlphanumericCharacters,
                this.passwordStrengthRegularExpression,
                this.defaultEmailFromAddress);

            return success;
        }

	    #endregion

		#region Public Methods

		public bool Save()
		{
			if(this.siteID > 0)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}

        
		

		#endregion

        #region ExpandoProperties

        private DataTable exapandoProperties = null;

        private void EnsureExpandoProperties()
        {
            if (exapandoProperties == null)
            {
                exapandoProperties = GetExpandoProperties(siteID);
            }

        }

        private void SaveExpandoProperties()
        {
            if (exapandoProperties == null)
            {
                log.Info("SiteSettings expandoProperties was null so nothing was saved");
                return;
            }

            foreach (DataRow row in exapandoProperties.Rows)
            {
                bool isDirty = Convert.ToBoolean(row["IsDirty"]);
                if (isDirty)
                {
                    DBSiteSettingsEx.SaveExpandoProperty(
                        siteID,
                        siteGuid,
                        row["GroupName"].ToString(),
                        row["KeyName"].ToString(),
                        row["KeyValue"].ToString());

                }

            }

        }

        public string GetExpandoProperty(string keyName)
        {
            EnsureExpandoProperties();

            foreach (DataRow row in exapandoProperties.Rows)
            {
                if (row["KeyName"].ToString().Trim().Equals(keyName, StringComparison.InvariantCulture))
                {
                    return row["KeyValue"].ToString();
                }

            }

            return null;

        }

        public void SetExpandoProperty(string keyName, string keyValue)
        {
            EnsureExpandoProperties();
            //bool found = false;
            foreach (DataRow row in exapandoProperties.Rows)
            {
                if (row["KeyName"].ToString().Trim().Equals(keyName, StringComparison.InvariantCulture))
                {
                    row["KeyValue"] = keyValue;
                    row["IsDirty"] = true;
                    //found = true;
                    break;
                }

            }

            //if (!found)
            //{
            //    DBSiteSettingsEx.SaveExpandoProperty(
            //            siteID,
            //            siteGuid,
            //            "General",
            //            keyName,
            //            keyValue);

            //}

        }


        private static DataTable GetExpandoProperties(int siteId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SiteID", typeof(int));
            dataTable.Columns.Add("KeyName", typeof(string));
            dataTable.Columns.Add("KeyValue", typeof(string));
            dataTable.Columns.Add("GroupName", typeof(string));
            dataTable.Columns.Add("IsDirty", typeof(bool));

            using (IDataReader reader = DBSiteSettingsEx.GetSiteSettingsExList(siteId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["SiteID"] = reader["SiteID"];
                    row["KeyName"] = reader["KeyName"];
                    row["KeyValue"] = reader["KeyValue"];
                    row["GroupName"] = reader["GroupName"];

                    row["IsDirty"] = false;

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }


        #endregion


        #region Static Methods


        public static string GetDataFolder(int siteId)
        {
            return String.Format("~/Data/Sites/{0}/", siteId.ToString(CultureInfo.InvariantCulture));
        }


        public static IDataReader GetSiteList() 
		{
			return DBSiteSettings.GetSiteList();
		}

        public static DataTable GetSiteIdList()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("SiteID", typeof(int));

            using (IDataReader reader = GetSiteList())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["SiteID"] = reader["SiteID"];
                    dataTable.Rows.Add(row);
                }
            }


            return dataTable;

        }

        public static Guid GetRootSiteGuid()
        {
            Guid result = Guid.Empty;

            using (IDataReader reader = DBSiteSettings.GetSiteList())
            {
                while (reader.Read())
                {
                    if (Convert.ToBoolean(reader["IsServerAdminSite"]))
                    {
                        result = new Guid(reader["SiteGuid"].ToString());
                        break;
                    }


                }
            }

            return result;
        }

        public static int SiteCount()
        {
            int sitesFound = 0;

            try
            {
                using (IDataReader reader = DBSiteSettings.GetSiteList())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            sitesFound += 1;

                        }
                    }
                }

            }
            catch (System.Data.Common.DbException) { }
            catch (InvalidOperationException) { }
            catch (System.Security.SecurityException) { }
            catch(System.Net.Sockets.SocketException) {}


            return sitesFound;


        }

        public static void AddFeature(Guid siteGuid, Guid featureGuid) 
		{
            DBSiteSettings.AddFeature(siteGuid, featureGuid);
		}

        public static void RemoveFeature(Guid siteGuid, Guid featureGuid) 
		{
            DBSiteSettings.RemoveFeature(siteGuid, featureGuid);
		}

		public static IDataReader GetHostList(int siteId) 
		{
			return DBSiteSettings.GetHostList(siteId);
		}

		public static void AddHost(Guid siteGuid, int siteId, string hostName) 
		{
			DBSiteSettings.AddHost(siteGuid, siteId, hostName);
		}

		public static void RemoveHost(int hostId) 
		{
			DBSiteSettings.DeleteHost(hostId);
		}

        public static int CreateNewSite()
        {
            return CreateNewSite("Cynthia") ;
        }

        public static int CreateNewSite(String siteName)
        {
            SiteSettings newSite = new SiteSettings { SiteName = siteName };
            newSite.Save();

            return newSite.SiteId;

        }

        public static void Delete(int siteId)
        {
            DBSiteSettings.Delete(siteId);
        }

        public static void EnsureExpandoSettings()
        {
            DBSiteSettingsEx.EnsureSettings();
        }

        /// <summary>
        /// when using related sites mode this mthod is used to sync shared settings across sites when the parent site is updated
        /// </summary>
        /// <param name="relatedSiteId"></param>
        /// <param name="usingFolderSites"></param>
        /// <param name="allowNewRegistration"></param>
        /// <param name="useSecureRegistration"></param>
        /// <param name="useLdapAuth"></param>
        /// <param name="autoCreateLdapUserOnFirstLogin"></param>
        /// <param name="ldapServer"></param>
        /// <param name="ldapDomain"></param>
        /// <param name="ldapPort"></param>
        /// <param name="ldapRootDn"></param>
        /// <param name="ldapUserDnKey"></param>
        /// <param name="allowUserFullNameChange"></param>
        /// <param name="useEmailForLogin"></param>
        /// <param name="allowOpenIdAuth"></param>
        /// <param name="allowWindowsLiveAuth"></param>
        /// <param name="windowsLiveAppId"></param>
        /// <param name="windowsLiveKey"></param>
        /// <param name="rpxNowApplicationName"></param>
        /// <param name="rpxNowApiKey"></param>
        public static void SyncRelatedSites(
            SiteSettings masterSite,
            bool usingFolderSites
            )
        {
            //TODO: need to pass more params like password validation stuff
            DBSiteSettings.UpdateRelatedSites(
                masterSite.siteID,
                masterSite.allowNewRegistration,
                masterSite.useSecureRegistration,
                masterSite.useLdapAuth,
                masterSite.autoCreateLDAPUserOnFirstLogin,
                masterSite.SiteLdapSettings.Server,
                masterSite.SiteLdapSettings.Domain,
                masterSite.SiteLdapSettings.Port,
                masterSite.SiteLdapSettings.RootDN,
                masterSite.SiteLdapSettings.UserDNKey,
                masterSite.allowUserFullNameChange,
                masterSite.useEmailForLogin,
                masterSite.allowOpenIDAuth,
                masterSite.allowWindowsLiveAuth,
                masterSite.allowPasswordRetrieval,
                masterSite.allowPasswordReset,
                masterSite.requiresQuestionAndAnswer,
                masterSite.maxInvalidPasswordAttempts,
                masterSite.passwordAttemptWindowMinutes,
                masterSite.requiresUniqueEmail,
                masterSite.passwordFormat,
                masterSite.minRequiredPasswordLength,
                masterSite.minRequiredNonAlphanumericCharacters,
                masterSite.passwordStrengthRegularExpression);


            if (usingFolderSites)
            {
                DBSiteSettings.UpdateRelatedSitesWindowsLive(masterSite.siteID, masterSite.windowsLiveAppID, masterSite.windowsLiveKey);

                DBSiteSettingsEx.UpdateRelatedSitesProperty(masterSite.siteID, "RpxNowApplicationName", masterSite.RpxNowApplicationName);
                DBSiteSettingsEx.UpdateRelatedSitesProperty(masterSite.siteID, "RpxNowApiKey", masterSite.RpxNowApiKey);
            }


        }

		#endregion


	}
}
