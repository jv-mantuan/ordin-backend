using ErrorOr;
using Ordin.Application.DTOs;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Transactions.UpdateTransaction;

public class UpdateTransactionCommandHandler : CommandHandlerBase<UpdateTransactionCommand, TransactionDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
    }
    
    public override async Task<ErrorOr<TransactionDto>> HandleAsync(UpdateTransactionCommand command, CancellationToken ct)
    {
        var transaction = await _transactionRepository.GetByIdAsync(command.Id, ct);

        if (transaction == null || transaction.IsDeleted)
            return Error.NotFound("Transaction.NotFound", "The transaction was not found.");
        
        transaction.Update(command.Name, command.Amount, command.Type, command.Date, command.CategoryId);
        
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