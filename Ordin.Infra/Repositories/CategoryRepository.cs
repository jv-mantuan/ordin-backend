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
        /// Returns all non-deleted categories belonging to the given user.
        /// </summary>
        public async Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .ToListAsync(cancellationToken);
        }
    }
}
