
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Cynthia.Web.Controls;
using Cynthia.Business;
using System.Collections.Generic;

namespace Cynthia.Web.UI
{
	public partial class SearchInput : ViewBase
	{
		#region member variables
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

		/// <summary>
		/// search url
		/// </summary>
		protected string SearchUrl { get; private set; }
		/// <summary>
		/// whether hide the control
		/// </summary>
		protected bool Hide { get; private set; }
		/// <summary>
		/// whether hide the form tag
		/// </summary>
		public bool HideFormTag { get; set; }

		private static readonly object _SynHelper = new object();
		private static List<ModuleDefinition> _SearchableModules;
		/// <summary>
		/// searchable modules
		/// </summary>
		public List<ModuleDefinition> SearchableModules
		{
			get
			{
				if (_SearchableModules == null)
				{
					lock (_SynHelper)
					{
						if (_SearchableModules == null)
						{
							_SearchableModules = ModuleDefinition.GetModules(CurSettings.SiteId, WebConfigSettings.SearchableFeatureGuidsToExclude);
						}
					}
				}
				return _SearchableModules;
			}
		} 
		#endregion


		protected void Page_Load(object sender, System.EventArgs e)
		{            
            setVisibility();
			if (Hide) {
				this.Visible = false;
				return;
			}
			setSearchUrl();
		}

		private void setVisibility()
		{
			if (WebConfigSettings.DisableSearchIndex) { Hide = true; return; }

			if (LinkOnly) { Hide = false; return; }

			Hide = WebConfigSettings.NOSearchBoxUrls.Split(',').ToList().Exists(x => Request.CurrentExecutionFilePath.IndexOf(x,StringComparison.InvariantCultureIgnoreCase)>-1);

		}

		private void setSearchUrl()
		{
			SearchUrl = String.Format("{0}/SearchResults.aspx", SiteUtils.GetNavigationSiteRoot());

			if (Request.IsSecureConnection)
			{
				if ((CurSettings != null) && (!CurSettings.UseSslOnAllPages)) { SearchUrl = SearchUrl.Replace("https", "http"); }

			}
		}
		//private void DoRedirectToSearchResults()
		//{
		//    if (
		//        (txtSearch.Text.Length > 0)
		//        && (txtSearch.Text != Resource.SearchInputWatermark)
		//        )
		//    {
		//        string redirectUrl = String.Format("{0}/SearchResults.aspx?q={1}", SiteUtils.GetNavigationSiteRoot(), Server.UrlEncode(txtSearch.Text));

		//        WebUtils.SetupRedirect(this, redirectUrl);
		//    }


		//}


		

		
	}
}
