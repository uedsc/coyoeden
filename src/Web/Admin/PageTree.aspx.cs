/// Author:					Joe Audette
/// Created:				2004-09-27
/// Last Modified:			2010-01-30
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI
{
   
    public partial class PageTreePage : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PageTreePage));

        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private bool canEditAnything = false;
        private int selectedPage = -1;
        private ArrayList sitePages = new ArrayList();
        private SiteMapDataSource siteMapDataSource;
        private bool userCanAddPages = false;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string EditPropertiesImage = WebConfigSettings.EditPropertiesImage;
        protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            
            PopulatePageArray();
            PopulateLabels();
            PopulateControls();
        }


        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            litWarning.Text = string.Empty;

            PopulatePageList();

            if (selectedPage > -1)
            {
                ListItem listItem = lbPages.Items.FindByValue(selectedPage.ToInvariantString());
                if (listItem != null)
                {
                    lbPages.ClearSelection();
                    listItem.Selected = true;
                }
            }

            if ((!userCanAddPages)&&(!isAdmin))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
            }

        }

        private void PopulatePageList()
        {
            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider = "Csite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            PopulateListControl(lbPages, siteMapNode, string.Empty);

        }

        private void PopulateListControl(
            ListControl listBox, 
            SiteMapNode siteMapNode, 
            string pagePrefix)
        {
            CSiteMapNode CNode = (CSiteMapNode)siteMapNode;

            if (!CNode.IsRootNode)
            {
                if ((isAdmin)
                        || ((isContentAdmin) && (CNode.ViewRoles != "Admins;"))
                        || ((isSiteEditor) && (CNode.ViewRoles != "Admins;"))
                        || (WebUser.IsInRoles(CNode.CreateChildPageRoles))
                        )
                {
                    if (CNode.ParentId > -1) pagePrefix += "-";
                    ListItem listItem = new ListItem();
                    listItem.Text = pagePrefix + Server.HtmlDecode(CNode.Title);
                    listItem.Value = CNode.PageId.ToInvariantString();

                    listBox.Items.Add(listItem);
                    userCanAddPages = true;
                }
            }


            foreach (SiteMapNode childNode in CNode.ChildNodes)
            {
                //recurse to populate children
                PopulateListControl(listBox, childNode, pagePrefix);

            }


        }


        private void UpDown_Click(Object sender, ImageClickEventArgs e)
        {
            string cmd = ((ImageButton) sender).CommandName;

            if (lbPages.SelectedIndex > -1)
            {
                foreach (CSiteMapNode page in sitePages)
                {
                    if (
                        (page.PageId.ToString() == lbPages.SelectedValue)
                        &&((canEditAnything) ||(WebUser.IsInRoles(page.CreateChildPageRoles)))
                        )
                    {
                        selectedPage = page.PageId;

                        PageSettings pageSettings = new PageSettings(siteSettings.SiteId, page.PageId);
                        if (cmd == "down")
                        {
                            pageSettings.MoveDown();
                        }
                        else
                        {
                            pageSettings.MoveUp();
                        }
                    }
                }

                CacheHelper.ResetSiteMapCache();

                if (selectedPage > -1)
                {
                    WebUtils.SetupRedirect(this, SiteRoot + "/Admin/PageTree.aspx?selpage=" + selectedPage.ToInvariantString());
                }
                else
                {
                    WebUtils.SetupRedirect(this, Page.Request.RawUrl);
                }
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }


        private void btnDelete_Click(Object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex > -1)
            {
                foreach (CSiteMapNode page in sitePages)
                {
                    if ((page.PageId.ToString() == lbPages.SelectedValue)&&((canEditAnything)||(WebUser.IsInRoles(page.EditRoles))))
                    {
                        
                        Module.DeletePageModules(page.PageId);
                        PageSettings.DeletePage(page.PageId);
                        FriendlyUrl.DeleteUrlByPageId(page.PageId);
                        PageSettings pageSettings = new PageSettings(siteSettings.SiteId, page.PageId);
                        IndexHelper.ClearPageIndexAsync(pageSettings);
                    }
                }

                CacheHelper.ResetSiteMapCache();

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }


        private void btnEdit_Click(Object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex == -1)
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
            else
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/PageLayout.aspx?pageid=" + lbPages.SelectedValue);
            }
        }


        private void btnSettings_Click(object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex != -1)
            {
                foreach (CSiteMapNode page in sitePages)
                {
                    if (page.PageId.ToString() == lbPages.SelectedValue)
                    {
                        WebUtils.SetupRedirect
                            (this, SiteRoot + "/Admin/PageSettings.aspx?pageid=" + page.PageId.ToInvariantString());
                        break;
                    }
                }
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }

        void btnViewPage_Click(object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex != -1)
            {
                foreach (CSiteMapNode page in sitePages)
                {
                    if (page.PageId.ToString() == lbPages.SelectedValue)
                    {
                        WebUtils.SetupRedirect(this, page.Url);
                        break;
                    }
                }
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }


        private void PopulateLabels()
        {
            if (canEditAnything)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuPageTreeLink);
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageTreeTitle);
                
            }

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            litNonAdminHeading.Text = Server.HtmlEncode(Resource.PageTreeTitle);

            lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
            lnkPageTree.ToolTip = Resource.AdminMenuPageTreeLink;
            lnkPageTree.NavigateUrl = SiteRoot + "/Admin/PageTree.aspx";

            lnkNewPage.InnerText = Resource.PagesAddButton;
            lnkNewPage.HRef = Page.ResolveUrl(SiteRoot + "/Admin/PageSettings.aspx");
            btnUp.AlternateText = Resource.PagesUpButtonAlternateText;
            btnUp.ToolTip = Resource.PagesUpButtonAlternateText;
            btnUp.ImageUrl = ImageSiteRoot + "/Data/SiteImages/up.gif";

            btnDown.AlternateText = Resource.PagesDownButtonAlternateText;
            btnDown.ToolTip = Resource.PagesDownButtonAlternateText;
            btnDown.ImageUrl = ImageSiteRoot + "/Data/SiteImages/dn.gif";

            btnEdit.AlternateText = Resource.PagesEditAlternateText;
            btnEdit.ToolTip = Resource.PagesEditAlternateText;
            btnEdit.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
            
            btnSettings.AlternateText = Resource.PagesEditSettingsAlternateText;
            btnSettings.ToolTip = Resource.PagesEditSettingsAlternateText;
            btnSettings.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditPropertiesImage;

            btnViewPage.AlternateText = Resource.PageViewPageLink;
            btnViewPage.ToolTip = Resource.PageViewPageLink;
            btnViewPage.ImageUrl = ImageSiteRoot + "/Data/SiteImages/search.gif";

            btnDelete.AlternateText = Resource.PagesDeleteAlternateText;
            btnDelete.ToolTip = Resource.PagesDeleteAlternateText;
            btnDelete.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.PageDeleteConfirmMessage);

            
        }

        private void PopulatePageArray()
        {
            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "Csite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
            CSiteMapProvider.PopulateArrayList(sitePages, siteMapNode);

        }

        private void LoadSettings()
        {
            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            canEditAnything = (isAdmin || isContentAdmin || isSiteEditor);

            selectedPage = WebUtils.ParseInt32FromQueryString("selpage", -1);
            

            if ((!isAdmin) && (!isSiteEditor) && (!isContentAdmin))
            {
                lnkNewPage.Visible = false;
                spnAdminLinks.Visible = false;
                litNonAdminHeading.Visible = true;
                
            }
            

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUp.Click += new ImageClickEventHandler(this.UpDown_Click);
            this.btnDown.Click += new ImageClickEventHandler(this.UpDown_Click);
            this.btnEdit.Click += new ImageClickEventHandler(this.btnEdit_Click);
            this.btnSettings.Click += new ImageClickEventHandler(btnSettings_Click);
            this.btnDelete.Click += new ImageClickEventHandler(this.btnDelete_Click);
            btnViewPage.Click += new ImageClickEventHandler(btnViewPage_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        

        #endregion
    }
}
