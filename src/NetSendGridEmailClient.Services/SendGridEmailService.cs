using NetSendGridEmailClient.Functions;
using NetSendGridEmailClient.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Services;

public class SendGridEmailService
{
    private readonly string _apiKey = string.Empty;

    private readonly SendGridSettings _settings;

    public SendGridEmailService(
        SendGridSettings settings
        )
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<(bool success, string details)> SendAsync(EmailPayload emailPayload)
    {
        SendGridMessage msg = new()
        {
            From = emailPayload.FromAddress.ToEmail(),
            Subject = emailPayload.Subject,
            HtmlContent = emailPayload.HtmlBody
        };

        msg.AddTos(emailPayload.To.ToEmailList());
        msg.AddCcs(emailPayload.Cc.ToEmailList());
        msg.AddBccs(emailPayload.Bcc.ToEmailList());

        var response = await new SendGridClient(_apiKey)
            .SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
            return (true, string.Empty);

        return (false, await response.Body.ReadAsStringAsync());
    }
}
