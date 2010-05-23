// Author:					Joe Audette
// Created:				    2004-07-19
// Last Modified:		    2010-02-03
// 
// TJ Fontaine contributed the LDAP authentication code
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using log4net;
using Cynthia.Data;
using Cynthia.Business.Properties;

namespace Cynthia.Business
{
	
    [Serializable]
	public class SiteUser 
	{
		
		#region Constructors

        private SiteUser()
        { }

		public SiteUser(SiteSettings settings)
		{
			if(settings != null)
			{
				siteSettings = settings;
				siteID = siteSettings.SiteId;
                siteGuid = siteSettings.SiteGuid;
                if (UseRelatedSiteMode) { siteID = RelatedSiteID; }
			}
		}

        public SiteUser(SiteSettings settings, bool overridRelatedSiteMode)
        {
            if (settings != null)
            {
                siteSettings = settings;
                siteID = siteSettings.SiteId;
                siteGuid = siteSettings.SiteGuid;
                if ((UseRelatedSiteMode) &&(!overridRelatedSiteMode))
                { 
                    siteID = RelatedSiteID; 
                }
            }
        }

		public SiteUser(SiteSettings settings, int userId)
		{
			if(settings != null)
			{
				siteSettings = settings;
                siteGuid = siteSettings.SiteGuid;
				siteID = siteSettings.SiteId;
                if (UseRelatedSiteMode) { siteID = RelatedSiteID; }
				GetUser(userId);
			}
		}

        
		public SiteUser(SiteSettings settings, string login)
		{
			if(settings != null)
			{
				siteSettings = settings;
                siteGuid = siteSettings.SiteGuid;
				siteID = siteSettings.SiteId;
                if (UseRelatedSiteMode) { siteID = RelatedSiteID; }
				GetUser(login);
			}
			
		}

        public SiteUser(SiteSettings settings, Guid userGuid)
        {
            if (settings != null)
            {
                siteSettings = settings;
                siteID = siteSettings.SiteId;
                if (UseRelatedSiteMode) { siteID = RelatedSiteID; }
                GetUser(userGuid);
            }

        }


		#endregion

		#region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(SiteUser));

        [NonSerialized]
		private SiteSettings siteSettings;
        private Guid siteGuid = Guid.Empty;
		private int userID = -1;
		private int siteID = -1;
		private Guid userGuid = Guid.Empty;
		private string name = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
		private String loginName = String.Empty;
		private string email = string.Empty;
		private string password = string.Empty;
		private string gender = string.Empty;
		private bool profileApproved = true;
		private bool approvedForGroups = true;
		private bool trusted = false;
		private bool displayInMemberList = true;
		private string webSiteUrl = string.Empty;
		private string country = string.Empty;
		private string state = string.Empty;
		private string occupation = string.Empty;
		private string interests = string.Empty;
		private string msn = string.Empty;
		private string yahoo = string.Empty;
		private string aim = string.Empty;
		private string icq = string.Empty;
		private int totalPosts = 0;
		private string avatarUrl = string.Empty;
		private double timeOffsetHours = 0;
		private string signature = string.Empty;
		private DateTime dateCreated = DateTime.UtcNow;
		private string skin = string.Empty;
		private bool isDeleted = false;

        private String loweredEmail = String.Empty;
        private String passwordQuestion = String.Empty;
        private String passwordAnswer = String.Empty;
        private DateTime lastActivityDate = DateTime.MinValue;
        private DateTime lastLoginDate = DateTime.MinValue;
        private DateTime lastPasswordChangedDate = DateTime.MinValue;
        private DateTime lastLockoutDate = DateTime.MinValue;
        private int failedPasswordAttemptCount;
        private DateTime failedPasswordAttemptWindowStart = DateTime.MinValue;
        private int failedPasswordAnswerAttemptCount;
        private DateTime failedPasswordAnswerAttemptWindowStart = DateTime.MinValue;
        private bool isLockedOut = false;
        private String mobilePIN = String.Empty;
        private String passwordSalt = String.Empty;
        private String comment = String.Empty;
        private Guid registerConfirmGuid = Guid.Empty;

        private bool nonLazyLoadedProfilePropertiesLoaded = false;
        private DataTable profileProperties = null;
        private ArrayList userRoles = new ArrayList();
        private bool rolesLoaded = false;

        private string openIDURI = string.Empty;
        private string windowsLiveID = string.Empty;
        private decimal totalRevenue = 0;
        private bool mustChangePwd = false;

        private string timeZoneId = "Eastern Standard Time"; //default
        private string newEmail = string.Empty;
        private Guid emailChangeGuid = Guid.Empty;
        private string editorPreference = string.Empty; // use site default

        

        private static bool UseRelatedSiteMode
        {
            get
            {
                if (
                    (ConfigurationManager.AppSettings["UseRelatedSiteMode"] != null)
                    && (ConfigurationManager.AppSettings["UseRelatedSiteMode"] == "true")
                )
                {
                    return true;
                }

                return false;
            }
        }

        

        private static int RelatedSiteID
        {
            get
            {
                int result = 1;
                if (ConfigurationManager.AppSettings["RelatedSiteID"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["RelatedSiteID"], out result);
                }

                return result;
            }
        }
        
       
		#endregion

		#region Public Properties

		public int UserId
		{	
			get {return userID;}
		}

