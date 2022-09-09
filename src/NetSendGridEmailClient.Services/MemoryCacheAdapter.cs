using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IMemoryCacheAdapter))]
public class MemoryCacheAdapter : IMemoryCacheAdapter
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCache _memoryCache;

    public MemoryCacheAdapter(ILogger<AttachmentStorageService> logger, IMemoryCache memoryCache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public TItem GetOrCreate<TItem>(Guid key, MemoryCacheEntryOptions options) where TItem : new()
    {
        var entryExists = _memoryCache.TryGetValue<TItem>(key, out var entry);
        if (entryExists) return entry;

        var newEntry = new TItem();
        _memoryCache.Set(key, newEntry, options);
        return newEntry;
    }

    public void Remove(Guid key)
    {
        if(!_memoryCache.TryGetValue(key, out _))
            return;
        _memoryCache.Remove(key);
    }
}
