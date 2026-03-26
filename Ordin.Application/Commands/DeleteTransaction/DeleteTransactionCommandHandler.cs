using ErrorOr;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : ICommandHandler<DeleteTransactionCommand, Deleted>
    {
        private readonly ITransactionRepository _transactionRepository;

        public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ErrorOr<Deleted>> HandleAsync(DeleteTransactionCommand command, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetByIdAsync(command.TransactionId, ct);

            if (transaction == null)
            {
                return Error.NotFound("Transaction.NotFound", "The transaction was not found.");
            }

            transaction.Delete();

            return Result.Deleted;
        }
    }
}
    