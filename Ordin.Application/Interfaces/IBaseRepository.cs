using Ordin.Domain.Entities;
using System.Linq.Expressions;

namespace Ordin.Application.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    IQueryable<T> GetAll(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    IQueryable<T> Query(Expression<Func<T, bool>> expression);
}