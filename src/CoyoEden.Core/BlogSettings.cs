#region Using

using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using SystemX;
using SystemX.Web;
using System.Globalization;
using CoyoEden.Core.DataContracts;
using SystemX.Infrastructure;

#endregion

namespace CoyoEden.Core
{
	/// <summary>
	/// Represents the configured settings for the blog engine.
	/// </summary>
	public class BlogSettings
	{
		//============================================================
		//	PUBLIC/PRIVATE/PROTECTED MEMBERS
		//============================================================
		#region PRIVATE/PROTECTED/PUBLIC MEMBERS

		public static event EventHandler<EventArgs> Changed;
		private static BlogSettings blogSettingsSingleton;
		private string configuredTheme = String.Empty;
		private int numberOfRecentPosts = 10;
		bool enableHttpCompression;

		#endregion

		//============================================================
		//	CONSTRUCTORS
		//============================================================
		#region BlogSettings()
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogSettings"/> class.
		/// </summary>
		private BlogSettings()
		{
			//------------------------------------------------------------
			//	Attempt to initialize class state
			//------------------------------------------------------------
			//try
			//{
			//------------------------------------------------------------
			//	Load the currently configured settings
			//------------------------------------------------------------
			Load();
			//}
			//catch
			//{
			//    //------------------------------------------------------------
			//    //	Rethrow exception
			//    //------------------------------------------------------------
			//    throw;
			//}
		}
		#endregion

		//============================================================
		//	STATIC PROPERTIES
		//============================================================
		#region Instance
		/// <summary>
		/// Gets the singleton instance of the <see cref="BlogSettings"/> class.
		/// </summary>
		/// <value>A singleton instance of the <see cref="BlogSettings"/> class.</value>
		/// <remarks></remarks>
		public static BlogSettings Instance
		{
			get
			{
				if (blogSettingsSingleton == null)
				{
					blogSettingsSingleton = new BlogSettings();
				}
				return blogSettingsSingleton;
			}
		}
		#endregion

		public List<Setting> CurrentSettings { get; set; }

		//============================================================
		//	GENERAL SETTINGS
		//============================================================
		#region Description
		/// <summary>
		/// Gets or sets the description of the blog.
		/// </summary>
		/// <value>A brief synopsis of the blog content.</value>
		/// <remarks>This value is also used for the description meta tag.</remarks>
		public string Description { get; set; }

		#endregion

		#region EnableHttpCompression
		/// <summary>
		/// Gets or sets a value indicating if HTTP compression is enabled.
		/// </summary>
		/// <value><b>true</b> if compression is enabled, otherwise returns <b>false</b>.</value>
		public bool EnableHttpCompression
		{
			get
			{
				return enableHttpCompression && !SystemX.Utils.IsMono;
			}
			set
			{
				enableHttpCompression = value;
			}
		}

		#endregion

		#region EnableReferrerTracking
		/// <summary>
		/// Gets or sets a value indicating if referral tracking is enabled.
		/// </summary>
		/// <value><b>true</b> if referral tracking is enabled, otherwise returns <b>false</b>.</value>
		public bool EnableReferrerTracking { get; set; }
		#endregion

        #region NumberOfReferrerDays
        /// <summary>
        /// Gets or sets a value indicating the number of days that referrer information should be stored.
        /// </summary>
        public int NumberOfReferrerDays { get; set; }
        #endregion

        #region EnableRelatedPosts
        /// <summary>
		/// Gets or sets a value indicating if related posts are displayed.
		/// </summary>
		/// <value><b>true</b> if related posts are displayed, otherwise returns <b>false</b>.</value>
		public bool EnableRelatedPosts { get; set; }
		#endregion

		#region AlternateFeedUrl
		/// <summary>Gets or sets the FeedBurner user name.</summary>
		public string AlternateFeedUrl { get; set; }
		#endregion

		#region TimeStampPostLinks
		/// <summary>Gets or sets whether or not to time stamp post links.</summary>
		public bool TimeStampPostLinks { get; set; }
		#endregion

