
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Net;
using Cynthia.Web.Controls.google;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;
using System.Collections.Generic;

namespace Cynthia.Web.BlogUI
{
    public partial class BlogViewControl : CUserControl
    {
        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(BlogViewControl));
        private Hashtable moduleSettings;
        private string virtualRoot;
        private string addThisAccountId = string.Empty;
        private bool useAddThisMouseOverWidget = true;
		protected Blog ThePost { get; set; }
        private Module module;
		protected bool HideDetailsFromUnauthencticated { get; set; }
        private SiteSettings siteSettings = null;
		protected string DeleteLinkImage = String.Format("~/Data/SiteImages/{0}", WebConfigSettings.DeleteLinkImage);

        #endregion

        #region Protected Properties

        protected int PageId = -1;
        protected int ModuleId = -1;
        protected int ItemId = -1;
        protected bool AllowComments = false;
        protected bool AllowWebSiteUrlForComments = true;
        protected string CommentDateTimeFormat;
        protected bool parametersAreInvalid = false;
        protected bool ShowCategories = false;
        protected bool BlogUseTagCloudForCategoriesSetting = false;
        protected bool ShowArchives = false;
        protected bool NavigationOnRight = false;
        protected bool ShowStatistics = true;
        protected bool ShowFeedLinks = true;
        protected bool ShowAddFeedLinks = true;
        protected bool UseCommentSpamBlocker = true;
        protected bool RequireAuthenticationForComments = false;
		protected bool IsEditable { get; set; }
        protected string addThisCustomBrand = string.Empty;
        protected string addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";
        protected string addThisCustomOptions = string.Empty;
        protected string addThisCustomLogoUrl = string.Empty;
        protected string addThisCustomLogoBackColor = string.Empty;
        protected string addThisCustomLogoForeColor = string.Empty;
        protected string GmapApiKey = string.Empty;
        protected int GoogleMapHeightSetting = 300;
        protected int GoogleMapWidthSetting = 500;
        protected bool GoogleMapEnableMapTypeSetting = false;
        protected bool GoogleMapEnableZoomSetting = false;
        protected bool GoogleMapShowInfoWindowSetting = false;
        protected bool GoogleMapEnableLocalSearchSetting = false;
        protected bool GoogleMapEnableDirectionsSetting = false;
        protected MapType mapType = MapType.G_SATELLITE_MAP;
        protected int GoogleMapInitialZoomSetting = 13;
        protected string OdiogoFeedIDSetting = string.Empty;
        protected bool EnableContentRatingSetting = false;
        protected bool EnableRatingCommentsSetting = false;
        protected bool ShowPostAuthorSetting = false;
		protected string BlogAuthor { get; set; }

        protected int ExcerptLength = 250;
        protected string ExcerptSuffix = "...";
        private string CommentSystem = "internal";
        private string DisqusSiteShortName = string.Empty;
        private string IntenseDebateAccountId = string.Empty;

        private bool showLeftContent = false;
        private bool showRightContent = false;

        protected string RegexRelativeImageUrlPatern = @"^/.*[_a-zA-Z0-9]+\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$";
		/// <summary>
		/// url of Previous Post 
		/// </summary>
		protected string PreviousPostUrl { get; set; }
		/// <summary>
		/// whether has previous post
		/// </summary>
		protected bool HasPrevious { get { return !string.IsNullOrEmpty(PreviousPostUrl); } }
		/// <summary>
		/// Url of Next post
		/// </summary>
		protected string NextPostUrl { get; set; }
		/// <summary>
		/// whether has next post
		/// </summary>
		protected bool HasNext { get { return !string.IsNullOrEmpty(NextPostUrl); } }
		protected string PostEditUrl { get; set; }
		/// <summary>
		/// Show excerpt only
		/// </summary>
		protected bool ShowExcerptOnly { get; set; }
		/// <summary>
		/// blog copyright text
		/// </summary>
		protected string BlogCopyright { get; set; }
		/// <summary>
		/// whether use external comment system like 'disqus'
		/// </summary>
		protected bool UseExternalCommentService { get; set; }
		/// <summary>
		/// whether show addthis button
		/// </summary>
		protected bool ShowAddThisButton { get; set; }
        #endregion

