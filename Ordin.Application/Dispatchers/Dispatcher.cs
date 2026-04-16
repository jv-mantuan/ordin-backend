using System.Reflection;
using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Ordin.Application.Attributes;
using Ordin.Application.Interfaces;

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

        foreach (var cacheKey in attribute.CacheKeys)
        {
            cache.Remove($"{cacheKey}_{userService.UserId}");
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