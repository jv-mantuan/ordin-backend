using System.Reflection;
using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using Ordin.Application.Attributes;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Decorators
{
    public class CacheableQueryHandlerDecorator<TQuery, TResult> : QueryHandlerBase<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _queryHandler;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserService _currentUserService;


        public CacheableQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler, IMemoryCache cache,
            ICurrentUserService currentUserService)
        {
            _queryHandler = queryHandler;
            _cache = cache;
            _currentUserService = currentUserService;
        }

        public override async Task<ErrorOr<TResult>> HandleAsync(TQuery query, CancellationToken ct)
        {
            var cacheAttribute = _queryHandler.GetType().GetCustomAttribute<CacheableQueryAttribute>();

            if (cacheAttribute is null)
                return await _queryHandler.HandleAsync(query, ct);

            var cacheKey = $"{cacheAttribute.CacheKey}_{_currentUserService.UserId}";
            if (_cache.TryGetValue(cacheKey, out TResult? cachedResult))
            {
                if (cachedResult != null)
                    return cachedResult;
            }

            var result = await _queryHandler.HandleAsync(query, ct);

            if(!result.IsError)
                _cache.Set(cacheKey, result.Value, TimeSpan.FromSeconds(cacheAttribute.DurationInSeconds));

            return result;
        }
    }
}