		#region Name
		/// <summary>
		/// Gets or sets the name of the blog.
		/// </summary>
		/// <value>The title of the blog.</value>
		public string Name { get; set; }
		#endregion

		#region NumberOfRecentPosts
		/// <summary>
		/// Gets or sets the default number of recent posts to display.
		/// </summary>
		/// <value>The number of recent posts to display.</value>
		public int NumberOfRecentPosts
		{
			get { return numberOfRecentPosts; }
			set { numberOfRecentPosts = value; }
		}
		#endregion

		#region NumberOfRecentComments
		/// <summary>
		/// Gets or sets the default number of recent posts to display.
		/// </summary>
		/// <value>The number of recent posts to display.</value>
		public int NumberOfRecentComments { get; set; }
		#endregion

		#region PostsPerPage
		/// <summary>
		/// Gets or sets the number of posts to show an each page.
		/// </summary>
		/// <value>The number of posts to show an each page.</value>
		public int PostsPerPage { get; set; }
		#endregion

		#region ShowLivePreview
		/// <summary>
		/// Gets or sets a value indicating if live preview of post is displayed.
		/// </summary>
		/// <value><b>true</b> if live previews are displayed, otherwise returns <b>false</b>.</value>
		public bool ShowLivePreview { get; set; }
		#endregion

		#region EnableRating
		/// <summary>
		/// Gets or sets a value indicating if live preview of post is displayed.
		/// </summary>
		/// <value><b>true</b> if live previews are displayed, otherwise returns <b>false</b>.</value>
		public bool EnableRating { get; set; }
		#endregion

		#region ShowDescriptionInPostList
		/// <summary>
		/// Gets or sets a value indicating if the full post is displayed in lists or only the description/exerpt.
		/// </summary>
		public bool ShowDescriptionInPostList { get; set; }
		#endregion

		#region DescriptionCharacters
		/// <summary>
		/// Gets or sets a value indicating how many characters should be shown of the description
		/// </summary>
		public int DescriptionCharacters { get; set; }
		#endregion

		#region StorageLocation
		/// <summary>
		/// Gets or sets the default storage location for blog data.
		/// </summary>
		/// <value>The default storage location for blog data.</value>
		public string StorageLocation { get; set; }
		#endregion

		#region Enclosure support
		/// <summary>
		/// Enable enclosures for RSS feeds
		/// </summary>
		public bool EnableEnclosures { get; set; }
		#endregion

		#region FileExtension
		/// <summary>
		/// The  file extension used for aspx pages
		/// </summary>
		public string FileExtension
		{
			get { return ConfigurationManager.AppSettings["CoyoEden.FileExtension"] ?? ".aspx"; }
		}
		#endregion

		#region AdministratorRole
		/// <summary>
		/// The role that has administrator persmissions
		/// </summary>
		public string AdministratorRole
		{
			get { return ConfigurationManager.AppSettings["CoyoEden.AdminRole"] ?? "administrators"; }
		}
		#endregion

		#region SyndicationFormat
		/// <summary>
		/// Gets or sets the default syndication format used by the blog.
		/// </summary>
		/// <value>The default syndication format used by the blog.</value>
		/// <remarks>If no value is specified, blog defaults to using RSS 2.0 format.</remarks>
		/// <seealso cref="CoyoEden.Core.SyndicationFormat"/>
		public string SyndicationFormat { get; set; }
		#endregion

		#region Theme
		/// <summary>
		/// Gets or sets the current theme applied to the blog.
		/// </summary>
		/// <value>The configured theme for the blog.</value>
		public string Theme
		{
			get
			{
				if (SystemX.Web.Utils.IsMobile && !string.IsNullOrEmpty(MobileTheme))
					return MobileTheme;

				return configuredTheme;
			}

			set
			{
				if (String.IsNullOrEmpty(value))
				{
					configuredTheme = String.Empty;
				}
				else
				{
					configuredTheme = value;
				}
			}
		}
		/// <summary>
		/// current theme applied to the admin dashboard
		/// </summary>
		public string ThemeBackfield
		{
			get;
			set;
		}
		#endregion

