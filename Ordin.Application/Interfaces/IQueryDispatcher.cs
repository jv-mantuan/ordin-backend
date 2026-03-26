namespace Ordin.Application.Interfaces
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken ct) where TQuery : IQuery<TResult>;
    }
}