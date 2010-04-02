///	Author:				        Dean Brettle
///	Created:			        2005-09-07
///	Last Modified:              2009-03-13

using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.ForumUI
{
	
    public partial class ForumModuleEditSubscriptions : CBasePage
	{
		
        private int moduleId = -1; 

        protected int ModuleId
        {
            get
            {
                return moduleId;
            }
            
        }
        private int pageId = -1; 

        protected int PageId
        {
            get
            {
                return pageId;
            }
            
        }
		private SiteUser siteUser;
		private ArrayList forumIDsToSubscribe = new ArrayList();
		private ArrayList forumIDsToUnsubscribe = new ArrayList();
        private String cacheDependencyKey;
        private Double timeOffset = 0; 

        protected Double TimeOffset
        {
            get
            {
                return timeOffset;
            }
           
        }
		
		private Hashtable ForumIdForControlId
		{
			get 
			{
				if (ViewState["forumIDForControlID"] == null)
				{
					ViewState["forumIDForControlID"] = new Hashtable();
				}
				return (Hashtable)ViewState["forumIDForControlID"];
			 }
		}

		// Create a logger for use in this class
		private static readonly ILog log = LogManager.GetLogger(typeof(ForumModuleEditSubscriptions));


        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadParams();

			siteUser = SiteUtils.GetCurrentSiteUser();
            
			PopulateLabels();
			
			if (!this.IsPostBack)
			{
                PopulateControls();
                //if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                //{
                //    hdnReturnUrl.Value = Request.UrlReferrer.ToString();

                //}
			}
		}

		private void PopulateControls()
		{
            Module module = new Module(moduleId, pageId);
            litHeading.Text = module.ModuleTitle;
			rptForums.ItemDataBound += new RepeaterItemEventHandler(Repeater_ItemDataBound);
            using (IDataReader reader = Forum.GetForums(moduleId, siteUser.UserId))
            {
                rptForums.DataSource = reader;
#if MONO
			rptForums.DataBind();
#else
                this.DataBind();
#endif
            }
			tdSave.Visible = tdSubscribedHead.Visible = Request.IsAuthenticated;
			btnSave.Click += new EventHandler(this.btnSave_Click);
			btnCancel.Click += new EventHandler(this.btnCancel_Click);

            if (rptForums.Items.Count == 0)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
            }
		}
		
		private void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e == null || e.Item == null) 
				return;
			CheckBox checkBox = e.Item.FindControl("chkSubscribed") as CheckBox;
			if (checkBox == null)
			checkBox = e.Item.FindControl("chkSubscribedAlt") as CheckBox;
			if (checkBox == null)
				return;
			int forumId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ItemID"));
			ForumIdForControlId[checkBox.UniqueID] = forumId;
		}
		
		protected void Subscribed_CheckedChanged(object sender, EventArgs e)
		{
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

			CheckBox checkBox = sender as CheckBox;			
			int forumId = Convert.ToInt32(ForumIdForControlId[checkBox.UniqueID]);
			if (checkBox.Checked)
			{
				forumIDsToSubscribe.Add(forumId);
			}
			else
			{
				forumIDsToUnsubscribe.Add(forumId);
			}
		}
		
		private void btnSave_Click(object sender, EventArgs e)
		{
			foreach (int forumId in forumIDsToSubscribe)
			{
				Forum forum = new Forum(forumId);
				forum.Subscribe(siteUser.UserId);
			}
			foreach (int forumId in forumIDsToUnsubscribe)
			{
				Forum forum = new Forum(forumId);
				forum.Unsubscribe(siteUser.UserId);
			}

            CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
            //if (hdnReturnUrl.Value.Length > 0)
            //{
            //    WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
            //    return;
            //}

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

		}
		
		private void btnCancel_Click(object sender, EventArgs e)
		{
            //if (hdnReturnUrl.Value.Length > 0)
            //{
            //    WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
            //    return;
            //}

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.ForumSubscriptions);
            //EditAltText = Resource.EditImageAltText;

            btnSave.Text = ForumResources.ForumEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnSave, ForumResources.ForumEditUpdateButtonAccessKey);

            btnCancel.Text = ForumResources.ForumEditCancelButton;
            SiteUtils.SetButtonAccessKey(btnCancel, ForumResources.ForumEditCancelButtonAccessKey);

            
        }

        private void LoadParams()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            cacheDependencyKey = "Module-" + moduleId.ToString();

        }

	}
}
