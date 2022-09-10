using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Interface;

public interface IMemoryCacheFacade
{
    public TItem GetOrCreate<TItem>(Guid key, MemoryCacheEntryOptions options) where TItem : new();

    public void Set<T>(Guid key, T value, MemoryCacheEntryOptions options);

    public (bool result, T? value) EntryExists<T>(Guid key);

    public void Remove(Guid key);
}
