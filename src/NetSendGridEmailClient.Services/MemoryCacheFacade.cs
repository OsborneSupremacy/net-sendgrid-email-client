using System.Data.SqlTypes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NetSendGridEmailClient.Models;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IMemoryCacheFacade))]
public class MemoryCacheFacade : IMemoryCacheFacade
{
    private readonly ILogger<AttachmentStorageService> _logger;

    private readonly IMemoryCache _memoryCache;

    public MemoryCacheFacade(ILogger<AttachmentStorageService> logger, IMemoryCache memoryCache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public (bool result, T? value) EntryExists<T>(Guid key)
    {
        return !_memoryCache.TryGetValue(key, out T value)
            ? (false, default(T))
            : (true, value);
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
        if (!_memoryCache.TryGetValue(key, out _))
            return;
        _memoryCache.Remove(key);
    }

    public void Set<T>(Guid key, T value, MemoryCacheEntryOptions options) =>
        _memoryCache.Set(key, value, options);
}
