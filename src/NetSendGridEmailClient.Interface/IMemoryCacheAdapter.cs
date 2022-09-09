using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Interface;

public interface IMemoryCacheAdapter
{
    public TItem GetOrCreate<TItem>(Guid key, MemoryCacheEntryOptions options) where TItem : new();

    public void Remove(Guid key);
}
