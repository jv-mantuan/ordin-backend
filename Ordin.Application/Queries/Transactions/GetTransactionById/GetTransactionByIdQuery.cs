using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Transactions.GetTransactionById
{
    public record GetTransactionByIdQuery(Guid Id) : IQuery<TransactionWithCategoryNameDto>;
}
