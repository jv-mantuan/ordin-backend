using Ordin.Domain.Entities;

namespace Ordin.Application.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        new Task Add(Transaction entity, CancellationToken cancellationToken = default);
        new Task<Transaction?> GetById(Guid id, CancellationToken cancellationToken = default);
        public Task<IReadOnlyList<Transaction>> GetTransactionsWithCategoriesByUserId(Guid userId, CancellationToken cancellationToken = default);
        public Task<IReadOnlyList<Transaction>> GetTransactionsWithCategoriesAsNoTracking(Guid userId, CancellationToken cancellationToken = default);
    }
}
