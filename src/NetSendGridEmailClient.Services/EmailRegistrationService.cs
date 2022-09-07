using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IEmailRegistrationService))]
public class EmailRegistrationService : IEmailRegistrationService
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCache _memoryCache;

    public EmailRegistrationService(
        ILogger<AttachmentStorageService> logger,
        IMemoryCache memoryCache
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public (bool result, DateTime? timeSent) HasEmailBeenSent(Guid emailPayloadId) =>
        !_memoryCache.TryGetValue<DateTime>(emailPayloadId, out var item)
            ? (false, null)
            : (true, item);

    public void RegisterSentEmail(Guid emailPayloadId) =>
        _memoryCache.Set(emailPayloadId, DateTime.Now);
}
