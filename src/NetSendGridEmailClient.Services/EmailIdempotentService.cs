namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IEmailService))]
public class EmailIdempotentService : IEmailService
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCacheFacade _memoryCacheFacade;

    private readonly ISendGridEmailService _emailService;

    public EmailIdempotentService(
        ILogger<AttachmentStorageService> logger,
        IMemoryCacheFacade memoryCacheFacade,
        ISendGridEmailService emailService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCacheFacade = memoryCacheFacade ?? throw new ArgumentNullException(nameof(memoryCacheFacade));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public async Task<Outcome<bool>> SendAsync(IEmailPayload emailPayload)
    {
        var cacheOutcome = _memoryCacheFacade
            .GetEntry<DateTime>(emailPayload.EmailPayloadId);

        if (cacheOutcome.IsSuccess)
            return new Outcome<bool>(
                new NotSupportedException($"This email was already sent, at {cacheOutcome.Value}. It will not be sent again."));

        var result = await _emailService.SendAsync(emailPayload);

        if(result.IsFaulted)
            return new Outcome<bool>(result.Exception);

        _memoryCacheFacade.Set(emailPayload.EmailPayloadId, DateTime.Now, new()
        {
            SlidingExpiration = TimeSpan.FromHours(2)
        });

        return true;
    }
}
