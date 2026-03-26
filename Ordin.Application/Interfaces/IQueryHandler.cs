using ErrorOr;

namespace Ordin.Application.Interfaces
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<ErrorOr<TResult>> HandleAsync(TQuery query, CancellationToken ct);
    }
}