		#region MobileTheme
		/// <summary>
		/// Gets or sets the mobile theme.
		/// </summary>
		/// <value>The mobile theme.</value>
		public string MobileTheme { get; set; }
		#endregion

		#region RemoveWhitespaceInStyleSheets
		/// <summary>
		/// Gets or sets a value indicating if whitespace in stylesheets should be removed
		/// </summary>
		/// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
		public bool RemoveWhitespaceInStyleSheets { get; set; }
		#endregion

		#region CompressWebResource
		/// <summary>
		/// Gets or sets a value indicating whether to compress WebResource.axd
		/// </summary>
		/// <value><c>true</c> if [compress web resource]; otherwise, <c>false</c>.</value>
		public bool CompressWebResource { get; set; }
		#endregion

		#region UseBlogNameInPageTitles
		/// <summary>
		/// Gets or sets a value indicating if whitespace in stylesheets should be removed
		/// </summary>
		/// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
		public bool UseBlogNameInPageTitles { get; set; }
		#endregion

		#region RequireSSLMetaWeblogAPI;
		/// <summary>
		/// Gets or sets a value indicating whether [require SSL for MetaWeblogAPI connections].
		/// </summary>
		public bool RequireSSLMetaWeblogAPI { get; set; }
		#endregion

		#region EnableOpenSearch
		/// <summary>
		/// Gets or sets a value indicating if whitespace in stylesheets should be removed
		/// </summary>
		/// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
		public bool EnableOpenSearch { get; set; }
		#endregion

		#region TrackingScript
		/// <summary>
		/// Gets or sets the tracking script used to collect visitor data.
		/// </summary>
		public string TrackingScript { get; set; }
		#endregion

		#region DisplayCommentsOnRecentPosts
		/// <summary>
		/// Gets or sets a value indicating if whitespace in stylesheets should be removed
		/// </summary>
		/// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
		public bool DisplayCommentsOnRecentPosts { get; set; }
		#endregion

		#region DisplayRatingsOnRecentPosts
		/// <summary>
		/// Gets or sets a value indicating if whitespace in stylesheets should be removed
		/// </summary>
		/// <value><b>true</b> if whitespace is removed, otherwise returns <b>false</b>.</value>
		public bool DisplayRatingsOnRecentPosts { get; set; }
		#endregion

		#region ShowPostNavigation
		/// <summary>
		/// Gets or sets a value indicating whether or not to show the post navigation.
		/// </summary>
		/// <value><c>true</c> if [show post navigation]; otherwise, <c>false</c>.</value>
		public bool ShowPostNavigation { get; set; }
		#endregion

		#region HandleWwwSubdomain
		/// <summary>
		/// Gets or sets how to handle the www subdomain of the url (for SEO purposes).
		/// </summary>
		public string HandleWwwSubdomain { get; set; }
		#endregion

		#region EnablePingBackSend
		/// <summary>
		/// Gets or sets a value indicating whether [enable ping back send].
		/// </summary>
		/// <value><c>true</c> if [enable ping back send]; otherwise, <c>false</c>.</value>
		public bool EnablePingBackSend { get; set; }
		#endregion

		#region EnablePingBackReceive;
		/// <summary>
		/// Gets or sets a value indicating whether [enable ping back receive].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [enable ping back receive]; otherwise, <c>false</c>.
		/// </value>
		public bool EnablePingBackReceive { get; set; }
		#endregion

		#region EnableTrackBackSend;
		/// <summary>
		/// Gets or sets a value indicating whether [enable track back send].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [enable track back send]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableTrackBackSend { get; set; }
		#endregion

		#region EnableTrackBackReceive;
		/// <summary>
		/// Gets or sets a value indicating whether [enable track back receive].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [enable track back receive]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableTrackBackReceive { get; set; }
		#endregion

        #region EnableErrorLogging;
        /// <summary>
        /// Gets or sets a value indicating whether unhandled errors are logged.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if unhandled errors are to be logged otherwise, <c>false</c>.
        /// </value>
        public bool EnableErrorLogging { get; set; }
        #endregion

