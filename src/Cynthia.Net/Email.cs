/// Author:				Joe Audette
/// Created:			2004-08-14
/// Last Modified:		2010-02-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using DotNetOpenMail;
using DotNetOpenMail.SmtpAuth;
using log4net;
using Cynthia.Web.Framework;

namespace Cynthia.Net
{
    /// <summary>
    /// A class for sending email.
    /// </summary>
    public static class Email
    {
       
        private static readonly ILog log = LogManager.GetLogger(typeof(Email));

        public const string PriorityLow = "Low";
        public const string PriorityNormal = "Normal";
        public const string PriorityHigh = "High";

        const int SmtpAuthenticated = 1;

        /// <summary>
        /// This method uses DotNetOpenMail. DotNetOpenMail doesn't work in Medium Trust.
        /// For Medium Trust use the SendMailNormal method.
        /// If Web.config setting RunningInMediumTrust is true then this method delegates to SendMailNormal.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        public static void SendEmail(
            SmtpSettings smtpSettings,
            string from,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null)&&(ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            if ((smtpSettings == null) || (!smtpSettings.IsValid))
            {
                log.Error("Invalid smtp settings detected in SendEmail ");
                return;
            }

            // DotNetOpenMail doesn't work in Medium Trust so fall back to built in System.Net classes
            if (ConfigurationManager.AppSettings["RunningInMediumTrust"] != null)
            {
                string runningInMediumTrust = ConfigurationManager.AppSettings["RunningInMediumTrust"];
                if (string.Equals(runningInMediumTrust, "true", StringComparison.InvariantCultureIgnoreCase))
                {
                    SendEmailNormal(smtpSettings, from, to, cc, bcc, subject, messageBody, html, priority);
                    return;
                }
            }

            if (ConfigurationManager.AppSettings["DisableDotNetOpenMail"] != null)
            {
                string disable = ConfigurationManager.AppSettings["DisableDotNetOpenMail"];
                if (string.Equals(disable, "true", StringComparison.InvariantCultureIgnoreCase))
                {
                    SendEmailNormal(smtpSettings, from, to, cc, bcc, subject, messageBody, html, priority);
                    return;
                }
            }

            if (log.IsDebugEnabled) log.DebugFormat("In SendEmail({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                                                    from,
                                                    to,
                                                    cc,
                                                    bcc,
                                                    subject,
                                                    messageBody,
                                                    html,
                                                    priority);

            EmailMessage mail = new EmailMessage();
            EmailAddress fromAddress = new EmailAddress(from);
            mail.FromAddress = fromAddress;

            if (smtpSettings.PreferredEncoding.Length > 0)
            {
                mail.HeaderEncoding = DotNetOpenMail.Encoding.EncodingType.Base64;
                mail.HeaderCharSet = Encoding.GetEncoding(smtpSettings.PreferredEncoding);
            }
            
            if (html)
            {
                mail.HtmlPart = new HtmlAttachment(messageBody);

                if (smtpSettings.PreferredEncoding.Length > 0)
                {
                    mail.HtmlPart.Encoding = DotNetOpenMail.Encoding.EncodingType.Base64;
                    mail.HtmlPart.CharSet = Encoding.GetEncoding(smtpSettings.PreferredEncoding);
                }
            }
            else
            {
                mail.TextPart = new TextAttachment(messageBody);
                if (smtpSettings.PreferredEncoding.Length > 0)
                {
                    mail.TextPart.Encoding = DotNetOpenMail.Encoding.EncodingType.Base64;
                    mail.TextPart.CharSet = Encoding.GetEncoding(smtpSettings.PreferredEncoding);
                }
            }

            EmailAddress toAddress;

            if (to.Contains(";"))
            {
                string[] toAddresses = to.Split(new char[] { ';' });
                foreach (string address in toAddresses)
                {
                    if (Email.IsValidEmailAddressSyntax(address))
                    {
                        toAddress = new EmailAddress(address);

                        mail.ToAddresses.Add(toAddress);
                    }
                }
            }
            else
            {
                if (Email.IsValidEmailAddressSyntax(to))
                {
                    toAddress = new EmailAddress(to);
                    mail.ToAddresses.Add(toAddress);
                }
            }

            if (mail.ToAddresses.Count == 0) throw new MailException("no valid email address provided for To");

            if (cc.Length > 0)
            {
                EmailAddress ccAddress = new EmailAddress(cc);
                mail.CcAddresses.Add(ccAddress);
            }
            if (bcc.Length > 0)
            {
                EmailAddress bccAddress = new EmailAddress(bcc);
                mail.BccAddresses.Add(bccAddress);
            }
            mail.Subject = subject.RemoveLineBreaks();

            switch (priority)
            {
                //X-Priority Values: 
                // 1 (Highest), 2 (High), 3 (Normal), 4 (Low), 5 (Lowest) 
                case PriorityHigh:
                    mail.AddCustomHeader("X-Priority", "1");
                    mail.AddCustomHeader("X-MSMail-Priority", "High");
                    break;

                case PriorityLow:
                    mail.AddCustomHeader("X-Priority", "5");
                    mail.AddCustomHeader("X-MSMail-Priority", "Low");
                    break;

                case PriorityNormal:
                default:
                    mail.AddCustomHeader("X-Priority", "3");
                    mail.AddCustomHeader("X-MSMail-Priority", "Normal");
                    break;
            }

            int timeoutMilliseconds = ConfigHelper.GetIntProperty("SMTPTimeoutInMilliseconds", 15000);

            SmtpServer smtpServer = new SmtpServer(smtpSettings.Server, smtpSettings.Port, smtpSettings.UseSsl);
            smtpServer.ServerTimeout = timeoutMilliseconds;

            if (smtpSettings.RequiresAuthentication)
            {
                smtpServer.SmtpAuthToken
                    = new SmtpAuthToken(
                    smtpSettings.User,
                    smtpSettings.Password);
            }

            try
            {
                mail.Send(smtpServer);
            }
            catch (MailException ex)
            {
                log.Error("error sending email to " + to + " from " + from + ", message was: " + messageBody, ex);
            }
            catch (SocketException ex)
            {
                log.Error("error sending email to " + to + " from " + from + ", message was: " + messageBody, ex);
            }

        }

