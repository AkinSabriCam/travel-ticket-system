namespace Common.Entity;

public interface IEntity<TId> where TId : IEquatable<TId>
{
    TId Id { get; set; }   
    bool IsDeleted { get; set; }
}