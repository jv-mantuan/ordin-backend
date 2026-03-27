using ErrorOr;
using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;
using Ordin.Domain.Entities;

namespace Ordin.Application.Commands.Transactions.CreateTransaction
{
    public class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, TransactionDto>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, ICurrentUserService currentUserService)
        {
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<TransactionDto>> HandleAsync(CreateTransactionCommand command, CancellationToken ct)
        {
            var transaction = new Transaction(command.Name, command.Amount, command.Type, command.Date, command.CategoryId, _currentUserService.UserId);

            await _transactionRepository.AddAsync(transaction, ct);

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
