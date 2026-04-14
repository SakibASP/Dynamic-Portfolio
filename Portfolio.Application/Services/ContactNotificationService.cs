using Microsoft.AspNetCore.Identity.UI.Services;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

/// <summary>
/// Use case: acknowledge a public contact submission over email. Encapsulates the
/// template + IEmailSender interaction so controllers don't have to know either.
/// Renamed (was inlined in MY_PROFILEController.Contact with a hard-coded template).
/// </summary>
public interface IEmailSenderRelay
{
    Task SendContactAcknowledgementAsync(CONTACTS contact);
}

public class EmailSenderRelay(IEmailSender emailSender) : IEmailSenderRelay
{
    private readonly IEmailSender _emailSender = emailSender;

    public Task SendContactAcknowledgementAsync(CONTACTS contact)
    {
        if (string.IsNullOrEmpty(contact.EMAIL)) return Task.CompletedTask;

        const string subject = "Thank You for Reaching Out!";
        var body = $@"<h5>Hello {contact.NAME},</h5>
<p>Thank you for reaching out! I've received your message and will get back to you as soon as possible.</p>
<p>Warm regards,<br><strong>Zarrif Zim</strong></p>";

        return _emailSender.SendEmailAsync(contact.EMAIL, subject, body);
    }
}
