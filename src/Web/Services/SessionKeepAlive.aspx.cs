using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;

namespace Cynthia.Web.Services
{
    public partial class SessionKeepAlive : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SessionKeepAlive));

        /// <summary>
        ///     This page does a simple job of keeping a session alive 
        ///     by calling this page from an iframe in edit pages, it will keep the seesion from
        ///     timing out before saving your changes
        ///     Just add
        ///     <portal:SessionKeepAliveControl id="ka1" runat="server" />
        ///     to you page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug("Requested SessionKeepAlive.aspx");
            }

            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) - 60));
            

        }
    }
}
