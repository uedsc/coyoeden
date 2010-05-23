/// Author:				Joe Audette
/// Created:			2004-10-13
/// Last Modified:	    2009-07-16
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Configuration;
using log4net;
using Cynthia.Web.Framework;

namespace Cynthia.Net
{
   
    /// <summary>
    /// this class is deprecated, better to create an EmailMessageTask messageTask = new EmailMessageTask(SiteUtils.GetSmtpSettings());
    /// </summary>
    public sealed class Notification
    {
        // Create a logger for use in this class
        private static readonly ILog log = LogManager.GetLogger(typeof(Notification));

        private Notification()
        {
            //only public static methods so no public constructor
        }

        public static void SendPassword(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string userEmail,
            string userPassword,
            string siteName,
            string siteLink)
        {

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{AdminEmail}", fromEmail);
            message.Replace("{UserEmail}", userEmail);
            message.Replace("{UserPassword}", userPassword);
            message.Replace("{SiteLink}", siteLink);

            Email.SendEmail(
                smtpSettings,
                fromEmail,
                userEmail, "", "",
                siteName,
                message.ToString(), false, "Normal");


        }

        public static void SendRegistrationConfirmationLink(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string userEmail,
            string siteName,
            string confirmationLink)
        {

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{AdminEmail}", fromEmail);
            message.Replace("{UserEmail}", userEmail);

            message.Replace("{ConfirmationLink}", confirmationLink);

            Email.SendEmail(
                smtpSettings,
                fromEmail,
                userEmail, "", "",
                siteName,
                message.ToString(), false, "Normal");


        }

        public static void SendGroupNotificationEmail(
            object oNotificationInfo)
        {
            if (oNotificationInfo == null) return;
            if (!(oNotificationInfo is GroupNotificationInfo)) return;

            GroupNotificationInfo notificationInfo = oNotificationInfo as GroupNotificationInfo;

            if (notificationInfo.Subscribers == null) return;

            if (log.IsDebugEnabled) log.Debug("In SendGroupNotificationEmail()");
            if (notificationInfo.Subscribers.Tables.Count > 0)
            {
                if (notificationInfo.Subscribers.Tables[0].Rows.Count > 0)
                {
                    int timeoutBetweenMessages = ConfigHelper.GetIntProperty("SmtpTimeoutBetweenMessages", 1000);

                    foreach (DataRow row in notificationInfo.Subscribers.Tables[0].Rows)
                    {
                        StringBuilder body = new StringBuilder();
                        body.Append(notificationInfo.BodyTemplate);
                        body.Replace("{SiteName}", notificationInfo.SiteName);
                        body.Replace("{ModuleName}", notificationInfo.ModuleName);
                        body.Replace("{GroupName}", notificationInfo.GroupName);
                        body.Replace("{AdminEmail}", notificationInfo.FromEmail);
                        body.Replace("{MessageLink}", notificationInfo.MessageLink);
                        body.Replace("{UnsubscribeGroupThreadLink}", notificationInfo.UnsubscribeGroupThreadLink);
                        body.Replace("{UnsubscribeGroupLink}", notificationInfo.UnsubscribeGroupLink);
                        StringBuilder emailSubject = new StringBuilder();
                        if (notificationInfo.SubjectTemplate.Length == 0)
                        {
                            notificationInfo.SubjectTemplate = "[{SiteName} - {GroupName}] {Subject}";
                        }
                        emailSubject.Append(notificationInfo.SubjectTemplate);
                        emailSubject.Replace("{SiteName}", notificationInfo.SiteName);
                        emailSubject.Replace("{ModuleName}", notificationInfo.ModuleName);
                        emailSubject.Replace("{GroupName}", notificationInfo.GroupName);
                        emailSubject.Replace("{Subject}", notificationInfo.Subject);

                        Email.SendEmail(
                            notificationInfo.SmtpSettings,
                            notificationInfo.FromEmail,
                            row["Email"].ToString(), "", "",
                            emailSubject.ToString(),
                            body.ToString(), false, "Normal");

                        Thread.Sleep(timeoutBetweenMessages);
                    }
                }
            }

        }


        //public static void SendGroupNotificationEmail(
        //    SmtpSettings smtpSettings,
        //    string subjectTemplate,
        //    string bodyTemplate,
        //    string fromEmail,
        //    string siteName,
        //    string moduleName,
        //    string forumName,
        //    string subject,
        //    DataSet subscribers,
        //    string messageLink,
        //    string unsubscribeGroupThreadLink,
        //    string unsubscribeGroupLink)
        //{
        //    if (log.IsDebugEnabled) log.Debug("In SendGroupNotificationEmail()");
        //    if (subscribers.Tables.Count > 0)
        //    {
        //        if (subscribers.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in subscribers.Tables[0].Rows)
        //            {
        //                StringBuilder body = new StringBuilder();
        //                body.Append(bodyTemplate);
        //                body.Replace("{SiteName}", siteName);
        //                body.Replace("{ModuleName}", moduleName);
        //                body.Replace("{GroupName}", forumName);
        //                body.Replace("{AdminEmail}", fromEmail);
        //                body.Replace("{MessageLink}", messageLink);
        //                body.Replace("{UnsubscribeGroupThreadLink}", unsubscribeGroupThreadLink);
        //                body.Replace("{UnsubscribeGroupLink}", unsubscribeGroupLink);
        //                StringBuilder emailSubject = new StringBuilder();
        //                if (subjectTemplate == null)
        //                {
        //                    subjectTemplate = "[{SiteName} - {GroupName}] {Subject}";
        //                }
        //                emailSubject.Append(subjectTemplate);
        //                emailSubject.Replace("{SiteName}", siteName);
        //                emailSubject.Replace("{ModuleName}", moduleName);
        //                emailSubject.Replace("{GroupName}", forumName);
        //                emailSubject.Replace("{Subject}", subject);

        //                Email.SendEmail(
        //                    smtpSettings,
        //                    fromEmail,
        //                    row["Email"].ToString(), "", "",
        //                    emailSubject.ToString(),
        //                    body.ToString(), false, "Normal");
        //            }
        //        }
        //    }


        //}

        public static void SendBlogCommentNotificationEmail(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string siteName,
            string authorEmail,
            string messageLink)
        {

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{MessageLink}", messageLink);


            Email.SendEmail(
                smtpSettings,
                fromEmail,
                authorEmail, "", "",
                siteName,
                message.ToString(), false, "Normal");



        }

        //public static void SendApprovalRequestRejectionNotificationEmail(
        //    SmtpSettings smtpSettings,
        //    string messageTemplate,
        //    string fromEmail,
        //    string siteName,
        //    string approvalRequestorEmail,
        //    string moduleTitle,
        //    DateTime approvalRequestedDate,
        //    string rejectionReason,
        //    string rejectedBy)
        //{
        //    StringBuilder message = new StringBuilder();
        //    message.Append(messageTemplate);
        //    message.Replace("{ModuleTitle}", moduleTitle);
        //    message.Replace("{ApprovalRequestedDate}", approvalRequestedDate.ToShortDateString());
        //    message.Replace("{RejectionReason}", rejectionReason);
        //    message.Replace("{RejectedBy}", rejectedBy);

        //    Email.SendEmail(
        //        smtpSettings,
        //        fromEmail,
        //        approvalRequestorEmail,
        //        "",
        //        "",
        //        siteName,
        //        message.ToString(), true, "Normal");

        //}


    }
}
