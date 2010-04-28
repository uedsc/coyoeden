
using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{

    public partial class CmsPage : CBasePage
	{
		private static readonly ILog log
            = LogManager.GetLogger(typeof(CmsPage));

		private HyperLink lnkEditPageSettings = new HyperLink();
        private HyperLink lnkEditPageContent = new HyperLink();
        
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private bool allowPageOverride = true;
        private int countOfIWorkflow = 0;
        private bool userCanEditPage = false;
        
        

        override protected void OnPreInit(EventArgs e)
        {

            SetMasterInBasePage = false;

            base.OnPreInit(e);

            try
            {
                SetupMasterPage();

            }
            catch (HttpException ex)
            {
				log.Error(String.Format("{0} - Error setting master page. Will try settingto default skin", SiteUtils.GetIP4Address()), ex);
                SetupFailsafeMasterPage();
            }

            StyleSheetCombiner styleCombiner = (StyleSheetCombiner)Master.FindControl("StyleSheetCombiner");
            if (styleCombiner != null) { styleCombiner.AllowPageOverride = allowPageOverride; }

            this.Load += new EventHandler(Page_Load);

            

        }

        void Page_Load(object sender, EventArgs e)
        { 
            LoadPage();

            
        }


        private void LoadPage()
        {
            EnsurePageAndSite();
            if (CurrentPage == null) return;
            LoadSettings();
            EnforceSecuritySettings();
            bool redirected = RedirectIfNeeded();
            if (redirected) { return; }
            SetupAdminLinks();
            if (CurrentPage.PageId == -1) return;

            if ((CurrentPage.ShowChildPageMenu)
                    || (CurrentPage.ShowBreadcrumbs)
                    || (CurrentPage.ShowChildPageBreadcrumbs)
                    )
            {
                // this is needed to override some hide logic in
                // layout.Master.cs
                this.MPContent.Visible = true;
                this.MPContent.Parent.Visible = true;
            }

            
            if (CurrentPage.PageTitle.Length > 0)
            {
                Title = Server.HtmlEncode(CurrentPage.PageTitle);
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName);
            }

            if (CurrentPage.PageMetaKeyWords.Length > 0)
            {
                MetaKeywordCsv = CurrentPage.PageMetaKeyWords;
            }
            

            if (CurrentPage.PageMetaDescription.Length > 0)
            {
                MetaDescription = CurrentPage.PageMetaDescription;
            }
            

            if (CurrentPage.CompiledMeta.Length > 0)
            {
                AdditionalMetaMarkup = CurrentPage.CompiledMeta;
            }

            if ((Page.Header != null)&& (CurrentPage.UseUrl) && (CurrentPage.Url.Length > 0))
            {
                string urlToUse = SiteRoot + CurrentPage.Url.Replace("~/", "/");
                if (CurrentPage.CanonicalOverride.Length > 0)
                {
                    urlToUse = CurrentPage.CanonicalOverride;
                }
                using (Literal link = new Literal { ID = "pageurl", Text = String.Format("\n<link rel='canonical' href='{0}' />", urlToUse) })
                {
                    Page.Header.Controls.Add(link);
                }
                
            }

            if (CurrentPage.Modules.Count == 0) { return; }

            foreach (Module module in CurrentPage.Modules)
            {
                if (!ModuleIsVisible(module)) { continue; }
                if(!WebUser.IsInRoles(module.ViewRoles)) { continue; }

                Control parent = this.MPContent;

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "leftpane"))
                {
                    parent = this.MPLeftPane;

                }

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "rightpane"))
                {
                    parent = this.MPRightPane;

                }

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent1"))
                {
                    if (AltPane1 != null)
                    {
                        parent = this.AltPane1;
                    }
                    else
                    {
                        log.Error("Content is assigned to altcontent1 placeholder but it does not exist in layout.master so using center.");
                        parent = this.MPContent;
                    }

                }

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent2"))
                {
                    if (AltPane2 != null)
                    {
                        parent = this.AltPane2;
                    }
                    else
                    {
                        log.Error("Content is assigned to altcontent2 placeholder but it does not exist in layout.master so using center.");
                        parent = this.MPContent;
                    }

                }

                // 2008-10-04 since more an more of our features use postback via ajax
                // its not feasible to use output caching as this breaks postback,
                // so I changed the default the use of WebConfigSettings.DisableContentCache to true
                // this also reduces the memory consumption footprint

                if ((module.CacheTime == 0) || (WebConfigSettings.DisableContentCache))
                {
                    //2008-10-16 in ulu's blog post:http://dotfresh.blogspot.com/2008/10/in-search-of-developer-friendly-cms.html
                    // he complains about having to inherit from a base class (SiteModuleControl) to make a plugin.
                    // he wishes he could just use a UserControl
                    // While SiteModuleControl "is a" UserControl that provides additional functionality
                    // Its easy enough to support using
                    // a plain UserControl so I'm making the needed change here now.
                    // The drawback of a plain UserControl is that is not reusable in the same way as SiteModuleControl.
                    // If you use a plain UserControl, its going to be exactly the same on any page you use it on.
                    // It has no instance specific properties.
                    // SiteModuleControl gives you instance specific ids and internal tracking of which instance this is so 
                    // that you can have different instances.
                    // For example the Blog is instance specific, if you put a blog on one page and then put a blog on another page
                    // those are 2 different blogs with different content.
                    // However, if you don't need a re-usable feature with instance specific properties
                    // you are now free to use a plain old UserControl and I think freedom is a good thing
                    // so this was valuable feedback from ulu.
                    // Those who do need instance specific features should read my developer Guidelines for building one:
                    //http://www.vivasky.com/addingfeatures.aspx

                    Control c = Page.LoadControl("~/" + module.ControlSource);
                    if (c == null) { continue; }

                    if (c is SiteModuleControl)
                    {
                        SiteModuleControl siteModule = (SiteModuleControl)c;
                        siteModule.SiteId = siteSettings.SiteId;
                        siteModule.ModuleConfiguration = module;

                        if (siteModule is IWorkflow)
                        {
                            countOfIWorkflow += 1;
                        }
                    }

                    
                   
                    parent.Controls.Add(c);
                    
                }
                else
                {
                    using (CachedSiteModuleControl siteModule = new CachedSiteModuleControl { SiteId = siteSettings.SiteId, ModuleConfiguration = module })
                    {
                        parent.Controls.Add(siteModule);
                    }
                }

                parent.Visible = true;
                parent.Parent.Visible = true;

               
            } //end foreach

            if ((!WebConfigSettings.DisableExternalCommentSystems) && (siteSettings != null) && (CurrentPage != null) && (CurrentPage.EnableComments))
            {
                switch (siteSettings.CommentProvider)
                {
                    case "disqus":

                        if (siteSettings.DisqusSiteShortName.Length > 0)
                        {
                            using (DisqusWidget disqus = new DisqusWidget())
                            {
                                disqus.SiteShortName = siteSettings.DisqusSiteShortName;
                                disqus.WidgetPageUrl = SiteUtils.GetCurrentPageUrl();
                                disqus.RenderWidget = true;
                                MPContent.Controls.Add(disqus);
                            }
                        }

                        break;

                    case "intensedebate":

                        if (siteSettings.IntenseDebateAccountId.Length > 0)
                        {
                            using (IntenseDebateDiscussion d = new IntenseDebateDiscussion())
                            {
                                d.AccountId = siteSettings.IntenseDebateAccountId;
                                d.PostUrl = SiteUtils.GetCurrentPageUrl();
                                MPContent.Controls.Add(d);
                            }

                        }

                        break;

                }
            }

            if (WebConfigSettings.HidePageViewModeIfNoWorkflowItems && (countOfIWorkflow == 0))
            {
                HideViewSelector();
            }

            // (to show the last mnodified time of a page we may have this control in layout.master, but I set it invisible by default
            // because we only want to show it on content pages not edit pages
            // since Default.aspx.cs is the handler for content pages, we look for it here and make it visible.
            Control pageLastMod = Master.FindControl("pageLastMod");
            if (pageLastMod != null) { pageLastMod.Visible = true; }
            
        }

        

        private void LoadSettings()
        {
            isAdmin = WebUser.IsAdminOrContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            userCanEditPage = WebUser.IsInRoles(CurrentPage.EditRoles);

            if (WebConfigSettings.DisablePageViewStateByDefault)
            {
                this.EnableViewState = false;
            }
        }


        private void EnsurePageAndSite()
        {
            if (CurrentPage == null)
            {
                int siteCount = SiteSettings.SiteCount();
                
                
                if (siteCount == 0)
                {
                    // no site data so redirect to setup
                    HttpContext.Current.Response.Redirect(WebUtils.GetSiteRoot() + "/Setup/Default.aspx");
                }

                
            }


        }

        private bool RedirectIfNeeded()
        {
            if (
                (!isAdmin)
                && (!isSiteEditor)
                && (!WebUser.IsInRoles(CurrentPage.AuthorizedRoles))
                )
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this, SiteUtils.GetCurrentPageUrl());
                    return true;

                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return true;
                }
            }

            return false;

        }

        private void EnforceSecuritySettings()
        {
            if (CurrentPage.PageId == -1) { return; }

            if (!CurrentPage.AllowBrowserCache)
            {
                SecurityHelper.DisableBrowserCache();
            }

            bool useSsl = false;
            if (SiteUtils.SslIsAvailable())
            {
                if (WebConfigSettings.ForceSslOnAllPages || siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl)
                {
                    useSsl = true;
                }
            }

            if (useSsl)
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }

            

        }

        private void SetupMasterPage()
        {
            // allow content system pages to use page settings
            bool allowPageOverride = true;
            if (
                (siteSettings != null)
                && (siteSettings.SiteGuid != Guid.Empty)
                )
            {
                SiteUtils.SetMasterPage(this, siteSettings, allowPageOverride);
                if (
                    (siteSettings.AllowPageSkins)
                    &&(CurrentPage != null)
                    &&(CurrentPage.Skin.Length > 0)
                    )
                {
                    this.Theme = "pageskin";
                }
            }

            MPLeftPane = (ContentPlaceHolder)Master.FindControl("leftContent");
            MPContent = (ContentPlaceHolder)Master.FindControl("mainContent");
            MPRightPane = (ContentPlaceHolder)Master.FindControl("rightContent");
            MPPageEdit = (ContentPlaceHolder)Master.FindControl("pageEditContent");

            AltPane1 = (ContentPlaceHolder)Master.FindControl("altContent1");
            AltPane2 = (ContentPlaceHolder)Master.FindControl("altContent2");

            StyleSheetControl = (StyleSheet)Master.FindControl("StyleSheet");

        }

        

        private void SetupAdminLinks()
        {

            // 2010-01-04 made it possible to add these links directly in layout.master so they can be arranged and styled easier
            if (Page.Master.FindControl("lnkPageContent") == null)
            {
                this.MPPageEdit.Controls.Add(new PageEditFeaturesLink());
            }

            if (Page.Master.FindControl("lnkPageSettings") == null)
            {
                this.MPPageEdit.Controls.Add(new PageEditSettingsLink());
            }

            SetupWorkflowControls();
            
        }

    }
}