		//============================================================
		//	EMAIL SETTINGS
		//============================================================
		#region Email
		/// <summary>
		/// Gets or sets the e-mail address notifications are sent to.
		/// </summary>
		/// <value>The e-mail address notifications are sent to.</value>
		public string Email { get; set; }
		#endregion

		#region SendMailOnComment
		/// <summary>
		/// Gets or sets a value indicating if an enail is sent when a comment is added to a post.
		/// </summary>
		/// <value><b>true</b> if email notification of new comments is enabled, otherwise returns <b>false</b>.</value>
		public bool SendMailOnComment { get; set; }

		#endregion

		#region SmtpPassword
		/// <summary>
		/// Gets or sets the password used to connect to the SMTP server.
		/// </summary>
		/// <value>The password used to connect to the SMTP server.</value>
		public string SmtpPassword { get; set; }
		#endregion

		#region SmtpServer
		/// <summary>
		/// Gets or sets the DNS name or IP address of the SMTP server used to send notification emails.
		/// </summary>
		/// <value>The DNS name or IP address of the SMTP server used to send notification emails.</value>
		public string SmtpServer { get; set; }
		#endregion

		#region SmtpServerPort
		/// <summary>
		/// Gets or sets the DNS name or IP address of the SMTP server used to send notification emails.
		/// </summary>
		/// <value>The DNS name or IP address of the SMTP server used to send notification emails.</value>
		public int SmtpServerPort { get; set; }
		#endregion

		#region SmtpUsername
		/// <summary>
		/// Gets or sets the user name used to connect to the SMTP server.
		/// </summary>
		/// <value>The user name used to connect to the SMTP server.</value>
		public string SmtpUserName { get; set; }
		#endregion

		#region EnableSsl
		/// <summary>
		/// Gets or sets a value indicating if SSL is enabled for sending e-mails
		/// </summary>
		public bool EnableSsl { get; set; }
		#endregion

		#region EmailSubjectPrefix
		/// <summary>
		/// Gets or sets the email subject prefix.
		/// </summary>
		/// <value>The email subject prefix.</value>
		public string EmailSubjectPrefix { get; set; }
		#endregion

		//============================================================
		//	COMMENT SETTINGS
		//============================================================
		#region DaysCommentsAreEnabled
		/// <summary>
		/// Gets or sets the number of days that a post accepts comments.
		/// </summary>
		/// <value>The number of days that a post accepts comments.</value>
		/// <remarks>After this time period has expired, comments on a post are disabled.</remarks>
		public int DaysCommentsAreEnabled { get; set; }
		#endregion

		#region EnableCountryInComments
		private bool enableCountryInComments;
		/// <summary>
		/// Gets or sets a value indicating if dispay of the country of commenter is enabled.
		/// </summary>
		/// <value><b>true</b> if commenter country display is enabled, otherwise returns <b>false</b>.</value>
		public bool EnableCountryInComments
		{
			get
			{
				return enableCountryInComments;
			}
			set
			{
				enableCountryInComments = value;
			}
		}
		#endregion

		#region IsCoCommentEnabled
		/// <summary>
		/// Gets or sets a value indicating if CoComment support is enabled.
		/// </summary>
		/// <value><b>true</b> if CoComment support is enabled, otherwise returns <b>false</b>.</value>
		public bool IsCoCommentEnabled { get; set; }
		#endregion

		#region IsCommentEnabled
		/// <summary>
		/// Gets or sets a value indicating if comments are enabled for posts.
		/// </summary>
		/// <value><b>true</b> if comments can be made against a post, otherwise returns <b>false</b>.</value>
		public bool IsCommentEnabled { get; set; }
		#endregion

		#region EnableCommentsModeration
		/// <summary>
		/// Gets or sets a value indicating if comments moderation is used for posts.
		/// </summary>
		/// <value><b>true</b> if comments are moderated for posts, otherwise returns <b>false</b>.</value>
		public bool EnableCommentsModeration { get; set; }
		#endregion

		#region Avatar
		/// <summary>
		/// Gets or sets a value indicating if Gravatars are enabled or not.
		/// </summary>
		/// <value><b>true</b> if Gravatars are enabled, otherwise returns <b>false</b>.</value>
		public string Avatar { get; set; }
		#endregion

