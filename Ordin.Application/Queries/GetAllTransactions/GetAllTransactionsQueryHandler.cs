using ErrorOr;
using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.GetAllTransactions
{
    public class GetAllTransactionsQueryHandler : IQueryHandler<GetAllTransactionsQuery, IReadOnlyList<TransactionWithCategoryNameDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITransactionRepository _transactionRepository;

        public GetAllTransactionsQueryHandler(ICurrentUserService currentUserService, ITransactionRepository transactionRepository)
        {
            _currentUserService = currentUserService;
            _transactionRepository = transactionRepository;
        }

        public async Task<ErrorOr<IReadOnlyList<TransactionWithCategoryNameDto>>> HandleAsync(GetAllTransactionsQuery query, CancellationToken ct)
        {
            var transactions = await _transactionRepository.GetTransactionsWithCategoriesByUserIdAsync(_currentUserService.UserId, ct);

            var dto = transactions.Select(t => new TransactionWithCategoryNameDto
            {
                Name = t.Name,
                Amount = t.Amount.Value,
                Type = t.Type,
                Date = t.Date,
                CategoryId = t.CategoryId,
                CategoryName = t.Category != null ? t.Category.Name : string.Empty
            }).ToList();

            return dto;
        }
    }
}