using ErrorOr;
using Ordin.Application.Attributes;
using Ordin.Application.DTOs;
using Ordin.Application.Enums;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;
using Ordin.Domain.Entities;

namespace Ordin.Application.Commands.Transactions.CreateTransaction
{
    [InvalidateCache(CacheKeys.Transactions)]
    public class CreateTransactionCommandHandler : CommandHandler<CreateTransactionCommand, TransactionDto>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, ICurrentUserService currentUserService)
        {
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
        }

        public override async Task<ErrorOr<TransactionDto>> HandleAsync(CreateTransactionCommand command, CancellationToken ct)
        {
            var transaction = new Transaction(command.Name, command.Amount, command.Type, command.Date, command.CategoryId, _currentUserService.UserId);

            await _transactionRepository.Add(transaction, ct);

            return new TransactionDto
            {
                Name = transaction.Name,
                Amount = (decimal)transaction.Amount,
                Type = transaction.Type,
                Date = transaction.Date,
                CategoryId = transaction.CategoryId
            };
        }
    }
}
