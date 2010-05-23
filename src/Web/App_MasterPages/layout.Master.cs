
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.UI;
using Cynthia.Web.Framework;
using SystemX.Web;

namespace Cynthia.Web
{
    
    public partial class layout : MasterPage,IHtmlWriterControl
    {
        private int leftModuleCount = 0;
        private int centerModuleCount = 0;
        private int rightModuleCount = 0;
		private int topModuleCnt, bottomModuleCnt;
        private PageSettings currentPage = null;
        private SiteMapDataSource siteMapDataSource = null;
        private SiteMapNode rootNode = null;
		/// <summary>
		/// page css class
		/// </summary>
		public string CssClass { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page is CmsPage) { currentPage = CacheHelper.GetCurrentPage(); }
            
            if (CurSettings == null) return;

            siteMapDataSource = (SiteMapDataSource)this.FindControl("SiteMapData");
            if(siteMapDataSource == null){ return;}

            siteMapDataSource.SiteMapProvider
					= String.Format("Csite{0}", CurSettings.SiteId.ToInvariantString());

            try
            {
            rootNode = siteMapDataSource.Provider.RootNode;
            }
            catch(HttpException)
            {
                return;
            }

            SetupLayout();
        }


        /// <summary>
        /// Count items in each of the 3 columns to determine what css class to assign to center and whether to hide side columns.
        /// This gives us automatic adjustment of column layout from 1 to 3 columns for the main layout.
        /// </summary>
        private void SetupLayout()
        {
            // Count menus if they exist within a content pane and are visible
            CountVisibleMenus();

            CountContentInstances();

            // Set css classes based on count of items in each column panel
            divLeft.Visible = (leftModuleCount > 0);
            divRight.Visible = (rightModuleCount > 0);
			divAlt1.Visible = (topModuleCnt > 0);
			divAlt2.Visible = (bottomModuleCnt > 0);

            if((divLeft.Visible)&&(!divRight.Visible))
            {
                divLeft.CssClass = "leftside left2column";
            }

            if ((divRight.Visible) && (!divLeft.Visible))
            {
                divRight.CssClass = "rightside right2column";
            }

            divCenter.CssClass =
                divLeft.Visible
                    ? (divRight.Visible ? "center-rightandleftmargins cmszone" : "center-leftmargin cmszone")
                    : (divRight.Visible ? "center-rightmargin cmszone" : "center-nomargins cmszone");

            divLeft.CssClass += " cmszone";
            divRight.CssClass += " cmszone";

            // these are optional panels that may exist in some skins
            // but are not part of the automatic column layout scheme
            Control alt = this.FindControl("divAlt1");
            if ((alt != null) && (alt is Panel))
            {
                ((Panel)alt).CssClass += " cmszone";
            }

            alt = this.FindControl("divAlt2");
            if ((alt != null) && (alt is Panel))
            {
                ((Panel)alt).CssClass += " cmszone";
            }

            alt = this.FindControl("divAltContent2");
            if ((alt != null) && (alt is Panel))
            {
                ((Panel)alt).CssClass += " cmszone";
            }

            
        }

        private void CountContentInstances()
        {
            if ((Page is CmsPage) && (currentPage != null))
            {
                foreach (Module module in currentPage.Modules)
                {
					if (!ModuleIsVisible(module)) continue;
					if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "leftpane"))
					{
						leftModuleCount++;
						continue;
					}

					if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "rightpane"))
					{
						rightModuleCount++;
						continue;
					}

					if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "contentpane"))
					{
						centerModuleCount++;
						continue;
					}
					if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent1"))
					{
						topModuleCnt++;
						continue;
					}
					if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent2"))
					{
						bottomModuleCnt++;
						continue;
					}

                }//foreach

                // this is to make room for ModuleWrapper or custom usercontrols if they exsits anywhere in left or right
                foreach (Control c in divRight.Controls)
                {
                    if (c is CUserControl) { rightModuleCount++; }
                }

                foreach (Control c in divLeft.Controls)
                {
                    if (c is CUserControl) { leftModuleCount++; }
                }

            }

            

        }

        private void CountVisibleMenus()
        {
            // Count menus if they exist within a content pane and are visible
            if ((SiteMenu1 != null) && SiteMenu1.Visible)
            {
                // printable view skin doesn't have a menu so it is null there
                if (SiteMenu1.Parent.ID == "divLeft") leftModuleCount++;
                if (SiteMenu1.Parent.ID == "divRight") rightModuleCount++;
            }

            Control c = this.FindControl("PageMenu1");
            if (
                (c != null)
                && (c.Visible)
                )
            {
                PageMenuControl p = (PageMenuControl)c;
                if ((!p.IsSubMenu)||(SiteUtils.TopPageHasChildren(rootNode, p.StartingNodeOffset)))
                {
                    if (c.Parent.ID == "divLeft") leftModuleCount++;
                    if (c.Parent.ID == "divRight") rightModuleCount++;
                }

            }

            c = this.FindControl("PageMenu2");
            if (
                (c != null)
                && (c.Visible)
                )
            {
                PageMenuControl p = (PageMenuControl)c;
                if (SiteUtils.TopPageHasChildren(rootNode, p.StartingNodeOffset))
                {
                    if (c.Parent.ID == "divLeft") leftModuleCount++;
                    if (c.Parent.ID == "divRight") rightModuleCount++;
                }
            }

            c = this.FindControl("PageMenu3");
            if (
                (c != null)
                && (c.Visible)
                )
            {
                PageMenuControl p = (PageMenuControl)c;
                if (SiteUtils.TopPageHasChildren(rootNode, p.StartingNodeOffset))
                {
                    if (c.Parent.ID == "divLeft") leftModuleCount++;
                    if (c.Parent.ID == "divRight") rightModuleCount++;
                }
            }

            c = this.FindControl("pnlMenu");
            if ((c != null) && (c.Parent.ID == "divLeft")) leftModuleCount++;

        }

        private bool ModuleIsVisible(Module module)
        {
            if ((module.HideFromAuthenticated) && (Request.IsAuthenticated)) { return false; }
            if ((module.HideFromUnauthenticated) && (!Request.IsAuthenticated)) { return false; }

            return true;
        }

		/// <summary>
		/// current site settings
		/// </summary>
		protected SiteSettings CurSettings { get; set; }
		/// <summary>
		/// short hand for Page.Request.IsAuthenticated
		/// </summary>
		protected bool IsAuthenticated
		{
			get
			{
				return Page.Request.IsAuthenticated;
			}
        }
        #region base overrides
        protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			CurSettings = CacheHelper.GetCurrentSiteSettings();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.Form.Attributes["class"] = CssClass ?? "main";
        }
        protected override void Render(HtmlTextWriter writer)
        {
            HtmlWriter = writer;
            base.Render(writer);
        }
        #endregion
        #region IHtmlWriterControl Members

        public Page CurPage
        {
            get { return Page; }
        }

        public HtmlTextWriter HtmlWriter
        {
            get;
            private set;
        }

        #endregion
    }
}