		#region IsCommentNestingEnabled
		/// <summary>
		/// Gets or sets a value indicated if comments should be displayed as nested.	
		/// </summary>
		/// <value><b>true</b> if comments should be displayed as nested, <b>false</b> for flat comments.</value>
		public bool IsCommentNestingEnabled { get; set; }
		#endregion

        #region Trust authenticated users
        ///<summary>
        /// If true comments from authenticated users always approved
        ///</summary>
        public bool TrustAuthenticatedUsers { get; set; }
        #endregion

        #region White list count
        ///<summary>
        /// Number of comments approved to add user to white list
        ///</summary>
        public int CommentWhiteListCount { get; set; }
        #endregion

        #region Black list count
        ///<summary>
        /// Number of comments approved to add user to white list
        ///</summary>
        public int CommentBlackListCount { get; set; }
        #endregion

        #region Comments per page
        /// <summary>
        /// Number of comments per page displayed in the comments asmin section
        /// </summary>
        public int CommentsPerPage { get; set; }
        #endregion

        #region Moderation type
        /// <summary>
        /// Gets or sets a value indicating type of moderation
        /// </summary>
        /// <value><0 - manual, 1 - auto</value>
        public int ModerationType { get; set; }
        #endregion

        //============================================================
		//	BLOG ROLL SETTINGS
		//============================================================
		#region BlogrollMaxLength
		/// <summary>
		/// Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
		/// </summary>
		/// <value>The maximum number of characters to display.</value>
		public int BlogrollMaxLength { get; set; }
		#endregion

		#region BlogrollUpdateMinutes
		/// <summary>
		/// Gets or sets the number of minutes to wait before polling blog-roll sources for changes.
		/// </summary>
		/// <value>The number of minutes to wait before polling blog-roll sources for changes.</value>
		public int BlogrollUpdateMinutes { get; set; }
		#endregion

		#region BlogrollVisiblePosts
		/// <summary>
		/// Gets or sets the number of posts to display from a blog-roll source.
		/// </summary>
		/// <value>The number of posts to display from a blog-roll source.</value>
		public int BlogrollVisiblePosts { get; set; }
		#endregion

		//============================================================
		//	SEARCH SETTINGS
		//============================================================
		#region EnableCommentSearch
		/// <summary>
		/// Gets or sets a value indicating if search of post comments is enabled.
		/// </summary>
		/// <value><b>true</b> if post comments can be searched, otherwise returns <b>false</b>.</value>
		public bool EnableCommentSearch { get; set; }
		#endregion

		#region SearchButtonText
		/// <summary>
		/// Gets or sets the search button text to be displayed.
		/// </summary>
		/// <value>The search button text to be displayed.</value>
		public string SearchButtonText { get; set; }
		#endregion

		#region SearchCommentLabelText
		/// <summary>
		/// Gets or sets the search comment label text to display.
		/// </summary>
		/// <value>The search comment label text to display.</value>
		public string SearchCommentLabelText { get; set; }
		#endregion

		#region SearchDefaultText
		/// <summary>
		/// Gets or sets the default search text to display.
		/// </summary>
		/// <value>The default search text to display.</value>
		public string SearchDefaultText { get; set; }
		#endregion

		//============================================================
		//	BLOG CHANNEL EXTENSION SETTINGS
		//============================================================
		#region Endorsement
		/// <summary>
		/// Gets or sets the URI of a web log that the author of this web log is promoting.
		/// </summary>
		/// <value>The <see cref="Uri"/> of a web log that the author of this web log is promoting.</value>
		public string Endorsement { get; set; }
		#endregion

		#region PostsPerFeed
		/// <summary>
		/// Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
		/// </summary>
		/// <value>The maximum number of characters to display.</value>
		public int PostsPerFeed { get; set; }
		#endregion

		//============================================================
		//	DUBLIN CORE EXTENSION SETTINGS
		//============================================================
		#region AuthorName
		/// <summary>
		/// Gets or sets the name of the author of this blog.
		/// </summary>
		/// <value>The name of the author of this blog.</value>
		public string AuthorName { get; set; }
		#endregion

