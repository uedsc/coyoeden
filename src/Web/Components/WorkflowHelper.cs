//  Author:                     Joe Audette
//  Created:                    2009-07-22
//	Last Modified:              2009-10-17
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
using System.Globalization;
using System.Text;
using System.Web;
using log4net;
using Cynthia.Business;
using Cynthia.Net;
using Cynthia.Web.Framework;


namespace Cynthia.Web
{
    public static class WorkflowHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WorkflowHelper));


        public static void SendApprovalRequestNotification(
            SmtpSettings smtpSettings,
            SiteSettings siteSettings,
            SiteUser submittingUser,
            ContentWorkflow draftWorkflow,
            string approvalRoles,
            string contentUrl
            )
        {
            if (string.IsNullOrEmpty(approvalRoles)) { approvalRoles = "Admins;Content Administrators;Content Publishers;"; }

            List<string> emailAddresses = SiteUser.GetEmailAddresses(siteSettings.SiteId, approvalRoles);

            int queuedMessageCount = 0;

            CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();
            string messageTemplate = ResourceHelper.GetMessageTemplate(defaultCulture, "ApprovalRequestNotification.config");
            string messageSubject = ResourceHelper.GetMessageTemplate(defaultCulture, "ApprovalRequestNotificationSubject.config").Replace("{SiteName}", siteSettings.SiteName);

            foreach (string email in emailAddresses)
            {
                if (WebConfigSettings.EmailAddressesToExcludeFromAdminNotifications.IndexOf(email, StringComparison.InvariantCultureIgnoreCase) > -1) { continue; }

                if (!Email.IsValidEmailAddressSyntax(email)) { continue; }

                EmailMessageTask messageTask = new EmailMessageTask(smtpSettings);
                messageTask.SiteGuid = siteSettings.SiteGuid;
                messageTask.EmailFrom = siteSettings.DefaultEmailFromAddress;
                messageTask.EmailReplyTo = submittingUser.Email;
                messageTask.EmailTo = email;

                messageTask.Subject = messageSubject;

                StringBuilder message = new StringBuilder();
                message.Append(messageTemplate);
                message.Replace("{ModuleTitle}", draftWorkflow.ModuleTitle);
                message.Replace("{ApprovalRequestedDate}", draftWorkflow.RecentActionOn.ToShortDateString());
                message.Replace("{SubmittedBy}", submittingUser.Name);
                message.Replace("{ContentUrl}", contentUrl);

                if (!Email.IsValidEmailAddressSyntax(draftWorkflow.RecentActionByUserEmail))
                {
                    //invalid address log it
                    log.Error("Failed to send workflow rejection message, invalid recipient email "
                        + draftWorkflow.RecentActionByUserEmail
                        + " message was " + message.ToString());

                    return;

                }

                messageTask.TextBody = message.ToString();
                messageTask.QueueTask();
                queuedMessageCount += 1;

            }

            if (queuedMessageCount > 0) { WebTaskManager.StartOrResumeTasks(); }

        }

        
        

        public static void SendRejectionNotification(
            SmtpSettings smtpSettings,
            SiteSettings siteSettings, 
            SiteUser rejectingUser, 
            ContentWorkflow rejectedWorkflow,
            string rejectionReason
            )
        {

            EmailMessageTask messageTask = new EmailMessageTask(smtpSettings);
            messageTask.SiteGuid = siteSettings.SiteGuid;
            messageTask.EmailFrom = siteSettings.DefaultEmailFromAddress;
            messageTask.EmailReplyTo = rejectingUser.Email;
            messageTask.EmailTo = rejectedWorkflow.RecentActionByUserEmail;

            CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();
           
            messageTask.Subject = ResourceHelper.GetMessageTemplate(defaultCulture, "ApprovalRequestRejectionNotificationSubject.config").Replace("{SiteName}", siteSettings.SiteName);

            StringBuilder message = new StringBuilder();
            message.Append(ResourceHelper.GetMessageTemplate(defaultCulture, "ApprovalRequestRejectionNotification.config"));
            message.Replace("{ModuleTitle}", rejectedWorkflow.ModuleTitle);
            message.Replace("{ApprovalRequestedDate}", rejectedWorkflow.RecentActionOn.ToShortDateString());
            message.Replace("{RejectionReason}", rejectionReason);
            message.Replace("{RejectedBy}", rejectingUser.Name);

            if (!Email.IsValidEmailAddressSyntax(rejectedWorkflow.RecentActionByUserEmail))
            {
                //invalid address log it
                log.Error("Failed to send workflow rejection message, invalid recipient email "
                    + rejectedWorkflow.RecentActionByUserEmail
                    + " message was " + message.ToString());

                return;

            }

            messageTask.TextBody = message.ToString();
            messageTask.QueueTask();

            WebTaskManager.StartOrResumeTasks();

        }

    }
}