        public decimal TotalRevenue
        {
            get { return totalRevenue; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            //set { siteGuid = value; }
        }

		public int SiteId
		{	
			get {return siteID;}
		}

		public bool IsDeleted
		{	
			get {return isDeleted;}
		}

		public Guid UserGuid
		{	
			get {return userGuid;}
		}

        public Guid RegisterConfirmGuid
        {
            get { return registerConfirmGuid; }
        }

		public string Name
		{	
			get {return name;}
			set
			{
				name = value;
			}
		}

		public string LoginName
		{	
			get {return loginName;}
			set
			{
                // don't allow @ because in some configuration
                // we allow login by either email or loginname
                // we want login name to not look like an email address
				loginName = value.Replace("@", ".");
			}
		}

		public string Email
		{	
			get {return email;}
			set 
			{
				email = value;
			}
		}

        public string TimeZoneId
        {
            get { return timeZoneId; }
            set { timeZoneId = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string NewEmail
        {
            get { return newEmail; }
            set { newEmail = value; }
        }

        public string EditorPreference
        {
            get { return editorPreference; }
            set { editorPreference = value; }
        }

        public Guid EmailChangeGuid
        {
            get { return emailChangeGuid; }
            set { emailChangeGuid = value; }
        }

		public string Password
		{	
			get {return password;}
			set
			{
				password = value;

			}
		}

        public bool MustChangePwd
        {
            get { return mustChangePwd; }
            set { mustChangePwd = value; }
        }

		public string Gender
		{	
			get {return gender;}
			set {gender = value;}
		}

		public bool ProfileApproved
		{	
			get {return profileApproved;}
			set {profileApproved = value;}
		}

		public bool ApprovedForGroups
		{	
			get {return approvedForGroups;}
			set {approvedForGroups = value;}
		}

		public bool Trusted
		{	
			get {return trusted;}
			set {trusted = value;}
		}

		public bool DisplayInMemberList
		{	
			get {return displayInMemberList;}
			set {displayInMemberList = value;}
		}

		
		public string WebSiteUrl
		{	
			get {return webSiteUrl;}
			set {webSiteUrl = value;}
		}

		public string Country
		{	
			get {return country;}
			set {country = value;}
		}

		public string State
		{	
			get {return state;}
			set {state = value;}
		}

		public string Occupation
		{	
			get {return occupation;}
			set {occupation = value;}
		}

		public string Interests
		{	
			get {return interests;}
			set {interests = value;}
		}

		public string MSN
		{	
			get {return msn;}
			set {msn = value;}
		}

		public string Yahoo
		{	
			get {return yahoo;}
			set {yahoo = value;}
		}

		public string AIM
		{	
			get {return aim;}
			set {aim = value;}
		}

		public string ICQ
		{	
			get {return icq;}
			set {icq = value;}
		}

		public int TotalPosts
		{	
			get {return totalPosts;}
			set {totalPosts = value;}
		}

		public string AvatarUrl
		{	
			get {return avatarUrl;}
			set {avatarUrl = value;}
		}

        /*
         * Originally implemented TimeOffsetHours as int in db
         * but need double so using ad hoc property
         * 
         */
        public double TimeOffsetHours
		{	
			get 
            {
                timeOffsetHours = Convert.ToDouble(GetProperty("TimeOffsetHours", SettingsSerializeAs.String),CultureInfo.InvariantCulture);
                return timeOffsetHours;
            }
			set 
            {
                timeOffsetHours = value;
                SetProperty("TimeOffsetHours", value, SettingsSerializeAs.String);
            
            }
        }

        public bool EnableLiveMessengerOnProfile
        {
            get
            {
                object o = GetProperty("EnableLiveMessengerOnProfile", SettingsSerializeAs.String);
                if (o != null) { return Convert.ToBoolean(o,CultureInfo.InvariantCulture); }
                return false;

            }
            set
            {

                SetProperty("EnableLiveMessengerOnProfile", value, SettingsSerializeAs.String);

            }
        }

        public string LiveMessengerId
        {
            get
            {
                object o = GetProperty("LiveMessengerId", SettingsSerializeAs.String);
                if (o != null) { return o.ToString(); }
                return string.Empty;
                
            }
            set
            {
               
                SetProperty("LiveMessengerId", value, SettingsSerializeAs.String);

            }
        }

        public string LiveMessengerDelegationToken
        {
            get
            {
                object o = GetProperty("LiveMessengerDelegationToken", SettingsSerializeAs.String);
                if (o != null) { return o.ToString(); }
                return string.Empty;

            }
            set
            {

                SetProperty("LiveMessengerDelegationToken", value, SettingsSerializeAs.String);

            }
        }


        public string Signature
		{	
			get {return signature;}
			set {signature = value;}
		}

		public DateTime DateCreated
		{	
			get {return dateCreated;}
		}


		public string Skin
		{	
			get {return skin;}
			set {skin = value;}
		}

        public String LoweredEmail
        {
            get { return loweredEmail; }
        }

        public string PasswordQuestion
        {
            get { return passwordQuestion; }
            set { passwordQuestion = value; }
        }

        public string PasswordAnswer
        {
            get { return passwordAnswer; }
            set { passwordAnswer = value; }
        }

        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
        }

        public DateTime LastLoginDate
        {
            get { return lastLoginDate; }
        }

        public DateTime LastPasswordChangedDate
        {
            get { return lastPasswordChangedDate; }
        }

        public DateTime LastLockoutDate
        {
            get { return lastLockoutDate; }
        }

        public int FailedPasswordAttemptCount
        {
            get { return failedPasswordAttemptCount; }
        }

        public DateTime FailedPasswordAttemptWindowStart
        {
            get { return failedPasswordAttemptWindowStart; }
        }

        public int FailedPasswordAnswerAttemptCount
        {
            get { return failedPasswordAnswerAttemptCount; }
        }

        public DateTime FailedPasswordAnswerAttemptWindowStart
        {
            get { return failedPasswordAnswerAttemptWindowStart; }
        }

        public bool IsLockedOut
        {
            get { return isLockedOut; }
        }

        public string MobilePin
        {
            get { return mobilePIN; }
            set { mobilePIN = value; }
        }

        public string PasswordSalt
        {
            get { return passwordSalt; }
            set { passwordSalt = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public string OpenIdUri
        {
            get { return openIDURI; }
            set { openIDURI = value; }
        }

        public string WindowsLiveId
        {
            get { return windowsLiveID; }
            set { windowsLiveID = value; }
        }

		#endregion

		#region Private Methods

        

		private void GetUser(string loginInfo)
		{
			IDataReader reader;

			if(
                (siteSettings.UseEmailForLogin)
                &&(!siteSettings.UseLdapAuth)
                &&(loginInfo.Contains("@"))
                )
			{
                using (reader = DBSiteUser.GetSingleUser(siteID, loginInfo))
                {
                    GetUser(reader);
                }
			}
			else
			{
                using (reader = DBSiteUser.GetSingleUserByLoginName(siteID, loginInfo))
                {
                    GetUser(reader);
                }
			}

		}

		

		private void GetUser(int userId)
		{
            using (IDataReader reader = DBSiteUser.GetSingleUser(userId))
            {
                GetUser(reader);
            }

		}

        private void GetUser(Guid userGuid)
        {
            using (IDataReader reader = DBSiteUser.GetSingleUser(userGuid))
            {
                GetUser(reader);
            }
        }

        private void GetUser(IDataReader reader)
        {

            if (reader.Read())
            {
                this.userID = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
                this.siteID = Convert.ToInt32(reader["SiteID"], CultureInfo.InvariantCulture);
                try
                {
                    this.totalRevenue = Convert.ToDecimal(reader["TotalRevenue"], CultureInfo.InvariantCulture);
                }
                catch (Exception)
                { }
                

                this.name = reader["Name"].ToString();
                this.loginName = reader["LoginName"].ToString();
                this.email = reader["Email"].ToString();
                this.password = reader["Pwd"].ToString();
                this.gender = reader["Gender"].ToString().Trim();
                // this is to support dbs that don't have bit data type
                string pApproved = reader["ProfileApproved"].ToString();
                this.profileApproved = (pApproved == "True" || pApproved == "1");
                string fApproved = reader["ApprovedForGroups"].ToString();
                this.approvedForGroups = (fApproved == "True" || fApproved == "1");
                string t = reader["Trusted"].ToString();
                this.trusted = (t == "True" || t == "1");
                string display = reader["DisplayInMemberList"].ToString();
                this.displayInMemberList = (display == "True" || display == "1");

                this.webSiteUrl = reader["WebSiteURL"].ToString();
                this.country = reader["Country"].ToString();
                this.state = reader["State"].ToString();
                this.occupation = reader["Occupation"].ToString();
                this.interests = reader["Interests"].ToString();
                this.msn = reader["MSN"].ToString();
                this.yahoo = reader["Yahoo"].ToString();
                this.aim = reader["AIM"].ToString();
                this.icq = reader["ICQ"].ToString();
                this.totalPosts = Convert.ToInt32(reader["TotalPosts"], CultureInfo.InvariantCulture);
                this.AvatarUrl = reader["AvatarUrl"].ToString();

                this.timeOffsetHours = Convert.ToInt32(reader["TimeOffsetHours"], CultureInfo.InvariantCulture);
                this.signature = reader["Signature"].ToString();

                try
                {
                    this.dateCreated = (DateTime)reader["DateCreated"];
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("DateCreated was invalid", ex);
                    }
                }


                try
                {
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                }
                catch (FormatException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("UserGuid was invalid", ex);
                    }
                }

                this.skin = reader["Skin"].ToString();

                string deleted = reader["IsDeleted"].ToString();
                this.isDeleted = (deleted == "True" || deleted == "1");

                this.isLockedOut = (
                    (string.Equals(reader["IsLockedOut"].ToString(), "true", StringComparison.InvariantCultureIgnoreCase)) ||
                    (reader["IsLockedOut"].ToString() == "1"));

                this.loweredEmail = reader["LoweredEmail"].ToString();
                this.passwordQuestion = reader["PasswordQuestion"].ToString();
                this.passwordAnswer = reader["PasswordAnswer"].ToString();
                try
                {
                    if (reader["LastActivityDate"] != DBNull.Value)
                    {
                        this.lastActivityDate = (DateTime)reader["LastActivityDate"];
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("LastActivityDate was invalid", ex);
                    }
                }

                try
                {
                    if (reader["LastLoginDate"] != DBNull.Value)
                    {
                        this.lastLoginDate = (DateTime)reader["LastLoginDate"];
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("LastLoginDate was invalid", ex);
                    }
                }

                try
                {
                    if (reader["LastPasswordChangedDate"] != DBNull.Value)
                    {
                        this.lastPasswordChangedDate = (DateTime)reader["LastPasswordChangedDate"];
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("LastPasswordChangedDate was invalid", ex);
                    }
                }

                try
                {
                    if (reader["LastLockoutDate"] != DBNull.Value)
                    {
                        this.lastLockoutDate = (DateTime)reader["LastLockoutDate"];
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("LastLockoutDate was invalid", ex);
                    }
                }

                try
                {
                    if (reader["FailedPasswordAttemptCount"] != DBNull.Value)
                    {
                        this.failedPasswordAttemptCount =
                            Convert.ToInt32(reader["FailedPasswordAttemptCount"], CultureInfo.InvariantCulture);
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("FailedPasswordAttemptCount was invalid", ex);
                    }
                }

                try
                {
                    if (reader["FailedPwdAttemptWindowStart"] != DBNull.Value)
                    {
                        this.failedPasswordAttemptWindowStart = (DateTime)reader["FailedPwdAttemptWindowStart"];
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("FailedPwdAttemptWindowStart was invalid", ex);
                    }
                }

                try
                {
                    if (reader["FailedPwdAnswerAttemptCount"] != DBNull.Value)
                    {
                        this.failedPasswordAnswerAttemptCount =
                            Convert.ToInt32(reader["FailedPwdAnswerAttemptCount"], CultureInfo.InvariantCulture);
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("FailedPwdAnswerAttemptCount was invalid", ex);
                    }
                }

                try
                {
                    if (reader["FailedPwdAnswerWindowStart"] != DBNull.Value)
                    {
                        this.failedPasswordAnswerAttemptWindowStart = (DateTime)reader["FailedPwdAnswerWindowStart"];
                    }
                }
                catch (InvalidCastException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("FailedPwdAnswerWindowStart was invalid", ex);
                    }
                }

                this.mobilePIN = reader["MobilePIN"].ToString();
                this.passwordSalt = reader["PasswordSalt"].ToString();
                this.comment = reader["Comment"].ToString();
                string confirmGuidString = reader["RegisterConfirmGuid"].ToString();
                if (confirmGuidString.Length == 36)
                {
                    this.registerConfirmGuid = new Guid(confirmGuidString);
                }

                this.openIDURI = reader["OpenIDURI"].ToString();
                this.windowsLiveID = reader["WindowsLiveID"].ToString();
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.mustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);

                this.emailChangeGuid = new Guid(reader["EmailChangeGuid"].ToString());
                this.newEmail = reader["NewEmail"].ToString();
                this.timeZoneId = reader["TimeZoneId"].ToString();
                this.editorPreference = reader["EditorPreference"].ToString();
                this.firstName = reader["FirstName"].ToString();
                this.lastName = reader["LastName"].ToString();
                
            }

        }

