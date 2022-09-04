using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Services;

public class AttachmentStorageService : IAttachmentStorageService
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCache _memoryCache;

    public AttachmentStorageService(
        ILogger<AttachmentStorageService> logger,
        IMemoryCache memoryCache
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public Task<IList<IAttachment>> GetAttachmentsAsync(Guid emailPayloadId)
    {
        if (_memoryCache.TryGetValue<IAttachmentCollection>(emailPayloadId, out var attachmentCollection))
            return Task.FromResult(attachmentCollection.GetAll());

        return Task.FromResult(Enumerable.Empty<IAttachment>().ToList() as IList<IAttachment>);
    }

    public Task<IResultIota> SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment)
    {
        if (_memoryCache.TryGetValue<IAttachmentCollection>(emailPayloadId, out var attachmentCollection))
            return Task.FromResult(attachmentCollection.Add(attachment));

        attachmentCollection = new AttachmentCollection();
        attachmentCollection.Add(attachment);

        _memoryCache.Set(emailPayloadId, attachmentCollection,
            new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(2)
            }
        );

        return Task.FromResult(new OkResultIota() as IResultIota);
    }

    public Task<IResultIota> RemoveAttachmentAsync(Guid emailPayloadId, Guid attachmentId)
    {
        if (!_memoryCache.TryGetValue<IAttachmentCollection>(emailPayloadId, out var attachmentCollection))
            return Task.FromResult(new OkResultIota() as IResultIota);

        attachmentCollection.Remove(attachmentId);

        return Task.FromResult(new OkResultIota() as IResultIota);
    }

    public Task<IResultIota> RemoveAllAsync(Guid emailPayloadId)
    {
        if (!_memoryCache.TryGetValue<IAttachmentCollection>(emailPayloadId, out _))
            return Task.FromResult(new OkResultIota() as IResultIota);

        _memoryCache.Remove(emailPayloadId);

        return Task.FromResult(new OkResultIota() as IResultIota);
    }
}
