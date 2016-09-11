using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Common
{
    public static class Email
    {
        #region Parameters

        public static List<string> AdminEmails
        {
            get
            {
                string emails = ConfigurationManager.AppSettings["AdminEmail"];
                return emails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }
        public static string SenderName = ConfigurationManager.AppSettings["SenderName"];
        public static string SenderEmail = ConfigurationManager.AppSettings["SenderEmail"];

        #endregion

        /// <summary>
        /// Sends a mail to the user with the the subject and body specified with the sender with sender's display name 
        /// </summary>
        /// <param name="senderEmail"></param>
        /// <param name="subject">Mail subject(string)</param>
        /// <param name="body">Mail body(string)</param>
        /// <param name="receiverEmail"></param>
        /// <param name="senderDisplayName"></param>
        /// <param name="isBodyHTML"></param>
        /// <param name="isAsync"></param>
        public static void SendMail(/*string smtpHost, */List<string> receiverEmail, string senderDisplayName, string senderEmail, string subject, string body, bool isBodyHTML, bool isAsync)
        {
            SendMail(/*string smtpHost, */receiverEmail, senderDisplayName, senderEmail, subject, body, isBodyHTML, isAsync, null);
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="receiverEmail">Receiver email.</param>
        /// <param name="senderDisplayName"></param>
        /// <param name="senderEmail">Sender email.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        /// <param name="isBodyHTML"></param>
        /// <param name="isAsync"></param>
        /// <param name="bcc">BCC.</param>
        public static void SendMail(/*string smtpHost, */List<string> receiverEmail, string senderDisplayName, string senderEmail, string subject, string body, bool isBodyHTML, bool isAsync, List<string> bcc)
        {
            if (receiverEmail.Count > 0)
            {
                var smtpClient = new SmtpClient();
                var fromAddress = senderDisplayName == string.Empty ? new MailAddress(senderEmail) : new MailAddress(senderEmail, senderDisplayName);
                var message = new MailMessage { From = fromAddress };

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestMode"]))
                {
                    var copiedReceiverEmail = new List<string>(receiverEmail);
                    receiverEmail.Clear();
                    string testEmail = ConfigurationManager.AppSettings["TestEmail"];
                    receiverEmail = testEmail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string email in receiverEmail)
                        message.To.Add(new MailAddress(email));
                    body += "<p>Test Email..." + "<br>Recipients: " + string.Join(", ", copiedReceiverEmail.ToArray()) + (bcc != null ? "<br>BCC: " + string.Join(", ", bcc.ToArray()) : "") + "</p>";
                    message.Subject = "TEST: " + subject;
                }
                else
                {
                    foreach (string email in receiverEmail)
                        message.To.Add(new MailAddress(email));
                    message.Subject = subject;
                    if (bcc != null && bcc.Count > 0)
                    {
                        foreach (var email in bcc)
                            message.Bcc.Add(new MailAddress(email));
                    }
                }

                message.Body = FixMailString(body);
                message.IsBodyHtml = isBodyHTML;

                smtpClient.Host = ConfigurationManager.AppSettings["SMTPPath"];
                if (ConfigurationManager.AppSettings["SMTPPort"] != null)
                    smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                if (ConfigurationManager.AppSettings["SMTPUsername"] != null)
                    smtpClient.Credentials =
                        new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUsername"],
                                                         ConfigurationManager.AppSettings["SMTPPassword"]);
                if (isAsync)
                {
                    Object state = message;
                    smtpClient.SendCompleted += smtpClient_SendCompleted;
                    smtpClient.SendAsync(message, state);
                }
                else
                {
                    smtpClient.Send(message);
                }
            }
        }

        /// <summary>
        /// Sends email with attachment
        /// </summary>
        /// <param name="receiverEmail">Receiver email.</param>
        /// <param name="senderDisplayName"></param>
        /// <param name="senderEmail">Sender email.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        /// <param name="isBodyHTML"></param>
        /// <param name="isAsync"></param>
        /// <param name="bcc">BCC.</param>
        /// <param name="fileAttachments">fileAttachments.</param>
        public static void SendMail(List<string> receiverEmail, string senderDisplayName, string senderEmail, string subject, string body, bool isBodyHTML, bool isAsync, List<string> bcc, IEnumerable<string> fileAttachments)
        {
            if (receiverEmail.Count > 0)
            {
                var smtpClient = new SmtpClient();
                MailAddress fromAddress = senderDisplayName == string.Empty ? new MailAddress(senderEmail) : new MailAddress(senderEmail, senderDisplayName);
                var message = new MailMessage { From = fromAddress };

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestMode"]))
                {
                    var copiedReceiverEmail = new List<string>(receiverEmail);
                    string testEmail = ConfigurationManager.AppSettings["TestEmail"];
                    string[] testEmailList = testEmail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    body += "<p>Test Email..." + "<br>Recipients: " + string.Join(", ", copiedReceiverEmail.ToArray()) + (bcc != null ? "<br>BCC: " + string.Join(", ", bcc.ToArray()) : "") + "</p>";
                    receiverEmail.Clear();
                    foreach (string email in testEmailList)
                        message.To.Add(new MailAddress(email));
                    message.Subject = "TEST: " + subject;
                    if (bcc != null)
                    {
                        bcc.Clear();
                        foreach (string email in testEmailList)
                            bcc.Add(email);
                    }
                }
                else
                {
                    foreach (string email in receiverEmail)
                        message.To.Add(new MailAddress(email));
                    message.Subject = subject;
                    if (bcc != null && bcc.Count > 0)
                    {
                        foreach (var email in bcc)
                            message.Bcc.Add(new MailAddress(email));
                    }
                }

                message.Body = FixMailString(body);
                message.IsBodyHtml = isBodyHTML;

                //attach files
                foreach (var filename in fileAttachments)
                {
                    message.Attachments.Add(new Attachment(filename));
                }

                smtpClient.Host = ConfigurationManager.AppSettings["SMTPPath"];

                if (isAsync)
                {
                    Object state = message;
                    smtpClient.SendCompleted += smtpClient_SendCompleted;
                    smtpClient.SendAsync(message, state);
                }
                else
                {
                    smtpClient.Send(message);
                }
            }
        }

        static void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

            var mail = e.UserState as MailMessage;

            if (!e.Cancelled && e.Error != null)
            {
                //message.Text = "Mail sent successfully";
            }
        }

        /// <summary>
        /// Fix the string to qmail systems by ensuring that the caridge returns and linefeeds are correctly set
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string FixMailString(string body)
        {
            return body.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n") + "\r\n\r\n";
        }
    }
}