		private bool Create()
		{ 
			bool userCreated = false;
			int newID = 0;
            this.userGuid = Guid.NewGuid();

            this.dateCreated = DateTime.UtcNow;
			newID = DBSiteUser.AddUser(
                this.siteGuid,
				this.siteID,
				this.name,
				this.loginName,
				this.email,
				this.password,
                this.userGuid,
                this.dateCreated,
                this.mustChangePwd,
                this.firstName,
                this.lastName,
                this.timeZoneId);

			userCreated = (newID > 0);

			if(userCreated)
			{
                
				this.userID = newID;
                this.dateCreated = DateTime.Now;
                this.lastPasswordChangedDate = DateTime.Now;

                // not all properties are added on insert so update
                Update();

                Role.AddUserToDefaultRoles(this);
			}

			return userCreated;

		}


		private bool Update()
		{
            // timeOffset is now a double and stored in profile system
            int legacyTimeOffset = 0;

			bool userUpdated = DBSiteUser.UpdateUser(
				this.userID,
				this.name,
				this.loginName,
				this.email,
				this.password,
				this.gender,
				this.profileApproved,
				this.approvedForGroups,
				this.trusted,
				this.displayInMemberList,
				this.webSiteUrl,
				this.country,
				this.state,
				this.occupation,
				this.interests,
				this.msn,
				this.yahoo,
				this.aim,
				this.icq,
				this.avatarUrl,
				this.signature,
				this.skin,
                this.email.ToLower(),
                this.passwordQuestion,
                this.passwordAnswer,
                this.comment,
                legacyTimeOffset,
                this.openIDURI,
                this.windowsLiveID,
                this.mustChangePwd,
                this.firstName,
                this.lastName,
                this.timeZoneId,
                this.editorPreference,
                this.newEmail,
                this.emailChangeGuid);

			return userUpdated;

		}

        private void LoadRoles()
        {
            using (IDataReader reader = GetRolesByUser(this.siteID, this.userID))
            {
                while (reader.Read())
                {
                    userRoles.Add(reader["RoleName"].ToString().Trim());
                }

                rolesLoaded = true;
            }
        }



		#endregion


		#region Public Methods

		

