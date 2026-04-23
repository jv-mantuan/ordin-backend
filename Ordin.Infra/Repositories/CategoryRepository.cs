using Ordin.Application.Interfaces;
using Ordin.Domain.Entities;
using Ordin.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ordin.Infra.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(OrdinContext context) : base(context)
        {
            _dbSet = context.Set<Category>();
        }

        /// <summary>
        /// Retrieves a category by its unique identifier, including its associated transactions.
        /// </summary>
        public async Task<Category?> GetByIdWithTransactions(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        /// <summary>
        /// Returns all non-deleted categories belonging to the given user.
        /// </summary>
        public async Task<IReadOnlyList<Category>> GetByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves all categories for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose categories are to be retrieved.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        public async Task<IReadOnlyList<Category>> GetByUserIdAsNoTracking(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
