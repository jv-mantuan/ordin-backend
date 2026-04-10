using ErrorOr;

namespace Ordin.Application.Interfaces
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<Error> HandleAsync(TCommand command, CancellationToken ct);
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<ErrorOr<TResult>> HandleAsync(TCommand command, CancellationToken ct);
    }
}