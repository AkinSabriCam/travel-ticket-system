using Common.Entity;

namespace Tenant.Domain.Tenant;

public class Tenant : Entity<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}