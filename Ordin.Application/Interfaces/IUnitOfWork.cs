using Ordin.Domain.Entities;

namespace Ordin.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}