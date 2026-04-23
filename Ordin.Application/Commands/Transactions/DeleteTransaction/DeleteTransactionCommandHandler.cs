using ErrorOr;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Transactions.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : CommandHandler<DeleteTransactionCommand, Deleted>
    {
        private readonly ITransactionRepository _transactionRepository;

        public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public override async Task<ErrorOr<Deleted>> HandleAsync(DeleteTransactionCommand command, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetById(command.TransactionId, ct);

            if (transaction == null || transaction.IsDeleted)
            {
                return Error.NotFound("Transaction.NotFound", "The transaction was not found.");
            }

            transaction.Delete();

            return Result.Deleted;
        }
    }
}
    