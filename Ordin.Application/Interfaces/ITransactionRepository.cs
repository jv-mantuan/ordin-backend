using Ordin.Domain.Entities;

namespace Ordin.Application.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        new Task AddAsync(Transaction entity, CancellationToken cancellationToken = default);
        new Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Transaction>> GetTransactionsWithCategoriesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
