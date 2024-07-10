using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachableRequest
{
    private readonly CacheSettings _cacheSettings;
    private readonly IDistributedCache _cache;

    public CachingBehavior(IDistributedCache cache, IConfiguration configuration)
    {
        _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? throw new InvalidOperationException();
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
        {
            return await next();
        }

        TResponse response;
        var cachedResponse = await _cache.GetStringAsync(request.CacheKey, cancellationToken);

        if(cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(cachedResponse);
        }
        else 
        {
            response = await GetResponseAndAddToCache(request, next, cancellationToken);
        }

        return response;
    }

    private async Task<TResponse?> GetResponseAndAddToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = await next();

        TimeSpan slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);

        DistributedCacheEntryOptions cacheOptions = new()
        {
            SlidingExpiration = slidingExpiration,
        };

        var serializeData = JsonSerializer.Serialize(response);

        await _cache.SetStringAsync(request.CacheKey, serializeData, cacheOptions, cancellationToken);

        return response;
    }
}
