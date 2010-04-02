//	Author:				Joe Audette
//	Created:			2010-03-15
//	Last Modified:		2010-03-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Controls.google;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// This control is used in conjunction with AnalyticsAsyncBottomScript and in replacement for CGoogleAnalyticsScript
    /// if you want to use the async loading approach described here:
    /// http://code.google.com/apis/analytics/docs/tracking/asyncUsageGuide.html#SplitSnippet
    /// 
    /// Remove CGoogleAnalyticsScript from your layout.master and replace with AnalyticsAsyncBottomScript just before the closing form element
    /// Add AnalyticsAsyncTopScript to layout.master just below the opening body element
    public class AnalyticsAsyncTopScript : WebControl
    {
        private SiteSettings siteSettings = null;
        private string googleAnalyticsProfileId = string.Empty;
        private List<AnalyticsTransaction> transactions = new List<AnalyticsTransaction>();
        private string memberLabel = "member-type";
        private string memberType = "member";
        private string pageToTrack = string.Empty;
        private string overrideDomain = string.Empty;
        private bool logToLocalServer = false;

        /// <summary>
        /// Requires at least one item
        /// </summary>
        public List<AnalyticsTransaction> Transactions
        {
            get { return transactions; }
        }

        /// <summary>
        /// If true pageTracker._setLocalRemoteServerMode(); wil be called resulting in analytics data appearing in the web logs.
        /// This data can be used for additional offline processing using Urchin.
        /// </summary>
        public bool LogToLocalServer
        {
            get { return logToLocalServer; }
            set { logToLocalServer = value; }
        }

        /// <summary>
        /// If you want to ovveride how the page is tracked you can enter it here.
        /// Like in SearchResults.aspx we track /SearchResults.aspx?q=searchterm
        /// </summary>
        public string PageToTrack
        {
            get { return pageToTrack; }
            set { pageToTrack = value; }
        }

        /// <summary>
        /// If you specify an overridedomain, ._setDomainName(...) will be called
        /// http://code.google.com/apis/analytics/docs/tracking/gaTrackingSite.html
        /// </summary>
        public string OverrideDomain
        {
            get { return overrideDomain; }
            set { overrideDomain = value; }
        }

        private bool setAllowLinker = true;
        public bool SetAllowLinker
        {
            get { return setAllowLinker; }
            set { setAllowLinker = value; }
        }

        private bool setAllowHash = true;
        public bool SetAllowHash
        {
            get { return setAllowHash; }
            set { setAllowHash = value; }
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DoInit();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
           
        }

        

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (string.IsNullOrEmpty(googleAnalyticsProfileId)) { return; }

                writer.Write("<script type=\"text/javascript\"> ");
                writer.Write("\n");

                writer.Write("var _gaq = _gaq || []; ");
                writer.Write("\n");

                writer.Write("_gaq.push(['_setAccount','" + googleAnalyticsProfileId + "']); ");
                writer.Write("\n");


                if (overrideDomain.Length > 0)
                {
                    //http://code.google.com/apis/analytics/docs/tracking/gaTrackingSite.html

                    writer.Write("_gaq.push(['_setDomainName','" + overrideDomain + "']);");
                    

                    // default is false so we only have to set it if we want true
                    if (setAllowLinker)
                    {
                        writer.Write("_gaq.push(['_setAllowLinker',true]);");
                        
                    }

                    //default is true so we only have to set it if we want false
                    if (!setAllowHash)
                    {
                        writer.Write("_gaq.push(['_setAllowHash',false]);");
                        
                    }
                }

                if (logToLocalServer)
                {
                    writer.Write("_gaq.push(['_setLocalRemoteServerMode']);");
                    
                }

                SetupUserTracking(writer);

                if (pageToTrack.Length > 0)
                {
                    writer.Write(" _gaq.push(['_trackPageview','" + pageToTrack.Replace("'", string.Empty).Replace("\"", string.Empty) + "']); ");
                }
                else
                {
                    writer.Write(" _gaq.push(['_trackPageview']); ");
                }
                writer.Write("\n");

                writer.Write(" </script>");
            }
        }

        private void SetupUserTracking(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(memberLabel)) { return; }


            writer.Write("_gaq.push(['_setCustomVar', 1, '" + memberType + "', '" + memberLabel + "', 1]);");

        }

        private void SetupTransactions(HtmlTextWriter writer)
        {
            bool foundValidTransactions = false;

            foreach (AnalyticsTransaction transaction in transactions)
            {
                if (transaction.IsValid())
                {
                    foundValidTransactions = true;

                    writer.Write("_gaq.push(['_addTrans',");
                    writer.Write("'" + transaction.OrderId + "',");
                    writer.Write("'" + transaction.StoreName + "',");
                    writer.Write("\"" + transaction.Total + "\",");
                    writer.Write("\"" + transaction.Tax + "\",");
                    writer.Write("\"" + transaction.Shipping + "\",");
                    writer.Write("\"" + transaction.City + "\",");
                    writer.Write("\"" + transaction.State + "\",");
                    writer.Write("\"" + transaction.Country + "\"");
                    writer.Write("]);");
                    

                    SetupTransactionItems(writer, transaction);

                }
            }

            if (foundValidTransactions)
            {
                writer.Write("_gaq.push(['_trackTrans']);");
                writer.Write("_gaq.push(['_trackPageview','/TransactionComplete.aspx']);");
            }

        }

        private void SetupTransactionItems(HtmlTextWriter writer, AnalyticsTransaction transaction)
        {
            foreach (AnalyticsTransactionItem item in transaction.Items)
            {
                if (item.IsValid())
                {
                    writer.Write("_gaq.push(['_addItem',");
                    writer.Write("'" + item.OrderId + "',");
                    writer.Write("'" + item.Sku + "',");
                    writer.Write("'" + item.ProductName + "',");
                    writer.Write("'" + item.Category + "',");
                    writer.Write("'" + item.Price + "',");
                    writer.Write("'" + item.Quantity + "'");
                    writer.Write("]);");
                    
                }
            }

        }

        private void DoInit()
        {
            if (HttpContext.Current == null) { return; }

            this.EnableViewState = false;

            memberType = WebConfigSettings.GoogleAnalyticsMemberLabel;
            memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAnonymous;
            LogToLocalServer = WebConfigSettings.LogGoogleAnalyticsDataToLocalWebLog;


            // lets always label Admins as admins, regardless whether they are also customers
            if (WebUser.IsAdminOrContentAdmin)
            {
                memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAdmin;
            }
            else
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if ((siteUser != null) && (siteUser.TotalRevenue > 0))
                    {
                        memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeCustomer;
                    }
                    else
                    {
                        memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAuthenticated;
                    }

                }
            }

            
            // let Web.config setting trump site settings. this meets my needs where I want to track the demo site but am letting people login as admin
            // this way if the remove or change it in site settings it still uses my profile id
            if (ConfigurationSettings.AppSettings["GoogleAnalyticsProfileId"] != null)
            {
                googleAnalyticsProfileId = ConfigurationSettings.AppSettings["GoogleAnalyticsProfileId"].ToString();
                return;

            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null) && (siteSettings.GoogleAnalyticsAccountCode.Length > 0))
            {
                googleAnalyticsProfileId = siteSettings.GoogleAnalyticsAccountCode;

            }
        }

    }
}
