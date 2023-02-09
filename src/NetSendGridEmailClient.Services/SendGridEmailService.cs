using SendGrid;
using SendGrid.Helpers.Errors.Model;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(ISendGridEmailService))]
public class SendGridEmailService : IEmailService, ISendGridEmailService
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

    public async Task<Outcome<bool>> SendAsync(IEmailPayload emailPayload)
    {
        var msg = await _emailStagingService.StageAsync(emailPayload);

        var domain = _settings
            .Domains
            .Where(x => x.Domain == emailPayload.FromDomain)
            .Single()!;

        try
        {
            var response = await new SendGridClient(domain.ApiKey)
                .SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
                return new Outcome<bool>(new RequestErrorException(await response.Body.ReadAsStringAsync()));

            await _attachmentStorageService
                .RemoveAllAsync(emailPayload.EmailPayloadId);

            return new Outcome<bool>(true);
        } catch (Exception ex)
        {
            return new Outcome<bool>(ex);
        }
    }
}
