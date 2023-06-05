using Master.Domain.Tenant;

namespace Master.Application.Queries.GetAllTenants;

public class TenantDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public TenantDto(Tenant tenant)
    {
        Id = tenant.Id;
        Country = tenant.Country;
        City = tenant.City;
        Code = tenant.Code;
        Name = tenant.Name;
        Email = tenant.Email;
    }
}