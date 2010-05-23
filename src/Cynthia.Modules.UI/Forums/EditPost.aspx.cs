/// Author:				        Joe Audette
/// Created:			        2004-09-18
///	Last Modified:              2010-02-02

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Net;
using Cynthia.Web.Controls;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.GroupUI
{
	
    public partial class GroupPostEdit : CBasePage
	{
        
		private int moduleId = -1;
        private int forumId = -1;
        private int threadId = -1;
        private int postId = -1;
        private int pageId = -1;
        private int pageNumber = 1;
        private SiteUser theUser;
        private Group forum;
        private string virtualRoot;
        private Double timeOffset = 0;
        private bool isSubscribedToGroup = false;
        private bool isSubscribedToThread = false;
        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;
        //Gravatar public enum RatingType { G, PG, R, X }
        protected Gravatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        protected bool allowGravatars = false;
        private bool disableAvatars = true;
        private bool useSpamBlockingForAnonymous = true;
        protected Hashtable moduleSettings;
        private bool isSiteEditor = false;
        private bool includePostBodyInNotification = false;

        public double TimeOffset
        {
            get { return timeOffset; }
        }

        

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);

            SiteUtils.SetupEditor(edMessage);
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            //this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            SecurityHelper.DisableBrowserCache();
            isSiteEditor = SiteUtils.UserIsSiteEditor();

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

            LoadSettings();

            if (forumId > -1)
            {
                forum = new Group(forumId);
                moduleId = forum.ModuleId;
                if (forum.ItemId == -1)
                {
                    Response.Redirect(siteSettings.SiteRoot);
                }
                if (!forum.AllowAnonymousPosts)
                {
                    if (!Request.IsAuthenticated)
                    {
                        SiteUtils.RedirectToLoginPage(this);
                        return;
                    }

                    pnlAntiSpam.Visible = false;
                    pnlEdit.Controls.Remove(pnlAntiSpam);
                }
                else
                {
                    if ((!useSpamBlockingForAnonymous) || (Request.IsAuthenticated))
                    {
                        pnlAntiSpam.Visible = false;
                        pnlEdit.Controls.Remove(pnlAntiSpam);
                    }

                }
            }

            
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            

            if (!CurrentPage.ContainsModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
           
			
            SetupCss();
			PopulateLabels();

			if(!Page.IsPostBack)
			{
				PopulateControls();
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

                }
			}
			

		}

		private void PopulateControls()
		{
            GroupThread thread = null;
		    if(threadId == -1)
			{
				this.btnDelete.Visible = false;
				this.rptMessages.Visible = false;
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + GroupResources.NewTopicLabel);
			}
			else
			{
			    
			    if(postId > -1)
				{
					thread = new GroupThread(threadId, postId);
					if (WebUser.IsAdmin
                        ||(isSiteEditor)
                        || (WebUser.IsInRoles(CurrentPage.EditRoles))
                        ||((this.theUser != null)&&(this.theUser.UserId == thread.PostUserId))
                        )
					{
						this.txtSubject.Text = thread.PostSubject;
						edMessage.Text = thread.PostMessage;
					}
				}
				else
				{  
					thread = new GroupThread(threadId);
                    this.txtSubject.Text
                        = ResourceHelper.GetMessageTemplate(ResourceHelper.GetDefaultCulture(), "GroupPostReplyPrefix.config") 
                        + thread.Subject;
                }

                if ((forum != null) && (thread != null))
                {
                    Title = SiteUtils.FormatPageTitle(siteSettings, forum.Title + " - " + thread.Subject);
                }

                if (forumId == -1)
                {
                    forumId = thread.GroupId;
                }

                using(IDataReader reader = thread.GetPostsReverseSorted())
                {
                    this.rptMessages.DataSource = reader;
                    this.rptMessages.DataBind();
                    
                }
			}

            if (forum != null)
            {
                litGroupPostLabel.Text = forum.Title;
                litGroupDescription.Text = forum.Description;
                
            }

            if (postId == -1)
            {
                string hookupInputScript = "<script type=\"text/javascript\">"
                     + "document.getElementById('" + this.txtSubject.ClientID + "').focus();</script>";

                if (!Page.ClientScript.IsStartupScriptRegistered("finitscript"))
                {
                    this.Page.ClientScript.RegisterStartupScript(
                        typeof(Page),
                        "finitscript", hookupInputScript);
                }
            }

            chkNotifyOnReply.Checked = isSubscribedToThread;

            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            lnkGroup.HRef = SiteRoot + "/Groups/GroupView.aspx?ItemID="
                + forum.ItemId.ToInvariantString()
                + "&amp;pageid=" + pageId.ToInvariantString()
                + "&amp;mid=" + forum.ModuleId.ToInvariantString();

            lnkGroup.InnerHtml = forum.Title;
            if (thread != null) { lblThreadDescription.Text = Server.HtmlEncode(thread.Subject); }
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
			GroupThread thread = new GroupThread(threadId,postId);
            thread.ContentChanged += new ContentChangedEventHandler(thread_ContentChanged);
            
            

			if(thread.DeletePost(postId))
			{
                CurrentPage.UpdateLastModifiedTime();
                //if (Request.IsAuthenticated)
                //{
                //    SiteUser user = SiteUtils.GetCurrentSiteUser();
                //    if(user != null)
                //    SiteUser.DecrementTotalPosts(user.UserId);
                //}
                if (thread.PostUserId > -1)
                {
                    Group.UpdateUserStats(thread.PostUserId);
                }

                SiteUtils.QueueIndexing();
			}

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        

		private void btnCancel_Click(object sender, EventArgs e)
		{
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnUpdate_Click(object sender, EventArgs e)
		{
            if (forum == null) { forum = new Group(forumId); }

            if (!forum.AllowAnonymousPosts)
            {
                captcha.Enabled = false;
                pnlAntiSpam.Visible = false;
                pnlEdit.Controls.Remove(pnlAntiSpam);
            }

			Page.Validate();
			if(!Page.IsValid)
			{
                PopulateControls();
                return;
            }
            else
            {
                if ((useSpamBlockingForAnonymous) && (pnlAntiSpam.Visible))
                {
                    if (!captcha.IsValid)
                    {
                        PopulateControls();
                        return;
                    }
                }

				GroupThread thread;
				bool userIsAllowedToUpdateThisPost = false;
				if(threadId == -1)
				{
					thread = new GroupThread();
					thread.GroupId = forumId;
				}
				else
				{

					if(postId > -1)
					{
						thread = new GroupThread(threadId,postId);
						if (WebUser.IsAdmin
                            || WebUser.IsInRoles(CurrentPage.EditRoles)
                            || (this.theUser.UserId == thread.PostUserId)
                           )
						{
							userIsAllowedToUpdateThisPost = true;
						}
					}
					else
					{
						thread = new GroupThread(threadId);
					}
					forumId = thread.GroupId;

				}

                thread.ContentChanged += new ContentChangedEventHandler(thread_ContentChanged);
				thread.PostSubject = this.txtSubject.Text;
                thread.PostMessage = edMessage.Text;
			
				if(Request.IsAuthenticated)
				{
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if (siteUser != null) 
					thread.PostUserId = siteUser.UserId;
                    if (chkSubscribeToGroup.Checked)
                    {
                        forum.Subscribe(siteUser.UserId);
                    }
                    else
                    {
                        thread.SubscribeUserToThread = this.chkNotifyOnReply.Checked;
                    }
				
				}
				else
				{
					thread.PostUserId = 0; //guest
				}

				string threadViewUrl = SiteRoot + "/Groups/Thread.aspx?thread=" 
					+ thread.ThreadId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    + "&pageid=" + pageId.ToInvariantString()
                    + "&ItemID=" + forumId.ToInvariantString()
                    + "&pagenumber=" + this.pageNumber.ToInvariantString();

				if((thread.PostId == -1)||(userIsAllowedToUpdateThisPost))
				{
					thread.Post();
                    CurrentPage.UpdateLastModifiedTime();

                    threadViewUrl = SiteRoot + "/Groups/Thread.aspx?thread="
                        + thread.ThreadId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&pageid=" + pageId.ToInvariantString()
                        + "&ItemID=" + forumId.ToInvariantString()
                        + "&pagenumber=" + this.pageNumber.ToInvariantString()
                        + "#post" + thread.PostId.ToInvariantString();

					// Send notification to subscribers
                    // this doesn't make sense it only gets thread subscribers not forum subscribers and yet I get the emails
					DataSet dsThreadSubscribers = thread.GetThreadSubscribers();

                    //ConfigurationManager.AppSettings["DefaultEmailFrom"]
                    GroupNotificationInfo notificationInfo = new GroupNotificationInfo();

                    CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();

                    notificationInfo.SubjectTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture, 
                        "GroupNotificationEmailSubject.config");

                    if (includePostBodyInNotification)
                    {
                        notificationInfo.BodyTemplate = Server.HtmlDecode(SecurityHelper.RemoveMarkup(thread.PostMessage)) + "\n\n\n";
                    }

                    notificationInfo.BodyTemplate
                        += ResourceHelper.GetMessageTemplate(defaultCulture, 
                        "GroupNotificationEmail.config");

                    notificationInfo.FromEmail = siteSettings.DefaultEmailFromAddress;
                    notificationInfo.SiteName = siteSettings.SiteName;
                    notificationInfo.ModuleName = new Module(moduleId).ModuleTitle;
                    notificationInfo.GroupName = new Group(forumId).Title;
                    notificationInfo.Subject = thread.PostSubject;
                    notificationInfo.Subscribers = dsThreadSubscribers;
                    notificationInfo.MessageLink = threadViewUrl;
                    notificationInfo.UnsubscribeGroupThreadLink = SiteRoot + "/Groups/UnsubscribeThread.aspx?threadid=" + thread.ThreadId;
                    notificationInfo.UnsubscribeGroupLink = SiteRoot + "/Groups/UnsubscribeGroup.aspx?mid=" + moduleId + "&itemid=" + thread.GroupId;
                    notificationInfo.SmtpSettings = SiteUtils.GetSmtpSettings();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(Notification.SendGroupNotificationEmail), notificationInfo);
                    

                    String cacheDependencyKey = "Module-" + moduleId.ToInvariantString();
                    CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    SiteUtils.QueueIndexing();
                   
				}

				//WebUtils.SetupRedirect(this, threadViewUrl);
                Response.Redirect(threadViewUrl);
			}
		}

        void thread_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["GroupThreadIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        protected int GetUserId(object obj)
        {
            if (obj == null) { return -1; }
            if (obj == DBNull.Value) { return -1; }
            return Convert.ToInt32(obj, CultureInfo.InvariantCulture);

        }

        public String GetAvatarUrl(String avatar)
        {
            if (allowGravatars) { return string.Empty; }
            if (disableAvatars) { return string.Empty; }

            if ((avatar == null) || (avatar == String.Empty))
            {
                avatar = "blank.gif";
            }
            return "<img  alt='' src='"
                + Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/" + avatar) + "' />";
        }

        private void PopulateLabels()
        {
            reqSubject.ErrorMessage = GroupResources.GroupEditSubjectRequiredHelp;
            lblThreadDescription.Text = GroupResources.NewTopicLabel;

            btnUpdate.Text = GroupResources.GroupPostEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, GroupResources.GroupPostEditUpdateButtonAccessKey);

            UIHelper.DisableButtonAfterClick(
                btnUpdate,
                GroupResources.ButtonDisabledPleaseWait,
                Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
                );

            lnkCancel.Text = GroupResources.GroupPostEditCancelButton;
            //btnCancel.Text = GroupResources.GroupPostEditCancelButton;
            //SiteUtils.SetButtonAccessKey(btnCancel, GroupResources.GroupEditCancelButtonAccessKey);

            btnDelete.Text = GroupResources.GroupPostEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, GroupResources.GroupEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, GroupResources.GroupDeletePostWarning);

            if (postId == -1)
            {
                this.btnDelete.Visible = false;
            }

            if (!Request.IsAuthenticated) pnlNotify.Visible = false;
            if (isSubscribedToGroup) pnlNotify.Visible = false;

            if (forumId == -1) pnlEdit.Visible = false;
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


	    private void LoadSettings()
        {
            virtualRoot = WebUtils.GetApplicationRoot();

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            forumId = WebUtils.ParseInt32FromQueryString("forumid", -1);
            threadId = WebUtils.ParseInt32FromQueryString("thread", -1);
            postId = WebUtils.ParseInt32FromQueryString("postid", -1);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            timeOffset = SiteUtils.GetUserTimeOffset();

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

            if (Request.IsAuthenticated)
            {
                theUser = SiteUtils.GetCurrentSiteUser();
                if (forumId > -1)
                {
                    isSubscribedToGroup = Group.IsSubscribed(forumId, theUser.UserId);
                }
                if (threadId > -1)
                {
                    isSubscribedToThread = GroupThread.IsSubscribed(threadId, theUser.UserId);
                }
            }

            if (WebUser.IsAdminOrContentAdmin)
            {
                edMessage.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            }
            else if ((Request.IsAuthenticated)&&(WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles)))
            {
                edMessage.WebEditor.ToolBar = ToolBar.GroupWithImages;
            }
            else
            {
                edMessage.WebEditor.ToolBar = ToolBar.Group;
            }

            edMessage.WebEditor.SetFocusOnStart = true;
            edMessage.WebEditor.Height = Unit.Parse("350px");

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            useSpamBlockingForAnonymous = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GroupEnableAntiSpamSetting", useSpamBlockingForAnonymous);

            includePostBodyInNotification = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "IncludePostBodyInNotificationEmail", includePostBodyInNotification);

           

            if (useSpamBlockingForAnonymous)
            {
                captcha.ProviderName = siteSettings.CaptchaProvider;
                captcha.Captcha.ControlID = "captcha" + moduleId.ToString(CultureInfo.InvariantCulture);
                captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
                captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;
            }

            


        }

	}
}
