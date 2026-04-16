using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Ordin.Application.Attributes;
using Ordin.Application.Interfaces;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Threading;

namespace Ordin.Application.Dispatchers;

public class Dispatcher(
    IServiceProvider provider,
    IUnitOfWork unitOfWork,
    ICurrentUserService userService,
    IMemoryCache cache) : IDispatcher
{
    public async Task<ErrorOr<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));
        dynamic handler = provider.GetRequiredService(handlerType);
        ErrorOr<TResult> result = await handler.HandleAsync((dynamic)command, ct);

        if (result.IsError)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(ct);

        Type handlerRuntimeType = handler.GetType();
        var attribute = handlerRuntimeType.GetCustomAttribute<InvalidateCacheAttribute>();
        if (attribute == null)
        {
            return result;
        }

        foreach (var cacheKeyEnum in attribute.CacheKeys)
        {
            var tokenKey = $"CacheToken_{cacheKeyEnum}_{userService.UserId}";
            if (cache.TryGetValue(tokenKey, out Lazy<CancellationTokenSource>? lazyCts))
            {
                // Accessing Value will safely evaluate the Lazy if it hasn't been yet, or return the singleton instance.
                lazyCts?.Value.Cancel();
                lazyCts?.Value.Dispose();
                cache.Remove(tokenKey);
            }
        }

        return result;
    }

    public async Task<ErrorOr<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct)
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
        dynamic handler = provider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)query, ct);
    }
}