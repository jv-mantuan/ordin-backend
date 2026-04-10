using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Transactions.GetAllTransactions
{
    public record GetAllTransactionsQuery : IQuery<IReadOnlyList<TransactionWithCategoryNameDto>>
    {

    }
}