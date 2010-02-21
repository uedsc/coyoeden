using System;
using SystemX.LunaBase;
using SystemX.LunaAtom.ClassDefinition;
using SystemX.LunaAtom.Loaders;
using System.IO;
using SystemX.LunaData;
using SystemX.LunaAtom;

namespace CoyoEden.Core.BootStrappers
{
    public sealed class ApplicationBoot:ApplicationX
    {
        #region .ctor
        public ApplicationBoot(string appName,string appVersion):base(appName,appVersion) { }
        #endregion

        #region member variables
        /// <summary>
        /// Sets the database configuration object, which contains basic 
        /// connection information along with the database vendor name 
        /// (eg. MySql, Oracle).
        /// </summary>
        public DatabaseConfig DatabaseConfig { get; set; }
        /// <summary>
        /// Sets the private key used to decrypt the database password. If your database password as supplied is
        /// in plaintext then this is not necessary. If you supply the DatabaseConfig object you can also set the
        /// private key on that instead.The private key (RSA) should be in xml format
        /// </summary>
        public string PrivateKey { get; set; }
        #endregion

        #region base overrides
        protected override void SetupClassDefs()
        {
            if (LoadClassDefs)
            {
                ClassDef.ClassDefs.Add(GetXmlClassDefsLoader().LoadClassDefs());
                ClassDefValidator validator = new ClassDefValidator(new DefClassFactory());
                validator.ValidateClassDefs(ClassDef.ClassDefs);
            }
        }

        protected override void SetupDatabaseConnection()
        {
            if (DatabaseConnection.CurrentConnection != null) return;
            if (DatabaseConfig == null) DatabaseConfig = DatabaseConfig.ReadFromAppSettings();
            string vendor = DatabaseConfig.Vendor;
            if (string.IsNullOrEmpty(vendor) || vendor.ToLower().Contains("memory"))
            {
                BORegistry.DataAccessor = new DataAccessorInMemory();
            }
            else
            {
                if (PrivateKey != null) DatabaseConfig.SetPrivateKey(PrivateKey);
                DatabaseConnection.CurrentConnection = DatabaseConfig.GetDatabaseConnection();
                BORegistry.DataAccessor = new DataAccessorDB();
            }
        }
        /// <summary>
        /// Sets up the exception notifier used to display
        /// exceptions to the final user.
        /// </summary>
        protected override void SetupExceptionNotifier()
        {
            //TODO:Implement a UIException logger.Email or just log to file.
            GlobalRegistry.UIExceptionNotifier = null;
        }
        /// <summary>
        /// Initialises the settings.  If not provided, DatabaseSettings
        /// is assumed.
        /// </summary>
        protected override void SetupSettings()
        {
            if (Settings == null) Settings = new DatabaseSettings(DatabaseConnection.CurrentConnection);
            GlobalRegistry.Settings = Settings;
        }
        #endregion

        #region helper methods
        /// <summary>
        /// Gets the loader for the xml class definitions
        /// </summary>
        /// <returns>Returns the loader</returns>
        private XmlClassDefsLoader GetXmlClassDefsLoader()
        {
            try
            {
                string classDefsXml;
                if (String.IsNullOrEmpty(ClassDefsXml))
                    classDefsXml = new StreamReader(ClassDefsFileName).ReadToEnd();
                else
                    classDefsXml = ClassDefsXml;

                return new XmlClassDefsLoader(classDefsXml, new DtdLoader(), new DefClassFactory());
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Unable to find Class Definitions file. " +
                                                "This file contains all the class definitions that match " +
                                                "objects to database tables. Ensure that you have a classdefs.xml file " +
                                                "and that the file is being copied to your output directory (eg. bin/debug).",
                                                ex);
            }
        }
        #endregion
    }
}
