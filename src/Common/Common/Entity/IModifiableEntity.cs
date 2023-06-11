namespace Common.Entity;

public interface IModifiableEntity
{
    Guid TenantId { get; set; }
    Guid? CreatedBy { get; set; }
    DateTime CreatedDate { get; set; }
    Guid? UpdatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }

}