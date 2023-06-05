using System.Linq.Expressions;
using Common.DataAccess;
using Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace Master.Infrastructure.EfCore.Repositories;

public class BaseRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId> where TId : IEquatable<TId>
{
    private readonly DbSet<TEntity> _dbTable;

    protected BaseRepository(DbContext dbContext)
    {
        _dbTable = dbContext.Set<TEntity>();
    }

    public Task<List<TEntity>> Get()
    {
        return _dbTable.AsNoTracking().ToListAsync();
    }

    public Task<List<TEntity>> Get(params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbTable.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        return query.ToListAsync();
    }

    public Task<List<TDto>> Get<TDto>(Expression<Func<TEntity, TDto>> projection)
    {
        return _dbTable.AsNoTracking().Select(projection).ToListAsync();
    }

    public Task<List<TEntity>> GetByFilter(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbTable.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> GetById(TId id)
    {
        var entity = await _dbTable.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (entity == null)
            throw new Exception($"Entity could not find with this id : {id}");

        return entity;
    }

    public async Task<TEntity> GetById(TId id, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbTable.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (entity == null)
            throw new Exception($"Entity could not find with this id : {id}");

        return entity;
    }

    public async Task<TDto> GetById<TDto>(TId id, Expression<Func<TEntity, TDto>> projection)
    {
        var entity = await _dbTable
            .Where(x => x.Id.Equals(id))
            .Select(projection)
            .FirstOrDefaultAsync();

        if (entity == null)
            throw new Exception($"Entity could not find with this id : {id}");

        return entity;
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
        var entity = await _dbTable.FirstOrDefaultAsync(x => x.Id.Equals(id));
        
        if (entity == null)
            throw new Exception($"Entity could not find with this id : {id}");

        entity.IsDeleted = true;
        _dbTable.Entry(entity).State = EntityState.Modified;
    }
}