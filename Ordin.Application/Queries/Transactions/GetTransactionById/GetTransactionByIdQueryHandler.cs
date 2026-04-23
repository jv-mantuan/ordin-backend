using ErrorOr;
using Ordin.Application.Attributes;
using Ordin.Application.DTOs;
using Ordin.Application.Enums;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Transactions.GetTransactionById
{
    [CacheableQuery(CacheKey = CacheKeys.Transactions, DurationInSeconds = 86400)]
    internal class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, TransactionWithCategoryNameDto>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ErrorOr<TransactionWithCategoryNameDto>> HandleAsync(GetTransactionByIdQuery query, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetById(query.Id, ct);

            if(transaction == null)
            {
                return Error.NotFound("Transaction.NotFound", "No transaction found");
            }

            var dto = new TransactionWithCategoryNameDto
            {
                Name = transaction.Name,
                Amount = transaction.Amount.Value,
                Type = transaction.Type,
                Date = transaction.Date,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category != null ? transaction.Category.Name : string.Empty
            };

            return dto;
        }
    }
}
