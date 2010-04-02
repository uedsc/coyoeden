using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.UI;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
    public partial class ViewPortMasterPage : MasterPage
    {
        private int leftModuleCount = 0;
        private int centerModuleCount = 0;
        private int rightModuleCount = 0;
        private SiteSettings siteSettings;
        private PageSettings currentPage;
        private SiteMapDataSource siteMapDataSource = null;
        private SiteMapNode rootNode = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            currentPage = CacheHelper.GetCurrentPage();
            if (siteSettings == null) return;

            siteMapDataSource = (SiteMapDataSource)this.FindControl("SiteMapData");
            if (siteMapDataSource == null) { return; }

            siteMapDataSource.SiteMapProvider
                    = "Csite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            try
            {
                rootNode = siteMapDataSource.Provider.RootNode;
            }
            catch (HttpException)
            {
                return;
            }

            SetupLayout();
        }

        private void SetupLayout()
        {
            
                if ((SiteMenu1 != null) && SiteMenu1.Visible)
                {
                    // printable view skin doesn't have a menu so it is null there
                    if (SiteMenu1.Parent.ID == "pnlWest") leftModuleCount++;
                    if (SiteMenu1.Parent.ID == "pnlEast") rightModuleCount++;
                }

                Control c = this.FindControl("PageMenu1");
                if (
                    (c != null)
                    && (c.Visible)
                    )
                {
                    PageMenuControl p = (PageMenuControl)c;
                    if (SiteUtils.TopPageHasChildren(rootNode, p.StartingNodeOffset))
                    {
                        if (c.Parent.ID == "pnlWest") leftModuleCount++;
                        if (c.Parent.ID == "pnlEast") rightModuleCount++;
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
                if ((c != null) && (c.Parent.ID == "pnlWest")) leftModuleCount++;

           

            if ((Page is CmsPage)&&(currentPage != null))
            {
                foreach (Module module in currentPage.Modules)
                {
                    if (ModuleIsVisible(module))
                    {
                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "leftpane"))
                        {
                            leftModuleCount++;
                        }

                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "rightpane"))
                        {
                            rightModuleCount++;
                        }

                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "contentpane"))
                        {
                            centerModuleCount++;
                        }
                    }
                }
            }

            if ((Request.RawUrl.Contains("MyPage.aspx"))
                        || (Request.RawUrl.Contains("ChooseContent.aspx"))
                        || (Request.RawUrl.Contains("SiteMail"))
                        )
            {
                //this.divLeft.CssClass = "left-mypage";
                //this.divCenter.CssClass = "center-mypage";
                //this.divRight.CssClass = "right-mypage";
                this.pnlWest.Visible = false;
                this.pnlEast.Visible = false;

            }
            else
            {
                this.pnlWest.Visible = (leftModuleCount > 0);
                this.pnlEast.Visible = (rightModuleCount > 0);

                //if ((pnlWest.Visible) && (!pnlEast.Visible))
                //{
                //    divLeft.CssClass = "leftside left2column";
                //}

                //if ((pnlEast.Visible) && (!pnlWest.Visible))
                //{
                //    divRight.CssClass = "rightside right2column";
                //}

                //this.divCenter.CssClass =
                //    this.pnlWest.Visible
                //        ? (this.pnlEast.Visible ? "center-rightandleftmargins" : "center-leftmargin")
                //        : (this.pnlEast.Visible ? "center-rightmargin" : "center-nomargins");
            }

        }
        

        private bool ModuleIsVisible(Module module)
        {
            if ((module.HideFromAuthenticated) && (Request.IsAuthenticated)) { return false; }
            if ((module.HideFromUnauthenticated) && (!Request.IsAuthenticated)) { return false; }

            return true;
        }

        //private bool IsContentSystemPage()
        //{
        //    if (Request.RawUrl.Contains("SiteMail")) return false;
        //    if (Request.CurrentExecutionFilePath.Contains("Default.aspx")) return true;
        //    if (Request.CurrentExecutionFilePath.Contains("default.aspx")) return true;
        //    if (Request.Url.ToString() == siteSettings.SiteRoot) return true;
        //    return false;
        //}

    }
}
