
using ErrorOr;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Transactions.DeleteTransaction
{
    public record DeleteTransactionCommand(Guid TransactionId) : ICommand<Deleted>;
}