        /// <summary>
        /// This method uses DotNetOpenMail. DotNetOpenMail doesn't work in Medium Trust.
        /// For Medium Trust use the SendMailNormal method.
        /// If Web.config setting RunningInMediumTrust is true then this method delegates to SendMailNormal.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="replyTo"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        public static void SendEmail(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            if ((smtpSettings == null) || (!smtpSettings.IsValid))
            {
                log.Error("Invalid smtp settings detected in SendEmail ");
                return;
            }

            // DotNetOpenMail doesn't work in Medium Trust so fall back to built in System.Net classes
            if (ConfigurationManager.AppSettings["RunningInMediumTrust"] != null)
            {
                string runningInMediumTrust = ConfigurationManager.AppSettings["RunningInMediumTrust"];
                if (string.Equals(runningInMediumTrust, "true", StringComparison.InvariantCultureIgnoreCase))
                {
                    SendEmailNormal(smtpSettings, from, to, cc, bcc, subject, messageBody, html, priority);
                    return;
                }
            }

            // DotNetOpenMail doesn't support replyto
            if (replyTo.Length > 0)
            {
                SendEmailNormal(
                    smtpSettings,
                    from,
                    replyTo,
                    to,
                    cc,
                    bcc,
                    subject,
                    messageBody,
                    html,
                    priority);

                return;
            }

