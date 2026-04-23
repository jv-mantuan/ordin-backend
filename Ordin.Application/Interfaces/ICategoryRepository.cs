using Ordin.Domain.Entities;

namespace Ordin.Application.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IReadOnlyList<Category>> GetByUserId(Guid userId, CancellationToken cancellationToken = default);
        Task<Category?> GetByIdWithTransactions(Guid id, CancellationToken cancellationToken = default);
    }
}
