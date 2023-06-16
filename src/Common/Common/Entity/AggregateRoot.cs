namespace Common.Entity;

public class AggregateRoot<TId> : Entity<TId>, IModifiableEntity where TId : IEquatable<TId>
{
    public Guid TenantId { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public List<Field> Changes { get; } = new();
}

public class AggregateRoot : AggregateRoot<Guid>
{
    protected AggregateRoot()
    {
        Id = Guid.NewGuid();
    }
}