using Master.Application.Abstraction;
using Master.Domain;
using Microsoft.Extensions.Logging;

namespace Master.Infrastructure.EfCore;

public class MasterUnitOfWork : IMasterUnitOfWork
{
    private readonly MasterDbContext _masterDbContext;
    private readonly ILogger<MasterUnitOfWork> _logger;
    
    public MasterUnitOfWork(MasterDbContext masterDbContext, ILogger<MasterUnitOfWork> logger)
    {
        _masterDbContext = masterDbContext;
        _logger = logger;
    }

    public Task SaveChangesAsync()
    {
        return _masterDbContext.SaveChangesAsync();
    }

    public async Task InvokeInATransactionScope(Func<Task> action)
    {
        await using var dbTransaction = await _masterDbContext.Database.BeginTransactionAsync();
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