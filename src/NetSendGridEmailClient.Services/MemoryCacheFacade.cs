using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IMemoryCacheFacade))]
public class MemoryCacheFacade : IMemoryCacheFacade
{
    private readonly ILogger<MemoryCacheFacade> _logger;

    private readonly IMemoryCache _memoryCache;

    public MemoryCacheFacade(ILogger<MemoryCacheFacade> logger, IMemoryCache memoryCache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public IOutcome<T> GetEntry<T>(Guid key)
    {
        if (_memoryCache.TryGetValue(key, out T? value))
            return new Outcome<T>(value!);
        return new Outcome<T>(new KeyNotFoundException($"{key} not found in memory cache."));
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
