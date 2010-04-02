/// Author:              Joe Audette
/// Created:             2006-02-06
/// Last Modified:       2009-04-01

using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace Cynthia.Web.Framework
{
    
    public class CEncryptionConfiguration
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(CEncryptionConfiguration));

        private XmlNode rsaNode;

        

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "RSAKeyValue")
                    rsaNode = child;
            }
        }

        public String RsaKey
        {
            get
            {
                if (this.rsaNode != null)
                    return this.rsaNode.OuterXml;

                return String.Empty;

            }

        }

        public static CEncryptionConfiguration GetConfig()
        {
            //return (CEncryptionConfiguration)ConfigurationManager.GetSection("system.web/CEncryption");

            try
            {
                if (
                    (HttpRuntime.Cache["CEncryptionConfiguration"] != null)
                    && (HttpRuntime.Cache["CEncryptionConfiguration"] is CEncryptionConfiguration)
                )
                {
                    return (CEncryptionConfiguration)HttpRuntime.Cache["CEncryptionConfiguration"];
                }

                CEncryptionConfiguration config
                    = new CEncryptionConfiguration();

                

                string pathToConfigFile
                    = System.Web.Hosting.HostingEnvironment.MapPath("~/CEncryption.config");

                log.Debug("path to crypto key " + pathToConfigFile);

                if (!File.Exists(pathToConfigFile))
                {
                    log.Error("crypto file not found " + pathToConfigFile);
                    return config;
                }

                FileInfo fileInfo = new FileInfo(pathToConfigFile);
                
                XmlDocument configXml = new XmlDocument();
                configXml.Load(fileInfo.FullName);
                config.LoadValuesFromConfigurationXml(configXml.DocumentElement);

                

                AggregateCacheDependency aggregateCacheDependency
                    = new AggregateCacheDependency();

               
                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

                System.Web.HttpRuntime.Cache.Insert(
                    "CEncryptionConfiguration",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (CEncryptionConfiguration)HttpRuntime.Cache["CEncryptionConfiguration"];

            }
            catch (HttpException ex)
            {
                log.Error(ex);

            }
            catch (System.Xml.XmlException ex)
            {
                log.Error(ex);

            }
            catch (ArgumentException ex)
            {
                log.Error(ex);

            }
            catch (NullReferenceException ex)
            {
                log.Error(ex);

            }

            return null;



        }


    }
}
