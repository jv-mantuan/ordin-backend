using ErrorOr;

namespace Ordin.Application.Interfaces;

public interface IDispatcher
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken ct) where TCommand : ICommand;
    Task<ErrorOr<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct);
    Task<ErrorOr<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct);
}
