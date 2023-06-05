namespace Common.Entity;

public class Entity<TId> : IEntity<TId> where TId : IEquatable<TId>
{
    public TId Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class Entity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}
