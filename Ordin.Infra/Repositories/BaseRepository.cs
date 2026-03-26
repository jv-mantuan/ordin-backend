using Ordin.Application.Interfaces;
using Ordin.Domain.Entities;
using Ordin.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ordin.Infra.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;

    public BaseRepository(OrdinContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<T> GetAll(CancellationToken cancellationToken = default)
    {
        return _dbSet;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public IQueryable<T> Query(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
}