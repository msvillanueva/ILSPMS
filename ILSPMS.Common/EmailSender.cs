using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Common
{
    public class EmailSender
    {
        public EmailSender()
        {
            To = new List<string>();
            RecipientName = "";
        }

        private string br = "<br/>";

        public List<string> To { get; set; }
        public string RecipientName { get; set; }

        public void SendAcceptRegistrationAsync(string password)
        {
            var body = new StringBuilder();
            body.Append($"Dear {this.RecipientName}, {br}{br}");
            body.Append($"Welcome to ILS Project Monitoring System. {br}");
            body.Append($"Please use the following information when accessing your <a href='{ConfigurationManager.AppSettings["WebLink"]}'>web dashboard</a>. {br} {br}");
            body.Append($"Login = {this.To.First()} {br}");
            body.Append($"Password = {password} {br}{br}");
            body.Append($"Best regards, {br}");
            body.Append("The ILS Team");

            new Task(() => { Email.SendMail(this.To, Email.SenderName, Email.SenderEmail, "ILS-PMS new account", body.ToString(), true, true); }).Start();
        }
    }
}
