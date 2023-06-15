namespace Master.Domain;

public interface IMasterUnitOfWork
{
    Task SaveChangesAsync();
    Task InvokeInADatabaseTransaction(Func<Task> action);
}