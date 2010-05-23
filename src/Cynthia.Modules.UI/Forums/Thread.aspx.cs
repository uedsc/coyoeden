/// Author:					Joe Audette
/// Created:				2004-09-19
/// Last Modified:			2010-01-14

using System;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.GroupUI
{
    public partial class GroupThreadView : CBasePage
	{
        protected int PageId = -1;
		protected int moduleId = -1;
        private int threadId = 0;
        protected int ItemId = -1;
        protected bool IsAdmin = false;
        private bool isSiteEditor = false;
        protected int PageNumber = 1;
        private int nextPageNumber = 0;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string EditPropertiesImage = WebConfigSettings.EditPropertiesImage;
        protected string ThreadImage = WebConfigSettings.GroupThreadImage;
        protected string NewThreadImage = WebConfigSettings.NewThreadImage;
        private int userID = -1;
        protected bool UserCanEdit = false;
        
        protected Double TimeOffset = 0;
        private static readonly ILog log = LogManager.GetLogger(typeof(GroupThreadView));
        //private bool allowExternalImages = false;
        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;

        //Gravatar public enum RatingType { G, PG, R, X }
        protected Gravatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        protected bool allowGravatars = false;
        private bool disableAvatars = true;

        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private bool isCommerceReportViewer = false;
        protected bool showUserRevenue = false;
        protected string notificationUrl = string.Empty;
        protected bool isSubscribedToGroup = false;
        private SiteUser currentUser = null;

        


        private void Page_Load(object sender, EventArgs e)
		{
            if ((siteSettings != null) && (CurrentPage != null))
            {
                if ((SiteUtils.SslIsAvailable())
                    && ((siteSettings.UseSslOnAllPages) || (CurrentPage.RequireSsl))
                    )
                {
                    SiteUtils.ForceSsl();
                }
                else
                {
                    SiteUtils.ClearSsl();
                }

            }

            isSiteEditor = SiteUtils.UserIsSiteEditor();
            

			if (WebUser.IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(CurrentPage.EditRoles))
			{
                UserCanEdit = true;
			}

            LoadParams();
            showUserRevenue = (WebConfigSettings.ShowRevenueInGroups && isCommerceReportViewer);

            if (!UserCanViewPage(moduleId))
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                }
                return;
            }

            if (Page.IsPostBack) return;

            PopulateLabels();

			if((Request.IsAuthenticated)&&(currentUser == null))
			{
                currentUser = SiteUtils.GetCurrentSiteUser();
                
			}

            AddConnoicalUrl();
            SetupCss();

            //EditAltText = Resource.EditImageAltText;
            
			PopulateControls();
		}

        
		private void PopulateControls()
		{
			GroupThread thread = new GroupThread(threadId);

            if (thread.ThreadId == -1)
            {
                //thread does not exist, probably just got deleted
                //redirect back to thread list
                WebUtils.SetupRedirect(this, SiteRoot + "/Groups/GroupView.aspx?ItemID="
                + ItemId.ToInvariantString()
                + "&amp;pageid=" + PageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString());

                return;

            }

			Group forum = new Group(thread.GroupId);
           
            if (forum.ModuleId != moduleId)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + thread.Subject);

            litGroupDescription.Text = forum.Description;
            
            MetaDescription = string.Format(CultureInfo.InvariantCulture, GroupResources.GroupThreadMetaDescriptionFormat, SecurityHelper.RemoveMarkup(thread.Subject));

            if ((thread.TotalReplies + 1) == forum.PostsPerPage)
            {
                nextPageNumber = PageNumber + 1;
            }
            else
            {
                nextPageNumber = PageNumber;
            }

            lnkGroup.HRef = SiteRoot + "/Groups/GroupView.aspx?ItemID="
                + forum.ItemId.ToInvariantString()
                + "&amp;pageid=" + PageId.ToInvariantString()
                + "&amp;mid=" + forum.ModuleId.ToInvariantString();

			lnkGroup.InnerHtml = forum.Title;
			lblThreadDescription.Text = Server.HtmlEncode(thread.Subject);
            lblHeading.Text = lblThreadDescription.Text;
			
            string pageUrl = SiteRoot
                + "/Groups/Thread.aspx?pageid="
                + PageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&amp;ItemID=" + ItemId.ToInvariantString()
				+ "&amp;thread=" + threadId.ToInvariantString()
				+ "&amp;pagenumber={0}";

            pgrTop.PageURLFormat = pageUrl;
            pgrTop.ShowFirstLast = true;
            pgrTop.CurrentIndex = PageNumber;
            pgrTop.PageSize = forum.TopicsPerPage;
            pgrTop.PageCount = thread.TotalPages;
            pgrTop.Visible = (pgrTop.PageCount > 1);

            pgrBottom.PageURLFormat = pageUrl;
            pgrBottom.ShowFirstLast = true;
            pgrBottom.CurrentIndex = PageNumber;
            pgrBottom.PageSize = forum.TopicsPerPage;
            pgrBottom.PageCount = thread.TotalPages;
            pgrBottom.Visible = (pgrBottom.PageCount > 1);


            if ((Request.IsAuthenticated) || (forum.AllowAnonymousPosts))
            {
                lnkNewThread.InnerHtml = "<img alt='' src='" + ImageSiteRoot + "/Data/SiteImages/" + NewThreadImage + "'  />&nbsp;"
                    + GroupResources.GroupThreadViewReplyLabel;

                lnkNewThread.HRef = siteSettings.SiteRoot
                    + "/Groups/EditPost.aspx?"
                    + "thread=" + threadId.ToString()
                    + "&amp;forumid=" + forum.ItemId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;pageid=" + PageId.ToString()
                    + "&amp;pagenumber=" + nextPageNumber.ToInvariantString();

                lnkNewThreadBottom.InnerHtml = "<img alt='' src='" + ImageSiteRoot + "/Data/SiteImages/" + NewThreadImage + "'  />&nbsp;"
                    + GroupResources.GroupThreadViewReplyLabel;

                lnkNewThreadBottom.HRef = SiteRoot
                    + "/Groups/EditPost.aspx?"
                    + "thread=" + threadId.ToInvariantString()
                    + "&amp;forumid=" + forum.ItemId.ToString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;pageid=" + PageId.ToInvariantString()
                    + "&amp;pagenumber=" + nextPageNumber.ToInvariantString();

                lnkLogin.Visible = false;
                lnkLoginBottom.Visible = false;
            }

            lnkLogin.NavigateUrl = SiteRoot + "/Secure/Login.aspx";
            lnkLogin.Text = GroupResources.GroupsLoginRequiredLink;

            lnkLoginBottom.NavigateUrl = SiteRoot + "/Secure/Login.aspx";
            lnkLoginBottom.Text = GroupResources.GroupsLoginRequiredLink;



            using (IDataReader reader = thread.GetPosts(PageNumber))
            {
                rptMessages.DataSource = reader;
                rptMessages.DataBind();
            }

            if (
                (rptMessages.Items.Count == 0)
                &&(ItemId > -1)
                )
            {
                // when the last post in a thread is deleted
                // the GroupPostEditPage redirects to this page
                // but it will hit this code and go back to the forum instead of showing 
                // the empty thread
                String redirectUrl = SiteRoot 
					+ "/Groups/GroupView.aspx?" 
                    + "ItemID=" + ItemId.ToInvariantString()
					+ "&pageid=" + PageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString();

                WebUtils.SetupRedirect(this, redirectUrl);
            }
            else
            {
                thread.UpdateThreadViewStats();
            }

		}

		public string GetLink(object url)
		{
			string sUrl = url.ToString();
			string result = string.Empty;
            if (sUrl.Length > 0)
			{
                result = "<a  href='" + sUrl + "'>" + sUrl + "</a>";
			}


			return result;

		}

        public String GetAvatarUrl(string avatar)
        {
            if (allowGravatars) { return string.Empty; }
            if (disableAvatars) { return string.Empty; }

            // if we get here we are using our own crappy avatars
            if ((avatar == null)||(avatar == String.Empty))
            {
                avatar = "blank.gif";
            }
            return "<img  alt='' src='"
                + Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/" + avatar) + "' />";
        }


        public bool GetPermission(object postUser)
        {
            int postUserID = Convert.ToInt32(postUser);
            if ((postUserID == userID) && (postUserID > -1)) return true;
            if (WebUser.IsAdmin) return true;

            return false;
        }


	    private void SetupCss()
        {
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("forumcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "forumcss";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl()
                    + "forummodule.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }
           
        }

        private void AddConnoicalUrl()
        {
            if (Page.Header == null) { return; }

            Literal link = new Literal();
            link.ID = "threadurl";
            link.Text = "\n<link rel='canonical' href='"
                + SiteRoot
                + "/Groups/Thread.aspx?pageid="
                + PageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&amp;ItemID=" + ItemId.ToInvariantString()
                + "&amp;thread=" + threadId.ToInvariantString()
                + "&amp;pagenumber=" + PageNumber.ToInvariantString()
                + "' />";

            Page.Header.Controls.Add(link);

        }

        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            pgrTop.NavigateToPageText = GroupResources.CutePagerNavigateToPageText;
            pgrTop.BackToFirstClause = GroupResources.CutePagerBackToFirstClause;
            pgrTop.GoToLastClause = GroupResources.CutePagerGoToLastClause;
            pgrTop.BackToPageClause = GroupResources.CutePagerBackToPageClause;
            pgrTop.NextToPageClause = GroupResources.CutePagerNextToPageClause;
            pgrTop.PageClause = GroupResources.CutePagerPageClause;
            pgrTop.OfClause = GroupResources.CutePagerOfClause;

            pgrBottom.NavigateToPageText = GroupResources.CutePagerNavigateToPageText;
            pgrBottom.BackToFirstClause = GroupResources.CutePagerBackToFirstClause;
            pgrBottom.GoToLastClause = GroupResources.CutePagerGoToLastClause;
            pgrBottom.BackToPageClause = GroupResources.CutePagerBackToPageClause;
            pgrBottom.NextToPageClause = GroupResources.CutePagerNextToPageClause;
            pgrBottom.PageClause = GroupResources.CutePagerPageClause;
            pgrBottom.OfClause = GroupResources.CutePagerOfClause;

            lnkNotify.Text = GroupResources.SubscribeLink;
            lnkNotify.ImageUrl = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png";
            lnkNotify.NavigateUrl = notificationUrl;

            lnkNotify2.Text = GroupResources.SubscribeLongLink;
            lnkNotify2.ToolTip = GroupResources.SubscribeLongLink;
            lnkNotify2.NavigateUrl = notificationUrl;

        }


        private void LoadParams()
        {
            PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            ItemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            threadId = WebUtils.ParseInt32FromQueryString("thread", -1);
            PageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            TimeOffset = SiteUtils.GetUserTimeOffset();
            IsAdmin = WebUser.IsAdmin;
            isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            //allowedImageUrlRegexPattern = SiteUtils.GetRegexRelativeImageUrlPatern();

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = true;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            notificationUrl = SiteRoot + "/Groups/EditSubscriptions.aspx?mid="
                + moduleId.ToInvariantString()
                + "&pageid=" + PageId.ToInvariantString() + "#forum" + ItemId.ToInvariantString();

            if (Request.IsAuthenticated)
            {
                if (currentUser == null) { currentUser = SiteUtils.GetCurrentSiteUser(); }

                if ((currentUser != null) && (ItemId > -1))
                {
                    userID = currentUser.UserId;
                    isSubscribedToGroup = Group.IsSubscribed(ItemId, currentUser.UserId);
                }

                if (!isSubscribedToGroup) { pnlNotify.Visible = true; }

            }

            

        }

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

	}
}