		#region Language
		/// <summary>
		/// Gets or sets the language this blog is written in.
		/// </summary>
		/// <value>The language this blog is written in.</value>
		/// <remarks>
		///     Recommended best practice for the values of the Language element is defined by RFC 1766 [RFC1766] which includes a two-letter Language Code (taken from the ISO 639 standard [ISO639]), 
		///     followed optionally, by a two-letter Country Code (taken from the ISO 3166 standard [ISO3166]).
		/// </remarks>
		/// <example>en-US</example>
		public string Language { get; set; }
		#endregion

		//============================================================
		//	GEOCODING EXTENSION SETTINGS
		//============================================================
		#region GeocodingLatitude
		/// <summary>
		/// Gets or sets the latitude component of the geocoding position for this blog.
		/// </summary>
		/// <value>The latitude value.</value>
		public float GeocodingLatitude { get; set; }
		#endregion

		#region GeocodingLongitude
		/// <summary>
		/// Gets or sets the longitude component of the geocoding position for this blog.
		/// </summary>
		/// <value>The longitude value.</value>
		public float GeocodingLongitude { get; set; }
		#endregion

		//============================================================
		//	CONTACT FORM
		//============================================================
		#region ContactFormMessage;
		/// <summary>
		/// Gets or sets the name of the author of this blog.
		/// </summary>
		/// <value>The name of the author of this blog.</value>
		public string ContactFormMessage { get; set; }
		#endregion

		#region ContactThankMessage
		/// <summary>
		/// Gets or sets the name of the author of this blog.
		/// </summary>
		/// <value>The name of the author of this blog.</value>
		public string ContactThankMessage { get; set; }
		#endregion

		#region HtmlHeader
		/// <summary>
		/// Gets or sets the name of the author of this blog.
		/// </summary>
		/// <value>The name of the author of this blog.</value>
		public string HtmlHeader { get; set; }
		#endregion

		#region Culture
		/// <summary>
		/// Gets or sets the name of the author of this blog.
		/// </summary>
		/// <value>The name of the author of this blog.</value>
		public string Culture { get; set; }
		#endregion

		#region Timezone
		/// <summary>
		/// Gets or sets the maximum number of characters that are displayed from a blog-roll retrieved post.
		/// </summary>
		/// <value>The maximum number of characters to display.</value>
		public double Timezone { get; set; }
		#endregion

		#region EnableContactAttachments
		/// <summary>
		/// Gets or sets whether or not to allow visitors to send attachments via the contact form.
		/// </summary>
		public bool EnableContactAttachments { get; set; }
		#endregion

		//============================================================
		//	PRIVATE ROUTINES
		//============================================================
		#region Load()
		/// <summary>
		/// Initializes the singleton instance of the <see cref="BlogSettings"/> class.
		/// </summary>
		private void Load()
		{
			//load from db
			CurrentSettings = Setting.LoadAll(SettingTypes.AppSetting);
			//init
			var themeb = CurrentSettings.SingleOrDefault(x => x.SettingName == "ThemeBackfield");
			if (themeb == null) {
				themeb = new Setting { 
					Id=GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential),
					SettingName = "ThemeBackfield",
					SettingValue="admin",
					SettingType=SettingTypes.AppSetting.ToString()
				};
				CurrentSettings.Add(themeb);
			};
			//full fill
			CurrentSettings.ForEach(x => {
				this.SetPropertyValue(x.SettingName, x.SettingValue, true);
			});

			StorageLocation =GetStorageLocation();
		}
		#endregion

		#region OnChanged()
		/// <summary>
		/// Occurs when the settings have been changed.
		/// </summary>
		private static void OnChanged()
		{
			//------------------------------------------------------------
			//	Attempt to raise the Changed event
			//------------------------------------------------------------
			try
			{
				//------------------------------------------------------------
				//	Execute event handler
				//------------------------------------------------------------
				if (BlogSettings.Changed != null)
				{
					BlogSettings.Changed(null, new EventArgs());
				}
			}
			catch
			{
				//------------------------------------------------------------
				//	Rethrow exception
				//------------------------------------------------------------
				throw;
			}
		}
		#endregion

