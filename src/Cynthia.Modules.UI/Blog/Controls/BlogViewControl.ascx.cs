
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
        private bool HideDetailsFromUnauthencticated = false;
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
        protected bool IsEditable = false;
        protected string addThisCustomBrand = string.Empty;
        protected string addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";
        protected string addThisCustomOptions = string.Empty;
        protected string addThisCustomLogoUrl = string.Empty;
        protected string addThisCustomLogoBackColor = string.Empty;
        protected string addThisCustomLogoForeColor = string.Empty;
        protected string EditContentImage = ConfigurationManager.AppSettings["EditContentImage"];
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
        protected string SiteRoot = string.Empty;
        protected string ImageSiteRoot = string.Empty;
        private CBasePage basePage;
        protected int ExcerptLength = 250;
        protected string ExcerptSuffix = "...";
        private string CommentSystem = "internal";
        private string DisqusSiteShortName = string.Empty;
        private string IntenseDebateAccountId = string.Empty;

        private bool showLeftContent = false;
        private bool showRightContent = false;

        protected string RegexRelativeImageUrlPatern = @"^/.*[_a-zA-Z0-9]+\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$";

        #endregion

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnPostComment.Click += new EventHandler(this.btnPostComment_Click);
            this.dlComments.ItemCommand += new RepeaterCommandEventHandler(dlComments_ItemCommand);
            this.dlComments.ItemDataBound += new RepeaterItemEventHandler(dlComments_ItemDataBound);
            base.OnInit(e);
            //this.EnableViewState = this.UserCanEditPage();
            basePage = Page as CBasePage;
            SiteRoot = basePage.SiteRoot;
            ImageSiteRoot = basePage.ImageSiteRoot;

            SiteUtils.SetupEditor(this.edComment);

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }


        #endregion

        private void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (
                (basePage == null)
                || (!basePage.UserCanViewPage())
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
                    if (WebUser.HasEditPermissions(basePage.SiteInfo.SiteId, ModuleId, basePage.CurrentPage.PageId))
                    {
                        IsEditable = true;
                    }
                }

                PopulateControls();
            }

            basePage.LoadSideContent(showLeftContent, showRightContent);

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
                &&(!basePage.UserCanEditModule(ModuleId))
                )
            {
                AllowComments = false;
                pnlBlog.Visible = false;
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            if (this.IsEditable)
            {
                this.lnkEdit.Visible = true;
                if (!basePage.UseTextLinksForFeatureSettings)
                {
					lnkEdit.ImageUrl = String.Format("{0}/Data/SiteImages/{1}", basePage.ImageSiteRoot, EditContentImage);
                    lnkEdit.Text = BlogResources.EditImageAltText;
                }
                else
                {
                    lnkEdit.Text = BlogResources.BlogEditEntryLink;
                }
               

                this.lnkEdit.NavigateUrl = basePage.SiteRoot + "/Blog/EditPost.aspx?pageid="
                    + PageId.ToString()
                    + "&ItemID=" + ItemId.ToString(CultureInfo.InvariantCulture)
                    + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture);

                
            }

            basePage.Title = SiteUtils.FormatPageTitle(basePage.SiteInfo, ThePost.Title);
            basePage.MetaDescription = ThePost.MetaDescription;
            basePage.MetaKeywordCsv = ThePost.MetaKeywords;
            basePage.AdditionalMetaMarkup = ThePost.CompiledMeta;
            litTitle.Text = Server.HtmlEncode(ThePost.Title);
            addThis1.TitleOfUrlToShare = litTitle.Text;
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
            litDescription.Text = ThePost.Description;
            litExcerpt.Text = GetExcerpt(ThePost);

            if (ThePost.PreviousPostUrl.Length > 0)
            {
                lnkPreviousPostTop.Visible = true;
                lnkPreviousPost.Visible = true;
                lnkPreviousPostTop.NavigateUrl = basePage.SiteRoot + ThePost.PreviousPostUrl.Replace("~", string.Empty);
                lnkPreviousPostTop.ToolTip = ThePost.PreviousPostTitle;

                lnkPreviousPost.NavigateUrl = basePage.SiteRoot + ThePost.PreviousPostUrl.Replace("~", string.Empty);
                lnkPreviousPost.ToolTip = ThePost.PreviousPostTitle;

            }
            
            if (ThePost.NextPostUrl.Length > 0)
            {
                lnkNextPostTop.Visible = true;
                lnkNextPost.Visible = true;
                lnkNextPostTop.NavigateUrl = basePage.SiteRoot + ThePost.NextPostUrl.Replace("~", string.Empty);
                lnkNextPostTop.ToolTip = ThePost.NextPostTitle;

                lnkNextPost.NavigateUrl = basePage.SiteRoot + ThePost.NextPostUrl.Replace("~", string.Empty);
                lnkNextPost.ToolTip = ThePost.NextPostTitle;
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
                if ((notify == "true")&&(Email.IsValidEmailAddressSyntax(email)))
                {
                    //added this 2008-08-07 due to blog coment spam and need to be able to ban the ip of the spammer
                    StringBuilder message = new StringBuilder();
                    message.Append(basePage.SiteRoot + ThePost.ItemUrl.Replace("~", string.Empty));

                    message.Append("\n\nHTTP_USER_AGENT: " + Page.Request.ServerVariables["HTTP_USER_AGENT"] + "\n");
                    message.Append("HTTP_HOST: " + Page.Request.ServerVariables["HTTP_HOST"] + "\n");
                    message.Append("REMOTE_HOST: " + Page.Request.ServerVariables["REMOTE_HOST"] + "\n");
                    message.Append("REMOTE_ADDR: " + SiteUtils.GetIP4Address() + "\n");
                    message.Append("LOCAL_ADDR: " + Page.Request.ServerVariables["LOCAL_ADDR"] + "\n");
                    message.Append("HTTP_REFERER: " + Page.Request.ServerVariables["HTTP_REFERER"] + "\n");

                    Notification.SendBlogCommentNotificationEmail(
                        SiteUtils.GetSmtpSettings(),
                        ResourceHelper.GetMessageTemplate(ResourceHelper.GetDefaultCulture(), "BlogCommentNotificationEmail.config"),
                        basePage.SiteInfo.DefaultEmailFromAddress,
                        basePage.SiteRoot,
                        email,
                        message.ToString());
                    
                }
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void PopulateLabels()
        {
            
            lnkEdit.ToolTip = BlogResources.BlogEditEntryLink;
            
           // edComment.WebEditor.ScriptBaseUrl = WebUtils.GetSiteRoot() + "/ClientScript";
            //edComment.WebEditor.SiteRoot = basePage.SiteRoot;
            //edComment.WebEditor.EditorCSSUrl = SiteUtils.GetEditorStyleSheetUrl();
            edComment.WebEditor.ToolBar = ToolBar.AnonymousUser;
            //edComment.WebEditor.SkinName = basePage.SiteInfo.EditorSkin.ToString();
            edComment.WebEditor.Height = Unit.Pixel(170);

            //CultureInfo defaultCulture = ResourceHelper.GetDefaultCulture();
            //if (defaultCulture.TextInfo.IsRightToLeft)
            //{
            //    edComment.WebEditor.TextDirection = Direction.RightToLeft;
            //}

            captcha.ProviderName = basePage.SiteInfo.CaptchaProvider;
            captcha.Captcha.ControlID = "captcha" + ModuleId.ToInvariantString();
            captcha.RecaptchaPrivateKey = basePage.SiteInfo.RecaptchaPrivateKey;
            captcha.RecaptchaPublicKey = basePage.SiteInfo.RecaptchaPublicKey;

            regexUrl.ErrorMessage = BlogResources.WebSiteUrlRegexWarning;

            btnPostComment.Text = BlogResources.BlogCommentPostCommentButton;
            SiteUtils.SetButtonAccessKey(btnPostComment, BlogResources.BlogCommentPostCommentButtonAccessKey);

            //UIHelper.DisableButtonAfterClick(
            //    btnPostComment,
            //    BlogResources.ButtonDisabledPleaseWait,
            //    Page.ClientScript.GetPostBackEventReference(this.btnPostComment, string.Empty)
            //    );

            litCommentsClosed.Text = BlogResources.BlogCommentsClosedMessage;
            litCommentsRequireAuthentication.Text = BlogResources.CommentsRequireAuthenticationMessage;

            addThis1.Text = BlogResources.AddThisButtonAltText;

            lnkPreviousPostTop.Text = Server.HtmlEncode(BlogResources.BlogPreviousPostLink);
            lnkNextPostTop.Text = Server.HtmlEncode(BlogResources.BlogNextPostLink);

            lnkPreviousPost.Text = Server.HtmlEncode(BlogResources.BlogPreviousPostLink);
            lnkNextPost.Text = Server.HtmlEncode(BlogResources.BlogNextPostLink);

        }

        private void LoadSettings()
        {
            ThePost = new Blog(ItemId);
            module = new Module(ModuleId, basePage.CurrentPage.PageId);

            siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (
                (module.ModuleId == -1)
                || (ThePost.ModuleId == -1)
                || (ThePost.ModuleId != module.ModuleId)
                ||(siteSettings == null)
                )
            {
                // query string params have been manipulated
                this.pnlBlog.Visible = false;
                AllowComments = false;
                parametersAreInvalid = true;
                return;
            }

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

            addThisAccountId = basePage.SiteInfo.AddThisDotComUsername;
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
                addThisCustomBrand = basePage.SiteInfo.SiteName;

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
                lblCopyright.Text = moduleSettings["BlogCopyrightSetting"].ToString();
            }

            if (moduleSettings.Contains("BlogAuthorSetting"))
            {
                BlogAuthor = moduleSettings["BlogAuthorSetting"].ToString().Trim();
            }
			BlogAuthor = BlogAuthor ?? "";

            pnlStatistics.Visible = ShowStatistics;
            pnlFeedback.Visible = AllowComments;

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
            


            if (AllowComments)
            {
                if ((CommentSystem == "disqus") && (DisqusSiteShortName.Length > 0))
                {
                   
                    // don't use new external comment system for existing posts that already have comments
                    if (ThePost.CommentCount == 0)
                    {
                        disqus.SiteShortName = DisqusSiteShortName;
                        disqus.RenderWidget = true;
                        disqus.WidgetPageUrl = FormatBlogUrl(ThePost.ItemUrl, ThePost.ItemId);
                        //disqus.WidgetPageId = blog.BlogGuid.ToString();
                        pnlFeedback.Visible = false;
                        if (UseCommentSpamBlocker) { this.Controls.Remove(pnlAntiSpam); }
                    }

                    stats.ShowCommentCount = false;

                }

                if ((CommentSystem == "intensedebate") && (IntenseDebateAccountId.Length > 0))
                {
                    if (ThePost.CommentCount == 0)
                    {
                        intenseDebate.AccountId = IntenseDebateAccountId;
                        intenseDebate.PostUrl = FormatBlogUrl(ThePost.ItemUrl, ThePost.ItemId);
                        pnlFeedback.Visible = false;
                        if (UseCommentSpamBlocker) { this.Controls.Remove(pnlAntiSpam); }
                    }
                    stats.ShowCommentCount = false;
                }
            }
            

           
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

            }
            else
            {
                if ((HideDetailsFromUnauthencticated)&&(ThePost.Description.Length > ExcerptLength))
                {
                    pnlDetails.Visible = false;
                    pnlExcerpt.Visible = true;
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

        private string GetExcerpt(Blog blog)
        {
            if ((blog.Excerpt.Length > 0) && (blog.Excerpt != "<p>&#160;</p>"))
            {
                return blog.Excerpt;
            }

            string result = string.Empty;
            if ((blog.Description.Length > ExcerptLength))
            {

                return UIHelper.CreateExcerpt(blog.Description, ExcerptLength, ExcerptSuffix);
                
            }

            return blog.Description;

        }

        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (itemUrl.Length > 0)
                return SiteRoot + itemUrl.Replace("~", string.Empty);

            return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + PageId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

        }


       

        private bool IsValidComment()
        {
            if (parametersAreInvalid) { return false; }

            if (!AllowComments) { return false; }

            if ((CommentSystem != "internal")&&(ThePost.CommentCount == 0)) { return false; }

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
            if (!basePage.UserCanViewPage(ModuleId)) { parametersAreInvalid = true; }

            addThisAccountId = basePage.SiteInfo.AddThisDotComUsername;
            addThisCustomBrand = basePage.SiteInfo.SiteName;


        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((Page.IsPostBack) &&(!pnlFeedback.Visible))
            { 
                WebUtils.SetupRedirect(this, Request.RawUrl); 
                return; 
            }

            base.Render(writer);
        }

    }
}