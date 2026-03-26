using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.GetTransactionById
{
    public record GetTransactionByIdQuery(Guid Id) : IQuery<TransactionWithCategoryNameDto>;
}
