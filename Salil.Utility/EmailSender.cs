using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //to wokr with from/to/subject/body
            var emailSender = new MimeMessage();
            emailSender.From.Add(MailboxAddress.Parse("Parihar@akouna.com"));
            emailSender.To.Add(MailboxAddress.Parse(email));
            emailSender.Subject = subject;
            emailSender.Body= new TextPart(MimeKit.Text.TextFormat.Html) { Text=htmlMessage};

            // now send email
            using (var smtpClient= new SmtpClient()) 
            {
                smtpClient.Connect("smtp.gmail.com",587, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate("akouanparihar@gmail.com", "jjplabcdkmobvlqp");
                smtpClient.Send(emailSender);
                smtpClient.Disconnect(true);
            }

                return Task.CompletedTask;
        }
    }
}