		public bool Save()
		{
			
			if(this.userID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}

        public bool IsValid()
        {
            return (GetRuleViolations().Count() == 0); 
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            //this is not used yet but gives a model for validation when we implement MVC

            if (string.IsNullOrEmpty(loginName))
                yield return new RuleViolation(Resources.SiteUserLoginRequired);

            if (string.IsNullOrEmpty(email))
                yield return new RuleViolation(Resources.SiteUserEmailIsRequired);

            if (!RegexHelper.IsValidEmailAddressSyntax(email))
                yield return new RuleViolation(Resources.SiteUserInvalidEmailFormat);

            //TODO: more checks

            yield break;
        }

		public bool DeleteUser()
		{
			if(this.siteSettings.ReallyDeleteUsers)
			{
                SubscriberRepository subscriptions = new SubscriberRepository();

                subscriptions.DeleteByUser(userGuid);

				bool result = DBSiteUser.DeleteUser(this.userID);
				if(result)
				{
					Role.DeleteUserRoles(this.userID);

				}
				return result;
			}
			else
			{
				return DBSiteUser.FlagAsDeleted(this.userID);
			}
		}

        public bool UpdateLastActivityTime()
        {
            return DBSiteUser.UpdateLastActivityTime(this.userGuid, DateTime.UtcNow);
        }

        public bool UpdateLastLoginTime()
        {
            return DBSiteUser.UpdateLastLoginTime(this.userGuid, DateTime.UtcNow);
        }

        public bool LockoutAccount()
        {
            return DBSiteUser.AccountLockout(this.userGuid, DateTime.UtcNow);
        }

        public bool UnlockAccount()
        {
            return DBSiteUser.AccountClearLockout(this.userGuid);
        }

        public bool UpdatePasswordQuestionAndAnswer(String newQuestion, String newAnswer)
        {
            return DBSiteUser.UpdatePasswordQuestionAndAnswer(this.userGuid, newQuestion, newAnswer);

        }

        public bool UpdateLastPasswordChangeTime()
        {
            return DBSiteUser.UpdateLastPasswordChangeTime(this.userGuid, DateTime.UtcNow);
        }

        public void IncrementPasswordAttempts(SiteSettings siteSettings)
        {
            if (siteSettings == null) return;

            if (
                (this.failedPasswordAttemptCount == 0)
                || (this.failedPasswordAttemptWindowStart.AddMinutes(siteSettings.PasswordAttemptWindowMinutes) < DateTime.UtcNow)
                )
            {
                this.failedPasswordAttemptWindowStart = DateTime.UtcNow;
                DBSiteUser.UpdateFailedPasswordAttemptStartWindow(
                    this.userGuid, this.failedPasswordAttemptWindowStart);

                this.failedPasswordAttemptCount = 1;

            }
            else
            {
                this.failedPasswordAttemptCount += 1;
            }

            DBSiteUser.UpdateFailedPasswordAttemptCount(
                this.userGuid, this.failedPasswordAttemptCount);

                
            if (this.failedPasswordAttemptCount >= siteSettings.MaxInvalidPasswordAttempts)
            {
                LockoutAccount();
            }
        }

	    public void IncrementPasswordAnswerAttempts(SiteSettings siteSettings)
        {
            if (siteSettings != null)
            {
                if (
                    (this.failedPasswordAnswerAttemptCount == 0)
                    || (this.FailedPasswordAnswerAttemptWindowStart.AddMinutes(
                    siteSettings.PasswordAttemptWindowMinutes) < DateTime.UtcNow)
                    )
                {
                    this.failedPasswordAnswerAttemptWindowStart = DateTime.UtcNow;

                    DBSiteUser.UpdateFailedPasswordAnswerAttemptStartWindow(
                        this.userGuid, this.failedPasswordAnswerAttemptWindowStart);

                    this.failedPasswordAnswerAttemptCount = 1;

                }
                else
                {
                    this.failedPasswordAnswerAttemptCount += 1;
                }

                DBSiteUser.UpdateFailedPasswordAnswerAttemptCount(
                    this.userGuid, this.failedPasswordAnswerAttemptCount);


                if (this.failedPasswordAnswerAttemptCount >= siteSettings.MaxInvalidPasswordAttempts)
                {
                    LockoutAccount();
                }

            }

        }

        public bool SetRegistrationConfirmationGuid(Guid registrationGuid)
        {
            if (registrationGuid == Guid.Empty)
            {
                // empty guid indicates already confirmed registration
                // static ConfirmRegistration is the only method that should be allowed to
                // this method is locking the account in addition to setting the registration
                // Guid. If the correct guid is passed on the ConfirmRegistration.aspx 
                // it will unlock the account. best not to lock an account with an empty guid
                // because known guids can easily be entered in the url
                return false;
            }

            this.registerConfirmGuid = registrationGuid;
            return DBSiteUser.SetRegistrationConfirmationGuid(this.userGuid, registrationGuid);
        }


        public bool IsInRoles(String semicolonSeparatedRoleNames)
        {
            if (String.IsNullOrEmpty(semicolonSeparatedRoleNames)) return false;

            if (!rolesLoaded) LoadRoles();

            foreach (string roleToCheck in semicolonSeparatedRoleNames.Split(new char[] {';'}))
            {
                foreach (String userRole in userRoles)
                {
                    int compareResult = String.Compare(roleToCheck.Trim(), userRole, true, CultureInfo.InvariantCulture);
                    //if(roleToCheck.Trim() == userRole)
                    //{
                    //    result = true;
                    //}
                    if (compareResult == 0) return true;
                }
            }
            return false;
        }

	    #region CProfile methods

        public void SetProperty(
            String propertyName,
            Object propertyValue,
            SettingsSerializeAs serializeAs)
        {
            bool lazyLoad = false;
            SetProperty(propertyName, propertyValue, serializeAs, lazyLoad);

        }

        public void SetProperty(
            String propertyName, 
            Object propertyValue, 
            SettingsSerializeAs serializeAs,
            bool lazyLoad)
        {
            
            switch (propertyName)
            {
                //the properties identified in the case statements
                //were already implemented on SiteUser
                //before the exstensible profile was implemented
                
                case "UserID":
                    //no change allowed through profile
                    break;

                case "SiteID":
                    //no change allowed through profile
                    break;

                case "IsDeleted":
                    //no change allowed through profile
                    break;

                case "UserGuid":
                    //no change allowed through profile
                    break;

                case "RegisterConfirmGuid":
                    //no change allowed through profile
                    break;

                case "Name":
                    if (propertyValue is String)
                    {
                        this.Name = propertyValue.ToString();
                        this.Save();
                    }
                    break;

                case "LoginName":
                    //no change allowed through profile
                    break;
                   
                case "Email":
                    //no change allowed through profile
                    break;

                case "Password":
                    //no change allowed through profile
                    break;

                case "Gender":
                    if (propertyValue is String)
                    {
                        this.Gender = propertyValue.ToString();
                        this.Save();
                    }
                    break;
                    
                case "ProfileApproved":
                    //no change allowed through profile
                    break;

                case "ApprovedForGroups":
                    //no change allowed through profile
                    break;

                case "Trusted":
                    //no change allowed through profile
                    break;

                case "DisplayInMemberList":
                    //no change allowed through profile
                    break;

                case "WebSiteUrl":
                    if (propertyValue is String)
                    {
                        this.WebSiteUrl = propertyValue.ToString();
                        if (this.webSiteUrl.Length > 100)
                        {
                            this.webSiteUrl = this.webSiteUrl.Substring(0, 100);
                        }
                        this.Save();
                    }
                    break;
                    
                case "Country":
                    if (propertyValue is String)
                    {
                        this.Country = propertyValue.ToString();
                        if (this.country.Length > 100)
                        {
                            this.country = this.country.Substring(0, 100);
                        }
                        this.Save();
                    }
                    break;
                    
                case "State":
                    if (propertyValue is String)
                    {
                        this.State = propertyValue.ToString();
                        if (this.state.Length > 100)
                        {
                            this.state = this.state.Substring(0, 100);
                        }
                        this.Save();
                    }
                    break;
                    
                case "Occupation":
                    if (propertyValue is String)
                    {
                        this.Occupation = propertyValue.ToString();
                        if (this.occupation.Length > 100)
                        {
                            this.occupation = this.occupation.Substring(0, 100);
                        }
                        this.Save();
                    }
                    break;
                    
                case "Interests":
                    if (propertyValue is String)
                    {
                        this.Interests = propertyValue.ToString();
                        if (this.interests.Length > 100)
                        {
                            this.interests = this.interests.Substring(0, 100);
                        }
                        this.Save();
                    }
                    break;
                    
                case "MSN":
                    if (propertyValue is String)
                    {
                        this.MSN = propertyValue.ToString();
                        if (this.msn.Length > 50)
                        {
                            this.msn = this.msn.Substring(0, 50);
                        }
                        this.Save();
                    }
                    break;
                   
                case "Yahoo":
                    if (propertyValue is String)
                    {
                        this.Yahoo = propertyValue.ToString();
                        if (this.yahoo.Length > 50)
                        {
                            this.yahoo = this.yahoo.Substring(0, 50);
                        }
                        this.Save();
                    }
                    break;
                    
                case "AIM":
                    if (propertyValue is String)
                    {
                        this.AIM = propertyValue.ToString();
                        if (this.aim.Length > 50)
                        {
                            this.aim = this.aim.Substring(0, 50);
                        }
                        this.Save();
                    }
                    break;
                    
                case "ICQ":
                    if (propertyValue is String)
                    {
                        this.ICQ = propertyValue.ToString();
                        if (this.icq.Length > 50)
                        {
                            this.icq = this.icq.Substring(0, 50);
                        }
                        this.Save();
                    }
                    break;
                    
                case "TotalPosts":
                    //no change allowed through profile
                    break;

                case "AvatarUrl":
                    //no change allowed through profile
                    break;
                    
                //case "TimeOffsetHours":
                //    //no change allowed through profile
                //    break;

                case "Signature":
                    if (propertyValue is String)
                    {
                        this.Signature = propertyValue.ToString();
                        if (this.signature.Length > 255)
                        {
                            this.signature = this.signature.Substring(0, 255);
                        }
                        this.Save();
                    }
                    break;
                    
                case "DateCreated":
                    //no change allowed through profile
                    break;

                case "Skin":
                    //no change allowed through profile
                    break;

                case "LoweredEmail":
                    //no change allowed through profile
                    break;

                case "PasswordQuestion":
                    //no change allowed through profile
                    break;

                case "PasswordAnswer":
                    //no change allowed through profile
                    break;

                case "LastActivityDate":
                    //no change allowed through profile
                    break;

                default:
                    // this is for properties added to config
                    //that were not previously implemented on SiteUser

                    bool propertyExists = DBSiteUser.PropertyExists(this.userGuid, propertyName);

                    if (!propertyExists)
                    {
                        CreateProperty(propertyName, propertyValue, serializeAs, lazyLoad);
                        if (!lazyLoad)
                        {
                            CreatePropertyLocalInstance(propertyName, propertyValue, serializeAs);
                        }
                    }
                    else
                    {
                        UpdateProperty(propertyName, propertyValue, serializeAs, lazyLoad);

                        if (!lazyLoad)
                        {
                            UpdatePropertyLocalInstance(propertyName, propertyValue, serializeAs);
                        }

                    }

                    break;

            }

        }

        private void CreateProperty(
            String propertyName,
            Object propertyValue,
            SettingsSerializeAs serializeAs,
            bool lazyLoad)
        {
            Guid propertyID = Guid.NewGuid();

            switch (serializeAs)
            {
                case SettingsSerializeAs.String:
                default:
                    // currently only serializing to string

                    DBSiteUser.CreateProperty(
                        propertyID,
                        this.userGuid,
                        propertyName,
                        propertyValue.ToString(),
                        null,
                        DateTime.UtcNow,
                        lazyLoad);

                    break;

            }

        }

        private void UpdateProperty(
            String propertyName,
            Object propertyValue,
            SettingsSerializeAs serializeAs,
            bool lazyLoad)
        {
           
            switch (serializeAs)
            {
                case SettingsSerializeAs.String:
                default:
                    // currently only serializing to string

                    DBSiteUser.UpdateProperty(
                        this.userGuid,
                        propertyName,
                        propertyValue.ToString(),
                        null,
                        DateTime.UtcNow,
                        lazyLoad);

                    break;

            }

        }

        private void CreatePropertyLocalInstance(
            String propertyName,
            Object propertyValue,
            SettingsSerializeAs serializeAs)
        {
            if (
                (this.profileProperties != null)
                &&(nonLazyLoadedProfilePropertiesLoaded)
                )
            {
                DataRow row = profileProperties.NewRow();
                row["UserGuid"] = this.userGuid.ToString();
                row["PropertyName"] = propertyName;

                switch (serializeAs)
                {
                    case SettingsSerializeAs.String:
                        row["PropertyValueString"] = propertyValue.ToString();
                        break;

                    default:
                        row["PropertyValueBinary"] = propertyValue;
                        break;

                }
                profileProperties.Rows.Add(row);

            }

        }


        private void UpdatePropertyLocalInstance(
            String propertyName,
            Object propertyValue,
            SettingsSerializeAs serializeAs)
        {
            if (
                (this.profileProperties != null)
                &&(nonLazyLoadedProfilePropertiesLoaded)
                )
            {
                foreach (DataRow row in this.profileProperties.Rows)
                {
                    if (row["PropertyName"].ToString() == propertyName)
                    {
                        switch (serializeAs)
                        {
                            case SettingsSerializeAs.String:
                                row["PropertyValueString"] = propertyValue.ToString();
                                break;

                            default:
                                row["PropertyValueBinary"] = propertyValue;
                                break;

                        }
                        return;
                    }

                }

            }

        }

        public Object GetProperty(
            String propertyName,
            SettingsSerializeAs serializeAs)
        {
            bool lazyLoad = false;
            return GetProperty(propertyName, serializeAs, lazyLoad);

        }
        
        public Object GetProperty(
            String propertyName, 
            SettingsSerializeAs serializeAs,
            bool lazyLoad)
        {

            switch (propertyName)
            {
                    //the properties identified in the case statements
                    //were already implemented on SiteUser
                    //before the exstensible profile was implemented
                    //so we just return the existing property for those
                    //other properties are handled by the bottom default case
                    //so arbitrary properties can be configured

                case "UserID":
                    return this.UserId;
                    
                case "SiteID":
                    return this.SiteId;
                    
                case "IsDeleted":
                    return this.IsDeleted;
                    
                case "UserGuid":
                    return this.UserGuid;
                    
                case "RegisterConfirmGuid":
                    return this.RegisterConfirmGuid;
                    
                case "Name":
                    return this.Name;
                    
                case "LoginName":
                    return this.LoginName;
                    
                case "Email":
                    return this.Email;
                    
                case "Password":
                    // don't return the password from the profile object
                    return String.Empty;
                    
                case "Gender":
                    return this.Gender;
                    
                case "ProfileApproved":
                    return this.ProfileApproved;
                    
                case "ApprovedForGroups":
                    return this.ApprovedForGroups;
                    
                case "Trusted":
                    return this.Trusted;
                    
                case "DisplayInMemberList":
                    return this.DisplayInMemberList;
                    
                case "WebSiteUrl":
                    return this.WebSiteUrl;
                    
                case "Country":
                    return this.Country;
                    
                case "State":
                    return this.State;
                    
                case "Occupation":
                    return this.Occupation;
                    
                case "Interests":
                    return this.Interests;
                    
                case "MSN":
                    return this.MSN;
                    
                case "Yahoo":
                    return this.Yahoo;
                   
                case "AIM":
                    return this.AIM;
                    
                case "ICQ":
                    return this.ICQ;
                   
                case "TotalPosts":
                    return this.TotalPosts;
                    
                case "AvatarUrl":
                    return this.AvatarUrl;
                    
                case "TimeOffsetHours":

                    EnsureNonLazyLoadedProperties();
                    object objTimeOffset = GetNonLazyLoadedProperty(propertyName, serializeAs);

                    if ((objTimeOffset != null)&&(objTimeOffset.ToString().Length > 0))
                    {
                        return objTimeOffset;
                    }
                    else
                    {
                        string timeOffset = "-5.00";
                        if (ConfigurationManager.AppSettings["PreferredGreenwichMeantimeOffset"] != null)
                        {
                            timeOffset = ConfigurationManager.AppSettings["PreferredGreenwichMeantimeOffset"];
                        }
                        return timeOffset;
                    }
                    
                    
                    //return this.TimeOffsetHours;
                    
                case "Signature":
                    return this.Signature;
                    
                case "DateCreated":
                    return this.DateCreated;
                    
                case "Skin":
                    return this.Skin;
                    
                case "LoweredEmail":
                    return this.LoweredEmail;
                    
                case "PasswordQuestion":
                    return this.PasswordQuestion;
                    
                case "PasswordAnswer":
                    return this.PasswordAnswer;
                    
                case "LastActivityDate":
                    return this.LastActivityDate;
                    
                
                default:
                    // this is for properties added to config
                    //that were not previously implemented on SiteUser
                    if (!lazyLoad)
                    {
                        EnsureNonLazyLoadedProperties();
                        return GetNonLazyLoadedProperty(propertyName, serializeAs);
                    }
                    else
                    {
                        // lazyLoaded Properties are either seldom used or expensive
                        // and therefore only loaded from the db as needed

                        return GetLazyLoadedProperty(propertyName, serializeAs);
                    }

            }


        }

        private void EnsureNonLazyLoadedProperties()
        {
            if (
                (profileProperties == null)
                || (!nonLazyLoadedProfilePropertiesLoaded)
                )
            {
                profileProperties = DBSiteUser.GetNonLazyLoadedPropertiesForUser(this.userGuid);
            }

        }

        private Object GetNonLazyLoadedProperty(
            String propertyName,
            SettingsSerializeAs serializeAs)
        {

            if (
                (profileProperties != null)
                && (profileProperties.Rows.Count > 0)
                )
            {
                switch (serializeAs)
                {
                    case SettingsSerializeAs.String:

                        foreach (DataRow row in profileProperties.Rows)
                        {
                            if (row["PropertyName"].ToString() == propertyName)
                            {
                                //return row["PropertyValueString"].ToString(CultureInfo.InvariantCulture);
                                return row["PropertyValueString"];
                            }

                        }

                        break;

                    default:
                        foreach (DataRow row in profileProperties.Rows)
                        {
                            if (row["PropertyName"].ToString() == propertyName)
                            {
                                return row["PropertyValueBinary"];
                            }

                        }

                        break;

                }


            }
            
            return null;
        }

        private Object GetLazyLoadedProperty(
            String propertyName,
            SettingsSerializeAs serializeAs)
        {
            object result = null;
            using (IDataReader reader = DBSiteUser.GetLazyLoadedProperty(this.userGuid, propertyName))
            {
                if (reader.Read())
                {
                    switch (serializeAs)
                    {
                        case SettingsSerializeAs.String:


                            if (reader["PropertyName"].ToString() == propertyName)
                            {
                                result = reader["PropertyValueString"].ToString();
                            }

                            break;

                        default:

                            if (reader["PropertyName"].ToString() == propertyName)
                            {
                                result = reader["PropertyValueBinary"];
                            }

                            break;

                    }
                }
            }


            return result;
        }

        

        #endregion


        #endregion



        #region Static Methods

        public static SiteUser GetByEmail(SiteSettings siteSettings, string email)
        {
            if (siteSettings == null) { return null; }
            if(string.IsNullOrEmpty(email)) { return null; }

            SiteUser siteUser = new SiteUser();
            using (IDataReader reader = DBSiteUser.GetSingleUser(siteSettings.SiteId, email))
            {
                siteUser.GetUser(reader);
            }

            if (siteUser.UserGuid != Guid.Empty) { return siteUser; }

            return null;
        }

        public static SiteUser GetByConfirmationGuid(SiteSettings siteSettings, Guid confirmGuid)
        {
            if (siteSettings == null) { return null; }
            if (confirmGuid == Guid.Empty) { return null; }

            SiteUser siteUser = new SiteUser();
            using (IDataReader reader = DBSiteUser.GetUserByRegistrationGuid(siteSettings.SiteId, confirmGuid))
            {
                siteUser.GetUser(reader);
            }

            if (siteUser.UserGuid != Guid.Empty) { return siteUser; }
           

            return null;
        }

        

        public static bool ConfirmRegistration(Guid registrationGuid)
        {
            if (registrationGuid == Guid.Empty)
            {
                
                return false;
            }

            return DBSiteUser.ConfirmRegistration(Guid.Empty, registrationGuid);
        }

	
		public static IDataReader GetUserList(int siteId)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

			return DBSiteUser.GetUserList(siteId);
		}

