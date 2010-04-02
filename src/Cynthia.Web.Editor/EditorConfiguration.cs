using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Cynthia.Web.Editor
{
    /// <summary>
    /// Author:		        Joe Audette
    /// Created:            2007/05/28
    /// Last Modified:      2007/05/28
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class EditorConfiguration 
    {
        private ProviderSettingsCollection providerSettingsCollection = new ProviderSettingsCollection();
        private string defaultProvider = "FCKeditorProvider";

        public EditorConfiguration(XmlNode node)
        {
            LoadValuesFromConfigurationXml(node);
        }

        public ProviderSettingsCollection Providers
        {
            get { return providerSettingsCollection; }
        }


        public string DefaultProvider
        {
            get {return defaultProvider;}
        }

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node.Attributes["defaultProvider"] != null)
            {
                defaultProvider = node.Attributes["defaultProvider"].Value;
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "providers")
                {
                    foreach (XmlNode providerNode in child.ChildNodes)
                    {
                        if (
                            (providerNode.NodeType == XmlNodeType.Element)
                            && (providerNode.Name == "add")
                            )
                        {
                            if (
                                (providerNode.Attributes["name"] != null)
                                && (providerNode.Attributes["type"] != null)
                                )
                            {
                                ProviderSettings providerSettings
                                    = new ProviderSettings(
                                    providerNode.Attributes["name"].Value,
                                    providerNode.Attributes["type"].Value);

                                providerSettingsCollection.Add(providerSettings);
                            }

                        }
                    }

                }
            }

            
           
        }

        public static EditorConfiguration GetConfig()
        {
            EditorConfiguration editorConfig = null;

            if (
                (HttpRuntime.Cache["CEditorConfig"] != null)
                && (HttpRuntime.Cache["CEditorConfig"] is EditorConfiguration)
            )
            {
                return (EditorConfiguration)HttpRuntime.Cache["CEditorConfig"];
            }
            else
            {
                String configFileName = "CEditor.config";
                if (ConfigurationManager.AppSettings["CEditorConfigFileName"] != null)
                {
                    configFileName = ConfigurationManager.AppSettings["CEditorConfigFileName"];

                }
 
                if (!configFileName.StartsWith("~/"))
                {
                    configFileName = "~/" + configFileName;
                }

                String pathToConfigFile = HttpContext.Current.Server.MapPath(configFileName);

                XmlDocument configXml = new XmlDocument();
                configXml.Load(pathToConfigFile);
                editorConfig = new EditorConfiguration(configXml.DocumentElement);

                AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));
                
                System.Web.HttpRuntime.Cache.Insert(
                    "CEditorConfig",
                    editorConfig,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (EditorConfiguration)HttpRuntime.Cache["CEditorConfig"];

            }

            //return editorConfig;

        }
       
        

    }
}
