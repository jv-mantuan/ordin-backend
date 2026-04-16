using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Ordin.Application.Attributes;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;
using System.Reflection;
using System.Text.Json;

namespace Ordin.Application.Decorators
{
    public class CacheableQueryHandlerDecorator<TQuery, TResult> : QueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _queryHandler;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserService _currentUserService;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };

        public CacheableQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler, IMemoryCache cache,
            ICurrentUserService currentUserService)
        {
            _queryHandler = queryHandler;
            _cache = cache;
            _currentUserService = currentUserService;
        }

        public override async Task<ErrorOr<TResult>> HandleAsync(TQuery query, CancellationToken ct)
        {
            var queryType = _queryHandler.GetType();
            var cacheAttribute = queryType.GetCustomAttribute<CacheableQueryAttribute>();

            if (cacheAttribute is null)
                return await _queryHandler.HandleAsync(query, ct);

            var queryJson = JsonSerializer.Serialize(query, _jsonOptions);
            var cacheKey = $"{queryType.Name}:{cacheAttribute.CacheKey}_{_currentUserService.UserId}:{queryJson}";

            if (_cache.TryGetValue(cacheKey, out TResult? cachedResult))
            {
                if (cachedResult != null)
                    return cachedResult;
            }

            var result = await _queryHandler.HandleAsync(query, ct);

            if(!result.IsError)
            {
                var tokenKey = $"CacheToken_{cacheAttribute.CacheKey}_{_currentUserService.UserId}";
                var lazyCts = _cache.GetOrCreate(tokenKey, entry => 
                {
                    entry.Priority = CacheItemPriority.NeverRemove;
                    return new Lazy<CancellationTokenSource>(() => new CancellationTokenSource());
                });

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheAttribute.DurationInSeconds))
                    .AddExpirationToken(new CancellationChangeToken(lazyCts!.Value.Token));

                _cache.Set(cacheKey, result.Value, cacheOptions);
            }

            return result;
        }
    }
}