        public static DataTable GetUserListForPasswordFormatChange(int siteId)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("Password", typeof(String));

            using (IDataReader reader = DBSiteUser.GetUserList(siteId))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["UserID"] = reader["UserID"];
                    row["Password"] = reader["Pwd"];
                    dt.Rows.Add(row);

                }
            }

            return dt;
        }

		public static int UserCount(int siteId)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
			return DBSiteUser.UserCount(siteId);
		}

        public static int UserCount(int siteId, String userNameBeginsWith)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.UserCount(siteId, userNameBeginsWith);
        }

        public static int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.CountOnlineSince(siteId, sinceTime);
        }

        public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.GetUsersOnlineSince(siteId, sinceTime);
        }

        public static IDataReader GetUsersTop50OnlineSince(int siteId, DateTime sinceTime)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.GetTop50UsersOnlineSince(siteId, sinceTime);
        }

        public static DataTable GetRoleMembers(int roleId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("LoginName", typeof(string));

            using (IDataReader reader = Role.GetRoleMembers(roleId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["UserID"] = reader["UserID"];
                    row["Name"] = reader["Name"];
                    row["Email"] = reader["Email"];
                    row["LoginName"] = reader["LoginName"];
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;

        }

        public static List<string> GetEmailAddresses(int siteId, string roleNamesSeparatedBySemiColons)
        {
            List<string> emailAddresses = new List<string>();
            List<int> roleIds = Role.GetRoleIds(siteId, roleNamesSeparatedBySemiColons);

            foreach (int roleId in roleIds)
            {
                using (IDataReader reader = Role.GetRoleMembers(roleId))
                {
                    while (reader.Read())
                    {
                        string email = reader["Email"].ToString().ToLower();
                        if (!emailAddresses.Contains(email)) { emailAddresses.Add(email); }
                        
                    }
                }

            }


            return emailAddresses;
        }

        /// <summary>
        /// Gets Data for a user registration by month chart. Fields Y, M, Label, Users
        /// Label is just a concat of Year-Month, Users is the count.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static IDataReader GetUserCountByYearMonth(int siteId)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.GetUserCountByYearMonth(siteId);

        }

        //public static DataSet GetUserListPage(int siteID, int pageNumber, int pageSize, string userNameBeginsWith)
        //{
        //    return dbSiteUser.GetUserListPage(siteID, pageNumber, pageSize, userNameBeginsWith);
        //}

        //public static DataTable GetUserListPageTable(int siteId, int pageNumber, int pageSize, string userNameBeginsWith)
        //{
        //    return DBSiteUser.GetUserListPageTable(siteId, pageNumber, pageSize, userNameBeginsWith);
        //}

        public static List<SiteUser> GetByIPAddress(Guid siteGuid, string ipv4Address)
        {
            List<SiteUser> userList
                = new List<SiteUser>();

            if (UseRelatedSiteMode) { siteGuid = Guid.Empty; }

            using (IDataReader reader = UserLocation.GetUsersByIPAddress(siteGuid, ipv4Address))
            {

                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    PopulateFromReaderRow(user, reader);
                    userList.Add(user);
                    
                }
            }

            return userList;

        }


        public static List<SiteUser> GetPage(
            int siteId,
            int pageNumber, 
            int pageSize, 
            string userNameBeginsWith,
            out int totalPages)
        {
            totalPages = 1;

            List<SiteUser> userList
                = new List<SiteUser>();

            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader
                = DBSiteUser.GetUserListPage(
                    siteId, pageNumber, pageSize, userNameBeginsWith))
            {

                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    PopulateFromReaderRow(user, reader);
                    userList.Add(user);
                    totalPages = Convert.ToInt32(reader["TotalPages"]);
                }
            }

            return userList;

        }

        public static List<SiteUser> GetUserSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            out int totalPages)
        {
            List<SiteUser> userList
                = new List<SiteUser>();

            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader = DBSiteUser.GetUserSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                out totalPages))
            {

                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    PopulateFromReaderRow(user, reader);
                    userList.Add(user);
                    
                }
            }

            return userList;

           
        }

        public static List<SiteUser> GetUserAdminSearchPage(
            int siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            out int totalPages)
        {
            List<SiteUser> userList
                = new List<SiteUser>();

            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader = DBSiteUser.GetUserAdminSearchPage(
                siteId,
                pageNumber,
                pageSize,
                searchInput,
                out totalPages))
            {

                while (reader.Read())
                {
                    SiteUser user = new SiteUser();
                    PopulateFromReaderRow(user, reader);
                    userList.Add(user);

                }
            }

            return userList;


        }

        private static void PopulateFromReaderRow(
            SiteUser user, 
            IDataReader reader)
        {
            user.userID = Convert.ToInt32(reader["UserID"]);
            user.siteID = Convert.ToInt32(reader["SiteID"]);
            if (UseRelatedSiteMode)
            {
                user.siteID = RelatedSiteID;
            }

            user.name = reader["Name"].ToString();
            user.loginName = reader["LoginName"].ToString();
            user.email = reader["Email"].ToString();
            user.loweredEmail = reader["LoweredEmail"].ToString();
            user.password = reader["Pwd"].ToString();
            user.passwordQuestion = reader["PasswordQuestion"].ToString();
            user.passwordAnswer = reader["PasswordAnswer"].ToString();
            user.gender = reader["Gender"].ToString();

            // this is to support dbs that don't have bit data type
            string pApproved = reader["ProfileApproved"].ToString();
            user.profileApproved = (pApproved == "True" || pApproved == "1");
            string fApproved = reader["ApprovedForGroups"].ToString();
            user.approvedForGroups = (fApproved == "True" || fApproved == "1");
            string t = reader["Trusted"].ToString();
            user.trusted = (t == "True" || t == "1");
            string display = reader["DisplayInMemberList"].ToString();
            user.displayInMemberList = (display == "True" || display == "1");

            string deleted = reader["IsDeleted"].ToString();
            user.isDeleted = (deleted == "True" || deleted == "1");

            user.isLockedOut = (
                (string.Equals(reader["IsLockedOut"].ToString(),"true", StringComparison.InvariantCultureIgnoreCase)) ||
                (reader["IsLockedOut"].ToString() == "1"));

            user.webSiteUrl = reader["WebSiteURL"].ToString();
            user.country = reader["Country"].ToString();
            user.state = reader["State"].ToString();
            user.occupation = reader["Occupation"].ToString();
            user.interests = reader["Interests"].ToString();
            user.msn = reader["MSN"].ToString();
            user.yahoo = reader["Yahoo"].ToString();
            user.aim = reader["AIM"].ToString();
            user.icq = reader["ICQ"].ToString();
            if (reader["TotalPosts"] != DBNull.Value)
            {
                user.totalPosts = Convert.ToInt32(reader["TotalPosts"]);
            }
            user.avatarUrl = reader["AvatarUrl"].ToString();
            //user.timeOffsetHours = Convert.ToInt32(reader["TimeOffsetHours"]);
            user.signature = reader["Signature"].ToString();
            if (reader["DateCreated"] != DBNull.Value)
            {
                user.dateCreated = Convert.ToDateTime(reader["DateCreated"]);
            }
            user.userGuid = new Guid(reader["UserGuid"].ToString());
            user.skin = reader["Skin"].ToString();


            if (reader["LastActivityDate"] != DBNull.Value)
            {
                user.lastActivityDate = Convert.ToDateTime(reader["LastActivityDate"]);
            }

            if (reader["LastLoginDate"] != DBNull.Value)
            {
                user.lastLoginDate = Convert.ToDateTime(reader["LastLoginDate"]);
            }

            if (reader["LastPasswordChangedDate"] != DBNull.Value)
            {
                user.lastPasswordChangedDate = Convert.ToDateTime(reader["LastPasswordChangedDate"]);
            }

            if (reader["LastLockoutDate"] != DBNull.Value)
            {
                user.lastLockoutDate = Convert.ToDateTime(reader["LastLockoutDate"]);
            }

            if (reader["FailedPasswordAttemptCount"] != DBNull.Value)
            {
                user.failedPasswordAttemptCount = Convert.ToInt32(reader["FailedPasswordAttemptCount"]);
            }

            if (reader["FailedPwdAttemptWindowStart"] != DBNull.Value)
            {
                user.failedPasswordAttemptWindowStart
                    = Convert.ToDateTime(reader["FailedPwdAttemptWindowStart"]);
            }

            if (reader["FailedPwdAnswerAttemptCount"] != DBNull.Value)
            {
                user.failedPasswordAnswerAttemptCount
                    = Convert.ToInt32(reader["FailedPwdAnswerAttemptCount"]);
            }

            if (reader["FailedPwdAnswerWindowStart"] != DBNull.Value)
            {
                user.failedPasswordAnswerAttemptWindowStart
                    = Convert.ToDateTime(reader["FailedPwdAnswerWindowStart"]);
            }

            user.passwordSalt = reader["PasswordSalt"].ToString();
            user.comment = reader["Comment"].ToString();
            user.mustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);

        }

        //public static DataTable GetUserListPageTable(
        //    int siteID, 
        //    int pageNumber, 
        //    int pageSize, 
        //    string userNameBeginsWith)
        //{
        //    return dbSiteUser.GetUserListPageTable(siteID, pageNumber, pageSize, userNameBeginsWith);
        //}

        public static IDataReader GetUserByEmail(int siteId, string email)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            return DBSiteUser.GetSingleUser(siteId, email);
        }

        public static IDataReader GetUserByLoginName(int siteId, String loginName)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            return DBSiteUser.GetSingleUserByLoginName(siteId, loginName);
        }

		public static IDataReader GetRolesByUser(int siteId, int userId)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

			return DBSiteUser.GetRolesByUser(siteId, userId);
		}

		public static string[] GetRoles(SiteSettings siteSettings, string identity) 
		{
			SiteUser siteUser = new SiteUser(siteSettings, identity);

            int siteId = siteSettings.SiteId;
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            ArrayList userRoles = new ArrayList();
            using (IDataReader reader = DBSiteUser.GetRolesByUser(siteId, siteUser.UserId))
            {
                while (reader.Read())
                {
                    userRoles.Add(reader["RoleName"]);
                }

            }
			
			return (string[]) userRoles.ToArray(typeof(string));
		}

		public static string Login(SiteSettings siteSettings, string loginId, string password)
		{
            SiteUser userCreatedForLdap = null;
            return Login(siteSettings, loginId, password, out userCreatedForLdap);
		}

        public static string Login(SiteSettings siteSettings, string loginId, string password, out SiteUser userCreatedForLdap)
        {
            userCreatedForLdap = null;
            int siteId = siteSettings.SiteId;
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            if (siteSettings.UseLdapAuth)
            {
                // if using ldap we don't login by email
                LdapUser user = LdapHelper.LdapLogin(siteSettings.SiteLdapSettings, loginId, password);
                if (user != null)
                {
                    bool existsInDB = LoginExistsInDB(siteId, loginId);

                    if (existsInDB)
                    {
                        return user.CommonName;
                    }
                    else
                    {
                        if (siteSettings.AutoCreateLdapUserOnFirstLogin)
                        {
                            userCreatedForLdap = new SiteUser(siteSettings);
                            userCreatedForLdap.name = user.CommonName;
                            userCreatedForLdap.LoginName = user.UserId;
                            userCreatedForLdap.email = user.Email;
                            // this password would only be used if the site was changed back from using
                            // LDAP to standard db authentication but we still need to populate it with something
                            userCreatedForLdap.Password = CreateRandomPassword(7);
                            userCreatedForLdap.Save();
                            //NewsletterHelper.ClaimExistingSubscriptions(u);
                            return user.CommonName;
                        }
                        else
                        {
                            return String.Empty;
                        }
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {

                if (siteSettings.UseEmailForLogin)
                {
                    String foundUser = DBSiteUser.LoginByEmail(siteId, loginId, password);

                    if (foundUser != String.Empty)
                    {
                        return foundUser;
                    }
                    else
                    {
                        return DBSiteUser.Login(siteId, loginId, password);
                    }

                }
                else
                {
                    return DBSiteUser.Login(siteId, loginId, password);
                }
            }
        }

        

		public static bool IncrementTotalPosts(int userId)
		{
			return DBSiteUser.IncrementTotalPosts(userId);
		}

		public static bool DecrementTotalPosts(int userId)
		{
			return DBSiteUser.DecrementTotalPosts(userId);
		}


		public static IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

			return DBSiteUser.GetSmartDropDownData(siteId, query, rowsToGet);
		}

		public static bool EmailExistsInDB(int siteId, string email)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUser(siteId, email))
            {
                while (r.Read()) { found = true; }
            }
			return found;
		}

		public static bool LoginExistsInDB(int siteId, string loginName)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            bool found = false;

            using (IDataReader r = DBSiteUser.GetSingleUserByLoginName(siteId, loginName))
            {
                while (r.Read()) { found = true; }
            }

			return found;
		}

		public static string CreateRandomPassword(int length) 
		{	
			if(length == 0)
			{
				length = 7;
			}

			char[] allowedChars = "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789*$".ToCharArray();
			char[] passwordChars = new char[length];
			byte[] seedBytes = new byte[4];
			RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
			crypto.GetBytes(seedBytes);

			int seed = (seedBytes[0] & 0x7f) << 24 |
				seedBytes[1]         << 16 |
				seedBytes[2]         <<  8 |
				seedBytes[3];

			Random  random  = new Random(seed);

			for (int i=0; i<length; i++)
			{
				passwordChars[i] = allowedChars[random.Next(0, allowedChars.Length)];
			}

			return new string(passwordChars);
			
		}

        

        public static String GetUserNameFromEmail(int siteId, String email)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            String result = String.Empty;
            if ((email != null) && (email.Length > 0) && (siteId > 0))
            {
                String comma = String.Empty;
                using (IDataReader reader = DBSiteUser.GetSingleUser(siteId, email))
                {
                    while (reader.Read())
                    {
                        result += comma + reader["LoginName"].ToString();
                        comma = ", ";

                    }
                }
            }

            return result;

        }



        public static int GetNewestUserId(int siteId)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            return DBSiteUser.GetNewestUserId(siteId);

        }

        public static SiteUser GetNewestUser(SiteSettings siteSettings)
        {
            int userID = GetNewestUserId(siteSettings.SiteId);
            SiteUser siteUser = new SiteUser(siteSettings, userID);
            if (siteUser.UserId == userID)
            {
                return siteUser;
            }

            return null;

        }

        public static int CountUsersByRegistrationDateRange(
            int siteId,
            DateTime beginDate,
            DateTime endDate)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            return DBSiteUser.CountUsersByRegistrationDateRange(siteId, beginDate, endDate);

        }

        public static Guid GetUserGuidFromOpenId(
            int siteId,
            string openIdUri)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.GetUserGuidFromOpenId(siteId, openIdUri);
        }

        public static Guid GetUserGuidFromWindowsLiveId(
            int siteId,
            string windowsLiveId)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBSiteUser.GetUserGuidFromWindowsLiveId(siteId, windowsLiveId);
        }

        public static void UpdateTotalRevenue(Guid userGuid)
        {
            DBSiteUser.UpdateTotalRevenue(userGuid);

        }

        /// <summary>
        /// updates the total revenue for all users
        /// </summary>
        public static void UpdateTotalRevenue()
        {
            DBSiteUser.UpdateTotalRevenue();
        }


		#endregion

		
	}

}
