using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.GetAllTransactions
{
    public record GetAllTransactionsQuery : IQuery<IReadOnlyList<TransactionWithCategoryNameDto>>
    {

    }
}