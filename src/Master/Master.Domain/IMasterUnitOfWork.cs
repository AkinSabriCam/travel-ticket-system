namespace Master.Domain;

public interface IMasterUnitOfWork
{
    Task SaveChangesAsync();
    Task InvokeInATransactionScope(Func<Task> action);
}