using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IEmailStagingService))]
public class EmailStagingService : IEmailStagingService
{
    private readonly SendGridSettings _settings;

    private readonly IAttachmentStorageService _attachmentStorageService;

    private readonly IAttachmentAdapter _attachmentAdapter;

    public EmailStagingService(
        SendGridSettings settings,
        IAttachmentStorageService attachmentStorageService,
        IAttachmentAdapter attachmentAdapter
        )
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _attachmentStorageService = attachmentStorageService ?? throw new ArgumentNullException(nameof(attachmentStorageService));
        _attachmentAdapter = attachmentAdapter ?? throw new ArgumentNullException(nameof(attachmentAdapter));
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

        await _attachmentAdapter.AddAsync(msg,
            await _attachmentStorageService
                .GetAttachmentsAsync(emailPayload.EmailPayloadId)
        );

        return msg;
    }
}
