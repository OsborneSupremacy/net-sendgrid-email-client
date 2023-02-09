using System.Diagnostics.CodeAnalysis;

namespace NetSendGridEmailClient.Services;

[ExcludeFromCodeCoverage]
[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IAttachmentStorageService))]
public class AttachmentStorageService : IAttachmentStorageService
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCacheFacade _memoryCacheFacade;

    public AttachmentStorageService(ILogger<AttachmentStorageService> logger, IMemoryCacheFacade memoryCacheFacade)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCacheFacade = memoryCacheFacade ?? throw new ArgumentNullException(nameof(memoryCacheFacade));
    }

    private IAttachmentCollection GetAttachmentCollection(Guid emailPayloadId) =>
        _memoryCacheFacade
            .GetOrCreate<AttachmentCollection>(emailPayloadId, new()
            {
                SlidingExpiration = TimeSpan.FromHours(2)
            });

    public Task<IAttachmentCollection> GetAttachmentCollectionAsync(Guid emailPayloadId) =>
        Task.FromResult(GetAttachmentCollection(emailPayloadId));

    public Task<Outcome<bool>> SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment)
    {
        var attachmentCollection = GetAttachmentCollection(emailPayloadId);
        attachmentCollection.Add(attachment);
        return Task.FromResult(new Outcome<bool>(true));
    }

    public Task<Outcome<bool>> RemoveAttachmentAsync(Guid emailPayloadId, Guid attachmentId)
    {
        var attachmentCollection = GetAttachmentCollection(emailPayloadId);
        attachmentCollection.Remove(attachmentId);
        return Task.FromResult(new Outcome<bool>(true));
    }

    public Task<Outcome<bool>> RemoveAllAsync(Guid emailPayloadId)
    {
        _memoryCacheFacade.Remove(emailPayloadId);
        return Task.FromResult(new Outcome<bool>(true));
    }
}
