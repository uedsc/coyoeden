///	Author:					Joe Audette
///	Created:				2004-09-11
/// Last Modified:			2009-03-11

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Web.Framework;
using Cynthia.Web.UI;
using Resources;

namespace Cynthia.Web.GroupUI
{
	public partial class GroupModule : SiteModuleControl
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(GroupModule));

        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string RssImageFile = WebConfigSettings.RSSImageFileName;
		private SiteUser siteUser;
        protected bool EnableRSSAtModuleLevel = false;
        protected bool EnableRSSAtGroupLevel = false;
        protected bool EnableRSSAtThreadLevel = false;
        protected bool showSubscriberCount = false;
        protected Double TimeOffset = 0;
        private int userId = -1;
        protected string notificationUrl = string.Empty;
        protected string notificationLink = string.Empty;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }
        
        
        protected void Page_Load(object sender, EventArgs e)
		{
			
            
			Title1.EditUrl = SiteRoot + "/Groups/EditGroup.aspx";
            Title1.EditText = GroupResources.EditImageAltText;

            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }
            LoadSettings();
            SetupCss();
			
			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
            using (IDataReader reader = Group.GetGroups(ModuleId, userId))
            {
                rptGroups.DataSource = reader;
#if MONO
			rptGroups.DataBind();
#else
                this.DataBind();
#endif
            }

            pnlGroupList.Visible = (rptGroups.Items.Count > 0);
         
		}


        protected string FormatSubscriberCount(int subscriberCount)
        {
            return string.Format(CultureInfo.InvariantCulture, GroupResources.SubscriberCountFormat, subscriberCount);

        }
        

		private void PopulateLabels()
		{
            Title1.EditText = GroupResources.GroupEditLabel;
            //EditAltText = Resource.EditImageAltText;
            
            divEditSubscriptions.Visible = tdSubscribedHead.Visible = Page.Request.IsAuthenticated;
            notificationUrl = SiteRoot + "/Groups/EditSubscriptions.aspx?mid="
                + ModuleId.ToString(CultureInfo.InvariantCulture)
                + "&pageid=" + PageId.ToString(CultureInfo.InvariantCulture);

            editSubscriptionsLink.NavigateUrl = notificationUrl;

            notificationLink = "<a title='" + GroupResources.GroupModuleEditSubscriptionsLabel 
                + "' href='" + SiteRoot + "/Groups/EditSubscriptions.aspx?mid="
                + ModuleId.ToString(CultureInfo.InvariantCulture)
                + "&amp;pageid=" + PageId.ToString(CultureInfo.InvariantCulture)
                + "'><img src='" + ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png' /></a>";

            lnkModuleRSS.NavigateUrl = SiteRoot
                + "/Groups/RSS.aspx?mid=" + this.ModuleId.ToString()
                + "&pageid=" + CurPageSettings.PageId.ToString();

            lnkModuleRSS.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + RssImageFile;

            lnkModuleRSS.Text = "RSS";

            lnkModuleRSS.Visible = EnableRSSAtModuleLevel;

            editSubscriptionsLink.Text = GroupResources.GroupModuleEditSubscriptionsLabel;

            

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
            EnableRSSAtModuleLevel = WebUtils.ParseBoolFromHashtable(
                Settings, "GroupEnableRSSAtModuleLevel", false);

            EnableRSSAtGroupLevel = WebUtils.ParseBoolFromHashtable(
                Settings, "GroupEnableRSSAtGroupLevel", false);

            EnableRSSAtThreadLevel = WebUtils.ParseBoolFromHashtable(
                Settings, "GroupEnableRSSAtThreadLevel", false);

            showSubscriberCount = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowSubscriberCount", showSubscriberCount);

            //siteUser = new SiteUser(siteSettings, Context.User.Identity.Name);
            siteUser = SiteUtils.GetCurrentSiteUser();

            if (siteUser != null) userId = siteUser.UserId;

            
            TimeOffset = SiteUtils.GetUserTimeOffset();
        }

    }
}
