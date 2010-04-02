// Author:             Joe Audette
// Created:            2006-10-29
// Last Modified:      2008-11-26

using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class CProfileConfiguration
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CProfileConfiguration));

        public CProfileConfiguration()
        {}


        public CProfileConfiguration(XmlNode node)
        {
            LoadValuesFromConfigurationXml(node);
        }

        private Collection<CProfilePropertyDefinition> propertyDefinitions = new Collection<CProfilePropertyDefinition>();

        public Collection<CProfilePropertyDefinition> PropertyDefinitions
        {
            get
            {
                // TODO: store in the cache?


                return propertyDefinitions;
            }
        }


        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "properties")
                {
                    foreach (XmlNode propertyNode in child.ChildNodes)
                    {
                        if (
                            (propertyNode.NodeType == XmlNodeType.Element)
                            && (propertyNode.Name == "add")
                            )
                        {
                            CProfilePropertyDefinition profilePropertyDefinition
                                = new CProfilePropertyDefinition();

                            XmlAttributeCollection attributeCollection = propertyNode.Attributes;

                            if (attributeCollection["name"] != null)
                            {
                                profilePropertyDefinition.Name = attributeCollection["name"].Value;
                            }

                            if (attributeCollection["iSettingControlSrc"] != null)
                            {
                                profilePropertyDefinition.ISettingControlSrc
                                    = attributeCollection["iSettingControlSrc"].Value;
                            }

                            if (attributeCollection["resourceFile"] != null)
                            {
                                profilePropertyDefinition.ResourceFile
                                    = attributeCollection["resourceFile"].Value;
                            }

                            if (attributeCollection["labelResourceKey"] != null)
                            {
                                profilePropertyDefinition.LabelResourceKey
                                    = attributeCollection["labelResourceKey"].Value;
                            }

                            if (attributeCollection["type"] != null)
                            {
                                profilePropertyDefinition.Type = attributeCollection["type"].Value;
                            }

                            if (attributeCollection["includeTimeForDate"] != null)
                            {
                                profilePropertyDefinition.IncludeTimeForDate
                                    = bool.Parse(attributeCollection["includeTimeForDate"].Value);
                            }


                            // for now only serialize as string

                            //if (attributeCollection["serializeAs"] != null)
                            //{
                            //    profilePropertyDefinition.SerializeAs
                            //        = (SettingsSerializeAs)Enum.Parse(typeof(SettingsSerializeAs), attributeCollection["serializeAs"].Value);
                            //}


                            if (attributeCollection["lazyLoad"] != null)
                            {
                                profilePropertyDefinition.LazyLoad
                                    = bool.Parse(attributeCollection["lazyLoad"].Value);
                            }

                            if (attributeCollection["allowMarkup"] != null)
                            {
                                profilePropertyDefinition.AllowMarkup
                                    = bool.Parse(attributeCollection["allowMarkup"].Value);
                            }


                            if (attributeCollection["requiredForRegistration"] != null)
                            {
                                profilePropertyDefinition.RequiredForRegistration
                                    = bool.Parse(attributeCollection["requiredForRegistration"].Value);
                            }

                            if (attributeCollection["onlyAvailableForRoles"] != null)
                            {
                                profilePropertyDefinition.OnlyAvailableForRoles = attributeCollection["onlyAvailableForRoles"].Value;
                            }

                            if (attributeCollection["onlyVisibleForRoles"] != null)
                            {
                                profilePropertyDefinition.OnlyVisibleForRoles = attributeCollection["onlyVisibleForRoles"].Value;
                            }

                            if (attributeCollection["allowAnonymous"] != null)
                            {
                                profilePropertyDefinition.AllowAnonymous
                                    = bool.Parse(attributeCollection["allowAnonymous"].Value);
                            }

                            if (attributeCollection["includeHelpLink"] != null)
                            {
                                profilePropertyDefinition.IncludeHelpLink
                                    = bool.Parse(attributeCollection["includeHelpLink"].Value);
                            }

                            if (attributeCollection["visibleToAnonymous"] != null)
                            {
                                profilePropertyDefinition.VisibleToAnonymous
                                    = bool.Parse(attributeCollection["visibleToAnonymous"].Value);
                            }

                            if (attributeCollection["visibleToAuthenticated"] != null)
                            {
                                profilePropertyDefinition.VisibleToAuthenticated
                                    = bool.Parse(attributeCollection["visibleToAuthenticated"].Value);
                            }

                            if (attributeCollection["visibleToUser"] != null)
                            {
                                profilePropertyDefinition.VisibleToUser
                                    = bool.Parse(attributeCollection["visibleToUser"].Value);
                            }

               
                            if (attributeCollection["editableByUser"] != null)
                            {
                                profilePropertyDefinition.EditableByUser
                                    = bool.Parse(attributeCollection["editableByUser"].Value);
                            }


                            if (attributeCollection["maxLength"] != null)
                            {
                                profilePropertyDefinition.MaxLength
                                    = int.Parse(attributeCollection["maxLength"].Value);
                            }

                            if (attributeCollection["rows"] != null)
                            {
                                profilePropertyDefinition.Rows
                                    = int.Parse(attributeCollection["rows"].Value);
                            }

                            if (attributeCollection["columns"] != null)
                            {
                                profilePropertyDefinition.Columns
                                    = int.Parse(attributeCollection["columns"].Value);
                            }

                            if (attributeCollection["regexValidationExpression"] != null)
                            {
                                profilePropertyDefinition.RegexValidationExpression
                                    = attributeCollection["regexValidationExpression"].Value;
                            }

                            if (attributeCollection["regexValidationErrorResourceKey"] != null)
                            {
                                profilePropertyDefinition.RegexValidationErrorResourceKey
                                    = attributeCollection["regexValidationErrorResourceKey"].Value;
                            }

                            if (attributeCollection["defaultValue"] != null)
                            {
                                profilePropertyDefinition.DefaultValue = attributeCollection["defaultValue"].Value;
                            }

                            if (propertyNode.HasChildNodes)
                            {
                                LoadOptionList(profilePropertyDefinition, propertyNode);
                            }


                            this.propertyDefinitions.Add(profilePropertyDefinition);
                        }
                        

                    }


                }
                

            }
        }

        private static void LoadOptionList(
            CProfilePropertyDefinition profilePropertyDefinition, 
            XmlNode propertyNode)
        {
            foreach (XmlNode optionListNode in propertyNode.ChildNodes)
            {
                if (optionListNode.Name == "OptionList")
                {
                    
                    foreach (XmlNode optionNode in optionListNode.ChildNodes)
                    {
                        if (optionNode.Name == "Option")
                        {
                            CProfilePropertyOption option = new CProfilePropertyOption();
                            if (optionNode.Attributes["TextResourceKey"] != null)
                            {
                                option.TextResourceKey = optionNode.Attributes["TextResourceKey"].Value;
                            }

                            if (optionNode.Attributes["value"] != null)
                            {
                                option.Value = optionNode.Attributes["value"].Value;
                            }

                            profilePropertyDefinition.OptionList.Add(option);

                        }
                    }

                    // should only be one OptionListNode
                    break;


                }

            }


        }

        public bool Contains(String propertyName)
        {
            bool result = false;

            foreach (CProfilePropertyDefinition profilePropertyDefinition in this.PropertyDefinitions)
            {
                if (profilePropertyDefinition.Name == propertyName)
                {
                    result = true;
                }

            }

            return result;
        }

        public CProfilePropertyDefinition GetPropertyDefinition(String propertyName)
        {
            foreach (CProfilePropertyDefinition profilePropertyDefinition in this.PropertyDefinitions)
            {
                if (profilePropertyDefinition.Name == propertyName)
                {
                    return profilePropertyDefinition;
                }

            }

            return null;
        }

        public bool HasRequiredCustomProperties()
        {
            bool result = false;
            foreach (CProfilePropertyDefinition profilePropertyDefinition in this.PropertyDefinitions)
            {
                if (profilePropertyDefinition.RequiredForRegistration)
                {
                    result = true;
                }

            }

            return result; ;
        }



        public static CProfileConfiguration GetConfig()
        {
            CProfileConfiguration profileConfig = null;
            string cacheKey = GetCacheKey();

            if (
                (System.Web.HttpRuntime.Cache[cacheKey] != null)
                && (System.Web.HttpRuntime.Cache[cacheKey] is CProfileConfiguration)
            )
            {
                return (CProfileConfiguration)System.Web.HttpRuntime.Cache[cacheKey];
            }
            else
            {
                string configFileName = GetConfigFileName();

                if (configFileName.Length == 0) { return profileConfig; }

                if (!configFileName.StartsWith("~/"))
                {
                    configFileName = "~/" + configFileName;
                }

                string pathToConfigFile = HttpContext.Current.Server.MapPath(configFileName);

                XmlDocument configXml = new XmlDocument();
                configXml.Load(pathToConfigFile);
                profileConfig = new CProfileConfiguration(configXml.DocumentElement);

                AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));
                // more dependencies can be added if needed

                System.Web.HttpRuntime.Cache.Insert(
                    cacheKey,
                    profileConfig,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (CProfileConfiguration)System.Web.HttpRuntime.Cache[cacheKey];

            
            }
 
        }

        private static string GetConfigFileName()
        {
            string configFileName = string.Empty;

            if (!WebConfigSettings.UseRelatedSiteMode)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

                if (ConfigurationManager.AppSettings["CProfileConfigFileName-" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)] != null)
                {
                    configFileName = ConfigurationManager.AppSettings["CProfileConfigFileName-" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)];
                    return configFileName;
                }
            }

            if (ConfigurationManager.AppSettings["CProfileConfigFileName"] != null)
            {
                configFileName = ConfigurationManager.AppSettings["CProfileConfigFileName"];
            }


            return configFileName;
        }

        private static string GetCacheKey()
        {
            string cacheKey = "CProfileConfig";

            if (!WebConfigSettings.UseRelatedSiteMode)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                cacheKey += siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
                return cacheKey;
            }

           
            return cacheKey;
        }

    }
}
