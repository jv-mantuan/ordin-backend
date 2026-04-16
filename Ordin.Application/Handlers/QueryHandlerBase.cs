using ErrorOr;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Handlers;

public abstract class QueryHandlerBase<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public abstract Task<ErrorOr<TResult>> HandleAsync(TQuery query, CancellationToken ct);
}