using Microsoft.Extensions.Logging;
using Tenant.Domain;

namespace Tenant.Infrastructure.EfCore;

public class TenantUnitOfWork : ITenantUnitOfWork
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly ILogger<TenantUnitOfWork> _logger;
    
    public TenantUnitOfWork(TenantDbContext tenantDbContext, ILogger<TenantUnitOfWork> logger)
    {
        _tenantDbContext = tenantDbContext;
        _logger = logger;
    }

    public Task SaveChangesAsync()
    {
        return _tenantDbContext.SaveChangesAsync();
    }

    public async Task InvokeInATransactionScope(Func<Task> action)
    {
        await using var dbTransaction = await _tenantDbContext.Database.BeginTransactionAsync();
        try
        {
            await action.Invoke();
            await dbTransaction.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error In Db Transaction : {ex.Message} \n Stack Trace :{ex.StackTrace}");
            await dbTransaction.RollbackAsync();
        }
    }
}