namespace Tenant.Domain.Passenger.Dtos;

public class CreatePassengerDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Identity { get; set; }
    public Guid ExpeditionId { get; set; }
    public string SeatNumber { get; set; }
}