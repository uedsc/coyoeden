// Author:					Joe Audette
// Created:				    2008-03-06
// Last Modified:			2008-08-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Configuration;
using System.Globalization;
using Cynthia.Business;
using Cynthia.Business.WebHelpers.PaymentGateway;
using Cynthia.Web.Framework;

namespace Cynthia.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class CommerceConfiguration
    {
        #region Constants

        private const string Is503TaxExemptSetting = "Is503TaxExempt";
        private const string PaymentGatewayUseTestModeConfig = "PaymentGatewayUseTestMode";
        private const string PrimaryPaymentGatewayConfig = "PrimaryPaymentGateway";

        private const string AuthorizeNetProductionAPILoginConfig = "AuthorizeNetProductionAPILogin";
        private const string AuthorizeNetProductionAPITransactionKeyConfig = "AuthorizeNetProductionAPITransactionKey";

        private const string AuthorizeNetSandboxAPILoginConfig = "AuthorizeNetSandboxAPILogin";
        private const string AuthorizeNetSandboxAPITransactionKeyConfig = "AuthorizeNetSandboxAPITransactionKey";

        private const string PlugNPayProductionAPIPublisherNameConfig = "PlugNPayProductionAPIPublisherName";
        private const string PlugNPayProductionAPIPublisherPasswordConfig = "PlugNPayProductionAPIPublisherPassword";

        private const string PlugNPaySandboxAPIPublisherNameConfig = "PlugNPaySandboxAPIPublisherName";
        private const string PlugNPaySandboxAPIPublisherPasswordConfig = "PlugNPaySandboxAPIPublisherPassword";


        private const string PayPalUsePayPalStandardConfig = "PayPalUsePayPalStandard";
        private const string PayPalStandardProductionEmailConfig = "PayPalStandardProductionEmail";
        private const string PayPalStandardSandboxEmailConfig = "PayPalStandardSandboxEmail";
        private const string PayPalStandardProductionPDTConfig = "PayPalStandardProductionPDTId";
        private const string PayPalStandardSandboxPDTConfig = "PayPalStandardSandboxPDTId";

        private const string PayPalStandardProductionUrlConfig = "PayPalStandardProductionUrl";
        private const string PayPalStandardSandboxUrlConfig = "PayPalStandardSandboxUrl";

        private const string PayPalProductionAPIUsernameConfig = "PayPalProductionAPIUsername";
        private const string PayPalProductionAPIPasswordConfig = "PayPalProductionAPIPassword";
        private const string PayPalProductionAPISignatureConfig = "PayPalProductionAPISignature";

        private const string PayPalSandboxAPIUsernameConfig = "PayPalSandboxAPIUsername";
        private const string PayPalSandboxAPIPasswordConfig = "PayPalSandboxAPIPassword";
        private const string PayPalSandboxAPISignatureConfig = "PayPalSandboxAPISignature";
      
        private const string GoogleProductionMerchantIDConfig = "GoogleProductionMerchantID";
        private const string GoogleProductionMerchantKeyConfig = "GoogleProductionMerchantKey";

        private const string GoogleSandboxMerchantIDConfig = "GoogleSandboxMerchantID";
        private const string GoogleSandboxMerchantKeyConfig = "GoogleSandboxMerchantKey";

        #endregion

        #region Private Properties

        private bool is503TaxExempt = false;
        private string configPrefix = string.Empty;

        private bool paymentGatewayUseTestMode = true;

        private string primaryPaymentGateway = string.Empty;

        private string authorizeNetProductionAPILogin = string.Empty;
        private string authorizeNetProductionAPITransactionKey = string.Empty;

        private string authorizeNetSandboxAPILogin = string.Empty;
        private string authorizeNetSandboxAPITransactionKey = string.Empty;

        // Plug N Pay
        private string PlugNPayProductionAPIPublisherName = string.Empty;
        private string PlugNPayProductionAPIPublisherPassword = string.Empty;

        private string PlugNPaySandboxAPIPublisherName = string.Empty;
        private string PlugNPaySandboxAPIPublisherPassword = string.Empty;
        // end Plug N Pay



        // PayPal Pro
        private string payPalProductionAPIUsername = string.Empty;
        private string payPalProductionAPIPassword = string.Empty;
        private string payPalProductionAPISignature = string.Empty;

        private string payPalSandboxAPIUsername = string.Empty;
        private string payPalSandboxAPIPassword = string.Empty;
        private string payPalSandboxAPISignature = string.Empty;
        // end PayPal Pro

        //PayPal Standard
        private bool usePayPalStandard = false;
        private string payPalAccountProductionEmailAddress = string.Empty;
        private string payPalAccountProductionPDTId = string.Empty;
        private string payPalStandardProductionUrl = "https://www.paypal.com/cgi-bin/webscr";
        private string payPalAccountSandboxEmailAddress = string.Empty;
        private string payPalAccountSandboxPDTId = string.Empty;
        private string payPalStandardSandboxUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";

        // end PayPalStandard
        
        private string googleProductionMerchantID = string.Empty;
        private string googleProductionMerchantKey = string.Empty;

        private string googleSandboxMerchantID = string.Empty;
        private string googleSandboxMerchantKey = string.Empty;

        private int defaultTimeoutInMilliseconds = 30000; // 30 seconds

        private string defaultConfirmationEmailSubjectTemplate = "DefaultOrderConfirmationEmailSubjectTemplate.config";
        private string defaultConfirmationEmailTextBodyTemplate = "DefaultOrderConfirmationPlainTextEmailTemplate.config";

        

        #endregion

        #region Public Properties

        public bool Is503TaxExempt
        {
            get { return is503TaxExempt; }
        }

        public bool PaymentGatewayUseTestMode
        {
            get { return paymentGatewayUseTestMode; }
        }

        public string PrimaryPaymentGateway
        {
            get { return primaryPaymentGateway; }
        }

        public string AuthorizeNetAPILogin
        {
            get 
            {
                if (paymentGatewayUseTestMode)
                    return authorizeNetSandboxAPILogin;

                return authorizeNetProductionAPILogin; 
            }
        }

        public string AuthorizeNetAPITransactionKey
        {
            get 
            {
                if (paymentGatewayUseTestMode)
                    return authorizeNetSandboxAPITransactionKey;

                return authorizeNetProductionAPITransactionKey; 
            }
        }

        public string PlugNPayAPIPublisherName
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return PlugNPaySandboxAPIPublisherName;

                return PlugNPayProductionAPIPublisherName;
            }
        }

        public string PlugNPayAPIPublisherPassword
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return PlugNPaySandboxAPIPublisherPassword;

                return PlugNPayProductionAPIPublisherPassword;
            }
        }



        public bool IsConfigured
        {
            get
            {
                if (CanProcessStandardCards) { return true; }
                if (GoogleCheckoutIsEnabled) { return true; }
                if (PayPalIsEnabled) { return true; }

                return false;

            }
        }

        /// <summary>
        /// indicates whether we can process payments directly.
        /// true if using PayPalDirect or Authorize.NET for the primary gateway.
        /// False if not using one of these. If we process using PayPal Standard or Google Checkout,
        /// we are not processing cards directly, they are processed on external sites.
        /// </summary>
        public bool CanProcessStandardCards
        {
            get
            {
                if (this.PrimaryPaymentGateway == "PayPalDirect")
                {
                    if (PayPalAPIUsername.Length == 0) { return false; }

                    if (PayPalAPIPassword.Length == 0) { return false; }

                    if (PayPalAPISignature.Length == 0) { return false; }

                    return true;

                }

                if (this.PrimaryPaymentGateway == "Authorize.NET")
                {
                    
                    if (AuthorizeNetAPILogin.Length == 0) { return false; }

                    if (AuthorizeNetAPITransactionKey.Length == 0) { return false; }

                    return true;

                }

                if (this.PrimaryPaymentGateway == "PlugNPay")
                {

                    if (PlugNPayAPIPublisherName.Length == 0) { return false; }

                    if (PlugNPayAPIPublisherPassword.Length == 0) { return false; }

                    return true;

                }
                
                
                return false;
            }
        }

        public bool GoogleCheckoutIsEnabled
        {
            get
            {
                if (GoogleMerchantID.Length == 0) { return false; }

                if (GoogleMerchantKey.Length == 0) { return false; }

                return true;
            }
        }

        public bool PayPalIsEnabled
        {
            get 
            {
                if (!PayPalUsePayPalStandard)
                {
                    if (PayPalAPIUsername.Length == 0) { return false; }

                    if (PayPalAPIPassword.Length == 0) { return false; }

                    if (PayPalAPISignature.Length == 0) { return false; }
                }

                if ((PayPalUsePayPalStandard) && (PayPalStandardEmailAddress.Length == 0)) { return false; }


                return true; 
            }
        }

        public bool PayPalUsePayPalStandard
        {
            get { return usePayPalStandard; }
        }

        public string PayPalStandardEmailAddress
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalAccountSandboxEmailAddress;

                return payPalAccountProductionEmailAddress;
            }
        }

        public string PayPalStandardPDTId
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalAccountSandboxPDTId;

                return payPalAccountProductionPDTId;
            }
        }

        public string PayPalStandardUrl
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalStandardSandboxUrl;

                return payPalStandardProductionUrl;
            }
        }

        public string PayPalAPIUsername
        {
            get 
            { 
                if(paymentGatewayUseTestMode)
                    return payPalSandboxAPIUsername;

                return payPalProductionAPIUsername;
            }
        }

        public string PayPalAPIPassword
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalSandboxAPIPassword;

                return payPalProductionAPIPassword;
            }
        }

        public string PayPalAPISignature
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalSandboxAPISignature;

                return payPalProductionAPISignature;
            }
        }

        public GCheckout.EnvironmentType GoogleEnvironment
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return GCheckout.EnvironmentType.Sandbox;

                return GCheckout.EnvironmentType.Production;
            }
        }

        public string GoogleMerchantID
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return googleSandboxMerchantID;

                return googleProductionMerchantID;
            }
        }

        public string GoogleMerchantKey
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return googleSandboxMerchantKey;

                return googleProductionMerchantKey;
            }
        }

        public int DefaultTimeoutInMilliseconds
        {
            get { return defaultTimeoutInMilliseconds; }
            
        }

        public string DefaultConfirmationEmailSubjectTemplate
        {
            get { return defaultConfirmationEmailSubjectTemplate; }
        }

        public string DefaultConfirmationEmailTextBodyTemplate
        {
            get { return defaultConfirmationEmailTextBodyTemplate; }
        }

        #endregion

        #region Constructors

        

        public CommerceConfiguration(SiteSettings siteSettings)
        {
            configPrefix = "Site" 
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) 
                + "-";

            LoadConfigSettings();


        }

        #endregion

        #region Public Methods

         public IPaymentGateway GetDirectPaymentGateway()
        {
            if (!CanProcessStandardCards) { return null; }

            IPaymentGateway gateway = null;

            switch (PrimaryPaymentGateway)
            {
                case "PayPalDirect":
                    gateway = new PayPalDirectPaymentGateway(
                        PayPalAPIUsername,
                        PayPalAPIPassword,
                        PayPalAPISignature);

                    gateway.UseTestMode = PaymentGatewayUseTestMode;

                    return gateway;

                case "Authorize.NET":

                    gateway = new AuthorizeNETPaymentGateway(
                        AuthorizeNetAPILogin,
                        AuthorizeNetAPITransactionKey);

                    gateway.UseTestMode = PaymentGatewayUseTestMode;

                    return gateway;
  
                case "PlugNPay":

                    gateway = new PlugNPayPaymentGateway(
                        PlugNPayAPIPublisherName,
                        PlugNPayAPIPublisherPassword);

                    gateway.UseTestMode = PaymentGatewayUseTestMode;

                    return gateway;

                case "PayPalExpress":
                case "GoogleCheckout":
                default:

                    return gateway;

            }

            

        }



        #endregion

        private void LoadConfigSettings()
        {
            is503TaxExempt
                    = ConfigHelper.GetBoolProperty(configPrefix + Is503TaxExemptSetting,
                    is503TaxExempt);

            paymentGatewayUseTestMode
                    = ConfigHelper.GetBoolProperty(configPrefix + PaymentGatewayUseTestModeConfig,
                    paymentGatewayUseTestMode);

            if (ConfigurationManager.AppSettings[configPrefix + PrimaryPaymentGatewayConfig] != null)
            {
                primaryPaymentGateway = ConfigurationManager.AppSettings[configPrefix + PrimaryPaymentGatewayConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPILoginConfig] != null)
            {
                authorizeNetProductionAPILogin = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPILoginConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPITransactionKeyConfig] != null)
            {
                authorizeNetProductionAPITransactionKey = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPITransactionKeyConfig];

            }


            if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPILoginConfig] != null)
            {
                authorizeNetSandboxAPILogin = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPILoginConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPITransactionKeyConfig] != null)
            {
                authorizeNetSandboxAPITransactionKey = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPITransactionKeyConfig];

            }


            //Plug N Pay
            if (ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherNameConfig] != null)
            {
                PlugNPayProductionAPIPublisherName = ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherNameConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherPasswordConfig] != null)
            {
                PlugNPayProductionAPIPublisherPassword = ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherPasswordConfig];

            }


            if (ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherNameConfig] != null)
            {
                PlugNPaySandboxAPIPublisherName = ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherNameConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherPasswordConfig] != null)
            {
                PlugNPaySandboxAPIPublisherPassword = ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherPasswordConfig];

            }


            // paypal standard
            if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionEmailConfig] != null)
            {
                payPalAccountProductionEmailAddress = ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionEmailConfig];

            }


            if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionPDTConfig] != null)
            {
                payPalAccountProductionPDTId = ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionPDTConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxPDTConfig] != null)
            {
                payPalAccountSandboxPDTId = ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxPDTConfig];

            }


            usePayPalStandard = ConfigHelper.GetBoolProperty(configPrefix + PayPalUsePayPalStandardConfig, usePayPalStandard);

            if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionUrlConfig] != null)
            {
                payPalStandardProductionUrl = ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionUrlConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxEmailConfig] != null)
            {
                payPalAccountSandboxEmailAddress = ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxEmailConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxUrlConfig] != null)
            {
                payPalStandardSandboxUrl = ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxUrlConfig];

            }


            if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIUsernameConfig] != null)
            {
                payPalProductionAPIUsername = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIUsernameConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIPasswordConfig] != null)
            {
                payPalProductionAPIPassword = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIPasswordConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPISignatureConfig] != null)
            {
                payPalProductionAPISignature = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPISignatureConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIUsernameConfig] != null)
            {
                payPalSandboxAPIUsername = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIUsernameConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIPasswordConfig] != null)
            {
                payPalSandboxAPIPassword = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIPasswordConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPISignatureConfig] != null)
            {
                payPalSandboxAPISignature = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPISignatureConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantIDConfig] != null)
            {
                googleProductionMerchantID = ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantIDConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantKeyConfig] != null)
            {
                googleProductionMerchantKey = ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantKeyConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantIDConfig] != null)
            {
                googleSandboxMerchantID = ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantIDConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantKeyConfig] != null)
            {
                googleSandboxMerchantKey = ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantKeyConfig];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailPlainTextTemplate"] != null)
            {
                defaultConfirmationEmailTextBodyTemplate = ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailPlainTextTemplate"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailSubjectTemplate"] != null)
            {
                defaultConfirmationEmailSubjectTemplate = ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailSubjectTemplate"];

            }



        }




        

        //public static string GetPaymentGatewayTestModeKey(SiteSettings siteSettings)
        //{
        //    return "Site" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
        //        + "-" + PaymentGatewayUseTestModeConfig;
        //}

    }
}