        private void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (
                (BasePage == null)
                || (!BasePage.UserCanViewPage())
            )
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (parametersAreInvalid)
            {
                AllowComments = false;
                this.pnlBlog.Visible = false;
                return;
            }

            LoadSettings();
            SetupCss();
            PopulateLabels();

            if (!IsPostBack && ModuleId > 0 && ItemId > 0)
            {

                if (Context.User.Identity.IsAuthenticated)
                {
                    if (WebUser.HasEditPermissions(BasePage.SiteInfo.SiteId, ModuleId, BasePage.CurrentPage.PageId))
                    {
                        IsEditable = true;
                    }
                }

                PopulateControls();
            }

            BasePage.LoadSideContent(showLeftContent, showRightContent);

        }



        protected virtual void PopulateControls()
        {
            if (parametersAreInvalid)
            {
                AllowComments = false;
                pnlBlog.Visible = false;
                return;
            }

            // if not published only the editor can see it
            if (
                ((!ThePost.IsPublished) || (ThePost.StartDate > DateTime.UtcNow))
                &&(!BasePage.UserCanEditModule(ModuleId))
                )
            {
                AllowComments = false;
                pnlBlog.Visible = false;
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }
			//set post edit url
			PostEditUrl = String.Format("{0}/Blog/EditPost.aspx?pageid={1}&ItemID={2}&mid={3}", BasePage.SiteRoot, PageId, ItemId.ToString(CultureInfo.InvariantCulture), ModuleId.ToString(CultureInfo.InvariantCulture));

            BasePage.Title = SiteUtils.FormatPageTitle(BasePage.SiteInfo, ThePost.Title);
            BasePage.MetaDescription = ThePost.MetaDescription;
            BasePage.MetaKeywordCsv = ThePost.MetaKeywords;
            BasePage.AdditionalMetaMarkup = ThePost.CompiledMeta;
			addThis1.TitleOfUrlToShare = ThePost.Title;
            addThis1.AccountId = addThisAccountId;
            addThis1.CustomBrand = addThisCustomBrand;
            addThis1.UseMouseOverWidget = useAddThisMouseOverWidget;
            addThis1.ButtonImageUrl = addThisButtonImageUrl;
            addThis1.CustomLogoBackgroundColor = addThisCustomLogoBackColor;
            addThis1.CustomLogoColor = addThisCustomLogoForeColor;
            addThis1.CustomLogoUrl = addThisCustomLogoUrl;
            addThis1.CustomOptions = addThisCustomOptions;

            txtCommentTitle.Text = "re: " + ThePost.Title;

            odiogoPlayer.OdiogoFeedId = OdiogoFeedIDSetting;
            odiogoPlayer.ItemId = ThePost.ItemId.ToString(CultureInfo.InvariantCulture);
            odiogoPlayer.ItemTitle = ThePost.Title;
            if (BlogAuthor.Length == 0) { BlogAuthor = ThePost.UserName; }

            if (ThePost.PreviousPostUrl.Length > 0)
            {
				PreviousPostUrl = String.Format("{0}{1}", BasePage.SiteRoot, ThePost.PreviousPostUrl.Replace("~", string.Empty));
            }
            
            if (ThePost.NextPostUrl.Length > 0)
            {
				NextPostUrl = String.Format("{0}{1}", BasePage.SiteRoot, ThePost.NextPostUrl.Replace("~", string.Empty));
            }
            
            if (ThePost.Location.Length > 0)
            {
                gmap.Visible = true;
                gmap.GMapApiKey = GmapApiKey;
                gmap.Location = ThePost.Location;
                gmap.EnableMapType = GoogleMapEnableMapTypeSetting;
                gmap.EnableZoom = GoogleMapEnableZoomSetting;
                gmap.ShowInfoWindow = GoogleMapShowInfoWindowSetting;
                gmap.EnableLocalSearch = GoogleMapEnableLocalSearchSetting;
                gmap.MapHeight = GoogleMapHeightSetting;
                gmap.MapWidth = GoogleMapWidthSetting;
                gmap.EnableDrivingDirections = GoogleMapEnableDirectionsSetting;
                gmap.GmapType = mapType;
                gmap.ZoomLevel = GoogleMapInitialZoomSetting;
                gmap.DirectionsButtonText = BlogResources.MapGetDirectionsButton;
                gmap.DirectionsButtonToolTip = BlogResources.MapGetDirectionsButton;
            }

            using (IDataReader dataReader = Blog.GetBlogComments(ModuleId, ItemId))
            {
                dlComments.DataSource = dataReader;
                dlComments.DataBind();
            }

            

            PopulateNavigation();

            if (Page.Header == null) { return; }

            Literal link = new Literal();
            link.ID = "blogurl";
            link.Text = "\n<link rel='canonical' href='"
                + SiteRoot
                + ThePost.ItemUrl.Replace("~/", "/")
                + "' />";

            Page.Header.Controls.Add(link);

        }

        
        protected virtual void PopulateNavigation()
        {
            Feeds.ModuleSettings = moduleSettings;
            Feeds.PageId = PageId;
            Feeds.ModuleId = ModuleId;
            Feeds.Visible = ShowFeedLinks;

            if (this.ShowCategories)
            {
                tags.CanEdit = IsEditable;
                tags.PageId = PageId;
                tags.ModuleId = ModuleId;
                tags.SiteRoot = SiteRoot;
                tags.RenderAsTagCloud = BlogUseTagCloudForCategoriesSetting;
            }
            else
            {
                this.pnlCategories.Visible = false;
                tags.Visible = false;
            }

            if (this.ShowArchives)
            {
                archive.PageId = PageId;
                archive.ModuleId = ModuleId;
                archive.SiteRoot = SiteRoot;
            }
            else
            {
                archive.Visible = false;
                this.pnlArchives.Visible = false;
            }

            int countOfDrafts = Blog.CountOfDrafts(ModuleId);

            stats.PageId = PageId;
            stats.ModuleId = ModuleId;
            stats.CountOfDrafts = countOfDrafts;
            stats.Visible = ShowStatistics;

        }



        private void PopulateLabels()
        {
            
            edComment.WebEditor.ToolBar = ToolBar.AnonymousUser;
            edComment.WebEditor.Height = Unit.Pixel(170);

            captcha.ProviderName = BasePage.SiteInfo.CaptchaProvider;
            captcha.Captcha.ControlID = "captcha" + ModuleId.ToInvariantString();
            captcha.RecaptchaPrivateKey = BasePage.SiteInfo.RecaptchaPrivateKey;
            captcha.RecaptchaPublicKey = BasePage.SiteInfo.RecaptchaPublicKey;

            regexUrl.ErrorMessage = BlogResources.WebSiteUrlRegexWarning;

            btnPostComment.Text = BlogResources.BlogCommentPostCommentButton;
            SiteUtils.SetButtonAccessKey(btnPostComment, BlogResources.BlogCommentPostCommentButtonAccessKey);

            litCommentsClosed.Text = BlogResources.BlogCommentsClosedMessage;
            litCommentsRequireAuthentication.Text = BlogResources.CommentsRequireAuthenticationMessage;

            addThis1.Text = BlogResources.AddThisButtonAltText;

        }


		#region UI helper methods
		private void btnPostComment_Click(object sender, EventArgs e)
		{
			if (!AllowComments)
			{
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}
			if (!IsValidComment())
			{
				PopulateControls();
				return;
			}
			if (ThePost == null) { return; }
			if (moduleSettings == null) { return; }
			if (ThePost.AllowCommentsForDays < 0)
			{
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}

			DateTime endDate = ThePost.StartDate.AddDays((double)ThePost.AllowCommentsForDays);

			if ((endDate < DateTime.UtcNow) && (ThePost.AllowCommentsForDays > 0)) return;

			if (this.chkRememberMe.Checked)
			{
				SetCookies();
			}

			Blog.AddBlogComment(
					ModuleId,
					ItemId,
					this.txtName.Text,
					this.txtCommentTitle.Text,
					this.txtURL.Text,
					edComment.Text,
					DateTime.UtcNow);

			if (moduleSettings.ContainsKey("ContentNotifyOnComment"))
			{
				string notify = moduleSettings["ContentNotifyOnComment"].ToString();
				string email = moduleSettings["BlogAuthorEmailSetting"].ToString();
				if ((notify == "true") && (Email.IsValidEmailAddressSyntax(email)))
				{
					//added this 2008-08-07 due to blog coment spam and need to be able to ban the ip of the spammer
					StringBuilder message = new StringBuilder();
					message.Append(BasePage.SiteRoot + ThePost.ItemUrl.Replace("~", string.Empty));

					message.Append("\n\nHTTP_USER_AGENT: " + Page.Request.ServerVariables["HTTP_USER_AGENT"] + "\n");
					message.Append("HTTP_HOST: " + Page.Request.ServerVariables["HTTP_HOST"] + "\n");
					message.Append("REMOTE_HOST: " + Page.Request.ServerVariables["REMOTE_HOST"] + "\n");
					message.Append("REMOTE_ADDR: " + SiteUtils.GetIP4Address() + "\n");
					message.Append("LOCAL_ADDR: " + Page.Request.ServerVariables["LOCAL_ADDR"] + "\n");
					message.Append("HTTP_REFERER: " + Page.Request.ServerVariables["HTTP_REFERER"] + "\n");

					Notification.SendBlogCommentNotificationEmail(
						SiteUtils.GetSmtpSettings(),
						ResourceHelper.GetMessageTemplate(ResourceHelper.GetDefaultCulture(), "BlogCommentNotificationEmail.config"),
						BasePage.SiteInfo.DefaultEmailFromAddress,
						BasePage.SiteRoot,
						email,
						message.ToString());

				}
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);

		}
		/// <summary>
        /// Handles the item command
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dlComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                Blog.DeleteBlogComment(int.Parse(e.CommandArgument.ToString()));
                WebUtils.SetupRedirect(this, Request.RawUrl);

            }
        }


        void dlComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
            UIHelper.AddConfirmationDialog(btnDelete, BlogResources.BlogDeleteCommentWarning);
		}

		#endregion


		#region helper methods

		private void LoadSettings()
		{
			ThePost = new Blog(ItemId);
			module = new Module(ModuleId, BasePage.CurrentPage.PageId);

			siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (
				(module.ModuleId == -1)
				|| (ThePost.ModuleId == -1)
				|| (ThePost.ModuleId != module.ModuleId)
				|| (siteSettings == null)
				)
			{
				// query string params have been manipulated
				this.pnlBlog.Visible = false;
				AllowComments = false;
				parametersAreInvalid = true;
				return;
			}
			ShowAddThisButton = siteSettings.AddThisDotComUsername.Length > 0;
			RegexRelativeImageUrlPatern = SiteUtils.GetRegexRelativeImageUrlPatern();

			moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);

			GmapApiKey = SiteUtils.GetGmapApiKey();

			ShowCategories = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogShowCategoriesSetting", ShowCategories);

			BlogUseTagCloudForCategoriesSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogUseTagCloudForCategoriesSetting", BlogUseTagCloudForCategoriesSetting);

			ShowArchives = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogShowArchiveSetting", ShowArchives);

			NavigationOnRight = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogNavigationOnRightSetting", NavigationOnRight);

			ShowStatistics = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogShowStatisticsSetting", ShowStatistics);

			ShowFeedLinks = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogShowFeedLinksSetting", ShowFeedLinks);

			ShowAddFeedLinks = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogShowAddFeedLinksSetting", ShowAddFeedLinks);

			UseCommentSpamBlocker = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogUseCommentSpamBlocker", UseCommentSpamBlocker);

			RequireAuthenticationForComments = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "RequireAuthenticationForComments", RequireAuthenticationForComments);

			AllowComments = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogAllowComments", AllowComments);

			EnableContentRatingSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "EnableContentRatingSetting", EnableContentRatingSetting);

			EnableRatingCommentsSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "EnableRatingCommentsSetting", EnableRatingCommentsSetting);

			ShowPostAuthorSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "ShowPostAuthorSetting", ShowPostAuthorSetting);

			AllowWebSiteUrlForComments = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "AllowWebSiteUrlForComments", AllowWebSiteUrlForComments);

			HideDetailsFromUnauthencticated = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "HideDetailsFromUnauthencticated", HideDetailsFromUnauthencticated);

			ExcerptLength = WebUtils.ParseInt32FromHashtable(
				moduleSettings, "BlogExcerptLengthSetting", ExcerptLength);

			if (moduleSettings.Contains("BlogExcerptSuffixSetting"))
			{
				ExcerptSuffix = moduleSettings["BlogExcerptSuffixSetting"].ToString();
			}

			if (moduleSettings.Contains("DisqusSiteShortName"))
			{
				DisqusSiteShortName = moduleSettings["DisqusSiteShortName"].ToString();
			}

			if (moduleSettings.Contains("CommentSystemSetting"))
			{
				CommentSystem = moduleSettings["CommentSystemSetting"].ToString();
			}

			if (moduleSettings.Contains("IntenseDebateAccountId"))
			{
				IntenseDebateAccountId = moduleSettings["IntenseDebateAccountId"].ToString();
			}

			CommentDateTimeFormat = DateFormat;
			if (moduleSettings.Contains("BlogDateTimeFormat"))
			{
				DateFormat = moduleSettings["BlogDateTimeFormat"].ToString().Trim();
				if (DateFormat.Length > 0)
				{
					try
					{
						string d = DateTime.Now.ToString(DateFormat, CultureInfo.CurrentCulture);
					}
					catch (FormatException)
					{
						DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
					}
				}
				else
				{
					DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
				}
			}

			divCommentUrl.Visible = AllowWebSiteUrlForComments;

			((CRating)Rating).Enabled = EnableContentRatingSetting;
			((CRating)Rating).AllowFeedback = EnableRatingCommentsSetting;
			((CRating)Rating).ContentGuid = ThePost.BlogGuid;

			if (moduleSettings.Contains("GoogleMapInitialMapTypeSetting"))
			{
				string gmType = moduleSettings["GoogleMapInitialMapTypeSetting"].ToString();
				try
				{
					mapType = (MapType)Enum.Parse(typeof(MapType), gmType);
				}
				catch (ArgumentException) { }

			}

			GoogleMapHeightSetting = WebUtils.ParseInt32FromHashtable(
				moduleSettings, "GoogleMapHeightSetting", GoogleMapHeightSetting);

			GoogleMapWidthSetting = WebUtils.ParseInt32FromHashtable(
				moduleSettings, "GoogleMapWidthSetting", GoogleMapWidthSetting);

			GoogleMapInitialZoomSetting = WebUtils.ParseInt32FromHashtable(
				moduleSettings, "GoogleMapInitialZoomSetting", GoogleMapInitialZoomSetting);


			GoogleMapEnableMapTypeSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "GoogleMapEnableMapTypeSetting", false);

			GoogleMapEnableZoomSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "GoogleMapEnableZoomSetting", false);

			GoogleMapShowInfoWindowSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "GoogleMapShowInfoWindowSetting", false);

			GoogleMapEnableLocalSearchSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "GoogleMapEnableLocalSearchSetting", false);

			GoogleMapEnableDirectionsSetting = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "GoogleMapEnableDirectionsSetting", false);

			if (moduleSettings.Contains("OdiogoFeedIDSetting"))
				OdiogoFeedIDSetting = moduleSettings["OdiogoFeedIDSetting"].ToString();

			addThisAccountId = BasePage.SiteInfo.AddThisDotComUsername;
			string altAddThisAccount = string.Empty;

			if (moduleSettings.Contains("BlogAddThisDotComUsernameSetting"))
				altAddThisAccount = moduleSettings["BlogAddThisDotComUsernameSetting"].ToString().Trim();

			if (altAddThisAccount.Length > 0)
				addThisAccountId = altAddThisAccount;

			useAddThisMouseOverWidget = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "BlogAddThisDotComUseMouseOverWidgetSetting", useAddThisMouseOverWidget);

			if (moduleSettings.Contains("BlogAddThisButtonImageUrlSetting"))
				addThisButtonImageUrl = moduleSettings["BlogAddThisButtonImageUrlSetting"].ToString().Trim();

			if (addThisButtonImageUrl.Length == 0)
				addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";

			if (moduleSettings.Contains("BlogAddThisCustomBrandSetting"))
				addThisCustomBrand = moduleSettings["BlogAddThisCustomBrandSetting"].ToString().Trim();

			if (addThisCustomBrand.Length == 0)
				addThisCustomBrand = BasePage.SiteInfo.SiteName;

			if (moduleSettings.Contains("BlogAddThisCustomOptionsSetting"))
				addThisCustomOptions = moduleSettings["BlogAddThisCustomOptionsSetting"].ToString().Trim();

			if (moduleSettings.Contains("BlogAddThisCustomLogoUrlSetting"))
				addThisCustomLogoUrl = moduleSettings["BlogAddThisCustomLogoUrlSetting"].ToString().Trim();

			if (moduleSettings.Contains("BlogAddThisCustomLogoBackColorSetting"))
				addThisCustomLogoBackColor = moduleSettings["BlogAddThisCustomLogoBackColorSetting"].ToString().Trim();

			if (moduleSettings.Contains("BlogAddThisCustomLogoForeColorSetting"))
				addThisCustomLogoForeColor = moduleSettings["BlogAddThisCustomLogoForeColorSetting"].ToString().Trim();

			if (moduleSettings.Contains("BlogCopyrightSetting"))
			{
				BlogCopyright = moduleSettings["BlogCopyrightSetting"].ToString();
			}

			if (moduleSettings.Contains("BlogAuthorSetting"))
			{
				BlogAuthor = moduleSettings["BlogAuthorSetting"].ToString().Trim();
			}
			BlogAuthor = BlogAuthor ?? "";

			pnlStatistics.Visible = ShowStatistics;

			divNav.Visible = false;
			if (ShowArchives
				|| ShowAddFeedLinks
				|| ShowCategories
				|| ShowFeedLinks
				|| ShowStatistics)
			{
				divNav.Visible = true;
			}

			if (!divNav.Visible)
			{
				divblog.CssClass = "blogcenter-nonav";
			}

			showLeftContent = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "ShowPageLeftContentSetting", showLeftContent);

			showRightContent = WebUtils.ParseBoolFromHashtable(
				moduleSettings, "ShowPageRightContentSetting", showRightContent);

			if (ThePost.AllowCommentsForDays < 0)
			{
				pnlNewComment.Visible = false;
				pnlCommentsClosed.Visible = true;
				AllowComments = false;
			}

			if (ThePost.AllowCommentsForDays == 0)
			{
				pnlNewComment.Visible = true;
				pnlCommentsClosed.Visible = false;
				AllowComments = true;
			}

			if (ThePost.AllowCommentsForDays > 0)
			{
				DateTime endDate = ThePost.StartDate.AddDays((double)ThePost.AllowCommentsForDays);


				if (endDate > DateTime.UtcNow)
				{
					pnlNewComment.Visible = true;
					pnlCommentsClosed.Visible = false;
					AllowComments = true;
				}
				else
				{
					pnlNewComment.Visible = false;
					pnlCommentsClosed.Visible = true;
					AllowComments = false;
				}
			}

			if (AllowComments)
			{
				if ((RequireAuthenticationForComments) && (!Request.IsAuthenticated))
				{
					AllowComments = false;
					pnlNewComment.Visible = false;
					pnlCommentsRequireAuthentication.Visible = true;
				}

			}

			if (!UseCommentSpamBlocker)
			{
				pnlAntiSpam.Visible = false;
				captcha.Visible = false;
				pnlNewComment.Controls.Remove(captcha);
			}

			//external comments service renderring
			loadExternalCommentService();

			if (!this.NavigationOnRight)
			{
				this.divNav.CssClass = "blognavleft";
				this.divblog.CssClass = "blogcenter-leftnav";

			}

			if (Request.IsAuthenticated)
			{
				SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
				this.txtName.Text = currentUser.Name;
				txtURL.Text = currentUser.WebSiteUrl;
				ShowExcerptOnly = false;
			}
			else
			{
				if ((HideDetailsFromUnauthencticated) && (ThePost.Description.Length > ExcerptLength))
				{
					pnlDetails.Visible = false;
					pnlExcerpt.Visible = true;
					ShowExcerptOnly = true;
				}

				if (CookieHelper.CookieExists("blogUser"))
				{
					this.txtName.Text = CookieHelper.GetCookieValue("blogUser");
				}
				if (CookieHelper.CookieExists("blogUrl"))
				{
					this.txtURL.Text = CookieHelper.GetCookieValue("blogUrl");
				}
			}

		}

		private void loadExternalCommentService()
		{
			if (!AllowComments) return;
			var externalSystem = new List<string> { "disqus", "intensedebate" };
			if (!externalSystem.Contains(CommentSystem)) return;
			if (ThePost.CommentCount > 0) return;

			switch (CommentSystem)
			{
				case "disqus":
					if (DisqusSiteShortName.Length > 0) {
						UseExternalCommentService = true;
						disqus.SiteShortName = DisqusSiteShortName;
						disqus.RenderWidget = true;
						disqus.WidgetPageUrl = FormatBlogUrl(ThePost.ItemUrl, ThePost.ItemId);
						if (UseCommentSpamBlocker) { this.Controls.Remove(pnlAntiSpam); }
					}
					break;
				case "intensedebate":
					if (IntenseDebateAccountId.Length > 0) {
						UseExternalCommentService = true;
						intenseDebate.AccountId = IntenseDebateAccountId;
						intenseDebate.PostUrl = FormatBlogUrl(ThePost.ItemUrl, ThePost.ItemId);
						if (UseCommentSpamBlocker) { this.Controls.Remove(pnlAntiSpam); }
					}
					break;
			}
			stats.ShowCommentCount=!UseExternalCommentService;
		}

		protected string FormatBlogUrl(string itemUrl, int itemId)
		{
			if (itemUrl.Length > 0)
				return SiteRoot + itemUrl.Replace("~", string.Empty);

			return String.Format("{0}/Blog/ViewPost.aspx?pageid={1}&ItemID={2}&mid={3}", SiteRoot, PageId.ToInvariantString(), itemId.ToInvariantString(), ModuleId.ToInvariantString());

		}


		private bool IsValidComment()
		{
			if (parametersAreInvalid) { return false; }

			if (!AllowComments) { return false; }

			if ((CommentSystem != "internal") && (ThePost.CommentCount == 0)) { return false; }

			if (edComment.Text.Length == 0) { return false; }
			if (edComment.Text == "<p>&#160;</p>") { return false; }

			bool result = true;

			try
			{
				Page.Validate();
				result = Page.IsValid;

			}
			catch (NullReferenceException)
			{
				//Recaptcha throws nullReference here if it is not visible/disabled
			}
			catch (ArgumentNullException)
			{
				//manipulation can make the Challenge null on recaptcha
			}


			try
			{
				if ((result) && (UseCommentSpamBlocker))
				{
					result = captcha.IsValid;
				}
			}
			catch (NullReferenceException)
			{
				return false;
			}
			catch (ArgumentNullException)
			{
				//manipulation can make the Challenge null on recaptcha
				return false;
			}


			return result;
		}
		private void SetCookies()
		{
			HttpCookie blogUserCookie = new HttpCookie("blogUser", this.txtName.Text);
			blogUserCookie.Expires = DateTime.Now.AddMonths(1);
			Response.Cookies.Add(blogUserCookie);

			HttpCookie blogUrlCookie = new HttpCookie("LinkUrl", this.txtURL.Text);
			blogUrlCookie.Expires = DateTime.Now.AddMonths(1);
			Response.Cookies.Add(blogUrlCookie);
		}
		private void SetupCss()
        {
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("blogcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "blogcss";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl()
                    + "blogmodule.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }

        }

        private void LoadParams()
        {
            virtualRoot = WebUtils.GetApplicationRoot();
            PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            ModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            ItemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);

            if (PageId == -1) parametersAreInvalid = true;
            if (ModuleId == -1) parametersAreInvalid = true;
            if (ItemId == -1) parametersAreInvalid = true;
            if (!BasePage.UserCanViewPage(ModuleId)) { parametersAreInvalid = true; }

            addThisAccountId = BasePage.SiteInfo.AddThisDotComUsername;
            addThisCustomBrand = BasePage.SiteInfo.SiteName;


		}
		#endregion

		#region base overrides

		override protected void OnInit(EventArgs e)
		{
			this.Load += new EventHandler(this.Page_Load);
			this.btnPostComment.Click += new EventHandler(this.btnPostComment_Click);
			this.dlComments.ItemCommand += new RepeaterCommandEventHandler(dlComments_ItemCommand);
			this.dlComments.ItemDataBound += new RepeaterItemEventHandler(dlComments_ItemDataBound);
			base.OnInit(e);

			SiteUtils.SetupEditor(this.edComment);

		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
		}
		protected override void Render(HtmlTextWriter writer)
        {
            if ((Page.IsPostBack) &&AllowComments&&(!UseExternalCommentService))
            { 
                WebUtils.SetupRedirect(this, Request.RawUrl); 
                return; 
            }

            base.Render(writer);
		}
		#endregion
	}
}