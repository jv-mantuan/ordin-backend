using Ordin.Application.Interfaces;
using Ordin.Domain.Entities;
using Ordin.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ordin.Infra.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly DbSet<Transaction> _dbSet;

        public TransactionRepository(OrdinContext context) : base(context)
        {
            _dbSet = context.Set<Transaction>(); 
        }

        /// <summary>
        /// Asynchronously retrieves all transactions for the specified user, including their associated categories.
        /// </summary>
        /// <remarks>The returned list includes each transaction's related category data. If the user has
        /// no transactions, the list will be empty.</remarks>
        /// <param name="userId">The unique identifier of the user whose transactions are to be retrieved.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A read-only list of transactions that belong to the specified user, each including its associated category.</returns>
        public async Task<IReadOnlyList<Transaction>> GetTransactionsWithCategoriesByUserId(Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(t => t.Category).Where(t => t.UserId == userId && !t.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves all transactions for the specified user, including their associated categories.
        /// </summary>
        /// <remarks>The returned list includes each transaction's related category data. If the user has
        /// no transactions, the list will be empty.</remarks>
        /// <param name="userId">The unique identifier of the user whose transactions are to be retrieved.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A read-only list of transactions that belong to the specified user, each including its associated category.</returns>
        public async Task<IReadOnlyList<Transaction>> GetTransactionsWithCategoriesAsNoTracking(Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(t => t.Category).Where(t => t.UserId == userId && !t.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves a transaction by its unique identifier, including its associated category
        /// information.
        /// </summary>
        /// <param name="id">The unique identifier of the transaction to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The transaction associated with the specified identifier.</returns>
        public new async Task<Transaction?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var transaction = await _dbSet.Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, cancellationToken);

            return transaction;
        }
    }
}