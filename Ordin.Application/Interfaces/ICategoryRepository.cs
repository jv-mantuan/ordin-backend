using Ordin.Domain.Entities;

namespace Ordin.Application.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Category?> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
