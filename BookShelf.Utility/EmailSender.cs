using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelf.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailtosend = new MimeMessage();
            emailtosend.From.Add(MailboxAddress.Parse("asharrasheed1997@gmail.com"));
            emailtosend.To.Add(MailboxAddress.Parse(email));
            emailtosend.Subject = subject;
            emailtosend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text=htmlMessage};
            //send email
            using (var emailClient= new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("asharrasheed1997@gmail.com", "qrxfbdylxiodvftg");
                emailClient.Send(emailtosend);
                emailClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
