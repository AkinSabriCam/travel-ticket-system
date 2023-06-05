namespace Tenant.Domain;

public interface ITenantUnitOfWork
{
    Task SaveChangesAsync();
    Task InvokeInATransactionScope(Func<Task> action);
}