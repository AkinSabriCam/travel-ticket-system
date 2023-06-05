using Common.Entity;

namespace Master.Domain.Tenant;

public class Tenant : Entity<Guid>
{
    public string Code { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}