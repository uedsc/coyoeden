/// Author:					Joe Audette
/// Created:				2005-06-26
/// Last Modified:		    2010-02-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI
{
	public partial class SearchInput : System.Web.UI.UserControl
	{
		protected Literal searchLink = new Literal();

        // these separator properties are deprecated
        // it is recommended not to use these properties
        // but instead to use Cynthia.Web.Controls.SeparatorControl
        private bool useLeftSeparator = false;
        /// <summary>
        /// deprecated
        /// </summary>
        public bool UseLeftSeparator
        {
            get { return useLeftSeparator; }
            set { useLeftSeparator = value; }
        }

		private bool linkOnly = true;
		public bool LinkOnly
		{	
			get {return linkOnly;}
			set {linkOnly = value;}
		}

        private bool useHeading = true;
        public bool UseHeading
        {
            get { return useHeading; }
            set { useHeading = value; }
        }

        private bool useWatermark = true;
        public bool UseWatermark
        {
            get { return useWatermark; }
            set { useWatermark = value; }
        }

        private string imageUrl = string.Empty;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        private string buttonCssClass = string.Empty;
        public string ButtonCssClass
        {
            get { return buttonCssClass; }
            set { buttonCssClass = value; }
        }

        private string textBoxCssClass = string.Empty;
        public string TextBoxCssClass
        {
            get { return textBoxCssClass; }
            set { textBoxCssClass = value; }
        }

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = "topnavitem";
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }

        private SiteSettings siteSettings = null;

        
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (WebConfigSettings.DisableSearchIndex)
            {
                this.Visible = false;
                return;
            }
            
            lblSearchHeading.Text = Resource.SiteSearchHeading;
            String searchButtonText = Resources.Resource.SearchButtonText;
            string urlToUse = SiteUtils.GetNavigationSiteRoot() + "/SearchResults.aspx";

            if (Request.IsSecureConnection)
            {
                siteSettings = CacheHelper.GetCurrentSiteSettings();
                if ((siteSettings != null) && (!siteSettings.UseSslOnAllPages)) { urlToUse = urlToUse.Replace("https", "http"); }

            }
			
            if(linkOnly)
			{
                pnlSearch.Visible = false;
                heading.Visible = false;
                this.txtSearch.Visible = false;
                this.btnSearch.Visible = false;
                


				if(UseLeftSeparator)
				{
					searchLink.Text = "<" + "span class='accent'>|</span>" 
						+ " <" + "a href='" 
						+ urlToUse + "' class='sitelink'>"
                        + searchButtonText + "<" + "/a>";
				}
				else
				{
                    if (renderAsListItem)
                    {
                        searchLink.Text = "<li class='" + listItemCSS + "'><" + "a href='"
                            + urlToUse + "' class='sitelink'>"
                            + searchButtonText + "<" + "/a></li>";
                    }
                    else
                    {
                        searchLink.Text = " <" + "a href='"
                            + urlToUse + "' class='sitelink'>"
                            + searchButtonText + "<" + "/a>";
                    }
					
				}
				this.Controls.Add(searchLink);
                

			}
			else
			{
                heading.Visible = useHeading;
                //btnSearch.PostBackUrl = urlToUse;
                //btnSearch2.PostBackUrl = btnSearch.PostBackUrl;

                if ((Request.CurrentExecutionFilePath.IndexOf("SearchResults.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                    && (Request.CurrentExecutionFilePath.IndexOf("Login.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                    && (Request.CurrentExecutionFilePath.IndexOf("Register.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                    && (Request.CurrentExecutionFilePath.IndexOf("RecoverPassword.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                    )
                {

                    this.btnSearch.Text = searchButtonText;
                    if (useWatermark)
                    {
                        this.txtSearch.Watermark = Resource.SearchInputWatermark;
                    }
                    else
                    {
                        this.txtSearch.Watermark = string.Empty;
                    }

                    if (buttonCssClass.Length > 0)
                    {
                        btnSearch2.CssClass = buttonCssClass;
                        btnSearch.CssClass = buttonCssClass;
                    }

                    if (textBoxCssClass.Length > 0)
                    {
                        txtSearch.CssClass = textBoxCssClass;
                    }

                    pnlSearch.Style.Add("display", "inline");

                    if (imageUrl.Length > 0)
                    {
                        if (imageUrl.Contains("skinbase_"))
                            imageUrl = imageUrl.Replace("skinbase_", SiteUtils.GetSkinBaseUrl(Page));

                        pnlSearch.DefaultButton = btnSearch2.ID;
                        btnSearch.Visible = false;
                        btnSearch2.Visible = true;
                        btnSearch2.ImageUrl = imageUrl;
                        btnSearch2.AlternateText = searchButtonText;

                    }
                    else
                    {
                        pnlSearch.DefaultButton = btnSearch.ID;
                    }
                    
                }
                else
                {
                    this.pnlSearch.Visible = false;
                    this.txtSearch.Visible = false;
                    this.btnSearch.Visible = false;
                }
			}
			

		}

        protected void btnSearch2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //if (Page.IsCrossPagePostBack) return;
            DoRedirectToSearchResults();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (Page.IsCrossPagePostBack) return;
            DoRedirectToSearchResults();
        }


        private void DoRedirectToSearchResults()
        {
            if (
                (txtSearch.Text.Length > 0)
                && (txtSearch.Text != Resource.SearchInputWatermark)
                )
            {
                string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/SearchResults.aspx?q=" + Server.UrlEncode(txtSearch.Text);

                WebUtils.SetupRedirect(this, redirectUrl);
            }


        }


		

		
	}
}
