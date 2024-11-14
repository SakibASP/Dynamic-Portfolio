using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Portfolio.Utils;

namespace Portfolio.Web.Common
{
    public class SendEmail : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var smtpClient = new SmtpClient();
            try
            {
                smtpClient.EnableSsl = EmailSettings.UseSsl;
                smtpClient.Host = EmailSettings.ServerName;
                smtpClient.Port = EmailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.Password);

                MailMessage mailMessage = new(EmailSettings.MailFromAddress, email)
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
