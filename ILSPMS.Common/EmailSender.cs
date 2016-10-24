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
            body.Append($"Welcome to ILS Project Lifecycle Monitoring System. {br}");
            body.Append($"Please use the following information when accessing your <a href='{ConfigurationManager.AppSettings["WebLink"]}'>web dashboard</a>. {br} {br}");
            body.Append($"Login = {this.To.First()} {br}");
            body.Append($"Password = {password} {br}{br}");
            body.Append($"Best regards, {br}");
            body.Append("The ILS Team");

            new Task(() => { Email.SendMail(this.To, Email.SenderName, Email.SenderEmail, "ILS-PLMS new account", body.ToString(), true, true); }).Start();
        }

        public void SendForgotPasswordAsync(string password)
        {
            var body = new StringBuilder();
            body.Append($"Dear {this.RecipientName}, {br}{br}");
            body.Append($"You have requested for a password reset. {br}");
            body.Append($"Your new password is {password}. {br}{br}");
            body.Append($"Best regards, {br}");
            body.Append("The ILS Team");

            new Task(() => { Email.SendMail(this.To, Email.SenderName, Email.SenderEmail, "Password reset", body.ToString(), true, true); }).Start();
        }

        public void SendNewProject(string project)
        {
            var body = new StringBuilder();
            body.Append($"Dear {this.RecipientName}, {br}{br}");
            body.Append($"A new project has been assigned to you. {br}");
            body.Append($"Project name: {project}. {br}");
            body.Append($"Kindly login to the system to view the project. {br}{br}");
            body.Append($"Best regards, {br}");
            body.Append("The ILS Team");

            new Task(() => { Email.SendMail(this.To, Email.SenderName, Email.SenderEmail, "New project", body.ToString(), true, true); }).Start();
        }

        public void SendRequestForApproval(string pm, string project)
        {
            var body = new StringBuilder();
            body.Append($"Dear {this.RecipientName}, {br}{br}");
            body.Append($"{pm} has requested your approval for project {project}. {br}");
            body.Append($"Kindly login to the system to view the project. {br}{br}");
            body.Append($"Best regards, {br}");
            body.Append("The ILS Team");

            new Task(() => { Email.SendMail(this.To, Email.SenderName, Email.SenderEmail, "Approval request", body.ToString(), true, true); }).Start();
        }

        public void SendApproved(string approver, string project)
        {
            var body = new StringBuilder();
            body.Append($"Dear {this.RecipientName}, {br}{br}");
            body.Append($"Your request for approval in project {project} has been granted by {approver}. {br}{br}");
            body.Append($"Best regards, {br}");
            body.Append("The ILS Team");

            new Task(() => { Email.SendMail(this.To, Email.SenderName, Email.SenderEmail, "Approval granted", body.ToString(), true, true); }).Start();
        }

        public void SendDeclined(string approver, string project)
        {
            var body = new StringBuilder();
            body.Append($"Dear {this.RecipientName}, {br}{br}");
            body.Append($"Your request for approval in project {project} has been denied by {approver}. {br}{br}");
            body.Append($"Best regards, {br}");
            body.Append("The ILS Team");

            new Task(() => { Email.SendMail(this.To, Email.SenderName, Email.SenderEmail, "Approval declined", body.ToString(), true, true); }).Start();
        }
    }
}
