//  Author:                     Joe Audette
//  Created:                    2008-09-04
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
using System.Data;
using System.Globalization;
using System.Web;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Business.WebHelpers.UserRegisteredHandlers;
using Cynthia.Web.Framework;
using Cynthia.Net;
using log4net;
using Resources;


namespace Cynthia.Web
{
    /// <summary>
    ///  
    /// </summary>
    public class NotifyAdminUserRegisteredHandler : UserRegisteredHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(NotifyAdminUserRegisteredHandler));

        public NotifyAdminUserRegisteredHandler()
        { }

        public override void UserRegisteredHandler(object sender, UserRegisteredEventArgs e)
        {
            //if (sender == null) return;
            if (e == null) return;
            if (e.SiteUser == null) return;

            if (!WebConfigSettings.NotifyAdminsOnNewUserRegistration) { return; }

            log.Debug("NotifyAdminUserRegisteredHandler called for new user " + e.SiteUser.Email);

            if (HttpContext.Current == null) { return; }

            //lookup admin users and send notification email with link to manage user

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            CultureInfo defaultCulture = ResourceHelper.GetDefaultCulture();

            //Role adminRole = Role.GetRoleByName(siteSettings.SiteId, "Admins");

            //if (adminRole == null)
            //{
            //    // TODO: log it?
            //    return;
            //}

            //DataTable admins = SiteUser.GetRoleMembers(adminRole.RoleId);

            string subjectTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture,
                        "NotifyAdminofNewUserRegistationSubject.config");

            string textBodyTemplate = ResourceHelper.GetMessageTemplate(defaultCulture,
                        "NotifyAdminofNewUserRegistationMessage.config");

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();

            List<string> adminEmails = SiteUser.GetEmailAddresses(siteSettings.SiteId, "Admins;");

            //foreach (DataRow row in admins.Rows)
            foreach(string email in adminEmails)
            {
                if (WebConfigSettings.EmailAddressesToExcludeFromAdminNotifications.IndexOf(email, StringComparison.InvariantCultureIgnoreCase) > -1) { continue; }

                EmailMessageTask messageTask = new EmailMessageTask(smtpSettings);
                
                messageTask.EmailFrom = siteSettings.DefaultEmailFromAddress;
                //messageTask.EmailTo = row["Email"].ToString();
                messageTask.EmailTo = email;

                messageTask.Subject = string.Format(
                    defaultCulture,
                    subjectTemplate,
                    e.SiteUser.Email,
                    siteRoot
                    );


                string manageUserLink = siteRoot + "/Admin/ManageUsers.aspx?userid="
                    + e.SiteUser.UserId.ToString(CultureInfo.InvariantCulture);

                messageTask.TextBody = string.Format(
                    defaultCulture,
                    textBodyTemplate,
                    siteSettings.SiteName,
                    siteRoot,
                    manageUserLink
                    );

                messageTask.SiteGuid = siteSettings.SiteGuid;
                messageTask.QueueTask();

            }

            WebTaskManager.StartOrResumeTasks();


        }


    }
}
