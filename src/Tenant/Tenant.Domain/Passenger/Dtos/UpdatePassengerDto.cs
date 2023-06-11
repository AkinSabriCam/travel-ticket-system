namespace Tenant.Domain.Passenger.Dtos;

public class UpdatePassengerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Identity { get; set; }
}