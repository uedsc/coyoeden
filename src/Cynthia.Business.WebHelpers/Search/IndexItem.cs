// Author:				Joe Audette
// Created:			    2005-06-26
// Last Modified:		2009-06-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace Cynthia.Business.WebHelpers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public class IndexItem
    {
        #region Constructors

        public IndexItem()
        {
        }

        public IndexItem(Lucene.Net.Documents.Document doc, float score)
        {
            ViewRoles = doc.Get("ViewRoles");
            SiteId = Convert.ToInt32(doc.Get("SiteID"), CultureInfo.InvariantCulture);
            PageId = Convert.ToInt32(doc.Get("PageID"), CultureInfo.InvariantCulture);
            PageName = doc.Get("PageName");
            PageIndex = Convert.ToInt32(doc.Get("PageIndex"), CultureInfo.InvariantCulture);
            PageNumber = Convert.ToInt32(doc.Get("PageNumber"), CultureInfo.InvariantCulture);
            
            string fid = doc.Get("FeatureId");
            if ((fid != null)&&(fid.Length > 0))
            {
                FeatureId = fid;
            }
            FeatureName = doc.Get("FeatureName");
            ItemId = Convert.ToInt32(doc.Get("ItemID"), CultureInfo.InvariantCulture);
            ModuleId = Convert.ToInt32(doc.Get("ModuleID"), CultureInfo.InvariantCulture);
            ModuleTitle = doc.Get("ModuleTitle");
            Title = doc.Get("Title");
            intro = doc.Get("Intro");
            ViewPage = doc.Get("ViewPage");
            QueryStringAddendum = doc.Get("QueryStringAddendum");
            
            DateTime pubBegin = DateTime.MinValue;
            if (DateTime.TryParse(doc.Get("PublishBeginDate"), out pubBegin))
            {
                this.publishBeginDate = pubBegin;
            }

            DateTime pubEnd = DateTime.MaxValue;
            if (DateTime.TryParse(doc.Get("PublishEndDate"), out pubEnd))
            {
                this.publishEndDate = pubEnd;
            }

            bool useQString;
            if (bool.TryParse(doc.Get("UseQueryStringParams"), out useQString))
            {
                this.useQueryStringParams = useQString;
            }

            
            boost = doc.GetBoost();

            
        }

        #endregion

        #region Private Properties

        private int siteID = -1;
        private int pageID = -1;
        private int moduleID = -1;
        private int itemID = -1;
        private int pageIndex = -1;
        private int pageNumber = 1; // for use in pageable modules like forums
        private string pageName = string.Empty;
        private string featureId = Guid.Empty.ToString();
        private string featureName = string.Empty;
        private string featureResourceFile = string.Empty;
        private string moduleTitle = string.Empty;
        private string title = string.Empty;
        private string content = string.Empty;
        private string otherContent = string.Empty;
        private string intro = string.Empty;
        private string viewRoles = string.Empty;
        private string moduleViewRoles = string.Empty;
        private string viewPage = "Default.aspx";
        private bool useQueryStringParams = true;
        private string queryStringAddendum = string.Empty;
        private DateTime publishBeginDate = DateTime.MinValue;
        private DateTime publishEndDate = DateTime.MaxValue;
        private string indexPath = string.Empty;
        private string itemKey = string.Empty;
        private string pageMetaDescription = string.Empty;
        private string pageMetaKeywords = string.Empty;

        private float score = 0;
        private float boost = 0;

        
        

        #endregion


        #region Public Properties

        public string IndexPath
        {
            get { return indexPath; }
            set { indexPath = value; }
        }

        public string Key
        {
            get
            {
                return this.siteID.ToString(CultureInfo.InvariantCulture) + "~"
                    + this.pageID.ToString(CultureInfo.InvariantCulture) + "~"
                    + this.moduleID.ToString(CultureInfo.InvariantCulture) + "~"
                    + ItemKey
                    + this.queryStringAddendum;
            }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public int PageId
        {
            get { return pageID; }
            set { pageID = value; }
        }

        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public int ItemId
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public string ItemKey
        {
            get 
            {
                if (ItemId > -1)
                {
                    //return ItemId.ToString(CultureInfo.InvariantCulture) + itemKey;
                    return ItemId.ToString(CultureInfo.InvariantCulture);
                }
                return itemKey; 
            }
            set { itemKey = value; }
        }

        /// <summary>
        /// legacy field not needed anymore
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }


        [XmlIgnore]
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }

        // This is needed to support xml serialization, string with special characterscan cause invalid xml, base 64 encoding them gets around the problem.

        [XmlElement(ElementName = "pageName", DataType = "base64Binary")]
        public byte[] PageNameSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(PageName);
            }
            set
            {
                PageName = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string PageMetaDescription
        {
            get { return pageMetaDescription; }
            set { pageMetaDescription = value; }
        }

        [XmlElement(ElementName = "pageMetaDescription", DataType = "base64Binary")]
        public byte[] PageMetaDescriptionSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(PageMetaDescription);
            }
            set
            {
                PageMetaDescription = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string PageMetaKeywords
        {
            get { return pageMetaKeywords; }
            set { pageMetaKeywords = value; }
        }

        [XmlElement(ElementName = "pageMetaKeywords", DataType = "base64Binary")]
        public byte[] PageMetaKeywordsSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(PageMetaKeywords);
            }
            set
            {
                PageMetaKeywords = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        public string FeatureId
        {
            get { return featureId; }
            set { featureId = value; }
        }

        [XmlIgnore]
        public string FeatureName
        {
            get { return featureName; }
            set { featureName = value; }
        }

        [XmlElement(ElementName = "featureName", DataType = "base64Binary")]
        public byte[] FeatureNameSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(FeatureName);
            }
            set
            {
                FeatureName = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        public string FeatureResourceFile
        {
            get { return featureResourceFile; }
            set { featureResourceFile = value; }
        }

        [XmlIgnore]
        public string ModuleTitle
        {
            get { return moduleTitle; }
            set { moduleTitle = value; }
        }

        [XmlElement(ElementName = "moduleTitle", DataType = "base64Binary")]
        public byte[] ModuleTitleSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ModuleTitle);
            }
            set
            {
                ModuleTitle = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [XmlElement(ElementName = "title", DataType = "base64Binary")]
        public byte[] TitleSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Title);
            }
            set
            {
                Title = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string Intro
        {
            get { return intro; }
            set { intro = value; }
        }

        [XmlElement(ElementName = "intro", DataType = "base64Binary")]
        public byte[] IntroSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Intro);
            }
            set
            {
                Intro = System.Text.Encoding.Unicode.GetString(value);
            }
        }  


        [XmlIgnore]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        [XmlElement(ElementName = "content", DataType = "base64Binary")]
        public byte[] ContentSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Content);
            }
            set
            {
                Content = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string OtherContent
        {
            get { return otherContent; }
            set { otherContent = value; }
        }

        [XmlElement(ElementName = "otherContent", DataType = "base64Binary")]
        public byte[] OtherContentSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(OtherContent);
            }
            set
            {
                OtherContent = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string ViewRoles
        {
            get { return viewRoles; }
            set { viewRoles = value; }
        }

        [XmlElement(ElementName = "viewRoles", DataType = "base64Binary")]
        public byte[] ViewRolesSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ViewRoles);
            }
            set
            {
                ViewRoles = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string ModuleViewRoles
        {
            get { return moduleViewRoles; }
            set { moduleViewRoles = value; }
        }

        [XmlElement(ElementName = "moduleViewRoles", DataType = "base64Binary")]
        public byte[] ModuleViewRolesSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ModuleViewRoles);
            }
            set
            {
                ModuleViewRoles = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string ViewPage
        {
            get { return viewPage; }
            set { viewPage = value; }
        }

        [XmlElement(ElementName = "viewPage", DataType = "base64Binary")]
        public byte[] ViewPageSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ViewPage);
            }
            set
            {
                ViewPage = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        public bool UseQueryStringParams
        {
            get { return useQueryStringParams; }
            set { useQueryStringParams = value; }
        }

        public string QueryStringAddendum
        {
            get { return queryStringAddendum; }
            set { queryStringAddendum = value; }
        }

        public DateTime PublishBeginDate
        {
            get { return publishBeginDate; }
            set { publishBeginDate = value; }
        }

        public DateTime PublishEndDate
        {
            get { return publishEndDate; }
            set { publishEndDate = value; }
        }

        public float Score
        {
            get { return score; }
        }

        public float Boost
        {
            get { return boost; }
        }

        #endregion

    }
}
