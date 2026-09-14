using CarRental.Application.Repositories;
using CarRental.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CarRental.Infrastructure.Repositories;

public class CachedCarCategoryRepository : ICarCategoryRepository
{
    private static readonly MemoryCacheEntryOptions CacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
    };

    private readonly ICarCategoryRepository _inner;
    private readonly IMemoryCache _cache;

    public CachedCarCategoryRepository(ICarCategoryRepository inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<CarCategory?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var cacheKey = $"{nameof(CarCategory)}:{name}";

        if (_cache.TryGetValue(cacheKey, out CarCategory? cached))
        {
            return cached;
        }

        var category = await _inner.GetByNameAsync(name, cancellationToken);
        if (category is not null)
        {
            _cache.Set(cacheKey, category, CacheEntryOptions);
        }

        return category;
    }
}
