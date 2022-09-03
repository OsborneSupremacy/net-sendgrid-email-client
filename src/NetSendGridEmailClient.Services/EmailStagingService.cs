using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Services;

public class EmailStagingService : IEmailStagingService
{
    private readonly SendGridSettings _settings;

    private readonly IAttachmentStorageService _attachmentStorageService;

    public EmailStagingService(
        SendGridSettings settings,
        IAttachmentStorageService attachmentStorageService
        )
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _attachmentStorageService = attachmentStorageService ?? throw new ArgumentNullException(nameof(attachmentStorageService));
    }

    public async Task<SendGridMessage> StageAsync(IEmailPayload emailPayload)
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

        if (emailPayload.Cc.AnyEmails())
            msg.AddCcs(emailPayload.Cc.ToEmailList());

        if (emailPayload.Bcc.AnyEmails())
            msg.AddBccs(emailPayload.Bcc.ToEmailList());

        var attachments = await _attachmentStorageService
            .GetAttachmentsAsync(emailPayload.EmailPayloadId);

        foreach (var attachment in attachments)
            await msg.AddAttachmentAsync(
                attachment.FileName,
                new MemoryStream(Convert.FromBase64String(attachment.Base64Content)),
                attachment.Type,
                "attachment",
                null,
                CancellationToken.None
                );

        return msg;
    }
}
