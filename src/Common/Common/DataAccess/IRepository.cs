using System.Linq.Expressions;
using Common.Entity;

namespace Common.DataAccess;

public interface IRepository<TEntity, TId> where TEntity : Entity<TId> where TId : IEquatable<TId>
{
    Task<List<TEntity>> Get();
    Task<List<TEntity>> Get(params Expression<Func<TEntity, object>>[] includes);
    Task<List<TDto>> Get<TDto>(Expression<Func<TEntity, TDto>> projection);
    Task<List<TEntity>> GetByFilter(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetById(TId id);
    Task<TEntity> GetById(TId id, params Expression<Func<TEntity, object>>[] includes);
    Task<TDto> GetById<TDto>(TId id, Expression<Func<TEntity, TDto>> projection);
    IQueryable<TEntity> QueryAsNoTracking();
    Task Create(TEntity entity);
    Task Update(TEntity entity, TId id);
    Task Delete(TId id);
}