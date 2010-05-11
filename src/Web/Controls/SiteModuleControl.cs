
using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
#if !MONO
using System.Web.UI.WebControls.WebParts;
#endif
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using System.Globalization;

namespace Cynthia.Web 
{
	
	#if !MONO
    public abstract class SiteModuleControl : CUserControl, IWebPart
	#else
	public abstract class SiteModuleControl : UserControl
	#endif
	{
		#region member variables
		private Module moduleConfiguration;
        private bool isEditable = false;
        private bool forbidModuleSettings = false;
        private int siteID = -1;
        private Hashtable settings;
        private bool renderInWebPartMode = false;
        private string imageSiteRoot = string.Empty;
        private bool isSiteEditor = false;
        private bool enableWorkflow = false;
        private bool IsOnInitExecuted = false;


        protected ScriptManager ScriptController;

		#endregion


		protected override void OnInit(EventArgs e)
        {
            // Alexander Yushchenko: workaround to make old custom modules work
            // Before 03.19.2007 this method was "new" and called from descendant classes
            // To avoid multiple self-calls a boolean flag is used
            if (IsOnInitExecuted) return;
            IsOnInitExecuted = true;

            base.OnInit(e);

            if (HttpContext.Current == null) { return; }

			LoadSettings();

            ScriptController = (ScriptManager)Page.Master.FindControl("ScriptManager1");

            if (SiteSettings != null)
            {
                this.siteID = SiteSettings.SiteId;
                if (!WebUser.IsAdminOrContentAdmin)
                {
                    forbidModuleSettings = WebUser.IsInRoles(SiteSettings.RolesNotAllowedToEditModuleSettings);
                }
            }

            if (Page.Request.IsAuthenticated)
            {
                isSiteEditor = SiteUtils.UserIsSiteEditor();

                if (WebUser.IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(CurPageSettings.EditRoles)
                    || ((moduleConfiguration != null)
                           && (WebUser.IsInRoles(moduleConfiguration.AuthorizedEditRoles))
                       )
                   )
                {
                    isEditable = true;
                }

                if (WebConfigSettings.EnableContentWorkflow && SiteSettings.EnableContentWorkflow && (this is IWorkflow))
                {
                    enableWorkflow = true;
                    if (!isEditable) 
                    {
                        if ((WebUser.IsInRoles(CurPageSettings.DraftEditOnlyRoles)) || (WebUser.IsInRoles(moduleConfiguration.DraftEditRoles)))
                        {
                            isEditable = true;
                            
                        }

                    }
                }
                
                if (!isEditable && (moduleConfiguration != null) && (moduleConfiguration.EditUserId > 0))
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if (
                        (siteUser != null)
                        &&(moduleConfiguration.EditUserId == siteUser.UserId)
                        )
                    {
                        isEditable = true;
                    }
                }
            }

            if (moduleConfiguration != null)
            {
                this.m_title = moduleConfiguration.ModuleTitle;
                this.m_description = moduleConfiguration.FeatureName;
				CssClass = moduleConfiguration.FeatureName.Replace(" ", "_").Trim();
            }
        }

		private void LoadSettings()
		{
			//date format string
			if (Settings.Contains("FeedDateFormatSetting"))
			{
				DateFormat = Settings["FeedDateFormatSetting"].ToString().Trim();
			}
			else if (settings.Contains("BlogDateTimeFormat"))
			{
				DateFormat = Settings["BlogDateTimeFormat"].ToString().Trim();
			}
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

#if !MONO
        [Personalizable(PersonalizationScope.Shared)] 
#endif
        public bool RenderInWebPartMode
        {
            get { return renderInWebPartMode; }
            set { renderInWebPartMode = value; }
        }

#if !MONO
        [Personalizable(PersonalizationScope.Shared)]
#endif
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ModuleId
        {
            get { return moduleConfiguration == null ? 0 : moduleConfiguration.ModuleId; }
            set
            {
                if (moduleConfiguration == null) moduleConfiguration = new Module(value);
                moduleConfiguration.ModuleId = value;
            }
        }

        public Guid ModuleGuid
        {
            get { return moduleConfiguration == null ? Guid.Empty : moduleConfiguration.ModuleGuid; }
            set
            {
                if (moduleConfiguration == null) moduleConfiguration = new Module(value);
                moduleConfiguration.ModuleGuid = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageId
        {
            get { return moduleConfiguration == null ? 0 : moduleConfiguration.PageId; }
        }


		public string SiteRoot
		{
			get 
            {
                if ((SiteSettings != null)
                    &&(SiteSettings.SiteFolderName.Length > 0))
                {
                    return SiteSettings.SiteRoot;
                }
                return WebUtils.GetSiteRoot(); 
            }
		}

        public string ImageSiteRoot
        {
            get
            {
                if (imageSiteRoot.Length == 0)
                {
                    imageSiteRoot = WebUtils.GetSiteRoot();
                }
                return imageSiteRoot;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEditable
        {
            get { return isEditable; }
        }

        public bool ForbidModuleSettings
        {
            get { return forbidModuleSettings; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableWorkflow
        {
            get { return enableWorkflow; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Module ModuleConfiguration
        {
            get { return moduleConfiguration; }
            set { moduleConfiguration = value; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Hashtable Settings
        {
            get
            {
                if (settings == null) settings = ModuleSettings.GetModuleSettings(ModuleId);
                return settings;
            }
        }


         #region IWebPart Members

         private string m_title = String.Empty;
         private string m_subTitle = String.Empty;
         private string m_description = String.Empty;
         private string m_titleUrl = String.Empty;
         private string m_titleIconImageUrl = String.Empty;
         private string m_catalogIconImageUrl = String.Empty;


         // Title
         public string Title
         {
             get
             {
                 return m_title;
             }
             set
             {
                 m_title = value;
             }
         }
         //  Subtitle
         public string Subtitle
         {
             get
             {
                 return m_subTitle;
             }
             set
             {
                 m_subTitle = value;
             }
         }
         //  Description
         public string Description
         {
             get
             {
                 return m_description;
             }
             set
             {
                 m_description = value;
             }
         }
         //  TitleUrl
         public string TitleUrl
         {
             get
             {
                 return m_titleUrl;
             }
             set
             {
                 m_titleUrl = value;
             }
         }
         //  TitleIconImageUrl
         public string TitleIconImageUrl
         {
             get
             {
                 return m_titleIconImageUrl;
             }
             set
             {
                 m_titleIconImageUrl = value;
             }
         }
         //  CatalogIconImageUrl
         public string CatalogIconImageUrl
         {
             get
             {
                 return m_catalogIconImageUrl;
             }
             set
             {
                 m_catalogIconImageUrl = value;
             }
         }


         #endregion

    }
    
    
}
