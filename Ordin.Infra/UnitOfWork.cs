using Ordin.Application.Interfaces;
using Ordin.Domain.Entities;
using Ordin.Infra.Contexts;
using Ordin.Infra.Repositories;

namespace Ordin.Infra
{
    public class UnitOfWork(OrdinContext context) : IUnitOfWork
    {
        private readonly OrdinContext _context = context;
        private readonly Dictionary<Type, object> _repositories = [];

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                var repository = new BaseRepository<TEntity>(_context);
                _repositories.Add(typeof(TEntity), repository);

                return repository;
            }

            return (IBaseRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context?.Dispose();
    }
}