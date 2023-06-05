namespace Master.Domain.Tenant.Dto;

public class CreateTenantDto
{
    public string Code { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
