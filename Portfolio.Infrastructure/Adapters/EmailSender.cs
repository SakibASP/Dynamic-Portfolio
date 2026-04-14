using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Portfolio.Application.Common;

namespace Portfolio.Infrastructure.Adapters;

/// <summary>
/// SMTP adapter that implements the Identity <see cref="IEmailSender"/> abstraction.
/// Part of the Infrastructure layer — moved out of Portfolio.Web so the presentation
/// layer has no knowledge of SMTP transport.
/// </summary>
public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var smtpClient = new SmtpClient
        {
            EnableSsl = EmailSettings.UseSsl,
            Host = EmailSettings.ServerName,
            Port = EmailSettings.ServerPort,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.Password)
        };

        try
        {
            using var mailMessage = new MailMessage(EmailSettings.MailFromAddress, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", email);
        }
    }
}
