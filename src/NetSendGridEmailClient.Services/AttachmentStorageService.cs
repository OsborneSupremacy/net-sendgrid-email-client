namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IAttachmentStorageService))]
public class AttachmentStorageService : IAttachmentStorageService
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCacheAdapter _memoryCacheAdapter;

    public AttachmentStorageService(ILogger<AttachmentStorageService> logger, IMemoryCacheAdapter memoryCacheAdapter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCacheAdapter = memoryCacheAdapter ?? throw new ArgumentNullException(nameof(memoryCacheAdapter));
    }

    private IAttachmentCollection GetAttachmentCollection(Guid emailPayloadId) =>
        _memoryCacheAdapter
            .GetOrCreate<AttachmentCollection>(emailPayloadId, new()
            {
                SlidingExpiration = TimeSpan.FromHours(2)
            });

    public Task<IAttachmentCollection> GetAttachmentCollectionAsync(Guid emailPayloadId) =>
        Task.FromResult(GetAttachmentCollection(emailPayloadId));

    public Task<IResultIota> SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment)
    {
        var attachmentCollection = GetAttachmentCollection(emailPayloadId);
        attachmentCollection.Add(attachment);
        return Task.FromResult(new OkResultIota() as IResultIota);
    }

    public Task<IResultIota> RemoveAttachmentAsync(Guid emailPayloadId, Guid attachmentId)
    {
        var attachmentCollection = GetAttachmentCollection(emailPayloadId);
        attachmentCollection.Remove(attachmentId);
        return Task.FromResult(new OkResultIota() as IResultIota);
    }

    public Task<IResultIota> RemoveAllAsync(Guid emailPayloadId)
    {
        _memoryCacheAdapter.Remove(emailPayloadId);
        return Task.FromResult(new OkResultIota() as IResultIota);
    }
}
