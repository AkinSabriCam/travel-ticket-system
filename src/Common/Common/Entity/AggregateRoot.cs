namespace Common.Entity;

public class AggregateRoot<TId> : Entity<TId>, IModifiableEntity where TId : IEquatable<TId>
{
    public Guid TenantId { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class AggregateRoot : AggregateRoot<Guid>
{
    public AggregateRoot()
    {
        Id = Guid.NewGuid();
    }
}