            SendEmail(
                smtpSettings,
                from,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority);

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        public static void SendEmailNormal(
            SmtpSettings smtpSettings,
            string from,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            string replyTo = string.Empty;
            SendEmailNormal(
                smtpSettings,
                from,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority);

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="replyTo"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        public static void SendEmailNormal(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            string[] attachmentPaths = new string[0];
            string[] attachmentNames = new string[0];

            SendEmailNormal(
                smtpSettings,
                from,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority,
                attachmentPaths,
                attachmentNames);

            

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="replyTo"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        /// <param name="attachmentPaths"></param>
        /// <param name="attachmentNames"></param>
        public static void SendEmailNormal(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority,
            string[] attachmentPaths,
            string[] attachmentNames)
        {
            Send(smtpSettings,
                from,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority,
                attachmentPaths,
                attachmentNames);

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="replyTo"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        /// <param name="attachmentPaths"></param>
        /// <param name="attachmentNames"></param>
        public static bool Send(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority,
            string[] attachmentPaths,
            string[] attachmentNames)
        {
            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return false;
            }

            if ((smtpSettings == null) || (!smtpSettings.IsValid))
            {
                log.Error("Invalid smtp settings detected in SendEmail ");
                return false;
            }

            if (log.IsDebugEnabled) log.DebugFormat("In SendEmailNormal({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                                                    from,
                                                    to,
                                                    cc,
                                                    bcc,
                                                    subject,
                                                    messageBody,
                                                    html,
                                                    priority);

            using (MailMessage mail = new MailMessage())
            {
                if (smtpSettings.PreferredEncoding.Length > 0)
                {
                    switch (smtpSettings.PreferredEncoding)
                    {
                        case "ascii":
                        case "us-ascii":
                            // do nothing since this is the default
                            break;

                        case "utf32":
                        case "utf-32":

                            mail.BodyEncoding = Encoding.UTF32;
                            mail.SubjectEncoding = Encoding.UTF32;

                            break;

                        case "unicode":

                            mail.BodyEncoding = Encoding.Unicode;
                            mail.SubjectEncoding = Encoding.Unicode;

                            break;

                        case "utf8":
                        case "utf-8":
                        default:

                            mail.BodyEncoding = Encoding.UTF8;
                            mail.SubjectEncoding = Encoding.UTF8;

                            break;
                    }

                }

                MailAddress fromAddress;
                try
                {
                    fromAddress = new MailAddress(from);
                }
                catch (ArgumentException)
                {
                    log.Error("invalid from address " + from);
                    log.Info("no valid from address was provided so not sending message " + messageBody);
                    return false;
                }
                catch (FormatException)
                {
                    log.Error("invalid from address " + from);
                    log.Info("no valid from address was provided so not sending message " + messageBody);
                    return false;
                }

                mail.From = fromAddress;

                List<string> toAddresses = to.Replace(";", ",").SplitOnChar(',');
                foreach (string toAddress in toAddresses)
                {
                    try
                    {
                        MailAddress a = new MailAddress(toAddress);
                        mail.To.Add(a);
                    }
                    catch (ArgumentException)
                    {
                        log.Error("ignoring invalid to address " + toAddress);
                    }
                    catch (FormatException)
                    {
                        log.Error("ignoring invalid to address " + toAddress);
                    }

                }

                if (mail.To.Count == 0)
                {
                    log.Error("no valid to address was provided so not sending message " + messageBody);
                    return false;
                }

                if (replyTo.Length > 0)
                {
                    try
                    {
                        MailAddress replyAddress = new MailAddress(replyTo);
                        mail.ReplyTo = replyAddress;
                    }
                    catch (ArgumentException)
                    {
                        log.Error("ignoring invalid replyto address " + replyTo);
                    }
                    catch (FormatException)
                    {
                        log.Error("ignoring invalid replyto address " + replyTo);
                    }
                }

                if (cc.Length > 0)
                {
                    List<string> ccAddresses = cc.Replace(";", ",").SplitOnChar(',');

                    foreach (string ccAddress in ccAddresses)
                    {
                        try
                        {
                            MailAddress a = new MailAddress(ccAddress);
                            mail.CC.Add(a);
                        }
                        catch (ArgumentException)
                        {
                            log.Error("ignoring invalid cc address " + ccAddress);
                        }
                        catch (FormatException)
                        {
                            log.Error("ignoring invalid cc address " + ccAddress);
                        }
                    }

                }

                if (bcc.Length > 0)
                {
                    List<string> bccAddresses = bcc.Replace(";", ",").SplitOnChar(',');

                    foreach (string bccAddress in bccAddresses)
                    {
                        try
                        {
                            MailAddress a = new MailAddress(bccAddress);
                            mail.Bcc.Add(a);
                        }
                        catch (ArgumentException)
                        {
                            log.Error("invalid bcc address " + bccAddress);
                        }
                        catch (FormatException)
                        {
                            log.Error("invalid bcc address " + bccAddress);
                        }
                    }

                }

                mail.Subject = subject.RemoveLineBreaks();

                switch (priority)
                {
                    case PriorityHigh:
                        mail.Priority = MailPriority.High;
                        break;

                    case PriorityLow:
                        mail.Priority = MailPriority.Low;
                        break;

                    case PriorityNormal:
                    default:
                        mail.Priority = MailPriority.Normal;
                        break;

                }

                

                if (html)
                {
                    mail.IsBodyHtml = true;
                    // this char can reportedly cause problems in some email clients so replace it if it exists
                    mail.Body = messageBody.Replace("\xA0", "&nbsp;");
                }
                else
                {
                    mail.Body = messageBody;
                }

                // add attachments if there are any
                if ((attachmentPaths.Length > 0) && (attachmentNames.Length == attachmentPaths.Length))
                {
                    for (int i = 0; i < attachmentPaths.Length; i++)
                    {
                        if (!File.Exists(attachmentPaths[i]))
                        {
                            log.Error("could not find file for email attachment " + attachmentPaths[i]);
                            continue;
                        }

                        Attachment a = new Attachment(attachmentPaths[i]);
                        a.Name = attachmentNames[i];
                        mail.Attachments.Add(a);

                    }

                }

                int timeoutMilliseconds = ConfigHelper.GetIntProperty("SMTPTimeoutInMilliseconds", 15000);
                SmtpClient smtpClient = new SmtpClient(smtpSettings.Server, smtpSettings.Port);
                smtpClient.EnableSsl = smtpSettings.UseSsl;
                smtpClient.Timeout = timeoutMilliseconds;

                if (smtpSettings.RequiresAuthentication)
                {

                    NetworkCredential smtpCredential
                        = new NetworkCredential(
                            smtpSettings.User,
                            smtpSettings.Password);

                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(smtpSettings.Server, smtpSettings.Port, "LOGIN", smtpCredential);

                    smtpClient.Credentials = myCache;
                }
                else
                {
                    //aded 2010-01-22 JA
                    smtpClient.UseDefaultCredentials = true;
                }


                try
                {
                    smtpClient.Send(mail);
                    //log.Debug("Sent Message: " + subject);
                    //log.Info("Sent Message: " + subject);
                    return true;
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    //log.Error("error sending email to " + to + " from " + from, ex);
                    log.Error("error sending email to " + mail.To.ToString() + " from " + mail.From.ToString() + ", will retry");
                    return RetrySend(mail, smtpClient, ex);

                }
                catch (WebException ex)
                {
                    log.Error("error sending email to " + to + " from " + from + ", message was: " + messageBody, ex);
                    return false;
                }
                catch (SocketException ex)
                {
                    log.Error("error sending email to " + to + " from " + from + ", message was: " + messageBody, ex);
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    log.Error("error sending email to " + to + " from " + from + ", message was: " + messageBody, ex);
                    return false;
                }
                catch (FormatException ex)
                {
                    log.Error("error sending email to " + to + " from " + from + ", message was: " + messageBody, ex);
                    return false;
                }

            }// end using MailMessage

        }

        private static bool RetrySend(MailMessage message, SmtpClient smtp, Exception ex)
        {
            //retry
            int timesToRetry = ConfigHelper.GetIntProperty("TimesToRetryOnSmtpError", 3);
            for (int i = 1; i <= timesToRetry; )
            {
                if (RetrySend(message, smtp, i)) { return true; }
                i += 1;
                Thread.Sleep(1000); // 1 second sleep in case it is a temporary network issue
            }

            // allows use of localhost as  backup 
            if (ConfigurationManager.AppSettings["BackupSmtpServer"] != null)
            {
                string backupServer = ConfigurationManager.AppSettings["BackupSmtpServer"];
                int timeoutMilliseconds = ConfigHelper.GetIntProperty("SMTPTimeoutInMilliseconds", 15000);
                int backupSmtpPort = ConfigHelper.GetIntProperty("BackupSmtpPort", 25);
                SmtpClient smtpClient = new SmtpClient(backupServer, backupSmtpPort);
                smtpClient.UseDefaultCredentials = true;

                try
                {
                    smtpClient.Send(message);
                    log.Info("success using backup smtp server sending email to " + message.To.ToString() + " from " + message.From);
                    return true;
                }
                catch (System.Net.Mail.SmtpException) { }
                catch (WebException) { }
                catch (SocketException) { }
                catch (InvalidOperationException) { }
                catch (FormatException) { }

            }

            //log.Info("all retries failed sending email to " + message.To.ToString() + " from " + message.From);
            log.Error("all retries failed sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);

            return false;

        }

        private static bool RetrySend(MailMessage message, SmtpClient smtp, int tryNumber)
        {
            try
            {
                smtp.Send(message);
                log.Info("success on retry " + tryNumber.ToInvariantString() + " sending email to " + message.To.ToString() + " from " + message.From);
                return true;
            }
            catch (System.Net.Mail.SmtpException) { }
            catch (WebException) { }
            catch (SocketException) { }
            catch (InvalidOperationException) { }
            catch (FormatException) { }

            return false;
        }

        public static bool IsValidEmailAddressSyntax(string emailAddress)
        {
            Regex emailPattern;
            //emailPattern = new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
            emailPattern = new Regex(SecurityHelper.RegexEmailValidationPattern);

            Match emailAddressToValidate = emailPattern.Match(emailAddress);

            if (emailAddressToValidate.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
