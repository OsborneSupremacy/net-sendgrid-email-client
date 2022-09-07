using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IEmailService))]
public class SendGridEmailService : IEmailService
{
    private readonly SendGridSettings _settings;

    private readonly IAttachmentStorageService _attachmentStorageService;

    private readonly IEmailStagingService _emailStagingService;

    public SendGridEmailService(
        SendGridSettings settings,
        IAttachmentStorageService attachmentStorageService,
        IEmailStagingService emailStagingService)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _attachmentStorageService = attachmentStorageService ?? throw new ArgumentNullException(nameof(attachmentStorageService));
        _emailStagingService = emailStagingService ?? throw new ArgumentNullException(nameof(emailStagingService));
    }

    public async Task<IResultIota> SendAsync(IEmailPayload emailPayload)
    {
        SendGridMessage msg = await _emailStagingService.StageAsync(emailPayload);

        var domain = _settings
            .Domains
            .Where(x => x.Domain == emailPayload.FromDomain)
            .Single()!;

        var response = await new SendGridClient(domain.ApiKey)
            .SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
            return new BadResultIota((int)response.StatusCode, await response.Body.ReadAsStringAsync());

        await _attachmentStorageService
            .RemoveAllAsync(emailPayload.EmailPayloadId);

        return new OkResultIota();
    }
}
