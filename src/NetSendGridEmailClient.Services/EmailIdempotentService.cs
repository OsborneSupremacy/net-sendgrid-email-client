using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IEmailService))]
public class EmailIdempotentService : IEmailService
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCache _memoryCache;

    private readonly SendGridEmailService _emailService;

    public EmailIdempotentService(
        ILogger<AttachmentStorageService> logger,
        IMemoryCache memoryCache,
        SendGridEmailService emailService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    protected (bool result, DateTime? timeSent) HasEmailBeenSent(Guid emailPayloadId) =>
        !_memoryCache.TryGetValue<DateTime>(emailPayloadId, out var item)
            ? (false, null)
            : (true, item);

    public async Task<IResultIota> SendAsync(IEmailPayload emailPayload)
    {
        var emailSent = HasEmailBeenSent(emailPayload.EmailPayloadId);
        if (emailSent.result)
            return new BadResultIota(StatusCodes.Status429TooManyRequests, 
                $"This email was already sent, at {emailSent.timeSent}. It will not be sent again.");

        var result = await _emailService.SendAsync(emailPayload);

        if (!result.Success)
            return new BadResultIota(StatusCodes.Status400BadRequest, result.Messages);

        _memoryCache.Set(emailPayload.EmailPayloadId, DateTime.Now);

        return new OkResultIota();
    }
}
