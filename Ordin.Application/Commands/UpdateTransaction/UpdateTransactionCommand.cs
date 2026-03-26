using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;
using Ordin.Domain.Enums;

namespace Ordin.Application.Commands.UpdateTransaction;

public record UpdateTransactionCommand(Guid Id, string Name, decimal Amount, TransactionType Type, DateTimeOffset Date, Guid CategoryId) : ICommand<TransactionDto>;