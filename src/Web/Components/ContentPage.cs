// Author:					Joe Audette
// Created:				    2007-08-07
// Last Modified:			2008-07-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.ObjectModel;
using System.Web;
using System.Xml;
using log4net;

namespace Cynthia.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentPage
    {
        private ContentPage()
        { }

        private static readonly ILog log
            = LogManager.GetLogger(typeof(ContentFeature));

        private Guid pageGuid = Guid.Empty;
        private Guid parentGuid = Guid.Empty;
        private string resourceFile = string.Empty;
        private string name = string.Empty;
        private string title = string.Empty;
        private string url = string.Empty;
        private string menuImage = string.Empty;
        private int pageOrder = 1;
        private bool requireSSL = false;
        private string visibleToRoles = "All Users;";
        private string editRoles = string.Empty;
        private string draftEditRoles = string.Empty;
        private string createChildPageRoles = string.Empty;
        private string pageMetaKeyWords = string.Empty;
        private string pageMetaDescription = string.Empty;
        
        
        private Collection<ContentPageItem> pageItems
            = new Collection<ContentPageItem>();

        public Guid PageGuid
        {
            get { return pageGuid; }
           
        }

        public Guid ParentGuid
        {
            get { return parentGuid; }

        }

        public string ResourceFile
        {
            get { return resourceFile; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Title
        {
            get { return title; }
        }
        public string Url
        {
            get { return url; }
        }

        public int PageOrder
        {
            get { return pageOrder; }
        }

        public string MenuImage
        {
            get { return menuImage; }
        }

        public bool RequireSsl
        {
            get { return requireSSL; }
        }

        public string VisibleToRoles
        {
            get { return visibleToRoles; }
        }

        public string EditRoles
        {
            get { return editRoles; }
        }

        public string DraftEditRoles
        {
            get { return draftEditRoles; }
        }

        public string CreateChildPageRoles
        {
            get { return createChildPageRoles; }
        }

        public string PageMetaKeyWords
        {
            get { return pageMetaKeyWords; }
        }

        public string PageMetaDescription
        {
            get { return pageMetaDescription; }
        }

        public Collection<ContentPageItem> PageItems
        {
            get
            {
                return pageItems;
            }
        }


        public static void LoadPages(
            ContentPageConfiguration contentPageConfig,
            XmlNode documentElement)
        {
            if (HttpContext.Current == null) return;
            if (documentElement.Name != "siteContent") return;

            XmlNode pagesNode = null;

            foreach (XmlNode node in documentElement.ChildNodes)
            {
                if (node.Name == "pages")
                {
                    pagesNode = node;
                    break;
                }
            }

            if(pagesNode == null) return;

            foreach (XmlNode node in pagesNode.ChildNodes)
            {
                if (node.Name == "page")
                {
                    ContentPage contentPage = new ContentPage();

                    XmlAttributeCollection attributeCollection
                        = node.Attributes;

                    if (attributeCollection["pageGuid"] != null)
                    {
                        contentPage.pageGuid = new Guid(attributeCollection["pageGuid"].Value);
                    }

                    if (attributeCollection["parentGuid"] != null)
                    {
                        contentPage.parentGuid = new Guid(attributeCollection["parentGuid"].Value);
                    }


                    if (attributeCollection["resourceFile"] != null)
                    {
                        contentPage.resourceFile = attributeCollection["resourceFile"].Value;
                    }

                    if (attributeCollection["name"] != null)
                    {
                        contentPage.name = attributeCollection["name"].Value;
                    }


                    if (attributeCollection["title"] != null)
                    {
                        contentPage.title = attributeCollection["title"].Value;
                    }

                   
                    if (attributeCollection["url"] != null)
                    {
                        contentPage.url = attributeCollection["url"].Value;
                    }

                    if (attributeCollection["menuImage"] != null)
                    {
                        contentPage.menuImage = attributeCollection["menuImage"].Value;
                    }

                    if (attributeCollection["pageOrder"] != null)
                    {
                        int sort = 1;
                        if (int.TryParse(attributeCollection["pageOrder"].Value,
                            out sort))
                        {
                            contentPage.pageOrder = sort;
                        }
                    }

                    if (attributeCollection["visibleToRoles"] != null)
                    {
                        contentPage.visibleToRoles = attributeCollection["visibleToRoles"].Value;
                    }

                    if (attributeCollection["editRoles"] != null)
                    {
                        contentPage.editRoles = attributeCollection["editRoles"].Value;
                    }

                    if (attributeCollection["draftEditRoles"] != null)
                    {
                        contentPage.draftEditRoles = attributeCollection["draftEditRoles"].Value;
                    }

                    if (attributeCollection["createChildPageRoles"] != null)
                    {
                        contentPage.createChildPageRoles = attributeCollection["createChildPageRoles"].Value;
                    }

                    if (attributeCollection["pageMetaKeyWords"] != null)
                    {
                        contentPage.pageMetaKeyWords = attributeCollection["pageMetaKeyWords"].Value;
                    }

                    if (attributeCollection["pageMetaDescription"] != null)
                    {
                        contentPage.pageMetaDescription = attributeCollection["pageMetaDescription"].Value;
                    }

                    if (
                        (attributeCollection["requireSSL"] != null)
                        && (attributeCollection["requireSSL"].ToString().ToLower() == "true")
                        )
                    {
                        contentPage.requireSSL = true;
                    }


                    foreach (XmlNode contentFeatureNode in node.ChildNodes)
                    {
                        if (contentFeatureNode.Name == "contentFeature")
                        {
                            ContentPageItem.LoadPageItem(
                                contentPage,
                                contentFeatureNode);
                        }
                    }

                    if (contentPage.pageGuid == Guid.Empty)
                    {
                        log.Error("could not install page " + contentPage.Name
                        + ". Invalid PageGuid.");
                        return;

                    }


                    contentPageConfig.ContentPages.Add(contentPage);
                    

                    
                }

            }


        }

    }
}