		//============================================================
		//	PUBLIC ROUTINES
		//============================================================
		#region Save()
		/// <summary>
		/// Saves the settings to disk.
		/// </summary>
        public void Save()
        {
            Type settingsType = this.GetType();

            //------------------------------------------------------------
            //	Enumerate through settings properties
            //------------------------------------------------------------
            foreach (PropertyInfo propertyInformation in settingsType.GetProperties())
            {
                if (propertyInformation.Name != "Instance")
                {
                    //------------------------------------------------------------
                    //	Extract property value and its string representation
                    //------------------------------------------------------------
                    object propertyValue = propertyInformation.GetValue(this, null);

                    string valueAsString;
                    //------------------------------------------------------------
                    //	Format null/default property values as empty strings
                    //------------------------------------------------------------
                    if (propertyValue == null || propertyValue.Equals(Int32.MinValue) || propertyValue.Equals(Single.MinValue))
                    {
                        valueAsString = String.Empty;
                    }
                    else
                    {
                        valueAsString = propertyValue.ToString();
                    }
                    
					updateSetting(propertyInformation.Name, valueAsString);
                }
            }

			Setting.SaveAll(CurrentSettings);
            OnChanged();
        }
		static void updateSetting(string name,string val) {
			var settings = BlogSettings.Instance.CurrentSettings;
			if (null == settings || settings.Count == 0) return ;
			var item = settings.SingleOrDefault(x=>x.SettingName.Equals(name,StringComparison.OrdinalIgnoreCase));
			if (null != item) {
				item.SettingValue = val;
			};
		}
		#endregion

		#region Version()

		private static string _Version;
		/// <summary>
		/// Returns the CoyoEden.NET version information.
		/// </summary>
		/// <value>The CoyoEden.NET major, minor, revision, and build numbers.</value>
		/// <remarks>The current version is determined by extracting the build version of the CoyoEden.Core assembly.</remarks>
		public string Version()
		{
			if (_Version == null)
				_Version = GetType().Assembly.GetName().Version.ToString();

			return _Version;
		}
		#endregion

		/// <summary>
		/// Gets the relative URL of the blog feed. If a Feedburner username
		/// is entered in the admin settings page, it will return the 
		/// absolute Feedburner URL to the feed.
		/// </summary>
		public static string FeedUrl
		{
			get
			{
				if (!string.IsNullOrEmpty(BlogSettings.Instance.AlternateFeedUrl))
					return BlogSettings.Instance.AlternateFeedUrl;
				else
                    return String.Format("{0}syndication.axd", SystemX.Web.Utils.AbsoluteWebRoot);
			}
		}
		/// <summary>
		/// Returns the default culture.  This is either the culture specified in the blog settings,
		/// or the default culture installed with the operating system.
		/// </summary>
		public static CultureInfo GetDefaultCulture()
		{
			if (string.IsNullOrEmpty(BlogSettings.Instance.Culture) ||
				BlogSettings.Instance.Culture.Equals("Auto", StringComparison.OrdinalIgnoreCase))
			{
				return CultureInfo.InstalledUICulture;
			}
			return CultureInfo.CreateSpecificCulture(BlogSettings.Instance.Culture);
		}
		public static string GetStorageLocation() {
			return AppSettingsHelper.Instance.GetString("App.StorageLocation", "~/app_data/");
		}
		public static string AppVersion() {
			return typeof(BlogSettings).Assembly.GetName().Version.ToString();
		}
		/// <summary>
		/// get a file of current theme in the relative url format
		/// </summary>
		/// <returns></returns>
		public static string GetFileOfCurTheme(string fileName,string defaultFile) {
			var file = string.Format("{0}themes/{1}/{2}",SystemX.Web.Utils.RelativeWebRoot,BlogSettings.Instance.Theme,fileName);
			var pFile = System.Web.HttpContext.Current.Request.MapPath(file);
			if (!System.IO.File.Exists(pFile)) {
				file = defaultFile;
			}
			return file;
		}
	}
}