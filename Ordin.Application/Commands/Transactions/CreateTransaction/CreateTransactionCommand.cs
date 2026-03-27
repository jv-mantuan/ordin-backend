using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;
using Ordin.Domain.Enums;

namespace Ordin.Application.Commands.Transactions.CreateTransaction
{
    public record CreateTransactionCommand(string Name, decimal Amount, TransactionType Type, DateTimeOffset Date, Guid CategoryId) : ICommand<TransactionDto>;
}
