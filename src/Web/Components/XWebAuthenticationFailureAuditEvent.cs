using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Management;

namespace Cynthia.Web
{
    public class CWebAuthenticationFailureAuditEvent : WebAuthenticationFailureAuditEvent
    {
        private string customCreatedMsg, customRaisedMsg;

        public CWebAuthenticationFailureAuditEvent(
            string msg, 
            object eventSource,
            int eventCode, 
            string userName)
            :
        base(msg, eventSource, eventCode, userName)
        {
            customCreatedMsg =
                string.Format(CultureInfo.InvariantCulture,"Event created at: {0}",
                DateTime.Now.TimeOfDay.ToString());
        }

        public CWebAuthenticationFailureAuditEvent(
            string msg, 
            object eventSource,
            int eventCode, 
            int detailedCode, 
            string userName)
            :
        base(msg, eventSource, eventCode, detailedCode, userName)
        {
            customCreatedMsg =
            string.Format(CultureInfo.InvariantCulture,"Event created at: {0}",
                DateTime.Now.TimeOfDay.ToString());
        }

        public override void Raise()
        {
            
            customRaisedMsg =
                string.Format(CultureInfo.InvariantCulture,"Event raised at: {0}",
                DateTime.Now.TimeOfDay.ToString());

            WebBaseEvent.Raise(this);
        }

        public WebRequestInformation GetRequestInformation()
        {
            return RequestInformation;
        }

        public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            base.FormatCustomEventDetails(formatter);

            formatter.AppendLine("");

            formatter.IndentationLevel += 1;
            formatter.AppendLine(
                "* CWebAuthenticationFailureAuditEvent Start *");
            formatter.AppendLine(string.Format("Request path: {0}",
                RequestInformation.RequestPath));
            formatter.AppendLine(string.Format("Request Url: {0}",
                RequestInformation.RequestUrl));

            formatter.AppendLine(customCreatedMsg);
            formatter.AppendLine(customRaisedMsg);

            formatter.AppendLine(
                "* CWebAuthenticationFailureAuditEvent End *");

            formatter.IndentationLevel -= 1;

        }
    }

}