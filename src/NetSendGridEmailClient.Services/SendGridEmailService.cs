using NetSendGridEmailClient.Functions;
using NetSendGridEmailClient.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Services;

public class SendGridEmailService
{
    private readonly SendGridSettings _settings;

    public SendGridEmailService(
        SendGridSettings settings
        )
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<(bool success, string details)> SendAsync(EmailPayload emailPayload)
    {
        var domain = _settings
            .Domains
            .Where(x => x.Domain == emailPayload.FromDomain)
            .Single()!;

        SendGridMessage msg = new()
        {
            From = emailPayload.FromAddress.ToEmail(),
            Subject = emailPayload.Subject ?? string.Empty,
            HtmlContent = emailPayload.HtmlBody
        };

        msg.AddTos(emailPayload.To.ToEmailList());
        if(emailPayload.Cc.AnyEmails())
            msg.AddCcs(emailPayload.Cc.ToEmailList());
        if(emailPayload.Bcc.AnyEmails())
            msg.AddBccs(emailPayload.Bcc.ToEmailList());

        var response = await new SendGridClient(domain.ApiKey)
            .SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
            return (true, string.Empty);

        return (false, await response.Body.ReadAsStringAsync());
    }
}
