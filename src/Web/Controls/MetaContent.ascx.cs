/// Author:				    Joe Audette
/// Created:			    2004-08-28
///		
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	
/// 
///	2005-09-24 added RSS link feature provided by Philip Gear
///		for enhanced RSS access in FireFox and Netscape
/// 
/// 2009-05-26 added autodiscovery link for OpenSearch support https://developer.mozilla.org/en/Creating_OpenSearch_plugins_for_Firefox
/// 2009-07-27 added config option for content type so its possible to serve as text/html in anticipation of moving to Html 5
/// 

using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.UI
{
	public partial class MetaContent : UserControl
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(MetaContent));

        //private const string metaEncodingXhtml = "\n<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />";
        //private const string metaEncodingHtml = "\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";

       
        private string keywordCsv = string.Empty;
        private string description = string.Empty;
        private string additionalMetaMarkup = string.Empty;
        private StringBuilder keywords = null;
        private SiteSettings siteSettings = null;

        public string KeywordCsv
        {
            get { return keywordCsv; }
            set { keywordCsv = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string AdditionalMetaMarkup
        {
            get { return additionalMetaMarkup; }
            set { additionalMetaMarkup = value; }
        }

        public void AddKeword(string keyword)
        {
            if (keyword == null) { return; }

            if (keywords == null)
            {
                keywords = new StringBuilder();
                keywords.Append(keyword);
                return;
            }

            if (keywords.Length > 0)
            {
                keywords.Append("," + keyword);
            }
            else
            {
                keywords.Append(keyword);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            AddDescription();
            AddKeywords();
            if (WebConfigSettings.AutoSetContentType) { AddEncoding(); }

            if (additionalMetaMarkup.Length > 0)
            {
                Literal additionalMeta = new Literal();
                additionalMeta.Text = additionalMetaMarkup;
                this.Controls.Add(additionalMeta);
            }

            AddOpenSearchLink();

        }

        private void AddKeywords()
        {
            if (keywords != null)
            {
                if (keywordCsv.Length > 0)
                {
                    keywordCsv = keywordCsv + "," + keywords.ToString();
                }
                else
                {
                    keywordCsv = keywords.ToString();
                }
            }

            if (keywordCsv.Length == 0) { return; }

            Literal metaKeywordsLiteral = new Literal();
            metaKeywordsLiteral.Text = "\n<meta name=\"keywords\" content=\"" + keywordCsv + "\" />"; 
            this.Controls.Add(metaKeywordsLiteral);

        }

        private void AddDescription()
        {
            if (description.Length == 0) { return; }

            Literal metaDescriptionLiteral = new Literal();
            metaDescriptionLiteral.Text = "\n<meta name=\"description\" content=\"" + description + "\" />";
            this.Controls.Add(metaDescriptionLiteral);
        }

        private void AddEncoding()
        {
            string contentTypeMeta = "\n<meta http-equiv=\"Content-Type\" content=\"" 
                + WebConfigSettings.ContentMimeType 
                + "; charset=" + WebConfigSettings.ContentEncoding + "\" />";

            Literal metaEncodingLiteral = new Literal();
            metaEncodingLiteral.Text = contentTypeMeta;
            this.Controls.Add(metaEncodingLiteral);
        }

        private void AddOpenSearchLink()
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (WebConfigSettings.DisableOpenSearchAutoDiscovery) { return; }

            if (siteSettings == null) { siteSettings = CacheHelper.GetCurrentSiteSettings(); }
            if (siteSettings == null) { return; }

            string searchTitle;
            if (siteSettings.OpenSearchName.Length > 0)
            {
                searchTitle = siteSettings.OpenSearchName;
            }
            else
            {
                searchTitle = string.Format(CultureInfo.InvariantCulture, Resource.SearchDiscoveryTitleFormat, siteSettings.SiteName);
            }

            Literal openSearchLink = new Literal();
            openSearchLink.Text = "\n<link rel=\"search\" type=\"application/opensearchdescription+xml\" title=\""
                + searchTitle + "\" href=\"" + SiteUtils.GetNavigationSiteRoot() + "/SearchEngineInfo.ashx" + "\" />";

            this.Controls.Add(openSearchLink);
        }

        // TODO: http://www.w3.org/P3P/validator/20020128/document implement xml
        //http://www.w3.org/P3P/validator.html
        //private void AddP3PLink()
        //{
        //    if (WebConfigSettings.DisableSearchIndex) { return; }
        //    if (WebConfigSettings.DisableOpenSearchAutoDiscovery) { return; }

        //    if (siteSettings == null) { siteSettings = CacheHelper.GetCurrentSiteSettings(); }
        //    if (siteSettings == null) { return; }

        //    string searchTitle = string.Format(CultureInfo.InvariantCulture, Resource.SearchDiscoveryTitleFormat, siteSettings.SiteName);

        //    Literal openSearchLink = new Literal();
        //    openSearchLink.Text = "\n<link rel=\"P3Pv1\" href=\"" + SiteUtils.GetNavigationSiteRoot() + "/SearchEngineInfo.ashx" + "\" />";

        //    this.Controls.Add(openSearchLink);
        //}

        

	}
}
