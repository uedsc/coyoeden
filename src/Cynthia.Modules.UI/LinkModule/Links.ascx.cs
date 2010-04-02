/// Author:				Joe Audette
/// Created:			2004-12-24
///	Last Modified:		2009-05-31
///	
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.LinksUI
{
	public partial  class LinksModule : SiteModuleControl
    {
        #region Properties

        protected string LinkImage = string.Empty;
        protected string DeleteImage = string.Empty;
        protected bool UseDescription = false;
        protected bool ShowDeleteIcon = false;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        private bool addWebSnaprCssToLinks = false;
        private string linkCssClass = "Clink";
        private string webSnaprKey = string.Empty;
        private bool descriptionOnly = false;
        private bool enablePager = false;
        private int pageNumber = 1;
        private int pageSize = 50;
        private int totalPages = 1;
        private string extraCssClass = string.Empty;

        
        private String cacheDependencyKey;

        #endregion

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.rptLinks.ItemCommand += new RepeaterCommandEventHandler(rptLinks_ItemCommand);
            this.rptLinks.ItemDataBound += new RepeaterItemEventHandler(rptLinks_ItemDataBound);
            pgr.Command += new CommandEventHandler(pgr_Command);
            Page.EnableViewState = true;
        }

        

        #endregion


        protected void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();
            SetupScripts();

            if (Page.IsPostBack) { return; }
            PopulateControls();
		}

        private void PopulateControls()
        {
            if (enablePager)
            {
                using (IDataReader reader = Link.GetPage(ModuleId, pageNumber, pageSize, out totalPages))
                {
                    pgr.ShowFirstLast = true;
                    pgr.PageSize = pageSize;
                    pgr.PageCount = totalPages;

                    if (descriptionOnly)
                    {
                        rptDescription.Visible = true;
                        rptLinks.Visible = false;
                        rptDescription.DataSource = reader;
                        rptDescription.DataBind();

                    }
                    else
                    {
                        rptDescription.Visible = false;
                        rptLinks.Visible = true;
                        rptLinks.DataSource = reader;
                        rptLinks.DataBind();

                    }

                    pgr.Visible = (totalPages > 1);

                }


            }
            else
            {

                using (IDataReader reader = Link.GetLinks(ModuleId))
                {
                    if (descriptionOnly)
                    {
                        rptDescription.Visible = true;
                        rptLinks.Visible = false;
                        rptDescription.DataSource = reader;
                        rptDescription.DataBind();

                    }
                    else
                    {
                        rptDescription.Visible = false;
                        rptLinks.Visible = true;
                        rptLinks.DataSource = reader;
                        rptLinks.DataBind();

                    }
                    
                }
            }

        }

        void pgr_Command(object sender, CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgr.CurrentIndex = pageNumber;
            PopulateControls();
            updPnl.Update();
        }

        void rptLinks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if ((e.CommandSource is Button) && (e.CommandName.Equals("delete")))
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                Link link = new Link(itemId);
                link.ContentChanged += new ContentChangedEventHandler(link_ContentChanged);
                link.Delete();
                
                CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);

                WebUtils.SetupRedirect(this, Page.Request.RawUrl);

            }
        }

        void link_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["LinksIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }


        void rptLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnDelete = e.Item.FindControl("btnDelete") as Button;
            UIHelper.AddConfirmationDialog(btnDelete, LinkResources.LinksDeleteLinkWarning);
        }


        protected string CreateLink(
            string title,
            string url,
            string description,
            string target)
        {
            if (string.IsNullOrEmpty(url)) { return string.Empty; }

            String link = "<a class='" + linkCssClass + "' href='" + GetLinkUrl(url) + "' "
                + GetOnClick(target)
                + GetTitle(title, description)
                + ">"
                + title
                + "</a>";

            return link;
        }

        private string GetTitle(String title, String description)
        {
            if ((title != null) && (title.Length > 0))
            {
                return " title='" + title + "' ";
            }

            return String.Empty;
        }

        private string GetOnClick(String target)
        {
            if(
                (target != null)
                &&(target == "_blank")
              )
            {
                return " onclick=\"window.open(this.href,'_blank');return false;\" ";
            }

            return string.Empty;

        }


        protected String GetLinkUrl(String dbFormatUrl)
        {
            if (dbFormatUrl.StartsWith("~/"))
            {
                return Page.ResolveUrl(SiteRoot + dbFormatUrl.Replace("~/", "/"));
            }

            return dbFormatUrl;

        }

        private void SetupScripts()
        {
            if (!addWebSnaprCssToLinks) { return; }
            if (webSnaprKey.Length == 0) { return; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page),
            "websnapr", "\n<script src=\"http://bubble.websnapr.com/"
            + webSnaprKey + "/swh/" + "\" type=\"text/javascript\" ></script>");

        }

        private void LoadSettings()
        {
            pnlContainer.ModuleId = ModuleId;
            cacheDependencyKey = "Module-" + ModuleId.ToString();
            Title1.EditUrl = SiteRoot + "/LinkModule/EditLink.aspx";
            Title1.EditText = LinkResources.EditLinksAddLinkLabel;
            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (IsEditable)
            {
                LinkImage =  ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
                DeleteImage = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;

                ShowDeleteIcon = WebUtils.ParseBoolFromHashtable(
                    Settings, "LinksShowDeleteIconSetting", false);

            }

            UseDescription = WebUtils.ParseBoolFromHashtable(
                    Settings, "LinksShowDescriptionSetting", UseDescription);

            descriptionOnly = WebUtils.ParseBoolFromHashtable(
                    Settings, "LinksShowOnlyDescriptionSetting", descriptionOnly);

            if (descriptionOnly) { UseDescription = true; }

            enablePager  = WebUtils.ParseBoolFromHashtable(
                    Settings, "LinksEnablePagingSetting", enablePager);

            pageSize = WebUtils.ParseInt32FromHashtable(
                    Settings, "LinksPageSizeSetting", pageSize);

            

            addWebSnaprCssToLinks = WebUtils.ParseBoolFromHashtable(
                    Settings, "LinksAddWebSnaprCss", addWebSnaprCssToLinks);

            if (addWebSnaprCssToLinks) { linkCssClass += " websnapr"; }

            if (Settings.Contains("LinksWebSnaprKeySetting"))
            {
                webSnaprKey = Settings["LinksWebSnaprKeySetting"].ToString();
            }

            if (Settings.Contains("LinksExtraCssClassSetting"))
            {
                extraCssClass = Settings["LinksExtraCssClassSetting"].ToString().Trim();
                if (extraCssClass.Length > 0) { pnlWrapper.CssClass += " " + extraCssClass; }
            }

            
            

            

        }

    }
}
