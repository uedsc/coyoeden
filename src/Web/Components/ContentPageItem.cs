// Author:					Joe Audette
// Created:				    2007-08-07
// Last Modified:			2008-01-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Xml;

namespace Cynthia.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentPageItem
    {
        private ContentPageItem()
        { }

        private Guid featureGuid = Guid.Empty;
        private string contentTitle = string.Empty;
        private string contentTemplate = string.Empty;
        private string location = "center";
        private int sortOrder = 1;
        private int cacheTimeInSeconds = 0;


        public Guid FeatureGuid
        {
            get { return featureGuid; }
        }

        public string ContentTitle
        {
            get { return contentTitle; }
        }

        public string ContentTemplate
        {
            get { return contentTemplate; } 
        }

        public string Location
        {
            get { return location; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
        }

        public int CacheTimeInSeconds
        {
            get { return cacheTimeInSeconds; }
        }

        

        public static void LoadPageItem(
            ContentPage contentPage,
            XmlNode pageItemNode)
        {
            if (contentPage == null) return;
            if (pageItemNode == null) return;

            if (pageItemNode.Name == "contentFeature")
            {
                ContentPageItem pageItem = new ContentPageItem();

                XmlAttributeCollection attributeCollection
                        = pageItemNode.Attributes;

                if (attributeCollection["featureGuid"] != null)
                {
                    pageItem.featureGuid = new Guid(attributeCollection["featureGuid"].Value);
                }

                if (attributeCollection["contentTitle"] != null)
                {
                    pageItem.contentTitle = attributeCollection["contentTitle"].Value;
                }

                if (attributeCollection["contentTemplate"] != null)
                {
                    pageItem.contentTemplate = attributeCollection["contentTemplate"].Value;
                }

                if (attributeCollection["location"] != null)
                {
                    string location = attributeCollection["location"].Value;
                    switch (location)
                    {
                        case "right":
                            pageItem.location = "rightpane";
                            break;

                        case "left":
                            pageItem.location = "leftpane";
                            break;

                        case "center":
                        default:
                            pageItem.location = "contentpane";
                            break;

                    }
                }

                if (attributeCollection["sortOrder"] != null)
                {
                    int sort = 1;
                    if (int.TryParse(attributeCollection["sortOrder"].Value,
                        out sort))
                    {
                        pageItem.sortOrder = sort;
                    }
                }

                if (attributeCollection["cacheTimeInSeconds"] != null)
                {
                    int cacheTimeInSeconds = 1;
                    if (int.TryParse(attributeCollection["cacheTimeInSeconds"].Value,
                        out cacheTimeInSeconds))
                    {
                        pageItem.cacheTimeInSeconds = cacheTimeInSeconds;
                    }
                }

                contentPage.PageItems.Add(pageItem);
                
            }


        }

    }
}
