using ErrorOr;
using Ordin.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ordin.Application.Dispatchers;

public class Dispatcher(IServiceProvider provider, IUnitOfWork unitOfWork) : IDispatcher
{
    private readonly IServiceProvider _provider = provider;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken ct) where TCommand : ICommand
    {
        var handler = _provider.GetRequiredService<ICommandHandler<TCommand>>();

        await handler.HandleAsync(command, ct);

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<ErrorOr<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));
        dynamic handler = _provider.GetRequiredService(handlerType);
        ErrorOr<TResult> result = await handler.HandleAsync((dynamic)command, ct);

        if (!result.IsError)
        {
            await unitOfWork.SaveChangesAsync(ct);
        }

        return result;
    }

    public async Task<ErrorOr<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct)
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
        dynamic handler = _provider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)query, ct);
    }
}
