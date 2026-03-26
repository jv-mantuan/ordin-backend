
using ErrorOr;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.DeleteTransaction
{
    public record DeleteTransactionCommand(Guid TransactionId) : ICommand<Deleted>;
}
