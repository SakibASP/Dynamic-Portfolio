using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Portfolio.Utils;

namespace Portfolio.Web.Common
{
    public class SendEmail(EmailSettings emailSettings) : IEmailSender
    {
        private readonly EmailSettings _emailSettings = emailSettings;

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.EnableSsl = _emailSettings.UseSsl;
                    smtpClient.Host = _emailSettings.ServerName;
                    smtpClient.Port = _emailSettings.ServerPort;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    MailMessage mailMessage = new(_emailSettings.MailFromAddress, //From
                                                email, //To
                                                subject, //Subject
                                                htmlMessage // Body
                                                );
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return Task.CompletedTask;
            }
        }

    }
}
