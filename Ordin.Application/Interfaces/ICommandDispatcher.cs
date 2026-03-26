namespace Ordin.Application.Interfaces
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command, CancellationToken ct)
            where TCommand : ICommand;

        Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken ct)
            where TCommand : ICommand<TResult>;
    }

}