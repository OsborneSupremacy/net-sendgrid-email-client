using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetSendGridEmailClient.Interface;
using NetSendGridEmailClient.Models;

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
        if (_memoryCache.TryGetValue<IList<IAttachment>>(emailPayloadId, out var emailAttachments))
            return Task.FromResult(emailAttachments);

        return Task.FromResult(Enumerable.Empty<IAttachment>().ToList() as IList<IAttachment>);
    }

    public Task SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment)
    {
        if(_memoryCache.TryGetValue<IAttachmentCollection>(emailPayloadId, out var attachmentCollection))
        {
            attachmentCollection.Add(attachment);
            return Task.CompletedTask;
        };

        var cacheEntry = _memoryCache.CreateEntry(emailPayloadId);

        attachmentCollection = new AttachmentCollection();
        attachmentCollection.Add(attachment);

        cacheEntry.Value = attachmentCollection;
        return Task.CompletedTask;
    }
}
