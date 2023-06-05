using System.Linq.Expressions;
using Common.DataAccess;
using Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace Tenant.Infrastructure.EfCore.Repository;

public class BaseRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId> where TId : IEquatable<TId>
{
    private readonly DbSet<TEntity> _dbTable;
    protected readonly TenantDbContext DbContext;

    public BaseRepository(TenantDbContext dbContext)
    {
        DbContext = dbContext;
        _dbTable = dbContext.Set<TEntity>();
    }

    public Task<List<TEntity>> Get()
    {
        return _dbTable.ToListAsync();
    }

    public Task<List<TEntity>> Get(params Expression<Func<TEntity, object>>[] includes)
    {
        var entities = _dbTable.AsQueryable();
        
        foreach (var includeItem in includes)
            entities = entities.Include(includeItem);

        return entities.ToListAsync();
    }

    public Task<List<TDto>> Get<TDto>(Expression<Func<TEntity, TDto>> projection)
    {
        return QueryAsNoTracking().Select(projection).ToListAsync();
    }

    public Task<List<TEntity>> GetByFilter(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbTable.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> GetById(TId id)
    {
        var entity = await _dbTable.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (entity == null)
            throw new ApplicationException("Entity could not found");

        return entity;
    }

    public Task<TEntity> GetById(TId id, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbTable.AsNoTracking();
        
        foreach (var include in includes)
            query = query.Include(include);

        return query.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<TDto> GetById<TDto>(TId id, Expression<Func<TEntity, TDto>> projection)
    {
        return QueryAsNoTracking()
            .Where(x => x.Id.Equals(id))
            .Select(projection)
            .FirstOrDefaultAsync();
    }

    public IQueryable<TEntity> QueryAsNoTracking() => _dbTable.AsNoTracking();

    public Task Create(TEntity entity)
    {
        _dbTable.Add(entity);
        return Task.CompletedTask;
    }

    public Task Update(TEntity entity, TId id)
    {
        _dbTable.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task Delete(TId id)
    {
        var entity = await _dbTable.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        
        if (entity == null)
            throw new ApplicationException("Entity could not found");

        entity.IsDeleted = true;
        _dbTable.Entry(entity).State = EntityState.Modified;
    }
}