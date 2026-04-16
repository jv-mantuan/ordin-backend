using ErrorOr;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Handlers;

public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    public abstract Task<ErrorOr<TResult>> HandleAsync(TCommand command, CancellationToken ct);
}