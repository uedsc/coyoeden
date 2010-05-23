using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Cynthia.Web.Framework;

namespace Cynthia.Net
{
    /// <summary>
    /// Author:				Joe Audette
    /// Created:			2008-05-19
    /// Last Modified:	    2008-09-12
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software. 
    /// </summary>
    public class GroupNotificationInfo
    {
        public GroupNotificationInfo()
        { }

        private string subjectTemplate = string.Empty;
        private string bodyTemplate = string.Empty;
        private string fromEmail = string.Empty;
        private string siteName = string.Empty;
        private string moduleName = string.Empty;
        private string forumName = string.Empty;
        private string subject = string.Empty;
        private DataSet subscribers = null;
        private string messageLink = string.Empty;
        private string unsubscribeGroupThreadLink = string.Empty;
        private string unsubscribeGroupLink = string.Empty;
        private SmtpSettings smtpSettings = null;

        public SmtpSettings SmtpSettings
        {
            get { return smtpSettings; }
            set { smtpSettings = value; }
        }

        public string SubjectTemplate
        {
            get { return subjectTemplate; }
            set { subjectTemplate = value; }
        }

        public string BodyTemplate
        {
            get { return bodyTemplate; }
            set { bodyTemplate = value; }
        }

        public string FromEmail
        {
            get { return fromEmail; }
            set { fromEmail = value; }
        }

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        public string GroupName
        {
            get { return forumName; }
            set { forumName = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public DataSet Subscribers
        {
            get { return subscribers; }
            set { subscribers = value; }
        }

        public string MessageLink
        {
            get { return messageLink; }
            set { messageLink = value; }
        }

        public string UnsubscribeGroupThreadLink
        {
            get { return unsubscribeGroupThreadLink; }
            set { unsubscribeGroupThreadLink = value; }
        }

        public string UnsubscribeGroupLink
        {
            get { return unsubscribeGroupLink; }
            set { unsubscribeGroupLink = value; }
        }
        
    }
}
